using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DIMOWAModLoader.Mod_Loading
{
    [Serializable]
    public struct ModList
    {
        public string modFolder;
        public string[] modList;
        public ModList(string modFolder, params string[] modList)
        {
            this.modFolder = modFolder;
            this.modList = modList;
        }

        public static ModList FromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ModList>(json);
        }

        public void ToJsonFile(string filePath)
        {
            string file = JsonUtility.ToJson(this);
            StreamWriter s = File.CreateText(filePath);
            s.Write(file);
            s.Flush();
            s.Close();
        }
    }

    public class SearchMods
    {
        static public MOWAP[] GetModsMOWAPS(string gamePath, ModList list)
        {
            ModAssemblyLoader assemblyLoader = new ModAssemblyLoader(list.modFolder, gamePath);
            List<MOWAP> modsMowaps = new List<MOWAP>();
            Type IMOWAModInnit = Assembly.LoadFrom(DirectorySearchTools.GetFilePathInDirectory("CAMOWA.dll", gamePath, list.modFolder)).GetType("CAMOWA.IMOWAModInnit", true);
            for (int i = 0; i < list.modList.Length; i++)
            {
                try
                {
                    modsMowaps.Add(assemblyLoader.GenerateModMOWAPFromDll(list.modList[i], IMOWAModInnit));
                }
                catch(Exception ex) { Debug.Log(string.Format("Coudldn't get mod info from {0} : {1} - {2} - {3}", list.modList[i], ex.Message, ex.Source, ex.StackTrace)); }
            }
               

            return modsMowaps.ToArray();
        }
    }
}
