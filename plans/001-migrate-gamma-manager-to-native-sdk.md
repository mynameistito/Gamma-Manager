# Plan 001: Migrate Gamma Manager to Native SDK with a Windows display bridge

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan in
> `plans/README.md`.
>
> **Drift check (run first)**: `git diff --stat 6c1abfc..HEAD -- "Gamma Manager" installer README.md dev.ps1`.
> The repository also has a working-tree diff that predates this plan. Do not
> reset, checkout, stash, or overwrite it. Compare the current files against
> the excerpts below before modifying them.

## Status

- **Priority**: P1
- **Effort**: L — staged migration across a legacy WinForms app, a new Native
  app, a Windows hardware bridge, tests, and packaging.
- **Risk**: HIGH — the app writes display hardware state and currently has no
  automated regression suite.
- **Depends on**: none
- **Category**: migration
- **Planned at**: commit `6c1abfc`, 2026-09-09

## Why this matters

Gamma Manager is a local Windows utility, not a browser application. Its value
comes from applying gamma ramps and hardware monitor controls through Windows
APIs while remaining available in the system tray. Native SDK can provide the
new declarative UI, model/update loop, Windows window host, tray, automation,
and a release binary, but the SDK does not document direct Win32, DXVA2, or WMI
access from a TypeScript app core. The safe migration is therefore a Native
frontend plus an explicit Windows display-control boundary, followed by a
go/no-go test for replacing that boundary with Zig/Native code.

## Current state

### Gamma Manager repository

- `Gamma Manager/Gamma Manager.csproj` — classic Visual Studio project,
  `WinExe`, .NET Framework 4.7.2, WinForms, x86 Release output, and legacy
  ClickOnce/signing properties (`:8-11`, `:40-60`, `:64-87`).
- `Gamma Manager/Program.cs` — STA process entry point; detects
  `--minimized` and starts the WinForms `Window` (`:8-27`).
- `Gamma Manager/Window.cs` — stateful form containing UI orchestration,
  monitor selection, tray commands, preset persistence, Windows theme/DWM
  integration, startup registry writes, and all slider/event handlers
  (`:11-42`, `:92-132`, `:333-433`, `:436-513`, `:564-1386`).
- `Gamma Manager/Window.Designer.cs` — current fixed 1060×452 form with gamma,
  contrast, brightness, monitor selectors, preset controls, hardware controls,
  calibration image, and tray icon (`:71-218`, `:247-405`, `:481-498`).
- `Gamma Manager/Gamma.cs` — pure ramp formula plus `gdi32.dll` calls. The
  compatibility contract is gamma `0.30..4.40`, contrast `0.10..100.00`,
  brightness `-1.00..1.00`, three channels, and a `3×256` 16-bit ramp
  (`:15-66`, `:69-91`).
- `Gamma Manager/Display.cs` — monitor enumeration, display identity, physical
  monitor discovery, DDC/CI capability detection, and retained physical
  handles (`:19-36`, `:63-148`).
- `Gamma Manager/ExternalMonitor.cs` — DXVA2 brightness and contrast reads and
  writes (`:8-58`).
- `Gamma Manager/InternalMonitor.cs` — WMI laptop brightness reads and writes
  under `root\WMI` (`:10-35`, `:74-95`).
- `Gamma Manager/Win.cs` — `user32.dll` monitor enumeration and device info
  interop (`:81-102`).
- `Gamma Manager/IniFile.cs` — Win32 profile-API persistence for the INI file
  (`:9-65`).
- `installer/GammaManager.iss` — Inno Setup Program Files install, shortcuts,
  uninstaller, and optional HKCU startup entry with `--minimized` (`:6-43`).
- `installer/build.ps1` — MSBuild Release rebuild followed by Inno Setup
  (`:11-37`).
- `dev.ps1` — MSBuild Debug build followed by launching the local executable
  (`:3-22`).
- `README.md` — user-visible behavior and adjustment ranges (`:3-37`) plus
  the current build commands (`:39-46`).

The current working tree is not clean. Existing user changes are present in
`Gamma Manager/Program.cs`, `Gamma Manager/Window.cs`,
`Gamma Manager/Window.Designer.cs`, `README.md`, `installer/GammaManager.iss`,
and untracked `dev.ps1`. The executor must preserve them and must not use them
as a reason to reset the tree.

### Native SDK facts verified at upstream commit `31663e1`

The reference repository is `https://github.com/vercel-labs/native`, cloned
read-only during planning at commit `31663e1`.

- `README.md:1-7` describes Native SDK as a native-rendered desktop toolkit:
  TypeScript plus Native markup compile to native code; no browser, WebView,
  or JavaScript runtime ships in a native-rendered binary.
- `README.md:49-76` establishes the authoring loop: `src/app.native`,
  `src/core.ts`, a manifest, `native check`, `native dev --core`, `native dev`,
  and `native build`.
- `README.md:111-114` says Windows has a Win32 host and is a mature desktop
  target, while `README.md:132-134` marks the SDK pre-1.0 and warns that APIs
  still move.
- The selected published SDK version is `0.10.1` (`packages/native-sdk` and
  `packages/core`); pin this version rather than tracking `main`. The upstream
  project is active and rapidly changing, with open Windows/build and helper
  packaging work (for example issues #365, #167, and #149 at
  `https://github.com/vercel-labs/native/issues`).
