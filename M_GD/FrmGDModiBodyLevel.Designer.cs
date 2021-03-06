namespace M_GD
{
    partial class FrmGDModiBodyLevel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GrpSearch = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtNick = new System.Windows.Forms.TextBox();
            this.lblNick = new System.Windows.Forms.Label();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.lblAccount = new System.Windows.Forms.Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.tbcResult = new System.Windows.Forms.TabControl();
            this.tpgCharacter = new System.Windows.Forms.TabPage();
            this.GrdCharacter = new System.Windows.Forms.DataGridView();
            this.tpgUserUnits = new System.Windows.Forms.TabPage();
            this.GrdUnits = new System.Windows.Forms.DataGridView();
            this.pnlUnits = new System.Windows.Forms.Panel();
            this.cmbUnits = new System.Windows.Forms.ComboBox();
            this.lblUnits = new System.Windows.Forms.Label();
            this.tpgModiBodyLevel = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NudNewLvl = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStrongLevel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSkill = new System.Windows.Forms.TextBox();
            this.lblSkill = new System.Windows.Forms.Label();
            this.btnResetLvl = new System.Windows.Forms.Button();
            this.btnModiLvl = new System.Windows.Forms.Button();
            this.lblNewLvl = new System.Windows.Forms.Label();
            this.txtCurLvl = new System.Windows.Forms.TextBox();
            this.lblCurLvl = new System.Windows.Forms.Label();
            this.backgroundWorkerFormLoad = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerSearch = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerModiBodyLevel = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerModiStrongLevel = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerUnits = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerReUnits = new System.ComponentModel.BackgroundWorker();
            this.GrpSearch.SuspendLayout();
            this.tbcResult.SuspendLayout();
            this.tpgCharacter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdCharacter)).BeginInit();
            this.tpgUserUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdUnits)).BeginInit();
            this.pnlUnits.SuspendLayout();
            this.tpgModiBodyLevel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GrpSearch
            // 
            this.GrpSearch.Controls.Add(this.label3);
            this.GrpSearch.Controls.Add(this.btnClose);
            this.GrpSearch.Controls.Add(this.btnSearch);
            this.GrpSearch.Controls.Add(this.txtNick);
            this.GrpSearch.Controls.Add(this.lblNick);
            this.GrpSearch.Controls.Add(this.txtAccount);
            this.GrpSearch.Controls.Add(this.lblAccount);
            this.GrpSearch.Controls.Add(this.cmbServer);
            this.GrpSearch.Controls.Add(this.lblServer);
            this.GrpSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.GrpSearch.Location = new System.Drawing.Point(0, 0);
            this.GrpSearch.Name = "GrpSearch";
            this.GrpSearch.Size = new System.Drawing.Size(814, 133);
            this.GrpSearch.TabIndex = 4;
            this.GrpSearch.TabStop = false;
            this.GrpSearch.Text = "搜索條件";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(418, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 12);
            this.label3.TabIndex = 32;
            this.label3.Text = "提示：查詢機體資訊後才能修改等級";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(300, 59);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 31;
            this.btnClose.Text = "關閉";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(300, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "查詢";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtNick
            // 
            this.txtNick.Location = new System.Drawing.Point(107, 92);
            this.txtNick.Name = "txtNick";
            this.txtNick.Size = new System.Drawing.Size(187, 21);
            this.txtNick.TabIndex = 29;
            // 
            // lblNick
            // 
            this.lblNick.AutoSize = true;
            this.lblNick.Location = new System.Drawing.Point(36, 95);
            this.lblNick.Name = "lblNick";
            this.lblNick.Size = new System.Drawing.Size(65, 12);
            this.lblNick.TabIndex = 28;
            this.lblNick.Text = "玩家昵稱：";
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(107, 58);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(187, 21);
            this.txtAccount.TabIndex = 27;
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(36, 61);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(65, 12);
            this.lblAccount.TabIndex = 26;
            this.lblAccount.Text = "玩家帳號：";
            // 
            // cmbServer
            // 
            this.cmbServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(107, 25);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(187, 20);
            this.cmbServer.TabIndex = 25;
            this.cmbServer.SelectedIndexChanged += new System.EventHandler(this.cmbServer_SelectedIndexChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(24, 28);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(77, 12);
            this.lblServer.TabIndex = 24;
            this.lblServer.Text = "遊戲伺服器：";
            // 
            // tbcResult
            // 
            this.tbcResult.Controls.Add(this.tpgCharacter);
            this.tbcResult.Controls.Add(this.tpgUserUnits);
            this.tbcResult.Controls.Add(this.tpgModiBodyLevel);
            this.tbcResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcResult.Location = new System.Drawing.Point(0, 133);
            this.tbcResult.Name = "tbcResult";
            this.tbcResult.SelectedIndex = 0;
            this.tbcResult.Size = new System.Drawing.Size(814, 412);
            this.tbcResult.TabIndex = 26;
            this.tbcResult.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbcResult_Selected);
            // 
            // tpgCharacter
            // 
            this.tpgCharacter.Controls.Add(this.GrdCharacter);
            this.tpgCharacter.Location = new System.Drawing.Point(4, 21);
            this.tpgCharacter.Name = "tpgCharacter";
            this.tpgCharacter.Size = new System.Drawing.Size(806, 387);
            this.tpgCharacter.TabIndex = 7;
            this.tpgCharacter.Text = "玩家角色信息";
            this.tpgCharacter.UseVisualStyleBackColor = true;
            // 
            // GrdCharacter
            // 
            this.GrdCharacter.AllowUserToAddRows = false;
            this.GrdCharacter.AllowUserToDeleteRows = false;
            this.GrdCharacter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GrdCharacter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrdCharacter.Location = new System.Drawing.Point(0, 0);
            this.GrdCharacter.Name = "GrdCharacter";
            this.GrdCharacter.ReadOnly = true;
            this.GrdCharacter.RowTemplate.Height = 23;
            this.GrdCharacter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GrdCharacter.Size = new System.Drawing.Size(806, 387);
            this.GrdCharacter.TabIndex = 11;
            this.GrdCharacter.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdCharacter_CellDoubleClick);
            // 
            // tpgUserUnits
            // 
            this.tpgUserUnits.Controls.Add(this.GrdUnits);
            this.tpgUserUnits.Controls.Add(this.pnlUnits);
            this.tpgUserUnits.Location = new System.Drawing.Point(4, 21);
            this.tpgUserUnits.Name = "tpgUserUnits";
            this.tpgUserUnits.Size = new System.Drawing.Size(806, 387);
            this.tpgUserUnits.TabIndex = 4;
            this.tpgUserUnits.Text = "玩家機體信息";
            this.tpgUserUnits.UseVisualStyleBackColor = true;
            // 
            // GrdUnits
            // 
            this.GrdUnits.AllowUserToAddRows = false;
            this.GrdUnits.AllowUserToDeleteRows = false;
            this.GrdUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GrdUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrdUnits.Location = new System.Drawing.Point(0, 37);
            this.GrdUnits.Name = "GrdUnits";
            this.GrdUnits.ReadOnly = true;
            this.GrdUnits.RowTemplate.Height = 23;
            this.GrdUnits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GrdUnits.Size = new System.Drawing.Size(806, 350);
            this.GrdUnits.TabIndex = 14;
            this.GrdUnits.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdUnits_CellDoubleClick);
            // 
            // pnlUnits
            // 
            this.pnlUnits.Controls.Add(this.cmbUnits);
            this.pnlUnits.Controls.Add(this.lblUnits);
            this.pnlUnits.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUnits.Location = new System.Drawing.Point(0, 0);
            this.pnlUnits.Name = "pnlUnits";
            this.pnlUnits.Size = new System.Drawing.Size(806, 37);
            this.pnlUnits.TabIndex = 13;
            // 
            // cmbUnits
            // 
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            this.cmbUnits.Location = new System.Drawing.Point(124, 8);
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.Size = new System.Drawing.Size(100, 20);
            this.cmbUnits.TabIndex = 1;
            this.cmbUnits.SelectedIndexChanged += new System.EventHandler(this.cmbUnits_SelectedIndexChanged);
            // 
            // lblUnits
            // 
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new System.Drawing.Point(20, 13);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(101, 12);
            this.lblUnits.TabIndex = 0;
            this.lblUnits.Text = "请选择查看页数：";
            // 
            // tpgModiBodyLevel
            // 
            this.tpgModiBodyLevel.Controls.Add(this.label10);
            this.tpgModiBodyLevel.Controls.Add(this.label9);
            this.tpgModiBodyLevel.Controls.Add(this.label8);
            this.tpgModiBodyLevel.Controls.Add(this.label7);
            this.tpgModiBodyLevel.Controls.Add(this.label6);
            this.tpgModiBodyLevel.Controls.Add(this.label5);
            this.tpgModiBodyLevel.Controls.Add(this.label4);
            this.tpgModiBodyLevel.Controls.Add(this.NudNewLvl);
            this.tpgModiBodyLevel.Controls.Add(this.numericUpDown1);
            this.tpgModiBodyLevel.Controls.Add(this.label2);
            this.tpgModiBodyLevel.Controls.Add(this.txtStrongLevel);
            this.tpgModiBodyLevel.Controls.Add(this.label1);
            this.tpgModiBodyLevel.Controls.Add(this.txtSkill);
            this.tpgModiBodyLevel.Controls.Add(this.lblSkill);
            this.tpgModiBodyLevel.Controls.Add(this.btnResetLvl);
            this.tpgModiBodyLevel.Controls.Add(this.btnModiLvl);
            this.tpgModiBodyLevel.Controls.Add(this.lblNewLvl);
            this.tpgModiBodyLevel.Controls.Add(this.txtCurLvl);
            this.tpgModiBodyLevel.Controls.Add(this.lblCurLvl);
            this.tpgModiBodyLevel.Location = new System.Drawing.Point(4, 21);
            this.tpgModiBodyLevel.Name = "tpgModiBodyLevel";
            this.tpgModiBodyLevel.Size = new System.Drawing.Size(806, 387);
            this.tpgModiBodyLevel.TabIndex = 8;
            this.tpgModiBodyLevel.Text = "修改等級";
            this.tpgModiBodyLevel.UseVisualStyleBackColor = true;
            this.tpgModiBodyLevel.Click += new System.EventHandler(this.tpgModiBodyLevel_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(476, 167);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(251, 12);
            this.label10.TabIndex = 40;
            this.label10.Text = "C級機體等級不能超過8級，強化等級不超過4級";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(476, 146);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(251, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "B級機體等級不能超過7級，強化等級不超過3級";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(475, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(275, 12);
            this.label8.TabIndex = 38;
            this.label8.Text = "A級/S級機體等級不能超過6級,強化等級不超過2級!";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(475, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(245, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "強化等級為4級的機體,機體等級不能超過8級!";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(475, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 12);
            this.label6.TabIndex = 36;
            this.label6.Text = "強化等級為3級的機體，機體等級不能超過7級!";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(475, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(257, 12);
            this.label5.TabIndex = 35;
            this.label5.Text = "強化等級為2級的機體，機體等級不能超過6級！";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(473, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "溫馨提示：";
            // 
            // NudNewLvl
            // 
            this.NudNewLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NudNewLvl.FormattingEnabled = true;
            this.NudNewLvl.Location = new System.Drawing.Point(248, 145);
            this.NudNewLvl.Name = "NudNewLvl";
            this.NudNewLvl.Size = new System.Drawing.Size(169, 20);
            this.NudNewLvl.TabIndex = 33;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.numericUpDown1.FormattingEnabled = true;
            this.numericUpDown1.Location = new System.Drawing.Point(248, 94);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(169, 20);
            this.numericUpDown1.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "新強化等級：";
            // 
            // txtStrongLevel
            // 
            this.txtStrongLevel.Location = new System.Drawing.Point(248, 67);
            this.txtStrongLevel.Name = "txtStrongLevel";
            this.txtStrongLevel.ReadOnly = true;
            this.txtStrongLevel.Size = new System.Drawing.Size(169, 21);
            this.txtStrongLevel.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "當前強化等級：";
            // 
            // txtSkill
            // 
            this.txtSkill.Location = new System.Drawing.Point(248, 41);
            this.txtSkill.Name = "txtSkill";
            this.txtSkill.ReadOnly = true;
            this.txtSkill.Size = new System.Drawing.Size(169, 21);
            this.txtSkill.TabIndex = 25;
            // 
            // lblSkill
            // 
            this.lblSkill.AutoSize = true;
            this.lblSkill.Location = new System.Drawing.Point(136, 45);
            this.lblSkill.Name = "lblSkill";
            this.lblSkill.Size = new System.Drawing.Size(65, 12);
            this.lblSkill.TabIndex = 24;
            this.lblSkill.Text = "機體名稱：";
            // 
            // btnResetLvl
            // 
            this.btnResetLvl.Location = new System.Drawing.Point(342, 195);
            this.btnResetLvl.Name = "btnResetLvl";
            this.btnResetLvl.Size = new System.Drawing.Size(75, 23);
            this.btnResetLvl.TabIndex = 23;
            this.btnResetLvl.Text = "重置";
            this.btnResetLvl.UseVisualStyleBackColor = true;
            this.btnResetLvl.Click += new System.EventHandler(this.btnResetLvl_Click);
            // 
            // btnModiLvl
            // 
            this.btnModiLvl.Location = new System.Drawing.Point(138, 195);
            this.btnModiLvl.Name = "btnModiLvl";
            this.btnModiLvl.Size = new System.Drawing.Size(104, 23);
            this.btnModiLvl.TabIndex = 22;
            this.btnModiLvl.Text = "更改機體等級";
            this.btnModiLvl.UseVisualStyleBackColor = true;
            this.btnModiLvl.Click += new System.EventHandler(this.btnModiLvl_Click);
            // 
            // lblNewLvl
            // 
            this.lblNewLvl.AutoSize = true;
            this.lblNewLvl.Location = new System.Drawing.Point(136, 149);
            this.lblNewLvl.Name = "lblNewLvl";
            this.lblNewLvl.Size = new System.Drawing.Size(77, 12);
            this.lblNewLvl.TabIndex = 20;
            this.lblNewLvl.Text = "新機體等級：";
            // 
            // txtCurLvl
            // 
            this.txtCurLvl.Location = new System.Drawing.Point(248, 119);
            this.txtCurLvl.Name = "txtCurLvl";
            this.txtCurLvl.ReadOnly = true;
            this.txtCurLvl.Size = new System.Drawing.Size(169, 21);
            this.txtCurLvl.TabIndex = 19;
            // 
            // lblCurLvl
            // 
            this.lblCurLvl.AutoSize = true;
            this.lblCurLvl.Location = new System.Drawing.Point(136, 123);
            this.lblCurLvl.Name = "lblCurLvl";
            this.lblCurLvl.Size = new System.Drawing.Size(89, 12);
            this.lblCurLvl.TabIndex = 18;
            this.lblCurLvl.Text = "當前機體等級：";
            // 
            // backgroundWorkerFormLoad
            // 
            this.backgroundWorkerFormLoad.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerFormLoad_DoWork);
            this.backgroundWorkerFormLoad.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerFormLoad_RunWorkerCompleted);
            // 
            // backgroundWorkerSearch
            // 
            this.backgroundWorkerSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSearch_DoWork);
            this.backgroundWorkerSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSearch_RunWorkerCompleted);
            // 
            // backgroundWorkerModiBodyLevel
            // 
            this.backgroundWorkerModiBodyLevel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerModiBodyLevel_DoWork);
            this.backgroundWorkerModiBodyLevel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerModiBodyLevel_RunWorkerCompleted);
            // 
            // backgroundWorkerModiStrongLevel
            // 
            this.backgroundWorkerModiStrongLevel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerModiStrongLevel_DoWork);
            this.backgroundWorkerModiStrongLevel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerModiStrongLevel_RunWorkerCompleted);
            // 
            // backgroundWorkerUnits
            // 
            this.backgroundWorkerUnits.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUnits_DoWork);
            this.backgroundWorkerUnits.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerUnits_RunWorkerCompleted);
            // 
            // backgroundWorkerReUnits
            // 
            this.backgroundWorkerReUnits.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerReUnits_DoWork);
            this.backgroundWorkerReUnits.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerReUnits_RunWorkerCompleted);
            // 
            // FrmGDModiBodyLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 545);
            this.Controls.Add(this.tbcResult);
            this.Controls.Add(this.GrpSearch);
            this.Name = "FrmGDModiBodyLevel";
            this.Text = "玩家機體等級修改";
            this.Load += new System.EventHandler(this.FrmGDModiBodyLevel_Load);
            this.GrpSearch.ResumeLayout(false);
            this.GrpSearch.PerformLayout();
            this.tbcResult.ResumeLayout(false);
            this.tpgCharacter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdCharacter)).EndInit();
            this.tpgUserUnits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdUnits)).EndInit();
            this.pnlUnits.ResumeLayout(false);
            this.pnlUnits.PerformLayout();
            this.tpgModiBodyLevel.ResumeLayout(false);
            this.tpgModiBodyLevel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GrpSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtNick;
        private System.Windows.Forms.Label lblNick;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TabControl tbcResult;
        private System.Windows.Forms.TabPage tpgCharacter;
        private System.Windows.Forms.TabPage tpgUserUnits;
        private System.Windows.Forms.DataGridView GrdCharacter;
        private System.Windows.Forms.Panel pnlUnits;
        private System.Windows.Forms.ComboBox cmbUnits;
        private System.Windows.Forms.Label lblUnits;
        private System.Windows.Forms.TabPage tpgModiBodyLevel;
        private System.ComponentModel.BackgroundWorker backgroundWorkerFormLoad;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSearch;
        private System.ComponentModel.BackgroundWorker backgroundWorkerModiBodyLevel;
        private System.Windows.Forms.TextBox txtSkill;
        private System.Windows.Forms.Label lblSkill;
        private System.Windows.Forms.Button btnResetLvl;
        private System.Windows.Forms.Button btnModiLvl;
        private System.Windows.Forms.Label lblNewLvl;
        private System.Windows.Forms.TextBox txtCurLvl;
        private System.Windows.Forms.Label lblCurLvl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStrongLevel;
        private System.ComponentModel.BackgroundWorker backgroundWorkerModiStrongLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox numericUpDown1;
        private System.Windows.Forms.ComboBox NudNewLvl;
        private System.Windows.Forms.DataGridView GrdUnits;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUnits;
        private System.ComponentModel.BackgroundWorker backgroundWorkerReUnits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}