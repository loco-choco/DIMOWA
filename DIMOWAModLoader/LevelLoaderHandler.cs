using UnityEngine;

namespace DIMOWAModLoader
{
    public class LevelLoaderHandler : MonoBehaviour
    {
        static private bool levelLoaderCreated = false;
        private int levelIndex = -1;
        bool loadMods = false;
        public int LoopCount { get; private set; }
        
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
            GlobalMessenger<int>.AddListener("StartOfTimeLoop", new Callback<int>(this.OnStartOfTimeLoop));
            DontDestroyOnLoad(gameObject);
        }

        void OnStartOfTimeLoop(int numberOfLoops)
        {
            loadMods = true;
            LoopCount = numberOfLoops;
        }

        //Ideia, no lugar de fazer patch no Assembly do jogo e ter chance de dar ruim, fazer com que isso ocorra nessa classe, como separação de
        //prioridade e em qual level o start vai ocorrer
        //Se a pessoa quer que o script dela ocorra antes de algum Awake ou sla, ela faz usando Harmony e tals

        //Acho não ser a melhor maneira, mas é mais facil e facilita muitas coisas
        void MainMenuStartHigh()
        {
            Debug.Log("High Priority Mods");//0
        }

        void MainMenuStartRegular()
        {
            Debug.Log("Regular Priority Mods");//1
        }

        void MainMenuStartLow()
        {
            Debug.Log("Low Priority Mods");//2
        }

        void MainMenuStart()//0
        {
            Debug.Log("MainMenuStart");
            MainMenuStartHigh();
            MainMenuStartRegular();
            MainMenuStartLow();
        }

        void SolarSystemStartHigh()
        {
            Debug.Log("High Priority Mods");//0
        }

        void SolarSystemStartRegular()
        {
            Debug.Log("Regular Priority Mods");//1
        }

        void SolarSystemStartLow()
        {
            Debug.Log("Low Priority Mods");//2
        }

        void SolarSystemStart()//1
        {
            Debug.Log("SolarSystemStart");
            SolarSystemStartHigh();
            SolarSystemStartRegular();
            SolarSystemStartLow();
        }

        void AllLevelStartHigh()
        {
            Debug.Log("High Priority Mods");
        }

        void AllLevelStartRegular()
        {
            Debug.Log("Regular Priority Mods");
        }

        void AllLevelStartLow()
        {
            Debug.Log("Low Priority Mods");
        }

        void AllLevelStart()//-1 , tem prioridade sobre os outros
        {
            Debug.Log("AllLevelStart");
            AllLevelStartHigh();
            AllLevelStartRegular();
            AllLevelStartLow();
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
                    SolarSystemStart();
                loadMods = false;
            }
        }
    }
}
