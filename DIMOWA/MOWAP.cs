using System;
using dnlib.DotNet.Emit;
using dnpatch;


namespace IMOWA
{
    public struct MOWAP
    {
        //Mod de Outer Wilds Alpha Padrão

        public Type ModType { get; set; }

        public string ModInnitMethod { get; set; }

        public string ModName { get; set; }

        public string DllFileName { get; set; }

        public int ModLoadingPlace { get; set; }

        public int ModPriority { get; set; }
    }

    public class MOWAPMaker
    {
        private readonly Target mainMenuStart = new Target()
        {
            Namespace = "DIMOWAModLoader",
            Class = "LevelLoaderHandler",
            Method = "MainMenuStart",
        };

        private readonly Target solarSystemStart = new Target()
        {
            Namespace = "DIMOWAModLoader",
            Class = "LevelLoaderHandler",
            Method = "SolarSystemStart",
        };

        private readonly Target allLevelStart = new Target()
        {
            Namespace = "DIMOWAModLoader",
            Class = "LevelLoaderHandler",
            Method = "AllLevelStart",
        };

        public Target MOWAPModInnitTarget(Type modClass, string modInnitMethod, string modName, Patcher p, int modLoadingPlace = 0, int modPriority = 0)
        {
            //Padrão genérico das Instructions para os MOWA
            Instruction[] opcodesModInnit = {
                Instruction.Create(OpCodes.Ldstr   ,  $"{modName} foi iniciado | was started"),
                Instruction.Create(OpCodes.Call, p.BuildCall(typeof(UnityEngine.Debug), "Log" , typeof(void) , new[]{ typeof(object) })),
                Instruction.Create(OpCodes.Ldstr   ,  modName),
                Instruction.Create(OpCodes.Call, p.BuildCall(modClass, modInnitMethod, typeof(void), new[] { typeof(string) }))
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
