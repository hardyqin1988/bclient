using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DevEditor.Custom
{
    public class FileCheckTools
    {
        public enum FileCheckTypes
        {
            CSharp = 1,
            Lua = 2,
            Config = 3,
        }

        public static void ClearExtraCSharpCode()
        {
            DirectoryInfo dti = new DirectoryInfo(Application.dataPath);

            EditorUtility.DisplayProgressBar("FindFile", "Finding", 0);
            List<string> path_list = new List<string>();
            FindFile(FileCheckTypes.CSharp, dti, ref path_list);

            List<string> filters = new List<string>();
            filters.Add("/*\r\nhttp://www.cgsoso.com/forum-211-1.html\r\n\r\nCG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！\r\n\r\nCGSOSO 主打游戏开发，影视设计等CG资源素材。\r\n\r\n插件如若商用，请务必官网购买！\r\n\r\ndaily assets update for try.\r\n\r\nU should buy the asset from home store if u use it in your project!\r\n*/\r\n\r\n");

            for (int i = 0; i < path_list.Count; i++)
            {
                EditorUtility.DisplayProgressBar("FindFile", string.Format("Finding {0}", path_list[i]), (float)i / (float)path_list.Count);

                ClearExtraCode(path_list[i], filters);
            }

            EditorUtility.ClearProgressBar();
        }

        private static void ClearExtraCode(string path, List<string> filters)
        {
	        bool change = false;
            StreamReader streamReader = new StreamReader(path);
            string content = streamReader.ReadToEnd();
            foreach (string filter in filters)
            {
                int index = content.LastIndexOf(filter);
                if (index < 0)
                {
                    continue;
                }
	            
	            change = true;

                content = content.Remove(index, filter.Length);
                Debug.Log(path);
            }
	        streamReader.Close();
	        
	        if (!change)
	        {
	        	return;
	        }

            StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.Write(content);
            streamWriter.Close();
        }

        private static void FindFile(FileCheckTypes type, DirectoryInfo directory_info, ref List<string> path_list)
        {
            FileInfo[] fs = directory_info.GetFiles();

            foreach (FileInfo f in fs)
            {
                if (f.FullName.Contains(".meta"))
                {
                    continue;
                }

                if (type == FileCheckTypes.CSharp && f.FullName.Contains(".cs"))
                {
                    path_list.Add(f.FullName);
                }
                else if (type == FileCheckTypes.Lua && f.FullName.Contains(".lua"))
                {
                    path_list.Add(f.FullName);
                }
                else if (type == FileCheckTypes.Config && f.FullName.Contains(".xml"))
                {
                    path_list.Add(f.FullName);
                }
            }

            DirectoryInfo[] ds = directory_info.GetDirectories();

            foreach (DirectoryInfo di in ds)
            {
                
                FindFile(type, di, ref path_list);
            }
        }

        public static void DeleteEmptyDir()
        {
            DelateEmptyDirAnd_mate(Application.dataPath + "/");
            AssetDatabase.Refresh();
        }

        static void DelateEmptyDirAnd_mate(string path)
        {
            // Application.dataPath   
            string[] files = Directory.GetFiles(path, "*.meta");
            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
            {

                string filesName = files[i].ToString();
                string filesSub = filesName.Substring(0, filesName.IndexOf(".meta"));

                if (File.Exists(filesSub))
                {

                }
                else
                {
                    File.Delete(files[i]);
                }
                DelateEmptyDirAnd_mate(dirs[i]);
            }
            try
            {
                Directory.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("无法删除，因为文件夹不为空:{0}", path, ex);
            }
            Debug.Log("删除完成");
        }
    }
}

