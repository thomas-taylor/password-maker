# .NET 10 Upgrade Plan for PasswordMaker Solution

## Table of Contents
- [1. Executive Summary](#1-executive-summary)
- [2. Migration Strategy](#2-migration-strategy)
- [3. Detailed Dependency Analysis](#3-detailed-dependency-analysis)
- [4. Project-by-Project Plans](#4-project-by-project-plans)
- [5. Package Update Reference](#5-package-update-reference)
- [6. Breaking Changes Catalog](#6-breaking-changes-catalog)
- [7. Testing & Validation Strategy](#7-testing--validation-strategy)
- [8. Complexity & Effort Assessment](#8-complexity--effort-assessment)
- [9. Source Control Strategy](#9-source-control-strategy)
- [10. Success Criteria](#10-success-criteria)

---

## 1. Executive Summary
This plan upgrades all projects in the `PasswordMaker` solution to `.NET 10` with an All-At-Once Strategy. All NuGet packages will be upgraded to the latest compatible versions during the framework upgrade.

### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in single operation.

**Rationale**:
- 8 projects (small/medium solution)
- Most projects currently on .NET 9; shared libraries on `netstandard2.1`
- Clear dependency structure (two shared libraries used across Blazor, Avalonia, WPF)
- Package upgrades known for Blazor/WebAssembly and System.Text.Json; Avalonia.Android has incompatibility to resolve

### Target Frameworks
- `PasswordMakerWpf` ? `net10.0-windows`
- `PasswordMakerAvalonia.Desktop` ? `net10.0-windows`
- `PasswordMakerAvalonia.Android` ? `net10.0-android`
- `PasswordMakerAvalonia.Browser` ? `net10.0-browser`
- `PasswordMakerAvalonia` ? `net10.0`
- `PasswordMakerBlazor` ? `net10.0`
- `PasswordMaker` ? stays `netstandard2.1` (cross-platform shared lib)
- `PasswordMakerClient` ? stays `netstandard2.1` (shared lib used by all)

### Complexity Classification
- Overall: Medium
- High-risk area: `PasswordMakerWpf` (307 binary incompatible APIs)
- Medium-risk areas: Blazor/WebAssembly behavioral changes (3 incidents in Blazor, 3 in Avalonia.Browser)
- Low-risk areas: Class libraries moving from net9.0 to net10.0

### Critical Issues
- `Avalonia.Android` incompatible packages (AndroidX SplashScreen) — requires version reconciliation
- `System.Uri` behavioral changes — validate URI usage paths (notably WPF)

### Iteration Strategy Used
- Phase 0: Preparation
- Phase 1: Atomic Upgrade (frameworks + packages + compilation fixes)
- Phase 2: Test Validation

## 2. Migration Strategy
We will perform a unified, atomic upgrade of all projects and packages.

### Ordering Principles
- Apply framework changes across all projects simultaneously
- Update package references across all projects simultaneously
- Restore and build once; fix compilation errors discovered; rebuild to verify
- Tests run only after solution builds with 0 errors

### Multi-Targeting
- No multi-targeting required. Libraries on `netstandard2.1` remain as-is for broad compatibility.

### Special Considerations
- Blazor/WebAssembly: upgrade `Microsoft.AspNetCore.Components.WebAssembly` and `DevServer` to `10.0.1`; upgrade `Microsoft.JSInterop` to `10.0.1`
- WPF: large set of binary incompatible APIs. Expect XAML and code-behind adjustments.
- Avalonia.Android: reconcile `AndroidX.Core.SplashScreen` version with `net10.0-android` toolchain; verify `AndroidManifest` and SDK compatibility.

## 3. Detailed Dependency Analysis
### Solution Graph Summary
- Roots (apps): `PasswordMakerBlazor`, `PasswordMakerAvalonia.Desktop`, `PasswordMakerAvalonia.Browser`, `PasswordMakerAvalonia.Android`, `PasswordMakerWpf`
- Shared libraries: `PasswordMaker`, `PasswordMakerClient`
- Dependencies:
  - Apps depend on shared libraries (`PasswordMaker`, `PasswordMakerClient`)
  - Avalonia apps also depend on `PasswordMakerAvalonia`

### Migration Grouping (All-At-Once)
- Single atomic group: all projects listed above upgraded together with package updates.

### Critical Path
- Ensure shared libs (`PasswordMaker`, `PasswordMakerClient`) remain compatible for consumers on net10.0
- Address WPF API incompatibilities in `PasswordMakerWpf` immediately after build identifies errors.

## 4. Project-by-Project Plans

### Project: PasswordMakerWpf
**Current State**: net9.0-windows; WPF; 6 files; 320+ LOC modifications expected; 307 binary incompatible APIs
**Target State**: net10.0-windows
**Migration Steps**:
1. Update `TargetFramework` to `net10.0-windows`
2. Validate WPF desktop support (implicit on windows TFM)
3. Review XAML and code-behind for API changes listed in assessment (TextBox, Grid, Uri behaviors)
4. Adjust event handlers and data binding where binary incompatibilities occur
5. Validation: builds; WPF UI smoke tests in Test Validation phase

### Project: PasswordMakerAvalonia.Desktop
**Current State**: net9.0; WinForms (hosting Avalonia?)
**Target State**: net10.0-windows
**Migration Steps**:
1. Update `TargetFramework` to `net10.0-windows`
2. Verify Avalonia.Desktop references remain compatible
3. Validation: builds

### Project: PasswordMakerAvalonia.Android
**Current State**: net9.0-android; incompatible `AndroidX.Core.SplashScreen`
**Target State**: net10.0-android
**Migration Steps**:
1. Update `TargetFramework` to `net10.0-android`
2. Upgrade `Xamarin.AndroidX.Core.SplashScreen` to version compatible with net10.0-android
3. Validate Android SDK and manifest compatibility
4. Validation: builds

### Project: PasswordMakerAvalonia.Browser
**Current State**: net9.0-browser; behavioral changes 3; depends on Avalonia and shared libs
**Target State**: net10.0-browser
**Migration Steps**:
1. Update `TargetFramework` to `net10.0-browser`
2. Upgrade `Microsoft.JSInterop` to `10.0.1`
3. Review behavioral changes (WebAssembly interop lifecycle)
4. Validation: builds; runtime validation in Test phase

### Project: PasswordMakerAvalonia
**Current State**: net9.0
**Target State**: net10.0
**Migration Steps**:
1. Update `TargetFramework` to `net10.0`
2. Verify Avalonia packages remain compatible
3. Validation: builds

### Project: PasswordMakerBlazor
**Current State**: net9.0; Blazor WebAssembly; 3 package upgrades recommended; 3 behavioral changes
**Target State**: net10.0
**Migration Steps**:
1. Update `TargetFramework` to `net10.0`
2. Upgrade `Microsoft.AspNetCore.Components.WebAssembly` and `DevServer` to `10.0.1`
3. Upgrade `Microsoft.JSInterop` to `10.0.1`
4. Review behavioral changes (JS interop, hosting model, boot lifecycle) and adjust Program.cs/Service registrations if needed
5. Validation: builds; run wasm app in Test phase

### Project: PasswordMaker
**Current State**: netstandard2.1; 1 package upgrade recommended (`System.Text.Json`)
**Target State**: netstandard2.1
**Migration Steps**:
1. Keep `TargetFramework` as `netstandard2.1`
2. Upgrade `System.Text.Json` to `10.0.1`
3. Validation: builds; consumers build clean

### Project: PasswordMakerClient
**Current State**: netstandard2.1; no issues
**Target State**: netstandard2.1
**Migration Steps**:
1. No framework change
2. Validation: builds; consumers build clean

## 5. Package Update Reference

### Common Package Updates (affecting multiple projects)
- `Microsoft.JSInterop`: 9.0.0 ? 10.0.1 (Projects: PasswordMakerBlazor, PasswordMakerAvalonia.Browser)

### Category-Specific Updates
**Blazor/WebAssembly**
- `Microsoft.AspNetCore.Components.WebAssembly`: 9.0.0 ? 10.0.1
- `Microsoft.AspNetCore.Components.WebAssembly.DevServer`: 9.0.0 ? 10.0.1

**Shared Libraries**
- `System.Text.Json`: 9.0.0 ? 10.0.1 (PasswordMaker)

**Android**
- `Xamarin.AndroidX.Core.SplashScreen`: 1.0.1.13 ? [Validate compatible version for net10.0-android]

### Project-Specific Exceptions
- Avalonia packages are compatible as-is per assessment; keep versions unless net10 toolchain requires bumps.

## 6. Breaking Changes Catalog

### Framework Breaking Changes
- WPF binary incompatible APIs (ref. assessment Top API Migration Challenges)
- `System.Uri` behavioral changes (constructor and AbsolutePath semantics)

### Package API Changes
- Blazor/WebAssembly and JS interop lifecycle adjustments in .NET 10
- DevServer configuration alignment with .NET 10 SDK

### Configuration Updates
- Blazor Program.cs hosting and service registrations may require minor adjustments
- Ensure Browser projects align with wasm boot changes

## 7. Testing & Validation Strategy

### Phase 2: Test Validation
- Execute all test projects (if any) and perform app smoke tests:
  - Start `PasswordMakerBlazor` wasm in dev server; verify basic navigation and JS interop calls
  - Launch `PasswordMakerAvalonia.Desktop`; verify window opens
  - Run `PasswordMakerAvalonia.Browser` wasm; verify boot
  - Build `PasswordMakerAvalonia.Android` and validate manifest
  - Launch `PasswordMakerWpf`; verify core views render

### Validation Checkpoints
- All projects build with 0 errors
- No package restore conflicts
- Apps start without critical runtime errors

## 8. Complexity & Effort Assessment
- `PasswordMakerWpf`: High complexity (binary incompatibilities)
- Blazor/WebAssembly projects: Medium complexity (behavioral changes, package upgrades)
- Avalonia projects: Low/Medium depending on Android compatibility
- Shared libraries: Low complexity

## 9. Source Control Strategy

### Branching
- Source branch: `main`
- Upgrade target branch: `upgrade-to-NET10`
- Pending changes action: `commit` prior to branch creation

### Commit Strategy
- Prefer single commit for the entire atomic upgrade if possible
- Commit message: "Upgrade all projects to .NET 10 and update packages (atomic)"

### Review & Merge
- Single PR from `upgrade-to-NET10` to `main`
- PR checklist:
  - Builds succeed
  - Apps start
  - Package updates aligned with plan

## 10. Success Criteria
- All projects target proposed frameworks
- All specified packages updated to target versions
- Solution builds with 0 errors
- Apps start with no critical issues
- No security vulnerabilities remain
