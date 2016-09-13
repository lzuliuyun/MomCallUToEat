using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShutdownPC.Panel
{
    class ClockInfo
    {
        private string name;
        private int hour;
        private int min;
        private bool shutdown;
        private bool sleep;
        private int playtime;
        private string[] playlist;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Hour
        {
            get
            {
                return hour;
            }

            set
            {
                hour = value;
            }
        }

        public int Min
        {
            get
            {
                return min;
            }

            set
            {
                min = value;
            }
        }

        public bool Shutdown
        {
            get
            {
                return shutdown;
            }

            set
            {
                shutdown = value;
            }
        }

        public bool Sleep
        {
            get
            {
                return sleep;
            }

            set
            {
                sleep = value;
            }
        }

        public int Playtime
        {
            get
            {
                return playtime;
            }

            set
            {
                playtime = value;
            }
        }

        public string[] Playlist
        {
            get
            {
                return playlist;
            }

            set
            {
                playlist = value;
            }
        }
    }
}
