using System;
using System.Collections.Generic;
using System.Linq;
using CAMOWA;
using System.Reflection;
using dnpatch;
using System.IO;

namespace IMOWA
{
    class DIMOWA // Desinstalador e Instalador de Mods do Outer Wilds Alpha
    {
        static bool IsTheDllAMod(string dllName)
        {
            //Se alguem colocar na pasta /mods algum desses dlls """sem querer""" não identifica-los só por segurança
            return !(dllName == "0Harmony.dll" || dllName == "IMOWAAnotations.dll" || dllName == "dnlib.dll" || dllName == "dnpatch.dll" ||
                dllName == "HarmonyDnet2Fixes.dll" || dllName == "Mono.Security.dll" || dllName == "Assembly-CSharp.dll" ||
                dllName == "Assembly-CSharp-firstpass.dll" || dllName == "Assembly-UnityScript.dll" || dllName == "Assembly-UnityScript-firstpass.dll" ||
                dllName == "Boo.Lang.dll" || dllName == "DecalSystem.Runtime.dll" || dllName == "mscorlib.dll" || dllName == "System.Core.dll" ||
                dllName == "System.dll" || dllName == "System.Xml.dll" || dllName == "UnityEngine.dll" || dllName == "UnityEngine.dll" ||
                dllName == "UnityEngine.Lang.dll");
        }
        
