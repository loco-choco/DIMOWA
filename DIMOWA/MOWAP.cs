using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using dnlib.DotNet.Emit;
using dnpatch;


namespace IMOWA
{
    public struct MOWAP 
    {
       //Mod de Outer Wilds Alpha Padrão
        
        

       public Type ModType { get; set; }

       public string ModName { get; set; }

       public string dllFileName { get; set; }

       public string ModMethodToTarget { get; set; }

       public string ModClassToTarget { get; set; }

       public string ModNamespaceToTarget { get; set; }

       public int IndiceOfIntructions { get; set; }

       

     
    }
}
