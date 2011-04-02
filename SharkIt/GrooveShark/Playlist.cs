using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SharkIt.GrooveShark
{
    public class Playlist : Hashtable
    {
        public Playlist(Hashtable p) : base(p)
        {
        }

        public override string ToString()
        {
            return (string)this["Name"];
        }
    }
}
