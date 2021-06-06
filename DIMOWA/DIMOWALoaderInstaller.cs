using dnlib.DotNet.Emit;
using dnpatch;
using System.Reflection;

namespace IMOWA
{
    public class DIMOWALoaderInstaller
    {
        public bool IsLoaderInstalled { get { return TargetIndex > -1; } set { } }

        private int TargetIndex;
        private Target LoaderTarget;
        private Patcher patcher;
        private string gameFolder;
        public string[] loaderDependecies { get; private set; }

        public DIMOWALoaderInstaller(string gameFolder, string directoryOfLoader)
        {
            this.gameFolder = gameFolder;
            patcher = new Patcher(ModManager.GetFilePathInDirectory("Assembly-CSharp.dll", gameFolder));
            LoaderTarget = CreateLoaderTargetFromPath(directoryOfLoader);
            TargetIndex = IMOWA.IndexOfInstalledMod(LoaderTarget, patcher);
        }
        public bool Install()
        {
            if (TargetIndex > -1)
                return false;
            bool b = IMOWA.InstallMod(LoaderTarget, patcher);
            TargetIndex = IMOWA.IndexOfInstalledMod(LoaderTarget, patcher);
            return b;
        }
        public bool Uninstall()
        {
            if (TargetIndex < 0)
                return false;
            bool b = IMOWA.UninstallMod(LoaderTarget, patcher, TargetIndex);
            TargetIndex = IMOWA.IndexOfInstalledMod(LoaderTarget, patcher);
            return b;
        }

        public void SaveModifications()
        {
            patcher.Save(false);
        }
        public void ResetLoaderInstaller()
        {
            patcher = new Patcher(ModManager.GetFilePathInDirectory("Assembly-CSharp.dll", gameFolder));
        }

        private Target CreateLoaderTargetFromPath(string folder)
        {
            Assembly unityEngineAssembly = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("UnityEngine.dll", gameFolder));
            Assembly levelLoader = Assembly.LoadFrom(ModManager.GetFilePathInDirectory("DIMOWAModLoader.dll", folder));// ("LevelLoaderHandler");

            Instruction[] modLoaderInstructions = {
                Instruction.Create(OpCodes.Ldstr   ,  "DIMOWA Level Loader Handler foi iniciado | was started"),
                Instruction.Create(OpCodes.Call, patcher.BuildCall(unityEngineAssembly.GetType("UnityEngine.Debug"), "Log" , typeof(void) , new[]{ typeof(object) })),
                Instruction.Create(OpCodes.Ldstr   ,  "TitleMenu - Awake"),
                Instruction.Create(OpCodes.Call, patcher.BuildCall(levelLoader.GetType("DIMOWAModLoader.LevelLoaderHandler"), "LevelLoaderInnit", typeof(void), new[] { typeof(string) }))
            };

            Target ModLoaderInnitTarget = new Target
            {
                Namespace = "",
                Class = "TitleScreenMenu",
                Method = "Awake",
                Indices = new int[] { 0, 1, 2, 3 },
                Instructions = modLoaderInstructions,
                InsertInstructions = true,
            };

            loaderDependecies = ModDataGatherer.ParseReferences(levelLoader.GetReferencedAssemblies());

            return ModLoaderInnitTarget;
        }
    }
}
