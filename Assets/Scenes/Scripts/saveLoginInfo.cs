using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class saveLoginInfo : MonoBehaviour
{
    private static readonly byte[] key = Encoding.UTF8.GetBytes("yFYDtjvbkjbrtvkb"); 
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("UIGKyrdjOMOihvtd");

    // Encrypt a string.
    public static string EncryptString(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    // Decrypt a string.
    public static string DecryptString(string cipherText)
    {
        var buffer = Convert.FromBase64String(cipherText);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(buffer))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    // Properties for UserId and Password
    public static string UserId
    {
        get => DecryptString(PlayerPrefs.GetString("UserId", ""));
        set
        {
            PlayerPrefs.SetString("UserId", EncryptString(value));
            PlayerPrefs.Save();
        }
    }

    public static string Password
    {
        get => DecryptString(PlayerPrefs.GetString("Password", ""));
        set
        {
            PlayerPrefs.SetString("Password", EncryptString(value));
            PlayerPrefs.Save();
        }
    }

    // Check if the login and password exist
    public static bool AreCredentialsSaved()
    {
        //decrypt and log the UserId for debugging purposes.
        //string testing1 = DecryptString(PlayerPrefs.GetString("UserId"));
        //Debug.Log($"UserId: {testing1}");
        // Check if both UserId and Password have been saved and are not empty
        return !string.IsNullOrEmpty(PlayerPrefs.GetString("UserId")) && !string.IsNullOrEmpty(PlayerPrefs.GetString("Password"));
        
    }

    public static void ResetCredentials()
    {
        PlayerPrefs.DeleteKey("UserId");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.Save();
    }

}

