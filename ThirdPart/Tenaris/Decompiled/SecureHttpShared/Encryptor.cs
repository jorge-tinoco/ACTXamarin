// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.Encryptor
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecureHttpShared
{
  public class Encryptor
  {
    private static readonly string Salt = "d5fg4df5sg4ds5fg45sdfg4";
    private static readonly int SizeOfBuffer = 8192;
    private static readonly string Password = "1DE4565C-73E6-40CD-BE8F-44BF21DA68CC";

    public static void EncryptFile(byte[] fileToEncrypt, string outputPath)
    {
      MemoryStream memoryStream = new MemoryStream(fileToEncrypt);
      FileStream fileStream = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);
      RijndaelManaged algorithm = Encryptor.CreateAlgorithm();
      ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
      using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, encryptor, CryptoStreamMode.Write))
        Encryptor.CopyStream((Stream) memoryStream, (Stream) cryptoStream);
      memoryStream.Close();
      fileStream.Close();
    }

    public static void EncryptString(string plainText, string outputPath)
    {
      RijndaelManaged algorithm = Encryptor.CreateAlgorithm();
      ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
            streamWriter.Write(plainText);
          File.WriteAllBytes(outputPath, memoryStream.ToArray());
        }
      }
    }

    public static string DecryptString(byte[] fileBytes)
    {
      RijndaelManaged algorithm = Encryptor.CreateAlgorithm();
      ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
      using (MemoryStream memoryStream = new MemoryStream(fileBytes))
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
            return streamReader.ReadToEnd();
        }
      }
    }

    public static byte[] DecryptFile(byte[] fileBytes)
    {
      MemoryStream memoryStream1 = new MemoryStream(fileBytes);
      MemoryStream memoryStream2 = new MemoryStream();
      RijndaelManaged algorithm = Encryptor.CreateAlgorithm();
      ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
      using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream2, decryptor, CryptoStreamMode.Write))
        Encryptor.CopyStream((Stream) memoryStream1, (Stream) cryptoStream);
      memoryStream1.Close();
      memoryStream2.Close();
      return memoryStream2.ToArray();
    }

    private static RijndaelManaged CreateAlgorithm()
    {
      RijndaelManaged rijndaelManaged1 = new RijndaelManaged();
      rijndaelManaged1.KeySize = 256;
      rijndaelManaged1.BlockSize = 128;
      RijndaelManaged rijndaelManaged2 = rijndaelManaged1;
      Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(Encryptor.Password, Encoding.ASCII.GetBytes(Encryptor.Salt));
      rijndaelManaged2.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged2.KeySize / 8);
      rijndaelManaged2.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged2.BlockSize / 8);
      return rijndaelManaged2;
    }

    private static void CopyStream(Stream input, Stream output)
    {
      using (output)
      {
        using (input)
        {
          byte[] buffer = new byte[Encryptor.SizeOfBuffer];
          int count;
          while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            output.Write(buffer, 0, count);
        }
      }
    }

    private static string ByteArrayToHexa(byte[] ba)
    {
      StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
      foreach (byte num in ba)
        stringBuilder.AppendFormat("{0:x2}", (object) num);
      return stringBuilder.ToString();
    }

    private static byte[] HexaToByteArray(string hex)
    {
      int length = hex.Length;
      byte[] numArray = new byte[length / 2];
      int startIndex = 0;
      while (startIndex < length)
      {
        numArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
        startIndex += 2;
      }
      return numArray;
    }

    public static string EncryptToHexa(string plainText)
    {
      return Encryptor.ByteArrayToHexa(Encryptor.Encrypt(plainText));
    }

    public static string DecryptFromHexa(string hexa)
    {
      return Encryptor.Decrypt(Encryptor.HexaToByteArray(hexa));
    }

    public static byte[] Encrypt(string plainText)
    {
      RijndaelManaged algorithm = Encryptor.CreateAlgorithm();
      ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
            streamWriter.Write(plainText);
          return memoryStream.ToArray();
        }
      }
    }

    public static string Decrypt(byte[] data)
    {
      RijndaelManaged algorithm = Encryptor.CreateAlgorithm();
      ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
            return streamReader.ReadToEnd();
        }
      }
    }
  }
}
