/**
 * SmartPassLib v4.0.0 - C# smart password generator
 * Cross-platform deterministic password generation
 * Same secret + same length = same password across all platforms
 * Decentralized by design — no central servers, no cloud dependency, no third-party trust required
 *
 * Compatible with smartpasslib Python/JS/Kotlin/Go implementations
 *
 * Key derivation:
 * - Private key: 15-30 iterations (dynamic, deterministic per secret)
 * - Public key: 45-60 iterations (dynamic, deterministic per secret)
 *
 * Secret phrase:
 *   - is not transferred anywhere
 *   - is not stored anywhere
 *   - is required to generate the private key when creating a smart password
 *   - minimum 12 characters (enforced)
 *
 * Password length:
 *   - minimum 12 characters (enforced)
 *   - maximum 100 characters (enforced)
 *
 * Ecosystem:
 *   - Core library (Python): https://github.com/smartlegionlab/smartpasslib
 *   - Core library (JS): https://github.com/smartlegionlab/smartpasslib-js
 *   - Core library (Kotlin): https://github.com/smartlegionlab/smartpasslib-kotlin
 *   - Core library (Go): https://github.com/smartlegionlab/smartpasslib-go
 *   - Core library (C#): https://github.com/smartlegionlab/smartpasslib-csharp
 *   - Desktop Python: https://github.com/smartlegionlab/smart-password-manager-desktop
 *   - Desktop C#: https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop
 *   - CLI Manager Python: https://github.com/smartlegionlab/clipassman
 *   - CLI Manager C#: https://github.com/smartlegionlab/SmartPasswordManagerCsharpCli
 *   - CLI Generator Python: https://github.com/smartlegionlab/clipassgen
 *   - CLI Generator C#: https://github.com/smartlegionlab/SmartPasswordGeneratorCsharpCli
 *   - Web: https://github.com/smartlegionlab/smart-password-manager-web
 *   - Android: https://github.com/smartlegionlab/smart-password-manager-android
 *
 * Author: Alexander Suvorov https://github.com/smartlegionlab
 * License: BSD 3-Clause https://github.com/smartlegionlab/smartpasslib-csharp/blob/master/LICENSE
 * Copyright (©) 2026, Alexander Suvorov. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SmartLegionLab.SmartPassLib
{
    // ============================================================
    // Core password generation
    // ============================================================

    /// <summary>
    /// SmartPassLib - Deterministic smart password generator
    /// </summary>
    public static class SmartPasswordGenerator
    {
        /// <summary>
        /// Library version
        /// </summary>
        public const string Version = "4.0.0";

        /// <summary>
        /// Character set for password generation (Google-compatible)
        /// Must match exactly with Python version: symbols + uppercase + digits + lowercase
        /// </summary>
        public const string Chars = "!@#$%^&*()_+-=[]{};:,.<>?/ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

        private static readonly RandomNumberGenerator SecureRandom = RandomNumberGenerator.Create();

        private static string Sha256(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        private static void ValidateSecret(string secret)
        {
            if (string.IsNullOrEmpty(secret) || secret.Length < 12)
                throw new ArgumentException($"Secret phrase must be at least 12 characters. Current: {secret?.Length ?? 0}");
        }

        private static void ValidatePasswordLength(int length)
        {
            if (length < 12)
                throw new ArgumentException($"Password length must be at least 12 characters. Current: {length}");
            if (length > 100)
                throw new ArgumentException($"Password length cannot exceed 100 characters. Current: {length}");
        }

        private static int GetStepsFromSecret(string secret, int minSteps, int maxSteps, string salt)
        {
            var hashValue = Sha256($"{secret}:{salt}");
            var hashInt = Convert.ToInt64(hashValue.Substring(0, 8), 16);
            var steps = minSteps + (int)(hashInt % (maxSteps - minSteps + 1));
            return steps;
        }

        private static string GenerateKey(string secret, int steps, string salt)
        {
            ValidateSecret(secret);

            var allHash = Sha256($"{secret}:{salt}");
            for (var i = 0; i < steps; i++)
            {
                var tempString = $"{allHash}:{i}";
                allHash = Sha256(tempString);
            }
            return allHash;
        }

        /// <summary>
        /// Generate private key (15-30 deterministic iterations) - for password generation, never stored
        /// </summary>
        public static string GeneratePrivateKey(string secret)
        {
            ValidateSecret(secret);
            var steps = GetStepsFromSecret(secret, 15, 30, "private");
            return GenerateKey(secret, steps, "private");
        }

        /// <summary>
        /// Generate public key (45-60 deterministic iterations) - for verification, stored on server
        /// </summary>
        public static string GeneratePublicKey(string secret)
        {
            ValidateSecret(secret);
            var steps = GetStepsFromSecret(secret, 45, 60, "public");
            return GenerateKey(secret, steps, "public");
        }

        /// <summary>
        /// Verify secret against public key
        /// </summary>
        public static bool VerifySecret(string secret, string publicKey) => GeneratePublicKey(secret) == publicKey;

        private static byte[] HexToBytes(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static string GeneratePasswordFromPrivateKey(string privateKey, int length)
        {
            ValidatePasswordLength(length);

            var result = new StringBuilder();
            var counter = 0;

            while (result.Length < length)
            {
                var data = $"{privateKey}:{counter}";
                var hashHex = Sha256(data);
                var hashBytes = HexToBytes(hashHex);

                foreach (var b in hashBytes)
                {
                    if (result.Length >= length) break;
                    result.Append(Chars[b % Chars.Length]);
                }
                counter++;
            }
            return result.ToString();
        }

        /// <summary>
        /// Generate deterministic smart password from secret phrase
        /// </summary>
        public static string GenerateSmartPassword(string secret, int length)
        {
            ValidateSecret(secret);
            ValidatePasswordLength(length);
            return GeneratePasswordFromPrivateKey(GeneratePrivateKey(secret), length);
        }

        /// <summary>
        /// Generate cryptographically secure random password
        /// </summary>
        public static string GenerateStrongPassword(int length)
        {
            ValidatePasswordLength(length);

            var bytes = new byte[length];
            SecureRandom.GetBytes(bytes);
            return new string(bytes.Select(b => Chars[b % Chars.Length]).ToArray());
        }

        /// <summary>
        /// Generate base random password
        /// </summary>
        public static string GenerateBasePassword(int length) => GenerateStrongPassword(length);

        /// <summary>
        /// Generate authentication code for 2FA (4-100 chars)
        /// </summary>
        public static string GenerateCode(int length)
        {
            if (length < 4)
                throw new ArgumentException($"Code length must be at least 4 characters. Current: {length}");
            if (length > 100)
                throw new ArgumentException($"Code length cannot exceed 100 characters. Current: {length}");

            var bytes = new byte[length];
            SecureRandom.GetBytes(bytes);
            return new string(bytes.Select(b => Chars[b % Chars.Length]).ToArray());
        }
    }

    // ============================================================
    // Metadata container
    // ============================================================

    /// <summary>
    /// Metadata container for smart password verification and generation
    /// </summary>
    public class SmartPassword
    {
        /// <summary>Public verification key</summary>
        public string PublicKey { get; set; }

        /// <summary>Service/account description</summary>
        public string Description { get; set; }

        /// <summary>Password length to generate</summary>
        public int Length { get; set; }

        /// <summary>
        /// Create new smart password metadata
        /// </summary>
        public SmartPassword(string publicKey, string description, int length = 12)
        {
            if (length < 12)
                throw new ArgumentException($"Password length must be at least 12 characters. Current: {length}");
            if (length > 100)
                throw new ArgumentException($"Password length cannot exceed 100 characters. Current: {length}");

            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Length = length;
        }

        /// <summary>
        /// Update metadata
        /// </summary>
        public void Update(string? description = null, int? length = null)
        {
            if (description != null)
                Description = description;
            if (length.HasValue)
            {
                if (length.Value < 12)
                    throw new ArgumentException($"Password length must be at least 12 characters. Current: {length.Value}");
                if (length.Value > 100)
                    throw new ArgumentException($"Password length cannot exceed 100 characters. Current: {length.Value}");
                Length = length.Value;
            }
        }

        /// <summary>
        /// Convert to dictionary for serialization
        /// </summary>
        public Dictionary<string, object> ToDict() => new Dictionary<string, object>
        {
            ["public_key"] = PublicKey,
            ["description"] = Description,
            ["length"] = Length
        };

        /// <summary>
        /// Create instance from dictionary
        /// </summary>
        public static SmartPassword FromDict(Dictionary<string, object> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return new SmartPassword(
                publicKey: data["public_key"]?.ToString() ?? throw new ArgumentException("public_key is required"),
                description: data["description"]?.ToString() ?? throw new ArgumentException("description is required"),
                length: Convert.ToInt32(data["length"])
            );
        }
    }

    // ============================================================
    // Manager for metadata storage
    // ============================================================

    /// <summary>
    /// Manager for smart password metadata storage and operations
    /// </summary>
    public class SmartPasswordManager
    {
        private Dictionary<string, SmartPassword> _passwords;
        private readonly string _filename;

        /// <summary>
        /// Initialize manager with storage file
        /// </summary>
        public SmartPasswordManager(string? filename = null)
        {
            if (string.IsNullOrEmpty(filename))
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var configDir = Path.Combine(home, ".config", "smart_password_manager");
                Directory.CreateDirectory(configDir);
                _filename = Path.Combine(configDir, "passwords.json");
            }
            else
            {
                _filename = filename;
            }
            _passwords = LoadData();
        }

        /// <summary>All stored smart password metadata</summary>
        public IReadOnlyDictionary<string, SmartPassword> Passwords => _passwords;

        /// <summary>Number of stored password metadata entries</summary>
        public int PasswordCount => _passwords.Count;

        /// <summary>File path of configuration</summary>
        public string FilePath => _filename;

        /// <summary>Add smart password metadata to storage</summary>
        public void AddSmartPassword(SmartPassword smartPassword)
        {
            if (smartPassword == null)
                throw new ArgumentNullException(nameof(smartPassword));

            _passwords[smartPassword.PublicKey] = smartPassword;
            WriteData();
        }

        /// <summary>Retrieve smart password metadata by public key</summary>
        public SmartPassword? GetSmartPassword(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
                return null;

            _passwords.TryGetValue(publicKey, out var result);
            return result;
        }

        /// <summary>Update metadata of an existing smart password</summary>
        public bool UpdateSmartPassword(string publicKey, string? description = null, int? length = null)
        {
            if (string.IsNullOrEmpty(publicKey))
                return false;

            if (!_passwords.TryGetValue(publicKey, out var password))
                return false;

            password.Update(description, length);
            WriteData();
            return true;
        }

        /// <summary>Delete smart password metadata by public key</summary>
        public bool DeleteSmartPassword(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
                return false;

            if (_passwords.Remove(publicKey))
            {
                WriteData();
                return true;
            }
            return false;
        }

        /// <summary>Clear all stored password metadata</summary>
        public void Clear()
        {
            _passwords.Clear();
            WriteData();
        }

        private Dictionary<string, SmartPassword> LoadData()
        {
            if (!File.Exists(_filename))
                return new Dictionary<string, SmartPassword>();

            try
            {
                var json = File.ReadAllText(_filename);
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                if (data == null)
                    return new Dictionary<string, SmartPassword>();

                var result = new Dictionary<string, SmartPassword>();
                foreach (var kv in data)
                {
                    var publicKey = kv.Value.GetProperty("public_key").GetString();
                    var description = kv.Value.GetProperty("description").GetString();
                    var length = kv.Value.GetProperty("length").GetInt32();

                    if (publicKey == null || description == null)
                        continue;

                    var sp = new SmartPassword(publicKey, description, length);
                    result[kv.Key] = sp;
                }
                return result;
            }
            catch
            {
                return new Dictionary<string, SmartPassword>();
            }
        }

        private void WriteData()
        {
            var dir = Path.GetDirectoryName(_filename);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            var data = new Dictionary<string, object>();
            foreach (var kv in _passwords)
            {
                data[kv.Key] = new Dictionary<string, object>
                {
                    ["public_key"] = kv.Value.PublicKey,
                    ["description"] = kv.Value.Description,
                    ["length"] = kv.Value.Length
                };
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filename, json);
        }
    }
}