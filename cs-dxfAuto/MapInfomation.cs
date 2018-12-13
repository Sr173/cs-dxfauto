using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_dxfAuto
{

    public struct MapI
    {
        public enum MapType
        {
            Normal,Spacial,Othther,Eiji,Yuangu
        };

        public MapI(string name,Int32 ID,Int32 minLevel,MapType mapType = MapType.Normal,bool Visible = true)
        {
            this.name = name;
            this.ID = ID;
            this.minLevel = minLevel;
            this.mapType = mapType;
            this.Visible = Visible;
        }
        public string name;
        public Int32 ID;
        public Int32 minLevel;
        public MapType mapType;
        public bool Visible;
    }

    public struct MapInfomation{
        public MapInfomation(string name,Int32 bigRegionID,Int32 smallRegionID,Int32 x,Int32 y,Int32 minLevel,Int32 maxLevel, Int32 maxDiff, MapI[] MapID)
        {
            this.name = name;
            this.bigRegionID = bigRegionID;
            this.smallRegionID = smallRegionID;
            this.x = x;
            this.y = y;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
            this.MapID = MapID;
            this.maxDiff = maxDiff;

        }
        public string name;
        public Int32 bigRegionID;
        public Int32 smallRegionID;
        public Int32 x;
        public Int32 y;
        public Int32 minLevel;
        public Int32 maxLevel;
        public Int32 maxDiff;
        public MapI[] MapID;
    }

    static public class MapInfo
    {
        public static MapInfomation[] mapInfo = new MapInfomation[26];
        
    }
}
