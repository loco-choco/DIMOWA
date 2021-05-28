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
            string programPath = Directory.GetCurrentDirectory();

            Console.Title = "DIMOWA Installer and Unistaller";
            Console.WriteLine("DIMOWA Installer and Unistaller");
            Patcher patcher = new Patcher(programPath + @"\Assembly-CSharp.dll");

            Instruction[] modLoaderInstructions = new Instruction[]
            {

                Instruction.Create(OpCodes.Ldstr   ,  "DIMOWA Level Loader Handler foi iniciado | was started"),
                Instruction.Create(OpCodes.Call, patcher.BuildCall(Assembly.LoadFrom(programPath + @"\UnityEngine.dll").GetType("Debug"), "Log" , typeof(void) , new[]{ typeof(object) })),
                Instruction.Create(OpCodes.Ldstr   ,  "TitleMenu - Awake"),
                Instruction.Create(OpCodes.Call, patcher.BuildCall(typeof(DIMOWAModLoader.LevelLoaderHandler), "LevelLoaderInnit", typeof(void), new[] { typeof(string) }))
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
                        int modIsUnistalled = IMOWA.IMOWA.UninstallMod(ModLoaderInnitTarget, patcher, isModInstalled);
                        patcher.Save(false);

                        if (modIsUnistalled == 0)
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




                        int modIsIstalled = IMOWA.IMOWA.InstallMod(ModLoaderInnitTarget, patcher);
                        patcher.Save(false);
                        programShouldBeOpen = false;
                        if (modIsIstalled == 0)
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
    }
}
