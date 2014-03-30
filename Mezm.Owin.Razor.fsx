#r "packages/FAKE.2.10.24.0/tools/FakeLib.dll"
open Fake

let buildDir = "./build/"
let testDir = "./tests/"
let packageDir = "./nuget-package/"
let appReferences = !! "**/*.csproj" -- "**/*.Tests.csproj"
let testModuleReferences = !! "**/*.Tests.csproj"
let nuspecFile = "./Mezm.Owin.Razor.nuspec"
let nuspecReferences = ["Mezm.Owin.Razor.dll"; "Mezm.Owin.Razor.pdb"]
let version = "0.1.2"

Target "Clean" (fun _ -> 
    CleanDirs [buildDir; testDir; packageDir]
)

Target "Compile" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences |> Log "Compile: "
)

Target "CompileTests" (fun _ ->
    MSBuildRelease testDir "Build" testModuleReferences |> Log "Compile: "
)

Target "Tests" (fun _ -> 
    !!(testDir + "*.Tests.dll") |> NUnit (fun x ->
        { x with DisableShadowCopy = true; OutputFile = testDir + "TestResults.xml"; ToolPath = "packages/NUnit.Runners.2.6.3/tools/" }
    )
)

Target "MakeNugetPackage" (fun _ ->
    let libDir = packageDir + "lib/net40"
    CreateDir libDir
    nuspecReferences |> List.map (fun x -> buildDir + x) |> CopyFiles libDir
    nuspecFile |> NuGet (fun x -> 
        { x with ToolPath = ".nuget/nuget.exe"; WorkingDir = packageDir; OutputPath = "."; Version = version }
    ) 
)

"Clean"
    ==> "Compile"
    ==> "CompileTests"
    ==> "Tests"
    ==> "MakeNugetPackage"

RunTargetOrDefault "Tests"