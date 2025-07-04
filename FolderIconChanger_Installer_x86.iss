
#define MyAppName "Folder Icon Changer"
#define MyAppVersion GetVersionNumbersString('FolderIconChangerWPF\bin\Publish\win-x86\Folder Icon Changer.exe')
#define MyAppPublisher "ezhassen"
#define MyAppExeName "Folder Icon Changer.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{4D6EBAB1-4959-48F2-A382-BB09167AD104}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
AppCopyright=Copyright (C) 2016-2025 ezhassen.
AppVerName={#MyAppName} {#MyAppVersion} x86
AppPublisher={#MyAppPublisher}
DefaultDirName={commonpf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=..\..\..\..\Folder Icon Changer Setup
OutputBaseFilename={#MyAppName} Setup {#MyAppVersion} x86
Compression=lzma2/ultra64
SolidCompression=yes
SourceDir=FolderIconChangerWPF\bin\Publish\win-x86
SetupIconFile=..\..\..\..\Installer_Icon.ico
ShowLanguageDialog=auto
InternalCompressLevel=ultra64
;MinVersion=10.0.10240
;PrivilegesRequired=lowest
;PrivilegesRequiredOverridesAllowed=dialog
WizardStyle=modern
ArchitecturesAllowed=x86
ArchitecturesInstallIn64BitMode=x86

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
; Source: "*.*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs ; for all files
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: {#MyAppExeName}; DestDir: "{app}"; Flags: ignoreversion
Source: "*.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "Folder Icon Changer.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "ar\Folder Icon Changer.resources.dll"; DestDir: "{app}\ar"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