- `docs/src/app/docs/platform-support/page.mdx:17-28` and `:66-78` list
  Windows real OS windows, menus, and tray support.
- `docs/src/app/docs/platform-support/page.mdx:106-119` says Windows packaging
  is directory-based early support and has no code-signing tooling.
- `docs/src/app/docs/platform-support/page.mdx:331-348` documents runtime
  feature queries and explicit unsupported-operation errors.
- `docs/src/app/docs/windows/page.mdx:168-183` documents `close_policy =
  "hide"`; on Windows it requires the tray capability because the tray is the
  only re-show path.
- `docs/src/app/docs/tray/page.mdx:1-5`, `:137-152`, and `:154-193` document
  Windows tray/status-item APIs, command dispatch, model-derived TypeScript
  status items, and the 32-row menu limit.
- `docs/src/app/docs/tray/page.mdx:347-393` is the reference menu-bar lifecycle
  recipe: tray capability, hidden/hidden-on-close window policy, Show, and Quit
  commands.
- `docs/src/app/docs/packaging/page.mdx:230-236` and `:321-330` document
  `native package --target windows`, early directory packaging, and the
  `native doctor`/`native validate` gates.
- `skill-data/ts-core/SKILL.md:6-20` and `:103-145` define the TypeScript core
  subset, `Cmd` effects, file/store boundaries, and the rule that the core is
  deterministic and has no general Node/DOM/JSON/regex runtime.
- `skill-data/ts-core/SKILL.md:157-164` documents `Cmd.spawn` as the supported
  subprocess bridge, with fixed literal argv, optional stdin, line or bounded
  collected stdout, and explicit exit/error messages.
- `skill-data/ts-services/SKILL.md:6-20` and `:112-126` document ordinary
  TypeScript services and subprocess carriers, but explicitly leave custom
  platform integration and new engine capabilities to Zig toolkit extensions.
- `AGENTS.md:5-12` says new apps default to TypeScript plus Native markup with
  `src/core.ts`, `src/app.native`, and `app.json`; use Zig only when explicitly
  chosen or required by a platform extension.
- `AGENTS.md:14-26` gives the SDK verification gates: `zig build test`,
  `zig build validate`, `scripts/gate.sh fast`, and `pnpm --dir docs check`
  when docs are changed. These apply to the SDK repository, not the Gamma
  Manager app unless the SDK itself is modified.

### Recommended target architecture

```
Native Windows app
  src/core.ts + src/app.native + app.json
      │ deterministic Msg/update model
      │ Cmd.spawn with a fixed executable name and bounded stdin/stdout
      ▼
Gamma Manager Display Bridge (Windows-only helper)
  stable request/response protocol
      │ owns user32, gdi32, dxva2, WMI, registry, INI compatibility
      ▼
Windows display hardware and %AppData%\Gamma Manager\GammaManager.ini
```

The bridge is an intentional boundary, not an invitation to shell arbitrary
user input. Every operation must be a fixed command, use a typed request, and
reject unknown operation names, display identifiers, malformed values, and
oversized input. Keep the old WinForms app as a fallback until hardware parity
is proven.

### Capability disposition

| Existing capability | Native SDK status | Migration decision |
|---|---|---|
| WinForms/.NET UI | No documented .NET/WinForms hosting path | Rewrite the UI; do not embed Native in the old form. |
| Native Windows window, controls, menus, tray | Supported on Windows | Port to `src/app.native`, `src/core.ts`, and `app.json`. |
| Gamma ramp (`gdi32`) | No documented SDK display-gamma API | Keep behind the Windows bridge first; evaluate Zig FFI only at the Step 4 gate. |
| DXVA2 external brightness/contrast | No documented SDK API | Keep in the bridge. |
| WMI internal brightness | No documented SDK API | Keep in the bridge. |
| Monitor enumeration/identity | No first-party display-enumeration capability found | Keep in the bridge and return stable IDs. |
| Existing INI presets | No built-in INI abstraction | Preserve through a bridge/importer; do not silently replace it with Native snapshots. |
| Tray per-monitor comboboxes | Tray is command-oriented and capped at 32 rows | Redesign as compact commands plus Settings fallback. |
| Start with Windows | Native login-item API is documented for macOS, not Windows | Keep HKCU Run/installer handling in the Windows boundary. |
| Inno Setup installer | Native Windows output is an early directory artifact | Keep Inno Setup around the Native binary and bridge. |

## Commands you will need

