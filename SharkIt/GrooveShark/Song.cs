using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SharkIt.GrooveShark
{
    /*
     * {"SongID":"23040605","Name":"Theme Black Orpheus","SongNameID":"3620182","AlbumID":"3399320","AlbumName":"<blank>","ArtistID":"504376","ArtistName":"Paul Desmond & Jim Hall","AvgRating":null,"IsVerified":"0","CoverArtFilename":null,"Year":null,"UserRating":"0","EstimateDuration":"249","Popularity":"0","TrackNum":"0","IsLowBitrateAvailable":"1","Flags":"0"}
     */
    public class Song : Hashtable
    {
        public Song(Hashtable json)
            : base(json)
        {
        }

        public int ID
        {
            get
            {
                return Int32.Parse((string)this["SongID"]);
            }
        }

        public string Name
        {
            get
            {
                return (string)this["Name"];
            }
        }
    }
}
