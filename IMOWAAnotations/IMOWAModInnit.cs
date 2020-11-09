using System;
using System.Collections.Generic;
using System.Text;

namespace IMOWAAnotations
{
   
    public class IMOWAModInnit : Attribute
    {
       
        public string classToPatch;
        public string methodToPatch;
        public string namespaceToPatch;

        public string dataFileName;
        public int indiceOfPatch;
        //Temporario \/
        public string modName;

        public IMOWAModInnit(string classToPatch, string methodToPatch, string namespaceToPatch = "", int indiceOfPatch=0)
        {
            this.classToPatch = classToPatch;
            this.methodToPatch = methodToPatch;
            this.namespaceToPatch = namespaceToPatch;
            if (indiceOfPatch > -1)
            {
                this.indiceOfPatch = indiceOfPatch;
            }
        }
    }
}
