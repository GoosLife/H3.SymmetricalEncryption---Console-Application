using SymmetricalEncryption;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace H3.SymmetricalEncryption
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string[] encryptors = new string[] { "AES", "DES", "TripleDES" };

			Console.WriteLine("Choose an algorithm:");
			for (int i = 0; i < encryptors.Length; i++)
			{
				Console.WriteLine($"{i + 1}. {encryptors[i]}");
			}

			int choice = int.Parse(Console.ReadLine());
			SymmetricAlgorithm algorithm = null;

			switch (choice)
			{
				case 1:
					algorithm = Aes.Create();
					break;
				case 2:
					algorithm = DES.Create();
					break;
				case 3:
					algorithm = TripleDES.Create();
					break;
				default:
					throw new InvalidCastException("Invalid choice");
			}

			SymmetricalEncryptor encryptor = new SymmetricalEncryptor(algorithm);

			Console.WriteLine("Encryptor key:\t" + ToHex(encryptor.GetKey()));
			Console.WriteLine("Encryptor iv:\t" + ToHex(encryptor.GetIV()));

			Console.WriteLine("Enter a string to encrypt:");
			string input = Console.ReadLine();

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			byte[] encrypted = encryptor.Encrypt(input);

			stopwatch.Stop();

			Console.WriteLine($"Encrypted ASCII:\t{ToASCII(encrypted)}");
			Console.WriteLine($"Encrypted HEX:\t\t{ToHex(encrypted)}");
			Console.WriteLine($"Encryption Time:\t{stopwatch.ElapsedMilliseconds} ms");
			Console.WriteLine();

            Console.WriteLine("Attempting to decrypt...");
			stopwatch.Restart();
			string decrypted = Encoding.UTF8.GetString(encryptor.Decrypt(encrypted));
			stopwatch.Stop();
            Console.WriteLine("Decrypted string:\t" + decrypted);
            Console.WriteLine($"Decryption Time:\t{stopwatch.ElapsedMilliseconds} ms");

			Console.ReadKey();
        }
		private static string ToASCII(byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in data)
			{
				sb.Append((char)b);
			}
			return sb.ToString();
		}
		
		private static string ToHex(byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in data)
			{
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}
	}
}
