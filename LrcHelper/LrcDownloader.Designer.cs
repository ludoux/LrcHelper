namespace LrcHelper
{
    partial class LrcDownloader
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LrcDownloader));
            this.IDtextBox = new System.Windows.Forms.TextBox();
            this.IDlabel = new System.Windows.Forms.Label();
            this.Functionlabel = new System.Windows.Forms.Label();
            this.MusicradioButton = new System.Windows.Forms.RadioButton();
            this.PlaylistradioButton = new System.Windows.Forms.RadioButton();
            this.GETbutton = new System.Windows.Forms.Button();
            this.Copyrightlabel = new System.Windows.Forms.Label();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.StatusgroupBox = new System.Windows.Forms.GroupBox();
            this.StatusPDTotalCountlabel = new System.Windows.Forms.Label();
            this.StatusPDFinishedCountlabel = new System.Windows.Forms.Label();
            this.StatusPDFinishedlabel = new System.Windows.Forms.Label();
            this.StatusPDTotallabel = new System.Windows.Forms.Label();
            this.StatusInfolabel = new System.Windows.Forms.Label();
            this.AutoSetcheckBox = new System.Windows.Forms.CheckBox();
            this.AlbumradioButton = new System.Windows.Forms.RadioButton();
            this.AdvancedSettingsgroupBox = new System.Windows.Forms.GroupBox();
            this.EncodingcomboBox = new System.Windows.Forms.ComboBox();
            this.ReviseRawcheckBox = new System.Windows.Forms.CheckBox();
            this.Savelabel = new System.Windows.Forms.Label();
            this.FilenamePatterncomboBox = new System.Windows.Forms.ComboBox();
            this.FilenamePatternLabel = new System.Windows.Forms.Label();
            this.DelayMsecnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.DelayMseclabel = new System.Windows.Forms.Label();
            this.LyricsStylenumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LyricsStylelabel = new System.Windows.Forms.Label();
            this.AdvancedSettingscheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.needhelplabel = new System.Windows.Forms.Label();
            this.CookieLabel = new System.Windows.Forms.Label();
            this.CookietextBox = new System.Windows.Forms.TextBox();
            this.StatusgroupBox.SuspendLayout();
            this.AdvancedSettingsgroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayMsecnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyricsStylenumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // IDtextBox
            // 
            this.IDtextBox.Location = new System.Drawing.Point(97, 15);
            this.IDtextBox.Margin = new System.Windows.Forms.Padding(7);
            this.IDtextBox.Name = "IDtextBox";
            this.IDtextBox.Size = new System.Drawing.Size(244, 42);
            this.IDtextBox.TabIndex = 0;
            // 
            // IDlabel
            // 
            this.IDlabel.AutoSize = true;
            this.IDlabel.Location = new System.Drawing.Point(30, 23);
            this.IDlabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.IDlabel.Name = "IDlabel";
            this.IDlabel.Size = new System.Drawing.Size(58, 30);
            this.IDlabel.TabIndex = 1;
            this.IDlabel.Text = "ID:";
            // 
            // Functionlabel
            // 
            this.Functionlabel.AutoSize = true;
            this.Functionlabel.Location = new System.Drawing.Point(25, 97);
            this.Functionlabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Functionlabel.Name = "Functionlabel";
            this.Functionlabel.Size = new System.Drawing.Size(118, 30);
            this.Functionlabel.TabIndex = 2;
            this.Functionlabel.Text = "这是...";
            // 
            // MusicradioButton
            // 
            this.MusicradioButton.AutoSize = true;
            this.MusicradioButton.Location = new System.Drawing.Point(207, 93);
            this.MusicradioButton.Margin = new System.Windows.Forms.Padding(7);
            this.MusicradioButton.Name = "MusicradioButton";
            this.MusicradioButton.Size = new System.Drawing.Size(94, 34);
            this.MusicradioButton.TabIndex = 3;
            this.MusicradioButton.TabStop = true;
            this.MusicradioButton.Text = "单曲";
            this.MusicradioButton.UseVisualStyleBackColor = true;
            // 
            // PlaylistradioButton
            // 
            this.PlaylistradioButton.AutoSize = true;
            this.PlaylistradioButton.Location = new System.Drawing.Point(335, 93);
            this.PlaylistradioButton.Margin = new System.Windows.Forms.Padding(7);
            this.PlaylistradioButton.Name = "PlaylistradioButton";
            this.PlaylistradioButton.Size = new System.Drawing.Size(94, 34);
            this.PlaylistradioButton.TabIndex = 4;
            this.PlaylistradioButton.TabStop = true;
            this.PlaylistradioButton.Text = "歌单";
            this.PlaylistradioButton.UseVisualStyleBackColor = true;
            // 
            // GETbutton
            // 
            this.GETbutton.Location = new System.Drawing.Point(363, 10);
            this.GETbutton.Margin = new System.Windows.Forms.Padding(7);
            this.GETbutton.Name = "GETbutton";
            this.GETbutton.Size = new System.Drawing.Size(187, 57);
            this.GETbutton.TabIndex = 5;
            this.GETbutton.Text = "下载";
            this.GETbutton.UseVisualStyleBackColor = true;
            this.GETbutton.Click += new System.EventHandler(this.GETbutton_Click);
            // 
            // Copyrightlabel
            // 
            this.Copyrightlabel.AutoSize = true;
            this.Copyrightlabel.Location = new System.Drawing.Point(562, 418);
            this.Copyrightlabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Copyrightlabel.Name = "Copyrightlabel";
            this.Copyrightlabel.Size = new System.Drawing.Size(418, 60);
            this.Copyrightlabel.TabIndex = 6;
            this.Copyrightlabel.Text = "Email:hi@luu.moe\r\ngithub.com/ludoux/lrchelper";
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Enabled = false;
            this.Cancelbutton.Location = new System.Drawing.Point(565, 10);
            this.Cancelbutton.Margin = new System.Windows.Forms.Padding(7);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(187, 57);
            this.Cancelbutton.TabIndex = 7;
            this.Cancelbutton.Text = "取消";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // StatusgroupBox
            // 
            this.StatusgroupBox.Controls.Add(this.StatusPDTotalCountlabel);
            this.StatusgroupBox.Controls.Add(this.StatusPDFinishedCountlabel);
            this.StatusgroupBox.Controls.Add(this.StatusPDFinishedlabel);
            this.StatusgroupBox.Controls.Add(this.StatusPDTotallabel);
            this.StatusgroupBox.Controls.Add(this.StatusInfolabel);
            this.StatusgroupBox.Location = new System.Drawing.Point(547, 147);
            this.StatusgroupBox.Margin = new System.Windows.Forms.Padding(7);
            this.StatusgroupBox.Name = "StatusgroupBox";
            this.StatusgroupBox.Padding = new System.Windows.Forms.Padding(7);
            this.StatusgroupBox.Size = new System.Drawing.Size(500, 250);
            this.StatusgroupBox.TabIndex = 8;
            this.StatusgroupBox.TabStop = false;
            this.StatusgroupBox.Text = "状态";
            // 
            // StatusPDTotalCountlabel
            // 
            this.StatusPDTotalCountlabel.AutoSize = true;
            this.StatusPDTotalCountlabel.Location = new System.Drawing.Point(220, 120);
            this.StatusPDTotalCountlabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StatusPDTotalCountlabel.Name = "StatusPDTotalCountlabel";
            this.StatusPDTotalCountlabel.Size = new System.Drawing.Size(28, 30);
            this.StatusPDTotalCountlabel.TabIndex = 4;
            this.StatusPDTotalCountlabel.Text = "0";
            // 
            // StatusPDFinishedCountlabel
            // 
            this.StatusPDFinishedCountlabel.AutoSize = true;
            this.StatusPDFinishedCountlabel.Location = new System.Drawing.Point(220, 150);
            this.StatusPDFinishedCountlabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StatusPDFinishedCountlabel.Name = "StatusPDFinishedCountlabel";
            this.StatusPDFinishedCountlabel.Size = new System.Drawing.Size(28, 30);
            this.StatusPDFinishedCountlabel.TabIndex = 3;
            this.StatusPDFinishedCountlabel.Text = "0";
            // 
            // StatusPDFinishedlabel
            // 
            this.StatusPDFinishedlabel.AutoSize = true;
            this.StatusPDFinishedlabel.Location = new System.Drawing.Point(103, 150);
            this.StatusPDFinishedlabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StatusPDFinishedlabel.Name = "StatusPDFinishedlabel";
            this.StatusPDFinishedlabel.Size = new System.Drawing.Size(58, 30);
            this.StatusPDFinishedlabel.TabIndex = 2;
            this.StatusPDFinishedlabel.Text = "已:";
            // 
            // StatusPDTotallabel
            // 
            this.StatusPDTotallabel.AutoSize = true;
            this.StatusPDTotallabel.Location = new System.Drawing.Point(103, 120);
            this.StatusPDTotallabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StatusPDTotallabel.Name = "StatusPDTotallabel";
            this.StatusPDTotallabel.Size = new System.Drawing.Size(58, 30);
            this.StatusPDTotallabel.TabIndex = 1;
            this.StatusPDTotallabel.Text = "总:";
            // 
            // StatusInfolabel
            // 
            this.StatusInfolabel.Location = new System.Drawing.Point(15, 43);
            this.StatusInfolabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StatusInfolabel.Name = "StatusInfolabel";
            this.StatusInfolabel.Size = new System.Drawing.Size(470, 77);
            this.StatusInfolabel.TabIndex = 0;
            this.StatusInfolabel.Text = "状态信息";
            this.StatusInfolabel.MouseHover += new System.EventHandler(this.StatusInfolabel_MouseHover);
            // 
            // AutoSetcheckBox
            // 
            this.AutoSetcheckBox.AutoSize = true;
            this.AutoSetcheckBox.Checked = true;
            this.AutoSetcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoSetcheckBox.Location = new System.Drawing.Point(852, 90);
            this.AutoSetcheckBox.Margin = new System.Windows.Forms.Padding(7);
            this.AutoSetcheckBox.Name = "AutoSetcheckBox";
            this.AutoSetcheckBox.Size = new System.Drawing.Size(155, 34);
            this.AutoSetcheckBox.TabIndex = 9;
            this.AutoSetcheckBox.Text = "Auto-Set";
            this.toolTip1.SetToolTip(this.AutoSetcheckBox, "从剪切板的链接中自动设置 ID");
            this.AutoSetcheckBox.UseVisualStyleBackColor = true;
            // 
            // AlbumradioButton
            // 
            this.AlbumradioButton.AutoSize = true;
            this.AlbumradioButton.Location = new System.Drawing.Point(463, 93);
            this.AlbumradioButton.Margin = new System.Windows.Forms.Padding(7);
            this.AlbumradioButton.Name = "AlbumradioButton";
            this.AlbumradioButton.Size = new System.Drawing.Size(94, 34);
            this.AlbumradioButton.TabIndex = 10;
            this.AlbumradioButton.TabStop = true;
            this.AlbumradioButton.Text = "专辑";
            this.AlbumradioButton.UseVisualStyleBackColor = true;
            // 
            // AdvancedSettingsgroupBox
            // 
            this.AdvancedSettingsgroupBox.Controls.Add(this.CookietextBox);
            this.AdvancedSettingsgroupBox.Controls.Add(this.CookieLabel);
            this.AdvancedSettingsgroupBox.Controls.Add(this.EncodingcomboBox);
            this.AdvancedSettingsgroupBox.Controls.Add(this.ReviseRawcheckBox);
            this.AdvancedSettingsgroupBox.Controls.Add(this.Savelabel);
            this.AdvancedSettingsgroupBox.Controls.Add(this.FilenamePatterncomboBox);
            this.AdvancedSettingsgroupBox.Controls.Add(this.FilenamePatternLabel);
            this.AdvancedSettingsgroupBox.Controls.Add(this.DelayMsecnumericUpDown);
            this.AdvancedSettingsgroupBox.Controls.Add(this.DelayMseclabel);
            this.AdvancedSettingsgroupBox.Controls.Add(this.LyricsStylenumericUpDown);
            this.AdvancedSettingsgroupBox.Controls.Add(this.LyricsStylelabel);
            this.AdvancedSettingsgroupBox.Location = new System.Drawing.Point(30, 155);
            this.AdvancedSettingsgroupBox.Margin = new System.Windows.Forms.Padding(7);
            this.AdvancedSettingsgroupBox.Name = "AdvancedSettingsgroupBox";
            this.AdvancedSettingsgroupBox.Padding = new System.Windows.Forms.Padding(7);
            this.AdvancedSettingsgroupBox.Size = new System.Drawing.Size(500, 382);
            this.AdvancedSettingsgroupBox.TabIndex = 11;
            this.AdvancedSettingsgroupBox.TabStop = false;
            this.AdvancedSettingsgroupBox.Visible = false;
            // 
            // EncodingcomboBox
            // 
            this.EncodingcomboBox.FormattingEnabled = true;
            this.EncodingcomboBox.Items.AddRange(new object[] {
            "UTF-8",
            "GB2312"});
            this.EncodingcomboBox.Location = new System.Drawing.Point(279, 330);
            this.EncodingcomboBox.Margin = new System.Windows.Forms.Padding(7);
            this.EncodingcomboBox.Name = "EncodingcomboBox";
            this.EncodingcomboBox.Size = new System.Drawing.Size(175, 38);
            this.EncodingcomboBox.TabIndex = 21;
            this.EncodingcomboBox.Text = "UTF-8";
            this.toolTip1.SetToolTip(this.EncodingcomboBox, "歌词文件的字符编码");
            // 
            // ReviseRawcheckBox
            // 
            this.ReviseRawcheckBox.AutoSize = true;
            this.ReviseRawcheckBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.ReviseRawcheckBox.Location = new System.Drawing.Point(20, 332);
            this.ReviseRawcheckBox.Margin = new System.Windows.Forms.Padding(7);
            this.ReviseRawcheckBox.Name = "ReviseRawcheckBox";
            this.ReviseRawcheckBox.Size = new System.Drawing.Size(170, 34);
            this.ReviseRawcheckBox.TabIndex = 20;
            this.ReviseRawcheckBox.Text = "ReviseRaw";
            this.ReviseRawcheckBox.UseVisualStyleBackColor = true;
            // 
            // Savelabel
            // 
            this.Savelabel.AutoSize = true;
            this.Savelabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Savelabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Savelabel.ForeColor = System.Drawing.Color.Blue;
            this.Savelabel.Location = new System.Drawing.Point(387, 35);
            this.Savelabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Savelabel.Name = "Savelabel";
            this.Savelabel.Size = new System.Drawing.Size(37, 15);
            this.Savelabel.TabIndex = 19;
            this.Savelabel.Text = "保存";
            this.toolTip1.SetToolTip(this.Savelabel, "软件将在下次启动时自动加载已保存的高级设置");
            this.Savelabel.Click += new System.EventHandler(this.Savelabel_Click);
            // 
            // FilenamePatterncomboBox
            // 
            this.FilenamePatterncomboBox.FormattingEnabled = true;
            this.FilenamePatterncomboBox.Items.AddRange(new object[] {
            "[title].lrc",
            "[track number]. [title].lrc",
            "[artist] - [title].lrc",
            "[title] - [artist].lrc"});
            this.FilenamePatterncomboBox.Location = new System.Drawing.Point(20, 207);
            this.FilenamePatterncomboBox.Margin = new System.Windows.Forms.Padding(7);
            this.FilenamePatterncomboBox.Name = "FilenamePatterncomboBox";
            this.FilenamePatterncomboBox.Size = new System.Drawing.Size(434, 38);
            this.FilenamePatterncomboBox.TabIndex = 18;
            this.FilenamePatterncomboBox.Text = "[title].lrc";
            this.toolTip1.SetToolTip(this.FilenamePatterncomboBox, "支持：[title][track number][album][artist]");
            // 
            // FilenamePatternLabel
            // 
            this.FilenamePatternLabel.AutoSize = true;
            this.FilenamePatternLabel.Location = new System.Drawing.Point(15, 170);
            this.FilenamePatternLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.FilenamePatternLabel.Name = "FilenamePatternLabel";
            this.FilenamePatternLabel.Size = new System.Drawing.Size(178, 30);
            this.FilenamePatternLabel.TabIndex = 17;
            this.FilenamePatternLabel.Text = "文件名样式:";
            // 
            // DelayMsecnumericUpDown
            // 
            this.DelayMsecnumericUpDown.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.DelayMsecnumericUpDown.Location = new System.Drawing.Point(223, 97);
            this.DelayMsecnumericUpDown.Margin = new System.Windows.Forms.Padding(7);
            this.DelayMsecnumericUpDown.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.DelayMsecnumericUpDown.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.DelayMsecnumericUpDown.Name = "DelayMsecnumericUpDown";
            this.DelayMsecnumericUpDown.Size = new System.Drawing.Size(95, 42);
            this.DelayMsecnumericUpDown.TabIndex = 16;
            this.toolTip1.SetToolTip(this.DelayMsecnumericUpDown, "100 意味着 1sec");
            this.DelayMsecnumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // DelayMseclabel
            // 
            this.DelayMseclabel.AutoSize = true;
            this.DelayMseclabel.Location = new System.Drawing.Point(13, 100);
            this.DelayMseclabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.DelayMseclabel.Name = "DelayMseclabel";
            this.DelayMseclabel.Size = new System.Drawing.Size(148, 30);
            this.DelayMseclabel.TabIndex = 15;
            this.DelayMseclabel.Text = "延迟Msec:";
            this.toolTip1.SetToolTip(this.DelayMseclabel, "How long should the translyrics be delayed?");
            // 
            // LyricsStylenumericUpDown
            // 
            this.LyricsStylenumericUpDown.Location = new System.Drawing.Point(223, 40);
            this.LyricsStylenumericUpDown.Margin = new System.Windows.Forms.Padding(7);
            this.LyricsStylenumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LyricsStylenumericUpDown.Name = "LyricsStylenumericUpDown";
            this.LyricsStylenumericUpDown.Size = new System.Drawing.Size(75, 42);
            this.LyricsStylenumericUpDown.TabIndex = 14;
            this.toolTip1.SetToolTip(this.LyricsStylenumericUpDown, "0:歌词原文和翻译将分行\r\n1:歌词原文和翻译将尽可能同行");
            // 
            // LyricsStylelabel
            // 
            this.LyricsStylelabel.AutoSize = true;
            this.LyricsStylelabel.Location = new System.Drawing.Point(15, 45);
            this.LyricsStylelabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LyricsStylelabel.Name = "LyricsStylelabel";
            this.LyricsStylelabel.Size = new System.Drawing.Size(148, 30);
            this.LyricsStylelabel.TabIndex = 13;
            this.LyricsStylelabel.Text = "歌词样式:";
            this.toolTip1.SetToolTip(this.LyricsStylelabel, "Different styles for different environment.");
            // 
            // AdvancedSettingscheckBox
            // 
            this.AdvancedSettingscheckBox.AutoSize = true;
            this.AdvancedSettingscheckBox.Location = new System.Drawing.Point(35, 150);
            this.AdvancedSettingscheckBox.Margin = new System.Windows.Forms.Padding(7);
            this.AdvancedSettingscheckBox.Name = "AdvancedSettingscheckBox";
            this.AdvancedSettingscheckBox.Size = new System.Drawing.Size(155, 34);
            this.AdvancedSettingscheckBox.TabIndex = 12;
            this.AdvancedSettingscheckBox.Text = "高级设置";
            this.toolTip1.SetToolTip(this.AdvancedSettingscheckBox, "More features but less stable.");
            this.AdvancedSettingscheckBox.UseVisualStyleBackColor = true;
            this.AdvancedSettingscheckBox.CheckedChanged += new System.EventHandler(this.AdvancedSettingscheckBox_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 0;
            this.toolTip1.ShowAlways = true;
            // 
            // needhelplabel
            // 
            this.needhelplabel.AutoSize = true;
            this.needhelplabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.needhelplabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.needhelplabel.ForeColor = System.Drawing.Color.Blue;
            this.needhelplabel.Location = new System.Drawing.Point(847, 32);
            this.needhelplabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.needhelplabel.Name = "needhelplabel";
            this.needhelplabel.Size = new System.Drawing.Size(87, 15);
            this.needhelplabel.TabIndex = 19;
            this.needhelplabel.Text = "Need Help?";
            this.needhelplabel.Click += new System.EventHandler(this.needhelplabel_Click);
            // 
            // CookieLabel
            // 
            this.CookieLabel.AutoSize = true;
            this.CookieLabel.Location = new System.Drawing.Point(15, 272);
            this.CookieLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.CookieLabel.Name = "CookieLabel";
            this.CookieLabel.Size = new System.Drawing.Size(178, 30);
            this.CookieLabel.TabIndex = 22;
            this.CookieLabel.Text = "登录Cookie:";
            // 
            // CookietextBox
            // 
            this.CookietextBox.Location = new System.Drawing.Point(198, 269);
            this.CookietextBox.Name = "CookietextBox";
            this.CookietextBox.Size = new System.Drawing.Size(256, 42);
            this.CookietextBox.TabIndex = 23;
            // 
            // LrcDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 553);
            this.Controls.Add(this.needhelplabel);
            this.Controls.Add(this.AdvancedSettingscheckBox);
            this.Controls.Add(this.AdvancedSettingsgroupBox);
            this.Controls.Add(this.AlbumradioButton);
            this.Controls.Add(this.AutoSetcheckBox);
            this.Controls.Add(this.StatusgroupBox);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.Copyrightlabel);
            this.Controls.Add(this.GETbutton);
            this.Controls.Add(this.PlaylistradioButton);
            this.Controls.Add(this.MusicradioButton);
            this.Controls.Add(this.Functionlabel);
            this.Controls.Add(this.IDlabel);
            this.Controls.Add(this.IDtextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.MaximizeBox = false;
            this.Name = "LrcDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "163lrc";
            this.Activated += new System.EventHandler(this.LrcDownloader_Activated);
            this.Load += new System.EventHandler(this.LrcDownloader_Load);
            this.StatusgroupBox.ResumeLayout(false);
            this.StatusgroupBox.PerformLayout();
            this.AdvancedSettingsgroupBox.ResumeLayout(false);
            this.AdvancedSettingsgroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayMsecnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyricsStylenumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IDtextBox;
        private System.Windows.Forms.Label IDlabel;
        private System.Windows.Forms.Label Functionlabel;
        private System.Windows.Forms.RadioButton MusicradioButton;
        private System.Windows.Forms.RadioButton PlaylistradioButton;
        private System.Windows.Forms.Button GETbutton;
        private System.Windows.Forms.Label Copyrightlabel;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.GroupBox StatusgroupBox;
        private System.Windows.Forms.Label StatusPDTotalCountlabel;
        private System.Windows.Forms.Label StatusPDFinishedCountlabel;
        private System.Windows.Forms.Label StatusPDFinishedlabel;
        private System.Windows.Forms.Label StatusPDTotallabel;
        private System.Windows.Forms.Label StatusInfolabel;
        private System.Windows.Forms.CheckBox AutoSetcheckBox;
        private System.Windows.Forms.RadioButton AlbumradioButton;
        private System.Windows.Forms.GroupBox AdvancedSettingsgroupBox;
        private System.Windows.Forms.CheckBox AdvancedSettingscheckBox;
        private System.Windows.Forms.NumericUpDown LyricsStylenumericUpDown;
        private System.Windows.Forms.Label LyricsStylelabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label DelayMseclabel;
        private System.Windows.Forms.NumericUpDown DelayMsecnumericUpDown;
        private System.Windows.Forms.ComboBox FilenamePatterncomboBox;
        private System.Windows.Forms.Label FilenamePatternLabel;
        private System.Windows.Forms.Label needhelplabel;
        private System.Windows.Forms.Label Savelabel;
        private System.Windows.Forms.CheckBox ReviseRawcheckBox;
        private System.Windows.Forms.ComboBox EncodingcomboBox;
        private System.Windows.Forms.TextBox CookietextBox;
        private System.Windows.Forms.Label CookieLabel;
    }
}