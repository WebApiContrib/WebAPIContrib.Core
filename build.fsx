#r "paket: groupref FakeBuild //"
#load "./.fake/build.fsx/intellisense.fsx"
#if !FAKE
#r "Facades/netstandard"
#r "netstandard"
#endif

open System.IO
open BlackFox.Fake
open Fake.BuildServer
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators

let cleanTask =
    BuildTask.create "Clean" [] {
        !! "artifacts" ++ "src/*/bin" ++ "test/*/bin"
        |> Shell.deleteDirs
    }

let buildTask =
    BuildTask.create "Build" [cleanTask] {
        !! "**/*.csproj"
        |> Seq.iter
          (DotNet.build
            (fun p ->
              { p with
                  Configuration = DotNet.BuildConfiguration.Release }))
    }

let testTask =
    BuildTask.create "Test" [] {
        let isAppVeyorBuild = AppVeyor.detect()
        !! "tests/**/*.csproj"
        |> Seq.iter
          (DotNet.test
            (fun p ->
              { p with
                  Configuration = DotNet.BuildConfiguration.Release
                  TestAdapterPath = if isAppVeyorBuild then Some "." else None
                  Logger = if isAppVeyorBuild then Some "AppVeyor" else None }))
    }

let packTask =
    BuildTask.create "Pack" [buildTask; testTask] {
        let artifactsDir = Path.Combine(__SOURCE_DIRECTORY__, "artifacts")
        !! "src/**/*.csproj"
        |> Seq.iter
          (DotNet.pack
            (fun p -> 
              { p with 
                  NoBuild = true 
                  Configuration = DotNet.BuildConfiguration.Release
                  OutputPath = Some artifactsDir }))
    }

BuildTask.create "Help" [] {
    Trace.logfn "Usage: fake [--target <target>] [<options>]"
    BuildTask.listAvailable()
}

let defaultTask =
    BuildTask.createEmpty "All" [packTask]

BuildTask.runOrDefault defaultTask
