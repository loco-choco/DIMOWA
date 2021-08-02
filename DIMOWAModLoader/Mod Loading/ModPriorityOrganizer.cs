using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DIMOWAModLoader.Mod_Loading
{
    public class ModPriorityOrganizer
    {
        public List<MethodInfo> HighPriority;
        public List<MethodInfo> MediumPriority;
        public List<MethodInfo> LowPriority;

        public ModPriorityOrganizer()
        {
            HighPriority = new List<MethodInfo>();
            MediumPriority = new List<MethodInfo>();
            LowPriority = new List<MethodInfo>();
        }

        public void AddMethodInfoFromPriority(MOWAP modMowap)
        {
            switch ((ModPriority)modMowap.ModPriority)
            {
                case ModPriority.High:
                    HighPriority.Add(modMowap.ModInnitMethod);
                    break;

                case ModPriority.Medium:
                    MediumPriority.Add(modMowap.ModInnitMethod);
                    break;

                case ModPriority.Low:
                    LowPriority.Add(modMowap.ModInnitMethod);
                    break;
                default:
                    break;
            }
        }
        public void RunAllMethodsInOrder()
        {
            RunModMethods(HighPriority);
            RunModMethods(MediumPriority);
            RunModMethods(LowPriority);
        }

        public void RunModMethods(List<MethodInfo> methods)
        {
            for (int i = 0; i < methods.Count; i++)
            {
                try
                {
                    methods[i].Invoke(null, new object[] { "" });
                }
                catch(Exception ex) { Debug.Log(string.Format("The start method from {0} couldn't be run: {1} - {2} - {3}", methods[i].GetType().Name, ex.Message, ex.Source, ex.StackTrace)); }
            }
        }
    }

    public enum ModPriority: byte
    {
        High,
        Medium,
        Low
    }
}