| Purpose | Command | Expected on success |
|---|---|---|
| Drift check | `git diff --stat 6c1abfc..HEAD -- "Gamma Manager" installer README.md dev.ps1` | No unexpected committed changes in the migration inputs; do not reset existing working-tree changes. |
| Existing baseline | `powershell -ExecutionPolicy Bypass -File dev.ps1` | MSBuild Debug succeeds and launches `Gamma Manager\\bin\\Debug\\Gamma Manager.exe`. |
| Existing release/installer | `powershell -ExecutionPolicy Bypass -File installer\\build.ps1` | Release rebuild succeeds and Inno Setup writes `artifacts\\GammaManagerSetup.exe`. |
| Native toolchain | `node --version; zig version` | Node 24+ and Zig 0.16.0 are available; stop before scaffolding if the pinned CLI cannot use the installed toolchain. |
| Native CLI availability | `native version` | Prints Native SDK CLI `0.10.1` (or the exact pinned commit build). Do not track `main`; stop if the CLI is unavailable or mismatched. |
| Native model contract | `native test` | App tests pass and refresh `zig-out/model-contract.zon`; run this before trusting typed markup checks. |
| Native app static check | `native check --strict` | Core, every `.native` view, the fresh model contract, and `app.json` validate with no warnings or errors. |
| Native core loop | `native dev --core --script tests\\core-scenarios.ndjson` | Fed JSON-line message scenarios produce the committed model/effect transcript; this loop does not launch the bridge or a window. |
| Native app development | `native dev` | A Windows native window opens and the real app can launch the bridge; markup edits hot-reload and the app remains responsive. |
| Native automation build | `native build -Dautomation=true` | Release binary includes the documented automation server. |
| Native UI smoke | `native automate wait; native automate snapshot; native automate assert 'ready=true'` | A running automation-enabled app reports ready state and expected accessible controls. Add `native automate tray-action <item-id>` and `native automate screenshot <view-label>` cases to the scripted smoke test. |
| Native release binary | `native build` | A ReleaseFast binary is produced under the Native build output. |
| Windows package | `native doctor --manifest app.json --strict; native validate app.json; native package --target windows --service-binary <staged-bridge>` | Strict environment/manifest checks pass and a Windows distributable directory is produced with the reserved service/bridge binary. |
| SDK-only verification | `zig build test; zig build validate; scripts/gate.sh fast` | Run only in the separate Native SDK checkout if SDK source is changed; all commands pass. |

The existing repository has no test script, package manifest, or test project.
Do not claim the old app is verified by a test suite until the migration adds
one.

## Suggested executor toolkit

- Read the Native SDK documentation at
  `https://native-sdk.dev/quick-start`, `/docs/typescript`,
  `/docs/tray`, `/docs/windows`, `/docs/platform-support`, and
  `/docs/packaging` before authoring the new app.
- Read the upstream examples `examples/menu-bar` and
  `examples/system-monitor-ts` at commit `31663e1` for tray lifecycle, model
  structure, subprocess effects, and markup conventions.
- Use the Native SDK's shipped `native-ui`, `ts-core`, and `ts-services` skills
  if the CLI makes them available. Do not add unsupported Node APIs to
  `src/core.ts`.

## Scope

**In scope** (the only paths the migration implementation may modify):

- `native/` — the new Native app: `app.json`, `package.json`, `tsconfig.json`,
  `src/core.ts`, `src/app.native`, `src/windows/` if needed, `assets/`, and
  app-local tests/scenarios.
- `bridge/` — the Windows display/persistence helper, the small startup
  launcher, and their tests. Reuse proven logic from `Gamma.cs`, `Display.cs`,
  `ExternalMonitor.cs`, `InternalMonitor.cs`, `Win.cs`, and `IniFile.cs`, but
  put the new protocol behind a typed boundary.
- `tests/` — `GammaManager.Tests.csproj`, characterization vectors, bridge
  protocol tests, `startup-registry-smoke.ps1`, `installer-smoke.ps1`, and
  migration acceptance fixtures. Do not place tests beside production source.
- `Gamma Manager.sln` and `Gamma Manager/` — only for extracting/reusing the
  old backend, adding characterization seams, and retiring the WinForms app
  after parity. Do not delete the old app during the proof-of-concept phase.
- `installer/` — update the build to package the Native binary, bridge, and
  startup launcher; retain Inno Setup until Native Windows packaging has a
  supported installer story.
- `README.md`, `.gitignore`, and `plans/README.md` — document the new build,
  compatibility, fallback, and status rules.

**Out of scope** (do not touch):

- The cloned `vercel-labs/native` checkout or upstream SDK source. If a missing
  SDK capability is discovered, record it and stop; do not fork the SDK inside
  Gamma Manager as an unplanned workaround.
- macOS/Linux display backends. Native may build cross-platform later, but the
  current product contract is Windows hardware control.
- A browser-hosted or remote version of Gamma Manager.
- A redesign of the gamma-ramp formula, adjustment ranges, reset semantics, or
  INI keys. Any intentional behavior change needs a separate decision and
  migration note.
- Secrets, signing certificates, or private-key material. Preserve existing
  signing behavior only after a separate credential review; never copy key
  values into source or plans.

## Git workflow

- Before implementation, have the operator commit or otherwise preserve the
  existing working-tree changes. Do not use reset/checkout/stash to make the
  tree clean, and do not overwrite `Window.cs` or the designer.
- Work on a dedicated migration branch. Match the repository's existing
  concise history style unless the maintainer has a newer convention.
- Keep commits atomic: characterization contract, backend seam, Native POC,
  bridge, Native UI, packaging/cutover.
- Do not push or open a pull request unless explicitly requested.

## Steps

### Step 1: Freeze the current behavior and preserve the existing diff

1. Record the working-tree state and the exact legacy executable that will be
   used for comparison. Do not change the existing files until the operator has
   preserved the current diff.
2. Run the Debug build and manually record a baseline matrix on real Windows
   hardware: internal laptop display, one DDC/CI external display, multiple
   displays, unsupported display, non-English decimal locale, existing INI,
   tray restore, startup minimized launch, and reset.
