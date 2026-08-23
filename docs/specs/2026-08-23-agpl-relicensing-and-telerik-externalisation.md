# AGPL-3.0 relicensing and Telerik externalisation

Status: approved, not yet implemented
Date: 2026-08-23

## Problem

Two problems, one of which blocks the other.

Oasis is to be released under AGPL-3.0. It currently carries no `LICENSE` file, which under
default copyright means nobody has any right to use it, despite the repository being public.

The repository also redistributes 113 Telerik UI for WinForms assemblies under `lib/RCWF/`.
These are trial builds, and the Telerik licence does not permit redistributing them. That is
already a licence violation on a public repository today, and it makes any open-source licence
incoherent, because the repository as published cannot legally be redistributed under it.

A third issue surfaced while scoping: AGPL-3.0 forbids conveying a covered work combined with
proprietary libraries. Oasis links Telerik and GemBox.Document and ships as a ClickOnce binary.
Plain AGPL would therefore prohibit Synovora from distributing its own product.

## Decisions

1. Licence is **AGPL-3.0 plus an additional permission under section 7** naming the proprietary
   components. This is what makes AGPL workable for a product with paid dependencies.
2. Telerik binaries leave the repository. Developers supply their own installation.
3. The build locates Telerik through a single MSBuild property with layered fallback, rather
   than through the Telerik NuGet feed or a bootstrap script.

## Current state

Telerik is used by exactly two projects, both referencing `2022.2.622.40.Trial`:

| Project | Assemblies referenced |
|---|---|
| `oasis/Oasis_WF.vbproj` | 10 |
| `Oasis_Common/Oasis_Common.vbproj` | 2 (`Telerik.WinControls.dll`, `TelerikCommon.dll`) |

That is 12 `<HintPath>` entries in total. The 10 distinct assemblies are:

```
Telerik.WinControls.dll                          TelerikCommon.dll
Telerik.WinControls.UI.dll                       Telerik.WinControls.GridView.dll
Telerik.WinControls.ChartView.dll                Telerik.WinControls.Scheduler.dll
Telerik.WinControls.RichTextEditor.dll           Telerik.Windows.Documents.Core.dll
Telerik.WinControls.Themes.Office2007Silver.dll  Telerik.Windows.Zip.dll
```

`lib/RCWF/` also holds `2022.1.222.40.Trial`, which is referenced only by the two untracked
backup projects. It has no live consumer and is deleted with the rest.

`oasis/Oasis_WF.vbproj` carries a VB namespace import `<Import Include="Telerik.WinControls.Design" />`
with no corresponding assembly reference. It resolves to nothing and produces a BC40056 warning
suppressed by the project's `NoWarn` list. Left alone; noted so the next reader does not chase it.

## Design

### Licensing

**`LICENSE`** contains the full AGPL-3.0 text with the exception appended below it, so the two
cannot be separated:

```
                    OASIS LINKING EXCEPTION

Additional permission under GNU AGPL version 3 section 7

If you modify this Program, or any covered work, by linking or combining it
with Telerik UI for WinForms, Telerik Document Processing, or GemBox.Document
(or modified versions of those libraries), the licensors of this Program grant
you additional permission to convey the resulting work.
```

Copyright holder stays Synovora, which preserves the option to dual-license.

**`NOTICE`** lists third-party components and their licences. Telerik and GemBox are proprietary
and stay that way. Nethereum, MailKit, MimeKit, BouncyCastle, Newtonsoft.Json, QRCoder and the
vendored front-end libraries keep their own terms. None of them become AGPL.

**AGPL section 13.** Users interacting with the program over a network must be offered the
corresponding source. The patient portal is exactly that case, so
`Oasis_Web/Views/Shared/_footer.vbhtml` gains a source link. This is a small view change and it is
the difference between complying and not complying.

**Healthcare disclaimer** in the README, modelled on the OpenMRS wording: no warranty, not a
certified medical device, deployers are responsible for regulatory compliance in their
jurisdiction. This does not replace legal advice on EU MDR classification, which is out of scope
here.

**README** licence section is rewritten from "Proprietary" to AGPL-3.0, with the exception and the
disclaimer summarised and linked.

### Telerik removal

`lib/RCWF/` is deleted from the working tree and ignored wholesale. The current `.gitignore`
allowlist that preserved two versions is removed, since no version is tracked any more.

Effect on repository size: roughly 193 MB down to roughly 76 MB.

### Build resolution

A committed `Directory.Build.props` at the repository root defines `$(TelerikWinFormsDir)`,
resolved in order, first hit wins:

1. An explicit `/p:TelerikWinFormsDir=...` on the MSBuild command line.
2. The `TELERIK_WINFORMS_DIR` environment variable.
3. `Telerik.props.user` at the repository root, if present. Gitignored.
4. The default Telerik Control Panel path,
   `C:\Program Files (x86)\Progress\Telerik UI for WinForms R2 2022\Bin40`.
