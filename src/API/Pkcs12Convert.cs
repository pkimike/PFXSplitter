using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Pkcs12Converter.API;

class Pkcs12Convert {
    public static Boolean ToCertAndKey(String pfxFilePath, String? unlockPassword, String certFilePath, String keyFilePath, String? keyPassword, out String error) {
        try {
            error = String.Empty;
            using var bundle = new X509Certificate2(pfxFilePath, unlockPassword, X509KeyStorageFlags.Exportable);
            AsymmetricAlgorithm key = (AsymmetricAlgorithm)bundle.GetRSAPrivateKey() ??
                                      bundle.GetECDsaPrivateKey() ??
                                      throw new ArgumentException("Could not load the private key");

            Byte[] publicKeyBytes = key.ExportSubjectPublicKeyInfo();
            Byte[] privateKeyBytes = keyPassword == null
                ? key.ExportPkcs8PrivateKey()
                : key.ExportEncryptedPkcs8PrivateKey(keyPassword,
                    new PbeParameters(
                        PbeEncryptionAlgorithm.Aes256Cbc,
                        HashAlgorithmName.SHA256,
                        iterationCount: 100));

            String encodedCert = new(PemEncoding.Write("PUBLIC KEY", publicKeyBytes));
            File.WriteAllText(certFilePath, encodedCert);
            String encodedKey = new(PemEncoding.Write("PRIVATE KEY", privateKeyBytes));
            File.WriteAllText(keyFilePath, encodedKey);
            return true;
        } catch (Exception ex) {
            error = $"An exception occurred: '{ex.Message}'\r\n\r\nStack Trace:\r\n{ex.StackTrace}";

            return false;
        }
    }

    public static Boolean ToPkcs12(String certFilePath, String keyFilePath, String? password, String pkcs12FilePath, out String error) {
        try {
            error = String.Empty;
            Byte[] publicKeyBytes = File.ReadAllBytes(certFilePath);
            using var cert = new X509Certificate2(publicKeyBytes);
            String[] encodedPrivateKey = File.ReadAllText(keyFilePath)
                .Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            Byte[] privateKeyBytes = Convert.FromBase64String(encodedPrivateKey[1]);
            using var rsa = RSA.Create();
            switch (encodedPrivateKey[0]) {
                case "BEGIN PRIVATE KEY":
                    rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                    break;
                case "BEGIN RSA PRIVATE KEY":
                    rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                    break;
                default:
                    throw new ArgumentException("Unsupported private key encoding");
            }

            X509Certificate2 bundle = cert.CopyWithPrivateKey(rsa);
            String encodedPfx = Convert.ToBase64String(bundle.Export(X509ContentType.Pkcs12, password));
            File.WriteAllText(pkcs12FilePath, encodedPfx);

            return true;
        } catch (Exception ex) {
            error = $"An exception occurred: '{ex.Message}'\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
                
            return false;
        }
    }
}