using native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseFix
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            bool debugging = false;


            var form = new Main();

            form.debugging = debugging;
            form.ShowInTaskbar = false;
            form.StartPosition = FormStartPosition.Manual;

            if (!debugging)
            {
                form.Visible = false;
                form.Location = new System.Drawing.Point(0, -2000);
            }

            form.Show();

            if (!debugging)
            {
                form.Hide();
            }
            
            Application.Run();
        }

    }




}
