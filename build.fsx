#r "nuget: Fun.Build, 1.1.2"
#r "nuget: Fake.IO.FileSystem, 6.0.0"

open System.IO
open Fake.IO
open Fake.IO.FileSystemOperators
open Fun.Build

let solutionFile = "Fullstack.sln"
let deployDir = Path.getFullName "dist"

let restoreStage =
    stage "Restore" {
        run "dotnet tool restore"
        run "dotnet restore --locked-mode"
    }

let clean (input: string seq) =
    async {
        input
        |> Seq.iter (fun dir ->
            if Directory.Exists(dir) then
                Directory.Delete(dir, true))
    }

pipeline "Build" {
    workingDir __SOURCE_DIRECTORY__
    restoreStage
    stage "Clean" { run (clean [| "dist"; "reports" |]) }

    stage "Main" {
        paralle

        stage "Client" {
            workingDir "src/Client"
            run "pnpm install --frozen-lockfile"
            run "dotnet fable -o .build --run vite preview -c vite.config.js"
        }
    }

    runIfOnlySpecified true
}

pipeline "Preview" {
    workingDir __SOURCE_DIRECTORY__
    restoreStage
    stage "Clean" { run (clean [| "dist"; "reports" |]) }

    stage "Install" { run "pnpm install --frozen-lockfile" }


    stage "Build" {
        workingDir "src/Client"
        run "dotnet fable -s -o .build --run vite build -c ../../vite.config.js"
    }

    stage "Preview" {
        workingDir "src/Client"
        run "dotnet fable -s -o .build --run vite preview -c ../../vite.config.js"
    }


    runIfOnlySpecified true
}

pipeline "Watch" {
    workingDir __SOURCE_DIRECTORY__
    stage "Clean" { run (clean [| "dist"; "reports" |]) }
    stage "Install" { run "pnpm install --frozen-lockfile" }

    stage "Main" {
        run "dotnet tool restore"
        paralle

        stage "Client" {
            workingDir "src/Client"
            run "dotnet fable watch -s -o .build --verbose --run vite -c ../../vite.config.js"
        }
    }

    runIfOnlySpecified true
}

pipeline "Client" {
    workingDir __SOURCE_DIRECTORY__
    stage "Clean" { run (clean [| "dist"; "reports" |]) }
    restoreStage

    stage "Client" {
        workingDir "src/Client"
        run "bun i --frozen-lockfile"
        run "bunx --bun vite"
    }

    runIfOnlySpecified true
}

pipeline "Analyze" {
    workingDir __SOURCE_DIRECTORY__
    stage "Report" { run $"dotnet msbuild /t:AnalyzeSolution %s{solutionFile}" }
    runIfOnlySpecified true
}

pipeline "Format" {
    workingDir __SOURCE_DIRECTORY__
    stage "Restore" { run "dotnet tool restore" }
    stage "Fantomas" { run "dotnet fantomas src build.fsx" }
    runIfOnlySpecified true
}

pipeline "Sign" {
    workingDir __SOURCE_DIRECTORY__
    stage "Restore" { run "dotnet tool restore" }

    stage "Main" { run "dotnet telplin src/Client/Client.fsproj -- /p:Configuration=Release" }

    runIfOnlySpecified true
}

tryPrintPipelineCommandHelp ()
