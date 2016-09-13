using AutoShutdownPC.Panel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoShutdownPC.JSON
{
    class JSONOperation
    {
        /// <summary>
        /// 根据路径读取json文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadFile(string path)
        {
            string jsonStr = "";
            byte[] byData = new byte[100];
            char[] charData = new char[1000];

            try
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    jsonStr = jsonStr + line.ToString();
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }

            return jsonStr;
        }

        public ClockInfo[] initConfig(string path)
        {
            JObject jo;
            using (StreamReader reader = File.OpenText(@path))
            {
                jo = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }

            string configs = jo["clocks"].ToString();
            JArray jArr = (JArray)jo["clocks"];
            //string ja1a = ja[1]["remarks"].ToString();
            int len = jArr.Count;
            ClockInfo[] clockInfos = new ClockInfo[len];
            for(int i = 0; i < len; i++)
            {
                JObject jClock = (JObject)jArr[i];
                ClockInfo clockinfo = new ClockInfo(); 

                clockinfo.Name = (string)jArr[i]["name"];
                clockinfo.Hour = (int)jClock["hour"];
                clockinfo.Min = (int)jClock["min"];
                clockinfo.Shutdown = (bool)jClock["shutdown"];
                clockinfo.Sleep = (bool)jClock["sleep"];
                clockinfo.Playtime = (int)jClock["playtime"];

                JArray playlistObj = (JArray)jClock["playlist"];
                IList<string> iplaylist = playlistObj.Select(x => (string) x).ToList();

                string[] playlist = new string[iplaylist.Count];
                iplaylist.CopyTo(playlist, 0);
                clockinfo.Playlist = playlist;

                clockInfos[i] = clockinfo;
            }

            return clockInfos;

            //IList<ClockInfo> clockInfo = ((JArray)ja).Select(x => new ClockInfo
            //{
            //    Name = (string)x["name"],
            //    Hour = (int)x["hour"],
            //    Min = (int)x["min"],
            //    Shutdown = (bool)x["shutdown"],
            //    Sleep = (bool)x["sleep"],
            //    Playtime = (int)x["playtime"],
            //    Playlist = (object)x["playlist"]
            //}).ToList();

        }

        public void WriteToFile(string path,object jObj)
        {
            try
            {
                File.WriteAllText(@path, JsonConvert.SerializeObject(JObject.Parse(jObj.ToString())));
            }
            catch(Exception ex)
            {

            }
        }

        private class Movie
        {
            public string Name { get; set; }
            public int Year { get; set; }
        }
    }
}
