#{
#  description = "Fullstack F#";
#
#  inputs = {
#    flake-schemas.url = "https://flakehub.com/f/DeterminateSystems/flake-schemas/*.tar.gz";
#
#    nixpkgs.url = "https://flakehub.com/f/NixOS/nixpkgs/0.1.*.tar.gz";
#  };
#
#  outputs =
#    {
#      self,
#      flake-schemas,
#      nixpkgs,
#    }:
#    let
#      supportedSystems = [
#        "x86_64-linux"
#        "aarch64-darwin"
#      ];
#      forEachSupportedSystem =
#        f: nixpkgs.lib.genAttrs supportedSystems (system: f { pkgs = import nixpkgs { inherit system; }; });
#    in
#    {
#      schemas = flake-schemas.schemas;
#
#      devShells = forEachSupportedSystem (
#        { pkgs }:
#        {
#          default = pkgs.mkShell {
#            packages = with pkgs; [
#              nixpkgs-fmt
#              fsautocomplete
#              dotnet-sdk_8
#              nodejs_20
#              pnpm
#            ];
#          };
#        }
#      );
#    };
#}
# https://nixos.org/manual/nixpkgs/unstable/#dotnet
{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils = {
      url = "github:numtide/flake-utils";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };
  outputs =
    { nixpkgs, flake-utils, ... }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = import nixpkgs { inherit system; };
      in
      with pkgs;
      {
        devShells.default = mkShell {
          nativeBuildInputs = [
            dotnet-sdk_8
            nodejs_20
            pnpm
          ];

          shellHook = ''
            set -a; source .env; set +a
          '';
        };
      }
    );
}
