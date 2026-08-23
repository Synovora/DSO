# AGPL relicensing and Telerik externalisation, implementation plan

> Execute task by task, in order. Each task ends with its own verification and commit.
> Steps use checkbox (`- [ ]`) syntax so progress can be tracked in place.

**Goal:** Publish Oasis under AGPL-3.0 with a section 7 linking exception, and stop redistributing commercial Telerik assemblies by resolving them from a developer-supplied installation.

**Architecture:** Licensing is three new plain-text files plus a view change to satisfy AGPL section 13. The Telerik change introduces one `Directory.Build.props` at the repository root that resolves `$(TelerikWinFormsDir)` through a five-step fallback, and repoints 12 `<HintPath>` entries in the two projects that reference Telerik. A `ValidateTelerik` target converts a missing installation from several hundred confusing type errors into one readable message.

**Tech Stack:** MSBuild 15 / Visual Studio 2022, VB.NET, .NET Framework 4.7.2, old-style `.vbproj` with `packages.config`.

**Spec:** `docs/specs/2026-08-23-agpl-relicensing-and-telerik-externalisation.md`

## Global Constraints

- Target framework is `.NET Framework 4.7.2`. Do not change it.
- Telerik version in use is `2022.2.622.40`. Assembly `Version=` attributes in the `.vbproj` files must not be edited.
- Only two projects reference Telerik: `oasis/Oasis_WF.vbproj` (10 assemblies) and `Oasis_Common/Oasis_Common.vbproj` (2 assemblies). Total 12 `<HintPath>` entries.
- Copyright holder is `Synovora`. Every licence header uses that name.
- No commit message, code comment, or committed prose may mention AI, agents, or assistants.
- No em dash character in any committed prose. Use a comma, a colon, parentheses, or a new sentence.
- No commercial binary may be added to git tracking at any point.
- The machine running this plan is macOS with no MSBuild, so no task may claim a build was verified. Build verification is deferred to Task 6.

## Deviation from the spec, read before starting

The spec says `lib/RCWF/` is "deleted from the working tree". **Do not delete it from disk.**
Untrack it and ignore it instead, leaving the files in place.

Reason: this is very likely the only Telerik copy on the machine, and fallback step 5 in
`Directory.Build.props` points at exactly that folder. Keeping it on disk means the local build
keeps working immediately after this change, while git stops redistributing it, which is the
actual objective. Deleting it achieves nothing extra and destroys the developer's toolchain.

---

### Task 1: Licence and attribution files

**Files:**
- Create: `LICENSE`
- Create: `NOTICE`

**Interfaces:**
- Consumes: nothing.
- Produces: `LICENSE` is referenced by Task 2 (footer link text), Task 5 (README licence section) and the `NOTICE` file.

- [x] **Step 1: Download the canonical AGPL-3.0 text**

```bash
cd /Users/paultheis/Documents/Code/DSO
curl -sS --max-time 60 -o LICENSE https://www.gnu.org/licenses/agpl-3.0.txt
```

**What actually happened:** gnu.org was unreachable at execution time, over both HTTPS and HTTP,
after four retries. The text was taken from the SPDX license list instead, which is the standard
machine-readable source and is what GitHub's own licence detection is built on:

```bash
curl -sS --connect-timeout 15 --max-time 60 -o LICENSE \
  https://raw.githubusercontent.com/spdx/license-list-data/main/text/AGPL-3.0-only.txt
```

The two differ only in whitespace: SPDX serves the text unwrapped and without the centring spaces
on headings, so it is 34020 bytes rather than 34523. The wording is identical. If you want the
canonical gnu.org formatting later, replacing the licence body is a safe one-line swap.

- [x] **Step 2: Verify the download is complete and untampered**

Byte count cannot be used with the SPDX copy, so verify by structure instead:

```bash
grep -cE '^ *[0-9]+\. [A-Z]' LICENSE            # expect 18, sections 0 through 17
grep -c "TERMS AND CONDITIONS" LICENSE          # expect 2
grep -c "END OF TERMS AND CONDITIONS" LICENSE   # expect 1
grep -c "13. Remote Network Interaction" LICENSE # expect 1, the clause that makes it AGPL
grep -c "How to Apply These Terms" LICENSE      # expect 1
```

