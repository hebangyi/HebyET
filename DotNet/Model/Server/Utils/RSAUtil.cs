namespace ET;
using System;
using System.Security.Cryptography;



public static class RSAUtil
{
    
    public static RSA LoadRSAPrivateKey(string pemStr)
    {
        pemStr = pemStr.Replace("\r", "");
        var pemLines = pemStr.Split('\n');
        for (var i = 0; i < pemLines.Length; i++)
        {
            var line = pemLines[i];
            if (line.StartsWith("-----")) pemLines[i] = "";
        }
    
        pemStr = string.Join("", pemLines);
        var bytes = Convert.FromBase64String(pemStr);
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(bytes, out _);
        return rsa;
    }
    
    public static RSA LoadRSAPublicKey(string pemStr)
    {
        pemStr = pemStr.Replace("\r", "");
        var pemLines = pemStr.Split('\n');
        for (var i = 0; i < pemLines.Length; i++)
        {
            var line = pemLines[i];
            if (line.StartsWith("-----")) pemLines[i] = "";
        }
    
        pemStr = string.Join("", pemLines);
        var bytes = Convert.FromBase64String(pemStr);
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(bytes, out _);
        return rsa;
    }
    
    public static byte[] SignData(RSA privateKey, byte[] data, string hashAlgorithm = "sha1")
    {
        var hsal = HashAlgorithmName.SHA1; // new HashAlgorithmName(hashAlgorithm);
        return privateKey.SignData(data, hsal, RSASignaturePadding.Pkcs1);
    }
    
    public static bool VerifyData(RSA publicKey, byte[] data, byte[] signBytes, string hashAlgorithm = "sha1")
    {
        if (data == null || signBytes == null)
        {
            return false;
        }
    
        var hsal = HashAlgorithmName.SHA1; // new HashAlgorithmName(hashAlgorithm);
        return publicKey.VerifyData(data, signBytes, hsal, RSASignaturePadding.Pkcs1);
    }
}