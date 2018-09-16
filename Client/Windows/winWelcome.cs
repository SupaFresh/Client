using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Widgets;

namespace Client.Logic.Windows
{
    class winWelcome : Core.WindowCore
    {
        Action postWelcomeLoad;

        Label lblWelcome;
        Label lblServerIP;
        TextBox txtServerIP;

        Button btnStart;

        public winWelcome(Action postWelcomeLoad) : base("winWelcome")
        {
            this.postWelcomeLoad = postWelcomeLoad;

            this.Windowed = true;
            this.ShowInWindowSwitcher = false;
            this.TitleBar.Text = "Welcome";
            this.TitleBar.CloseButton.Visible = false;
            //this.BackgroundImage = Skins.SkinManager.LoadGui("Credits");
            //this.Size = this.BackgroundImage.Size;
            this.BackColor = Color.White;
            this.Size = new Size(400, 225);
            this.Location = DrawingSupport.GetCenter(SdlDotNet.Graphics.Video.Screen.Size, this.Size);

            lblWelcome = new Label("lblWelcome");
            lblWelcome.Font = Graphics.FontManager.LoadFont("PMDCP", 22);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(30, 20);
            lblWelcome.Text = $"Welcome to {Constants.GameName}!\n\nTo get started, fill in the settings below:";

            lblServerIP = new Label("lblServerIP");
            lblServerIP.Font = Graphics.FontManager.LoadFont("PMDCP", 18);
            lblServerIP.AutoSize = true;
            lblServerIP.Location = new Point(30, 100);
            lblServerIP.Text = "Server:";

            txtServerIP = new TextBox("txtServerIP");
            txtServerIP.Font = Graphics.FontManager.LoadFont("PMDCP", 18);
            txtServerIP.Size = new Size(300, 20);
            txtServerIP.Location = new Point(40, 120);

            btnStart = new Button("btnStart");
            btnStart.Font = Graphics.FontManager.LoadFont("PMDCP", 18);
            btnStart.Text = "Start";
            btnStart.Size = new Size(100, 30);
            btnStart.Location = new Point(DrawingSupport.GetCenter(this.Size, btnStart.Size).X, 160);
            btnStart.Click += BtnStart_Click;

            this.AddWidget(lblWelcome);
            this.AddWidget(lblServerIP);
            this.AddWidget(txtServerIP);
            this.AddWidget(btnStart);
        }

        private void BtnStart_Click(object sender, MouseButtonEventArgs e)
        {
            IO.Options.ConnectionIP = txtServerIP.Text;
            IO.Options.SaveXml();

            postWelcomeLoad();
            this.Close();
        }
    }
}