3. Create `tests/legacy-compatibility.md` with the observed results and the
   exact values/ranges from `README.md:14-18`. Record that gamma state currently
   is not restored on exit; `Gamma.GetDeviceGammaRamp` is commented out in
   `Gamma.cs:74-85`, so do not silently introduce restore-on-exit behavior.
4. Add deterministic gamma-ramp characterization tests under `tests/` or an
   explicit legacy test project. Cover defaults, every clamp boundary, extreme
   contrast, all three independent channels, and representative non-unity
   gamma/contrast/brightness combinations. Compare all `3×256` ushort values,
   not only the midpoint.

**Verify**: `powershell -ExecutionPolicy Bypass -File dev.ps1` → the current
legacy Debug executable builds and launches; after creating the test project,
`dotnet test tests\\GammaManager.Tests\\GammaManager.Tests.csproj --configuration
Release` → all recorded vectors pass; `git diff --name-only` → only preserved
user changes plus the allowed `tests/` additions are present.

### Step 2: Define the platform-neutral contract and bridge protocol

1. Define typed records in `bridge/` for:
   - `list_displays` → stable display ID, display name, link, internal/external
     classification, gamma/hardware capability flags, current hardware values.
   - `apply_color` → display ID or an explicit all-displays selector, selected
     channel(s), gamma/contrast/brightness values, and the extreme-contrast
     mode.
   - `apply_hardware` → display ID, brightness, and optional contrast.
   - `reset` → display ID or all-displays selector.
   - `list_presets`, `save_preset`, `delete_preset`, and `set_startup`.
2. Use a bounded one-request/one-response protocol over stdin/stdout that is
   safe for `Cmd.spawn`: each helper process accepts exactly one UTF-8 request
   line of at most 4 KiB on stdin, writes only protocol records to stdout, and
   exits after a terminal record. Use newline-delimited ASCII tags with
   percent- or hex-encoded UTF-8 fields, no embedded newlines, and a final
   `done`/`error` record. Keep every stdout line at or below the documented
   4 KiB `Cmd.spawn` line limit; reject oversized requests/records before doing
   any Windows work. The core may parse this fixed grammar with byte helpers;
   do not use JSON, regular expressions, dynamic imports, or Node APIs in
   `src/core.ts`. If JSON is needed for a richer service boundary, parse it in
   `src/services/` instead and document the generated command contract.
3. Define the `Cmd.spawn` result mapping explicitly: streamed `line` messages
   carry protocol records, a normal helper completion is handled by `exit`,
   and transport/startup/cancellation failures use the documented `err` arm.
   A non-zero helper exit is not assumed to arrive as `err`; the bridge must
   emit a terminal typed error record before exiting, and the app must surface
   a missing terminal record as a transport error. Keep stdin below 4 KiB and
   the full collected transcript below 512 KiB if `collect: true` is used.
4. Make display identity stable within the bridge and decouple it from the
   display label shown in the UI. Preserve legacy INI lookup by display name,
   but write a migration map when a stable ID is first observed. Do not pass
   `IntPtr` physical handles across the process boundary.
5. Define explicit error codes for unavailable DDC/CI, unavailable WMI,
   invalid display, invalid value, ramp failure, registry failure, malformed
   INI, and bridge startup/transport failure. The Native UI must render these
   as recoverable per-display errors instead of crashing or silently ignoring
   a failed Windows call.
6. Keep the bridge command surface closed: fixed operation names, fixed argv,
   no shell command construction, no arbitrary executable path, and no
   user-controlled command-line fragments. Preset names and display IDs are
   data fields only.

**Verify**: `dotnet test
tests\\GammaManager.Tests\\GammaManager.Tests.csproj --configuration Release
--filter FullyQualifiedName~Protocol` → malformed, oversized, unknown,
unsupported, valid, non-zero-exit, missing-terminal-record, and Unicode-field
requests produce deterministic responses; the test's protocol fixture → every
request is below 4 KiB, every response line is below 4 KiB, and any collected
transcript is below 512 KiB; `git diff --check` → no whitespace errors.

### Step 3: Extract and harden the Windows backend without changing the product contract

1. For the first bridge implementation, prefer a separate classic Windows
   helper targeting the existing .NET Framework 4.7.2 runtime so the proven
   P/Invoke/WMI code can be reused with minimal behavior change. Do not
   retarget the first bridge to .NET 8 or rewrite it in Zig unless a concrete
   prerequisite fails; record that decision in `native/README.md`. The later
   pure-Zig/FFI option is a separate optimization after parity.
2. Move the reusable Windows logic behind the bridge boundary. Treat complete
   `3×256` ushort characterization vectors from Step 1 as authoritative; keep
   the existing `Gamma.cs:17-66` arithmetic and rounding behavior until every
   vector passes. “Preserve byte-for-byte” is not a cross-language acceptance
   criterion.
3. Implement monitor discovery using the existing `user32`/DXVA2 calls, but
   make failure explicit. A monitor whose physical API fails must still be
   represented as an internal/unsupported display when possible; it must not
   disappear silently.
4. Ensure every successfully acquired physical monitor handle is released on
   every path, including partial enumeration and bridge shutdown. The existing
   commented disposal method in `Display.cs:150-157` is not sufficient.
