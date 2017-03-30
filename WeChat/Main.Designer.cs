namespace WeChat
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panelList = new System.Windows.Forms.Panel();
            this.pbTop = new System.Windows.Forms.Panel();
            this.pbMin = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbMessage = new System.Windows.Forms.PictureBox();
            this.pbFriends = new System.Windows.Forms.PictureBox();
            this.panelChat = new System.Windows.Forms.Panel();
            this.pbTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFriends)).BeginInit();
            this.SuspendLayout();
            // 
            // panelList
            // 
            this.panelList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panelList.Location = new System.Drawing.Point(51, -1);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(215, 446);
            this.panelList.TabIndex = 0;
            // 
            // pbTop
            // 
            this.pbTop.Controls.Add(this.pbMin);
            this.pbTop.Controls.Add(this.pbClose);
            this.pbTop.Location = new System.Drawing.Point(266, 0);
            this.pbTop.Name = "pbTop";
            this.pbTop.Size = new System.Drawing.Size(530, 38);
            this.pbTop.TabIndex = 2;
            this.pbTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTop_MouseDown);
            this.pbTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTop_MouseMove);
            // 
            // pbMin
            // 
            this.pbMin.BackgroundImage = global::WeChat.Properties.Resources.min;
            this.pbMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbMin.Location = new System.Drawing.Point(464, 10);
            this.pbMin.Name = "pbMin";
            this.pbMin.Size = new System.Drawing.Size(20, 20);
            this.pbMin.TabIndex = 2;
            this.pbMin.TabStop = false;
            this.pbMin.Click += new System.EventHandler(this.pbMin_Click);
            // 
            // pbClose
            // 
            this.pbClose.BackgroundImage = global::WeChat.Properties.Resources.close;
            this.pbClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbClose.Location = new System.Drawing.Point(498, 10);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(20, 20);
            this.pbClose.TabIndex = 1;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.pbMessage);
            this.panel3.Controls.Add(this.pbFriends);
            this.panel3.Location = new System.Drawing.Point(-1, -1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(53, 447);
            this.panel3.TabIndex = 1;
            // 
            // pbMessage
            // 
            this.pbMessage.BackgroundImage = global::WeChat.Properties.Resources.message2_1_;
            this.pbMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbMessage.Location = new System.Drawing.Point(6, 21);
            this.pbMessage.Name = "pbMessage";
            this.pbMessage.Size = new System.Drawing.Size(35, 32);
            this.pbMessage.TabIndex = 2;
            this.pbMessage.TabStop = false;
            this.pbMessage.Click += new System.EventHandler(this.pbMessage_Click);
            // 
            // pbFriends
            // 
            this.pbFriends.BackgroundImage = global::WeChat.Properties.Resources.haoyou;
            this.pbFriends.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFriends.Location = new System.Drawing.Point(6, 74);
            this.pbFriends.Name = "pbFriends";
            this.pbFriends.Size = new System.Drawing.Size(35, 37);
            this.pbFriends.TabIndex = 0;
            this.pbFriends.TabStop = false;
            this.pbFriends.Click += new System.EventHandler(this.pbFriends_Click);
            // 
            // panelChat
            // 
            this.panelChat.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelChat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelChat.BackgroundImage")));
            this.panelChat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelChat.Location = new System.Drawing.Point(266, 38);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(529, 408);
            this.panelChat.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 445);
            this.Controls.Add(this.pbTop);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelChat);
            this.Controls.Add(this.panelList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeChat";
            this.pbTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFriends)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbFriends;
        private System.Windows.Forms.Panel pbTop;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbMin;
        private System.Windows.Forms.PictureBox pbMessage;
    }
}