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


using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Logic.Maps
{
    class MapProperties
    {
        public string Name { get; set; }
        public int Right { get; set; }
        public int Left { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }
        public string Music { get; set; }
        public string YouTubeMusicID { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public Enums.MapMoral Moral { get; set; }
        public Enums.Weather Weather { get; set; }
        public int Darkness { get; set; }
        public bool Indoors { get; set; }
        public bool Belly { get; set; }
        public bool Recruit { get; set; }
        public bool Exp { get; set; }
        public int TimeLimit { get; set; }
        public bool Instanced { get; set; }
        public int DungeonIndex { get; set; }
        public List<MapNpcSettings> Npcs { get; set; }
        public int MinNpcs { get; set; }
        public int MaxNpcs { get; set; }
        public int NpcSpawnTime { get; set; }
        public Enums.MapEffect Effect { get; set; }

        public MapProperties()
        {
            Npcs = new List<MapNpcSettings>();
        }
    }
}
