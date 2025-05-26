using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Core.IO;
using System.Text;
using System;
using System.Net.NetworkInformation;

//custom args
//--InstallerOnly
//--Trimmed
//--InstallerOutputLocation ""
//--BuildOnly
//--NugetConfigFile ""
//--SelfContained
//--offline //offline restore

public static class Program
{
    public static int Main(string[] args)
    {
        //Console.WriteLine("Test");
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseWorkingDirectory("..")//Project root
                                      //.InstallTool(new Uri("nuget:?package=NuGet.CommandLine&version=5.10.0"))
            .Run(args);
    }
}

public static class Helper
{
    public static string GetFullPathPathFixDirectorySeparatorChar(this Cake.Core.IO.Path path)
    {
        return PathFixDirectorySeparatorChar(path.FullPath);
    }

    private static string PathCombineExInte(string firstStr, params string[] strs)
    {
        string addedStr = firstStr ?? "";
        var strBuilder = new StringBuilder(addedStr);
        foreach (var str in strs)
        {
            if (str.StartsWith(System.IO.Path.DirectorySeparatorChar))
            {
                if (addedStr.EndsWith(System.IO.Path.DirectorySeparatorChar))
                {
                    addedStr = PathFixDirectorySeparatorChar(str).Remove(0, 1);
                }
                else
                {
                    addedStr = PathFixDirectorySeparatorChar(str);
                }
            }
            else
            {
                if (addedStr.EndsWith(System.IO.Path.DirectorySeparatorChar))
                {
                    addedStr = PathFixDirectorySeparatorChar(str);
                }
                else
                {
                    addedStr = System.IO.Path.DirectorySeparatorChar + PathFixDirectorySeparatorChar(str);
                }
            }
            strBuilder.Append(addedStr);
        }
        return strBuilder.ToString();
    }
    public static string PathCombineEx(this Cake.Core.IO.Path path, params string[] strs)
    {
        return PathCombineExInte(GetFullPathPathFixDirectorySeparatorChar(path), strs);
    }

    public static string PathCombineEx(params string[] strs)
    {
        return PathCombineExInte("", strs);
    }
    public static string PathFixDirectorySeparatorChar(string str) => str.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace('\\', System.IO.Path.DirectorySeparatorChar);
    public static string GetFullPathFromWorkingDirectory(this BuildContext context, string pathToCombine)
    {
        //var dicInfo = new DirectoryInfo(context.Environment.WorkingDirectory.FullPath);

        var res = PathCombineEx(context.Environment.WorkingDirectory, pathToCombine);//.Combine(pathToCombine);
        //var res = System.IO.Path.Combine(dicInfo.FullName, pathToCombine);//.Combine(pathToCombine);
        //var res = System.IO.Path.Combine(context.Environment.WorkingDirectory.FullPath, pathToCombine);//.Combine(pathToCombine);
        //var res = context.Environment.WorkingDirectory.CombineWithFilePath(pathToCombine);

        //return res.FullPath;
        return res;
    }
    public static bool ThereIsInternet()
    {
        try
        {
            string host = "1.1.1.1";
            Ping ping = new Ping();

            var replay = ping.Send(host, 1000, new byte[32], new PingOptions());
            return replay.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }

    }
    public static void NuGetRestoreProject(this BuildContext context, string root)
    {
        if (context.Offline || !ThereIsInternet())
        {
            context.Log.Information($"Restore in Offline Mode. source: {BuildContext.OfflineNugetPackageCache}");
            context.DotNetRestore(root, new DotNetRestoreSettings()
            {
                Sources = new[] { BuildContext.OfflineNugetPackageCache },
                //DiagnosticOutput = true
            });
        }
        else
        {
            context.Log.Information($"Restore in Online Mode");
            context.DotNetRestore(root, new DotNetRestoreSettings()
            {
                ConfigFile = context.NugetConfigFile// ?? @"C:\Nuget\NuGet.custom.config"
            });
        }
    }
    public static void NuGetRestoreProject(this BuildContext context)
    {
        if (context.Offline)
        {
            context.DotNetRestore(new DotNetRestoreSettings()
            {
                Sources = new[] { BuildContext.OfflineNugetPackageCache }
            });
        }
        else
        {

            context.DotNetRestore(new DotNetRestoreSettings()
            {
                ConfigFile = context.NugetConfigFile// ?? @"C:\Nuget\NuGet.custom.config"
            });
        }
    }

    public static void BuildTheInstaller(this BuildContext context)
    {
        //INNO Setup
        var installerConfFile = context.GetFullPathFromWorkingDirectory(@"\ElForsan_Installer.iss");
        BuildTheInstaller(context, installerConfFile);
    }
    public static void BuildTheInstaller(this BuildContext context, string installerRelativeFile)
    {
        var installerConfFile = context.GetFullPathFromWorkingDirectory(installerRelativeFile);
        context.Log.Information($"Building Installer : {installerConfFile}");
       
        //build
        var buildInstallerCommand = $"\"{installerConfFile}\"";
        context.Log.Information($"buildInstallerCommand is : {buildInstallerCommand}");
        using (var process = context.StartAndReturnProcess("ISCC.exe",
            new ProcessSettings { Arguments = buildInstallerCommand }))
        {
            process.WaitForExit();
            // This should output 0 as valid arguments supplied
            context.Log.Information("Exit code: {0}", process.GetExitCode());
        }
    }