        static void Main(string[] args)
        {

            List<MOWAP> listOfMods = new List<MOWAP>();

            string caminhoDessePrograma = Directory.GetCurrentDirectory();

            if (!Directory.Exists(caminhoDessePrograma + @"\mods"))
                Directory.CreateDirectory(caminhoDessePrograma + @"\mods");

            string[] todosOsDlls = Directory.GetFiles(caminhoDessePrograma + @"\mods\", "*.dll");



            List<string> dllsDosMods = new List<string>();


            for (int i = 0; i < todosOsDlls.Length; i++)
            {

                if (IsTheDllAMod(todosOsDlls[i].Remove(0, caminhoDessePrograma.Count() + 6)))
                {
                    //Ver quais dlls foram aceitos como possiveis mods
                    //Console.WriteLine("Dll:");
                    //Console.WriteLine(todosOsDlls[i]);
                    dllsDosMods.Add(todosOsDlls[i]);
                }
            }


            //Se isso aki n é grande, eu não sei o que é \/

            //Ir em cada dll
            foreach (string filePath in dllsDosMods)
            {
                //Carregar as classes em cada dll
                Type[] classesNoDll = Assembly.LoadFrom(filePath).GetTypes();
                //Ir em cada classe 
                foreach (Type classeDoDll in classesNoDll)
                {
                    //Ir em cada método de cada classe
                    foreach (MethodInfo mInfo in classeDoDll.GetMethods())
                    {
                        //Ir em cada atributo de cada método
                        foreach (Attribute attr in
                            Attribute.GetCustomAttributes(mInfo))
                        {
                            // Ver se é o atributo que queremos
                            if (attr.GetType() == typeof(IMOWAModInnit))
                            {
                                //Montar MOWAP e adicionar a lista de mods
                                listOfMods.Add(new MOWAP()
                                {
                                    ModType = classeDoDll,

                                    ModInnitMethod = mInfo.Name,

                                    ModName = ((IMOWAModInnit)attr).modName,

                                    ModLoadingPlace = ((IMOWAModInnit)attr).modLoadingPlace,

                                    ModPriority = ((IMOWAModInnit)attr).modPriority,

                                    DllFileName = filePath.Remove(0, caminhoDessePrograma.Count() + 6),

                                });
                                break;
                            }
                        }

                        //Se achar o método não tem o porque continuar procurando, só se aceita um por vez
                        if (listOfMods.Count > 0) // Para não dar erro de que não tem elemento na lista
                            if (listOfMods.Last().ModInnitMethod == mInfo.Name)
                                break;
                    }
                }
            }




            Patcher patcher = new Patcher("DIMOWAModLoader.dll");
            MOWAPMaker mowapMaker = new MOWAPMaker();

            int amountOfMods = listOfMods.Count;

            Target[] listOfModTarget = new Target[amountOfMods];

            for (int i = 0; i < amountOfMods; i++)
            {
                listOfModTarget[i] = mowapMaker.MOWAPModInnitTarget(listOfMods[i], patcher);
            }


            int[] indexOfModInnits = new int[amountOfMods];


            bool shouldTheProgamBeOpen = true;
            string resposta = "listademods";
            int indexofmod = -1;

            while (shouldTheProgamBeOpen) // Rai pf não me mata porcausa disso ;-;, isso aki é realmente uma abominação \/
            {

                //Carrega o menu com a lista de mods
                if ((resposta == "listademods" || resposta == "recarregar" || resposta == "refresh" || resposta == "menu" || resposta == "r") && indexofmod < 0)
                {
                    Console.Clear();
                    Console.WriteLine(" --- DIMOWA v.1.0.3 --- ");
                    for (int i = 0; i < listOfMods.Count; i++)
                    {
                        indexOfModInnits[i] = ModInstallingHandler.CheckIfModInstalled(listOfModTarget[i], patcher);
                    }

                    Console.WriteLine($" {amountOfMods} Mods Detectados/ Detected Mods: " + '\n');
                    for (int i = 0; i < amountOfMods; i++)
                    {
                        Console.WriteLine('\t' + $"{i}  * {listOfMods[i].ModName} - Status: " + ((indexOfModInnits[i] > -1) ? "Instalado / Installed" : "Não Instalado / Not Installed"));

                    }
                    Console.WriteLine('\n' + "Escreva / Write  [instalar todos / it | install all / ia] para instalar todos os mods / to install all the mods");
                    Console.WriteLine("Ou / Or escreva / write  [desinstalar todos/dt | uninstall all / ua] para desinstalar todos os mods / to uninstall all the mods");
                    Console.WriteLine("Ou tambem escolha um mod usando o numero da sequencia para mais opcoes");
                    Console.WriteLine("Or you can too choose a mod using the number of the sequence for more options");
                    Console.WriteLine("E se voce quer sair do programa digite [sair / s]");
                    Console.WriteLine("And if you want to close the program, write [close / c]");


                }
                //instalar todos os mods
                else if ((resposta == "instalar todos" || resposta == "it" || resposta == "install all" || resposta == "ia") && indexofmod < 0 && amountOfMods > 0)
                {
                    if (amountOfMods < 0)
                    {
                        Console.WriteLine("Nao ha mods para instalar");
                        Console.WriteLine("There are no mods to install");
                    }
                    else
                    {
                        Console.Clear();

                        for (int i = 0; i < listOfModTarget.Length; i++)
                        {
                            if (indexOfModInnits[i] < 0)
                            {
                                Console.WriteLine('\n' + $"Instalando / Installing {listOfMods[i].ModName} . . .");
                                ModInstallingHandler.InstallOrUninstallMod(listOfModTarget[i], patcher, indexOfModInnits[i], listOfMods[i].DllFileName, caminhoDessePrograma);
                            }
                        }

                        patcher.Save(false);
                        patcher = new Patcher("DIMOWAModLoader.dll");

                        Console.WriteLine('\n' + "Todos os mods estao agora instalados");

                        Console.WriteLine('\n' + "Digite [recarregar / r / menu] para recarregar o menu");
                        Console.WriteLine("Write [refresh / r / menu] to reload the menu");
                        Console.WriteLine("E se voce quer sair do programa digite [sair / s]");
                        Console.WriteLine("And if you want to close the program, write [close / c]");
                        indexofmod = -1;
                    }
                }

                //desinstalar todos os mods
                else if ((resposta == "desinstalar todos" || resposta == "dt" || resposta == "uninstall all" || resposta == "ua") && indexofmod < 0)
                {
                    if (amountOfMods < 0)
                    {
                        Console.WriteLine("Nao ha mods para desinstalar");
                        Console.WriteLine("There are no mods to uninstall");
                    }
                    else
                    {
                        Console.Clear();



                        for (int i = 0; i < listOfModTarget.Length; i++)
                        {
                            if (indexOfModInnits[i] > -1)
                            {
                                Console.WriteLine('\n' + $"Desinstalando / Unistalling {listOfMods[i].ModName} . . .");
                                ModInstallingHandler.InstallOrUninstallMod(listOfModTarget[i], patcher, indexOfModInnits[i], listOfMods[i].DllFileName, caminhoDessePrograma);
                            }
                        }
                        patcher.Save(false);
                        patcher = new Patcher("DIMOWAModLoader.dll");

                        Console.WriteLine('\n' + "Todos os mods estao agora desinstalados");

                        Console.WriteLine('\n' + "Digite [recarregar / r / menu] para recarregar o menu");
                        Console.WriteLine("Write [refresh / r / menu] to reload the menu");
                        Console.WriteLine("E se voce quer sair do programa digite [sair / s]");
                        Console.WriteLine("And if you want to close the program, write [close / c]");
                        indexofmod = -1;
                    }
                }

                //Se a resposta da pessoa for uma dessas e um mod tiver sido escolhido, entao instalar ou desinstalar
                else if ((resposta == "sim" || resposta == "s" || resposta == "yes" || resposta == "y") && indexofmod > -1)
                {

                    ModInstallingHandler.InstallOrUninstallMod(listOfModTarget[indexofmod], patcher, indexOfModInnits[indexofmod], listOfMods[indexofmod].DllFileName, caminhoDessePrograma);
                    patcher.Save(false);
                    patcher = new Patcher("DIMOWAModLoader.dll");

                    Console.WriteLine('\n' + "Digite [recarregar / r / menu] para recarregar o menu");
                    Console.WriteLine("Write [refresh / r / menu] to reload the menu");
                    Console.WriteLine("E se voce quer sair do programa digite [sair / s]");
                    Console.WriteLine("And if you want to close the program, write [close / c]");
                    indexofmod = -1;
                }
                //Se for n fazer nada
                else if ((resposta == "nao" || resposta == "n" || resposta == "no") && indexofmod > -1)
                {
                    
                    Console.WriteLine('\n' + "Digite [recarregar / r / menu] para recarregar o menu");
                    Console.WriteLine("Write [refresh / r / menu] to reload the menu");
                    Console.WriteLine("E se voce quer sair do programa digite [sair / s]");
                    Console.WriteLine("And if you want to close the program, write [close / c]");
                    indexofmod = -1;
                }
                else if ((resposta == "s" || resposta == "sair" || resposta == "c" || resposta == "close") && indexofmod < 0)
                {

                    shouldTheProgamBeOpen = false;
                }

                else
                {
                    try
                    {
                        if (indexofmod < 0)
                        {
                            indexofmod = Convert.ToInt32(resposta);

                            if (indexofmod < amountOfMods && indexofmod > -1)
                            {
                                Console.Clear();

                                bool isModinstalled = indexOfModInnits[indexofmod] > -1;

                                Console.WriteLine('\n' + $"Mod {listOfMods[indexofmod].ModName} is " + (isModinstalled ? "" : "not ") + "installed. Would you like to " + (isModinstalled ? "unnistall " : "install ") + "it?");
                                Console.WriteLine($"O mod {listOfMods[indexofmod].ModName} " + (isModinstalled ? "" : "não ") + "está instalado. Voce gostaria de " + (isModinstalled ? "desinstalar " : "instalar ") + "ele?");
                                Console.WriteLine('\n' + "For Yes -> yes / y | For No -> no / n");
                                Console.WriteLine("Para Sim -> sim / s | Para Nao -> nao / n");
                            }
                            else
                            {
                                Console.WriteLine("Esse numero nao e um index da lista");
                                Console.WriteLine("That number is not an index of the list");
                                indexofmod = -1;
                            }

                        }
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("O valor do numero e acima de qualquer int (32)");
                        Console.WriteLine("The value of the number is superior to any int (32)");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("O escrito nao e um comando valido");
                        Console.WriteLine("That's not a valid command");
                    }
                }

                if (shouldTheProgamBeOpen)
                    resposta = Console.ReadLine();


            }

        }
    }
}