Do not proceed with a partial licence file.

- [ ] **Step 3: Append the section 7 linking exception**

```bash
cat >> LICENSE <<'EOF'

-----------------------------------------------------------------------

                        OASIS LINKING EXCEPTION

Additional permission under GNU AGPL version 3 section 7

If you modify this Program, or any covered work, by linking or combining it
with Telerik UI for WinForms, Telerik Document Processing, or GemBox.Document
(or modified versions of any of those libraries), the licensors of this
Program grant you additional permission to convey the resulting work.

This exception applies only to the libraries named above. It does not extend
to any other non-free component, and it does not alter your obligations under
the GNU Affero General Public License with respect to the Program itself.

Copyright (C) 2026 Synovora
EOF
```

- [ ] **Step 4: Create the NOTICE file**

```bash
cat > NOTICE <<'EOF'
Oasis
Copyright (C) 2026 Synovora

This product is licensed under the GNU Affero General Public License v3.0,
with an additional permission under section 7 for linking against the
proprietary components listed below. See the LICENSE file.

This product bundles or depends on third-party components. Those components
are NOT covered by the AGPL and remain subject to their own licence terms.

Proprietary, licence required, not redistributed with this repository
---------------------------------------------------------------------
  Telerik UI for WinForms          Progress Software, commercial
  Telerik Document Processing      Progress Software, commercial
  GemBox.Document                  GemBox Ltd, commercial

Open source dependencies
------------------------
  Nethereum (Signer, Hex, RLP, Util, Model)    MIT
  MailKit                                      MIT
  MimeKit                                      MIT
  Newtonsoft.Json                              MIT
  QRCoder                                      MIT
  X.PagedList                                  MIT
  BouncyCastle / Portable.BouncyCastle         MIT
  Microsoft ASP.NET MVC, Web API, Razor        Apache-2.0
  Microsoft.Extensions.*                       MIT

Vendored front-end assets under Oasis_Web/
------------------------------------------
  Bootstrap                                    MIT
  Popper.js                                    MIT
  Leaflet                                      BSD-2-Clause
  ECharts                                      Apache-2.0
  Dropzone                                     MIT
  ion.rangeSlider                              MIT
  pdfmake                                      MIT

Obtaining a Telerik licence is the responsibility of each developer and
deployer. See the README section "Telerik setup".
EOF
```

- [ ] **Step 5: Verify no forbidden characters entered the prose**

```bash
grep -c '—' LICENSE NOTICE                      # expect 0 for NOTICE
grep -ciE 'claude|agent|assistant' NOTICE       # expect 0
```

Note: the AGPL body itself is verbatim upstream text and is never edited, including if it were to
contain punctuation the project style forbids. Only the appended exception and `NOTICE` are ours.

- [ ] **Step 6: Commit**

```bash
git add LICENSE NOTICE
git commit -m "Add AGPL-3.0 licence with linking exception, and third-party notices

The linking exception is required because Oasis links Telerik UI for
WinForms and GemBox.Document, both proprietary. The AGPL's copyleft terms
on conveying a combined work would otherwise prohibit Synovora from
distributing its own ClickOnce binaries. Section 7 is what permits an
exception of this kind to be granted.

NOTICE records which dependencies stay under their own terms. None of them
become AGPL by inclusion."
```

---

### Task 2: AGPL section 13 source offer in the patient portal

**Files:**
- Modify: `Oasis_Web/Views/Shared/_footer.vbhtml`

**Interfaces:**
- Consumes: the `LICENSE` file created in Task 1, referenced by name in the link text.
- Produces: nothing consumed by later tasks.

AGPL section 13 requires that users who interact with the program over a network be offered the
corresponding source. The patient portal is that case. The footer partial is rendered by
`Oasis_Web/Views/Shared/_Layout.vbhtml:31`, so a link there appears on every portal page.

- [ ] **Step 1: Replace the footer contents**

