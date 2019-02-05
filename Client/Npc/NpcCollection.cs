using System;
using System.Collections.Generic;
using System.Text;
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


namespace Client.Logic.Npc
{
    class NpcCollection
    {
        #region Fields

        private List<Npc> mNpcArray;

        #endregion Fields

        #region Constructors

        internal NpcCollection(int maxNpcs)
        {
            mNpcArray = new List<Npc>(maxNpcs);
        }

        #endregion Constructors

        #region Indexers

        public Npc this[int index]
        {
            get { return mNpcArray[index - 1]; }
            set { mNpcArray[index - 1] = value; }
        }

        #endregion Indexers


        public void AddNpc(string name)
        {
            Npc npc = new Npc();
            npc.Name = name;
            npc.AttackSay = "";
            npc.AttackSay2 = "";
            npc.AttackSay3 = "";
            npc.Behavior = Enums.NpcBehavior.AttackOnSight;
            npc.ShinyChance = 0;
            for (int a = 0; a < MaxInfo.MAX_NPC_DROPS; a++)
            {
                npc.ItemDrops[a] = new NpcDrop();
                npc.ItemDrops[a].Chance = 0;
                npc.ItemDrops[a].ItemNum = 0;
                npc.ItemDrops[a].ItemValue = 0;
            }

            mNpcArray.Add(npc);
        }
    }
}
