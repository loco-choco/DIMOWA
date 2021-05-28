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
            string caminhoDoJogo = "" ,caminhoDaPastaDeMods = "", caminhoDaPastaDeManifestos =  "";

            string[] possibleConfig = Directory.GetFiles(Directory.GetCurrentDirectory(), "*config.json");
            if (possibleConfig.Length < 1)
            {
                Console.WriteLine("Caminho do jogo");
                caminhoDoJogo = Console.ReadLine();
                Console.WriteLine("Caminho da pasta de mods");
                caminhoDaPastaDeMods = Console.ReadLine();
                Console.WriteLine("Caminho dos manifestos");
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
			
            Type imowaModInnitType = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("CAMOWA.dll",caminhoDoJogo)).GetType("CAMOWA.IMOWAModInnit");
            Assembly unityEngine = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("UnityEngine.dll", caminhoDoJogo));
            
            List<MOWAP> listOfMods = new List<MOWAP>();
            foreach (string fileName in new HashSet<string>(dllsDosMods))
                listOfMods.Add(ModManager.GenerateModMOWAPsFromDll(caminhoDaPastaDeMods + @"\" + fileName, imowaModInnitType));


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
                Console.WriteLine(listOfMods[0].ModName + " esta ativado? " + modStatus[0]);
                Console.WriteLine("Index do mod");
                string str = Console.ReadLine();
                try
                {
                    if (str == "close")
                        break;
                    int index = Convert.ToInt32(str);
                    Console.WriteLine("Ativar/Desativar " + listOfMods[index].ModName);
                    str = Console.ReadLine();
                    if (str.ToLower() == "ativar")
                        modStatus[index] = true;
                    else if (str.ToLower() == "desativar")
                        modStatus[index] = false;
                }
                catch
                {
                    Console.WriteLine("O valor dado nao eh numerico ou eh invalido (ex: negativo ou acima do tamanho da lista)");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Efetuar as modificacoes no jogo?(sim/nao)");
            string s = Console.ReadLine();
            if (s.ToLower() == "sim")
            {
                for (int i = 0; i < modStatus.Length; i++)
                {
                    if (modStatus[i])//Instalar == true
                    {
                        if (modManager.InstallMod(i))
                            Console.WriteLine(listOfMods[i].ModName + " foi instalado com sucesso");
                    }
                    else
                    {
                        if (modManager.UninstallMod(i))
                            Console.WriteLine(listOfMods[i].ModName + " foi desinstalado com sucesso");
                    }
                }

                modManager.SaveModifications();
                Console.WriteLine("Todas as modificacoes foram efetuadas e salvas");
            }

            Console.WriteLine("Pressione ENTER para fechar a janela. . .");
            Console.ReadLine();
        }
    }
}
