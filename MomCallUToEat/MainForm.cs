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
using AutoShutdownPC.Mp3;
using AutoShutdownPC.Panel;
using AutoShutdownPC.JSON;

namespace AutoShutdownPC
{
    public partial class MainForm : Form
    {
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
            ClockInfo[] clockInfos = jOper.initConfig("./app.config.json");

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
    }
}
