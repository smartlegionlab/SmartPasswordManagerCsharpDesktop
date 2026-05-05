# Migration Guide: v1.x.x to v4.0.0

> **📌 Version Note:** SmartPasswordManagerCsharpDesktop jumps from v1.x.x directly to v4.0.0 to align with smartpasslib-csharp v4.0.0. All smartpasslib implementations (Python, C#, JS, Go, Kotlin) now share the same version number and algorithm.

## ⚠️ Breaking Change Notice

**SmartPasswordManagerCsharpDesktop v4.0.0 is NOT backward compatible with v1.x.x**

| Version    | Status      | Why                                                              |
|------------|-------------|------------------------------------------------------------------|
| v1.x.x     | Deprecated  | Fixed iterations (30/60), limited character set                  |
| **v4.0.0** | **Current** | Dynamic iterations (15-30/45-60), expanded charset, max security |

Smart passwords generated with v1.x.x cannot be regenerated using v4.0.0 due to fundamental changes in the deterministic generation algorithm.

---

## Why the change?

**v4.0.0 introduces fundamental improvements:**

- **Dynamic iteration counts** — deterministic steps vary per secret (15-30 for private, 45-60 for public)
- **Expanded character set** — Google-compatible symbols
- **Enhanced key derivation** — salt separation for public/private keys ("private"/"public")
- **Unified length validation** — password length must be 12-100 characters (was 12-1000)
- **Input validation** — secret phrases must be at least 12 characters (enforced)
- **Maximum security** — no secret exposure in logs or iterations

---

## What changed in the Desktop Manager:

| Aspect                 | v1.x.x             | v4.0.0                                |
|------------------------|--------------------|---------------------------------------|
| Private key iterations | Fixed 30           | Dynamic 15-30                         |
| Public key iterations  | Fixed 60           | Dynamic 45-60                         |
| Character set          | `abc...!@#$&*-_`   | `!@#$%^&*()_+-=[]{};:,.<>?/A-Za-z0-9` |
| Password max length    | 1000               | 100                                   |
| Secret validation      | None (min 4 chars) | Min 12 characters (enforced)          |
| Key derivation salt    | None               | "private"/"public"                    |
| Secret in iterations   | Yes (exposed)      | No (secure)                           |

---

## Metadata File Compatibility

**The old `passwords.json` file is NOT compatible with v4.0.0**

Public keys stored in v1.x.x files cannot be used with v4.0.0 because:
- Iteration counts changed from fixed 60 to dynamic 45-60
- Salt "public" was added to key derivation

**Result:** Old entries will load but secret verification will fail. Passwords cannot be regenerated.

---

## Migration Steps

### Step 1: Retrieve existing passwords using old version

Before upgrading, retrieve all actual passwords from v1.x.x:
- Open the old version of the application
- For each entry, click **Get** and copy the password
- Save all retrieved passwords in a safe place

### Step 2: Backup old metadata file

The old metadata file is located at `%USERPROFILE%\.config\smart_password_manager\passwords.json`

Copy this file to a safe location (e.g., `passwords.json.v1.bak`).

### Step 3: Upgrade to v4.0.0

Download the new binary or build from source. Replace the old executable.

### Step 4: Remove old metadata file

The old metadata file must be removed or moved away from the default location. v4.0.0 will create a new empty file on first run.

### Step 5: Re-add entries

Launch the application and add all entries again using the **same secret phrases and lengths** as before.

### Step 6: Update services

Replace old passwords (from Step 1) with newly generated ones on each website/service.

### Step 7: Verify

Log in using new passwords. Confirm regeneration works (same secret → same password).

---

## Important Notes

- **No automatic migration** — manual password regeneration required
- **No database migration** — old metadata file is incompatible
- **Your secret phrases remain the same** — use them to recreate entries
- **Secret phrases shorter than 12 characters will now be rejected**
- **Password lengths between 101 and 1000 will now be rejected**
- **Old passwords still work** on services until you change them
- Test with non-essential accounts first

---

## Rollback

If you need to rollback to v1.x.x, use the old binary and restore your backup metadata file.

---

## Need Help?

- **Issues**: [GitHub Issues](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/issues)
- **Core Library Issues**: [smartpasslib Issues](https://github.com/smartlegionlab/smartpasslib/issues)

---

