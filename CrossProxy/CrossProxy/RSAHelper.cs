﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace 用户模式
{
    public enum KeyFormat
    {
        ASN,
        XML,
        PEM
    }
    class RSAHelper
    {
         /// <summary>
        /// xml key
        /// </summary>
        private string _key;
        private KeyFormat _format;
        private RSACryptoServiceProvider _provider;
        private bool _isPrivate;
        public RSAHelper(string key, KeyFormat format = KeyFormat.XML)
        {
            this._key = key;
            this._format = format;
        }

        public string Encrypt(string data)
        {
            this._MakesureProvider();

            //原生.NET不提供私钥加密，公钥解密的方法，所以只能自行实现，但性能不知道如何。
            byte[] bytes = _isPrivate ? this._EncryptByPriKey(UTF8Encoding.UTF8.GetBytes(data), this._provider) : this._provider.Encrypt(UTF8Encoding.UTF8.GetBytes(data), false);
            // byte[] bytes = this._provider.Encrypt(UTF8Encoding.UTF8.GetBytes(data), false);
            return Convert.ToBase64String(bytes);
        }

        public string Decrypt(string data)
        {
            this._MakesureProvider();

            //原生.NET不提供私钥加密，公钥解密的方法，所以只能自行实现，但性能不知道如何。
            byte[] bytes = _isPrivate ? this._provider.Decrypt(Convert.FromBase64String(data), false) : this._DecryptByPubKey(Convert.FromBase64String(data), this._provider);
            return UTF8Encoding.UTF8.GetString(bytes);
        }

        private void _MakesureProvider()
        {
            if (this._provider != null) return;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);

            switch (this._format)
            {

                case KeyFormat.PEM:
                    {
                        this._key = this._key.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "")
                                              .Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "")
                                              .Replace("\r\n", "");
                        goto case KeyFormat.ASN;
                    }
                case KeyFormat.ASN:
                    {
                       
                        break;
                    }
                case KeyFormat.XML:
                default:
                    _isPrivate = this._key.IndexOf("<D>") > -1;
                    rsa.FromXmlString(this._key);
                    break;
            }

            this._provider = rsa;
        }

        #region 自行实现的RSA PKCS1填充方式的算法

        //填充
        private byte[] _AddPKCS1Padding(byte[] oText, int blockLen)
        {
            byte[] result = new byte[blockLen];
            result[0] = 0x00;
            result[1] = 0x01;
            int padLen = blockLen - 3 - oText.Length;
            for (int i = 0; i < padLen; i++)
            {
                result[i + 2] = 0xff;
            }
            result[padLen + 2] = 0x00;
            int j = 0;
            for (int i = padLen + 3; i < blockLen; i++)
            {
                result[i] = oText[j++];
            }
            return result;
        }

        //私钥加密
        private byte[] priEncrypt(byte[] block, RSACryptoServiceProvider key)
        {
            RSAParameters param = key.ExportParameters(true);
            BigInteger d = new BigInteger(param.D);
            BigInteger n = new BigInteger(param.Modulus);
            BigInteger biText = new BigInteger(block);
            BigInteger biEnText = biText.modPow(d, n);
            return biEnText.getBytes();
        }

        private byte[] _EncryptByPriKey(byte[] oText, RSACryptoServiceProvider key)
        {
            //获得明文字节数组
            //byte[] oText = System.Text.Encoding.UTF8.GetBytes(src);
            //填充
            oText = this._AddPKCS1Padding(oText, 128);
            //加密
            byte[] result = this.priEncrypt(oText, key);
            return result;
        }

        //公钥解密
        public byte[] _DecryptByPubKey(byte[] enc, RSACryptoServiceProvider key)
        {

            byte[] result = new byte[enc.Length];
            int k = 0;
            int blockLen = 128;
            int i = 0;
            do
            {
                //String temp = enc.Substring(i, blockLen);
                int length = (enc.Length - blockLen * i) > blockLen ? blockLen : (enc.Length - blockLen * i);
                byte[] oText = new byte[length];
                Array.Copy(enc, i * blockLen, oText, 0, length);

                //解密
                byte[] dec = pubDecrypt(oText, key);
                //if (dec.Length < blockLen)
                //{
                //    int offset = blockLen - dec.Length;
                //    Byte[] fitBytes = new byte[blockLen];
                //    for (int j = 0; j < offset; j++)
                //    {
                //        fitBytes[j] = 0x00;
                //    }
                //    Array.Copy(dec, 0, fitBytes, offset, dec.Length);
                //    dec = fitBytes;
                //}
                //去除填充
                dec = remove_PKCS1_padding(dec);
                Array.Copy(dec, 0, result, k, dec.Length);
                k += dec.Length;
                //result += System.Text.Encoding.Default.GetString(dec);

                i++;
            } while (i * blockLen < enc.Length);

            byte[] data = new byte[k];
            Array.Copy(result,0,data,0,k);
            return data;
        }

        //公钥解密
        private byte[] pubDecrypt(byte[] block, RSACryptoServiceProvider key)
        {
            RSAParameters param = key.ExportParameters(false);
            BigInteger e = new BigInteger(param.Exponent);
            BigInteger n = new BigInteger(param.Modulus);
            BigInteger biText = new BigInteger(block);
            BigInteger biEnText = biText.modPow(e, n);
            return biEnText.getBytes();
        }

        //去除填充
        private byte[] remove_PKCS1_padding(byte[] oText)
        {
            int i = 2;
            byte b = (byte)(oText[i] & 0xff);
            while (b != 0)
            {
                i++;
                b = (byte)(oText[i] & 0xff);
            }
            i++;

            byte[] result = new byte[oText.Length - i];
            int j = 0;
            while (i < oText.Length)
            {
                result[j++] = oText[i++];
            }
            return result;
        }

        #endregion
    }
}
