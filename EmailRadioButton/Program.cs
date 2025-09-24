using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace ADBNotifier
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {
            String thisprocessname = Process.GetCurrentProcess().ProcessName;
            //makes sure the current process isn't already running
            //if it is, prevent it from running again
            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
            {

                MessageBox.Show("ADBNotifier is already running! Check the system tray in the bottom right.", "ADBNotifier Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //otherwise, continue execution as normal
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //lets us override shit to create a systemtray/NotifyIcon application
            Application.Run(new myApplicationContext());
        }

        public class myApplicationContext : ApplicationContext
        {
            public NotifyIcon trayIcon;
            public myApplicationContext()
            {
                SystemTray systemtray = new SystemTray(); //creates a new SystemTray object
                    trayIcon = new NotifyIcon()
                    {
                        Icon = Properties.Resources.Icon1,//sets our cool new system tray icon
                        ContextMenu = new ContextMenu(new MenuItem[] { //gives us a cool right-click menu
                        new MenuItem("Settings", openSettings), //openSettings method opens the Settings dialogue
                        new MenuItem("Exit", Exit), //Exit method kills the process
                        }),
                        Visible = true
                    };
            }//end of myApplicationContext constructor

            private void openSettings(object sender, EventArgs e)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f is Settings)
                    {
                        f.BringToFront(); //prevents multiple Settings from being open with this check
                        return;
                    }
                }
                new Settings().Show(); //if it isn't already open, open a new one
            } //end of openSettings()

            void Exit(object sender, EventArgs e)
            {
                trayIcon.Visible = false; // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Dispose();
                Application.Exit();
            }//end of Exit()
        } //end of myApplicationContext

    }
}