5. `lib\RCWF\2022.2.622.40.Trial`, if it still exists on disk.

Step 5 is deliberate. Every developer who has the folder today keeps building with no action on
the day this lands, which turns a breaking change into an opt-in migration.

Both projects import `Microsoft.Common.props`, which imports `Directory.Build.props`, and both
declare `ToolsVersion="15.0"`. The mechanism is therefore available to them.

The 12 `<HintPath>` entries change from

```xml
<HintPath>..\lib\RCWF\2022.2.622.40.Trial\Telerik.WinControls.dll</HintPath>
```

to

```xml
<HintPath>$(TelerikWinFormsDir)\Telerik.WinControls.dll</HintPath>
```

### Failure mode

A `ValidateTelerik` target runs `BeforeTargets="CoreCompile"` in `Directory.Build.props` and, when
the directory does not resolve or a required assembly is absent, fails the build with the
resolution order, the list of required assemblies, and a pointer to the README section.

This matters more than it looks. Without it, a missing Telerik produces several hundred "type not
found" errors originating in generated Designer files, which tells a new developer nothing about
the actual cause.

### Design-time assemblies

`TELERIK_WINFORMS_DIR` must point at a complete `Bin40` folder, not a hand-picked copy of the 10
referenced assemblies. The Visual Studio WinForms designer loads design-time assemblies such as
`Telerik.WinControls.UI.Design.dll` that the compiler never references. A minimal copy compiles
cleanly while every `RadForm` fails to open in the designer, which is a confusing failure to
diagnose. The README says so explicitly and the validation target checks for one design-time
assembly as a proxy.

### Documentation

README gains a "Telerik setup" subsection under Requirements covering how to obtain a licence,
where the Control Panel installs, the three ways to point the build at an installation, and how to
read the `ValidateTelerik` error.

## Files touched

| File | Change |
|---|---|
| `LICENSE` | new, AGPL-3.0 plus section 7 exception |
| `NOTICE` | new, third-party components and licences |
| `Directory.Build.props` | new, Telerik resolution and validation |
| `Telerik.props.user.example` | new, template for local override |
| `.gitignore` | ignore `lib/RCWF/` wholesale and `Telerik.props.user`; drop the allowlist |
| `oasis/Oasis_WF.vbproj` | 10 HintPaths repointed |
| `Oasis_Common/Oasis_Common.vbproj` | 2 HintPaths repointed |
| `Oasis_Web/Views/Shared/_footer.vbhtml` | AGPL section 13 source link |
| `README.md` | licence section, healthcare disclaimer, Telerik setup |
| `lib/RCWF/` | deleted from working tree |

## Verification

There is no MSBuild, Visual Studio or Windows on the machine where this is being written, so none
of the build changes can be executed here. The design rests on documented MSBuild behaviour rather
than on an observed build.

Before this is trusted, on a Windows machine with Visual Studio 2022:

1. `msbuild Oasis_WF.sln /p:Configuration=Debug` with `lib/RCWF/` still present. Must succeed via
   fallback step 5.
2. Rename `lib/RCWF/`. The build must fail with the `ValidateTelerik` message, not with type
   errors.
3. Set `TELERIK_WINFORMS_DIR` to a real Control Panel `Bin40`. Build must succeed.
4. Open `oasis/Form/Synthese/RadFSynthese.vb` in the WinForms designer. It must render, which is
   what proves the design-time assemblies resolved.
5. `vstest.console.exe UnitTest\bin\Debug\UnitTest.dll`. Unaffected, but confirms nothing regressed.

The README will state that the build changes are unverified until someone completes this list.

## Out of scope

- Rotating the exposed database credential. Separate operational task.
- The hardcoded admin password in `OasisAdmini/Form/FrmAdministrateur.vb`.
- Replacing Telerik or GemBox with open alternatives. That is a rewrite of roughly 140k lines of
  UI code, not a licensing task.
- A contributor licence agreement. Needed before dual-licensing, but it is a legal document rather
  than an engineering change.
- EU MDR classification advice.

## Risks

**The prior history is public.** Relicensing applies going forward. The Telerik binaries existed in
the repository's published history until the reset on 2026-08-23, and clones or forks taken before
then still contain them. Removing them now stops the ongoing violation; it does not undo it.

**Contributor copyright.** The 758 archived commits may include contributions from people who never
assigned copyright. Relicensing assumes Synovora holds it. Worth confirming before publicising the
licence change.

**Telerik version drift.** Pointing at "whatever the developer installed" allows a build against a
version other than 2022.2.622.40. The validation target checks the directory and assemblies exist,
not that versions match, because `<Reference>` entries carry explicit `Version=` attributes and
MSBuild will warn on mismatch. Accepted rather than solved.
