using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using DIMOWAModLoader.Mod_Loading;

namespace DIMOWAModLoader
{
    public class LevelLoaderHandler : MonoBehaviour
    {
        static private bool levelLoaderCreated = false;
        private int levelIndex = -1;
        private GameObject HasANewSceneBeenLoaded;

        private const string ModListOWFileName = "ModList.ow";

        //private ModPriorityOrganizer MainMenuMods;
        //private ModPriorityOrganizer GameSceneMods;
        //private ModPriorityOrganizer AllScenesMods;
        private Dictionary<int, ModPriorityOrganizer> SpecificSceneMods;

        public static void LevelLoaderInnit(string porOndeTaInicializando)
        {
            if (!levelLoaderCreated)
            {
                Debug.Log($"Esta iniciando por {porOndeTaInicializando}");
                new GameObject("DIMOWALevelLoaderHandler").AddComponent<LevelLoaderHandler>();
                Debug.Log("O GameObject do Handler foi criado");
                levelLoaderCreated = !levelLoaderCreated;
            }
        }
        
        void Awake()
        {
            Debug.Log("No awake de mod loader");
            gameObject.AddComponent<ClientDebuggerSide>();
            DontDestroyOnLoad(gameObject);

            try
            {
                //MainMenuMods = new ModPriorityOrganizer();
                //GameSceneMods = new ModPriorityOrganizer();
                SpecificSceneMods = new Dictionary<int, ModPriorityOrganizer>();
                //AllScenesMods = new ModPriorityOrganizer();

                string dllExecutingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string modListFilePath = DirectorySearchTools.GetFilePathInDirectory(ModListOWFileName, dllExecutingPath);

                ModFolderAndList list = new ModFolderAndList();
               ClassSerializer.ReadFromFile(modListFilePath,list);

                MOWAP[] mods = SearchMods.GetModsMOWAPS(dllExecutingPath, list);
                SeparateModsMethod(mods);
            }
            catch (Exception ex)
            { Debug.Log(string.Format("Something went wrong while loading the mods: {0}  - {1} - {2}", ex.Message, ex.Source, ex.StackTrace));}
        }
        private void SeparateModsMethod(MOWAP[] mods)
        {
            Debug.Log("Separating The Mods " + mods.Length);
            for (int i = 0; i < mods.Length; i++)
            {
                Debug.Log("Nome: " + mods[i].ModInnitMethod.Name);
                if (SpecificSceneMods.TryGetValue(mods[i].ModLoadingPlace, out ModPriorityOrganizer modLoadingPlace))
                    modLoadingPlace.AddMethodInfoFromPriority(mods[i]);
                else
                {
                    ModPriorityOrganizer modPriorityOrganizer = new ModPriorityOrganizer();
                    modPriorityOrganizer.AddMethodInfoFromPriority(mods[i]);
                    SpecificSceneMods.Add(mods[i].ModLoadingPlace, modPriorityOrganizer);
                }
            }
        }

        //0 - Main Menu (1st scene) , 1 - Game (2nd scene), -1 All (any scene)
        void TryRunningModsInScene(int index)//0
        {
            if (SpecificSceneMods.TryGetValue(index, out ModPriorityOrganizer modLoadingPlace))
                modLoadingPlace.RunAllMethodsInOrder();
        }
        void AllLevelStart()//-1 , tem prioridade sobre os outros
        {
            Debug.Log("AllLevelStart");
            TryRunningModsInScene(-1);
        }
        void Update()
        {
            if (HasANewSceneBeenLoaded == null)
            {
                AllLevelStart();
                levelIndex = Application.loadedLevel;
                TryRunningModsInScene(levelIndex);
                HasANewSceneBeenLoaded = new GameObject("HasANewSceneBeenLoadedChecker");
            }
        }
    }
}