Current file is 15 lines. Replace it entirely with:

```vbhtml
﻿<footer class="footer">
    <div class="container-fluidr">

        <div class="row">
            <div class="col-sm-6">
                <script>document.write(new Date().getFullYear())</script> © Synovora.
            </div>
            <div class="col-sm-6">
                <div class="text-sm-right d-none d-sm-block">
                    Conçu et Réalisé par Synovora &middot;
                    <a href="https://github.com/Synovora/DSO" target="_blank" rel="noopener">
                        Code source (AGPL-3.0)
                    </a>
                </div>
            </div>
        </div>
    </div>
</footer>
```

The file starts with a UTF-8 BOM. Preserve it. Writing the file with a leading `\ufeff` keeps
Razor and Visual Studio happy with the existing accented French text.

- [ ] **Step 2: Verify the BOM and the accented text survived**

```bash
head -c 3 Oasis_Web/Views/Shared/_footer.vbhtml | xxd | head -1   # expect efbb bf
grep -c "Conçu et Réalisé" Oasis_Web/Views/Shared/_footer.vbhtml  # expect 1
grep -c "AGPL-3.0" Oasis_Web/Views/Shared/_footer.vbhtml          # expect 1
```

Expected: `efbbbf` as the first three bytes, and both greps returning 1. If the accented text greps
as 0, the file was written in the wrong encoding and must be redone.

- [ ] **Step 3: Commit**

```bash
git add Oasis_Web/Views/Shared/_footer.vbhtml
git commit -m "Offer source to portal users, as AGPL section 13 requires

Section 13 obliges us to offer corresponding source to users who interact
with the program over a network. The patient portal is exactly that case.
The footer partial renders on every portal page via _Layout.vbhtml."
```

---

### Task 3: Resolve Telerik from a developer-supplied installation

**Files:**
- Create: `Directory.Build.props`
- Create: `Telerik.props.user.example`
- Modify: `oasis/Oasis_WF.vbproj` (10 `<HintPath>` entries)
- Modify: `Oasis_Common/Oasis_Common.vbproj` (2 `<HintPath>` entries)

**Interfaces:**
- Consumes: nothing.
- Produces: MSBuild property `$(TelerikWinFormsDir)`, an absolute path to a Telerik `Bin40`
  directory containing at minimum `Telerik.WinControls.dll`. Task 4 relies on fallback step 5
  inside this file continuing to point at `lib\RCWF\2022.2.622.40.Trial`.

Both projects import `Microsoft.Common.props`, which imports `Directory.Build.props`, and both
declare `ToolsVersion="15.0"`, so the mechanism is available to them.

- [ ] **Step 1: Create Directory.Build.props**

