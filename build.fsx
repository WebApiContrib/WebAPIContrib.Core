#r "packages/FAKE.Dotnet/tools/Fake.Dotnet.dll"
#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Dotnet 

Target "Clean" (fun _ ->
    !! "artifacts" ++ "src/*/bin" ++ "test/*/bin"
        |> DeleteDirs
)

Target "PrepareDotnetCli" (fun _ ->
    let sdkVersion = GlobalJsonSdk "global.json"
    tracefn "Using global.json sdk version: %s" sdkVersion

    // set custom install directory
    let customInstallDirectory = environVar "LocalAppData" @@ "Microsoft" @@ "dotnetbld" @@ sdkVersion

    let setOptions (options: DotNetCliInstallOptions) = 
        { options with 
            Version = Version sdkVersion
            InstallerBranch = "rel/1.0.0-preview2"
            Channel = Channel "preview"
            CustomInstallDir = Some customInstallDirectory
            AlwaysDownload = false
        }    

    DefaultDotnetCliDir <- customInstallDirectory
    DotnetCliInstall setOptions
)

Target "RestorePackage" (fun _ ->
    DotnetRestore id (currentDirectory @@ "src")
)

Target "BuildProjects" (fun _ ->
      !! "src/*/project.json" 
      |> Seq.iter(fun proj ->  
          DotnetRestore id proj
          DotnetPack (fun c -> 
              { c with 
                  Configuration = Debug;                    
                  OutputPath = Some (currentDirectory @@ "artifacts")
              }) proj
      )
)

"Clean"
      ==> "PrepareDotnetCli"
      ==> "RestorePackage"
      ==> "BuildProjects"

Run "BuildProjects"