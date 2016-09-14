using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomCallUToEat.Panel
{
    class ClockConfig
    {
        private ClockInfo[] clockinfos;
        private bool startup;

        public bool Startup
        {
            get
            {
                return startup;
            }

            set
            {
                startup = value;
            }
        }

        internal ClockInfo[] Clockinfos
        {
            get
            {
                return clockinfos;
            }

            set
            {
                clockinfos = value;
            }
        }
    }
}
