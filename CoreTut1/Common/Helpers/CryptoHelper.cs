using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers
{
	public class CryptoHelper
	{
		private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
		private static string sharedSecret = "o6806642kbM7c5";

		/// <summary>
		/// Encrypt the given string using AES.  The string can be decrypted using
		/// DecryptStringAES().  The sharedSecret parameters must match.
		/// </summary>
		/// <param name="plainText">The text to encrypt.</param>
		/// <param name="sharedSecret">A password used to generate a key for encryption.</param>
		public static string EncryptStringAES(string plainText)
		{
			if (string.IsNullOrEmpty(plainText))
				throw new ArgumentNullException("plainText");
			if (string.IsNullOrEmpty(sharedSecret))
				throw new ArgumentNullException("sharedSecret");

			string outStr = null;
			RijndaelManaged aesAlg = null;

			try
			{
				using (var key = new Rfc2898DeriveBytes(sharedSecret, _salt))
				{
					using (aesAlg = new RijndaelManaged())
					{
						aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
						var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

						using (var msEncrypt = new MemoryStream())
						{
							msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
							msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

							using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
							{
								using (var swEncrypt = new StreamWriter(csEncrypt))
								{
									swEncrypt.Write(plainText);
								}
							}

							outStr = Convert.ToBase64String(msEncrypt.ToArray());
						}
					}
				}
			}
			finally
			{
				if (aesAlg != null)
					aesAlg.Clear();
			}

			return outStr;
		}

		/// <summary>
		/// Decrypt the given string.  Assumes the string was encrypted using
		/// EncryptStringAES(), using an identical sharedSecret.
		/// </summary>
		/// <param name="cipherText">The text to decrypt.</param>
		/// <param name="sharedSecret">A password used to generate a key for decryption.</param>
		public static string DecryptStringAES(string cipherText)
		{
			if (string.IsNullOrEmpty(cipherText))
				throw new ArgumentNullException("cipherText");
			if (string.IsNullOrEmpty(sharedSecret))
				throw new ArgumentNullException("sharedSecret");

			RijndaelManaged aesAlg = null;

			string plaintext = null;

			try
			{
				using (var key = new Rfc2898DeriveBytes(sharedSecret, _salt))
				{
					var bytes = Convert.FromBase64String(cipherText);
					using (var msDecrypt = new MemoryStream(bytes))
					{
						using (aesAlg = new RijndaelManaged())
						{
							aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
							aesAlg.IV = ReadByteArray(msDecrypt);
							var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

							using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
							{
								using (var srDecrypt = new StreamReader(csDecrypt))
								{
									plaintext = srDecrypt.ReadToEnd();
								}
							}
						}
					}
				}
			}
			catch (Exception exp)
			{
				return "Undefined Password";
			}
			finally
			{
				if (aesAlg != null)
					aesAlg.Clear();
			}

			return plaintext;
		}

		private static byte[] ReadByteArray(Stream s)
		{
			var rawLength = new byte[sizeof(int)];

			if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
			{
				throw new SystemException("Stream did not contain properly formatted byte array");
			}

			var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];

			if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new SystemException("Did not read byte array properly");
			}

			return buffer;
		}
		/*
		public static HttpCookie EncodeCookie(HttpCookie cookie)
		{
			if (cookie == null) return null;

			char[] chars = { ',' };
			var splits = ConfigurationManager.AppSettings["CookieEncodingKey"].Split(chars);
			var AlgorithmName = splits[0];
			var Key = new byte[Int32.Parse(splits[1])];// = KEY_64;
			for (int i = 2; i < Key.Length + 2; i++)
				Key[i - 2] = byte.Parse(splits[i].Trim());

			var eCookie = new HttpCookie(cookie.Name);

			for (int i = 0; i < cookie.Values.Count; i++)
			{
				var value = HttpContext.Current.Server.UrlEncode(Encode(cookie.Values[i], Key, AlgorithmName));
				var name = HttpContext.Current.Server.UrlEncode(Encode(cookie.Values.GetKey(i), Key, AlgorithmName));
				eCookie.Values.Set(name, value);
			}
			return eCookie;
		}
		*/

		public static string Encode(string value, Byte[] key, string AlgorithmName)
		{
			var ClearData = System.Text.Encoding.UTF8.GetBytes(value);

			var Algorithm = SymmetricAlgorithm.Create(AlgorithmName);
			Algorithm.Key = key;
			var Target = new MemoryStream();

			Algorithm.GenerateIV();
			Target.Write(Algorithm.IV, 0, Algorithm.IV.Length);

			using (var cs = new CryptoStream(Target, Algorithm.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cs.Write(ClearData, 0, ClearData.Length);
				cs.FlushFinalBlock();

				return Convert.ToBase64String(Target.GetBuffer(), 0, (int)Target.Length);
			}
		}
		/*
		public static HttpCookie DecodeCookie(HttpCookie cookie)
		{
			if (cookie == null) return null;

			char[] chars = { ',' };
			var splits = ConfigurationManager.AppSettings["CookieEncodingKey"].Split(chars);
			var AlgorithmName = splits[0];
			var Key = new byte[Int32.Parse(splits[1])];// = KEY_64;
			for (int i = 2; i < Key.Length + 2; i++)
				Key[i - 2] = byte.Parse(splits[i].Trim());

			var dCookie = new HttpCookie(cookie.Name);

			for (int i = 0; i < cookie.Values.Count; i++)
			{
				var value = Decode(HttpContext.Current.Server.UrlDecode(cookie.Values[i]), Key, AlgorithmName);
				var name = Decode(HttpContext.Current.Server.UrlDecode(cookie.Values.GetKey(i)), Key, AlgorithmName);
				dCookie.Values.Set(name, value);
			}
			return dCookie;
		}
		*/
		public static string Decode(string value, Byte[] key, string AlgorithmName)
		{
			var ClearData = Convert.FromBase64String(value);

			var Algorithm = SymmetricAlgorithm.Create(AlgorithmName);
			Algorithm.Key = key;
			var Target = new MemoryStream();

			var ReadPos = 0;
			var IV = new byte[Algorithm.IV.Length];
			Array.Copy(ClearData, IV, IV.Length);
			Algorithm.IV = IV;
			ReadPos += Algorithm.IV.Length;

			using (var cs = new CryptoStream(Target, Algorithm.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cs.Write(ClearData, ReadPos, ClearData.Length - ReadPos);
				cs.FlushFinalBlock();

				return System.Text.Encoding.UTF8.GetString(Target.ToArray());
			}
		}
	}
}