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
        public ModManager ModManager;
        private string[] dllsDosMods;
        private string dimowaModLoaderPath;
        private int indexOfTheLoader;
        private MOWAP[] modMowaps;


        public ModDataHandler(string caminhoDoJogo, string caminhoDaPastaDeMods, string caminhoDaPastaDeManifestos)
        {
            indexOfTheLoader = -1;

            string[] todosOsJsons = Directory.GetFiles(caminhoDaPastaDeManifestos, "*manifest.json", SearchOption.AllDirectories);
            dllsDosMods = new string[todosOsJsons.Length];

            for (int i = 0; i < todosOsJsons.Length; i++)
            {
                JsonReader json = new JsonReader(todosOsJsons[i]);
                dllsDosMods[i] = json.GetJsonElementFromRoot("filename").Value;
                if (dllsDosMods[i] == "DIMOWAModLoader.dll")
                {
                    if (indexOfTheLoader < 0)
                        indexOfTheLoader = i;
                }
            }
            if (indexOfTheLoader < 0)
            {
                ConsoleWindowHelper.FatalException("DIMOWAModLoader.dll needs to be inside the mods folder with its manifest.json, make sure that it is in there.");
            }

            string camowaPath = CopyAndGetImportantFileToManaged("CAMOWA.dll", caminhoDoJogo);

            Type imowaModInnitType = Assembly.LoadFrom(camowaPath).GetType("CAMOWA.IMOWAModInnit");
            Assembly unityEngine = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("UnityEngine.dll", caminhoDoJogo));

            List<MOWAP> listOfMods = new List<MOWAP>();
            ModDataGatherer gatherer = new ModDataGatherer(caminhoDaPastaDeMods, caminhoDoJogo);

            foreach (string fileName in new HashSet<string>(dllsDosMods))
                if (fileName != dllsDosMods[indexOfTheLoader])
                    listOfMods.Add(gatherer.GenerateModMOWAPFromDll(fileName, imowaModInnitType));

           dimowaModLoaderPath = CopyAndGetImportantFileToManaged("DIMOWAModLoader.dll", caminhoDoJogo);
            if (dimowaModLoaderPath == "")
            {
                dimowaModLoaderPath = ModManager.GetFilePathInDirectory("DIMOWAModLoader.dll", caminhoDaPastaDeMods);
                File.Copy(dimowaModLoaderPath, ModManager.GetDirectoryInDirectory("Managed", caminhoDoJogo) + "\\DIMOWAModLoader.dll");
            }
            
            DIMOWALoaderInstaller = new DIMOWALoaderInstaller(caminhoDoJogo, caminhoDaPastaDeMods);
            ModManager = new ModManager(caminhoDoJogo, listOfMods, unityEngine);
            modMowaps = listOfMods.ToArray();
        }
        public string GetModDllFileName(int index)
        {
            if (index >= indexOfTheLoader)
                index += 1;
            return dllsDosMods[index];
        }
        public string GetLoaderDllFilePath()
        {
            return dimowaModLoaderPath;
        }
        public MOWAP GetModMOWAP(int index)
        {
            return modMowaps[index];
        }

        public static string CopyAndGetImportantFileToManaged(string file, string gameFolder)
        {
            string filePath = "";
            try
            {
                filePath = ModManager.GetFilePathInDirectory(file, gameFolder);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(FileNotFoundException))
                {
                    try
                    {
                        filePath = ModManager.GetFilePathInDirectory(file, Form1.programPath);
                        File.Copy(filePath, ModManager.GetDirectoryInDirectory("Managed", gameFolder) + "\\" + file);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(FileNotFoundException))
                            ConsoleWindowHelper.FatalException("The file " + file + " isn't inside the game's folder nor the program folder. Check if you got all the files from the download.");
                        else if (e.GetType() == typeof(DirectoryNotFoundException))
                            ConsoleWindowHelper.FatalException("Couldn't find the Managed sub folder inside the game's folder, make sure you gave the correct folder by deleting config.json or by editing config.json");
                        else
                            ConsoleWindowHelper.FatalException("An unexpected exception occured: " + e.Message + "  " + ex.StackTrace + "  " + ex.Source);
                        return "";
                    }
                }
            }
            return filePath;
        }
    }
}
