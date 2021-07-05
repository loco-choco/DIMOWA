using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IMOWA.GUI;

namespace IMOWA
{
    public class ModDataHandler
    {
        public DIMOWALoaderInstaller DIMOWALoaderInstaller;
        public ModManifestJson[] ManifestsDosMods;
        public ModManifestJson ManifestDoLoader;
        public ModListJson ModEnabledList; 

        public const string dimowaModLoaderFile = "DIMOWAModLoader.dll";
        public const string loaderModJsonFile = "ModList.json";
        public ModDataHandler(string caminhoDoJogo, string caminhoDaPastaDeMods, string caminhoDaPastaDeManifestos)
        {
            string loaderJsonPath = "";
            try
            {
                loaderJsonPath = DirectorySearchTools.GetFilePathInDirectory(loaderModJsonFile, caminhoDoJogo);
            }
            catch { ConsoleWindowHelper.Warning(string.Format("{0} couldn't be found, the mod loader probrabilly hasn't been installed yet.",loaderModJsonFile)); }

            try
            {
                ModEnabledList = JsonReader.ReadFromJson<ModListJson>(loaderJsonPath);
            }
            catch
            {
                ConsoleWindowHelper.Exception(string.Format("{0} couldn't be read, the problem can be fixed by pressing save.", loaderModJsonFile));
                ModEnabledList = new ModListJson() { ModFolder = "", ModList = new string[] { } };
            }

            string[] todosOsJsons = Directory.GetFiles(caminhoDaPastaDeManifestos, "*manifest.json", SearchOption.AllDirectories);
            List<ModManifestJson> manifests = new List<ModManifestJson>();

            for (int i = 0; i < todosOsJsons.Length; i++)
            {
                var modData = JsonReader.ReadFromJson<ModManifestJson>(todosOsJsons[i]);
                if (modData.FileName != dimowaModLoaderFile)
                    manifests.Add(modData);
                else
                    ManifestDoLoader = modData;
            }
            if (string.IsNullOrEmpty(ManifestDoLoader.FileName))
                ConsoleWindowHelper.FatalException("DIMOWAModLoader.dll needs to be inside the mods folder with its manifest.json, make sure that it is in there.");

            ManifestsDosMods = manifests.ToArray();
            DIMOWALoaderInstaller = new DIMOWALoaderInstaller(caminhoDoJogo, caminhoDaPastaDeMods);
        }
        //public string GetModDllFileName(int index)
        //{
        //    if (index >= indexOfTheLoader)
        //        index += 1;
        //    return DllsDosMods[index];
        //}
        //public string GetLoaderDllFilePath()
        //{
        //    return dimowaModLoaderPath;
        //}
        //public MOWAP GetModMOWAP(int index)
        //{
        //    return modMowaps[index];
        //}

        //public static string CopyAndGetImportantFileToManaged(string file, string gameFolder)
        //{
        //    string filePath = "";
        //    try
        //    {
        //        filePath = ModManager.GetFilePathInDirectory(file, gameFolder);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.GetType() == typeof(FileNotFoundException))
        //        {
        //            try
        //            {
        //                filePath = ModManager.GetFilePathInDirectory(file, Form1.programPath);
        //                File.Copy(filePath, ModManager.GetDirectoryInDirectory("Managed", gameFolder) + "\\" + file);
        //            }
        //            catch (Exception e)
        //            {
        //                if (e.GetType() == typeof(FileNotFoundException))
        //                    ConsoleWindowHelper.FatalException("The file " + file + " isn't inside the game's folder nor the program folder. Check if you got all the files from the download.");
        //                else if (e.GetType() == typeof(DirectoryNotFoundException))
        //                    ConsoleWindowHelper.FatalException("Couldn't find the Managed sub folder inside the game's folder, make sure you gave the correct folder by deleting config.json or by editing config.json");
        //                else
        //                    ConsoleWindowHelper.FatalException("An unexpected exception occured: " + e.Message + "  " + ex.StackTrace + "  " + ex.Source);
        //                return "";
        //            }
        //        }
        //    }
        //    return filePath;
        //}
    }
}
