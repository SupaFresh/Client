using System;
using System.Collections.Generic;
using System.Text;
using Client.Logic.Players;
using Client.Logic.Algorithms.Pathfinder;
// This file is part of Mystery Dungeon eXtended.

// Copyright (C) 2015 Pikablu, MDX Contributors, PMU Staff

// Mystery Dungeon eXtended is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Mystery Dungeon eXtended is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Mystery Dungeon eXtended.  If not, see <http://www.gnu.org/licenses/>.


namespace Client.Logic.Maps
{
    [Serializable]
    class Map
    {
        IPathfinder pathfinder;

        #region Constructors

        public Map(string mapID)
        {
            MapID = mapID;
            Tile = new Tile[20, 15];
            OriginalTiles = new Maps.Tile[20, 15];
            Npc = new List<MapNpcSettings>();
            MapItems = new MapItem[MaxInfo.MaxMapItems];
            MapNpcs = new MapNpc[MaxInfo.MAX_MAP_NPCS];
            Loaded = false;
            for (int i = 0; i < MaxInfo.MAX_MAP_NPCS; i++)
            {
                MapNpcs[i] = new MapNpc();
            }
            for (int i = 0; i < MaxInfo.MaxMapItems; i++)
            {
                MapItems[i] = new MapItem();
            }
        }

        #endregion Constructors

        #region Properties

        public MapItem[] MapItems
        {
            get;
            set;
        }

        public IPathfinder Pathfinder
        {
            get
            {
                if (pathfinder == null)
                {
                    pathfinder = new Algorithms.Pathfinder.AStarPathfinder(this);
                }
                return pathfinder;
            }
        }

        public int MinNpcs { get; set; }
        public int MaxNpcs { get; set; }

        public int NpcSpawnTime { get; set; }

        public MapNpc[] MapNpcs
        {
            get;
            set;
        }

        public int Down
        {
            get;
            set;
        }

        public bool Indoors
        {
            get;
            set;
        }

        public int Left
        {
            get;
            set;
        }

        public string MapID
        {
            get;
            set;
        }

        public int MaxX
        {
            get;
            set;
        }

        public int MaxY
        {
            get;
            set;
        }

        public Enums.MapMoral Moral
        {
            get;
            set;
        }

        public string Music
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<MapNpcSettings> Npc
        {
            get;
            set;
        }

        public int Darkness
        {
            get;
            set;
        }

        public string Owner
        {
            get;
            set;
        }

        public int Revision
        {
            get;
            set;
        }

        public int Right
        {
            get;
            set;
        }

        //Public Shop As Integer
        public Tile[,] Tile
        {
            get;
            set;
        }

        public Tile[,] OriginalTiles
        {
            get;
            set;
        }

        public int Up
        {
            get;
            set;
        }

        public Enums.Weather Weather
        {
            get;
            set;
        }

        public bool Loaded
        {
            get;
            set;
        }


        public int DungeonIndex
        {
            get;
            set;
        }

        public bool HungerEnabled
        {
            get;
            set;
        }

        public bool RecruitEnabled
        {
            get;
            set;
        }

        public bool ExpEnabled
        {
            get;
            set;
        }

        public int TimeLimit
        {
            get;
            set;
        }

        public bool Cacheable
        {
            get;
            set;
        }

        public bool Instanced
        {
            get;
            set;
        }



        public string ImpersonatingMap
        {
            get;
            set;
        }

        public string YouTubeMusicID { get; set; }
        public Enums.GameplayMode GameplayMode { get; set; }

        #endregion Properties

        public List<IPlayer> Players
        {
            get;
            set;
        }

        public void DoOverlayChecks()
        {
            Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetOverlay(Enums.Overlay.None);
            if (Indoors == false)
            {
                //Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetWeather(Weather);
            }
            else
            {
                //Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetOverlay(Enums.Overlay.None);
                //Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetWeather(Enums.Weather.None);
                Globals.ActiveWeather = Enums.Weather.None;
            }
        }