5. Check the return value of `CreateDC`, `DeleteDC`, `SetDeviceGammaRamp`, all
   DXVA2 reads and writes, and WMI operations. Return the typed error codes from
   Step 2, and release every `CreateDC` handle with `DeleteDC` on success and
   failure paths; the legacy `Gamma.cs:87-91` implementation currently leaks
   that handle.
6. Preserve reset defaults: gamma/contrast `1.00`, brightness `0.00`, external
   monitor brightness `100`, and external monitor contrast `50`, as implemented
   in `Window.cs:1122-1194`.
7. Preserve the INI location and old section/key names. Continue the first-run
   copy from the executable directory to `%AppData%\Gamma Manager` described
   at `Window.cs:528-538`. Harden malformed legacy values so one bad preset is
   reported and skipped rather than terminating a UI event.
8. Implement startup registry operations in the bridge or a dedicated Windows
   shell adapter, restricted to `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`.
   Do not rely on the Native TypeScript core inspecting process arguments; the
   startup launcher contract is specified and tested in Step 4 and Step 6.

**Verify**: `dotnet test
tests\\GammaManager.Tests\\GammaManager.Tests.csproj --configuration Release
--filter FullyQualifiedName~WindowsBackend` on a machine with one internal and
one external display → enumerate, read, set, reset, and release handles without
errors; `dotnet test
tests\\GammaManager.Tests\\GammaManager.Tests.csproj --configuration Release
--filter FullyQualifiedName~GammaRamp` → unchanged ramp vectors; `dotnet test
tests\\GammaManager.Tests\\GammaManager.Tests.csproj --configuration Release
--filter FullyQualifiedName~Ini` → old sections load and round-trip without key
loss; `powershell -ExecutionPolicy Bypass -File dev.ps1` → the old app still
builds and runs with the extracted backend changes.

### Step 4: Build the Native SDK Windows proof of concept before porting the UI

1. Pin the app to Native SDK `0.10.1` and record the pin in `native/README.md`
   and the editor/package metadata. Install the CLI with the repository's
   package-manager rule (`bun add --global @native-sdk/cli@0.10.1`) and verify
   `native version`; do not silently substitute an unreleased `main` build.
2. Create `native/` with the current Native SDK default authoring shape:
   `src/core.ts`, `src/app.native`, `app.json`, `package.json`, `tsconfig.json`,
   and `assets/`. Use `native init` or an equivalent scaffold from the pinned
   SDK; prefer `app.json` for a new app.
3. Declare a Windows desktop window, the `tray` capability, native views, and
   the minimum permissions needed for the chosen subprocess/service path. Set
   the window to `close_policy: "hide"`; expose tray commands for Open Settings
   and Quit. Do not set the whole app to `initially_hidden` until the startup
   argument behavior is proven.
4. Implement a minimal `Model`/`Msg`/`update` loop and a single bridge request
   using `Cmd.spawn` with a fixed executable name and the Step 2 line protocol.
   Exercise success, terminal typed error, non-zero exit, spawn failure,
   cancellation, malformed response, missing terminal record, and a
   display-control error. `native dev --core --script` must feed synthetic
   bridge result messages; only a real `native dev` run may launch the helper.
5. Prove the install-layout behavior explicitly. In development, copy the
   helper to the exact fixed relative path used by the app. For packaging,
   build the helper first, stage it as the Native package's reserved
   `<app>_services.exe` (or pass it through the documented
   `--service-binary` option), run `native package --target windows`, then
   assert the final directory contains the expected executable. If the package
   command cannot include this helper, add a post-package copy step with a
   clean staging directory and a manifest of hashes; do not assume an
   arbitrary `bridge.exe` is copied. If a fixed argv cannot reliably locate the
   helper in both layouts, stop and choose one of these explicit alternatives:
   a Native Zig core with a supported Windows FFI boundary, a Native SDK
   extension proposed upstream, or a small stable launcher/helper placed on
   PATH by the installer. Do not build a PowerShell-shelling workaround.
6. Prove the lifecycle behavior on Windows: close hides, tray Open restores,
   tray Quit exits once, and the tray remains available after the window is
   hidden. Prove whether a startup-launched process can be distinguished from a
   normal launch. If Native has no supported launch-argument channel, retain
   startup minimization in the installer/launcher boundary and document the
   behavior instead of adding an undocumented API.
7. Add a small Windows launcher executable to the migration scope. It must
   accept a visible-launch or startup-launch mode, write an atomic per-user
   launch marker, start the Native binary without shell interpretation, and
   return the child exit code. The Native boot flow must make one bridge request
   to consume that marker; normal launch shows the window and startup launch
   keeps it hidden behind the tray. The HKCU Run value must target the startup
   launcher, not the Native binary with an unsupported `--minimized` argument.
   Test missing, stale, duplicate, and concurrent markers and document the
   safe default (normal launch or hidden launch) in `native/README.md`.
8. Run the Windows automation/screenshot path where available. The POC must
   expose accessible labels for the main window, tray actions, controls, and
   error state.

**Verify**: from `native/`, `native test` → app tests pass and refresh the
model contract; `native check --strict` → clean typed check; `native dev --core
--script tests\\core-scenarios.ndjson` → expected model/effect transcript with
synthetic bridge results; `native build -Dautomation=true` → an automation
binary; `native dev -Dautomation=true` → live window, helper, launcher, and
tray lifecycle work; `native automate wait`, `native automate snapshot`,
`native automate assert 'ready=true'`, `native automate tray-action <item-id>`,
and `native automate screenshot <view-label>` → the scripted smoke checks;
`native doctor --manifest app.json --strict` → no blocking diagnostics; the
packaging smoke test → the staged helper is found in the final Windows
artifact.

