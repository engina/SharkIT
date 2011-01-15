using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SharkIt
{
    public class Playlist : JObject
    {
        public Playlist(JObject p) : base(p)
        {
        }

        public override string ToString()
        {
            return (string)this["Name"];
        }
    }
}
