using System;
using System.Windows.Forms;

namespace Mod003263
{
    public class Launcher
    {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Transport());
        }
    }
}