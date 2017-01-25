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
            this.IDtextBox = new System.Windows.Forms.TextBox();
            this.IDlabel = new System.Windows.Forms.Label();
            this.Functionlabel = new System.Windows.Forms.Label();
            this.MusicradioButton = new System.Windows.Forms.RadioButton();
            this.PlaylistradioButton = new System.Windows.Forms.RadioButton();
            this.GETbutton = new System.Windows.Forms.Button();
            this.Infolabel = new System.Windows.Forms.Label();
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
            // Infolabel
            // 
            this.Infolabel.AutoSize = true;
            this.Infolabel.Location = new System.Drawing.Point(12, 69);
            this.Infolabel.Name = "Infolabel";
            this.Infolabel.Size = new System.Drawing.Size(185, 24);
            this.Infolabel.TabIndex = 6;
            this.Infolabel.Text = "Email:chinaluchang@live.com\r\nAT github.com/Ludoux/LRCHelper";
            // 
            // LrcDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 119);
            this.Controls.Add(this.Infolabel);
            this.Controls.Add(this.GETbutton);
            this.Controls.Add(this.PlaylistradioButton);
            this.Controls.Add(this.MusicradioButton);
            this.Controls.Add(this.Functionlabel);
            this.Controls.Add(this.IDlabel);
            this.Controls.Add(this.IDtextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LrcDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LrcDownloader";
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
        private System.Windows.Forms.Label Infolabel;
    }
}