```xml
<Project>

  <!--
    Telerik UI for WinForms resolution.

    Telerik assemblies are commercial and are not committed to this repository.
    Each developer supplies their own installation. This file locates it, trying
    each source in order and taking the first that exists:

      1. /p:TelerikWinFormsDir=...        explicit MSBuild override
      2. TELERIK_WINFORMS_DIR             environment variable
      3. Telerik.props.user               local file, gitignored
      4. the Telerik Control Panel default install location
      5. lib\RCWF\2022.2.622.40.Trial     legacy in-repo folder, if still present

    Point it at a COMPLETE Bin40 folder, not a hand-picked set of assemblies.
    The Visual Studio WinForms designer loads design-time assemblies that the
    compiler never references. A minimal copy compiles but every RadForm fails
    to open in the designer.

    See the README section "Telerik setup".
  -->

  <PropertyGroup>
    <!-- 2. environment variable -->
    <TelerikWinFormsDir Condition="'$(TelerikWinFormsDir)' == '' AND '$(TELERIK_WINFORMS_DIR)' != ''">$(TELERIK_WINFORMS_DIR)</TelerikWinFormsDir>
  </PropertyGroup>

  <!-- 3. local override file, gitignored -->
  <Import Project="$(MSBuildThisFileDirectory)Telerik.props.user"
          Condition="'$(TelerikWinFormsDir)' == '' AND Exists('$(MSBuildThisFileDirectory)Telerik.props.user')" />

  <PropertyGroup>
    <!-- 4. Telerik Control Panel default for R2 2022 -->
    <_TelerikDefaultDir>$(ProgramFiles(x86))\Progress\Telerik UI for WinForms R2 2022\Bin40</_TelerikDefaultDir>
    <TelerikWinFormsDir Condition="'$(TelerikWinFormsDir)' == '' AND Exists('$(_TelerikDefaultDir)')">$(_TelerikDefaultDir)</TelerikWinFormsDir>

    <!-- 5. legacy in-repo folder, untracked but often still on disk -->
    <_TelerikLegacyDir>$(MSBuildThisFileDirectory)lib\RCWF\2022.2.622.40.Trial</_TelerikLegacyDir>
    <TelerikWinFormsDir Condition="'$(TelerikWinFormsDir)' == '' AND Exists('$(_TelerikLegacyDir)')">$(_TelerikLegacyDir)</TelerikWinFormsDir>
  </PropertyGroup>

  <!--
    Fail early and legibly. Without this, a missing Telerik produces several
    hundred "type not found" errors originating in generated Designer files,
    none of which name the real cause.
  -->
  <Target Name="ValidateTelerik"
          BeforeTargets="CoreCompile"
          Condition="'$(MSBuildProjectName)' == 'Oasis_WF' OR '$(MSBuildProjectName)' == 'Oasis_Common'">

    <Error Condition="'$(TelerikWinFormsDir)' == ''"
           Code="OASIS001"
           Text="Telerik UI for WinForms was not found.%0A%0AProject '$(MSBuildProjectName)' needs it. Set one of the following:%0A  1. msbuild /p:TelerikWinFormsDir=&quot;C:\path\to\Bin40&quot;%0A  2. setx TELERIK_WINFORMS_DIR &quot;C:\path\to\Bin40&quot;%0A  3. copy Telerik.props.user.example to Telerik.props.user and edit it%0A  4. install via the Telerik Control Panel to the default location%0A%0AExpected version 2022.2.622.40. See the README section 'Telerik setup'." />

    <Error Condition="'$(TelerikWinFormsDir)' != '' AND !Exists('$(TelerikWinFormsDir)\Telerik.WinControls.dll')"
           Code="OASIS002"
           Text="TelerikWinFormsDir resolved to '$(TelerikWinFormsDir)' but Telerik.WinControls.dll is not there.%0A%0AThe path must point at a Telerik Bin40 folder. See the README section 'Telerik setup'." />

    <Warning Condition="'$(TelerikWinFormsDir)' != '' AND Exists('$(TelerikWinFormsDir)\Telerik.WinControls.dll') AND !Exists('$(TelerikWinFormsDir)\Telerik.WinControls.UI.Design.dll')"
             Code="OASIS003"
             Text="Telerik.WinControls.UI.Design.dll is missing from '$(TelerikWinFormsDir)'. The build will succeed but RadForm screens will not open in the Visual Studio designer. Point TelerikWinFormsDir at a complete Bin40 folder." />

    <Message Importance="normal" Text="Telerik UI for WinForms: $(TelerikWinFormsDir)" />
  </Target>

</Project>
```

- [ ] **Step 2: Create Telerik.props.user.example**

```xml
<!--
  Copy this file to Telerik.props.user (same directory) and set the path to
  your Telerik UI for WinForms installation. Telerik.props.user is gitignored.

  Use the full Bin40 folder from a Telerik Control Panel install, not a
  hand-picked copy of individual DLLs. The Visual Studio WinForms designer
  needs design-time assemblies that the compiler never references.

  Expected version: 2022.2.622.40
-->
<Project>
  <PropertyGroup>
    <TelerikWinFormsDir>C:\Program Files (x86)\Progress\Telerik UI for WinForms R2 2022\Bin40</TelerikWinFormsDir>
  </PropertyGroup>
</Project>
```

- [ ] **Step 3: Repoint the 12 HintPath entries**

