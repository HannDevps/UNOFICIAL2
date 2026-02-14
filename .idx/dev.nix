# To learn more about how to use Nix to configure your environment
# see: https://developers.google.com/idx/guides/customize-idx-env
{ pkgs, ... }: {
  # Which nixpkgs channel to use.
  # Use unstable to ensure .NET 9 is available.
  channel = "unstable";

  # Use https://search.nixos.org/packages to find packages
  packages = [
    pkgs.gh
    pkgs.dotnet-sdk_9

    # Python
    pkgs.python311
    pkgs.python311Packages.pip

    # JavaScript / Node.js
    pkgs.nodejs_20

    # Java
    pkgs.jdk17
    pkgs.maven
  ];

  # Sets environment variables in the workspace
  env = {};

  idx = {
    # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
    extensions = [
      "google.gemini-cli-vscode-ide-companion"

      # Core .NET / C# experience
      "muhammad-sammy.csharp"
      "ms-dotnettools.vscode-dotnet-runtime"
      "fernandoescolar.vscode-solution-explorer"
      "tintoy.msbuild-project-tools"
      "patcx.vscode-nuget-gallery"
      "josefpihrt-vscode.roslynator"
      "csharpier.csharpier-vscode"

      # Python
      "ms-python.python"

      # Java
      "redhat.java"
      "vscjava.vscode-java-debug"
      "vscjava.vscode-java-test"
      "vscjava.vscode-maven"

      # JavaScript / TypeScript
      "dbaeumer.vscode-eslint"
      "esbenp.prettier-vscode"
      "xabikos.JavaScriptSnippets"

      # Useful general tooling
      "humao.rest-client"
      "ms-azuretools.vscode-docker"
      "EditorConfig.EditorConfig"
    ];

    previews = {
      enable = true;
      previews = { };
    };

    workspace = {
      onCreate = {
        default.openFiles = [ ".idx/dev.nix" "README.md" ];
      };

      onStart = {
      };
    };
  };
}
