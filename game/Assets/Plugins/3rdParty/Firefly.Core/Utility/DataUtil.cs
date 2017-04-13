using Firefly.Core.Data;
using Firefly.Core.Variant;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Firefly.Core.Utility
{
    public static class DataUtil
    {
        public static VariantList SplitList(this string str, VariantType parse_type, char split_tag = ';')
        {
            VariantList list = VariantList.New();

            string[] str_list = str.Split(split_tag);
            foreach (string temp in str_list)
            {
                switch (parse_type)
                {
                    case VariantType.Bool:
                        list.Append(Convert.ToBoolean(temp));
                        break;
                    case VariantType.Byte:
                        list.Append(Convert.ToByte(temp));
                        break;
                    case VariantType.Int:
                        list.Append(Convert.ToInt32(temp));
                        break;
                    case VariantType.Long:
                        list.Append(Convert.ToInt64(temp));
                        break;
                    case VariantType.Float:
                        list.Append(Convert.ToSingle(temp));
                        break;
                    case VariantType.Bytes:
                        list.Append(Bytes.Parse(temp));
                        break;
                    case VariantType.String:
                        list.Append(temp);
                        break;
                    case VariantType.PersistID:
                        list.Append(PersistID.Parse(temp));
                        break;
                }
            }

            return list;
        }

        public static bool IsNullOrEmpty(this VariantList list)
        {
            return list == VariantList.Empty || list == null;
        }

        public static byte[] ObjectToBytes<T>(T t)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                Serializer.Serialize(ms, t);
            }catch(Exception ex)
            {
                throw new Exception(String.Format("{0}\n{1}\n", ex.Message, ex.StackTrace));
            }
            byte[] bytes = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bytes, 0, bytes.Length);
            ms.Dispose();
            ms = null;

            return bytes;
        }

        public static T BytesToObject<T>(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            T t;
            try
            {
                t = Serializer.Deserialize<T>(ms);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}\n{1}\n", ex.Message, ex.StackTrace));
            }
            ms.Dispose();
            ms = null;

            return t;
        }

        public static VariantList Flatten(this IList<VariantList> list)
        {
            return list.Flatten(0, list.Count);
        }

        /// <summary>
        /// IList<VariantList>组装成一个VariantList
        /// </summary>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static VariantList Flatten(this IList<VariantList> list, int start, int stop)
        {
            VariantList result = new VariantList();
            stop = Math.Min(list.Count, stop);
            for (int j = start; j < stop; j++)
            {
                result.Append(list[j].Count).Append(list[j]);
            }
            return result;
        }

        /// <summary>
        /// Flatten的逆操作
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<VariantList> Cut(this VariantList list)
        {
            int step = list.SubtractInt(0);
            if (step <= 0)
                return new List<VariantList>();
            var result = new List<VariantList>(list.Count / step);
            int i = 0;
            while (list.Count > 0)
            {
                result.Add(list.SubtractRange(i, step));
                i += step;
                step = list.SubtractInt(i);
                if (step <= 0)
                    break;
            }
            return result;
        }

        //将Byte转换为结构体类型
        public static byte[] StructToBytes(object structure)
        {
            int size = Marshal.SizeOf(structure);
            Console.WriteLine(size);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        //将Byte转换为结构体类型
        public static object ByteToStruct(byte[] bytes, Type strcut_type)
        {
            int size = Marshal.SizeOf(strcut_type);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcut_type);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