```bash
cd /Users/paultheis/Documents/Code/DSO
for f in oasis/Oasis_WF.vbproj Oasis_Common/Oasis_Common.vbproj; do
  python3 - "$f" <<'PY'
import io, sys
p = sys.argv[1]
# newline='' on BOTH read and write. These files are CRLF. Without it Python
# rewrites every line ending and the diff becomes 3650 lines instead of 12.
with io.open(p, "r", encoding="utf-8-sig", newline='') as fh:
    s = fh.read()
old = r"..\lib\RCWF\2022.2.622.40.Trial" + "\\"
new = "$(TelerikWinFormsDir)" + "\\"
n = s.count(old)
s = s.replace(old, new)
with io.open(p, "w", encoding="utf-8-sig", newline='') as fh:
    fh.write(s)
print("%s: %d replaced" % (p, n))
PY
done
```

Expected output: `oasis/Oasis_WF.vbproj: 10 replaced` and `Oasis_Common/Oasis_Common.vbproj: 2 replaced`.

If either count differs, stop. The reference block was not what this plan assumed.

- [ ] **Step 4: Verify the edit**

```bash
grep -c 'RCWF' oasis/Oasis_WF.vbproj Oasis_Common/Oasis_Common.vbproj          # expect 0 and 0
grep -c 'TelerikWinFormsDir' oasis/Oasis_WF.vbproj Oasis_Common/Oasis_Common.vbproj  # expect 10 and 2
grep -o '<HintPath>\$(TelerikWinFormsDir)[^<]*' oasis/Oasis_WF.vbproj | sort
python3 -c "import xml.dom.minidom as m; [m.parse(f) for f in ['oasis/Oasis_WF.vbproj','Oasis_Common/Oasis_Common.vbproj','Directory.Build.props']]; print('all XML well-formed')"
```

Expected: zero `RCWF` matches, 10 and 2 `TelerikWinFormsDir` matches, ten distinct assembly paths
listed, and `all XML well-formed`. The XML parse is the real gate here: a malformed `.vbproj`
fails at solution load with a message that does not name the cause.

- [ ] **Step 5: Confirm the version attributes were not touched**

```bash
grep -c 'Version=2022.2.6' oasis/Oasis_WF.vbproj          # expect 10
git diff --unified=0 oasis/Oasis_WF.vbproj Oasis_Common/Oasis_Common.vbproj \
  | grep -E '^[+-]' | grep -v '^[+-][+-]' | grep -vc 'HintPath'   # expect 0
file oasis/Oasis_WF.vbproj                                # expect "with CRLF line terminators"
```

Expected: 10 references, zero non-HintPath changed lines, and CRLF preserved.

Note the version split: 8 assemblies are `2022.2.622.40` (Telerik UI for WinForms) and 2 are
`2022.2.613.40` (Telerik Document Processing, which ships on its own version stream). Grepping for
`2022.2.622.40` alone returns 8, not 10, which looks like a failed edit and is not one.

The non-HintPath line count is the check that matters. If it is not 0, line endings were mangled;
revert with `git checkout HEAD -- <files>` and redo with `newline=''`.

- [ ] **Step 6: Commit**

```bash
git add Directory.Build.props Telerik.props.user.example oasis/Oasis_WF.vbproj Oasis_Common/Oasis_Common.vbproj
git commit -m "Resolve Telerik from a developer-supplied install

Telerik assemblies are commercial and cannot be redistributed, so the 12
HintPath entries in Oasis_WF and Oasis_Common now point at a
\$(TelerikWinFormsDir) property instead of lib/RCWF.

Directory.Build.props resolves that property from, in order: an MSBuild
override, TELERIK_WINFORMS_DIR, a gitignored Telerik.props.user, the Control
Panel default location, and finally the legacy in-repo folder. The last step
keeps existing checkouts building with no action required.

A ValidateTelerik target fails with one readable message when nothing
resolves, instead of several hundred type errors from generated Designer
files.

Not yet verified against a real build. There is no MSBuild on the machine
this was written on. See the README for the verification steps."
```

---

### Task 4: Stop tracking the Telerik binaries

