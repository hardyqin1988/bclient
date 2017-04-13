using Firefly.Unity.Global;
using System.Collections.Generic;

namespace Firefly.Unity.Utility
{
    public static class SystemUtil
    {
        public static int FloatToIntBits(float f)
        {
            return System.BitConverter.ToInt32(System.BitConverter.GetBytes(f), 0);
        }

        public static float IntBitsToFloat(int i)
        {
            return System.BitConverter.ToSingle(System.BitConverter.GetBytes(i), 0);
        }
        
        public static void StringToList<T>(ref List<T> ret, string s, char sep = ',') where T : System.IConvertible
        {
            ret.Clear();
            string[] ss = s.Split(sep);
            for (int i = 0; i < ss.Length; i++)
            {
                try
                {
                    T v = (T)System.Convert.ChangeType(ss[i], typeof(T));
                    ret.Add(v);
                }
                catch (System.Exception exp)
                {
                    LogAssert.Util.Warn("Convert \"{0}\" to {1} List error.({2})", s, typeof(T).Name, exp.ToString());
                }
            }
        }
    }
}
