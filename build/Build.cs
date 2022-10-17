using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{

    public static int Main() => Execute<Build>(x => x.All);

    //[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    //readonly Configuration Configuration = Configuration.Debug;
    readonly Configuration Configuration = Configuration.Release;

    [Solution] readonly Solution Solution;
    //[GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    Project MainProject => Solution.GetProject("Eir.MFSH");

    AbsolutePath SourceDirectory => RootDirectory / "Projects";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);

            if (MainProject == null)
                throw new System.Exception($"MainProject not found");
        });

    Target Restore => _ => _
         .Unlisted()
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });


    Target CompileMain => _ => _
         .Unlisted()
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetPublish(s => s
                .SetConfiguration(Configuration)
                //.SetAssemblyVersion(GitVersion.AssemblySemVer)
                //.SetFileVersion(GitVersion.AssemblySemFileVer)
                //.SetInformationalVersion(GitVersion.AssemblySemFileVer)
                //.SetDescription(GitVersion.InformationalVersion)
                .SetSelfContained(false)
                .SetProject(MainProject)
                .SetOutput(OutputDirectory / MainProject.Name)
            );
        });


    Target Compile => _ => _
        .DependsOn(Clean)
        .DependsOn(CompileMain)
        ;

    Target Push => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .SetSource("github")
                .CombineWith(OutputDirectory.GlobFiles("*.nupkg"), (s, p) => s.SetTargetPath(p))
            );
        });

    Target Pack => _ => _
        .DependsOn(Clean)
        .DependsOn(Compile)
        .Before(Push)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(MainProject)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.AssemblySemFileVer)
                .SetDescription(GitVersion.InformationalVersion)
                .SetProperty("PackageVersion", GitVersion.NuGetVersionV2)
                .SetOutputDirectory(OutputDirectory)
            );
        });

    Target All => _ => _
        .DependsOn(Clean)
        .DependsOn(Compile)
        .DependsOn(Pack)
        ;
}
