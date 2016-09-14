using MomCallUToEat.JSON;
using MomCallUToEat.Mp3;
using MomCallUToEat.PCSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MomCallUToEat.Panel
{
    class ClockGroup
    {
        private ClockInfo clockInfo;
        private Mp3Player mp3;
        private System.Timers.Timer timer;

        public ClockGroup(ClockInfo clockinfo)
        {
            this.clockInfo = clockinfo;
        }

        public GroupBox initGroup()
        {
            GroupBox groupbox = new GroupBox();
            groupbox.Width = 245;
            groupbox.Height = 120;
            //groupbox.Margin = 0;

            TextBox hourTxt = new TextBox();
            TextBox minTxt = new TextBox();
            hourTxt.Font = new Font(hourTxt.Font.Name,36);
            hourTxt.Size = new Size(55, 55);
            hourTxt.Multiline = true;

            minTxt.Font = new Font(minTxt.Font.Name, 36);
            minTxt.Size = new Size(55, 55);
            minTxt.Multiline = true;

            hourTxt.Location = new Point(11,35);
            minTxt.Location = new Point(70, 35);

            groupbox.Controls.Add(hourTxt);
            groupbox.Controls.Add(minTxt);            

            Label des = new Label();
            des.Text = "播放时长：分钟";
            des.Location = new Point(135, 15);
            groupbox.Controls.Add(des);

            NumericUpDown playtime = new NumericUpDown();
            playtime.Size = new Size(95, 21);
            playtime.Location = new Point(135, 40);
            playtime.Maximum = 60;
            playtime.Minimum = 1;

            groupbox.Controls.Add(playtime);

            GroupBox groupradio = new GroupBox();
            RadioButton shutdownRadio = new RadioButton();
            RadioButton sleepRadio = new RadioButton();

            sleepRadio.Text = "锁屏";
            shutdownRadio.Text = "关机";

            sleepRadio.Size = new Size(50, 20);
            sleepRadio.Location = new Point(5, 15);

            shutdownRadio.Size = new Size(50, 20);
            shutdownRadio.Location = new Point(55, 15);

            groupradio.Text = "选项";
            groupradio.Controls.Add(sleepRadio);
            groupradio.Controls.Add(shutdownRadio); 

            groupradio.Size = new Size(110, 40);
            groupradio.Location = new Point(135, 70);
            groupbox.Controls.Add(groupradio);

            //赋值
            groupbox.Text = clockInfo.Name;
            hourTxt.Text = string.Format("{0:D2}", clockInfo.Hour);
            minTxt.Text = string.Format("{0:D2}", clockInfo.Min);
            shutdownRadio.Checked = clockInfo.Shutdown;
            sleepRadio.Checked = clockInfo.Sleep;
            playtime.Value = clockInfo.Playtime;

            hourTxt.TextChanged += new EventHandler(hourTxtChange);
            hourTxt.Leave += new EventHandler(hourTxtLeave);
            
            minTxt.TextChanged += new EventHandler(minTxtChange);
            minTxt.Leave += new EventHandler(minTxtLeave);
            playtime.ValueChanged += new EventHandler(playtimeChange);
            sleepRadio.CheckedChanged += new EventHandler(sleepRadioChange);
            shutdownRadio.CheckedChanged += new EventHandler(shutdownRadioChange);

            timer = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timeout); //到达时间的时候执行事件；   
            timer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            timer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

            return groupbox;
        }

        public void timeout(object source, System.Timers.ElapsedEventArgs e)
        {
            DateTime time = DateTime.Now;
            System.Console.WriteLine("Now"+time.Hour+":"+time.Minute+":"+time.Second+"  Clock:"+clockInfo.Hour+":"+clockInfo.Min);
            if (time.Hour == this.clockInfo.Hour && time.Minute == this.clockInfo.Min)
            {
                timer.Stop();
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timeout); //到达时间的时候执行事件； 
                timer.Dispose();

                mp3 = new Mp3Player();
                mp3.PlayList = clockInfo.Playlist;
                mp3.play();

                timer = new System.Timers.Timer(1000 * 60* this.clockInfo.Playtime);   
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timePlay);
                timer.AutoReset = false;   
                timer.Enabled = true; 
            }
        }

        public void timePlay(object source, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timePlay); //到达时间的时候执行事件； 
            timer.Dispose();

            mp3.close();

            if (this.clockInfo.Shutdown)
            {
                shutdown();
            }
            else if (this.clockInfo.Sleep)
            {
                sleep();
            }
        }

        public void shutdown()
        {
            WindowsExit.PowerOff();
        }

        public void sleep()
        {
            WindowsExit.Lock();
        }

        private void sleepRadioChange(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            this.clockInfo.Sleep = temp.Checked;

            saveToJSONConfigFile();
        }


        private void hourTxtChange(object sender, EventArgs e)
        {
            TextBox temp = sender as TextBox;
            try
            {
                if (temp.Text == "") return;
                int time = Convert.ToInt32(temp.Text);
                if(time >= 0 && time < 24)
                {
                    this.clockInfo.Hour = time;
                    saveToJSONConfigFile();
                }
                else
                {
                    temp.Text = string.Format("{0:D2}", clockInfo.Hour);
                }               
            }
            catch (Exception ex)
            {

            }
        }

        private void hourTxtLeave(object sender, EventArgs e)
        {
            TextBox temp = sender as TextBox;
            try
            {
                if (temp.Text == "") return;
                int time = Convert.ToInt32(temp.Text);
                if (time >= 0 && time <24)
                {
                    this.clockInfo.Hour = time;
                    temp.Text = string.Format("{0:D2}", clockInfo.Hour);                   
                    saveToJSONConfigFile();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void minTxtLeave(object sender, EventArgs e)
        {
            TextBox temp = sender as TextBox;
            try
            {
                if (temp.Text == "") return;
                int time = Convert.ToInt32(temp.Text);
                if (time >= 0 && time <= 60)
                {
                    this.clockInfo.Hour = time;
                    temp.Text = string.Format("{0:D2}", clockInfo.Min);
                    saveToJSONConfigFile();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void minTxtChange(object sender, EventArgs e)
        {
            TextBox temp = sender as TextBox;
            try
            {
                if (temp.Text == "") return;
                int time = Convert.ToInt32(temp.Text);

                if (time >= 0 && time <= 60)
                {
                    this.clockInfo.Min = time;
                    saveToJSONConfigFile();
                }
                else
                {
                    temp.Text = string.Format("{0:D2}", clockInfo.Min);
                }
            }
            catch(Exception ex)
            {

            }           
        }

        private void shutdownRadioChange(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            this.clockInfo.Shutdown = temp.Checked;

            saveToJSONConfigFile();
        }

        private void playtimeChange(object sender, EventArgs e)
        {
            NumericUpDown temp = sender as NumericUpDown;
            this.clockInfo.Playtime = (int)temp.Value;

            saveToJSONConfigFile();
        }
        

        private void saveToJSONConfigFile()
        {
            JSONOperation jOp = new JSONOperation();

            string path = Application.StartupPath + "//app.config.json";
            JObject jo;
            using (StreamReader reader = File.OpenText(@path))
            {
                jo = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }

            JArray jArr = (JArray)jo["clocks"];

            string[] abc = new string[] { "1234","333"};

            int len = jArr.Count;
            for (int i = 0; i < len; i++)
            {
                JObject jClock = (JObject)jArr[i];
                if(clockInfo.Name == (string)jArr[i]["name"])
                {
                    JArray playlistObj = (JArray)jClock["playlist"];
                    jClock["name"] = clockInfo.Name;
                    jClock["hour"] = clockInfo.Hour;
                    jClock["min"] = clockInfo.Min;
                    jClock["shutdown"] = clockInfo.Shutdown;
                    jClock["sleep"] = clockInfo.Sleep;
                    jClock["playtime"] = clockInfo.Playtime;
                    jOp.WriteToFile(path, jo);
                    break;
                } 
            }
        }
    }
}
