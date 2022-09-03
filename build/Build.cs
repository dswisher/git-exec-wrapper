using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    // TODO - enable Fetch during CI/CD builds
    [GitVersion(NoFetch = true)] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TestResultsDirectory => RootDirectory / "test-results";


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
            EnsureCleanDirectory(TestResultsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            Project project = Solution.GetProject("GitExecWrapper.UnitTests");

            DotNetTest(s => s
                .EnableNoBuild()
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(TestResultsDirectory)
                .SetProjectFile(project));
        });

    Target CodeCoverage => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            ReportGenerator(s => s
                .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll", framework: "net6.0"))
                .SetTargetDirectory(TestResultsDirectory / "reports")
                .AddReports(TestResultsDirectory / "**/coverage.cobertura.xml"));

            string link = TestResultsDirectory / "reports" / "index.html";
            Log.Information($"Code coverage report: \x1b]8;;file://{link.Replace('\\', '/')}\x1b\\{link}\x1b]8;;\x1b\\");
        });

    Target Pack => _ => _
        .DependsOn(CodeCoverage)
        .Executes(() =>
        {
            Project project = Solution.GetProject("GitExecWrapper");
            
            Log.Information("Package version: {Version}", GitVersion.NuGetVersionV2);

            DotNetPack(s => s
                .SetProject(project)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration("Release")
                .EnableNoLogo()
                .EnableNoRestore()
                .EnableContinuousIntegrationBuild()
                .SetVersion(GitVersion.NuGetVersionV2));
        });
}
