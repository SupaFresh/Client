﻿using System;
using System.Collections.Generic;
using System.Text;

using PMDCP.Core;
using Client.Logic.Players;
using Client.Logic.Windows;
using Client.Logic.Network;
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
    class MapHelper
    {
        #region Fields

        static MapCollection mMaps;

        #endregion Fields

        #region Methods

        public static void InitMapHelper()
        {
            mMaps = new MapCollection();
        }

        public static void LoadMapFromFile(int mapNum)
        {
            Map loadedMap = LoadMapFromFile(IO.Paths.MapPath + "Map" + mapNum.ToString() + ".dat");
            if (loadedMap != null)
            {
                loadedMap.MapID = "s" + mapNum;
                mMaps[Enums.MapID.TempActive] = loadedMap;
            }
        }

        public static Map LoadMapFromFile(string filePath)
        {
            filePath = IO.Paths.CreateOSPath(filePath);

            if (System.IO.File.Exists(filePath) == false)
            {
                return null;
            }

            int lineNum = 1;
            bool headerFound = false;
            string line = null;

            Map map = new Map("?");
            string[] parse;
            using (System.IO.StringReader reader = new System.IO.StringReader(new string(System.Text.Encoding.Unicode.GetChars(Globals.Encryption.DecryptBytes(System.IO.File.ReadAllBytes(filePath))))))
            {
                do
                {
                    if (headerFound == false && lineNum > 1)
                    {
                        System.IO.File.Delete(filePath);
                        break;
                    }
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    else
                    {
                        parse = line.Split('|');
                    }

                    switch (parse[0].ToLower())
                    {
                        case "mapdata":
                            {
                                headerFound = true;
                                if (parse[1].ToLower() != "v13")
                                {
                                    System.IO.File.Delete(filePath);
                                    return null;
                                }
                                map.Revision = parse[2].ToInt();
                                map.MaxX = parse[3].ToInt(19);
                                map.MaxY = parse[4].ToInt(14);

                                if (map.MaxX < 19)
                                    map.MaxX = 19;
                                if (map.MaxY < 14)
                                    map.MaxY = 14;

                                map.Tile = new Tile[map.MaxX + 1, map.MaxY + 1];
                                map.OriginalTiles = new Tile[map.MaxX + 1, map.MaxY + 1];
                            }
                            break;
                        case "data":
                            {
                                map.Name = parse[1];
                                map.Moral = (Enums.MapMoral)parse[2].ToInt();
                                map.Up = parse[3].ToInt();
                                map.Down = parse[4].ToInt();
                                map.Left = parse[5].ToInt();
                                map.Right = parse[6].ToInt();
                                map.Music = parse[7];
                                map.Indoors = parse[8].ToBool();
                                map.Owner = parse[9];
                                map.Weather = (Enums.Weather)parse[10].ToInt();
                                map.Darkness = parse[11].ToInt();
                                map.HungerEnabled = parse[12].ToBool();
                                map.RecruitEnabled = parse[13].ToBool();
                                map.ExpEnabled = parse[14].ToBool();
                                map.TimeLimit = parse[15].ToInt();
                                map.DungeonIndex = parse[16].ToInt();
                                map.MinNpcs = parse[17].ToInt();
                                map.MaxNpcs = parse[18].ToInt();
                                map.NpcSpawnTime = parse[19].ToInt();
                                map.Instanced = parse[20].ToBool();
                                map.YouTubeMusicID = parse[21];
                                map.GameplayMode = (Enums.GameplayMode)parse[22].ToInt();
                                map.Effect = (Enums.MapEffect)parse[23].ToInt();
                            }
                            break;
                        case "npcsettings":
                            {
                                int npcSlot = parse[1].ToInt();
                                map.Npc.Add(new MapNpcSettings());
                                map.Npc[npcSlot].NpcNum = parse[2].ToInt();
                                map.Npc[npcSlot].SpawnX = parse[3].ToInt();
                                map.Npc[npcSlot].SpawnY = parse[4].ToInt();
                                map.Npc[npcSlot].MinLevel = parse[5].ToInt();
                                map.Npc[npcSlot].MaxLevel = parse[6].ToInt();
                                map.Npc[npcSlot].AppearanceRate = parse[7].ToInt();
                                map.Npc[npcSlot].StartStatus = (Enums.StatusAilment)parse[8].ToInt();
                                map.Npc[npcSlot].StartStatusCounter = parse[9].ToInt();
                                map.Npc[npcSlot].StartStatusChance = parse[10].ToInt();
                            }
                            break;
                        //case "npcdata": {
                        //    int n = 1;
                        //    for (int i = 0; i < MaxInfo.MAX_MAP_NPCS; i++) {
                        //        map.Npc[i] = parse[n].ToInt();
                        //        n++;
                        //    }
                        //}
                        //break;
                        //case "spawnx": {
                        //    int n = 1;
                        //    for (int i = 0; i < MaxInfo.MAX_MAP_NPCS; i++) {
                        //        map.SpawnX[i] = parse[n].ToInt();
                        //        n++;
                        //    }
                        //}
                        //break;
                        //case "spawny": {
                        //    int n = 1;
                        //    for (int i = 0; i < MaxInfo.MAX_MAP_NPCS; i++) {
                        //        map.SpawnY[i] = parse[n].ToInt();
                        //        n++;
                        //    }
                        //}
                        //break;
                        case "tile":
                            {
                                Tile tile = map.Tile[parse[1].ToInt(), parse[2].ToInt()] = new Tile();
                                tile.Ground = parse[3].ToInt();
                                tile.GroundAnim = parse[4].ToInt();
                                tile.Mask = parse[5].ToInt();
                                tile.Anim = parse[6].ToInt();
                                tile.Mask2 = parse[7].ToInt();
                                tile.M2Anim = parse[8].ToInt();
                                tile.Mask3 = parse[9].ToInt();
                                tile.M3Anim = parse[10].ToInt();
                                tile.Mask4 = parse[11].ToInt();
                                tile.M4Anim = parse[12].ToInt();
                                tile.Mask5 = parse[13].ToInt();
                                tile.M5Anim = parse[14].ToInt();
                                tile.Fringe = parse[15].ToInt();
                                tile.FAnim = parse[16].ToInt();
                                tile.Fringe2 = parse[17].ToInt();
                                tile.F2Anim = parse[18].ToInt();
                                tile.Fringe3 = parse[19].ToInt();
                                tile.F3Anim = parse[20].ToInt();
                                tile.Fringe4 = parse[21].ToInt();
                                tile.F4Anim = parse[22].ToInt();
                                tile.Fringe5 = parse[23].ToInt();
                                tile.F5Anim = parse[24].ToInt();
                                tile.Type = (Enums.TileType)parse[25].ToInt();
                                tile.Data1 = parse[26].ToInt();
                                tile.Data2 = parse[27].ToInt();
                                tile.Data3 = parse[28].ToInt();
                                tile.String1 = parse[29];
                                tile.String2 = parse[30];
                                tile.String3 = parse[31];
                                tile.RDungeonMapValue = parse[32].ToInt();
                                tile.GroundSet = parse[33].ToInt();
                                tile.GroundAnimSet = parse[34].ToInt();
                                tile.MaskSet = parse[35].ToInt();
                                tile.AnimSet = parse[36].ToInt();
                                tile.Mask2Set = parse[37].ToInt();
                                tile.M2AnimSet = parse[38].ToInt();
                                tile.Mask3Set = parse[39].ToInt();
                                tile.M3AnimSet = parse[40].ToInt();
                                tile.Mask4Set = parse[41].ToInt();
                                tile.M4AnimSet = parse[42].ToInt();
                                tile.Mask5Set = parse[43].ToInt();
                                tile.M5AnimSet = parse[44].ToInt();
                                tile.FringeSet = parse[45].ToInt();
                                tile.FAnimSet = parse[46].ToInt();
                                tile.Fringe2Set = parse[47].ToInt();
                                tile.F2AnimSet = parse[48].ToInt();
                                tile.Fringe3Set = parse[49].ToInt();
                                tile.F3AnimSet = parse[50].ToInt();
                                tile.Fringe4Set = parse[51].ToInt();
                                tile.F4AnimSet = parse[52].ToInt();
                                tile.Fringe5Set = parse[53].ToInt();
                                tile.F5AnimSet = parse[54].ToInt();
                                tile.DoorOpen = false;
                                map.OriginalTiles[parse[1].ToInt(), parse[2].ToInt()] = (Tile)tile.Clone();

                                // Load tiles into cache
                                tile.GroundGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.GroundSet].GetTileGraphic(tile.Ground);
                                tile.GroundAnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.GroundAnimSet].GetTileGraphic(tile.GroundAnim);
                                tile.MaskGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.MaskSet].GetTileGraphic(tile.Mask);
                                tile.AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.AnimSet].GetTileGraphic(tile.Anim);
                                tile.Mask2Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask2Set].GetTileGraphic(tile.Mask2);
                                tile.M2AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M2AnimSet].GetTileGraphic(tile.M2Anim);
                                tile.Mask3Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask3Set].GetTileGraphic(tile.Mask3);
                                tile.Mask3AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M3AnimSet].GetTileGraphic(tile.M3Anim);
                                tile.Mask4Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask4Set].GetTileGraphic(tile.Mask4);
                                tile.Mask4AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M4AnimSet].GetTileGraphic(tile.M4Anim);
                                tile.Mask5Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask5Set].GetTileGraphic(tile.Mask5);
                                tile.Mask5AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M5AnimSet].GetTileGraphic(tile.M5Anim);
                                tile.FringeGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.FringeSet].GetTileGraphic(tile.Fringe);
                                tile.FAnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.FAnimSet].GetTileGraphic(tile.FAnim);
                                tile.Fringe2Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe2Set].GetTileGraphic(tile.Fringe2);
                                tile.F2AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F2AnimSet].GetTileGraphic(tile.F2Anim);
                                tile.Fringe3Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe3Set].GetTileGraphic(tile.Fringe3);
                                tile.Fringe3AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F3AnimSet].GetTileGraphic(tile.F3Anim);
                                tile.Fringe4Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe4Set].GetTileGraphic(tile.Fringe4);
                                tile.Fringe4AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F4AnimSet].GetTileGraphic(tile.F4Anim);
                                tile.Fringe5Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe5Set].GetTileGraphic(tile.Fringe5);
                                tile.Fringe5AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F5AnimSet].GetTileGraphic(tile.F5Anim);
                            }
                            break;
                    }
                    lineNum++;
                } while (true);
            }
            return map;
        }

        public static void LoadMapFromPacket(string[] parse)
        {
            string mapID = parse[2];
            Maps.Map map = new Client.Logic.Maps.Map(mapID);

            int n = 2;

            //if (parse[13].ToInt() < 19 || parse[14].ToInt() < 14) {
            //    parse[13] = "19";
            //    parse[14] = "14";
            //}

            map.Tile = new Client.Logic.Maps.Tile[parse[13].ToInt() + 1, parse[14].ToInt() + 1];
            map.OriginalTiles = new Tile[parse[13].ToInt() + 1, parse[14].ToInt() + 1];

            Enums.MapID mapType = (Enums.MapID)(parse[1].ToInt());

            map.Name = parse[n + 1];
            map.Revision = parse[n + 2].ToInt();
            map.Moral = (Enums.MapMoral)parse[n + 3].ToInt();
            map.Up = parse[n + 4].ToInt();
            map.Down = parse[n + 5].ToInt();
            map.Left = parse[n + 6].ToInt();
            map.Right = parse[n + 7].ToInt();
            map.Music = parse[n + 8];
            map.Indoors = parse[n + 9].ToBool();
            map.Weather = (Enums.Weather)parse[n + 10].ToInt();
            map.MaxX = parse[n + 11].ToInt();
            map.MaxY = parse[n + 12].ToInt();
            map.Darkness = parse[n + 13].ToInt();
            map.HungerEnabled = parse[n + 14].ToBool();
            map.RecruitEnabled = parse[n + 15].ToBool();
            map.ExpEnabled = parse[n + 16].ToBool();
            map.TimeLimit = parse[n + 17].ToInt();
            map.MinNpcs = parse[n + 18].ToInt();
            map.MaxNpcs = parse[n + 19].ToInt();
            map.NpcSpawnTime = parse[n + 20].ToInt();
            map.Cacheable = parse[n + 21].ToBool();
            map.ImpersonatingMap = parse[n + 22];
            map.YouTubeMusicID = parse[n + 23];
            map.GameplayMode = (Enums.GameplayMode)parse[n + 24].ToInt();
            map.Effect = (Enums.MapEffect)parse[n + 25].ToInt();

            n += 26;

            for (int y = 0; y <= map.MaxY; y++)
            {
                for (int x = 0; x <= map.MaxX; x++)
                {
                    Maps.Tile tile = new Client.Logic.Maps.Tile();
                    tile.Ground = parse[n].ToInt();
                    tile.GroundAnim = parse[n + 1].ToInt();
                    tile.Mask = parse[n + 2].ToInt();
                    tile.Anim = parse[n + 3].ToInt();
                    tile.Mask2 = parse[n + 4].ToInt();
                    tile.M2Anim = parse[n + 5].ToInt();
                    tile.Mask3 = parse[n + 6].ToInt();
                    tile.M3Anim = parse[n + 7].ToInt();
                    tile.Mask4 = parse[n + 8].ToInt();
                    tile.M4Anim = parse[n + 9].ToInt();
                    tile.Mask5 = parse[n + 10].ToInt();
                    tile.M5Anim = parse[n + 11].ToInt();
                    tile.Fringe = parse[n + 12].ToInt();
                    tile.FAnim = parse[n + 13].ToInt();
                    tile.Fringe2 = parse[n + 14].ToInt();
                    tile.F2Anim = parse[n + 15].ToInt();
                    tile.Fringe3 = parse[n + 16].ToInt();
                    tile.F3Anim = parse[n + 17].ToInt();
                    tile.Fringe4 = parse[n + 18].ToInt();
                    tile.F4Anim = parse[n + 19].ToInt();
                    tile.Fringe5 = parse[n + 20].ToInt();
                    tile.F5Anim = parse[n + 21].ToInt();
                    tile.Type = (Enums.TileType)parse[n + 22].ToInt();
                    tile.Data1 = parse[n + 23].ToInt();
                    tile.Data2 = parse[n + 24].ToInt();
                    tile.Data3 = parse[n + 25].ToInt();
                    tile.String1 = parse[n + 26];
                    tile.String2 = parse[n + 27];
                    tile.String3 = parse[n + 28];
                    tile.RDungeonMapValue = parse[n + 29].ToInt();
                    tile.GroundSet = parse[n + 30].ToInt();
                    tile.GroundAnimSet = parse[n + 31].ToInt();
                    tile.MaskSet = parse[n + 32].ToInt();
                    tile.AnimSet = parse[n + 33].ToInt();
                    tile.Mask2Set = parse[n + 34].ToInt();
                    tile.M2AnimSet = parse[n + 35].ToInt();
                    tile.Mask3Set = parse[n + 36].ToInt();
                    tile.M3AnimSet = parse[n + 37].ToInt();
                    tile.Mask4Set = parse[n + 38].ToInt();
                    tile.M4AnimSet = parse[n + 39].ToInt();
                    tile.Mask5Set = parse[n + 40].ToInt();
                    tile.M5AnimSet = parse[n + 41].ToInt();
                    tile.FringeSet = parse[n + 42].ToInt();
                    tile.FAnimSet = parse[n + 43].ToInt();
                    tile.Fringe2Set = parse[n + 44].ToInt();
                    tile.F2AnimSet = parse[n + 45].ToInt();
                    tile.Fringe3Set = parse[n + 46].ToInt();
                    tile.F3AnimSet = parse[n + 47].ToInt();
                    tile.Fringe4Set = parse[n + 48].ToInt();
                    tile.F4AnimSet = parse[n + 49].ToInt();
                    tile.Fringe5Set = parse[n + 50].ToInt();
                    tile.F5AnimSet = parse[n + 51].ToInt();
                    map.Tile[x, y] = tile;
                    map.OriginalTiles[x, y] = (Tile)tile.Clone();
                    n += 52;

                    // Load tiles into cache
                    tile.GroundGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.GroundSet].GetTileGraphic(tile.Ground);
                    tile.GroundAnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.GroundAnimSet].GetTileGraphic(tile.GroundAnim);
                    tile.MaskGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.MaskSet].GetTileGraphic(tile.Mask);
                    tile.AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.AnimSet].GetTileGraphic(tile.Anim);
                    tile.Mask2Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask2Set].GetTileGraphic(tile.Mask2);
                    tile.M2AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M2AnimSet].GetTileGraphic(tile.M2Anim);
                    tile.Mask3Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask3Set].GetTileGraphic(tile.Mask3);
                    tile.Mask3AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M3AnimSet].GetTileGraphic(tile.M3Anim);
                    tile.Mask4Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask4Set].GetTileGraphic(tile.Mask4);
                    tile.Mask4AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M4AnimSet].GetTileGraphic(tile.M4Anim);
                    tile.Mask5Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Mask5Set].GetTileGraphic(tile.Mask5);
                    tile.Mask5AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.M5AnimSet].GetTileGraphic(tile.M5Anim);
                    tile.FringeGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.FringeSet].GetTileGraphic(tile.Fringe);
                    tile.FAnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.FAnimSet].GetTileGraphic(tile.FAnim);
                    tile.Fringe2Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe2Set].GetTileGraphic(tile.Fringe2);
                    tile.F2AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F2AnimSet].GetTileGraphic(tile.F2Anim);
                    tile.Fringe3Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe3Set].GetTileGraphic(tile.Fringe3);
                    tile.Fringe3AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F3AnimSet].GetTileGraphic(tile.F3Anim);
                    tile.Fringe4Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe4Set].GetTileGraphic(tile.Fringe4);
                    tile.Fringe4AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F4AnimSet].GetTileGraphic(tile.F4Anim);
                    tile.Fringe5Graphic = Logic.Graphics.GraphicsManager.Tiles[tile.Fringe5Set].GetTileGraphic(tile.Fringe5);
                    tile.Fringe5AnimGraphic = Logic.Graphics.GraphicsManager.Tiles[tile.F5AnimSet].GetTileGraphic(tile.F5Anim);
                }
            }

            int npcs = parse[n].ToInt();

            n++;

            for (int i = 0; i < npcs; i++)
            {
                map.Npc.Add(new MapNpcSettings());
                map.Npc[i].NpcNum = parse[n].ToInt();
                map.Npc[i].SpawnX = parse[n + 1].ToInt();
                map.Npc[i].SpawnY = parse[n + 2].ToInt();
                map.Npc[i].MinLevel = parse[n + 3].ToInt();
                map.Npc[i].MaxLevel = parse[n + 4].ToInt();
                map.Npc[i].AppearanceRate = parse[n + 5].ToInt();
                map.Npc[i].StartStatus = (Enums.StatusAilment)parse[n + 6].ToInt();
                map.Npc[i].StartStatusCounter = parse[n + 7].ToInt();
                map.Npc[i].StartStatusChance = parse[n + 8].ToInt();
                n += 9;
            }

            bool temp = parse[n].ToBool();

            map.MapID = mapID;

            if (map.Cacheable && !temp && (mapID.StartsWith("s") || mapID.StartsWith("h")))
            {
                mMaps[mapType] = map;
                SaveLocalMap(mapType);
                //mMaps[Enums.MapID.TempActive] = map;
                //SaveLocalMap(Enums.MapID.TempActive);
            }
            else
            {
                mMaps[mapType] = map;
                //mMaps[Enums.MapID.TempActive] = map;
                //				TODO: Random dungeon loading info hiding
                //				RMapLoaded = True
                //				If RMapInfoDone Then
                //				GameInstance.RDungeonInfo.Hide()
                //				RMapLoaded = False
                //				RMapInfoDone = False
                //				End If
            }

            if (mapType == Enums.MapID.TempActive)
            {
                PlayerManager.MyPlayer.MapID = mapID;

                // Close editors [Map + House editor]
                Windows.Editors.EditorManager.CloseMapEditor();
            }
            /*
            switch (map.Weather) {
                case Enums.Weather.None:
                    Globals.ActiveTime = Globals.GameTime;
                    Globals.ActiveWeather = Globals.GameWeather;
                    break;
                case Enums.Weather.Raining:
                    Globals.ActiveWeather = Enums.Weather.Raining;
                    Globals.ActiveTime = Globals.GameTime;
                    break;
                case Enums.Weather.Snowing:
                    Globals.ActiveWeather = Enums.Weather.Snowing;
                    Globals.ActiveTime = Globals.GameTime;
                    break;
                case Enums.Weather.Thunder:
                    Globals.ActiveWeather = Enums.Weather.Thunder;
                    Globals.ActiveTime = Globals.GameTime;
                    break;
            }
            */

            //Globals.ActiveWeather = map.Weather;

            Globals.ActiveTime = Globals.GameTime;

            Globals.SavingMap = false;
        }

        public static void UpdateTile(string[] parse)
        {
            int n = 3;

            Maps.Tile tile = new Client.Logic.Maps.Tile();
            tile.Ground = parse[n].ToInt();
            tile.GroundAnim = parse[n + 1].ToInt();
            tile.Mask = parse[n + 2].ToInt();
            tile.Anim = parse[n + 3].ToInt();
            tile.Mask2 = parse[n + 4].ToInt();
            tile.M2Anim = parse[n + 5].ToInt();
            tile.Mask3 = parse[n + 6].ToInt();
            tile.M3Anim = parse[n + 7].ToInt();
            tile.Mask4 = parse[n + 8].ToInt();
            tile.M4Anim = parse[n + 9].ToInt();
            tile.Mask5 = parse[n + 10].ToInt();
            tile.M5Anim = parse[n + 11].ToInt();
            tile.Fringe = parse[n + 12].ToInt();
            tile.FAnim = parse[n + 13].ToInt();
            tile.Fringe2 = parse[n + 14].ToInt();
            tile.F2Anim = parse[n + 15].ToInt();
            tile.Fringe3 = parse[n + 16].ToInt();
            tile.F3Anim = parse[n + 17].ToInt();
            tile.Fringe4 = parse[n + 18].ToInt();
            tile.F4Anim = parse[n + 19].ToInt();
            tile.Fringe5 = parse[n + 20].ToInt();
            tile.F5Anim = parse[n + 21].ToInt();
            tile.Type = (Enums.TileType)parse[n + 22].ToInt();
            tile.Data1 = parse[n + 23].ToInt();
            tile.Data2 = parse[n + 24].ToInt();
            tile.Data3 = parse[n + 25].ToInt();
            tile.String1 = parse[n + 26];
            tile.String2 = parse[n + 27];
            tile.String3 = parse[n + 28];
            tile.RDungeonMapValue = parse[n + 29].ToInt();
            tile.GroundSet = parse[n + 30].ToInt();
            tile.GroundAnimSet = parse[n + 31].ToInt();
            tile.MaskSet = parse[n + 32].ToInt();
            tile.AnimSet = parse[n + 33].ToInt();
            tile.Mask2Set = parse[n + 34].ToInt();
            tile.M2AnimSet = parse[n + 35].ToInt();
            tile.Mask3Set = parse[n + 36].ToInt();
            tile.M3AnimSet = parse[n + 37].ToInt();
            tile.Mask4Set = parse[n + 38].ToInt();
            tile.M4AnimSet = parse[n + 39].ToInt();
            tile.Mask5Set = parse[n + 40].ToInt();
            tile.M5AnimSet = parse[n + 41].ToInt();
            tile.FringeSet = parse[n + 42].ToInt();
            tile.FAnimSet = parse[n + 43].ToInt();
            tile.Fringe2Set = parse[n + 44].ToInt();
            tile.F2AnimSet = parse[n + 45].ToInt();
            tile.Fringe3Set = parse[n + 46].ToInt();
            tile.F3AnimSet = parse[n + 47].ToInt();
            tile.Fringe4Set = parse[n + 48].ToInt();
            tile.F4AnimSet = parse[n + 49].ToInt();
            tile.Fringe5Set = parse[n + 50].ToInt();
            tile.F5AnimSet = parse[n + 51].ToInt();

            mMaps[Enums.MapID.Active].Tile[parse[1].ToInt(), parse[2].ToInt()] = tile;
            Windows.WindowSwitcher.GameWindow.MapViewer.ActiveMap = mMaps[Enums.MapID.Active];
        }

        public static Map ActiveMap
        {
            get
            {
                return mMaps[Enums.MapID.Active];
            }
        }

        public static void SaveLocalMap(Enums.MapID mapID)
        {
            SaveLocalMap(mMaps[mapID]);
        }

        public static void SaveLocalMap(Map map)
        {
            string filePath = IO.Paths.CreateOSPath(IO.Paths.MapPath + "Map-" + map.MapID.ToString() + ".dat");
            StringBuilder writer = new StringBuilder();

            writer.AppendLine("MapData|V13|" + map.Revision.ToString() + "|" + map.MaxX.ToString() + "|" + map.MaxY.ToString() + "|");
            writer.AppendLine("Data|" + map.Name + "|" + ((int)map.Moral).ToString() + "|" + map.Up.ToString() + "|" + map.Down.ToString() + "|" +
                              map.Left.ToString() + "|" + map.Right.ToString() + "|" + map.Music + "|" + map.Indoors.ToString() +
                              "|" + map.Owner + "|" + ((int)map.Weather).ToString() + "|" + map.Darkness.ToString() + "|" +
                              map.HungerEnabled.ToIntString() + "|" + map.RecruitEnabled.ToIntString() + "|" + map.ExpEnabled.ToIntString() + "|" + map.TimeLimit.ToString() + "|" + map.DungeonIndex.ToString() + "|" + map.MinNpcs.ToString() + "|" + map.MaxNpcs.ToString() + "|" +
                              map.NpcSpawnTime.ToString() + "|" + map.Instanced.ToIntString() + "|" + map.YouTubeMusicID + "|" + ((int)map.GameplayMode).ToString() + "|" + ((int)map.Effect).ToString() + "|");
            //string npcData = "NpcData|";
            //string spawnXData = "SpawnX|";
            //string spawnYData = "SpawnY|";
            for (int i = 0; i < map.Npc.Count; i++)
            {
                writer.AppendLine("NpcSettings|" + i.ToString() + "|" + map.Npc[i].NpcNum + "|" + map.Npc[i].SpawnX + "|" + map.Npc[i].SpawnY + "|"
                                + map.Npc[i].MinLevel + "|" + map.Npc[i].MaxLevel + "|" + map.Npc[i].AppearanceRate + "|"
                                + (int)map.Npc[i].StartStatus + "|" + map.Npc[i].StartStatusCounter + "|" + map.Npc[i].StartStatusChance + "|");
            }
            //writer.AppendLine(npcData);
            //writer.AppendLine(spawnXData);
            //writer.AppendLine(spawnYData);
            for (int x = 0; x <= map.MaxX; x++)
            {
                for (int y = 0; y <= map.MaxY; y++)
                {
                    Tile tile = map.Tile[x, y];
                    writer.AppendLine("Tile|" + x + "|" + y + "|" + tile.Ground + "|" + tile.GroundAnim + "|" +
                        tile.Mask + "|" + tile.Anim + "|" + tile.Mask2 + "|" + tile.M2Anim + "|" + tile.Mask3 + "|" + tile.M3Anim + "|" + tile.Mask4 + "|" + tile.M4Anim + "|" + tile.Mask5 + "|" + tile.M5Anim + "|" +
                        tile.Fringe + "|" + tile.FAnim + "|" + tile.Fringe2 + "|" + tile.F2Anim + "|" + tile.Fringe3 + "|" + tile.F3Anim + "|" + tile.Fringe4 + "|" + tile.F4Anim + "|" + tile.Fringe5 + "|" + tile.F5Anim + "|" +
                        ((int)tile.Type).ToString() + "|" + tile.Data1 + "|" + tile.Data2 + "|" + tile.Data3 + "|" + tile.String1 + "|" + tile.String2 + "|" + tile.String3 + "|" + tile.RDungeonMapValue + "|" +
                        tile.GroundSet + "|" + tile.GroundAnimSet + "|" +
                        tile.MaskSet + "|" + tile.AnimSet + "|" + tile.Mask2Set + "|" + tile.M2AnimSet + "|" + tile.Mask3Set + "|" + tile.M3AnimSet + "|" + tile.Mask4Set + "|" + tile.M4AnimSet + "|" + tile.Mask5Set + "|" + tile.M5AnimSet + "|" +
                        tile.FringeSet + "|" + tile.FAnimSet + "|" + tile.Fringe2Set + "|" + tile.F2AnimSet + "|" + tile.Fringe3Set + "|" + tile.F3AnimSet + "|" + tile.Fringe4Set + "|" + tile.F4AnimSet + "|" + tile.Fringe5Set + "|" + tile.F5AnimSet + "|");
                }
            }

            System.IO.File.WriteAllBytes(filePath, Globals.Encryption.EncryptBytes(System.Text.Encoding.Unicode.GetBytes(writer.ToString())));
            //System.IO.File.WriteAllText(filePath, writer.ToString());
        }

        #endregion Methods

        public static MapCollection Maps
        {
            get { return mMaps; }
        }

        public static void HandleMapDone()
        {
            WindowSwitcher.GameWindow.MapViewer.ActiveMap = ActiveMap;

            PlayerManager.MyPlayer.MapID = ActiveMap.MapID;
            ActiveMap.DoOverlayChecks();
            if (PlayerManager.MyPlayer.Darkness > -2)
            {
                Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetDarkness(PlayerManager.MyPlayer.Darkness);
            }
            else
            {
                Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetDarkness(ActiveMap.Darkness);
            }
            Logic.Graphics.Renderers.Screen.ScreenRenderer.DeactivateOffscreenSprites();
            //Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetOverlay((Enums.Overlay)Maps.MapHelper.ActiveMap.Overlay);
            //Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetOverlay(Enums.Overlay.Sandstorm);
            //Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.SetWea(Maps.MapHelper.ActiveMap.Weather);

            Logic.Graphics.Effects.Overlays.ScreenOverlays.MapChangeInfoOverlay infoOverlay = Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.ScreenOverlay as Logic.Graphics.Effects.Overlays.ScreenOverlays.MapChangeInfoOverlay;
            if (infoOverlay != null)
            {
                if (infoOverlay.MinTimePassed)
                {
                    Logic.Graphics.Renderers.Screen.ScreenRenderer.RenderOptions.ScreenOverlay = null;
                }
            }
            PlayerManager.MyPlayer.SetCurrentRoom();
            //Music.Music.AudioPlayer.PlayMusic(ActiveMap.Music);
            ((Client.Logic.Music.Bass.BassAudioPlayer)Logic.Music.Music.AudioPlayer).FadeToNext(ActiveMap.Music, 1000);

            if (Stories.StoryProcessor.ActiveStory != null && Stories.StoryProcessor.ActiveStory.Segments[Stories.StoryProcessor.ActiveStory.State.CurrentSegment].Action == Enums.StoryAction.Warp)
            {
                if (ActiveMap.MapID == ((Stories.Segments.WarpSegment)Stories.StoryProcessor.ActiveStory.Segments[Stories.StoryProcessor.ActiveStory.State.CurrentSegment]).Map)
                {
                    if (Stories.StoryProcessor.ActiveStory.State.StoryPaused)
                    {
                        Stories.StoryProcessor.ActiveStory.State.Unpause();
                        Stories.StoryProcessor.ActiveStory.State.StoryPaused = false;
                    }
                }
            }
            Globals.GettingMap = false;
            Globals.RefreshLock = false;

            Messenger.SendMapLoaded();
        }
    }
}
