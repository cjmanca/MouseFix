using native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MouseFix
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        
        DateTime now = DateTime.Now;
        DateTime ignoreUntil = DateTime.Now;
        DateTime nextScreenUpdate = DateTime.Now;

        // initialize
        NativePoint prev;
        cScreen prevMonitor = null;
        NativePoint dp = new NativePoint();
        cScreen dm = null;
        double scaleFactor;

        bool alreadyWorking = false;
        object oLock = new object();
        object safetyLock = new object();
        System.Drawing.Point setPoint = new System.Drawing.Point();

        public bool debugging = false;

        Hooks hook;

        private void Main_Load(object sender, EventArgs LoadEvent)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    object inStartup = key.GetValue("Mouse Fix");

                    if (inStartup != null)
                    {
                        contextWindowsStartup.Checked = true;
                        contextWindowsStartup.CheckState = CheckState.Checked;
                    }
                }
            }
            catch
            {

            }

            timeUpdate.Start();
            timeUpdate_Tick(); // initialize the monitors

            NativePoint pt = new NativePoint();

            pt.X = Cursor.Position.X;
            pt.Y = Cursor.Position.Y;

            prev = pt;

            prevMonitor = cScreen.FromPoint(pt);

            if (prevMonitor != null)
            {
                //Cursor.Clip = prevMonitor.Bounds;
            }

            hook = new Hooks();
            hook.InstallHook(MouseMoved);

            contextExit.Click += ContextExit_Click;

        }

        private void ContextExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void timeUpdate_Tick(object sender = null, EventArgs e = null)
        {
            now = DateTime.Now;
        }

        public bool MouseMoved(NativePoint pt)
        {
            lock (oLock)
            {
                if (alreadyWorking)
                {
                    return false;
                }
                alreadyWorking = true;
            }

            if (pt.X == prev.X && pt.Y == prev.Y)
            {
                alreadyWorking = false;
                return false;
            }

            if (ignoreUntil > now)
            {
                alreadyWorking = false;
                return false;
            }


            if (now >= nextScreenUpdate)
            {
                nextScreenUpdate = now + TimeSpan.FromSeconds(5);

                cScreen.screens.Clear();

                foreach (var s in Screen.AllScreens)
                {
                    var cs = new cScreen();

                    cs.DeviceName = s.DeviceName;
                    cs.Bounds.X = s.Bounds.X;
                    cs.Bounds.Y = s.Bounds.Y;
                    cs.Bounds.Width = s.Bounds.Width - 1;
                    cs.Bounds.Height = s.Bounds.Height;

                    cScreen.screens.Add(cs);

                    /*
                    cScreen.screens.Sort(delegate (cScreen s1, cScreen s2) 
                    {
                        return s1.Bounds.Left.CompareTo(s2.Bounds.Left);
                    });
                    */
                }

                /*
                if (debugging)
                {
                    foreach (var cs in cScreen.screens)
                    {
                        debug("Left: " + cs.Bounds.Left + " Right: " + cs.Bounds.Right);
                    }
                }
                */
            }

            cScreen m = prevMonitor;

            if (m == null)
            {
                m = cScreen.FromPoint(pt);
            }

            if (m == null)
            {
                alreadyWorking = false;
                return false;
            }

            if (prevMonitor == null)
            {
                prevMonitor = m;
            }


            if (pt.X < m.Bounds.Left)
            {
                pt.X = m.Bounds.Left;
            }

            if (pt.X > m.Bounds.Right)
            {
                pt.X = m.Bounds.Right;
            }

            bool moved = false;
           
            dp = pt;


            if (pt.X == m.Bounds.Left)
            {
                dp.X -= 100;
                dm = cScreen.FromPoint(dp);

                if (dm != null && m.Bounds.Left != dm.Bounds.Left)
                {
                    scaleFactor = (double)dm.Bounds.Height / (double)prevMonitor.Bounds.Height;
                    dp.X = dm.Bounds.Right - 1;
                    dp.Y = (int)Math.Round((double)dp.Y * (double)scaleFactor);

                    if (debugging)
                    {
                        debug("Left Moving from: " + prev.X + ", " + prev.Y + " to " + dp.X + ", " + dp.Y + " (" + pt.X + ", " + pt.Y + ")");
                    }

                    prevMonitor = m;
                    pt = dp;
                    m = dm;

                    setPoint.X = pt.X;
                    setPoint.Y = pt.Y;

                    //Cursor.Clip = m.Bounds;
                    Cursor.Position = setPoint;
                    
                    moved = true;

                    ignoreUntil = now + TimeSpan.FromSeconds(0.1);
                }
            }
            else if (pt.X == m.Bounds.Right)
            {
                dp.X += 100;
                dm = cScreen.FromPoint(dp);

                if (dm != null && m.Bounds.Left != dm.Bounds.Left)
                {
                    scaleFactor = (double)dm.Bounds.Height / (double)prevMonitor.Bounds.Height;
                    dp.X = dm.Bounds.Left + 1;
                    dp.Y = (int)Math.Round((double)dp.Y * (double)scaleFactor);

                    if (debugging)
                    {
                        debug("Right Moving from: " + prev.X + ", " + prev.Y + " to " + dp.X + ", " + dp.Y + " (" + pt.X + ", " + pt.Y + ")");
                    }

                    prevMonitor = m;
                    pt = dp;
                    m = dm;

                    setPoint.X = pt.X;
                    setPoint.Y = pt.Y;

                    //Cursor.Clip = m.Bounds;
                    Cursor.Position = setPoint;
                    
                    moved = true;

                    ignoreUntil = now + TimeSpan.FromSeconds(0.1);
                }
            }
            

            prev = pt;
            prevMonitor = m;

            alreadyWorking = false;

            return moved;
        }


        class cScreen
        {
            public static readonly List<cScreen> screens = new List<cScreen>();

            public string DeviceName = "";
            public Rectangle Bounds = new Rectangle();

            static public cScreen FromPoint(NativePoint p)
            {
                foreach (var cs in cScreen.screens)
                {
                    if (cs != null && cs.Bounds.Width > 250)
                    {
                        if (p.X >= cs.Bounds.Left && p.X <= cs.Bounds.Right)
                        {
                            return cs;
                        }
                    }
                }

                return null;
            }
        }





        Queue<string> debugBuffer = new Queue<string>();

        void debug(string msg)
        {
            debugBuffer.Enqueue(now.ToString("[HH:mm:ss.ffff] ") + msg);

            while (debugBuffer.Count > 20)
            {
                debugBuffer.Dequeue();
            }

            string buff = "";

            foreach(string line in debugBuffer)
            {
                buff += line + Environment.NewLine;
            }

            this.InvokeIfRequired(() =>
            {
                Console.Text = buff;
            });
        }

        private void contextWindowsStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (contextWindowsStartup.Checked)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.SetValue("Mouse Fix", "\"" + Application.ExecutablePath + "\"");
                }
            }
            else
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.DeleteValue("Mouse Fix", false);
                }
            }
        }
    }
}
