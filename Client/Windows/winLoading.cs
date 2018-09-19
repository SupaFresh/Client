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


namespace Client.Logic.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    using Gfx = Client.Logic.Graphics;
    //using Gui = Client.Logic.Gui;
    using SdlDotNet.Widgets;

    class winLoading : Core.WindowCore
    {
        #region Fields

        Label lblInfo;

        #endregion Fields

        #region Constructors

        public winLoading()
            : base("winLoading")
        {
            this.BackgroundImageSizeMode = ImageSizeMode.AutoSize;
            this.BackgroundImage = Skins.SkinManager.LoadGui("Loading Bar");
            this.Location = Gfx.DrawingSupport.GetCenter(SdlDotNet.Graphics.Video.Screen.Size, this.Size);
            this.Windowed = false;
            this.Name = "winLoading";
            this.ShowInWindowSwitcher = false;

            lblInfo = new Label("lblInfo");
            lblInfo.Font = Gfx.FontManager.LoadFont("PMDCP", 32);
            lblInfo.Location = new Point(35, 10);
            lblInfo.AutoSize = false;
            lblInfo.AntiAlias = false;
            lblInfo.Size = new Size(230, 32);
            lblInfo.BackColor = Color.Transparent;
            lblInfo.ForeColor = Color.Black;

            this.AddWidget(lblInfo);

            this.LoadComplete();
        }

        #endregion Constructors

        #region Methods

        public void UpdateLoadText(string newText)
        {
            lblInfo.Text = newText;
        }

        #endregion Methods
    }
}
