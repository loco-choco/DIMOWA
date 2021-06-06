using System;
using dnlib.DotNet.Emit;
using dnpatch;
using System.Reflection;
using System.IO;


namespace IMOWA
{
    public struct MOWAP
    {
        //Mod de Outer Wilds Alpha Padrão

        public Type ModType { get; set; }

        public string ModInnitMethod { get; set; }

        public string ModName { get; set; }

        public string DllFilePath { get; set; }

        public int ModLoadingPlace { get; set; }

        public int ModPriority { get; set; }

        public string[] Dependencies { get; set; }
    }

    public class MOWAPMaker
    {
        private Type debugType;

        public MOWAPMaker (string gameFolder)
        {
            var possibleFiles = Directory.GetFiles(gameFolder, "UnityEngine.dll", SearchOption.AllDirectories);
            string unityEngineDllPath = "";
            if (possibleFiles.Length > 0)
                unityEngineDllPath = possibleFiles[0];
            else
                throw new Exception("UnityEngine.dll was not found inside the given game folder or inside its child's directories");

            debugType = Assembly.LoadFrom(unityEngineDllPath).GetType("UnityEngine.Debug");
        }

        public MOWAPMaker(Assembly UnityEngine)
        {
            debugType = UnityEngine.GetType("UnityEngine.Debug");
        }


        private static readonly Target mainMenuStart = new Target()
        {
            Namespace = "DIMOWAModLoader",
            Class = "LevelLoaderHandler",
            Method = "MainMenuStart",
        };

        private static readonly Target solarSystemStart = new Target()
        {
            Namespace = "DIMOWAModLoader",
            Class = "LevelLoaderHandler",
            Method = "SolarSystemStart",
        };

        private static readonly Target allLevelStart = new Target()
        {
            Namespace = "DIMOWAModLoader",
            Class = "LevelLoaderHandler",
            Method = "AllLevelStart",
        };

        public Target MOWAPModInnitTarget(Type modClass, string modInnitMethod, string modName, Patcher p, int modLoadingPlace = 0, int modPriority = 0)
        {

            //Padrão genérico das Instructions para os MOWA
            Instruction[] opcodesModInnit = {
                Instruction.Create(OpCodes.Ldstr   ,  modName),
                Instruction.Create(OpCodes.Call, p.BuildCall(modClass, modInnitMethod, typeof(void), new[] { typeof(string) })),
                Instruction.Create(OpCodes.Ldstr   ,  $"{modName} foi iniciado | was started"),
                Instruction.Create(OpCodes.Call, p.BuildCall(debugType, "Log" , typeof(void) , new[]{ typeof(object) }))
                };

            Target targetMod = new Target()
            {
                Namespace = "DIMOWAModLoader",
                Class = "LevelLoaderHandler",
                Instructions = opcodesModInnit,
                Indices = new int[] { 0, 1, 2, 3 },
                InsertInstructions = true

            };
            string modPriorityMethod;
            switch (modPriority)
            {
                case 0:
                    modPriorityMethod = "High";
                    break;

                case 1:
                    modPriorityMethod = "Regular";
                    break;

                case 2:
                default:
                    modPriorityMethod = "Low";
                    break;
            }
            switch (modLoadingPlace)
            {
                case 0:
                    targetMod.Method = "MainMenuStart" + modPriorityMethod;
                    break;

                case 1:
                    targetMod.Method = "SolarSystemStart" + modPriorityMethod;
                    break;

                default:
                    targetMod.Method = "AllLevelStart" + modPriorityMethod;
                    break;
            }
            return targetMod;
        }

        public Target MOWAPModInnitTarget(MOWAP modMOWAP, Patcher modPatcher)
        {
            return MOWAPModInnitTarget(modMOWAP.ModType, modMOWAP.ModInnitMethod, modMOWAP.ModName, modPatcher, modMOWAP.ModLoadingPlace, modMOWAP.ModPriority);
        }


    }
}