**STOP / GO gate**:

- GO with Native TypeScript + bridge if the POC can launch the helper, preserve
  tray/close behavior, and deliver typed error states.
- GO with Native Zig core only if direct Win32 access compiles and the same
  bridge contract can be tested without depending on undocumented SDK internals.
- STOP and report if neither route can control a real display on Windows. Keep
  the existing WinForms application as the supported product and do not port
  the UI for appearance alone.

### Step 5: Port the state machine and UI to Native markup

1. Define `native/src/core.ts` around an explicit immutable model:
   `displays`, `selectedDisplayId`, `allMonitors`, `selectedChannel`, per-display
   color/hardware values, `presets`, `presetDraft`, `extremeContrast`, startup
   preference, theme/appearance, loading flags, and typed error fields.
2. Define an exhaustive `Msg` union for boot/discovery, every slider/text
   change, channel selection, all-monitors selection, reset, preset list/load/
   save/delete, startup toggle, tray commands, bridge success/error, window
   hide/show, and quit. Keep all effects in `update` return values; markup only
   binds model values and dispatches messages.
3. Port the control semantics from `Window.cs:564-975`: decimal values use
   hundredths, both current and invariant cultures are accepted at the input
   boundary, values clamp to the existing slider ranges, and extreme contrast
   expands only the contrast maximum from `3.00` to `100.00`.
4. Port all-monitors behavior from `Window.cs:253-260` and the handlers at
   `:572-801`: apply the selected color adjustment and hardware setting to
   every eligible display, while preserving each display's model state and
   returning per-display errors.
5. Create Native markup for the current feature groups: color adjustment,
   display/presets, monitor hardware, and calibration preview. Reuse
   `Gamma Manager/Resources/TestMonitor.jpg` or convert it to an asset under
   `native/assets/`; package it through the manifest rather than a hard-coded
   source-tree path.
6. Implement the Windows tray through `statusItem(model)` and `commandMsg`.
   Keep Open Settings, Reset where safe, and Quit in the tray. Do not attempt
   to recreate WinForms' nested monitor/preset comboboxes in the tray: Native
   tray menus are flat and capped at 32 rows. If the current monitor/preset
   set exceeds the budget, show a concise monitor summary and direct the user
   to Settings.
7. Use `close_policy = "hide"` and the documented Windows tray requirement.
   Preserve double-click/Open Settings restore behavior. Treat a real Quit as
   `Cmd.quitApp`, not as merely hiding the window.
8. Map Windows appearance changes to Native's supported appearance/theme
   channel and design tokens. Keep theme logic out of the display bridge. If
   Native's Windows appearance event is not supported by the pinned SDK, use a
   stable light/dark default and record the limitation rather than importing
   WinForms/DWM code into the TypeScript core.

**Verify**: `native test` → app tests pass and refresh the model contract;
`native check --strict` → no subset, markup, manifest, or typed-binding
diagnostics; `native dev --core --script tests\\core-scenarios.ndjson` → every
message produces the expected model/effect transition; `native build
-Dautomation=true` plus the documented `native automate wait`, `snapshot`,
`assert`, `tray-action`, and `screenshot` commands → all controls have labels
and the tray commands open, hide, restore, and quit correctly.

### Step 6: Preserve settings, startup, and migration behavior

1. On first Native launch, discover the existing `%AppData%\Gamma Manager\GammaManager.ini`
   and legacy executable-directory INI through the bridge. Do not silently
   replace the file with Native's model persistence until an explicit migration
   decision is made.
2. Preserve the current preset fields and section semantics from
   `Window.cs:1079-1107`: monitor, RGB gamma, RGB contrast, RGB brightness,
   monitor brightness, and monitor contrast. Normalize only the display identity
   mapping; keep user-visible preset names intact.
3. Make save/delete operations atomic at the bridge boundary and return errors
   to the Native model. A blank preset name must remain a validation error.
4. Preserve startup enable/disable behavior in HKCU without requiring runtime
   elevation. The installer may still create the optional Run value, but the
   app's checkbox must update it through the same controlled bridge operation.
5. Use the Step 4 launcher contract for startup minimization. Point the HKCU
   Run value at the packaged startup launcher, migrate an existing value that
   still points directly at the legacy executable, and remove the old value on
   uninstall only when it still belongs to Gamma Manager. A normal shortcut
   must select visible-launch mode; the startup task must select
   startup-hidden mode. Do not pass `--minimized` to the Native binary because
   the TypeScript core has no supported argv channel.
6. Add a one-time migration diagnostic to the README and, if needed, a backup
   of an INI file before rewriting it. Never delete the legacy file until the
   new app has successfully loaded it. Record launcher marker consumption,
   stale-marker cleanup, and existing Run-value migration in the upgrade log.

**Verify**: `dotnet test tests\\GammaManager.Tests\\GammaManager.Tests.csproj
--configuration Release --filter FullyQualifiedName~Ini` → all legacy INI
fixtures round-trip; manual upgrade on a machine with an existing profile →
presets appear and apply to the same display; `powershell -ExecutionPolicy
Bypass -File tests\\startup-registry-smoke.ps1 -ExecutablePath <installed-launcher>`
→ enable, disable, migration, and uninstall leave the expected HKCU value;
the same script plus a normal shortcut and the startup launcher → the window
is visible for normal launch, hidden-but-restorable for startup launch, and the
marker is consumed.

