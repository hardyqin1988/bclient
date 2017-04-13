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
using System.Globalization;

namespace Firefly.Core.Data
{
    [ProtoContract]
    public struct Int2 : IEquatable<Int2>, IFormattable
    {
        public static readonly Int2 Zero = new Int2();

        public static Int2 New() { return new Int2(0); }

        public static Int2 New(int x, int y) { return new Int2(x, y); }

        [ProtoMember(Constant.INT_PROTO_X)]
        public int Item1;

        [ProtoMember(Constant.INT_PROTO_Y)]
        public int Item2;

        public Int2(int value)
        {
            Item1 = value;
            Item2 = value;
        }

        public Int2(int x, int y)
        {
            Item1 = x;
            Item2 = y;
        }

        public Int2(int[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 3)
                throw new ArgumentOutOfRangeException("values", "There must be two and only two input values for Int2.");

            Item1 = values[0];
            Item2 = values[1];
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Item1;
                    case 1: return Item2;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Int2 run from 0 to 1, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: Item1 = value; break;
                    case 1: Item2 = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Int2 run from 0 to 1, inclusive.");
                }
            }
        }

        public void Negate()
        {
            Item1 = -Item1;
            Item2 = -Item2;
        }

        public void Abs()
        {
            Item1 = Math.Abs(Item1);
            Item2 = Math.Abs(Item2);
        }

        public int[] ToArray()
        {
            return new int[] { Item1, Item2 };
        }

        public static void Sqrt(ref Int2 value, out Int2 result)
        {
            result.Item1 = (int)Math.Sqrt(value.Item1);
            result.Item2 = (int)Math.Sqrt(value.Item2);
        }

        public static Int2 Sqrt(Int2 value)
        {
            Int2 temp;
            Sqrt(ref value, out temp);
            return temp;
        }

        public static Int2 operator +(Int2 left, Int2 right)
        {
            return new Int2(left.Item1 + right.Item1, left.Item2 + right.Item2);
        }

        public static Int2 operator +(Int2 value)
        {
            return value;
        }

        public static Int2 operator -(Int2 left, Int2 right)
        {
            return new Int2(left.Item1 - right.Item1, left.Item2 - right.Item2);
        }

        public static Int2 Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Zero;
            }

            string[] Int2 = value.Split(',');
            if (Int2.Length != 3)
            {
                return Zero;
            }

            int x = Convert.ToInt32(Int2[0]);
            int y = Convert.ToInt32(Int2[1]);

            return New(x, y);
        }

        public static Int2 operator -(Int2 value)
        {
            return new Int2(-value.Item1, -value.Item2);
        }

        public static Int2 operator *(int scalar, Int2 value)
        {
            return new Int2(value.Item1 * scalar, value.Item2 * scalar);
        }

        public static Int2 operator *(Int2 value, int scalar)
        {
            return new Int2(value.Item1 * scalar, value.Item2 * scalar);
        }

        public static Int2 operator /(Int2 value, int scalar)
        {
            return new Int2(value.Item1 / scalar, value.Item2 / scalar);
        }

        public static bool operator ==(Int2 left, Int2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Int2 left, Int2 right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Item1, Item2);
        }

        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}",
                Item1.ToString(format, CultureInfo.CurrentCulture),
                Item2.ToString(format, CultureInfo.CurrentCulture));
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "X:{0} Y:{1}", Item1, Item2);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                return ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1}",
                Item1.ToString(format, formatProvider),
                Item2.ToString(format, formatProvider));
        }

        public override int GetHashCode()
        {
            return Item1.GetHashCode() + Item2.GetHashCode();
        }

        public bool Equals(Int2 other)
        {
            return (Item1 == other.Item1) && (Item2 == other.Item2);
        }

        public bool Equals(Int2 other, int epsilon)
        {
            return Math.Abs(other.Item1 - Item1) < epsilon &&
                Math.Abs(other.Item2 - Item2) < epsilon;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Int2)obj);
        }
    }
}
