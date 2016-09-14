using System;


//***************************useful source*****************
//https://msdn.microsoft.com/en-us/library/windows/desktop/dd562388(v=vs.85).aspx
//https://msdn.microsoft.com/en-us/library/windows/desktop/dd562692(v=vs.85).aspx

namespace MomCallUToEat.Mp3
{
    class Mp3Player
    {
        private string[] playList;
        private WMPLib.WindowsMediaPlayer wmp;
        private int playIndex = 0;

        public Mp3Player()
        {
            wmp = new WMPLib.WindowsMediaPlayer();
            wmp.settings.setMode("loop", false);
            wmp.settings.volume = 80;

            //https://msdn.microsoft.com/en-us/library/windows/desktop/dd562692(v=vs.85).aspx
            wmp.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(playStateChanged);

        }

        public string[] PlayList
        {
            get { return playList; }
            set { playList = value; }
        }

        //public WMPLib.WMPPlayState playstate
        //{
        //    get
        //    {
        //        return wmp.playState;
        //    }
        //}

        //public void setPalyList(string[] playlist)
        //{
        //    this.playList = playlist;
        //}

        public string[] getPlayList()
        {
            return this.playList;
        }

        public void play()
        {
            wmp.controls.stop();
            wmp.URL = this.playList[playIndex];
            //wmp.URL = "E:\\Program Files\\Netease\\Music Download\\周杰伦 - 七里香.mp3";
            wmp.controls.play();
        }

        public void playNext()
        {
            wmp.controls.stop();

            playIndex++;
            if (playIndex == playList.Length)
            {
                playIndex = 0;
            }

            wmp.URL = this.playList[playIndex];
            wmp.controls.play();
        }

        public void playPrevious()
        {
            wmp.controls.stop();

            playIndex--;
            if (playIndex == -1)
            {
                playIndex = playList.Length - 1;
            }

            wmp.URL = this.playList[playIndex];
            wmp.controls.play();
        }

        public void close()
        {
            wmp.PlayStateChange -= new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(playStateChanged);
            wmp.controls.stop();
            wmp.close();
        }

        private void playStateChanged(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                playNext();
            }
        }
    }
}
