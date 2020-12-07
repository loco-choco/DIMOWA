using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOWA
{
    public class IMOWAModIntaller : Attribute
    {

        //public string classToPatch;
        //public string methodToPatch;
        //public string namespaceToPatch;
        public int gameLevel;// -1, 0 , 1
        public int modPriority; //[0,1000]

        public string dataFileName;

        //Temporario \/
        public string modName;

        public IMOWAModIntaller(int gameLevel = 1, int modPriority = 1000)
        {
            this.gameLevel = gameLevel;
            this.modPriority = modPriority;

        }
    }
}