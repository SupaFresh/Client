using System;
using System.Collections.Generic;
using System.Text;
using PMDCP.Core;
using Client.Logic.Stories.Segments;
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

namespace Client.Logic.Stories
{
    class Segment
    {
        ListPair<string, string> parameters;

        public Segment()
        {
            parameters = new ListPair<string, string>();
        }

        #region Properties

        public Enums.StoryAction Action
        {
            get;
            set;
        }

        public ListPair<string, string> Parameters
        {
            get { return parameters; }
        }

        public void AddParameter(string paramID, string value)
        {
            parameters.Add(paramID, value);
        }

        public ISegment ToSpecific()
        {
            ISegment specific = null;
            switch (Action)
            {
                case Enums.StoryAction.Say:
                    specific = new SaySegment();
                    break;
                case Enums.StoryAction.AskQuestion:
                    specific = new AskQuestionSegment();
                    break;
                case Enums.StoryAction.HideBackground:
                    specific = new HideBackgroundSegment();
                    break;
                case Enums.StoryAction.HideImage:
                    specific = new HideImageSegment();
                    break;
                case Enums.StoryAction.MapVisibility:
                    specific = new MapVisibilitySegment();
                    break;
                case Enums.StoryAction.Padlock:
                    specific = new PadlockSegment();
                    break;
                case Enums.StoryAction.Pause:
                    specific = new PauseSegment();
                    break;
                case Enums.StoryAction.PlayerPadlock:
                    specific = new PlayerPadlockSegment();
                    break;
                case Enums.StoryAction.PlayMusic:
                    specific = new PlayMusicSegment();
                    break;
                case Enums.StoryAction.ShowBackground:
                    specific = new ShowBackgroundSegment();
                    break;
                case Enums.StoryAction.ShowImage:
                    specific = new ShowImageSegment();
                    break;
                case Enums.StoryAction.StopMusic:
                    specific = new StopMusicSegment();
                    break;
                case Enums.StoryAction.Warp:
                    specific = new WarpSegment();
                    break;
                case Enums.StoryAction.CreateFNPC:
                    specific = new CreateFNPCSegment();
                    break;
                case Enums.StoryAction.MoveFNPC:
                    specific = new MoveFNPCSegment();
                    break;
                case Enums.StoryAction.WarpFNPC:
                    specific = new WarpFNPCSegment();
                    break;
                case Enums.StoryAction.ChangeFNPCDir:
                    specific = new ChangeFNPCDirectionSegment();
                    break;
                case Enums.StoryAction.DeleteFNPC:
                    specific = new DeleteFNPCSegment();
                    break;
                case Enums.StoryAction.RunScript:
                    specific = new RunScriptSegment();
                    break;
                case Enums.StoryAction.HidePlayers:
                    specific = new HidePlayersSegment();
                    break;
                case Enums.StoryAction.ShowPlayers:
                    specific = new ShowPlayersSegment();
                    break;
                case Enums.StoryAction.GoToSegment:
                    specific = new GoToSegmentSegment();
                    break;
                case Enums.StoryAction.MovePlayer:
                    specific = new MovePlayerSegment();
                    break;
                case Enums.StoryAction.ChangePlayerDir:
                    specific = new ChangePlayerDirectionSegment();
                    break;
                case Enums.StoryAction.PlayerSay:
                    specific = new PlayerSaySegment();
                    break;
                case Enums.StoryAction.SpawnNPC:
                    specific = new SpawnNPCSegment();
                    break;
                case Enums.StoryAction.UnblockTile:
                    specific = new UnblockTileSegment();
                    break;
            }
            if (specific != null)
            {
                specific.LoadFromSegmentData(parameters);
            }
            return specific;
        }

        #endregion Properties
    }
}
