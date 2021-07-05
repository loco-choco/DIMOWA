using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace IMOWA.GUI
{
    public partial class Form1 : Form
    {
        public static string programPath { get; private set; }
        private List<string> allEnabledMods;
        private ManagerConfig Config;
        private ModDataHandler ModDataHandler;
        public Form1()
        {
            programPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            InitializeComponent();
            ConsoleWindowHelper Console = new ConsoleWindowHelper(ConsoleWindow);
            try
            {
                tabControl1.Appearance = TabAppearance.Normal;
                MainMenu();
                SettingsMenu();

                string[] possibleConfig = Directory.GetFiles(Form1.programPath, "*config.json");
                if (possibleConfig.Length <= 0)
                {
                    tabControl1.SelectedTab = tabPage2;
                }
                else
                {
                    try
                    {
                        Config = JsonReader.ReadFromJson<ManagerConfig>(possibleConfig[0]);
                    }
                    catch(Exception ex)
                    {
                        ConsoleWindowHelper.Exception(ex.Message + " - " + ex.StackTrace);
                        throw new IOException("Something went wrong while reading the config.json file, try deleting it and running the program again");
                    }

                    ModsFolderTextBox.Text = Config.ModsFolder;
                    GameFolderTextBox.Text = Config.GameFolder;

                    ModDataHandler = new ModDataHandler(Config.GameFolder, Config.ModsFolder, Config.ModsFolder);
                    new ModEnableElement(panel1, ModDataHandler.ManifestDoLoader, ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled);

                    allEnabledMods = ModDataHandler.ModEnabledList.ModList.ToList();
                    for (int i = 0; i < ModDataHandler.ManifestsDosMods.Length; i++)
                    {
                        var e = new ModEnableElement(panel1, ModDataHandler.ManifestsDosMods[i], allEnabledMods.IndexOf(ModDataHandler.ManifestsDosMods[i].FileName) > -1);
                        if (i == 0)
                            e.ChangeRow(75);
                        else
                            e.ChangeRow(ModEnableElement.ModEnableElements[i].GetRow() + 25);

                        if (!ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled)
                            e.LockElement(Color.Gray);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWindowHelper.FatalException("A Fatal Exception occured while starting the program: " + ex.Message + " check the Console Windows for more information.");
                ConsoleWindowHelper.Exception(string.Format("{0}: {1} {2}\nInner Exception Message: {3}", ex.Source, ex.Message, ex.StackTrace, ex.InnerException));
            }
        }
        private void MainMenu()
        {
            tabPage1.Text = "IMOWA";
        }
        private void SettingsMenu()
        {
            tabPage2.Text = "Settings";
            ModsFolderTextBox.ReadOnly = true;
            GameFolderTextBox.ReadOnly = true;
        }

        private void SaveModsButton_Click(object sender, EventArgs e)
        {
            try
            {
                for( int i = 0; i < ModEnableElement.ModEnableElements.Count; i++)
                {
                    if (ModEnableElement.ModEnableElements[i].json.FileName != ModDataHandler.dimowaModLoaderFile)
                    {
                        if (ModEnableElement.ModEnableElements[i].IsEnabled && allEnabledMods.IndexOf(ModEnableElement.ModEnableElements[i].json.FileName) < 0)
                            allEnabledMods.Add(ModEnableElement.ModEnableElements[i].json.FileName);
                        else if (!ModEnableElement.ModEnableElements[i].IsEnabled && allEnabledMods.IndexOf(ModEnableElement.ModEnableElements[i].json.FileName) > -1)
                            allEnabledMods.Remove(ModEnableElement.ModEnableElements[i].json.FileName);
                    }
                }
                
                ModListJson modList = new ModListJson()
                {
                    ModList = allEnabledMods.ToArray(),
                    ModFolder = Config.ModsFolder
                };
                JsonReader.WriteToJson(modList, Path.Combine(DirectorySearchTools.GetDirectoryInDirectory("Managed", Config.GameFolder), ModDataHandler.loaderModJsonFile));
                if (ModEnableElement.ModEnableElements[0].IsEnabled) //Primeiro elemento é sempre do loader
                {//DIMOWALoaderInstaller.Install()/.Unistall() consomem bastante ram e cpu quanto chamados com o .Save() e continuam consumindo ele em seguida, parece ser algo relacionado com o tamanho do DLL (como se ele estivesse carregando todo ele para salvar mas não deletando esse objeto)
                    ;
                    if (ModDataHandler.DIMOWALoaderInstaller.Install())
                    {
                        File.Copy(DirectorySearchTools.GetFilePathInDirectory(ModDataHandler.ManifestDoLoader.FileName, Config.ModsFolder, Config.GameFolder), Path.Combine(DirectorySearchTools.GetDirectoryInDirectory("Managed", Config.GameFolder), ModDataHandler.ManifestDoLoader.FileName));

                        ModDataHandler.DIMOWALoaderInstaller.SaveModifications();
                        ModDataHandler.DIMOWALoaderInstaller.ResetLoaderInstaller();

                        ConsoleWindowHelper.Log("DIMOWAModLoader has been successfully installed");
                        foreach (var element in ModEnableElement.ModEnableElements)
                            element.UnlockElement();
                    }
                }
                else
                {
                    if (ModDataHandler.DIMOWALoaderInstaller.Uninstall())
                    {
                        
                        ModDataHandler.DIMOWALoaderInstaller.SaveModifications();
                        ModDataHandler.DIMOWALoaderInstaller.ResetLoaderInstaller();

                        File.Delete(Path.Combine(DirectorySearchTools.GetDirectoryInDirectory("Managed", Config.GameFolder), ModDataHandler.ManifestDoLoader.FileName));
                        File.Delete(Path.Combine(DirectorySearchTools.GetDirectoryInDirectory("Managed", Config.GameFolder), ModDataHandler.loaderModJsonFile));

                        ConsoleWindowHelper.Log("DIMOWAModLoader has been successfully uninstalled");
                        for (int i = 1; i < ModEnableElement.ModEnableElements.Count; i++)
                            ModEnableElement.ModEnableElements[i].LockElement(Color.Gray);
                    }
                }

                ConsoleWindowHelper.Log("All the changes have been performed and saved");
            }
            catch (Exception ex)
            {
                ConsoleWindowHelper.FatalException("A Fatal Exception occured while saving: " + ex.Message + " check the Console Windows for more information.");
                ConsoleWindowHelper.Exception(string.Format("{0}: {1} {2}\nInner Exception Message: {3}", ex.Source, ex.Message, ex.StackTrace, ex.InnerException));
            }
        }

        //Game Folder
        private void GameFolderChanger_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                GameFolderTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        //Mod Folder
        private void ModsFolderChanger_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ModsFolderTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ModsFolderTextBox.Text != "" && GameFolderTextBox.Text != "")
                {
                    Config = new ManagerConfig { GameFolder = GameFolderTextBox.Text, ModsFolder = ModsFolderTextBox.Text };
                    JsonReader.WriteToJson(Config, Path.Combine(programPath, "config.json"));
                    //Reiniciar IMOWA e o Loader Installer
                    if (ModDataHandler == null)
                    {
                        ModDataHandler = new ModDataHandler(Config.GameFolder, Config.ModsFolder, Config.ModsFolder);

                        new ModEnableElement(panel1, ModDataHandler.ManifestDoLoader, ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled);
                        allEnabledMods = ModDataHandler.ModEnabledList.ModList.ToList();
                        for (int i = 0; i < ModDataHandler.ManifestsDosMods.Length; i++)
                        {
                            var ele = new ModEnableElement(panel1, ModDataHandler.ManifestsDosMods[i], allEnabledMods.IndexOf(ModDataHandler.ManifestsDosMods[i].FileName) > -1);
                            if (i == 0)
                                ele.ChangeRow(75);
                            else
                                ele.ChangeRow(ModEnableElement.ModEnableElements[i].GetRow() + 25);

                            if (!ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled)
                                ele.LockElement(Color.Gray);
                        }
                    }
                }
                else if (ModsFolderTextBox.Text != "" && GameFolderTextBox.Text == "")
                {
                    ConsoleWindowHelper.Warning("The Game Folder path is missing");
                    MessageBox.Show("The Game Folder path is missing");
                }
                else if (ModsFolderTextBox.Text == "" && GameFolderTextBox.Text != "")
                {
                    ConsoleWindowHelper.Warning("The Mod Folder path is missing");
                    MessageBox.Show("The Mod Folder path is missing");
                }
                else
                {
                    ConsoleWindowHelper.Warning("Both paths are missing");
                    MessageBox.Show("Both paths are missing");
                }
            }
            catch (Exception ex)
            {
                ConsoleWindowHelper.FatalException("A Fatal Exception occured while saving the settings: " + ex.Message + " check the Console Windows for more information.");
                ConsoleWindowHelper.Exception(string.Format("{0}: {1} {2}\nInner Exception Message: {3}", ex.Source, ex.Message, ex.StackTrace, ex.InnerException));
            }
        }
    }
}
