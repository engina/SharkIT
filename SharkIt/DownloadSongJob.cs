using System;
using System.Collections.Generic;
using System.Text;
using SharkIt.GrooveShark;

namespace SharkIt
{
    public class DownloadSongJob : DownloadJob
    {
        /*
        public DownloadSongJob(API api, string token)
        {
            api.GetSongFromToken(token, new API.GetSongFromTokenHandler(api_GetSongFromTokenHandler), null);
        }
        */
        public DownloadSongJob(API api, Song song)
        {
            api.GetStreamKeyBySongId(song.ID, new API.GetStreamKeyHandler(api_GetStreamKeyHandler), null);
        }

        private void api_GetStreamKeyHandler(API sender, StreamKey key, object state)
        {
        }
    }
}
