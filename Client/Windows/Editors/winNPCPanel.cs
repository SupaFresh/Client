﻿// This file is part of Mystery Dungeon eXtended.

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
using System.Drawing;
using SdlDotNet.Widgets;
using Client.Logic.Network;
using PMDCP.Sockets;
using PMDCP.Core;

namespace Client.Logic.Windows.Editors
{
    class winNPCPanel : Core.WindowCore
    {
        #region Fields

        int itemNum = -1;
        int currentTen = 0;

        Panel pnlNPCList;
        public Panel pnlNPCEditor;

        ListBox lbxNPCList;
        ListBoxTextItem lbiArrow;
        Button btnBack;
        Button btnForward;
        Button btnCancel;
        Button btnEdit;

        Button btnEditorCancel;
        Button btnEditorOK;

        Label lblName;
        TextBox txtName;
        Label lblAttackSay;
        TextBox txtAttackSay;
        Label lblAttackSay2;
        TextBox txtAttackSay2;
        Label lblAttackSay3;
        TextBox txtAttackSay3;
        Label lblForm;
        NumericUpDown nudForm;
        Label lblSpecies;
        NumericUpDown nudSpecies;
        Label lblRange;
        NumericUpDown nudShinyChance;
        CheckBox chkSpawnsAtDawn;
        CheckBox chkSpawnsAtDay;
        CheckBox chkSpawnsAtDusk;
        CheckBox chkSpawnsAtNight;
        Label lblBehaviour;
        ComboBox cmbBehaviour;
        Label lblRecruitRate;
        NumericUpDown nudRecruitRate;
        Label[] lblMove;
        Label[] lblMoveInfo;
        NumericUpDown[] nudMove;
        Label lblDropSelector;
        NumericUpDown nudDropSelector;
        Label lblDropItemNum;
        NumericUpDown nudDropItemNum;
        Label lblDropItemAmount;
        NumericUpDown nudDropItemAmount;
        Label lblDropItemChance;
        NumericUpDown nudDropItemChance;
        Label lblDropItemTag;
        TextBox txtDropItemTag;

        Logic.Editors.NPCs.EditableNPC npc;

        #endregion Fields