**Files:**
- Modify: `.gitignore`

**Interfaces:**
- Consumes: fallback step 5 of `Directory.Build.props` from Task 3, which is what keeps the local
  build working after the files are untracked.
- Produces: nothing consumed by later tasks.

Files stay on disk. See the deviation note at the top of this plan.

- [ ] **Step 1: Replace the Telerik block in .gitignore**

Find this block:

```
# --- Vendored Telerik binaries -------------------------------------------
# Only the versions referenced by a .vbproj are tracked:
#   lib/RCWF/2022.2.622.40.Trial  (Oasis_WF)
#   lib/RCWF/2022.1.222.40.Trial
# Every other drop is ignored. See README "Telerik UI for WinForms".
/lib/RCWF/*
!/lib/RCWF/2022.2.622.40.Trial/
!/lib/RCWF/2022.1.222.40.Trial/
```

Replace it with:

```
# --- Telerik UI for WinForms ---------------------------------------------
# Telerik assemblies are commercial. Redistributing them is not permitted by
# their licence, so nothing under lib/RCWF is ever committed. Developers
# supply their own installation; see README "Telerik setup".
/lib/RCWF/
Telerik.props.user
```

- [ ] **Step 2: Untrack the binaries without deleting them**

```bash
cd /Users/paultheis/Documents/Code/DSO
git rm -r --cached lib/RCWF -q
git add .gitignore
```

- [ ] **Step 3: Verify the files survived on disk and left the index**

```bash
git ls-files lib | wc -l                              # expect 0
ls lib/RCWF/2022.2.622.40.Trial/*.dll | wc -l         # expect a non-zero count, files still present
git check-ignore -v lib/RCWF/2022.2.622.40.Trial/Telerik.WinControls.dll
du -sh lib                                            # unchanged on disk
```

Expected: zero tracked files under `lib`, the DLLs still present on disk, and `check-ignore`
naming the new `/lib/RCWF/` rule. If `ls` returns nothing, the files were deleted by mistake.
Recover them with `git checkout HEAD -- lib/RCWF` before committing.

- [ ] **Step 4: Verify no commercial binary remains tracked anywhere**

```bash
git ls-files | grep -iE 'telerik|gembox' || echo "clean: no commercial binaries tracked"
git ls-files -z | xargs -0 du -ch 2>/dev/null | tail -1    # expect roughly 76M
```

Expected: `clean: no commercial binaries tracked`, and a tracked total near 76 MB, down from 193 MB.

- [ ] **Step 5: Commit**

```bash
git commit -m "Stop tracking Telerik assemblies

Their licence does not permit redistribution, and this repository is public.
The files stay on disk, untracked, so the legacy fallback in
Directory.Build.props keeps local builds working.

Tracked content drops from roughly 193 MB to roughly 76 MB. This removes the
ongoing violation. It does not undo it: clones taken before today still
contain the binaries."
```

---

### Task 5: Document the licence and the Telerik setup

**Files:**
- Modify: `README.md`

**Interfaces:**
- Consumes: `LICENSE` and `NOTICE` from Task 1, `$(TelerikWinFormsDir)` and the OASIS001 to
  OASIS003 diagnostic codes from Task 3.
- Produces: nothing consumed by later tasks.

- [ ] **Step 1: Replace the "Telerik UI for WinForms" subsection under Requirements**

Find the subsection beginning `### Telerik UI for WinForms` and ending just before `## Local setup`.
Replace the whole subsection with:

