using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Runtime.Serialization;

namespace DIMOWAModLoader.Mod_Loading
{
    public class ModFolderAndList : DIMOWASerializable
    {
        public string ModFolder { get; set; }
        public string[] ModList { get; set; }

        public ModFolderAndList()
        {
            ModFolder = "";
            ModList = new string[0];
        }
        public override void Deserialize(ref BinaryReader reader)
        {
            ModFolder = reader.ReadString();
            ModList = new string[reader.ReadInt32()];
            for (int i = 0; i < ModList.Length; i++)
                ModList[i] = reader.ReadString();
        }
        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(ModFolder);
            writer.Write(ModList.Length);
            for (int i = 0; i < ModList.Length; i++)
                writer.Write(ModList[i]);
        }
    }

    public class SearchMods
    {
        static public MOWAP[] GetModsMOWAPS(string gamePath, ModFolderAndList list)
        {
            ModAssemblyLoader assemblyLoader = new ModAssemblyLoader(list.ModFolder, gamePath);
            List<MOWAP> modsMowaps = new List<MOWAP>();
            Type IMOWAModInnit = Assembly.LoadFrom(DirectorySearchTools.GetFilePathInDirectory("CAMOWA.dll", gamePath, list.ModFolder)).GetType("CAMOWA.IMOWAModInnit", true);
            for (int i = 0; i < list.ModList.Length; i++)
            {
                try
                {
                    modsMowaps.Add(assemblyLoader.GenerateModMOWAPFromDll(list.ModList[i], IMOWAModInnit));
                }
                catch(Exception ex) { Debug.Log(string.Format("Coudldn't get mod info from {0} : {1} - {2} - {3}", list.ModList[i], ex.Message, ex.Source, ex.StackTrace)); }
            }
               

            return modsMowaps.ToArray();
        }
    }
}
