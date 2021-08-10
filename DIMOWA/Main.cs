using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace IMOWA
{
    class MainLoop
    {
        static void Main(string[] args)
        {
            Console.Title = "IMOWA 1.4";

            string caminhoDoJogo = "", caminhoDaPastaDeMods = "", caminhoDaPastaDeManifestos = "";

            string[] possibleConfig = Directory.GetFiles(Directory.GetCurrentDirectory(), "*config.json");
            if (possibleConfig.Length < 1)
            {
                Console.WriteLine("Game Path (can be any path that has the 'Managed' folder inside it)");
                caminhoDoJogo = Console.ReadLine();
                Console.WriteLine("Mod folder path");
                caminhoDaPastaDeMods = Console.ReadLine();
                caminhoDaPastaDeManifestos = caminhoDaPastaDeMods;

                StreamWriter writer = new StreamWriter(File.Create(Directory.GetCurrentDirectory() + "/config.json"));
                //Descobrir maneira de colocar a char " dentro da string de maneira, "mais bela"
                string json = "{\n  " + (char)34 + "gameFolder" + (char)34 + ": " + (char)34 + caminhoDoJogo.Replace("\\", "/") + (char)34
                    + ",\n  " + (char)34 + "modsFolder" + (char)34 + ": " + (char)34 + caminhoDaPastaDeMods.Replace("\\", "/") + (char)34
                    + "\n}";
                writer.Write(json);
                writer.Flush();
                writer.Close();
            }
            else
            {
                try
                {
                    JsonReader json = new JsonReader(possibleConfig[0]);
                    caminhoDoJogo = json.GetJsonElementFromRoot("gameFolder").Value;
                    caminhoDaPastaDeMods = json.GetJsonElementFromRoot("modsFolder").Value;
                    caminhoDaPastaDeManifestos = caminhoDaPastaDeMods;
                }

                catch(Exception ex)
                {
                    Console.WriteLine("Something went wrong while reading the config.json file, try deleting it and running the program again");
                    Console.WriteLine("Exception Message: " + ex.Message + "  " + ex.StackTrace + "  " + ex.Source);
                    Console.ReadLine();
                    return;
                }
            }
            int indexOfTheLoader = -1;

            string[] todosOsJsons = Directory.GetFiles(caminhoDaPastaDeManifestos, "*manifest.json", SearchOption.AllDirectories);
            string[] dllsDosMods = new string[todosOsJsons.Length];

            for (int i = 0; i < todosOsJsons.Length; i++)
            {
                JsonReader json = new JsonReader(todosOsJsons[i]);
                dllsDosMods[i] = json.GetJsonElementFromRoot("filename").Value;
                if (dllsDosMods[i] == "DIMOWAModLoader.dll")
                {

                    if(indexOfTheLoader<0)
                        indexOfTheLoader = i;
                }
            }
            if (indexOfTheLoader < 0)
            {
                Console.WriteLine("DIMOWAModLoader.dll needs to be inside the mods folder with its manifest.json, make sure that it is in there.");
                Console.ReadLine();
                return;
            }

            string camowaPath = CopyAndGetImportantFileToManaged("CAMOWA.dll", caminhoDoJogo);

            Type imowaModInnitType = Assembly.LoadFrom(camowaPath).GetType("CAMOWA.IMOWAModInnit");
            Assembly unityEngine = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("UnityEngine.dll", caminhoDoJogo));

            List<MOWAP> listOfMods = new List<MOWAP>();
            ModDataGatherer gatherer = new ModDataGatherer(caminhoDaPastaDeMods, caminhoDoJogo);

            foreach (string fileName in new HashSet<string>(dllsDosMods))

                if (fileName != dllsDosMods[indexOfTheLoader])
                    listOfMods.Add(gatherer.GenerateModMOWAPFromDll(fileName, imowaModInnitType));

            string dimowaModLoaderPath = CopyAndGetImportantFileToManaged("DIMOWAModLoader.dll", caminhoDoJogo);
            if (dimowaModLoaderPath == "")
            {
                dimowaModLoaderPath = ModManager.GetFilePathInDirectory("DIMOWAModLoader.dll", caminhoDaPastaDeMods);
                File.Copy(dimowaModLoaderPath, ModManager.GetDirectoryInDirectory("Managed", caminhoDoJogo) + "\\DIMOWAModLoader.dll");
            }

            //criar o patcher para o loader e verificar se ele esta instalado
            DIMOWALoaderInstaller loaderInstaller = new DIMOWALoaderInstaller(caminhoDoJogo, caminhoDaPastaDeMods);
            bool loaderStatus = loaderInstaller.IsLoaderInstalled;


            ModManager modManager = new ModManager(caminhoDoJogo, listOfMods, unityEngine);//Impedir selecionar os mods até que o loader esteja instalado
                                                                                           //fazer a parte de instalar os mods, mas que ele não feche o programa ao salvar


            bool[] modStatus = new bool[listOfMods.Count];
            for (int i = 0; i < modStatus.Length; i++)
                modStatus[i] = modManager.IsTheModInstalled(i);
            while (true)
            {
                Console.Clear();
                Console.WriteLine($" {modManager.AmountOfMods()} Mods ");
                Console.WriteLine("Mod index | Mod Name | Mod Status");

                Console.WriteLine("0 - Is DIMOWAModLoader enabled? " + (loaderStatus ? "yes" : "no"));

                if(loaderInstaller.IsLoaderInstalled)

                    for (int i = 0; i < modStatus.Length; i++)
                        Console.WriteLine($"{i + 1} - Is " + listOfMods[i].ModName + " enabled? " + (modStatus[i] ? "yes" : "no"));

                Console.WriteLine("Write the mod index to have more options, and 'close' to close the program");
                string str = Console.ReadLine();
                try
                {
                    if (str == "close")
                        break;

                    int index = Convert.ToInt32(str) -1;

                    if (index != -1 && loaderInstaller.IsLoaderInstalled)
                    {
                        Console.WriteLine("Do you want to enable or disable " + listOfMods[index].ModName + "? (enable/disable)");
                        str = Console.ReadLine();
                        if (str.ToLower() == "enable")
                            modStatus[index] = true;
                        else if (str.ToLower() == "disable")
                            modStatus[index] = false;
                    }
                    else if (index == -1)
                    {
                        Console.WriteLine("Do you want to enable or disable DIMOWAModLoader? (enable/disable)");
                        str = Console.ReadLine();
                        if (str.ToLower() == "enable")
                            loaderStatus = true;
                        else if (str.ToLower() == "disable")
                            loaderStatus = false;
                    }
                }
                catch
                {
                    Console.WriteLine("The value you gave isn't numeric or it isn't valid (ie: negative or above the size of the mod list)");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Perform the changes to the game?(yes/no)");
            string s = Console.ReadLine();
            if (s.ToLower() == "yes")
            {
                string managedDirectoryPath = Directory.GetDirectories(caminhoDoJogo, "*Managed", SearchOption.AllDirectories)[0];

                if (loaderInstaller.IsLoaderInstalled)
                {
                    for (int i = 0; i < modStatus.Length; i++)
                    {
                        int ind = i;
                        if (i >= indexOfTheLoader)
                            ind += 1;
                        string arquivoDoMod = dllsDosMods[ind];
                        if (modStatus[i])//Instalar == true
                        {
                            if (modManager.InstallMod(i))
                            {
                                Console.WriteLine(listOfMods[i].ModName + " has been successfully installed");
                                CopyFileAndReferencesToFolder(listOfMods[i], managedDirectoryPath, caminhoDaPastaDeMods, caminhoDoJogo);
                            }
                        }
                        else
                        {
                            if (modManager.UninstallMod(i))
                            {
                                Console.WriteLine(listOfMods[i].ModName + " has been successfully uninstalled");
                                if (Directory.GetFiles(managedDirectoryPath, arquivoDoMod).Length > 0)
                                {
                                    Console.WriteLine("{0} is still in the folder, removing it from there", arquivoDoMod);
                                    File.Delete(managedDirectoryPath + '/' + arquivoDoMod);
                                }
                            }
                        }
                    }
                    modManager.SaveModifications();
                }

                if (loaderStatus)
                {
                    if (loaderInstaller.Install())
                    {
                        Console.WriteLine("DIMOWAModLoader has been successfully installed");

                        CopyFileAndReferencesToFolder(new MOWAP { Dependencies = loaderInstaller.loaderDependecies, DllFilePath = dimowaModLoaderPath }, managedDirectoryPath, caminhoDaPastaDeMods, caminhoDoJogo);

                    }
                }
                else
                {
                    loaderInstaller.Uninstall();
                }

                loaderInstaller.SaveModifications();

                Console.WriteLine("All the changes have been performed and saved");
            }

            Console.WriteLine("Press ENTER to close the window. . .");
            Console.ReadLine();
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
                        filePath = ModManager.GetFilePathInDirectory(file, Directory.GetCurrentDirectory());
                        File.Copy(filePath, ModManager.GetDirectoryInDirectory("Managed", gameFolder) + "\\" + file);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(FileNotFoundException))

                            Console.WriteLine("The file "+file+" isn't inside the game's folder nor the program folder. Check if you got all the files from the download.");

                        else if (e.GetType() == typeof(DirectoryNotFoundException))
                            Console.WriteLine("Couldn't find the Managed sub folder inside the game's folder, make sure you gave the correct folder by deleting config.json or by editing config.json");
                        else
                            Console.WriteLine("An unexpected exception occured: " + e.Message + "  " + ex.StackTrace + "  " + ex.Source);
                        Console.ReadLine();

                        return "";
                    }
                }
            }
            return filePath;
        }

 static void CopyFileAndReferencesToFolder(MOWAP modData, string folderToCopy, params string[] originFolders)
        {
            string fileName = Path.GetFileName(modData.DllFilePath);
            if (Directory.GetFiles(folderToCopy, fileName).Length == 0)
            {
                Console.WriteLine(Path.GetFileName(modData.DllFilePath) + " wasn't in " + folderToCopy + ", copying it there");
                File.Copy(modData.DllFilePath, folderToCopy + '/' + fileName);
            }

            for(int i =0; i < modData.Dependencies.Length; i++)
            {
                if (Directory.GetFiles(folderToCopy, modData.Dependencies[i]).Length == 0)
                {
                    Console.WriteLine("{0} wasn't in the folder, copying it there", modData.Dependencies[i]);
                    string filePath = "";

                    foreach (string origin in originFolders)
                        try
                        {
                            filePath = ModManager.GetFilePathInDirectory(modData.Dependencies[i], origin);

                        }
                        catch { }
                    if (filePath != "")
                        File.Copy(modData.DllFilePath, folderToCopy + '/' + modData.Dependencies[i]);
                    else
                        Console.WriteLine("Couldn't find the file {0}, a dependencie of {1}, in any folder, make sure that it is in there", modData.Dependencies[i], modData.ModName);
                }
            }
        }
    }
}

