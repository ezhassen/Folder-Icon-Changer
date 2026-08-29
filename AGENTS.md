# AGENTS.md

## Build / Run — Use Cake Frosting, Not Raw `dotnet build`

This is a **WPF .NET 10.0 Windows-only** desktop app. Full publish + installer is via Cake Frosting. Raw `dotnet build` works for quick verification but does not produce installers.

```powershell
# Prereqs: .NET SDK 10.0.301+ (see global.json rollForward: latestFeature), Inno Setup 6 (ISCC.exe on PATH) for installers
dotnet --version  # must be 10.0.x

# Full publish both runtimes + build both installers (output: FolderIconChangerWPF/bin/Publish/win-x64|win-x86 + "Folder Icon Changer Setup/")
./build.ps1
./build.sh  # same on git bash / WSL (calls dotnet run --project ./build/Build.csproj)

# Focused variants (all under build/Program.cs:261 BuildContext):
./build.ps1 --BuildOnly              # clean + publish, skip Inno Setup
./build.ps1 --InstallerOnly          # only ISCC.exe, requires prior publish (ISS reads version from bin/Publish/win-x64/Folder Icon Changer.exe via GetVersionNumbersString)
./build.ps1 --offline                # restore from C:\NugetPackageCache\ (pings 1.1.1.1 to detect online; see Helper.ThereIsInternet)
./buildOffline.ps1                   # alias for --offline
./build.ps1 --SelfContained --Trimmed

# Quick verification without Cake (no installer):
dotnet build "Folder Icon Changer.slnx" -c Release
dotnet build FolderIconChangerWPF/FolderIconChangerWPF.csproj -c Release -p:Platform=x64
```

Clean stale artifacts (bin/obj are gitignored): `.\DeleteObjBinFolders.ps1 -Bin` (without `-Bin` only deletes `obj`).

## Solution / Project Layout

- **Solution:** `Folder Icon Changer.slnx` — new XML-format solution (VS 2022 17.12+ / .NET 10 SDK). Not a `.sln`. Open with `dotnet sln` or VS. Maps `Solution *|x64 -> Project x64`.
- **Main app:** `FolderIconChangerWPF/FolderIconChangerWPF.csproj` — `net10.0-windows`, `UseWPF=true`, `AllowUnsafeBlocks=true`, `OutputType=WinExe`, `StartupObject=FolderIconChangerWPF.App` (`App.xaml` -> `MainWindow.xaml`). `RuntimeIdentifiers=win-x86;win-x64`, `Platforms=AnyCPU;x86;x64`. Version is single source in `<Version>4.2.3.0</Version>` (propagates to `AssemblyVersion`/`FileVersion`).
- **Build orchestration:** `build/build.csproj` — `net10.0`, `ImplicitUsings`, `Cake.Frosting 6.2.0`. `Program.cs` sets `UseWorkingDirectory("..")` so it runs from repo root regardless of `dotnet run --project build/Build.csproj`. Publish settings: `Framework=net10.0-windows`, `SelfContained=false` by default, `PublishSingleFile=false`, `PublishReadyToRun=false`.
- **Installers:** `FolderIconChanger_Installer_x64.iss` / `x86.iss` — `SourceDir=FolderIconChangerWPF\bin\Publish\win-x64` (or x86), `OutputDir=..\..\..\..\Folder Icon Changer Setup`, version extracted at compile via `#define MyAppVersion GetVersionNumbersString('...Folder Icon Changer.exe')`.
- **Key dirs under `FolderIconChangerWPF/`:** `Pages/`+`ViewModels/` (MVVM, `ApplicationViewModel`, `MainPageViewModel`), `Services/` (`SettingsService`), `Helpers/`+`Ezz_Helper/`, `Controls/`, `Styles/`/`Themes/` (MahApps.Metro), `MultilingualResources/` (WPFLocalizeExtensionEzzFork), `Fonts/`/`Images/`/`Resources/`.

## Quirks & Gotchas

- **Windows-only:** WPF + `net10.0-windows` + Inno Setup. Cannot build/publish meaningfully on Linux/macOS — verify on Windows.
- **`global.json` pins SDK `10.0.301`** with `rollForward: latestFeature`. If `dotnet --version` is 9.x, install 10.0.4xx or set `allowPrerelease=false` will fail.
- **Quote spaced paths:** `Folder Icon Changer` has spaces — always quote solution/project paths.
- **Publish before installer:** ISS `#define MyAppVersion` fails if `Folder Icon Changer.exe` not yet published. Run full `./build.ps1` or publish first then `--InstallerOnly`.
- **NuGet offline:** `--offline` forces `Sources=[C:\NugetPackageCache\]`. Online restore uses `NugetConfigFile` arg if provided (`--NugetConfigFile "path"`). `Helper.ThereIsInternet()` pings `1.1.1.1` — firewalled env falls back to offline.
- **No tests/lint/typecheck/CI:** No `*.Tests.csproj`, no `*.yml` workflows, no `.opencode/` or `opencode.json`. Verification is build + manual run of `Folder Icon Changer.exe`. Don't add `dotnet test`/`npm` steps.
- **Dependencies:** `MahApps.Metro 2.4.11`, `WPFLocalizeExtensionEzzFork 3.10.1`, `Newtonsoft.Json 13.0.4`, `PortableJsonSettingsProvider 0.2.2`, `Microsoft.Windows.Compatibility 10.0.11`. Update via `PackageReference` in csproj.
- **Git remotes:** `origin` → `github.com/ezhassen/Folder-Icon-Changer.git`, `azure` → `visualstudio.com` backup. Branches `main`/`develop`/`WPFDev`/`oldWinForms`/`optimizaions`; `develop` is default working branch. Don't assume `main` has latest — check `git log`.

## No Existing Agent Instructions

No `CLAUDE.md`, `.cursor/rules/`, or `.github/copilot-instructions.md` found. Previous `README.md` is minimal. This file is the sole source.
