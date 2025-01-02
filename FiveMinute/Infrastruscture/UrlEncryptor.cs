using System.Text;
using System.Security.Cryptography;

public static class UrlEncryptor
{
	private static readonly string Key = "31963366d40b22eebe5debac53bd98884ea8e8f8a38f2f6ce662a5bd031aafa0";
	private static readonly string FixedIV = "d41d8cd98f00b204e9800998ecf8427e"; // Use a fixed IV (hex-encoded)

	public static string Encrypt(int id)
	{
		var keyBytes = Convert.FromHexString(Key);
		var ivBytes = Convert.FromHexString(FixedIV);

		using var aes = Aes.Create();
		aes.Key = keyBytes;
		aes.IV = ivBytes;

		using var encryptor = aes.CreateEncryptor();
		var idBytes = Encoding.UTF8.GetBytes(id.ToString());
		var encryptedBytes = encryptor.TransformFinalBlock(idBytes, 0, idBytes.Length);

		// Encode only the encrypted data, as IV is fixed
		return Convert.ToBase64String(encryptedBytes);
	}

	public static int Decrypt(string encryptedId)
	{
		var keyBytes = Convert.FromHexString(Key);
		var ivBytes = Convert.FromHexString(FixedIV);
		var encryptedBytes = Convert.FromBase64String(encryptedId);

		using var aes = Aes.Create();
		aes.Key = keyBytes;
		aes.IV = ivBytes;

		using var decryptor = aes.CreateDecryptor();
		var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

		var decryptedString = Encoding.UTF8.GetString(decryptedBytes);
		return int.Parse(decryptedString);
	}
}
