using UnityEngine;
using DIMOWAModLoader.Mod_Loading;
using System;
using System.IO;
using System.Reflection;

namespace DIMOWAModLoader
{
    public class LevelLoaderHandler : MonoBehaviour
    {
        static private bool levelLoaderCreated = false;
        private int levelIndex = -1;
        bool loadMods = false;

        private const string ModListOWFileName = "ModList.ow";

        private ModPriorityOrganizer MainMenuMods;
        private ModPriorityOrganizer GameSceneMods;
        private ModPriorityOrganizer AllScenesMods;

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
                MainMenuMods = new ModPriorityOrganizer();
                GameSceneMods = new ModPriorityOrganizer();
                AllScenesMods = new ModPriorityOrganizer();

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
                switch (mods[i].ModLoadingPlace)
                {
                    case 0:
                        MainMenuMods.AddMethodInfoFromPriority(mods[i]);
                        break;

                    case 1:
                        GameSceneMods.AddMethodInfoFromPriority(mods[i]);
                        break;

                    default:
                        AllScenesMods.AddMethodInfoFromPriority(mods[i]);
                        break;
                }
            }
        }

        //0 - Main Menu (1st scene) , 1 - Game (2nd scene), -1 All (any scene)
        void MainMenuStart()//0
        {
            Debug.Log("MainMenuStart");
            MainMenuMods.RunAllMethodsInOrder();
        }        
        void GameStart()//1
        {
            Debug.Log("GameStartStart");
            GameSceneMods.RunAllMethodsInOrder();
        }
        void AllLevelStart()//-1 , tem prioridade sobre os outros
        {
            Debug.Log("AllLevelStart");
            AllScenesMods.RunAllMethodsInOrder();
        }

        void Update()
        {
            if (levelIndex != Application.loadedLevel || loadMods)
            {
                AllLevelStart();
                levelIndex = Application.loadedLevel;
                if (levelIndex == 0)
                    MainMenuStart();
                else if (levelIndex == 1)
                    GameStart();
                loadMods = false;
            }
        }
    }
}
