namespace MediaSync
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            groupBox1 = new GroupBox();
            mediaSyncPathPC = new TextBox();
            mediaListPC = new ListView();
            mediaName = new ColumnHeader();
            mediaPath = new ColumnHeader();
            mobileGroupBox = new GroupBox();
            mediaSyncPathMobile = new MetroFramework.Controls.MetroComboBox();
            mediaListMobile = new ListView();
            mediaName2 = new ColumnHeader();
            mediaPath2 = new ColumnHeader();
            syncBtn = new MetroFramework.Controls.MetroButton();
            syncWorker = new System.ComponentModel.BackgroundWorker();
            mediaDetectionWorker = new System.ComponentModel.BackgroundWorker();
            generalProgressLabel = new MetroFramework.Controls.MetroLabel();
            refreshBtn = new MetroFramework.Controls.MetroButton();
            albumArt = new PictureBox();
            artistTitle = new MetroFramework.Controls.MetroLabel();
            musicTimer = new MetroFramework.Controls.MetroTrackBar();
            playBtn = new PictureBox();
            rewindBtn = new PictureBox();
            forwardBtn = new PictureBox();
            songTitle = new MetroFramework.Controls.MetroLabel();
            volRocker = new MetroFramework.Controls.MetroTrackBar();
            volLabel = new MetroFramework.Controls.MetroLabel();
            seekTimer = new System.Windows.Forms.Timer(components);
            currentTimeStamp = new MetroFramework.Controls.MetroLabel();
            totalTimeStamp = new MetroFramework.Controls.MetroLabel();
            versionString = new MetroFramework.Controls.MetroLabel();
            groupBox1.SuspendLayout();
            mobileGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)albumArt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playBtn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rewindBtn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)forwardBtn).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(mediaSyncPathPC);
            groupBox1.Controls.Add(mediaListPC);
            groupBox1.Location = new Point(23, 53);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(410, 438);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Computer Media";
            // 
            // mediaSyncPathPC
            // 
            mediaSyncPathPC.Dock = DockStyle.Top;
            mediaSyncPathPC.Font = new Font("Segoe UI", 12F);
            mediaSyncPathPC.Location = new Point(3, 19);
            mediaSyncPathPC.Name = "mediaSyncPathPC";
            mediaSyncPathPC.ReadOnly = true;
            mediaSyncPathPC.Size = new Size(404, 29);
            mediaSyncPathPC.TabIndex = 1;
            mediaSyncPathPC.Text = "C:\\Users\\Admin\\Music";
            // 
            // mediaListPC
            // 
            mediaListPC.Columns.AddRange(new ColumnHeader[] { mediaName, mediaPath });
            mediaListPC.Dock = DockStyle.Bottom;
            mediaListPC.GridLines = true;
            mediaListPC.Location = new Point(3, 53);
            mediaListPC.Name = "mediaListPC";
            mediaListPC.Size = new Size(404, 382);
            mediaListPC.TabIndex = 0;
            mediaListPC.UseCompatibleStateImageBehavior = false;
            mediaListPC.View = View.Details;
            mediaListPC.SelectedIndexChanged += mediaListPC_SelectedIndexChanged;
            mediaListPC.DoubleClick += mediaListPC_DoubleClick;
            // 
            // mediaName
            // 
            mediaName.DisplayIndex = 1;
            mediaName.Text = "Name";
            mediaName.Width = 300;
            // 
            // mediaPath
            // 
            mediaPath.DisplayIndex = 0;
            mediaPath.Text = "Path";
            // 
            // mobileGroupBox
            // 
            mobileGroupBox.Controls.Add(mediaSyncPathMobile);
            mobileGroupBox.Controls.Add(mediaListMobile);
            mobileGroupBox.Location = new Point(439, 53);
            mobileGroupBox.Name = "mobileGroupBox";
            mobileGroupBox.Size = new Size(408, 438);
            mobileGroupBox.TabIndex = 1;
            mobileGroupBox.TabStop = false;
            mobileGroupBox.Text = "Mobile Media";
            // 
            // mediaSyncPathMobile
            // 
            mediaSyncPathMobile.Dock = DockStyle.Top;
            mediaSyncPathMobile.FormattingEnabled = true;
            mediaSyncPathMobile.ItemHeight = 23;
            mediaSyncPathMobile.Location = new Point(3, 19);
            mediaSyncPathMobile.Name = "mediaSyncPathMobile";
            mediaSyncPathMobile.Size = new Size(402, 29);
            mediaSyncPathMobile.TabIndex = 3;
            // 
            // mediaListMobile
            // 
            mediaListMobile.Columns.AddRange(new ColumnHeader[] { mediaName2, mediaPath2 });
            mediaListMobile.Dock = DockStyle.Bottom;
            mediaListMobile.GridLines = true;
            mediaListMobile.Location = new Point(3, 53);
            mediaListMobile.Name = "mediaListMobile";
            mediaListMobile.Size = new Size(402, 382);
            mediaListMobile.TabIndex = 2;
            mediaListMobile.UseCompatibleStateImageBehavior = false;
            mediaListMobile.View = View.Details;
            // 
            // mediaName2
            // 
            mediaName2.DisplayIndex = 1;
            mediaName2.Text = "Name";
            mediaName2.Width = 300;
            // 
            // mediaPath2
            // 
            mediaPath2.DisplayIndex = 0;
            mediaPath2.Text = "Path";
            // 
            // syncBtn
            // 
            syncBtn.FlatStyle = FlatStyle.Flat;
            syncBtn.Location = new Point(23, 526);
            syncBtn.Name = "syncBtn";
            syncBtn.Size = new Size(75, 23);
            syncBtn.TabIndex = 2;
            syncBtn.Text = "Sync";
            syncBtn.UseVisualStyleBackColor = true;
            syncBtn.Click += syncBtn_Click;
            // 
            // syncWorker
            // 
            syncWorker.WorkerReportsProgress = true;
            syncWorker.WorkerSupportsCancellation = true;
            syncWorker.DoWork += syncWorker_DoWork;
            syncWorker.ProgressChanged += syncWorker_ProgressChanged;
            syncWorker.RunWorkerCompleted += syncWorker_RunWorkerCompleted;
            // 
            // mediaDetectionWorker
            // 
            mediaDetectionWorker.WorkerReportsProgress = true;
            mediaDetectionWorker.DoWork += mediaDetectionWorker_DoWork;
            mediaDetectionWorker.ProgressChanged += mediaDetectionWorker_ProgressChanged;
            mediaDetectionWorker.RunWorkerCompleted += mediaDetectionWorker_RunWorkerCompleted;
            // 
            // generalProgressLabel
            // 
            generalProgressLabel.AutoSize = true;
            generalProgressLabel.Location = new Point(23, 552);
            generalProgressLabel.Name = "generalProgressLabel";
            generalProgressLabel.Size = new Size(82, 19);
            generalProgressLabel.TabIndex = 3;
            generalProgressLabel.Text = "Please wait...";
            // 
            // refreshBtn
            // 
            refreshBtn.Location = new Point(23, 497);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(75, 23);
            refreshBtn.TabIndex = 4;
            refreshBtn.Text = "Refresh";
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // albumArt
            // 
            albumArt.Image = Properties.Resources.default_album;
            albumArt.Location = new Point(881, 106);
            albumArt.Name = "albumArt";
            albumArt.Size = new Size(256, 256);
            albumArt.SizeMode = PictureBoxSizeMode.Zoom;
            albumArt.TabIndex = 5;
            albumArt.TabStop = false;
            // 
            // artistTitle
            // 
            artistTitle.AutoSize = true;
            artistTitle.Location = new Point(881, 384);
            artistTitle.Name = "artistTitle";
            artistTitle.Size = new Size(62, 19);
            artistTitle.TabIndex = 6;
            artistTitle.Text = "No Artist";
            artistTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // musicTimer
            // 
            musicTimer.BackColor = Color.Transparent;
            musicTimer.Location = new Point(920, 406);
            musicTimer.Name = "musicTimer";
            musicTimer.Size = new Size(178, 23);
            musicTimer.TabIndex = 7;
            // 
            // playBtn
            // 
            playBtn.Cursor = Cursors.Hand;
            playBtn.Image = Properties.Resources.play;
            playBtn.Location = new Point(977, 431);
            playBtn.Name = "playBtn";
            playBtn.Size = new Size(64, 64);
            playBtn.SizeMode = PictureBoxSizeMode.Zoom;
            playBtn.TabIndex = 8;
            playBtn.TabStop = false;
            playBtn.Click += playBtn_Click;
            // 
            // rewindBtn
            // 
            rewindBtn.Cursor = Cursors.Hand;
            rewindBtn.Image = Properties.Resources.rewind;
            rewindBtn.Location = new Point(920, 441);
            rewindBtn.Name = "rewindBtn";
            rewindBtn.Size = new Size(45, 45);
            rewindBtn.SizeMode = PictureBoxSizeMode.Zoom;
            rewindBtn.TabIndex = 9;
            rewindBtn.TabStop = false;
            rewindBtn.Click += rewindBtn_Click;
            // 
            // forwardBtn
            // 
            forwardBtn.Cursor = Cursors.Hand;
            forwardBtn.Image = Properties.Resources.foward;
            forwardBtn.Location = new Point(1053, 441);
            forwardBtn.Name = "forwardBtn";
            forwardBtn.Size = new Size(45, 45);
            forwardBtn.SizeMode = PictureBoxSizeMode.Zoom;
            forwardBtn.TabIndex = 10;
            forwardBtn.TabStop = false;
            forwardBtn.Click += forwardBtn_Click;
            // 
            // songTitle
            // 
            songTitle.AutoSize = true;
            songTitle.Location = new Point(881, 365);
            songTitle.Name = "songTitle";
            songTitle.Size = new Size(109, 19);
            songTitle.TabIndex = 11;
            songTitle.Text = "Nothing Selected";
            songTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // volRocker
            // 
            volRocker.BackColor = Color.Transparent;
            volRocker.Location = new Point(1048, 526);
            volRocker.Name = "volRocker";
            volRocker.Size = new Size(103, 23);
            volRocker.TabIndex = 12;
            volRocker.Text = "Volume";
            volRocker.Theme = MetroFramework.MetroThemeStyle.Light;
            volRocker.Value = 15;
            volRocker.Scroll += volRocker_Scroll;
            // 
            // volLabel
            // 
            volLabel.AutoSize = true;
            volLabel.Location = new Point(1015, 527);
            volLabel.Name = "volLabel";
            volLabel.Size = new Size(21, 19);
            volLabel.TabIndex = 13;
            volLabel.Text = "15";
            volLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // seekTimer
            // 
            seekTimer.Enabled = true;
            seekTimer.Interval = 500;
            seekTimer.Tick += seekTimer_Tick;
            // 
            // currentTimeStamp
            // 
            currentTimeStamp.AutoSize = true;
            currentTimeStamp.Location = new Point(881, 407);
            currentTimeStamp.Name = "currentTimeStamp";
            currentTimeStamp.Size = new Size(33, 19);
            currentTimeStamp.TabIndex = 14;
            currentTimeStamp.Text = "0:00";
            currentTimeStamp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // totalTimeStamp
            // 
            totalTimeStamp.AutoSize = true;
            totalTimeStamp.Location = new Point(1104, 407);
            totalTimeStamp.Name = "totalTimeStamp";
            totalTimeStamp.Size = new Size(33, 19);
            totalTimeStamp.TabIndex = 15;
            totalTimeStamp.Text = "0:00";
            totalTimeStamp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // versionString
            // 
            versionString.AutoSize = true;
            versionString.Location = new Point(234, 30);
            versionString.Name = "versionString";
            versionString.Size = new Size(40, 19);
            versionString.TabIndex = 16;
            versionString.Text = "v1.0.0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            ClientSize = new Size(1160, 580);
            Controls.Add(versionString);
            Controls.Add(totalTimeStamp);
            Controls.Add(currentTimeStamp);
            Controls.Add(volLabel);
            Controls.Add(volRocker);
            Controls.Add(songTitle);
            Controls.Add(forwardBtn);
            Controls.Add(rewindBtn);
            Controls.Add(playBtn);
            Controls.Add(musicTimer);
            Controls.Add(artistTitle);
            Controls.Add(albumArt);
            Controls.Add(refreshBtn);
            Controls.Add(generalProgressLabel);
            Controls.Add(syncBtn);
            Controls.Add(mobileGroupBox);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(1160, 580);
            MinimumSize = new Size(1160, 580);
            Name = "Form1";
            Text = "Android Media Sync";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            mobileGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)albumArt).EndInit();
            ((System.ComponentModel.ISupportInitialize)playBtn).EndInit();
            ((System.ComponentModel.ISupportInitialize)rewindBtn).EndInit();
            ((System.ComponentModel.ISupportInitialize)forwardBtn).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox mediaSyncPathPC;
        private ListView mediaListPC;
        private ColumnHeader mediaPath;
        private ColumnHeader mediaName;
        private GroupBox mobileGroupBox;
        private ListView mediaListMobile;
        private ColumnHeader mediaPath2;
        private ColumnHeader mediaName2;
        private MetroFramework.Controls.MetroButton syncBtn;
        private System.ComponentModel.BackgroundWorker syncWorker;
        private System.ComponentModel.BackgroundWorker mediaDetectionWorker;
        private MetroFramework.Controls.MetroLabel generalProgressLabel;
        private MetroFramework.Controls.MetroButton refreshBtn;
        private PictureBox albumArt;
        private MetroFramework.Controls.MetroLabel artistTitle;
        private MetroFramework.Controls.MetroTrackBar musicTimer;
        private PictureBox playBtn;
        private PictureBox rewindBtn;
        private PictureBox forwardBtn;
        private MetroFramework.Controls.MetroLabel songTitle;
        private MetroFramework.Controls.MetroTrackBar volRocker;
        private MetroFramework.Controls.MetroLabel volLabel;
        private System.Windows.Forms.Timer seekTimer;
        private MetroFramework.Controls.MetroLabel currentTimeStamp;
        private MetroFramework.Controls.MetroLabel totalTimeStamp;
        private MetroFramework.Controls.MetroComboBox mediaSyncPathMobile;
        private MetroFramework.Controls.MetroLabel versionString;
    }
}
