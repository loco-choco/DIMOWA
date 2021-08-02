using System;
using System.Reflection;
using UnityEngine;

namespace DIMOWAModLoader.Mod_Loading
{
    class ModAssemblyLoader
    {
        private string modFolder;
        private string gameFolder;

        public ModAssemblyLoader(string modFolder, string gameFolder)
        {
            this.modFolder = modFolder;
            this.gameFolder = gameFolder;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public MOWAP GenerateModMOWAPFromDll(string file, Type imowaModInnitType)
        {
            MOWAP mowap = new MOWAP();
            string path = DirectorySearchTools.GetFilePathInDirectory(file, modFolder);
            Assembly modAssembly = Assembly.LoadFrom(path);
            Type[] classesNoDll = modAssembly.GetTypes();
            bool gotAllTheModInfo = false;
            //Ir em cada classe 
            foreach (Type classeDoDll in classesNoDll)
            {
                //Ir em cada método de cada classe
                foreach (MethodInfo mInfo in classeDoDll.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    //Ir em cada atributo de cada método
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo, imowaModInnitType))
                    {
                        int modLoadingPlace = 0, modPriority = 0;
                        FieldInfo[] fields = attr.GetType().GetFields();
                        
                        foreach (FieldInfo f in fields)
                        {
                            if (f.Name == "modLoadingPlace")
                                modLoadingPlace = (int)f.GetValue(attr);
                            else if (f.Name == "modPriority")
                                modPriority = (int)f.GetValue(attr);
                        }
                        mowap = new MOWAP()
                        {
                            ModInnitMethod = mInfo,
                            ModLoadingPlace = modLoadingPlace,
                            ModPriority = modPriority
                        };
                        gotAllTheModInfo = true;
                        break;
                    }
                    if (gotAllTheModInfo)
                        break;
                }
                if (gotAllTheModInfo)
                    break;
            }
            return mowap;
        }

        static public string[] ParseReferences(AssemblyName[] assemblies)
        {
            string[] files = new string[assemblies.Length];
            for (int i = 0; i < assemblies.Length; i++)
                files[i] = assemblies[i].Name + ".dll";

            return files;
        }

        //https://weblog.west-wind.com/posts/2016/Dec/12/Loading-NET-Assemblies-out-of-Seperate-Folders
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
                return null;
            Assembly assembly = LINQExtensions.FirstOrDefault(AppDomain.CurrentDomain.GetAssemblies(), a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;
            string filename = (args.Name.Split(',')[0] + ".dll").ToLower(), filePath = "";

            string[] possiblePathsForTheAssembly = { modFolder, gameFolder };
            foreach (string s in possiblePathsForTheAssembly)
            {
                try
                {
                    filePath = DirectorySearchTools.GetFilePathInDirectory(filename, s);
                    break;
                }
                catch { }
            }
            try
            {
                if (filePath != "")
                    return Assembly.LoadFrom(filePath);

                Debug.Log("Couldn't find the reference: " + filename);
                return null;
            }
            catch
            {
                Debug.Log("Couldn't load the reference in : " + filePath);
                return null;
            }

        }
    }
}
