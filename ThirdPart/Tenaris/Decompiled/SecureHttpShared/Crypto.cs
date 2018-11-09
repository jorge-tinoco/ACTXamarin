using Newtonsoft.Json;
using SecureHttpShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text; 
namespace SecureHttpShared
{
    /// <summary>
    /// Encrypt/decript data using an external library (<seealso cref="SecureHttpShared"/>)
    /// </summary>
    internal class Crypto
    {
        /// <summary>
        /// Decript a file an turns into a Key-Value dictionary.
        /// </summary>
        /// <param name="fileName"></param>
        internal Dictionary<string,object> DecryptFile(string fileName)
        {
            byte[] fileInBytes = AssetToBytes(fileName);
            byte[] decriptedInBytes = Encryptor.DecryptFile(fileInBytes);
            Dictionary<String, Object> decriptedData = BytesToJson(decriptedInBytes);
            return decriptedData;
        }

        /// <summary>
        /// Encrypt a file in android assets. And store encrypted file in Android personal folder as "encrypted_file.txt"
        /// </summary>
        /// <example>
        ///     <code>
        ///         Encrypt("config_tl.txt");
        ///     </code>
        /// </example>
        /// <param name="fileName"></param>
        internal void EncryptFile(string fileName)
        {
            Stream fileStream = Forms.Context.Assets.Open(fileName);
            byte[] inputFile;
            using (MemoryStream ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                inputFile = ms.ToArray();
            }
            string documentsPath = System.Environment.GetFolderPath(
                                                System.Environment.SpecialFolder.Personal);
            string OutputPath = string.Concat(documentsPath, "/encrypted_file.txt");
            Encryptor.EncryptFile(inputFile, OutputPath);
        }


        /// <summary>
        /// Get a byte array from a file stored in Assets
        /// </summary>
        /// <param name="assetPath">File name to get.</param>
        /// <returns>File in byte[] format.</returns>
        private byte[] AssetToBytes(string assetPath)
        {
            Stream fileStream = Forms.Context.Assets.Open(assetPath);
            byte[] inputFile;
            using (MemoryStream ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                inputFile = ms.ToArray();
            }
            return inputFile;
        }


        /// <summary>
        /// Convert an byte array to  dictionary.
        /// </summary>
        /// <example>
        ///     The byte array must be encoded from a json formated text like this:
        ///     {
        ///         "key": "value",
        ///         "key": "value"
        ///     }
        /// </example>
        /// <param name="arrayInJson">Byte array that contains data in JSON format.</param>
        /// <returns></returns>
        private Dictionary<String, Object> BytesToJson(byte[] arrayInJson)
        {
            try
            {
                var reader = new StreamReader(new MemoryStream(arrayInJson), Encoding.Default);
                string content = reader.ReadToEnd();
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (values == null)
                    throw new Exception("No se ha podido recuperar los datos del array de bytes");
                return values;
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error( "WTT", ex.Message);
                throw ex;
            }
            
        }
    }
}