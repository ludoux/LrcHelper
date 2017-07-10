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
            this.DelayMsecnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.DelayMseclabel = new System.Windows.Forms.Label();
            this.LyricsStylenumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LyricsStylelabel = new System.Windows.Forms.Label();
            this.AdvancedSettingscheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.StatusgroupBox.SuspendLayout();
            this.AdvancedSettingsgroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayMsecnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyricsStylenumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // IDtextBox
            // 
            this.IDtextBox.Location = new System.Drawing.Point(39, 6);
            this.IDtextBox.Name = "IDtextBox";
            this.IDtextBox.Size = new System.Drawing.Size(100, 21);
            this.IDtextBox.TabIndex = 0;
            // 
            // IDlabel
            // 
            this.IDlabel.AutoSize = true;
            this.IDlabel.Location = new System.Drawing.Point(12, 9);
            this.IDlabel.Name = "IDlabel";
            this.IDlabel.Size = new System.Drawing.Size(23, 12);
            this.IDlabel.TabIndex = 1;
            this.IDlabel.Text = "ID:";
            // 
            // Functionlabel
            // 
            this.Functionlabel.AutoSize = true;
            this.Functionlabel.Location = new System.Drawing.Point(12, 39);
            this.Functionlabel.Name = "Functionlabel";
            this.Functionlabel.Size = new System.Drawing.Size(65, 12);
            this.Functionlabel.TabIndex = 2;
            this.Functionlabel.Text = "This is...";
            // 
            // MusicradioButton
            // 
            this.MusicradioButton.AutoSize = true;
            this.MusicradioButton.Location = new System.Drawing.Point(83, 37);
            this.MusicradioButton.Name = "MusicradioButton";
            this.MusicradioButton.Size = new System.Drawing.Size(53, 16);
            this.MusicradioButton.TabIndex = 3;
            this.MusicradioButton.TabStop = true;
            this.MusicradioButton.Text = "Music";
            this.MusicradioButton.UseVisualStyleBackColor = true;
            // 
            // PlaylistradioButton
            // 
            this.PlaylistradioButton.AutoSize = true;
            this.PlaylistradioButton.Location = new System.Drawing.Point(142, 37);
            this.PlaylistradioButton.Name = "PlaylistradioButton";
            this.PlaylistradioButton.Size = new System.Drawing.Size(71, 16);
            this.PlaylistradioButton.TabIndex = 4;
            this.PlaylistradioButton.TabStop = true;
            this.PlaylistradioButton.Text = "Playlist";
            this.PlaylistradioButton.UseVisualStyleBackColor = true;
            // 
            // GETbutton
            // 
            this.GETbutton.Location = new System.Drawing.Point(145, 4);
            this.GETbutton.Name = "GETbutton";
            this.GETbutton.Size = new System.Drawing.Size(75, 23);
            this.GETbutton.TabIndex = 5;
            this.GETbutton.Text = "GET";
            this.GETbutton.UseVisualStyleBackColor = true;
            this.GETbutton.Click += new System.EventHandler(this.GETbutton_Click);
            // 
            // Copyrightlabel
            // 
            this.Copyrightlabel.AutoSize = true;
            this.Copyrightlabel.Location = new System.Drawing.Point(303, 3);
            this.Copyrightlabel.Name = "Copyrightlabel";
            this.Copyrightlabel.Size = new System.Drawing.Size(167, 24);
            this.Copyrightlabel.TabIndex = 6;
            this.Copyrightlabel.Text = "Email:chinaluchang@live.com\r\ngithub.com/ludoux/lrchelper";
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Enabled = false;
            this.Cancelbutton.Location = new System.Drawing.Point(226, 4);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 7;
            this.Cancelbutton.Text = "Cancel";
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
            this.StatusgroupBox.Location = new System.Drawing.Point(219, 59);
            this.StatusgroupBox.Name = "StatusgroupBox";
            this.StatusgroupBox.Size = new System.Drawing.Size(200, 100);
            this.StatusgroupBox.TabIndex = 8;
            this.StatusgroupBox.TabStop = false;
            this.StatusgroupBox.Text = "Status";
            // 
            // StatusPDTotalCountlabel
            // 
            this.StatusPDTotalCountlabel.AutoSize = true;
            this.StatusPDTotalCountlabel.Location = new System.Drawing.Point(88, 48);
            this.StatusPDTotalCountlabel.Name = "StatusPDTotalCountlabel";
            this.StatusPDTotalCountlabel.Size = new System.Drawing.Size(11, 12);
            this.StatusPDTotalCountlabel.TabIndex = 4;
            this.StatusPDTotalCountlabel.Text = "0";
            // 
            // StatusPDFinishedCountlabel
            // 
            this.StatusPDFinishedCountlabel.AutoSize = true;
            this.StatusPDFinishedCountlabel.Location = new System.Drawing.Point(88, 60);
            this.StatusPDFinishedCountlabel.Name = "StatusPDFinishedCountlabel";
            this.StatusPDFinishedCountlabel.Size = new System.Drawing.Size(11, 12);
            this.StatusPDFinishedCountlabel.TabIndex = 3;
            this.StatusPDFinishedCountlabel.Text = "0";
            // 
            // StatusPDFinishedlabel
            // 
            this.StatusPDFinishedlabel.AutoSize = true;
            this.StatusPDFinishedlabel.Location = new System.Drawing.Point(23, 60);
            this.StatusPDFinishedlabel.Name = "StatusPDFinishedlabel";
            this.StatusPDFinishedlabel.Size = new System.Drawing.Size(59, 12);
            this.StatusPDFinishedlabel.TabIndex = 2;
            this.StatusPDFinishedlabel.Text = "Finished:";
            // 
            // StatusPDTotallabel
            // 
            this.StatusPDTotallabel.AutoSize = true;
            this.StatusPDTotallabel.Location = new System.Drawing.Point(41, 48);
            this.StatusPDTotallabel.Name = "StatusPDTotallabel";
            this.StatusPDTotallabel.Size = new System.Drawing.Size(41, 12);
            this.StatusPDTotallabel.TabIndex = 1;
            this.StatusPDTotallabel.Text = "Total:";
            // 
            // StatusInfolabel
            // 
            this.StatusInfolabel.Location = new System.Drawing.Point(6, 17);
            this.StatusInfolabel.Name = "StatusInfolabel";
            this.StatusInfolabel.Size = new System.Drawing.Size(188, 31);
            this.StatusInfolabel.TabIndex = 0;
            this.StatusInfolabel.Text = "StatusInfo";
            // 
            // AutoSetcheckBox
            // 
            this.AutoSetcheckBox.AutoSize = true;
            this.AutoSetcheckBox.Checked = true;
            this.AutoSetcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoSetcheckBox.Location = new System.Drawing.Point(278, 38);
            this.AutoSetcheckBox.Name = "AutoSetcheckBox";
            this.AutoSetcheckBox.Size = new System.Drawing.Size(72, 16);
            this.AutoSetcheckBox.TabIndex = 9;
            this.AutoSetcheckBox.Text = "Auto-Set";
            this.toolTip1.SetToolTip(this.AutoSetcheckBox, "To get  the song ID in URLs from clipboard");
            this.AutoSetcheckBox.UseVisualStyleBackColor = true;
            // 
            // AlbumradioButton
            // 
            this.AlbumradioButton.AutoSize = true;
            this.AlbumradioButton.Location = new System.Drawing.Point(219, 37);
            this.AlbumradioButton.Name = "AlbumradioButton";
            this.AlbumradioButton.Size = new System.Drawing.Size(53, 16);
            this.AlbumradioButton.TabIndex = 10;
            this.AlbumradioButton.TabStop = true;
            this.AlbumradioButton.Text = "Album";
            this.AlbumradioButton.UseVisualStyleBackColor = true;
            // 
            // AdvancedSettingsgroupBox
            // 
            this.AdvancedSettingsgroupBox.Controls.Add(this.DelayMsecnumericUpDown);
            this.AdvancedSettingsgroupBox.Controls.Add(this.DelayMseclabel);
            this.AdvancedSettingsgroupBox.Controls.Add(this.LyricsStylenumericUpDown);
            this.AdvancedSettingsgroupBox.Controls.Add(this.LyricsStylelabel);
            this.AdvancedSettingsgroupBox.Location = new System.Drawing.Point(12, 62);
            this.AdvancedSettingsgroupBox.Name = "AdvancedSettingsgroupBox";
            this.AdvancedSettingsgroupBox.Size = new System.Drawing.Size(200, 83);
            this.AdvancedSettingsgroupBox.TabIndex = 11;
            this.AdvancedSettingsgroupBox.TabStop = false;
            this.AdvancedSettingsgroupBox.Visible = false;
            // 
            // DelayMsecnumericUpDown
            // 
            this.DelayMsecnumericUpDown.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.DelayMsecnumericUpDown.Location = new System.Drawing.Point(89, 39);
            this.DelayMsecnumericUpDown.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.DelayMsecnumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DelayMsecnumericUpDown.Name = "DelayMsecnumericUpDown";
            this.DelayMsecnumericUpDown.Size = new System.Drawing.Size(38, 21);
            this.DelayMsecnumericUpDown.TabIndex = 16;
            this.toolTip1.SetToolTip(this.DelayMsecnumericUpDown, "100 is 1sec");
            this.DelayMsecnumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // DelayMseclabel
            // 
            this.DelayMseclabel.AutoSize = true;
            this.DelayMseclabel.Location = new System.Drawing.Point(18, 41);
            this.DelayMseclabel.Name = "DelayMseclabel";
            this.DelayMseclabel.Size = new System.Drawing.Size(65, 12);
            this.DelayMseclabel.TabIndex = 15;
            this.DelayMseclabel.Text = "DelayMsec:";
            this.toolTip1.SetToolTip(this.DelayMseclabel, "How long should the translyrics be delayed?");
            // 
            // LyricsStylenumericUpDown
            // 
            this.LyricsStylenumericUpDown.Location = new System.Drawing.Point(89, 16);
            this.LyricsStylenumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LyricsStylenumericUpDown.Name = "LyricsStylenumericUpDown";
            this.LyricsStylenumericUpDown.Size = new System.Drawing.Size(30, 21);
            this.LyricsStylenumericUpDown.TabIndex = 14;
            this.toolTip1.SetToolTip(this.LyricsStylenumericUpDown, "0:OriLyrics and TransLyrics will be displayed in the different lines.\r\n1:OriLyric" +
        "s and TransLyrics will be displayed  in the same line if it can.");
            // 
            // LyricsStylelabel
            // 
            this.LyricsStylelabel.AutoSize = true;
            this.LyricsStylelabel.Location = new System.Drawing.Point(6, 18);
            this.LyricsStylelabel.Name = "LyricsStylelabel";
            this.LyricsStylelabel.Size = new System.Drawing.Size(77, 12);
            this.LyricsStylelabel.TabIndex = 13;
            this.LyricsStylelabel.Text = "LyricsStyle:";
            this.toolTip1.SetToolTip(this.LyricsStylelabel, "Different styles for different environment.");
            // 
            // AdvancedSettingscheckBox
            // 
            this.AdvancedSettingscheckBox.AutoSize = true;
            this.AdvancedSettingscheckBox.Location = new System.Drawing.Point(14, 60);
            this.AdvancedSettingscheckBox.Name = "AdvancedSettingscheckBox";
            this.AdvancedSettingscheckBox.Size = new System.Drawing.Size(120, 16);
            this.AdvancedSettingscheckBox.TabIndex = 12;
            this.AdvancedSettingscheckBox.Text = "AdvancedSettings";
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
            // LrcDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 172);
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
            this.MaximizeBox = false;
            this.Name = "LrcDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LrcDownloader";
            this.Activated += new System.EventHandler(this.LrcDownloader_Activated);
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
    }
}