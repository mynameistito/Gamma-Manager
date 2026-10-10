#define MyAppName "Gamma Manager"
#define MyAppVersion "1.3.0.0"
#define MyAppPublisher "mynameistito"
#define MyAppExeName "Gamma Manager.exe"

[Setup]
AppId={{C625D24D-2184-496D-A439-CF2F405044BA}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\Gamma Manager
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=admin
OutputDir=..\artifacts
OutputBaseFilename=GammaManagerSetup
SetupIconFile=..\Gamma Manager\TrayIcon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription=Display gamma and monitor controls
VersionInfoProductName={#MyAppName}

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked
Name: "startwithwindows"; Description: "Start Gamma Manager with Windows"; GroupDescription: "Startup options:"; Flags: unchecked

[Files]
Source: "build\Release\Gamma Manager.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "build\Release\Gamma Manager.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "build\Release\Gamma Manager.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Gamma Manager"; ValueData: """{app}\{#MyAppExeName}"" --minimized"; Flags: uninsdeletevalue; Tasks: startwithwindows

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch Gamma Manager"; Flags: postinstall nowait skipifsilent
