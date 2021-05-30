using System;
using IMOWA;
using dnpatch;
using dnlib.DotNet.Emit;
using System.IO;
using System.Reflection;

namespace DIMOWAIU
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] requiredImowaFiles =
            {
                "CAMOWA.dll",
                "DIMOWAModLoader.dll"
            };
            string[] optionalImowaFiles =
            {
                "0Harmony.dll",
                "HarmonyDnet2Fixes.dll"
            };

            Console.Title = "DIMOWA Installer and Unistaller";
            Console.WriteLine("DIMOWA Installer and Unistaller");
            Console.Write("Escreva o caminho da pasta do jogo / Write the path for the game folder: ");
            string gameFolderPath = Console.ReadLine();

            string assemblyPath = ModManager.GetFilePathInDirectory("Assembly-CSharp.dll", gameFolderPath);
            Patcher patcher = new Patcher(assemblyPath);

            string managedFolder = assemblyPath.Remove(assemblyPath.Length - "Assembly-CSharp.dll".Length - 1, "Assembly-CSharp.dll".Length + 1);
            if(Directory.GetFiles(managedFolder, "DIMOWAModLoader.dll").Length == 0)
            {
                Console.Write($"For you to be able to run the installer the file DIMOWAModLoader.dll needs to be in the folder {managedFolder}, do you want to copy it there?(yes/no) ");
                string a = Console.ReadLine();
                if (a == "yes")
                    File.Copy(Directory.GetCurrentDirectory() + "\\DIMOWAModLoader.dll", managedFolder + "\\DIMOWAModLoader.dll");
                else
                {
                    Console.WriteLine("Pressione ENTER para fechar o programa");
                    Console.WriteLine("Press ENTER to close the program");
                    Console.ReadLine();
                    return;
                }

            }

            Assembly unityEngineDebug = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("UnityEngine.dll", gameFolderPath));
            Assembly levelLoader = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("DIMOWAModLoader.dll", gameFolderPath));// ("LevelLoaderHandler");

            Instruction[] modLoaderInstructions = {
                Instruction.Create(OpCodes.Ldstr   ,  "DIMOWA Level Loader Handler foi iniciado | was started"),
                Instruction.Create(OpCodes.Call, patcher.BuildCall(unityEngineDebug.GetType("UnityEngine.Debug"), "Log" , typeof(void) , new[]{ typeof(object) })),
                Instruction.Create(OpCodes.Ldstr   ,  "TitleMenu - Awake"),
                Instruction.Create(OpCodes.Call, patcher.BuildCall(levelLoader.GetType("DIMOWAModLoader.LevelLoaderHandler"), "LevelLoaderInnit", typeof(void), new[] { typeof(string) }))
            };

            Target ModLoaderInnitTarget = new Target
            {
                Namespace = "",
                Class = "TitleScreenMenu",
                Method = "Awake",
                Indices = new int[] { 0,1,2,3},
                Instructions = modLoaderInstructions,
                InsertInstructions = true,
            };
			
            string managedPath = ModManager.GetFilePathInDirectory("UnityEngine.dll", gameFolderPath);
            managedPath = managedPath.Remove(managedPath.Length - "UnityEngine.dll".Length -1);
            Console.WriteLine($"(All these files are supposed to be copied to {managedPath})");
            Console.WriteLine("These files  are required for mods and for the manager to work:");
            foreach (string s in requiredImowaFiles)
                Console.WriteLine("    " + s + " - status - " + (IsTheFileThere(s, managedPath) ? "File is in the folder" : "File is not in the folder"));
            Console.WriteLine("These files  aren't required for the manager to work, but some mods need them:");
            foreach (string s in optionalImowaFiles)
                Console.WriteLine("    " + s + " - status - " + (IsTheFileThere(s, managedPath) ? "File is in the folder" : "File is not in the folder"));
            Console.WriteLine();

            int isModInstalled = IMOWA.IMOWA.IndexOfInstalledMod(ModLoaderInnitTarget, patcher);
            bool programShouldBeOpen = true;
            if (isModInstalled >= 0)
            {

                Console.WriteLine("DIMOWA ja esta instalado, voce gostaria desinstala-lo? s / n");
                Console.WriteLine("DIMOWA is already installed, would you like to uninstall it? y / n");
                while (programShouldBeOpen)
                {
                    string resposta = Console.ReadLine();

                    if (resposta == "y" || resposta == "s")
                    {
                        Console.WriteLine('\n' + "Desinstalando| Uninstalling . . .");
                        programShouldBeOpen = false;
                        bool modIsUnistalled = IMOWA.IMOWA.UninstallMod(ModLoaderInnitTarget, patcher, isModInstalled);
                        patcher.Save(false);

                        if (modIsUnistalled)
                        {
                            Console.WriteLine("O mod foi desinstalado com sucesso");
                            Console.WriteLine("The mod was sucesffuly unistalled");

                            
                        }
                        else
                        {
                            Console.WriteLine("O mod não foi desinstalado");
                            Console.WriteLine("The mod was not uninstalled");
                        }
                    }
                    else if (resposta == "n")
                        programShouldBeOpen = false;
                    
                }

            }
            else
            {
                Console.WriteLine("DIMOWA nao esta instalado, deseja instala-lo? s / n");
                Console.WriteLine("DIMOWA is not installed, would you like to install it? y / n");
                while (programShouldBeOpen)
                {
                    string resposta = Console.ReadLine();

                    if (resposta == "y" || resposta == "s")
                    {
                        Console.WriteLine($"Instalando | Instaling DIMOWA");

                        bool modIsIstalled = IMOWA.IMOWA.InstallMod(ModLoaderInnitTarget, patcher);
                        patcher.Save(false);
                        programShouldBeOpen = false;
                        if (modIsIstalled)
                        {
                            Console.WriteLine("DIMOWA foi instalado com sucesso");
                            Console.WriteLine("DIMOWA was sucesffuly installed");
                        }
                        else
                        {
                            Console.WriteLine("DIMOWA não foi instalado");
                            Console.WriteLine("DIMOWA was not installed");
                        }
                    }
                    else if (resposta == "n")
                        programShouldBeOpen = false;
                }
            }
            Console.WriteLine("Pressione ENTER para fechar o programa");
            Console.WriteLine("Press ENTER to close the program");
            Console.ReadLine();
        }

        static bool IsTheFileThere(string fileName, string path)
        {
            return Directory.GetFiles(path, fileName).Length > 0;
        }
    }
}
