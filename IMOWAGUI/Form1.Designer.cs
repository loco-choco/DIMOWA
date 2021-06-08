namespace IMOWA.GUI
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.SaveButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ModsFolderTextBox = new System.Windows.Forms.RichTextBox();
            this.GameFolderTextBox = new System.Windows.Forms.RichTextBox();
            this.ModsFolderChange = new System.Windows.Forms.Label();
            this.GameFolderChange = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ConsoleWindow = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Calibri Light", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(541, 357);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(157, 40);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save Modifications";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-3, -2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(715, 435);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.SaveButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(707, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(11, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 327);
            this.panel1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabPage2.Controls.Add(this.SaveSettingsButton);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(707, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SaveSettingsButton.Location = new System.Drawing.Point(521, 180);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(157, 40);
            this.SaveSettingsButton.TabIndex = 1;
            this.SaveSettingsButton.Text = "Save Settings";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ModsFolderTextBox);
            this.panel2.Controls.Add(this.GameFolderTextBox);
            this.panel2.Controls.Add(this.ModsFolderChange);
            this.panel2.Controls.Add(this.GameFolderChange);
            this.panel2.Location = new System.Drawing.Point(33, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(645, 168);
            this.panel2.TabIndex = 0;
            // 
            // ModsFolderTextBox
            // 
            this.ModsFolderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModsFolderTextBox.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModsFolderTextBox.Location = new System.Drawing.Point(158, 79);
            this.ModsFolderTextBox.Name = "ModsFolderTextBox";
            this.ModsFolderTextBox.Size = new System.Drawing.Size(480, 24);
            this.ModsFolderTextBox.TabIndex = 3;
            this.ModsFolderTextBox.Text = "";
            // 
            // GameFolderTextBox
            // 
            this.GameFolderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GameFolderTextBox.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameFolderTextBox.Location = new System.Drawing.Point(160, 10);
            this.GameFolderTextBox.Name = "GameFolderTextBox";
            this.GameFolderTextBox.Size = new System.Drawing.Size(480, 24);
            this.GameFolderTextBox.TabIndex = 2;
            this.GameFolderTextBox.Text = "";
            // 
            // ModsFolderChange
            // 
            this.ModsFolderChange.AutoSize = true;
            this.ModsFolderChange.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ModsFolderChange.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ModsFolderChange.Cursor = System.Windows.Forms.Cursors.Cross;
            this.ModsFolderChange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ModsFolderChange.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModsFolderChange.Location = new System.Drawing.Point(35, 79);
            this.ModsFolderChange.Name = "ModsFolderChange";
            this.ModsFolderChange.Size = new System.Drawing.Size(117, 26);
            this.ModsFolderChange.TabIndex = 1;
            this.ModsFolderChange.Text = "Mods Folder:";
            this.ModsFolderChange.Click += new System.EventHandler(this.ModsFolderChanger_Click);
            // 
            // GameFolderChange
            // 
            this.GameFolderChange.AutoSize = true;
            this.GameFolderChange.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.GameFolderChange.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GameFolderChange.Cursor = System.Windows.Forms.Cursors.Cross;
            this.GameFolderChange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GameFolderChange.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameFolderChange.Location = new System.Drawing.Point(35, 10);
            this.GameFolderChange.Name = "GameFolderChange";
            this.GameFolderChange.Size = new System.Drawing.Size(120, 26);
            this.GameFolderChange.TabIndex = 0;
            this.GameFolderChange.Text = "Game Folder:";
            this.GameFolderChange.Click += new System.EventHandler(this.GameFolderChanger_Click);
            // 
            // ConsoleWindow
            // 
            this.ConsoleWindow.BackColor = System.Drawing.Color.Silver;
            this.ConsoleWindow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConsoleWindow.Location = new System.Drawing.Point(1, 435);
            this.ConsoleWindow.Name = "ConsoleWindow";
            this.ConsoleWindow.Size = new System.Drawing.Size(707, 103);
            this.ConsoleWindow.TabIndex = 2;
            this.ConsoleWindow.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 538);
            this.Controls.Add(this.ConsoleWindow);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Calibri Light", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "IMOWA 1.4";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label GameFolderChange;
        private System.Windows.Forms.Label ModsFolderChange;
        private System.Windows.Forms.RichTextBox ConsoleWindow;
        private System.Windows.Forms.RichTextBox ModsFolderTextBox;
        private System.Windows.Forms.RichTextBox GameFolderTextBox;
        private System.Windows.Forms.Button SaveSettingsButton;
    }
}

