#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.DotNetCli

Target "Clean" (fun _ ->
    !! "artifacts" ++ "src/*/bin" ++ "test/*/bin"
        |> DeleteDirs
)

Target "Build" (fun _ ->
    DotNetCli.Restore id

    !! "**/*.csproj"
    |> DotNetCli.Build id
)

Target "Test" (fun _ ->
    !! "tests/**/*.csproj"
    |> DotNetCli.Test id
)

Target "Pack" (fun _ ->
    !! "src/**/*.csproj"
    |> DotNetCli.Pack
      (fun p -> 
         { p with 
            Configuration = "Release"
            OutputPath = "artifacts" })
)

"Clean"
      ==> "Build"
      ==> "Pack"

RunTargetOrDefault "Pack"