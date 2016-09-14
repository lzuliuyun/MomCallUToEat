using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Media;
using MomCallUToEat.Mp3;
using MomCallUToEat.Panel;
using MomCallUToEat.JSON;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace MomCallUToEat
{
    public partial class MainForm : Form
    {
        private ClockConfig clockconfig;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();

            initPanel();
        }

        private void initPanel()
        {
            JSONOperation jOper = new JSONOperation();
            clockconfig = jOper.initConfig(Application.StartupPath+"//app.config.json");
            ClockInfo[] clockInfos = clockconfig.Clockinfos;

            //修改配置文件在clockgroup
            int height = 0;
            for (int i = 0; i < clockInfos.Length; i++)
            {
                ClockGroup clockgroup = new ClockGroup(clockInfos[i]);
                GroupBox group = clockgroup.initGroup();
                height = height+ group.Height + 12;
                flowLayoutPanel.Controls.Add(group);
            }

            if (this.Height == 30) return;
            this.Height = height + 30 - (clockInfos.Length -1)*3;

            if (clockconfig.Startup)
            {
                startUpWithPC();
            }
            else
            {
                stopStartUpWithPC();
            }
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        private void hideMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showMenu_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void startup_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string text = item.Text;
            if(text == "开机启动:N")
            {                
                startUpWithPC();
            }
            else
            {
               
                stopStartUpWithPC();
            }
        }

        private void startUpWithPC()
        {
            startupItem.Text = "开机启动:Y";
            saveToJSONConfigFile(true);

            string path = Application.ExecutablePath;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue("MamCallUToEat", path);
            rk.Close();
        }

        private void stopStartUpWithPC()
        {
            startupItem.Text = "开机启动:N";
            saveToJSONConfigFile(false);

            string path = Application.ExecutablePath;
            RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            rk.DeleteValue("MamCallUToEat", false);
            rk.Close();
        }

        private void saveToJSONConfigFile(bool startup)
        {
            JSONOperation jOp = new JSONOperation();

            string path = Application.StartupPath + "//app.config.json";
            JObject jo;
            using (StreamReader reader = File.OpenText(@path))
            {
                jo = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }

            jo["startup"] = startup;
            jOp.WriteToFile(path, jo);
        }
    }
}
