using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firefly.Core.Utility
{
    public class OpcodeUtil
    {
        private static readonly object initLock = new object();
        public static Dictionary<Type, Dictionary<int, string>> _IntToNames = new Dictionary<Type, Dictionary<int, string>>();

        private static Dictionary<int, string> GenNameDic<T>()
        {
            Type t = typeof(T);
            FieldInfo[] field_infos = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return field_infos.Where(fi => fi.IsLiteral && fi.FieldType.Name.StartsWith("Int")).ToDictionary(f => (int)f.GetValue(null), f => f.Name);
        }

        public static string ToName<T>(int opcode)
        {
            Type t = typeof(T);
            if (!_IntToNames.ContainsKey(t))
            {
                lock (initLock)
                    _IntToNames[t] = GenNameDic<T>();
            }

            string name = "[UNKOWN]";
            _IntToNames[t].TryGetValue(opcode, out name);
            return name;
        }

        private static Dictionary<Type, HashSet<int>> _OpcodeSet = new Dictionary<Type, HashSet<int>>();

        private static HashSet<int> GenOpcodeDict<T>()
        {
            Type t = typeof(T);
            FieldInfo[] field_infos = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            HashSet<int> set = new HashSet<int>();
            for (int i = 0; i < field_infos.Length; i++)
            {
                FieldInfo fi = field_infos[i];
                if (!fi.IsLiteral || !fi.FieldType.Name.StartsWith("Int"))
                {
                    continue;
                }

                int opcode = (int)fi.GetValue(null);

                if (set.Contains(opcode))
                {
                    continue;
                }

                set.Add(opcode);
            }

            return set;
        }

        public static bool Contains<T>(int opcode)
        {
            Type t = typeof(T);
            if (!_OpcodeSet.ContainsKey(t))
            {
                lock (initLock)
                    _OpcodeSet[t] = GenOpcodeDict<T>();
            }

            return _OpcodeSet[t].Contains(opcode);
        }
    }
}
