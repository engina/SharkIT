using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharkIt.GrooveShark;

namespace SharkIt
{
    public class DownloadSongJob : DownloadJob
    {
        public DownloadSongJob(API api, string token)
        {
            api.GetSongFromToken(token, new API.GetSongFromTokenHandler(api_GetSongFromTokenHandler), null);
        }

        public DownloadSongJob(API api, Song song)
        {
            //api.GetStreamKeyBySongId(song.ID, new API.GetStreamKeyHandler(api_GetStreamKeyHandler), null);
            //api.GetStreamKeyBySongToken(songToken, new API.GetStreamKeyHandler(StreamKeyHandler), 
        }

        private void api_GetSongFromTokenHandler(API sender, Song song, object state)
        {
            //DownloadSongJob(sender, song);
        }
    }
}
