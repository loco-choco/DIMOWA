using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMOWA.GUI
{
    public class ConsoleWindowHelper
    {
        private static RichTextBox TextBox;
        public ConsoleWindowHelper(RichTextBox textBox)
        {
            TextBox = textBox;
            TextBox.ReadOnly = true;
        }
        public static void AppendWithColor(string text, Color color)
        {
            TextBox.SelectionStart = TextBox.TextLength;
            TextBox.SelectionLength = 0;
            TextBox.SelectionColor = color;
            TextBox.AppendText(text);
            TextBox.SelectionColor = TextBox.ForeColor;
        }
        public static void Log(string log)
        {
            AppendWithColor(log + '\n', Color.DarkOliveGreen);
        }
        public static void Warning(string warning)
        {
            AppendWithColor(warning + '\n', Color.SaddleBrown);
        }
        public static void Exception(string exception)
        {
            AppendWithColor(exception + '\n', Color.DarkRed);
        }
        public static void FatalException(string fatalException)
        {
            AppendWithColor(fatalException + '\n', Color.DarkRed);
            MessageBox.Show(fatalException);
        }
    }
}
