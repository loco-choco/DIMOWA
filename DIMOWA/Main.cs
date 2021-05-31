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
            string caminhoDoJogo = "", caminhoDaPastaDeMods = "", caminhoDaPastaDeManifestos = "";

            string[] possibleConfig = Directory.GetFiles(Directory.GetCurrentDirectory(), "*config.json");
            if (possibleConfig.Length < 1)
            {
                Console.WriteLine("Game Path (can be any path that has the 'Managed' folder inside it)");
                caminhoDoJogo = Console.ReadLine();
                Console.WriteLine("Mod folder path");
                caminhoDaPastaDeMods = Console.ReadLine();
                Console.WriteLine("Mod's manifest path (can be same path of the mod folder)");
                caminhoDaPastaDeManifestos = Console.ReadLine();

                StreamWriter writer = new StreamWriter(File.Create(Directory.GetCurrentDirectory() + "/config.json"));
                //Descobrir maneira de colocar a char " dentro da string de maneira, "mais bela"
                string json = "{\n  " + (char)34 + "pastaDoJogo" + (char)34 + ": " + (char)34 + caminhoDoJogo + (char)34
                    + ",\n  " + (char)34 + "pastaDeMods" + (char)34 + ": " + (char)34 + caminhoDaPastaDeMods + (char)34
                    + ",\n  " + (char)34 + "pastaDeManifestos" + (char)34 + ": " + (char)34 + caminhoDaPastaDeManifestos + (char)34
                    + "\n}";
                writer.Write(json);
                writer.Flush();
                writer.Close();
            }
            else
            {
                JsonReader json = new JsonReader(possibleConfig[0]);
                caminhoDoJogo = json.GetJsonElementFromRoot("pastaDoJogo").Value;
                caminhoDaPastaDeMods = json.GetJsonElementFromRoot("pastaDeMods").Value;
                caminhoDaPastaDeManifestos = json.GetJsonElementFromRoot("pastaDeManifestos").Value;
            }


            //novo formato 
            string[] todosOsJsons = Directory.GetFiles(caminhoDaPastaDeManifestos, "*manifest.json");
            string[] dllsDosMods = new string[todosOsJsons.Length];

            for (int i = 0; i < todosOsJsons.Length; i++)
            {
                JsonReader json = new JsonReader(todosOsJsons[i]);
                dllsDosMods[i] = json.GetJsonElementFromRoot("filename").Value;
            }

            Type imowaModInnitType = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("CAMOWA.dll", caminhoDoJogo)).GetType("CAMOWA.IMOWAModInnit");
            Assembly unityEngine = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("UnityEngine.dll", caminhoDoJogo));

            List<MOWAP> listOfMods = new List<MOWAP>();
            ModDataGatherer gatherer = new ModDataGatherer(caminhoDaPastaDeMods, caminhoDoJogo);
            foreach (string fileName in new HashSet<string>(dllsDosMods))
                listOfMods.Add(gatherer.GenerateModMOWAPFromDll(fileName, imowaModInnitType));

            //A parte interesante, agora muito mais facil de usar:tm:
            Console.Title = "IMOWA 1.4";
            ModManager modManager = new ModManager(caminhoDoJogo, listOfMods, unityEngine);
            bool[] modStatus = new bool[listOfMods.Count];
            for (int i = 0; i < modStatus.Length; i++)
                modStatus[i] = modManager.IsTheModInstalled(i);
            while (true)
            {
                Console.Clear();
                Console.WriteLine($" {modManager.AmountOfMods()} Mods ");
                Console.WriteLine("Mod index | Mod Name | Mod Status");
                for(int i =0;i<modStatus.Length;i++)
                    Console.WriteLine($"{i} - Is " + listOfMods[i].ModName + " enabled? " + (modStatus[i]?"yes":"no"));

                Console.WriteLine("Write the mod index to have more options, and 'close' to close the program");
                string str = Console.ReadLine();
                try
                {
                    if (str == "close")
                        break;
                    int index = Convert.ToInt32(str);
                    Console.WriteLine("Do you want to enable or disable " + listOfMods[index].ModName + "? (enable/disable)");
                    str = Console.ReadLine();
                    if (str.ToLower() == "enable")
                        modStatus[index] = true;
                    else if (str.ToLower() == "disable")
                        modStatus[index] = false;
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
                for (int i = 0; i < modStatus.Length; i++)
                {
                    string managedDirectoryPath = Directory.GetDirectories(caminhoDoJogo, "*Managed", SearchOption.AllDirectories)[0];
                    string arquivoDoMod = listOfMods[i].DllFilePath.Remove(0, caminhoDaPastaDeMods.Length + 2);

                    if (modStatus[i])//Instalar == true
                    {
                        if (modManager.InstallMod(i))
                        {
                            Console.WriteLine(listOfMods[i].ModName + " has been successfully installed");
                            if (Directory.GetFiles(managedDirectoryPath, arquivoDoMod).Length == 0)
                            {
                                Console.WriteLine("File wasn't in the folder, copying it there");
                                File.Copy(listOfMods[i].DllFilePath, managedDirectoryPath + '/' + arquivoDoMod);
                            }
                        }
                    }
                    else
                    {
                        if (modManager.UninstallMod(i))
                        {
                            Console.WriteLine(listOfMods[i].ModName + " has been successfully uninstalled");
                            if (Directory.GetFiles(managedDirectoryPath, arquivoDoMod).Length > 0)
                            {
                                Console.WriteLine("File is still in the folder, removing it from there");
                                File.Delete(managedDirectoryPath + '/' + arquivoDoMod);
                            }
                        }
                    }
                }

                modManager.SaveModifications();
                Console.WriteLine("All the changes have been performed and saved");
            }

            Console.WriteLine("Press ENTER to close the window. . .");
            Console.ReadLine();
        }
    }
}
