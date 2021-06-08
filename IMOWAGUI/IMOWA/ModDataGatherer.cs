using System;
using System.Linq;
using System.Reflection;

namespace IMOWA
{
    class ModDataGatherer
    {
        private string modFolder;
        private string gameFolder;

        public ModDataGatherer(string modFolder, string gameFolder)
        {
            this.modFolder = modFolder;
            this.gameFolder = gameFolder;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public MOWAP GenerateModMOWAPFromDll(string file, Type imowaModInnitType)
        {
            MOWAP mowap = new MOWAP();
            string path = ModManager.GetFilePathInDirectory(file, modFolder);
            Assembly modAssembly = Assembly.LoadFrom(path);
            Type[] classesNoDll = modAssembly.GetTypes();

            //Ir em cada classe 
            foreach (Type classeDoDll in classesNoDll)
            {
                //Ir em cada método de cada classe
                foreach (MethodInfo mInfo in classeDoDll.GetMethods())
                {
                    //Ir em cada atributo de cada método
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        // Ver se é o atributo que queremos
                        if (attr.GetType() == imowaModInnitType)
                        {
                            string modName = "";
                            int modLoadingPlace = 0, modPriority = 0;
                            FieldInfo[] fields = attr.GetType().GetFields();

                            foreach (FieldInfo f in fields)
                            {
                                if (f.Name == "modName")
                                    modName = (string)f.GetValue(attr);
                                else if (f.Name == "modLoadingPlace")
                                    modLoadingPlace = (int)f.GetValue(attr);
                                else if (f.Name == "modPriority")
                                    modPriority = (int)f.GetValue(attr);
                            }

                            mowap = new MOWAP()
                            {
                                ModType = classeDoDll,
                                ModInnitMethod = mInfo.Name,
                                ModName = modName,
                                ModLoadingPlace = modLoadingPlace,
                                ModPriority = modPriority,
                                DllFilePath = path,
                                Dependencies = ParseReferences(modAssembly.GetReferencedAssemblies())
                            };
                            break;
                        }
                    }
                    if (mowap.ModInnitMethod == mInfo.Name)
                        break;
                }
                if (mowap.ModType == classeDoDll)
                    break;
            }
            return mowap;
        }
        static public string[] ParseReferences(AssemblyName[] assemblies)
        {
            string[] files = new string[assemblies.Length];
            for (int i = 0; i < assemblies.Length; i++)
                files[i] = (assemblies[i].Name + ".dll");

            return files;
        }

        //https://weblog.west-wind.com/posts/2016/Dec/12/Loading-NET-Assemblies-out-of-Seperate-Folders
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
                return null;
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;
            string filename = (args.Name.Split(',')[0] + ".dll").ToLower(), filePath = "";

            string[] possiblePathsForTheAssembly = { modFolder, gameFolder };
            foreach (string s in possiblePathsForTheAssembly)
            {
                try
                {
                    filePath = ModManager.GetFilePathInDirectory(filename, s);
                    break;
                }
                catch { }
            }
            try
            {
                if (filePath != "")
                    return Assembly.LoadFrom(filePath);

                Console.WriteLine("Couldn't find the reference: " + filename);
                return null;
            }
            catch
            {
                Console.WriteLine("Couldn't load the reference in : " + filePath);
                return null;
            }

        }
    }
}