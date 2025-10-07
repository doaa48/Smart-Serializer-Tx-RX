using System;
using System.Security.Cryptography;
using System.Text;

namespace WaterMeter_id
{
	// Token: 0x02000003 RID: 3
	public class CryptoEngine
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002098 File Offset: 0x00000298
		public static string Encrypt(string input, string key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(key);
			tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
			tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
			ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
			byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
			tripleDESCryptoServiceProvider.Clear();
			return Convert.ToBase64String(array, 0, array.Length);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002104 File Offset: 0x00000304
		public static string Decrypt(string input, string key)
		{
			byte[] array = Convert.FromBase64String(input);
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(key);
			tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
			tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
			ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
			byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			tripleDESCryptoServiceProvider.Clear();
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