```markdown
### Telerik setup

The desktop client is built on Telerik UI for WinForms. Those assemblies are commercial and are
not distributed with this repository, so you supply your own copy. The build expects version
**2022.2.622.40** (Telerik R2 2022).

Get a licence from [Telerik](https://www.telerik.com/products/winforms.aspx), install through the
Telerik Control Panel, then point the build at the install. Any one of these works, and the build
takes the first it finds:

| | How | When to use it |
|---|---|---|
| 1 | `msbuild /p:TelerikWinFormsDir="C:\path\to\Bin40"` | One-off or scripted builds |
| 2 | `setx TELERIK_WINFORMS_DIR "C:\path\to\Bin40"` | Normal developer setup, and CI agents |
| 3 | Copy `Telerik.props.user.example` to `Telerik.props.user` and edit the path | When you want the setting to live with the checkout. The file is gitignored |
| 4 | Nothing | The Control Panel default path is found automatically: `C:\Program Files (x86)\Progress\Telerik UI for WinForms R2 2022\Bin40` |

Point it at a **complete `Bin40` folder**, not a hand-picked copy of the referenced DLLs. The
Visual Studio WinForms designer loads design-time assemblies that the compiler never references.
A minimal copy compiles fine while every `RadForm` fails to open in the designer, which is a
frustrating failure to diagnose.

Ten assemblies are referenced, by `Oasis_WF` (all ten) and `Oasis_Common` (the first two):

```
Telerik.WinControls.dll                          TelerikCommon.dll
Telerik.WinControls.UI.dll                       Telerik.WinControls.GridView.dll
Telerik.WinControls.ChartView.dll                Telerik.WinControls.Scheduler.dll
Telerik.WinControls.RichTextEditor.dll           Telerik.Windows.Documents.Core.dll
Telerik.WinControls.Themes.Office2007Silver.dll  Telerik.Windows.Zip.dll
```

If Telerik cannot be found the build stops with one message rather than hundreds of type errors:

| Code | Meaning |
|---|---|
| `OASIS001` | No installation found by any of the four methods above |
| `OASIS002` | The path resolved but `Telerik.WinControls.dll` is not in it. It is probably not a `Bin40` folder |
| `OASIS003` | Warning only. Runtime assemblies found but design-time ones missing, so builds work and designers do not |

Resolution lives in `Directory.Build.props` at the repository root.

> **Not yet verified against a real build.** The Telerik indirection was written on a machine
> without MSBuild. Someone should complete the checklist in
> `docs/specs/2026-08-23-agpl-relicensing-and-telerik-externalisation.md` on Windows before this
> is relied on.
```

- [ ] **Step 2: Replace the Licence section at the end of the README**

Find the final section beginning `## Licence` and replace it with:

```markdown
## Licence

Oasis is free software licensed under the
[GNU Affero General Public License v3.0](LICENSE), with an additional permission under section 7
allowing it to be linked with Telerik UI for WinForms, Telerik Document Processing and
GemBox.Document. Without that permission the AGPL would forbid distributing Oasis binaries at all,
since those libraries are proprietary.

If you run a modified Oasis as a network service, AGPL section 13 obliges you to offer your users
the corresponding source.

Third-party components keep their own licences and do not become AGPL by being used here. See
[NOTICE](NOTICE).

Copyright © 2026 Synovora.

### Healthcare disclaimer

Oasis is not a certified medical device and carries no warranty of any kind, express or implied,
including fitness for a particular purpose. It is not a substitute for professional clinical
judgement.

Anyone deploying Oasis is responsible for validating it for their intended use and for meeting the
regulatory, data-protection and patient-safety obligations of their jurisdiction. In the European
Union that may include the Medical Device Regulation and the GDPR; in France it may additionally
include HDS certification for hosting personal health data. Those assessments are the deployer's
responsibility, not the authors'.
```

- [ ] **Step 3: Verify the README**

```bash
grep -c '—' README.md                      # expect 0
grep -c 'Proprietary. © Synovora' README.md # expect 0, the old licence line is gone
grep -c 'AGPL-3.0' README.md               # expect at least 2
grep -c 'OASIS001' README.md               # expect 1
grep -c 'Telerik setup' README.md          # expect at least 2, the heading and the ToC or a cross-reference
awk '/^```/{n++} END{print "fenced blocks:", n, "(must be even)"}' README.md
```

Expected: no em dashes, no trace of the old proprietary line, and an even number of code fences.
An odd count means a fence was left unclosed and the rest of the document renders as code.

- [ ] **Step 4: Commit**

```bash
git add README.md
git commit -m "Document the AGPL licence and the Telerik setup

