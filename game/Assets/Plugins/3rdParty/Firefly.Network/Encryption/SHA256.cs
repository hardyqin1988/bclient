using Firefly.Core.Utility;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Firefly.Network.Encryption
{
    public class SHA256
    {
        public static SHA256Managed Sha256 = new SHA256Managed();
        public static byte[] Encrypt(string uid, string token, byte[] rnd, int rnd_start, int rnd_length)
        {
            MemoryStream ms = new MemoryStream();
            byte[] uuid_bytes = Encoding.ASCII.GetBytes(uid);
            byte[] token_bytes = Encoding.ASCII.GetBytes(token);
            ms.Write(uuid_bytes, 0, uuid_bytes.Length);
            ms.Write(token_bytes, 0, token_bytes.Length);

            ms.Write(rnd, rnd_start, rnd_length);
            return Sha256.ComputeHash(ms.ToArray());
        }
        public static byte[] LoginEncrypt(string uid, string token)
        {
            long now = TimeUtil.NowMilliseconds;
            return Encrypt(uid, token, GetBytes(now), 2, 6); // discard lower two bytes
        }
        public static byte[] GetBytes(long n)
        {
            byte[] bs = BitConverter.GetBytes(n);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bs);
            }
            return bs;
        }
        public static bool LoginValidate(string uid, string token, byte[] hash)
        {
            long now = TimeUtil.NowMilliseconds;
            long pre = now - 65536;
            long next = now + 65536;
            byte[] now_bs = GetBytes(now);
            byte[] pre_bs = GetBytes(pre);
            byte[] next_bs = GetBytes(next);
            return Encrypt(uid, token, now_bs, 2, 6).SequenceEqual(hash)
                || Encrypt(uid, token, pre_bs, 2, 6).SequenceEqual(hash)
                || Encrypt(uid, token, next_bs, 2, 6).SequenceEqual(hash);
        }
    }
}