    public static void FullProjectPublish(this BuildContext context, string projectRelativeFile_csproj, string? installerRelativeFile = null)
    {
        var projectFile = context.GetFullPathFromWorkingDirectory(projectRelativeFile_csproj);
        var projectDir = System.IO.Path.GetDirectoryName(projectFile);
        var projectPublishDir = PathCombineExInte(projectDir!, @"\bin\Publish\");

        if (!string.IsNullOrEmpty(installerRelativeFile) && context.InstallerOnly)
        {
            //BuildTheInstaller
            context.BuildTheInstaller(installerRelativeFile);
            return;
        }

        //RestoreBeforeClean //Restore Before Clean is A workaround of getting errors and Restore Before Cleaning solves it
        context.NuGetRestoreProject(projectFile);

        //Clean

        context.Log.Information("Cleaning");
        context.DotNetClean(projectFile, new Cake.Common.Tools.DotNet.Clean.DotNetCleanSettings()
        {
            Configuration = "Release",
            //Runtime = "win-x86",
            //Framework = "net9.0-windows",
        });

        context.Log.Information($"Cleaning Dir Publish : {projectPublishDir}");
        context.CleanDirectory(projectPublishDir);

        //Restore
        context.NuGetRestoreProject(projectFile);


        //Publish
        context.DotNetPublish(projectFile, new DotNetPublishSettings()
        {
            NoRestore = true,//set to false--> Workaround weird error !!
            Configuration = "Release",
            OutputDirectory = projectPublishDir,//@$"{context.Environment.WorkingDirectory}\El Forsan\bin\Publish\",
            Runtime = "win-x86",
            Framework = "net9.0-windows",
            SelfContained = context.SelfContained,
            PublishReadyToRun = false,
            PublishSingleFile = false,
            PublishTrimmed = context.Trimmed
        });

        if (!string.IsNullOrEmpty(installerRelativeFile) && !context.BuildOnly)
        {
            //BuildTheInstaller
            //context.BuildTheInstaller(@"\ElForsan_ProductsPricesList_Installer.iss");
            context.BuildTheInstaller(installerRelativeFile);
        }
    }
}

public class BuildContext : FrostingContext
{

    public const string OfflineNugetPackageCache = "C:\\NugetPackageCache\\";

    //public bool Delay { get; set; }

    public bool InstallerOnly { get; }
    public string InstallerOutputLocation { get; }
    public bool SelfContained { get; set; }
    public bool Trimmed { get; }

    /// <summary>
    /// Just Build no Publish. won't run installer build too.
    /// </summary>
    public bool BuildOnly { get; }
    public string NugetConfigFile { get; }
    public bool Offline { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        Trimmed = context.Arguments.HasArgument("Trimmed");
        InstallerOnly = context.Arguments.HasArgument("InstallerOnly");
        SelfContained = context.Arguments.HasArgument("SelfContained");
        InstallerOutputLocation = context.Arguments.GetArgument("InstallerOutputLocation");
        if (!string.IsNullOrEmpty(InstallerOutputLocation)) InstallerOutputLocation = Helper.PathFixDirectorySeparatorChar(InstallerOutputLocation);
        BuildOnly = context.Arguments.HasArgument("BuildOnly");
        NugetConfigFile = context.Arguments.GetArgument("NugetConfigFile");
        if (!string.IsNullOrEmpty(NugetConfigFile)) NugetConfigFile = Helper.PathFixDirectorySeparatorChar(NugetConfigFile);
        this.Log.Information($"ApplicationRoot is : {this.Environment.ApplicationRoot.GetFullPathPathFixDirectorySeparatorChar()}");
        this.Log.Information($"WorkingDirectory is : {this.Environment.WorkingDirectory.GetFullPathPathFixDirectorySeparatorChar()}");

        this.Log.Information($"Trimmed=={Trimmed}");
        this.Log.Information($"InstallerOnly=={InstallerOnly}");
        this.Log.Information($"InstallerOutputLocation=={InstallerOutputLocation}");
        this.Log.Information($"BuildOnly=={BuildOnly}");
        this.Log.Information($"NugetConfigFile=={NugetConfigFile}");
        this.Log.Information($"SelfContained=={SelfContained}");
        Offline = context.Arguments.HasArgument("offline");
        this.Log.Information($"Offline=={Offline}");
    }
}

[TaskName("ProjectBuild")]
//[IsDependentOn(typeof(CleanTask))]
public sealed class ProjectBuildTask : FrostingTask<BuildContext> //AsyncFrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        return true;
        //return context.ShouldRunBuild();
        //return base.ShouldRun(context);
    }

    // Tasks can be asynchronous
    public override void Run(BuildContext context)
    {
        context.FullProjectPublish(@"\FolderIconChangerWPF\FolderIconChangerWPF.csproj", @"\FolderIconChanger_Installer.iss");
    }
}


[TaskName("Default")]
[IsDependentOn(typeof(ProjectBuildTask))]
public class DefaultTask : FrostingTask
{

}