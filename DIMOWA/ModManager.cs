﻿using dnpatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOWA
{
    public class ModManager //Renomear para IMOWA depois
    {
        private Patcher patcher;
        private string dimowaPath;
        private Target[] modTargets;
        private MOWAP[] modsInfo;

        public ModManager(string gameFolder, List<MOWAP> modList, Assembly UnityEngine = null)
        {
            dimowaPath = GetFilePathInDirectory("DIMOWAModLoader.dll", gameFolder);
            patcher = new Patcher(dimowaPath);
            modTargets = new Target[modList.Count];
            modsInfo = modList.ToArray();
            MOWAPMaker mowapMaker;
            if (UnityEngine == null)
                mowapMaker = new MOWAPMaker(gameFolder);
            else
                mowapMaker = new MOWAPMaker(UnityEngine);

            for (int i = 0; i < modList.Count; i++)
                modTargets[i] = mowapMaker.MOWAPModInnitTarget(modList[i], patcher);
        }

        public int AmountOfMods()
        {
            return modsInfo.Length;
        }

        public bool IsTheModInstalled(int index)
        {
            return IMOWA.IndexOfInstalledMod(modTargets[index], patcher) >= 0;
        }

        /// <summary>
        /// Returns the index of the mod from its name, returns -1 if it isn't found
        /// </summary>
        /// <param name="modName"></param>
        /// <returns></returns>
        public int ModIndexInArray(string modName)
        {
            int i = -1;
            bool foundMod = false;
            for (i = 0; i < modsInfo.Length; i++)
                if (modsInfo[i].ModName == modName)
                {
                    foundMod = true;
                    break;
                }
            if (!foundMod)
                return -1;

            return i;
        }

        /// <summary>
        /// Returns true if installed, false if already installed
        /// </summary>
        /// <param name="index">The index of the mod in the MOWAP list</param>
        /// <returns></returns>
        public bool InstallMod(int index)
        {
            if (IMOWA.IndexOfInstalledMod(modTargets[index], patcher) > -1)
                return false;
            return IMOWA.InstallMod(modTargets[index], patcher);
        }

        /// <summary>
        /// >Returns true if installed, false if already installed or if it was not found
        /// </summary>
        /// <param name="modName">The name from its MOWAP</param>
        /// <returns</returns>
        public bool InstallMod(string modName)
        {
            return InstallMod(ModIndexInArray(modName));
        }

        /// <summary>
        /// Returns true if uninstalled, false if already uninstalled
        /// </summary>
        /// <param name="index">The index of the mod in the MOWAP list</param>
        /// <returns></returns>
        public bool UninstallMod(int index)
        {
            int i = IMOWA.IndexOfInstalledMod(modTargets[index], patcher);
            if (i < 0)
                return false;
            return IMOWA.UninstallMod(modTargets[index], patcher, i);
        }

        /// <summary>
        /// Returns true if installed, false if already uninstalled or if it was not found
        /// </summary>
        /// <param name="modName">The name from its MOWAP</param>
        /// <returns></returns>
        public bool UninstallMod(string modName)
        {
            return UninstallMod(ModIndexInArray(modName));
        }

        public void SaveModifications()
        {
            patcher.Save(false);
        }

        /// <summary>
        /// Creates a new Patcher without deleting the mod data, use it after using SaveModifications if you want to continue patching the file
        /// </summary>
        public void RefreshAfterSave()
        {
            patcher = new Patcher(dimowaPath);

        }



        public static string GetFilePathInDirectory(string fileName, string folder)
        {
            var possibleFiles = Directory.GetFiles(folder, fileName, SearchOption.AllDirectories);
            if (possibleFiles.Length > 0)
                return possibleFiles[0];
            else
                throw new FileNotFoundException($"{fileName} was not found inside {folder} or inside its child's directories");
        }

        public static string GetDirectoryInDirectory(string directoryName, string folder)
        {
            var possibleDirectories = Directory.GetDirectories(folder, directoryName, SearchOption.AllDirectories);
            if (possibleDirectories.Length > 0)
                return possibleDirectories[0];
            else
                throw new DirectoryNotFoundException($"{directoryName} was not found inside {folder} or inside its child's directories");
        }
    }

    
}
