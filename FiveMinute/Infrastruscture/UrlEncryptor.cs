using System.Text;
using System.Security.Cryptography;

namespace FiveMinute.Utils
{
	public static class UrlEncryptor
	{
		private static readonly string Key = "31963366d40b22eebe5debac53bd98884ea8e8f8a38f2f6ce662a5bd031aafa0";

		public static string Encrypt(int id)
		{
			var keyBytes = Convert.FromHexString(Key);
			using var aes = Aes.Create();
			aes.Key = keyBytes;
			aes.GenerateIV(); // Use a unique IV for every encryption
			var iv = aes.IV;

			using var encryptor = aes.CreateEncryptor();
			var idBytes = Encoding.UTF8.GetBytes(id.ToString());
			var encryptedBytes = encryptor.TransformFinalBlock(idBytes, 0, idBytes.Length);

			// Combine IV and encrypted data into a single string
			var result = Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(encryptedBytes);
			return result;
		}

		public static int Decrypt(string encryptedId)
		{
			var keyBytes = Convert.FromHexString(Key);
			var parts = encryptedId.Split(':');
			if (parts.Length != 2)
				throw new FormatException("Invalid encrypted data format.");

			var iv = Convert.FromBase64String(parts[0]);
			var encryptedBytes = Convert.FromBase64String(parts[1]);

			using var aes = Aes.Create();
			aes.Key = keyBytes;
			aes.IV = iv;

			using var decryptor = aes.CreateDecryptor();
			var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

			var decryptedString = Encoding.UTF8.GetString(decryptedBytes);
			return int.Parse(decryptedString);
		}
	}
}
