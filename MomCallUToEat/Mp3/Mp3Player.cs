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
            wmp.settings.setMode("loop", true);
            wmp.settings.volume = 80;

            //https://msdn.microsoft.com/en-us/library/windows/desktop/dd562692(v=vs.85).aspx
           // wmp.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(playStateChanged);

        }

        public string[] PlayList
        {
            get { return playList; }
            set
            {
                playList = value;

                wmp.currentPlaylist.clear();

                for (int i = 0; i < value.Length; i++)
                {
                    WMPLib.IWMPMedia middleMedia = (WMPLib.IWMPMedia)wmp.newMedia(value[i]);
                    wmp.currentPlaylist.appendItem(middleMedia);
                }
            }
        }


        public string[] getPlayList()
        {
            return this.playList;
        }

        public void play()
        {
            wmp.controls.play();
        }

        public void playNext()
        {
            wmp.controls.next();
        }

        public void playPrevious()
        {
            wmp.controls.previous();
        }

        public void close()
        {
           // wmp.PlayStateChange -= new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(playStateChanged);
            wmp.controls.stop();
            wmp.close();
        }

        private void playStateChanged(int NewState)
        {
            //if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsMediaEnded)
            //{
            //    playNext();
            //}
        }
    }
}
