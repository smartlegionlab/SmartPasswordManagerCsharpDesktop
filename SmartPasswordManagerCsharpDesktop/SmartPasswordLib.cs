/**
 * SmartPasswordLib v1.0.3 - C# smart password generator
 * Cross-platform deterministic password generation
 * Same secret + same length = same password across all platforms
 *
 * Compatible with smartpasslib Python/JS/Kotlin/Go implementations
 *
 * Key derivation:
 * - Private key: 30 iterations of SHA-256 (used for password generation)
 * - Public key: 60 iterations of SHA-256 (used for verification, stored on server)
 *
 * Ecosystem:
 * - Core library (Python): https://github.com/smartlegionlab/smartpasslib
 * - Core library (JS): https://github.com/smartlegionlab/smartpasslib-js
 * - Core library (Kotlin): https://github.com/smartlegionlab/smartpasslib-kotlin
 * - Core library (Go): https://github.com/smartlegionlab/smartpasslib-go
 * - Core library (C#): https://github.com/smartlegionlab/smartpasslib-csharp
 * - Desktop Python: https://github.com/smartlegionlab/smart-password-manager-desktop
 * - Desktop C#: https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop
 * - CLI Manager Python: https://github.com/smartlegionlab/clipassman
 * - CLI Manager C#: https://github.com/smartlegionlab/SmartPasswordManagerCsharpCli
 * - CLI Generator Python: https://github.com/smartlegionlab/clipassgen
 * - CLI Manager C#: https://github.com/smartlegionlab/SmartPasswordGeneratorCsharpCli
 * - Web: https://github.com/smartlegionlab/smart-password-manager-web
 * - Android: https://github.com/smartlegionlab/smart-password-manager-android
 *
 * Author: Alexander Suvorov
 * License: BSD 3-Clause
 * Copyright (c) 2026, Alexander Suvorov
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SmartLegionLab.SmartPasswordLib
{
    // ============================================================
    // Core password generation
    // ============================================================

    /// <summary>
    /// SmartPasswordLib - Deterministic smart password generator
    /// </summary>
    public static class SmartPasswordGenerator
    {
        /// <summary>
        /// Library version
        /// </summary>
        public const string Version = "1.0.3";

        /// <summary>
        /// Character set for password generation
        /// </summary>
        public const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$&*-_";

        private const int PrivateIterations = 30;
        private const int PublicIterations = 60;
        private static readonly RandomNumberGenerator SecureRandom = RandomNumberGenerator.Create();

        private static string Sha256(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        private static string GenerateKey(string secret, int iterations)
        {
            if (string.IsNullOrEmpty(secret) || secret.Length < 12)
                throw new ArgumentException($"Secret phrase must be at least 12 characters. Current: {secret?.Length ?? 0}");

            var allHash = Sha256(secret);
            for (var i = 0; i < iterations; i++)
            {
                var tempString = $"{allHash}:{secret}:{i}";
                allHash = Sha256(tempString);
            }
            return allHash;
        }

        /// <summary>
        /// Generate private key (30 iterations) - for password generation, never stored
        /// </summary>
        public static string GeneratePrivateKey(string secret) => GenerateKey(secret, PrivateIterations);

        /// <summary>
        /// Generate public key (60 iterations) - for verification, stored on server
        /// </summary>
        public static string GeneratePublicKey(string secret) => GenerateKey(secret, PublicIterations);

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
            if (length < 12 || length > 1000)
                throw new ArgumentException($"Password length must be between 12 and 1000. Current: {length}");

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
            if (string.IsNullOrEmpty(secret) || secret.Length < 12)
                throw new ArgumentException($"Secret phrase must be at least 12 characters. Current: {secret?.Length ?? 0}");
            if (length < 12 || length > 1000)
                throw new ArgumentException($"Password length must be between 12 and 1000. Current: {length}");

            return GeneratePasswordFromPrivateKey(GeneratePrivateKey(secret), length);
        }

        /// <summary>
        /// Generate cryptographically secure random password
        /// </summary>
        public static string GenerateStrongPassword(int length)
        {
            if (length < 12 || length > 1000)
                throw new ArgumentException($"Password length must be between 12 and 1000. Current: {length}");

            var bytes = new byte[length];
            SecureRandom.GetBytes(bytes);
            return new string(bytes.Select(b => Chars[b % Chars.Length]).ToArray());
        }

        /// <summary>
        /// Generate base random password
        /// </summary>
        public static string GenerateBasePassword(int length) => GenerateStrongPassword(length);

        /// <summary>
        /// Generate authentication code for 2FA (4-20 chars)
        /// </summary>
        public static string GenerateCode(int length)
        {
            if (length < 4 || length > 20)
                throw new ArgumentException($"Code length must be between 4 and 20. Current: {length}");

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
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Length = length;
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

            if (description != null) password.Description = description;
            if (length.HasValue) password.Length = length.Value;
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