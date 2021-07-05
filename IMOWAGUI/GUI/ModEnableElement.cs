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
        private const float DistanceOfElements = 1.1f;

        CheckBox checkBox;
        Label label;
        Label moreDetails;
        Color unlockedColor;
        public readonly ModManifestJson json;
        public static List<ModEnableElement> ModEnableElements = new List<ModEnableElement>();

        public bool IsEnabled { get { return checkBox.Checked && true; } private set { } }
        public ModEnableElement(Panel panel, ModManifestJson json, bool Status)
        {
            this.json = json;
            
            label = new Label();
            label.Text = string.Format("{0} [{1}]",json.Name,json.UniqueName);
            panel.Controls.Add(label);
            label.Width = (int)(panel.Width * 0.75f);
            label.BackColor = Color.Transparent;

            moreDetails = new Label();
            moreDetails.Text = string.Format("{0}  -  Version: {1}  By: {2}", json.Description, json.Version, json.Author);
            moreDetails.Width = (int)(panel.Width * 0.9f);
            moreDetails.BackColor = Color.Transparent;
            moreDetails.Top = (int)(label.Height * DistanceOfElements);
            moreDetails.ForeColor = Color.DarkSlateGray;
            panel.Controls.Add(moreDetails);

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
            moreDetails.Top = (int)(label.Height * DistanceOfElements) + y;
        }

        public int GetRow()
        {
            return label.Top +(int)(label.Height * DistanceOfElements) + moreDetails.Height;
        }
    }
}