### Step 7: Integrate Windows packaging and retain a safe fallback

1. Make `native build` produce the release binary and build both Windows
   executables: the bridge and the startup launcher. Stage them in a clean,
   deterministic directory with a manifest of SHA-256 hashes. Run `native
   package --target windows --service-binary <staged-bridge>` (or the exact
   documented Windows shortcut) and then verify whether the package copied the
   reserved `<app>_services.exe`; if it did not, copy only the declared bridge
   and launcher into the final staging directory in a post-package step. Never
   assume an arbitrary `bridge.exe` is included by Native packaging.
2. Update `installer/GammaManager.iss` to install the Native binary, bridge,
   startup launcher, assets, and any required runtime files. Preserve Program
   Files, Start Menu, optional desktop shortcut, uninstaller, and the HKCU
   startup task pointing at the launcher with its startup-hidden mode. Do not
   ship only the Native SDK directory artifact as the user-facing installer.
   Because the first bridge remains .NET Framework 4.7.2, add an explicit
   prerequisite check/bootstrap path for that framework or stop and report that
   the helper cannot be supported on the target machine; do not leave a silent
   process-start failure.
3. Update `installer/build.ps1` to run the Native release/package commands,
   compile the bridge and launcher, perform the post-package copy/hash check,
   and then invoke Inno Setup. Fail if any of the three executables or required
   assets is missing, and stage only release artifacts. Remove obsolete
   ClickOnce inputs only after the new installer reproduces the existing
   install/uninstall behavior.
4. Decide architecture explicitly. The current Release is x86; Native and the
   bridge must use a tested common Windows architecture. Prefer x64 unless a
   required legacy API or target machine proves x86 is necessary. Do not ship
   an untested mixture of x86 and x64 binaries.
5. Keep the WinForms executable available as a separately named rollback build
   for at least one release cycle. Do not make the installer delete it until
   the acceptance matrix in Step 8 passes on real hardware.
6. Update `README.md` with Native development, core-check, packaging, bridge
   prerequisites, supported Windows behavior, the fallback/rollback path, and
   the fact that Native Windows packaging is directory-based under the SDK
   version used.

**Verify**: `native package --target windows --service-binary
<staged-bridge>` plus the post-package staging check → the final directory
contains the Native binary, reserved/explicit bridge, startup launcher, icon,
and assets; `powershell -ExecutionPolicy Bypass -File installer\\build.ps1` →
Inno Setup succeeds and produces `artifacts\\GammaManagerSetup.exe`;
`powershell -ExecutionPolicy Bypass -File
tests\\installer-smoke.ps1 -InstallerPath artifacts\\GammaManagerSetup.exe`
on a clean Windows account → Program Files, shortcuts, AppData, startup,
tray, helper resolution, and rollback all behave as documented.

### Step 8: Run the final parity and release gates

Run the following on real Windows hardware, not only in a unit-test process:

- One internal/WMI display: brightness read/write/reset.
- One DDC/CI external display: brightness and contrast read/write/reset.
- A display with unavailable DDC/CI: visible unsupported state and no crash.
- Gamma/contrast/brightness defaults, lower/upper clamps, extreme contrast,
  RGB-only edits, and all-monitors mode.
- Multiple monitors, display reorder, unplug/replug, and a refresh/restart.
- Existing INI file with legacy sections, malformed values, duplicate names,
  and a blank preset name.
- Tray Open/Hide/Settings, monitor/preset selection within and above the
  Native 32-row tray limit, startup minimized launch, normal launch, and Quit.
- Light/dark Windows theme and per-monitor DPI/scaling.
- Install, upgrade over an existing release, uninstall, and rollback to the
  WinForms build.

**Verify**: all manual cases are checked in
`tests/native-windows-acceptance.md`; `native test` → model contract refreshed
and app tests pass; `native check --strict` → typed checks pass; `native build
-Dautomation=true` and the documented `native automate wait/snapshot/assert`,
`tray-action`, and `screenshot` commands → UI smoke passes;
`dotnet test tests\\GammaManager.Tests\\GammaManager.Tests.csproj
--configuration Release` → bridge and migration tests pass;
`powershell -ExecutionPolicy Bypass -File installer\\build.ps1` plus
`powershell -ExecutionPolicy Bypass -File
tests\\installer-smoke.ps1 -InstallerPath artifacts\\GammaManagerSetup.exe` →
packaging and installation pass; no legacy WinForms references remain in the
new Native app except the explicitly documented rollback path.

## Test plan

- `tests/GammaManager.Tests/GammaManager.Tests.csproj` — Windows test project
  target and the exact `dotnet test` entry point for the following suites.
- `tests/gamma-ramp-vectors.*` — exact 3×256 ramp vectors for the old formula,
  bounds, default values, extreme contrast, and channel independence.
- `tests/bridge-protocol.*` — protocol encoding/decoding, fixed operation
  allowlist, bounds, malformed data, typed error codes, and stable display IDs.
- `tests/legacy-ini-fixtures.*` — old AppData and executable-directory files,
  migration, malformed values, section naming, save/delete, and round-trip
  compatibility.
