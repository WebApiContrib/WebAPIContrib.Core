#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.DotNetCli

Target "Clean" (fun _ ->
    !! "artifacts" ++ "src/*/bin" ++ "test/*/bin"
        |> DeleteDirs
)

Target "Build" (fun _ ->
    DotNetCli.Restore id

    !! "src/**/project.json"
    |> DotNetCli.Build id
)

Target "Test" (fun _ ->
    !! "tests/**/project.json"
    |> DotNetCli.Test id
)

Target "Pack" (fun _ ->
    !! "src/**/project.json"
    |> DotNetCli.Pack
      (fun p -> 
         { p with 
            Configuration = "Release"
            OutputPath = "artifacts" })
)

"Clean"
      ==> "Build"
      ==> "Pack"

Run "Pack"