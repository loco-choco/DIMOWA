using System;
using System.Reflection;
using System.IO;

namespace DIMOWAModLoader.Mod_Loading
{
    public struct MOWAP
    {
        //Mod de Outer Wilds Alpha Padrão

        public MethodInfo ModInnitMethod { get; set; }

        public int ModLoadingPlace { get; set; }

        public int ModPriority { get; set; }
    }
}
