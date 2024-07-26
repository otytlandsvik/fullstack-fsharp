# Fable F# template using FUI

This is a simple web application template using FS.FluentUI.

## Requirements

* [Dotnet SDK](https://www.microsoft.com/net/download/core) 8.0 or higher
* [Node.js](https://nodejs.org) 18 or higher
* pnpm
* An F# editor like [JetBrains Rider](https://www.jetbrains.com/rider/), Visual Studio and Visual Studio Code with [Ionide](http://ionide.io/).

Or you can use the [VS Code Remote Container](https://code.visualstudio.com/docs/remote/containers?WT.mc_id=dotnet-33392-aapowell) for development, as it will set up all the required dependencies.

## Features

### Client

* [Fable](https://fable.io/) for F# to JavaScript compilation.
* Styling with [Fluent UI](https://github.com/sydsutton/FS.FluentUI).
* Shared client-server types.

## Build

After cloning the repository, you should restore the local dotnet tools and the solution:

```console
dotnet tool restore
dotnet restore
```

Afterward, you can run the default build script. This will build the project.

```console
dotnet fsi build.fsx
```

Alternately, you can also run some other pipelines using `-p`. Examples:

- `dotnet fsi build.fsx -- -p Preview` will bundle the client with vite and serve it on
  `localhost:8083`

- `dotnet fsi build.fsx -- -p Watch` will start the client in development
mode, watching for changes.

- `dotnet fsi build.fsx -- -p Format` will format all changed files using the fantomas
formatter.j

- `dotnet fsi build.fsx -- -p Sign` will generate signature files (`.fsi`) for the
project.

- `dotnet fsi build.fsx -- -p Analyze` will run the `G-Research` and `Ionide` to diagnose
source code and surface custom errors, warnings and code fixes.


## Nix

This template also includes a [Nix](https://nixos.org/) flake with [direnv](https://direnv.net/) for development.
To use it, simply run `direnv allow` in the root of the project. This will install all the dependencies and set up the environment for you.
