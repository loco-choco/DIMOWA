using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMOWA.GUI
{
    public class ModEnableElement
    {
        CheckBox checkBox;
        Label label;
        Color unlockedColor;
        public string ModName { get; private set; }
        public static List<ModEnableElement> ModEnableElements = new List<ModEnableElement>();

        public bool IsEnabled { get { return checkBox.Checked && true; } private set { } }
        public ModEnableElement(Panel panel, string ModName, bool Status)
        {
            this.ModName = ModName;

            label = new Label();
            label.Text = ModName;
            panel.Controls.Add(label);
            label.Width = (int)(panel.Width * 0.75f);
            label.BackColor = Color.Transparent;

            unlockedColor = Color.Black;
            label.ForeColor = unlockedColor;

            checkBox = new CheckBox();
            checkBox.Checked = Status;
            checkBox.Left = (int)(panel.Width * 0.8f);
            checkBox.Width = 20;
            panel.Controls.Add(checkBox);

            ModEnableElements.Add(this);
        }
        public void LockElement(Color colorOfLockedelement)
        {
            label.ForeColor = colorOfLockedelement;
            checkBox.AutoCheck = false;
        }
        public void UnlockElement()
        {
            label.ForeColor = unlockedColor;
            checkBox.AutoCheck = true;
        }
    
        public void ChangeRow(int y)
        {
            label.Top = y;
            checkBox.Top = y;
        }

        public int GetRow()
        {
            return label.Top;
        }
    }
}