Replaces the proprietary licence line with AGPL-3.0 plus the linking
exception, and adds a healthcare disclaimer covering medical device and data
protection obligations.

The Telerik section now explains the four ways to point the build at an
installation, why it has to be a complete Bin40 folder rather than the ten
referenced DLLs, and what the OASIS001 to OASIS003 diagnostics mean."
```

---

### Task 6: Verify on Windows

**Files:** none. This task changes nothing and exists so the plan is not marked complete on
unverified work.

**Interfaces:**
- Consumes: everything from Tasks 1 to 5.
- Produces: a verified build, or a defect list.

This task **cannot be executed on the machine where the rest of this plan runs.** It needs Windows
with Visual Studio 2022 and a licensed Telerik install. Hand it to someone who has both.

- [ ] **Step 1: Build with the legacy folder still present**

```powershell
nuget restore Oasis_WF.sln
msbuild Oasis_WF.sln /p:Configuration=Debug
```

Expected: success, via fallback step 5. This proves existing checkouts are not broken.

- [ ] **Step 2: Confirm the resolved path is reported**

```powershell
msbuild oasis\Oasis_WF.vbproj /p:Configuration=Debug /v:normal | Select-String "Telerik UI for WinForms:"
```

Expected: one line naming the resolved directory.

- [ ] **Step 3: Force the failure path**

```powershell
Rename-Item lib\RCWF\2022.2.622.40.Trial 2022.2.622.40.Trial.off
msbuild oasis\Oasis_WF.vbproj /p:Configuration=Debug
```

Expected: build fails with `OASIS001` and the four numbered remedies. It must **not** fail with
type errors from `.Designer.vb` files. If it does, `ValidateTelerik` is running too late and needs
an earlier `BeforeTargets`.

- [ ] **Step 4: Build against a real Telerik install**

```powershell
setx TELERIK_WINFORMS_DIR "C:\Program Files (x86)\Progress\Telerik UI for WinForms R2 2022\Bin40"
# open a new shell so the variable is visible
msbuild Oasis_WF.sln /p:Configuration=Debug
```

Expected: success, with no `OASIS003` warning. An `OASIS003` warning means the path is not a
complete `Bin40`.

- [ ] **Step 5: Open a form in the designer**

Open `oasis/Form/Synthese/RadFSynthese.vb` in the Visual Studio WinForms designer.

Expected: the form renders. This is the only step that proves the design-time assemblies resolved.
Compilation succeeding does not prove it.

- [ ] **Step 6: Run the tests**

```powershell
vstest.console.exe UnitTest\bin\Debug\UnitTest.dll
```

Expected: the same results as before this plan. Nothing here touches `Oasis_Common` logic, so a
change in results means something unintended happened.

- [ ] **Step 7: Restore the renamed folder**

```powershell
Rename-Item lib\RCWF\2022.2.622.40.Trial.off 2022.2.622.40.Trial
```

- [ ] **Step 8: Remove the unverified warning from the README**

Only once steps 1 to 6 all pass. Delete the blockquote added in Task 5 Step 1 that begins
**Not yet verified against a real build**, then:

```bash
git add README.md
git commit -m "Confirm the Telerik resolution builds on Windows

Verified: fallback to the legacy folder, OASIS001 on a missing install,
a clean build against a Control Panel install, RadFSynthese opening in the
designer, and an unchanged test run."
```

---

## Notes for whoever executes this

**Nothing here has automated tests, and that is not an oversight.** The changes are licence text,
MSBuild configuration and documentation. There is no unit-testable behaviour. The verification
steps in each task are file assertions and XML well-formedness checks, which are the real gates
available on macOS. Task 6 is where the actual behaviour gets proven, and it needs Windows.

**Task order matters in one place.** Task 3 must land before Task 4. If the binaries are untracked
before the props file exists, nothing resolves them.

**Do not push until Task 5 is committed.** Tasks 1 to 4 leave the repository in a state where the
README still says the project is proprietary while `LICENSE` says AGPL. That contradiction should
never be the visible state of a public repository, even briefly.