        #region Constructors
        public winNPCPanel()
            : base("winNPCPanel")
        {
            this.Windowed = true;
            this.ShowInWindowSwitcher = false;
            this.Size = new System.Drawing.Size(200, 270);
            this.Location = new System.Drawing.Point(210, WindowSwitcher.GameWindow.ActiveTeam.Y + WindowSwitcher.GameWindow.ActiveTeam.Height + 0);
            this.AlwaysOnTop = true;
            this.TitleBar.CloseButton.Visible = true;
            this.TitleBar.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            this.TitleBar.Text = "NPC Panel";

            pnlNPCList = new Panel("pnlNPCList");
            pnlNPCList.Size = new System.Drawing.Size(200, 270);
            pnlNPCList.Location = new Point(0, 0);
            pnlNPCList.BackColor = Color.White;
            pnlNPCList.Visible = true;

            pnlNPCEditor = new Panel("pnlNPCEditor");
            pnlNPCEditor.Size = new System.Drawing.Size(300, 460);
            pnlNPCEditor.Location = new Point(0, 0);
            pnlNPCEditor.BackColor = Color.White;
            pnlNPCEditor.Visible = false;

            #region NPC Selector

            lbxNPCList = new ListBox("lbxNPCList");
            lbxNPCList.Location = new Point(10, 10);
            lbxNPCList.Size = new Size(180, 140);
            for (int i = 0; i < 10; i++)
            {
                ListBoxTextItem lbiNPC = new ListBoxTextItem(Graphics.FontManager.LoadFont("tahoma", 10), (i + 1) + ": " + Npc.NpcHelper.Npcs[(i + 1) + 10 * currentTen].Name);
                lbxNPCList.Items.Add(lbiNPC);
            }

            btnBack = new Button("btnBack");
            btnBack.Location = new Point(10, 160);
            btnBack.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            btnBack.Size = new System.Drawing.Size(64, 16);
            btnBack.Visible = true;
            btnBack.Text = "<--";
            btnBack.Click += new EventHandler<MouseButtonEventArgs>(btnBack_Click);

            btnForward = new Button("btnForward");
            btnForward.Location = new Point(126, 160);
            btnForward.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            btnForward.Size = new System.Drawing.Size(64, 16);
            btnForward.Visible = true;
            btnForward.Text = "-->";
            btnForward.Click += new EventHandler<MouseButtonEventArgs>(btnForward_Click);

            btnEdit = new Button("btnEdit");
            btnEdit.Location = new Point(10, 190);
            btnEdit.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            btnEdit.Size = new System.Drawing.Size(64, 16);
            btnEdit.Visible = true;
            btnEdit.Text = "Edit";
            btnEdit.Click += new EventHandler<MouseButtonEventArgs>(btnEdit_Click);

            btnCancel = new Button("btnCancel");
            btnCancel.Location = new Point(126, 190);
            btnCancel.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            btnCancel.Size = new System.Drawing.Size(64, 16);
            btnCancel.Visible = true;
            btnCancel.Text = "Cancel";
            btnCancel.Click += new EventHandler<MouseButtonEventArgs>(btnCancel_Click);

            #endregion

            #region NPC Editor

            lblName = new Label("lblName");
            lblName.Font = Graphics.FontManager.LoadFont("Tahoma", 10);
            lblName.Text = "NPC Name:";
            lblName.AutoSize = true;
            lblName.Location = new Point(10, 5);

            txtName = new TextBox("txtName");
            txtName.Size = new Size(200, 15);
            txtName.Location = new Point(75, 5);
            txtName.Font = Graphics.FontManager.LoadFont("Tahoma", 12);
            txtName.Text = "";

            lblAttackSay = new Label("lblAttackSay");
            lblAttackSay.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblAttackSay.Text = "Attack Say:";
            lblAttackSay.AutoSize = true;
            lblAttackSay.Location = new Point(10, 25);

            txtAttackSay = new TextBox("txtAttackSay");
            txtAttackSay.Size = new System.Drawing.Size(200, 15);
            txtAttackSay.Location = new Point(75, 25);
            txtAttackSay.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 12);

            lblAttackSay2 = new Label("lblAttackSay2");
            lblAttackSay2.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblAttackSay2.Text = "Attack Say2:";
            lblAttackSay2.AutoSize = true;
            lblAttackSay2.Location = new Point(10, 45);

            txtAttackSay2 = new TextBox("txtAttackSay2");
            txtAttackSay2.Size = new System.Drawing.Size(200, 15);
            txtAttackSay2.Location = new Point(75, 45);
            txtAttackSay2.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 12);

            lblAttackSay3 = new Label("lblAttackSay3");
            lblAttackSay3.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblAttackSay3.Text = "Attack Say3:";
            lblAttackSay3.AutoSize = true;
            lblAttackSay3.Location = new Point(10, 65);

