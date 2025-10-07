using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using UnifyWaterCard.Security;

namespace WaterMeter_id
{
    public class Signer

    {
        public static byte[] Sign(string privateKeyHex, byte[] data)
        {
            AsymmetricKeyParameter eCPrivateKeyFromHex = GetECPrivateKeyFromHex(privateKeyHex);
            ISigner signer = SignerUtilities.GetSigner(HashType.SHA256withECDSA.ToString());
            signer.Init(forSigning: true, eCPrivateKeyFromHex);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        public static bool ValidateSignature(byte[] data, byte[] signature, string publicKeyHex)
        {
            AsymmetricKeyParameter eCPublicKeyFromHex = GetECPublicKeyFromHex(publicKeyHex);
            ISigner signer = SignerUtilities.GetSigner(HashType.SHA256withECDSA.ToString());
            signer.Init(forSigning: false, eCPublicKeyFromHex);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.VerifySignature(signature);
        }

        public static AsymmetricKeyParameter GetPublicKeyFromPrivateKey(EncryptionAlgorithms algorithm, AsymmetricKeyParameter privateKey)
        {
            X9ECParameters byName = SecNamedCurves.GetByName(GetAlgorithmName(algorithm));
            ECDomainParameters eCDomainParameters = new ECDomainParameters(byName.Curve, byName.G, byName.N, byName.H);
            byte[] bytes = ((ECPrivateKeyParameters)privateKey).D.ToByteArrayUnsigned();
            BigInteger b = new BigInteger(1, bytes);
            Org.BouncyCastle.Math.EC.ECPoint q = eCDomainParameters.G.Multiply(b);
            return new ECPublicKeyParameters(q, eCDomainParameters);
        }

        public static string ComputeSha256Hash(byte[] rawData)
        {
            using (SHA256 sHA = SHA256.Create())
            {
                byte[] array = sHA.ComputeHash(rawData);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        public static AsymmetricKeyParameter GeneratePrivateKey(EncryptionAlgorithms algorithm)
        {
            X9ECParameters byName = ECNamedCurveTable.GetByName(GetAlgorithmName(algorithm));
            ECDomainParameters domainParameters = new ECDomainParameters(byName.Curve, byName.G, byName.N, byName.H, byName.GetSeed());
            IAsymmetricCipherKeyPairGenerator keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("ECDSA");
            keyPairGenerator.Init(new ECKeyGenerationParameters(domainParameters, new SecureRandom()));
            AsymmetricCipherKeyPair asymmetricCipherKeyPair = keyPairGenerator.GenerateKeyPair();
            return (ECPrivateKeyParameters)asymmetricCipherKeyPair.Private;
        }

        public static KeyPair GenerateKeyPairs()
        {
            AsymmetricKeyParameter asymmetricKeyParameter = GeneratePrivateKey(EncryptionAlgorithms.ECDSA_secp256r1);
            AsymmetricKeyParameter publicKeyFromPrivateKey = GetPublicKeyFromPrivateKey(EncryptionAlgorithms.ECDSA_secp256r1, asymmetricKeyParameter);
            string privateKey = GeneralUtility.ByteArrayToHex(((ECPrivateKeyParameters)asymmetricKeyParameter).D.ToByteArray());
            string text = ((ECPublicKeyParameters)publicKeyFromPrivateKey).Q.XCoord.ToString();
            string text2 = ((ECPublicKeyParameters)publicKeyFromPrivateKey).Q.YCoord.ToString();
            while (text.Length < 32)
            {
                text = "0" + text;
            }

            while (text2.Length < 32)
            {
                text2 = "0" + text2;
            }

            return new KeyPair
            {
                PrivateKey = privateKey,
                PublicKey = text + text2
            };
        }

        public static string CertifyPublicKey(string publicKeyHex, string byPrivatKeyHex)
        {
            ECPrivateKeyParameters eCPrivateKeyFromHex = GetECPrivateKeyFromHex(byPrivatKeyHex);
            ECPublicKeyParameters eCPublicKeyFromHex = GetECPublicKeyFromHex(publicKeyHex);
            X509V3CertificateGenerator x509V3CertificateGenerator = GenerateCertificateGenerator(eCPublicKeyFromHex);
            return GeneralUtility.ByteArrayToHex(x509V3CertificateGenerator.Generate(eCPrivateKeyFromHex).GetEncoded());
        }

        public static string CertifyWithExtension(string byPrivatKeyHex, string publicKeyHex, params uint[] ids)
        {
            try
            {
                ECPrivateKeyParameters eCPrivateKeyFromHex = GetECPrivateKeyFromHex(byPrivatKeyHex);
                ECPublicKeyParameters eCPublicKeyFromHex = GetECPublicKeyFromHex(publicKeyHex);
                X509V3CertificateGenerator x509V3CertificateGenerator = GenerateCertificateGenerator(eCPublicKeyFromHex);
                List<byte> extensionValue = new List<byte>();
                ids.ToList().ForEach(delegate (uint id)
                {
                    extensionValue.AddRange(BitConverter.GetBytes(id));
                });
                x509V3CertificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage.Id, critical: false, extensionValue.ToArray());
                return GeneralUtility.ByteArrayToHex(x509V3CertificateGenerator.Generate(eCPrivateKeyFromHex).GetEncoded());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool ValidateCertificate(string certificateHex, string publicKeyHex)
        {
            try
            {
                X509Certificate x509Certificate = new X509Certificate(GeneralUtility.HexToByteArray(certificateHex));
                x509Certificate.Verify(GetECPublicKeyFromHex(publicKeyHex));
                return x509Certificate.IsValid(DateTime.UtcNow);
            }
            catch
            {
                return false;
            }
        }

        public static string ExtractPublicKeyFromCertificate(byte[] certificate, out byte[] extensionValue)
        {
            X509Certificate x509Certificate = new X509Certificate(certificate);
            extensionValue = x509Certificate.GetExtensionValue(X509Extensions.ExtendedKeyUsage.Id).GetEncoded();
            extensionValue = Tlv.ParseTlv(Tlv.ParseTlv(extensionValue).First().Value).First().Value;
            AsymmetricKeyParameter publicKey = x509Certificate.GetPublicKey();
            return ((ECPublicKeyParameters)publicKey).Q.XCoord.ToString() + ((ECPublicKeyParameters)publicKey).Q.YCoord.ToString();
        }

        public static string ExtractPublicKeyFromCertificate(byte[] certificate)
        {
            try
            {
                AsymmetricKeyParameter publicKey = new X509Certificate(certificate).GetPublicKey();
                return ((ECPublicKeyParameters)publicKey).Q.XCoord.ToString() + ((ECPublicKeyParameters)publicKey).Q.YCoord.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string ExtractTargetNameFromCertificate(byte[] certificate)
        {
            try
            {
                X509CertificateParser x509CertificateParser = new X509CertificateParser();
                X509Certificate x509Certificate = x509CertificateParser.ReadCertificate(certificate);
                X509CertificateStructure instance = X509CertificateStructure.GetInstance(x509Certificate.GetEncoded());
                X509Name subject = instance.Subject;
                string result = null;
                foreach (DerObjectIdentifier oid in subject.GetOidList())
                {
                    if (oid.Equals(X509Name.CN))
                    {
                        result = subject.GetValueList(oid).Cast<string>().FirstOrDefault();
                        break;
                    }
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        private static X509V3CertificateGenerator GenerateCertificateGenerator(AsymmetricKeyParameter publicKey)
        {
            X509V3CertificateGenerator x509V3CertificateGenerator = new X509V3CertificateGenerator();
            x509V3CertificateGenerator.SetSerialNumber(BigInteger.ProbablePrime(120, new Random()));
            x509V3CertificateGenerator.SetSubjectDN(new X509Name("CN=HCWW"));
            x509V3CertificateGenerator.SetIssuerDN(new X509Name("CN=HCWW"));
            x509V3CertificateGenerator.SetNotBefore(DateTime.UtcNow);
            x509V3CertificateGenerator.SetNotAfter(DateTime.UtcNow.Add(new TimeSpan(365, 0, 0, 0)));
            x509V3CertificateGenerator.SetSignatureAlgorithm(HashType.SHA256withECDSA.ToString());
            x509V3CertificateGenerator.SetPublicKey(publicKey);
            return x509V3CertificateGenerator;
        }

        private static string GetAlgorithmName(EncryptionAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case EncryptionAlgorithms.ECDSA_secp128r1:
                    return "secp128r1";
                case EncryptionAlgorithms.ECDSA_secp192r1:
                    return "secp192r1";
                case EncryptionAlgorithms.ECDSA_secp256r1:
                    return "secp256r1";
                default:
                    return null;
            }
        }

        private static ECPrivateKeyParameters GetECPrivateKeyFromHex(string privateKeyHex)
        {
            X9ECParameters byName = SecNamedCurves.GetByName(GetAlgorithmName(EncryptionAlgorithms.ECDSA_secp256r1));
            ECDomainParameters parameters = new ECDomainParameters(byName.Curve, byName.G, byName.N, byName.H);
            BigInteger d = new BigInteger(1, GeneralUtility.HexToByteArray(privateKeyHex));
            return new ECPrivateKeyParameters(d, parameters);
        }

        private static ECPublicKeyParameters GetECPublicKeyFromHex(string publicKeyHex)
        {
            try
            {
                if (publicKeyHex.Length == 127)
                {
                    publicKeyHex = "0" + publicKeyHex;
                }

                X9ECParameters byName = SecNamedCurves.GetByName(GetAlgorithmName(EncryptionAlgorithms.ECDSA_secp256r1));
                ECFieldElement x = new FpFieldElement(((FpCurve)byName.Curve).Q, new BigInteger(1, GeneralUtility.HexToByteArray(publicKeyHex.Substring(0, 64))));
                ECFieldElement y = new FpFieldElement(((FpCurve)byName.Curve).Q, new BigInteger(1, GeneralUtility.HexToByteArray(publicKeyHex.Substring(64, 64))));
                Org.BouncyCastle.Math.EC.ECPoint q = new FpPoint(byName.Curve, x, y);
                return new ECPublicKeyParameters("ECDH", q, SecObjectIdentifiers.SecP256r1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GenerateX509Certificate(string IssuerEntityName, string IssuerPrivateKey, string TargetEntityName, string TargetPublicKey, byte[] extensionsData = null)
        {
            try
            {
                ECPrivateKeyParameters eCPrivateKeyFromHex = GetECPrivateKeyFromHex(IssuerPrivateKey);
                ECPublicKeyParameters eCPublicKeyFromHex = GetECPublicKeyFromHex(TargetPublicKey);
                X509V3CertificateGenerator x509V3CertificateGenerator = new X509V3CertificateGenerator();
                x509V3CertificateGenerator.SetSerialNumber(BigInteger.ProbablePrime(120, new Random()));
                x509V3CertificateGenerator.SetSubjectDN(new X509Name("CN=" + TargetEntityName));
                x509V3CertificateGenerator.SetIssuerDN(new X509Name("CN=" + IssuerEntityName));
                x509V3CertificateGenerator.SetNotBefore(DateTime.Now);
                x509V3CertificateGenerator.SetNotAfter(DateTime.Now.Add(new TimeSpan(365, 0, 0, 0)));
                if (extensionsData != null && extensionsData.Length != 0)
                {
                    x509V3CertificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage.Id, critical: true, extensionsData);
                }

                x509V3CertificateGenerator.SetSignatureAlgorithm(HashType.SHA256withECDSA.ToString());
                x509V3CertificateGenerator.SetPublicKey(eCPublicKeyFromHex);
                X509Certificate x509Certificate = x509V3CertificateGenerator.Generate(eCPrivateKeyFromHex);
                return ByteArrayToHex(x509Certificate.GetEncoded());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static bool VerifyX509Certificate(string TargetEntityCertification, string IssuerPublicKey)
        {
            try
            {
                X509Certificate x509Certificate = new X509CertificateParser().ReadCertificate(HexToByteArray(TargetEntityCertification));
                x509Certificate.Verify(GetECPublicKeyFromHex(IssuerPublicKey));
                return x509Certificate.IsValid(DateTime.Now);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string ByteArrayToHex(byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }

        private static byte[] HexToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] array = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return array;
        }
    }
}