- `native/tests/core-scenarios.ndjson` (or the Native app's supported test
  location) — boot, discovery, slider edits, RGB selection, all-monitors,
  reset, presets, startup, bridge failures, tray commands, and quit.
- Native automation smoke — accessibility labels, button dispatch, text/slider
  updates, error rendering, window hide/show, and tray command routing using
  `native automate` against a `-Dautomation=true` build.
- `tests/native-windows-acceptance.md` — real-hardware matrix from Step 8,
  including internal, external, unsupported, multiple-monitor, locale, DPI,
  upgrade, and rollback cases.
- `tests/startup-registry-smoke.ps1` — launcher marker, HKCU Run migration,
  visible/startup-hidden launch, stale-marker, and uninstall checks.
- `tests/installer-smoke.ps1` — clean-account install, executable-resolution,
  startup, tray, upgrade, uninstall, and rollback checks; it must fail loudly
  rather than report a successful install when the bridge cannot start.

Use the Native SDK's `native dev --core` transcript loop for deterministic
model tests and `native test`/automation for UI behavior. Do not replace real
hardware tests with mocks for the Win32, DXVA2, WMI, registry, or tray paths.

## Done criteria

- [ ] The current user changes were preserved; no reset/checkout/stash was used
      to discard them.
- [ ] The Native app passes `native check` and `native test`.
- [ ] The Native core has no unsupported Node, DOM, JSON, regex, or arbitrary
      filesystem calls.
- [ ] A fixed, typed bridge protocol covers display enumeration, gamma ramps,
      hardware controls, reset, presets, and startup; it rejects unknown or
      unsafe input and stays within the `Cmd.spawn` framing limits.
- [ ] All legacy gamma-ramp vectors pass exactly, and no range/reset behavior
      changed unintentionally.
- [ ] Existing INI files load and round-trip without losing preset fields.
- [ ] Real Windows hardware passes the acceptance matrix for internal,
      external, unsupported, multiple, and reordered displays.
- [ ] Windows tray Open/Hide/Settings/Quit behavior works; tray menus stay
      within the documented 32-row limit or route to Settings.
- [ ] `native build` and strict `native doctor` pass.
- [ ] `native package --target windows` produces a distributable directory
      containing the Native binary, bridge, startup launcher, icon, and assets;
      the post-package hash check proves the bridge was not silently omitted.
- [ ] `powershell -ExecutionPolicy Bypass -File installer\\build.ps1` produces
      a working Inno installer; `tests\\startup-registry-smoke.ps1` and
      `tests\\installer-smoke.ps1` pass, and uninstall leaves no stale startup
      entry.
- [ ] The existing WinForms build remains a documented rollback until the
      maintainer explicitly removes it.
- [ ] `plans/README.md` status row is updated to `DONE`, `BLOCKED`, or
      `REJECTED` with a reason.

## STOP conditions

Stop and report back instead of improvising if:

- The current code does not match the cited excerpts, or the existing
  uncommitted user changes are not preserved.
- Native CLI behavior differs from the pinned upstream commit enough that the
  documented TypeScript/core/tray/package APIs are unavailable.
- `Cmd.spawn` cannot safely and deterministically launch the bridge from both
  development and the packaged Windows directory within the documented stdin,
  line, exit, and output bounds.
- Native packaging silently omits the bridge or launcher and no explicit,
  hash-checked post-package staging step can make the final artifact complete.
- The launcher cannot distinguish normal from startup-hidden launch without
  an undocumented Native argv/environment channel, an unsafe marker protocol,
  or shell interpretation.
- A pure Native/TypeScript app cannot access the required Windows controls and
  no tested bridge path is available.
- The bridge requires arbitrary shell commands, user-controlled executable
  paths, or undocumented Native host internals.
- The Native tray cannot restore a hidden Windows window, or the required menu
  exceeds the documented 32-row limit without a clear Settings fallback.
- Existing INI data cannot be parsed/migrated without changing user-visible
  preset behavior.
- A required hardware or packaging test can only be passed by weakening the
  acceptance criteria or hiding an error.
- Any verification command fails twice after a reasonable, scoped fix attempt,
  or a fix requires an out-of-scope path.

## Maintenance notes

- The bridge is the compatibility seam. Any future monitor API, display-ID,
  protocol, or privilege change must update the bridge contract and its
  fixtures before changing `src/core.ts`.
- Keep the Native SDK commit/version pinned. It is pre-1.0 and its APIs move;
  after every SDK update, run `native test` first to refresh the model contract,
  then `native check --strict`, tray lifecycle automation, Windows packaging,
  and the installer smoke tests.
- The Native tray menu is not a replacement for arbitrary WinForms controls.
  Reviewers should check the 32-row cap, display/preset overflow behavior, and
  accessibility labels.
- Do not place user-visible text in dynamic JavaScript strings in the core;
  Native's TypeScript subset uses bytes for runtime text. Keep protocol parsing
  and ordinary Node/JSON work in the documented service/bridge boundary.
- The old formula intentionally remains unchanged in this migration. A future
  color-science change needs new vectors, a versioned preset decision, and a
  separate release note.
- Revisit removal of the WinForms fallback only after one release cycle of
  hardware and installer telemetry/feedback; it is not part of the first
  Native cutover.
