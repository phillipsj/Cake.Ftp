#tool "nuget:?package=xunit.runner.console&version=2.1.0"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Cake.Ftp/bin") + Directory(configuration);
var artifactsDir = Directory("./src/artifacts");
var assemblyInfo = ParseAssemblyInfo("./src/Cake.Ftp/Properties/AssemblyInfo.cs");
var buildNumber = AppVeyor.Environment.Build.Number;
Information(assemblyInfo.AssemblyVersion);
var version = AppVeyor.IsRunningOnAppVeyor ? assemblyInfo.AssemblyVersion : assemblyInfo.AssemblyVersion + "." + buildNumber; 
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>{
    CleanDirectory(buildDir);
    CleanDirectory(artifactsDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() => {
        NuGetRestore("./src/Cake.Ftp.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
        // Use MSBuild
        MSBuild("./src/Cake.Ftp.sln", settings => settings.SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() => {
        XUnit2("./src/**/bin/" + configuration +"/*.Tests.dll", new XUnit2Settings {
            OutputDirectory = artifactsDir,
            XmlReportV1 = true,
            NoAppDomain = true
        });
});

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>{
        var nuGetPackSettings = new NuGetPackSettings {
            Id = "Cake.Ftp",
            Version = version,
            Title = "Cake.Ftp",
            Authors = new []{"Jamie Phillips"},
            Owners = new []{"Jamie Phillips"},
            Description = "Cake Addin for working with FTP from a Cake script.",
            ProjectUrl = new Uri("https://github.com/phillipsj/Cake.Ftp"),
            IconUrl = new Uri("https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png"),
            LicenseUrl = new Uri("https://github.com/phillipsj/Cake.Ftp/blob/master/LICENSE.md"),
            Copyright = string.Format("Jamie Phillips {0}", DateTime.Now.Year),
            Tags = new []{"Cake", "FTP"},
            RequireLicenseAcceptance = false,
            Symbols =  false,
            NoPackageAnalysis = true,
            Files = new []{
                new NuSpecContent {Source = "Cake.Ftp.dll", Target = "bin"}
            },
            BasePath = "./src/Cake.Ftp/bin/" + configuration,
            OutputDirectory = artifactsDir
        };
        NuGetPack(nuGetPackSettings);
});

Task("Publish")
   .IsDependentOn("Package")
   .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
   .Does(() => {
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve Nuget API key.");
    }
    // Push the package.
    var package = "./src/artifacts/Cake.Ftp." + version + ".nupkg";
    NuGetPush(package, new NuGetPushSettings { 
        Source = "https://api.nuget.org/v3/index.json",
        ApiKey = apiKey
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

