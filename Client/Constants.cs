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


namespace Client.Logic
{
    class Constants
    {
        #region Fields

        public const string CLIENT_EDITION = "PMDSCore";
        public const int CLIENT_VERSION = 6;
        public const int FRAME_RATE = 30;
        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;

        //public const string SEC_CODE1 = "12345678";
        //public const string SEC_CODE2 = "fsvsdfvl";
        //public const string SEC_CODE3 = "*&9694ng";
        //public const string SEC_CODE4 = "^78fvd)!";
        public const string SEC_CODE1 = "sdbsiadobascbsac874hfbnerfcwea9cvv9ehrvndcsdicnsducbsucbsdc";
        public const string SEC_CODE2 = "bfmpdgobmdfbdfvndifvsdnfpsaubcdonsicoaskcsmlrjsdicnjsodciwsncsacdassdc";
        public const string SEC_CODE3 = "blahblahblahblahblahblahblahblahblahblahblahhablahblahblahblah";
        public const string SEC_CODE4 = "4782934742639475264072365275462075256035732625421652164126432107460264732642";

        public static readonly int MovementClusteringFrquency = 10;

        public const string GameName = "Pokemon Mystery Dungeon: Shift!";
        public const string GameNameShort = "PMDS";

        #endregion Fields
    }
}