            txtAttackSay3 = new TextBox("txtAttackSay3");
            txtAttackSay3.Size = new System.Drawing.Size(200, 15);
            txtAttackSay3.Location = new Point(75, 65);
            txtAttackSay3.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 12);

            lblForm = new Label("lblForm");
            lblForm.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblForm.Text = "Form:";
            lblForm.AutoSize = true;
            lblForm.Location = new Point(10, 85);

            nudForm = new NumericUpDown("nudForm");
            nudForm.Maximum = 1000;
            nudForm.Location = new Point(75, 85);
            nudForm.Size = new System.Drawing.Size(200, 15);
            nudForm.ValueChanged += new EventHandler<ValueChangedEventArgs>(nudForm_ValueChanged);

            lblSpecies = new Label("lblSpecies");
            lblSpecies.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            lblSpecies.Text = "Species:";
            lblSpecies.AutoSize = true;
            lblSpecies.Location = new Point(10, 105);

            nudSpecies = new NumericUpDown("nudSpecies");
            nudSpecies.Location = new Point(75, 105);
            nudSpecies.Size = new System.Drawing.Size(200, 15);
            nudSpecies.Minimum = -1;
            nudSpecies.Maximum = 1000;
            nudForm.ValueChanged += new EventHandler<ValueChangedEventArgs>(nudSpecies_ValueChanged);

            lblRange = new Label("lblRange");
            lblRange.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblRange.Text = "Shiny:";
            lblRange.AutoSize = true;
            lblRange.Location = new Point(10, 125);

            nudShinyChance = new NumericUpDown("nudShinyChance");
            nudShinyChance.Location = new Point(75, 125);
            nudShinyChance.Size = new System.Drawing.Size(200, 15);
            nudShinyChance.Maximum = Int32.MaxValue;
            nudShinyChance.Minimum = 0;

            chkSpawnsAtDawn = new CheckBox("chkSpawnsAtDawn");
            chkSpawnsAtDawn.Size = new Size(60, 17);
            chkSpawnsAtDawn.Location = new Point(75, 145);
            chkSpawnsAtDawn.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            chkSpawnsAtDawn.Text = "Dawn";

            chkSpawnsAtDay = new CheckBox("chkSpawnsAtDay");
            chkSpawnsAtDay.Size = new Size(60, 17);
            chkSpawnsAtDay.Location = new Point(175, 145);
            chkSpawnsAtDay.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            chkSpawnsAtDay.Text = "Day";

            chkSpawnsAtDusk = new CheckBox("chkSpawnsAtDusk");
            chkSpawnsAtDusk.Size = new Size(60, 17);
            chkSpawnsAtDusk.Location = new Point(75, 165);
            chkSpawnsAtDusk.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            chkSpawnsAtDusk.Text = "Dusk";

            chkSpawnsAtNight = new CheckBox("chkSpawnsAtNight");
            chkSpawnsAtNight.Size = new Size(60, 17);
            chkSpawnsAtNight.Location = new Point(175, 165);
            chkSpawnsAtNight.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            chkSpawnsAtNight.Text = "Night";

            lblBehaviour = new Label("lblBehaviour");
            lblBehaviour.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblBehaviour.Text = "Behaviour:";
            lblBehaviour.AutoSize = true;
            lblBehaviour.Location = new Point(10, lblRange.Y + 60);

            cmbBehaviour = new ComboBox("cmbBehaviour");
            cmbBehaviour.Location = new Point(75, lblBehaviour.Y);
            cmbBehaviour.Size = new System.Drawing.Size(200, 15);
            for (int i = 0; i < Enum.GetNames(typeof(Enums.NpcBehavior)).Length; i++)
            {
                cmbBehaviour.Items.Add(new ListBoxTextItem(Graphics.FontManager.LoadFont("tahoma", 10), Enum.GetName(typeof(Enums.NpcBehavior), i)));
            }

            lblRecruitRate = new Label("lblRecruitRate");
            lblRecruitRate.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblRecruitRate.Text = "Recruit Rate:";
            lblRecruitRate.AutoSize = true;
            lblRecruitRate.Location = new Point(10, lblBehaviour.Y + 20);

            nudRecruitRate = new NumericUpDown("nudRecruitRate");
            nudRecruitRate.Location = new Point(85, lblRecruitRate.Y);
            nudRecruitRate.Size = new System.Drawing.Size(190, 15);
            nudRecruitRate.Minimum = -1000;
            nudRecruitRate.Maximum = 1000;

            lblMove = new Label[4];
            lblMoveInfo = new Label[4];
            nudMove = new NumericUpDown[4];
            for (int i = 0; i < lblMove.Length; i++)
            {
                lblMove[i] = new Label("lblMove" + i);
                lblMoveInfo[i] = new Label("lblMoveInfo" + i);
                nudMove[i] = new NumericUpDown("nudMove" + i);
                nudMove[i].ValueChanged += new EventHandler<ValueChangedEventArgs>(winNPCPanel_ValueChanged);

                lblMove[i].Font = Graphics.FontManager.LoadFont("tahoma", 10);
                lblMove[i].Text = "Move " + (i + 1) + ":";
                lblMove[i].AutoSize = true;
                lblMove[i].Location = new Point(10, (lblRecruitRate.Y + 20) + (20 * i));

                nudMove[i].Location = new Point(75, (lblRecruitRate.Y + 20) + (20 * i));
                nudMove[i].Size = new Size(150, 15);
                nudMove[i].Minimum = -1;
                nudMove[i].Maximum = MaxInfo.MaxMoves;

                lblMoveInfo[i].Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
                lblMoveInfo[i].AutoSize = true;
                lblMoveInfo[i].Location = new Point(230, (lblRecruitRate.Y + 20) + (20 * i));
            }

            lblDropSelector = new Label("lblDropSelector");
            lblDropSelector.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblDropSelector.AutoSize = true;
            lblDropSelector.Text = "Drop #:";
            lblDropSelector.Location = new Point(10, lblMove[lblMove.Length - 1].Y + 20);

            nudDropSelector = new NumericUpDown("nudDropSelector");
            nudDropSelector.Location = new Point(75, lblDropSelector.Y);
            nudDropSelector.Size = new System.Drawing.Size(200, 15);
            nudDropSelector.Minimum = 1;
            nudDropSelector.Maximum = MaxInfo.MAX_NPC_DROPS;
            nudDropSelector.ValueChanged += new EventHandler<ValueChangedEventArgs>(nudDropSelector_ValueChanged);

            lblDropItemNum = new Label("lblDropItemNum");
            lblDropItemNum.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblDropItemNum.AutoSize = true;
            lblDropItemNum.Text = "Drop Item #:";
            lblDropItemNum.Location = new Point(10, lblDropSelector.Y + 20);

            nudDropItemNum = new NumericUpDown("nudDropItemNum");
            nudDropItemNum.Location = new Point(75, lblDropItemNum.Y);
            nudDropItemNum.Size = new System.Drawing.Size(200, 15);
            nudDropItemNum.Minimum = 0;
            nudDropItemNum.Maximum = MaxInfo.MaxItems;
            nudDropItemNum.ValueChanged += new EventHandler<ValueChangedEventArgs>(nudDropItemNum_ValueChanged);

            lblDropItemAmount = new Label("lblDropItemAmount");
            lblDropItemAmount.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblDropItemAmount.AutoSize = true;
            lblDropItemAmount.Text = "Drop Amount:";
            lblDropItemAmount.Location = new Point(10, lblDropItemNum.Y + 20);

            nudDropItemAmount = new NumericUpDown("nudDropItemAmount");
            nudDropItemAmount.Location = new Point(85, lblDropItemAmount.Y);
            nudDropItemAmount.Size = new System.Drawing.Size(190, 15);
            nudDropItemAmount.Minimum = 1;
            nudDropItemAmount.Maximum = Int32.MaxValue;
            nudDropItemAmount.ValueChanged += new EventHandler<ValueChangedEventArgs>(nudDropItemAmount_ValueChanged);

            lblDropItemChance = new Label("lblDropItemChance");
            lblDropItemChance.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblDropItemChance.AutoSize = true;
            lblDropItemChance.Text = "Drop Chance:";
            lblDropItemChance.Location = new Point(10, lblDropItemAmount.Y + 20);

            nudDropItemChance = new NumericUpDown("nudDropItemChance");
            nudDropItemChance.Location = new Point(85, lblDropItemChance.Y);
            nudDropItemChance.Size = new System.Drawing.Size(190, 15);
            nudDropItemChance.Minimum = 1;
            nudDropItemChance.Maximum = 100;
            nudDropItemChance.ValueChanged += new EventHandler<ValueChangedEventArgs>(nudDropItemChance_ValueChanged);

            lblDropItemTag = new Label("lblDropItemTag");
            lblDropItemTag.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            lblDropItemTag.AutoSize = true;
            lblDropItemTag.Text = "Item Tag:";
            lblDropItemTag.Location = new Point(10, lblDropItemChance.Y + 20);

            txtDropItemTag = new TextBox("nudDropItemTag");
            txtDropItemTag.Location = new Point(85, lblDropItemTag.Y);
            txtDropItemTag.Size = new System.Drawing.Size(190, 15);
            txtDropItemTag.Font = Logic.Graphics.FontManager.LoadFont("tahoma", 10);
            txtDropItemTag.TextChanged += new EventHandler(txtDropItemTag_TextChanged);



            #endregion

            btnEditorCancel = new Button("btnEditorCancel");
            btnEditorCancel.Location = new Point(20, lblDropItemTag.Y + 20);
            btnEditorCancel.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            btnEditorCancel.Size = new System.Drawing.Size(64, 16);
            btnEditorCancel.Visible = true;
            btnEditorCancel.Text = "Cancel";
            btnEditorCancel.Click += new EventHandler<MouseButtonEventArgs>(btnEditorCancel_Click);

            btnEditorOK = new Button("btnEditorOK");
            btnEditorOK.Location = new Point(120, lblDropItemTag.Y + 20);
            btnEditorOK.Font = Graphics.FontManager.LoadFont("tahoma", 10);
            btnEditorOK.Size = new System.Drawing.Size(64, 16);
            btnEditorOK.Visible = true;
            btnEditorOK.Text = "OK";
            btnEditorOK.Click += new EventHandler<MouseButtonEventArgs>(btnEditorOK_Click);

            #region NPC Selector

            pnlNPCList.AddWidget(lbxNPCList);
            pnlNPCList.AddWidget(btnBack);
            pnlNPCList.AddWidget(btnForward);
            pnlNPCList.AddWidget(btnEdit);
            pnlNPCList.AddWidget(btnCancel);

            #endregion

            #region NPC Editor

            pnlNPCEditor.AddWidget(lblName);
            pnlNPCEditor.AddWidget(txtName);
            pnlNPCEditor.AddWidget(lblAttackSay);
            pnlNPCEditor.AddWidget(txtAttackSay);
            pnlNPCEditor.AddWidget(lblAttackSay2);
            pnlNPCEditor.AddWidget(txtAttackSay2);
            pnlNPCEditor.AddWidget(lblAttackSay3);
            pnlNPCEditor.AddWidget(txtAttackSay3);
            pnlNPCEditor.AddWidget(lblForm);
            pnlNPCEditor.AddWidget(nudForm);
            pnlNPCEditor.AddWidget(lblSpecies);
            pnlNPCEditor.AddWidget(nudSpecies);
            pnlNPCEditor.AddWidget(lblRange);
            pnlNPCEditor.AddWidget(nudShinyChance);
            pnlNPCEditor.AddWidget(chkSpawnsAtDawn);
            pnlNPCEditor.AddWidget(chkSpawnsAtDay);
            pnlNPCEditor.AddWidget(chkSpawnsAtDusk);
            pnlNPCEditor.AddWidget(chkSpawnsAtNight);
            pnlNPCEditor.AddWidget(lblBehaviour);
            pnlNPCEditor.AddWidget(cmbBehaviour);
            pnlNPCEditor.AddWidget(lblRecruitRate);
            pnlNPCEditor.AddWidget(nudRecruitRate);

            for (int i = 0; i < lblMove.Length; i++)
            {
                pnlNPCEditor.AddWidget(lblMove[i]);
                pnlNPCEditor.AddWidget(nudMove[i]);
                pnlNPCEditor.AddWidget(lblMoveInfo[i]);
            }

            pnlNPCEditor.AddWidget(lblDropSelector);
            pnlNPCEditor.AddWidget(nudDropSelector);
            pnlNPCEditor.AddWidget(lblDropItemNum);
            pnlNPCEditor.AddWidget(nudDropItemNum);
            pnlNPCEditor.AddWidget(lblDropItemAmount);
            pnlNPCEditor.AddWidget(nudDropItemAmount);
            pnlNPCEditor.AddWidget(lblDropItemChance);
            pnlNPCEditor.AddWidget(nudDropItemChance);
            pnlNPCEditor.AddWidget(lblDropItemTag);
            pnlNPCEditor.AddWidget(txtDropItemTag);
            pnlNPCEditor.AddWidget(btnEditorCancel);
            pnlNPCEditor.AddWidget(btnEditorOK);

            #endregion

            this.AddWidget(pnlNPCList);
            this.AddWidget(pnlNPCEditor);

            RefreshNPCList();
            this.LoadComplete();
        }

        void txtDropItemTag_TextChanged(object sender, EventArgs e)
        {
            npc.Drops[nudDropSelector.Value - 1].Tag = txtDropItemTag.Text;
        }

        void nudDropItemChance_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            npc.Drops[nudDropSelector.Value - 1].Chance = e.NewValue;
        }

        void nudDropItemAmount_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            npc.Drops[nudDropSelector.Value - 1].ItemValue = e.NewValue;
        }

        void nudDropItemNum_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            npc.Drops[nudDropSelector.Value - 1].ItemNum = e.NewValue;
        }

        void nudDropSelector_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            nudDropItemNum.Value = npc.Drops[e.NewValue - 1].ItemNum;
            nudDropItemAmount.Value = npc.Drops[e.NewValue - 1].ItemValue;
            nudDropItemChance.Value = npc.Drops[e.NewValue - 1].Chance;
            txtDropItemTag.Text = npc.Drops[e.NewValue - 1].Tag;
        }

        void winNPCPanel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int index = Array.IndexOf(nudMove, sender);
            if (e.NewValue == 0)
            {
                lblMoveInfo[index].Text = "None";
            }
            else if (e.NewValue == -1)
            {
                lblMoveInfo[index].Text = "Auto";
            }
            else
            {
                lblMoveInfo[index].Text = Moves.MoveHelper.Moves[e.NewValue].Name;
            }
        }

        void nudForm_ValueChanged(object sender, ValueChangedEventArgs e)
        {
        }

        void nudSpecies_ValueChanged(object sender, ValueChangedEventArgs e)
        {
        }

        #endregion

        public void LoadNPC(string[] parse)
        {
            pnlNPCList.Visible = false;
            pnlNPCEditor.Visible = true;
            this.Size = pnlNPCEditor.Size;

            npc = new Logic.Editors.NPCs.EditableNPC();

            var n = 2;

            npc.Name = parse[n];
            npc.AttackSay = parse[n + 1];
            npc.AttackSay2 = parse[n + 2];
            npc.AttackSay3 = parse[n + 3];
            npc.Form = parse[n + 4].ToInt();
            npc.Species = parse[n + 5].ToInt();
            npc.ShinyChance = parse[n + 6].ToInt();
            npc.Behavior = (Enums.NpcBehavior)parse[n + 7].ToInt();
            npc.RecruitRate = parse[n + 8].ToInt();
            npc.AIScript = parse[n + 9];
            npc.SpawnsAtDawn = parse[n + 10].ToBool();
            npc.SpawnsAtDay = parse[n + 11].ToBool();
            npc.SpawnsAtDusk = parse[n + 12].ToBool();
            npc.SpawnsAtNight = parse[n + 13].ToBool();
            npc.SpawnDirection = (Enums.Direction)parse[n + 14].ToInt();
            npc.SpawnWeather = (Enums.Weather)parse[n + 15].ToInt();
            npc.Story = parse[n + 16].ToInt();
            npc.Shop = parse[n + 17].ToInt();
            npc.DeathStory = parse[n + 18].ToInt();

            n += 19;
            // Load npc moves
            for (int i = 0; i < npc.Moves.Length; i++)
            {
                npc.Moves[i] = parse[n].ToInt();

                n += 1;
            }
            // Load npc drops
            for (int i = 0; i < npc.Drops.Length; i++)
            {
                npc.Drops[i] = new Logic.Editors.NPCs.EditableNpcDrop();
                npc.Drops[i].ItemNum = parse[n].ToInt();
                npc.Drops[i].ItemValue = parse[n + 1].ToInt();
                npc.Drops[i].Chance = parse[n + 2].ToInt();
                npc.Drops[i].Tag = parse[n + 3];

                n += 4;
            }

            txtName.Text = npc.Name;
            txtAttackSay.Text = npc.AttackSay;
            txtAttackSay2.Text = npc.AttackSay2;
            txtAttackSay3.Text = npc.AttackSay3;
            nudForm.Value = npc.Form;
            nudSpecies.Value = npc.Species;
            nudShinyChance.Value = npc.ShinyChance;
            chkSpawnsAtDawn.Checked = npc.SpawnsAtDawn;
            chkSpawnsAtDay.Checked = npc.SpawnsAtDay;
            chkSpawnsAtDusk.Checked = npc.SpawnsAtDusk;
            chkSpawnsAtNight.Checked = npc.SpawnsAtNight;
            cmbBehaviour.SelectItem(npc.Behavior.ToString());

            nudRecruitRate.Value = npc.RecruitRate;
            for (int i = 0; i < npc.Moves.Length; i++)
            {
                nudMove[i].Value = npc.Moves[i];
            }
            nudDropSelector.Value = 1;
            nudDropItemNum.Value = npc.Drops[0].ItemNum;
            nudDropItemAmount.Value = npc.Drops[0].ItemValue;
            nudDropItemChance.Value = npc.Drops[0].Chance;
            txtDropItemTag.Text = npc.Drops[0].Tag;

            btnEdit.Text = "Edit";
        }

        void btnBack_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            if (currentTen > 0)
            {
                currentTen--;
            }
            RefreshNPCList();
        }

        void btnForward_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            if (currentTen < ((MaxInfo.MaxNpcs - 1) / 10))
            {
                currentTen++;
            }
            RefreshNPCList();
        }

        public void RefreshNPCList()
        {
            for (int i = 0; i < 10; i++)
            {
                if ((i + currentTen * 10) < MaxInfo.MaxNpcs)
                {
                    ((ListBoxTextItem)lbxNPCList.Items[i]).Text = ((i + 10 * currentTen) + 1 + ": " + Npc.NpcHelper.Npcs[(i + 10 * currentTen) + 1].Name);
                }
                else
                {
                    ((ListBoxTextItem)lbxNPCList.Items[i]).Text = "---";
                }
            }
        }

        void btnEdit_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            if (lbxNPCList.SelectedItems.Count == 1)
            {
                string[] index = ((ListBoxTextItem)lbxNPCList.SelectedItems[0]).Text.Split(':');
                if (index[0].IsNumeric())
                {
                    itemNum = index[0].ToInt();
                    Messenger.SendEditNpc(itemNum);
                }
            }
        }

        void btnCancel_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            this.Close();
            return;
        }

        void btnEditorCancel_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            itemNum = -1;
            pnlNPCEditor.Visible = false;
            pnlNPCList.Visible = true;
            this.Size = pnlNPCList.Size;
        }

        void hsbPic_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //pic.Image = Tools.CropImage(Logic.Graphics.GraphicsManager.Arrows, new Rectangle(0, hsbSpecies.Value * 32, 32, 32));
        }

        void btnEditorOK_Click(object sender, SdlDotNet.Widgets.MouseButtonEventArgs e)
        {
            npc.Name = txtName.Text;
            npc.AttackSay = txtAttackSay.Text;
            npc.AttackSay2 = txtAttackSay2.Text;
            npc.AttackSay3 = txtAttackSay3.Text;
            npc.Form = nudForm.Value;
            npc.Species = nudSpecies.Value;
            npc.ShinyChance = nudShinyChance.Value;
            npc.SpawnsAtDawn = chkSpawnsAtDawn.Checked;
            npc.SpawnsAtDay = chkSpawnsAtDay.Checked;
            npc.SpawnsAtDusk = chkSpawnsAtDusk.Checked;
            npc.SpawnsAtNight = chkSpawnsAtNight.Checked;
            npc.Behavior = (Enums.NpcBehavior)cmbBehaviour.SelectedIndex;
            // 10 = spawn time...
            npc.RecruitRate = nudRecruitRate.Value;

            // Save npc moves
            for (int i = 0; i < npc.Moves.Length; i++)
            {
                npc.Moves[i] = nudMove[i].Value;
            }
            Messenger.SendSaveNpc(itemNum, npc);
            pnlNPCEditor.Visible = false;
            pnlNPCList.Visible = true;
            this.Size = pnlNPCList.Size;
        }
    }
}
