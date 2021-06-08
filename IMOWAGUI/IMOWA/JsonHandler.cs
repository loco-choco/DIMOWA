using IMOWA.GUI;
using System;
using System.IO;
using System.Windows.Forms;

namespace IMOWA
{
    class JsonHandler
    {
        public string CaminhoDoJogo { get; private set; }
        public string CaminhoDaPastaDeMods { get; private set; }
        public string CaminhoDaPastaDeManifestos { get; private set; }

        public  bool ConfigFound { get; private set; }
        public JsonHandler()
        {
            TryToReadConfigFile();
        }
        public void TryToReadConfigFile()
        {
            string[] possibleConfig = Directory.GetFiles(Form1.programPath, "*config.json");
            ConfigFound = possibleConfig.Length > 0;
            if (ConfigFound)
            {
                try
                {
                    JsonReader json = new JsonReader(possibleConfig[0]);
                    CaminhoDoJogo = json.GetJsonElementFromRoot("gameFolder").Value;
                    CaminhoDaPastaDeMods = json.GetJsonElementFromRoot("modsFolder").Value;
                    CaminhoDaPastaDeManifestos = CaminhoDaPastaDeMods;
                }
                catch (Exception ex)
                {
                    ConsoleWindowHelper.FatalException("Something went wrong while reading the config.json file, try deleting it and running the program again");
                    ConsoleWindowHelper.Exception("Exception Message: " + ex.Message + "  " + ex.StackTrace + "  " + ex.Source);
                    throw new IOException("Something went wrong while reading the config.json file, try deleting it and running the program again");
                }
            }
            else
            {
                ConsoleWindowHelper.Log("No config file found, please make one using the settings menu");
                MessageBox.Show("No config file found, search the folders using the settings menu and hit save");
            }
        }
        public void CreteOrChangeConfigFile(string gameFolder, string modFolder)
        {
            if (gameFolder != CaminhoDoJogo || modFolder != CaminhoDaPastaDeMods)
            {
                StreamWriter writer = new StreamWriter(File.Create(Form1.programPath + "/config.json"));
                //Descobrir maneira de colocar a char " dentro da string de maneira, "mais bela"
                string json = "{\n  " + (char)34 + "gameFolder" + (char)34 + ": " + (char)34 + gameFolder.Replace("\\", "/") + (char)34
                    + ",\n  " + (char)34 + "modsFolder" + (char)34 + ": " + (char)34 + modFolder.Replace("\\", "/") + (char)34
                    + "\n}";
                writer.Write(json);
                writer.Flush();
                writer.Close();

                ConsoleWindowHelper.Log("All the modifications to the config.json file have been saved");
            }
            CaminhoDoJogo = gameFolder;
            CaminhoDaPastaDeMods = modFolder;
            CaminhoDaPastaDeManifestos = modFolder;
        }
    }
}