        public void LoadFromHouseClass(HouseProperties properties)
        {
            Music = properties.Music;
        }

        public void LoadFromPropertiesClass(MapProperties properties)
        {
            Name = properties.Name;
            Right = properties.Right;
            Left = properties.Left;
            Up = properties.Up;
            Down = properties.Down;
            Music = properties.Music;
            YouTubeMusicID = properties.YouTubeMusicID;
            MaxX = properties.MaxX;
            MaxY = properties.MaxY;
            Moral = properties.Moral;
            Weather = properties.Weather;
            Darkness = properties.Darkness;
            Indoors = properties.Indoors;
            HungerEnabled = properties.Belly;
            RecruitEnabled = properties.Recruit;
            ExpEnabled = properties.Exp;
            TimeLimit = properties.TimeLimit;
            Instanced = properties.Instanced;
            DungeonIndex = properties.DungeonIndex;
            MinNpcs = properties.MinNpcs;
            MaxNpcs = properties.MaxNpcs;
            NpcSpawnTime = properties.NpcSpawnTime;


            Npc = new List<MapNpcSettings>();
            for (int i = 0; i < properties.Npcs.Count; i++)
            {
                Npc.Add(new MapNpcSettings());
                Npc[i].NpcNum = properties.Npcs[i].NpcNum;
                Npc[i].SpawnX = properties.Npcs[i].SpawnX;
                Npc[i].SpawnY = properties.Npcs[i].SpawnY;
                Npc[i].MinLevel = properties.Npcs[i].MinLevel;
                Npc[i].MaxLevel = properties.Npcs[i].MaxLevel;
                Npc[i].AppearanceRate = properties.Npcs[i].AppearanceRate;
                Npc[i].StartStatus = properties.Npcs[i].StartStatus;
                Npc[i].StartStatusCounter = properties.Npcs[i].StartStatusCounter;
                Npc[i].StartStatusChance = properties.Npcs[i].StartStatusChance;
            }
        }

        public HouseProperties ExportToHouseClass()
        {
            HouseProperties properties = new HouseProperties();
            properties.Music = Music;
            return properties;
        }

        public MapProperties ExportToPropertiesClass()
        {
            MapProperties properties = new MapProperties();
            properties.Name = Name;
            properties.Right = Right;
            properties.Left = Left;
            properties.Up = Up;
            properties.Down = Down;
            properties.Music = Music;
            properties.YouTubeMusicID = YouTubeMusicID;
            properties.MaxX = MaxX;
            properties.MaxY = MaxY;
            properties.Moral = Moral;
            properties.Weather = Weather;
            properties.Darkness = Darkness;
            properties.Indoors = Indoors;
            properties.Belly = HungerEnabled;
            properties.Recruit = RecruitEnabled;
            properties.Exp = ExpEnabled;
            properties.TimeLimit = TimeLimit;
            properties.Instanced = Instanced;
            properties.DungeonIndex = DungeonIndex;
            properties.MinNpcs = MinNpcs;
            properties.MaxNpcs = MaxNpcs;
            properties.NpcSpawnTime = NpcSpawnTime;


            for (int i = 0; i < Npc.Count; i++)
            {
                properties.Npcs.Add(new MapNpcSettings());
                properties.Npcs[i].NpcNum = Npc[i].NpcNum;
                properties.Npcs[i].SpawnX = Npc[i].SpawnX;
                properties.Npcs[i].SpawnY = Npc[i].SpawnY;
                properties.Npcs[i].MinLevel = Npc[i].MinLevel;
                properties.Npcs[i].MaxLevel = Npc[i].MaxLevel;
                properties.Npcs[i].AppearanceRate = Npc[i].AppearanceRate;
                properties.Npcs[i].StartStatus = Npc[i].StartStatus;
                properties.Npcs[i].StartStatusCounter = Npc[i].StartStatusCounter;
                properties.Npcs[i].StartStatusChance = Npc[i].StartStatusChance;
            }

            return properties;
        }
    }
}
