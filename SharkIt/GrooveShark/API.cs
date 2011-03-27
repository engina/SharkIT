using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharkIt.GrooveShark
{
    public class API
    {
        Session m_session;

        public API() : this(new Session())
        {
        }

        public API(Session session)
        {
            m_session = session;
        }


        public delegate void GetPlaylistHandler(API sender, Playlist p, object state);
        public void GetPlaylist(int id, GetPlaylistHandler handler, object state)
        {
        }

        public delegate void GetStreamKeyHandler(API sender, StreamKey p, object state);
        public void GetStreamKeyBySongId(int id, GetStreamKeyHandler handler, object state)
        {
        }

        public delegate void GetSongFromTokenHandler(API sender, Song song, object state);
        public void GetSongFromToken(string token, GetSongFromTokenHandler handler, object state)
        {
        }
    }
}
