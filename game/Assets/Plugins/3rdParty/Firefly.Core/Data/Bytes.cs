#region License
/*****************************************************************************
 *MIT License
 *
 *Copyright (c) 2017 cathy33

 *Permission is hereby granted, free of charge, to any person obtaining a copy
 *of this software and associated documentation files (the "Software"), to deal
 *in the Software without restriction, including without limitation the rights
 *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *copies of the Software, and to permit persons to whom the Software is
 *furnished to do so, subject to the following conditions:

 *The above copyright notice and this permission notice shall be included in all
 *copies or substantial portions of the Software.

 *THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *SOFTWARE.
 *****************************************************************************/
#endregion

using ProtoBuf;
using System;
using System.Text;

namespace Firefly.Core.Data
{
    /// <summary>
    /// Represents a three dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [ProtoContract]
    public struct Bytes : IComparable<Bytes>
    {
        public static readonly Bytes Zero = new Bytes();

        public static Bytes New() { return new Bytes(); }

        public static Bytes New(byte[] bytes) { return new Bytes(bytes); }

        [ProtoMember(1)]
        public byte[] ByteArray;

        public Bytes(byte[] bytes)
        {
            ByteArray = new byte[bytes.Length];
            bytes.CopyTo(ByteArray, 0);
        }

        public static Bytes Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Zero;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(value);

            return New(bytes);
        }
        public static implicit operator byte[] (Bytes bytes)
        {
            return bytes.ByteArray;
        }
        public static bool operator ==(Bytes left, Bytes right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Bytes left, Bytes right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(this.ByteArray);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() == typeof(byte[]))
                return ByteArray == obj;

            return false;
        }

        public static int CompareBytes(byte[] b1, byte[] b2)
        {
            int result = 0;
            if (b1.Length != b2.Length)
                result = b1.Length - b2.Length;
            else
            {
                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                    {
                        result = (int)(b1[i] - b2[i]);
                        break;
                    }
                }
            }
            return result;
        }

        public int CompareTo(Bytes other)
        {
            return CompareBytes(this.ByteArray, other.ByteArray);
        }
    }
}
