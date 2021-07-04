using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOWA //Classes Adicionais para Mods de Outer Wilds Alpha
{
    //IMOWAAnotations
    public class IMOWAModInnit : Attribute
    {
        public int modLoadingPlace;
        public int modPriority;
        //Temporario \/
        public string modName;

        public IMOWAModInnit(string modName = "", int modLoadingPlace = 0, int modPriority = 0)
        {
            this.modName = modName;
            this.modLoadingPlace = modLoadingPlace;
            this.modPriority = modPriority;
        }
    }
}
