using System;
using System.Collections.Generic;
using System.Text;

namespace SharkIt.GrooveShark
{
    public class API
    {
        Session m_session;
        
        public delegate void ReadyHandler(object sender);
        public event ReadyHandler Ready;

        public API() : this(new Session())
        {
        }

        public API(Session session)
        {
            m_session = session;
            m_session.GotToken += new Session.GotTokenHandler(m_session_GotToken);
        }

        void m_session_GotToken(object sender, string token)
        {
            if (Ready != null)
                Ready(this);
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
