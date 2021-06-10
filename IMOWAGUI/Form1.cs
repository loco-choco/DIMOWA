using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using IMOWA;
using System.Drawing;

namespace IMOWA.GUI
{
    public partial class Form1 : Form
    {
        public static string programPath { get; private set; }
        private JsonHandler JsonHandler;
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

                JsonHandler = new JsonHandler();
                if (!JsonHandler.ConfigFound)
                {
                    tabControl1.SelectedTab = tabPage2;
                }
                else
                {
                    ModsFolderTextBox.Text = JsonHandler.CaminhoDaPastaDeMods;
                    GameFolderTextBox.Text = JsonHandler.CaminhoDoJogo;

                    ModDataHandler = new ModDataHandler(JsonHandler.CaminhoDoJogo, JsonHandler.CaminhoDaPastaDeMods, JsonHandler.CaminhoDaPastaDeManifestos);
                    new ModEnableElement(panel1, "DIMOWAModLoader", ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled);
                    for (int i = 0; i < ModDataHandler.ModManager.AmountOfMods(); i++)
                    {
                        var e = new ModEnableElement(panel1, ModDataHandler.ModManager.NameOfMod(i), ModDataHandler.ModManager.IsTheModInstalled(i));
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

        public static void CopyFileAndReferencesToFolder(MOWAP modData, string folderToCopy, params string[] originFolders)
        {
            string fileName = Path.GetFileName(modData.DllFilePath);
            if (Directory.GetFiles(folderToCopy, fileName).Length == 0)
            {
                ConsoleWindowHelper.Warning(Path.GetFileName(modData.DllFilePath) + " wasn't in " + folderToCopy + ", copying it there");
                File.Copy(modData.DllFilePath, folderToCopy + '/' + fileName);
            }
            for (int i = 0; i < modData.Dependencies.Length; i++)
            {
                if (Directory.GetFiles(folderToCopy, modData.Dependencies[i]).Length == 0)
                {
                    ConsoleWindowHelper.Warning(string.Format("{0} wasn't in the folder, copying it there", modData.Dependencies[i]));
                    string filePath = "";
                    foreach (string origin in originFolders){
                        try
                        {
                            filePath = ModManager.GetFilePathInDirectory(modData.Dependencies[i], origin);
							if(filePath != "")
								break;
                        }
						catch { }
					}
                    if (filePath != "")
                        File.Copy(filePath, folderToCopy + '/' + modData.Dependencies[i]);
                    else
                        ConsoleWindowHelper.Exception(string.Format("Couldn't find the file {0}, a dependencie of {1}, in any folder, make sure that it is in there", modData.Dependencies[i], modData.ModName));
                }
            }
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string managedDirectoryPath = Directory.GetDirectories(JsonHandler.CaminhoDoJogo, "*Managed", SearchOption.AllDirectories)[0];

                if (ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled)
                {
                    for (int i = 1; i < ModEnableElement.ModEnableElements.Count; i++)
                    {
                        string modName = ModEnableElement.ModEnableElements[i].ModName;
                        int modIndexInManager = ModDataHandler.ModManager.ModIndexInArray(modName);
                        if (ModEnableElement.ModEnableElements[i].IsEnabled)//Instalar == true
                        {
                            if (ModDataHandler.ModManager.InstallMod(modIndexInManager))
                            {
                                ConsoleWindowHelper.Log(ModDataHandler.ModManager.NameOfMod(modIndexInManager) + " has been successfully installed");
                                CopyFileAndReferencesToFolder(ModDataHandler.GetModMOWAP(modIndexInManager), managedDirectoryPath, JsonHandler.CaminhoDaPastaDeMods, JsonHandler.CaminhoDoJogo);
                            }
                        }
                        else
                        {
                            if (ModDataHandler.ModManager.UninstallMod(modIndexInManager))
                            {
                                string modFileName = ModDataHandler.GetModDllFileName(modIndexInManager);
                                ConsoleWindowHelper.Log(modName + " has been successfully uninstalled");
                                if (Directory.GetFiles(managedDirectoryPath, modFileName).Length > 0)
                                {
                                    ConsoleWindowHelper.Warning(string.Format("{0} is still in the folder, removing it from there", modFileName));
                                    File.Delete(managedDirectoryPath + '/' + modFileName);
                                }
                            }
                        }
                    }
                    ModDataHandler.ModManager.SaveModifications();
                    ModDataHandler.ModManager.RefreshAfterSave();
                }

                if (ModEnableElement.ModEnableElements[0].IsEnabled) //Primeiro elemento é sempre do loader
                {//DIMOWALoaderInstaller.Install()/.Unistall() consomem bastante ram e cpu quanto chamados com o .Save() e continuam consumindo ele em seguida, parece ser algo relacionado com o tamanho do DLL (como se ele estivesse carregando todo ele para salvar mas não deletando esse objeto)
                    ;
                    if (ModDataHandler.DIMOWALoaderInstaller.Install())
                    {
                        ConsoleWindowHelper.Log("DIMOWAModLoader has been successfully installed");
                        CopyFileAndReferencesToFolder(new MOWAP { Dependencies = ModDataHandler.DIMOWALoaderInstaller.loaderDependecies, DllFilePath = ModDataHandler.GetLoaderDllFilePath() }, managedDirectoryPath, JsonHandler.CaminhoDaPastaDeMods, JsonHandler.CaminhoDoJogo);

                        ModDataHandler.DIMOWALoaderInstaller.SaveModifications();
                        ModDataHandler.DIMOWALoaderInstaller.ResetLoaderInstaller();

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
                    JsonHandler.CreteOrChangeConfigFile(GameFolderTextBox.Text, ModsFolderTextBox.Text);
                    //Reiniciar IMOWA e o Loader Installer
                    if (ModDataHandler == null)
                    {
                        ModDataHandler = new ModDataHandler(JsonHandler.CaminhoDoJogo, JsonHandler.CaminhoDaPastaDeMods, JsonHandler.CaminhoDaPastaDeManifestos);

                        new ModEnableElement(panel1, "DIMOWAModLoader", ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled);
                        for (int i = 0; i < ModDataHandler.ModManager.AmountOfMods(); i++)
                        {
                            var element = new ModEnableElement(panel1, ModDataHandler.ModManager.NameOfMod(i), ModDataHandler.ModManager.IsTheModInstalled(i));
                            element.ChangeRow(ModEnableElement.ModEnableElements[i].GetRow() + 25);

                            if (!ModDataHandler.DIMOWALoaderInstaller.IsLoaderInstalled)
                                element.LockElement(Color.Gray);
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
