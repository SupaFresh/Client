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

using SdlDotNet.Widgets;
using System.Drawing;

namespace Client.Logic.Windows
{
    class winAccountSettings : Core.WindowCore
    {
        Button btnChangePassword;
        Label lblBack;

        public winAccountSettings()
            : base("winAccountSettings")
        {
            this.Windowed = true;
            this.ShowInWindowSwitcher = false;
            this.TitleBar.Text = "Account Settings";
            this.TitleBar.CloseButton.Visible = false;
            this.Size = new Size(260, 200);
            //this.BackgroundImage = Skins.SkinManager.LoadGui("Account Settings");
            this.Location = new Point(DrawingSupport.GetCenter(SdlDotNet.Graphics.Video.Screen.Size, this.Size).X, 5);

            btnChangePassword = new Button("btnChangePassword");
            btnChangePassword.Font = Graphics.FontManager.LoadFont("PMDCP", 16);
            btnChangePassword.Text = "Change Password";
            btnChangePassword.Location = new Point(65, 55);
            btnChangePassword.Size = new System.Drawing.Size(130, 32);
            btnChangePassword.Click += new EventHandler<MouseButtonEventArgs>(btnChangePassword_Click);

            lblBack = new Label("lblBack");
            lblBack.Font = Graphics.FontManager.LoadFont("PMDCP", 32);
            lblBack.Text = "Return to Login Screen";
            lblBack.Location = new Point(25, 100);
            lblBack.AutoSize = true;
            lblBack.ForeColor = Color.Black;
            lblBack.Click += new EventHandler<MouseButtonEventArgs>(lblBack_Click);

            this.AddWidget(btnChangePassword);
            this.AddWidget(lblBack);

            this.LoadComplete();
        }

        void btnChangePassword_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            this.Close();
            WindowManager.AddWindow(new winChangePassword());
        }

        void lblBack_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            this.Close();
            WindowSwitcher.ShowMainMenu();
        }
    }
}
