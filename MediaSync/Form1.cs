using MediaDevices;
using NAudio.Wave;
using ATL.AudioData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static MediaSync.AlbumArt;
using ATL;
using System.Reflection;
using MediaSync.Properties;

namespace MediaSync
{

    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        Int32 fileCountPC = 0;
        Int32 fileCountMobile = 0;
        Int32 currentlySelectedPC = 0;
        string deviceId = null;
        public static MediaDevice mDevice = null;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        TimeSpan currentSongTime;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            versionString.Text = $"v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            deviceId = MediaDevice.GetDevices().FirstOrDefault()?.DeviceId;
            if (deviceId == null)
            {
                MessageBox.Show("Error enumerating MTP devices!", "No MTP Devices Detected!");
                Application.Exit();
            } else
            {
                var devices = MediaDevice.GetDevices();
                mDevice = devices.FirstOrDefault(d => d.DeviceId == deviceId);
            }
            if (mDevice != null)
            {
                mDevice.DeviceRemoved += mDevice_DeviceRemoved;
            }
            mediaSyncPathPC.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            List<string> foldersMtp = ScanConnectedDevicesForMusicFolders();
            foreach(string folder in foldersMtp)
            {
                mediaSyncPathMobile.Items.Add(folder);
            }
            if (mediaSyncPathMobile.Items.Count > 0)
            {
                mediaSyncPathMobile.SelectedItem = mediaSyncPathMobile.Items[0];
            } else
            {
                MessageBox.Show("A mobile device was found, but no music folders detected on the mobile device.\nIf this is an Android device running Android 6.0 or higher, please switch the device to \"File Transfer\" mode in the notifications.", "Error");
                Application.Exit();
            }
            mobileGroupBox.Text = $"Connected to {mDevice.FriendlyName}";
            runScanner();
        }

        private void mDevice_DeviceRemoved(object? sender, MediaDeviceEventArgs e)
        {
            MessageBox.Show("The device has been disconnected, closing application!", "Device Disconnected!");
            Application.Exit();
        }
        /// <summary>
        /// Scans a specific media device for folders named "Music" in its root directory and storage containers.
        /// </summary>
        /// <param name="deviceId">The device ID of the media device to scan.</param>
        /// <returns>A list of full paths to "Music" folders found on the specified device.</returns>
        public static List<string> ScanConnectedDevicesForMusicFolders()
        {
            var musicFolders = new List<string>();

            try
            {
                // Connect to the device
                mDevice.Connect();

                // Get the root directory
                MediaDirectoryInfo rootPath = mDevice.GetDirectoryInfo(@"\");

                if (rootPath != null)
                {
                    // Enumerate directories in the root
                    foreach (var directory in rootPath.EnumerateDirectories())
                    {
                        // Check if the directory name is "Music" (case-insensitive)
                        if (directory.Name.Equals("Music", StringComparison.OrdinalIgnoreCase))
                        {
                            musicFolders.Add(directory.FullName);
                        }
                        // Check for common storage container names
                        else if (directory.Name.Contains("Storage", StringComparison.OrdinalIgnoreCase) ||
                                 directory.Name.Contains("SD Card", StringComparison.OrdinalIgnoreCase) ||
                                 directory.Name.Contains("SDCard", StringComparison.OrdinalIgnoreCase) ||
                                 directory.Name.Contains("Card", StringComparison.OrdinalIgnoreCase))
                        {
                            // Scan inside storage containers for Music folders
                            try
                            {
                                foreach (var subDirectory in directory.EnumerateDirectories())
                                {
                                    if (subDirectory.Name.Equals("Music", StringComparison.OrdinalIgnoreCase))
                                    {
                                        musicFolders.Add(subDirectory.FullName);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error scanning storage container {directory.Name}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                MessageBox.Show($"Error scanning device {mDevice.FriendlyName}: {ex.Message}");
            }
            finally
            {
                // Disconnect from the device
                if (mDevice.IsConnected)
                {
                    mDevice.Disconnect();
                }
            }

            return musicFolders;
        }
        private void runScanner()
        {
            fileCountMobile = 0;
            fileCountPC = 0;
            generalProgressLabel.Text = "Scanning media, please wait...";
            syncBtn.Enabled = false;
            refreshBtn.Enabled = false;
            mediaSyncPathMobile.Enabled = false;
            mediaDetectionWorker.RunWorkerAsync();
        }
        public void ScanMusicFolder()
        {
            try
            {
                if (mDevice != null)
                {
                    mDevice.Connect();

                    string internalShared = "";
                    mediaSyncPathMobile.Invoke(new Action(() =>
                    {
                        internalShared = mediaSyncPathMobile.SelectedItem.ToString();
                    }));

                    bool folderExists = mDevice.GetDirectoryInfo(internalShared) != null;

                    if (folderExists)
                    {
                        var musicFolder = mDevice.GetDirectoryInfo(internalShared);
                        if (musicFolder != null)
                        {
                            ScanRecursive(musicFolder);
                        }

                        mDevice.Disconnect();
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"There was an error in ScanMusicFolder() function. {ex.Message}", "Error");
            }

        }

        private void ScanRecursive(MediaDirectoryInfo directory)
        {
            try
            {
                var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".m4a", ".wma" };

                foreach (var file in directory.EnumerateFiles())
                {
                    string fileName = file.Name;
                    string filePath = file.FullName;
                    if (extensions.Contains(Path.GetExtension(fileName)))
                    {
                        // Update UI on main thread
                        mediaListMobile.Invoke(new Action(() =>
                        {
                            var item = new ListViewItem(fileName);
                            item.SubItems.Add(filePath);
                            mediaListMobile.Items.Add(item);
                        }));
                        fileCountMobile++;
                    }
                }

                foreach (var subDir in directory.EnumerateDirectories())
                {
                    ScanRecursive(subDir);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void syncFolders(System.Windows.Forms.ListView pcMediaFiles, System.Windows.Forms.ListView mobileMediaFiles)
        {
            syncBtn.Enabled = false;
            string pcRootPath = mediaSyncPathPC.Text;
            string mobileRootPath = "";
            mediaSyncPathMobile.Invoke(new Action(() =>
            {
                mobileRootPath = mediaSyncPathMobile.SelectedItem.ToString();
            }));
            List<string> allDiffs = GetDifferences(pcMediaFiles, mobileMediaFiles);
            int count = allDiffs.Count;
            if (count > 0)
            {
                if (MessageBox.Show("There are " + count + " differences between the PC and Mobile Device. Would you like to see the differences before syncing?", $"{count} Pending Transfers", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Form form = new DifferencesForm(allDiffs, count);
                    form.ShowDialog();
                }
                Form worker = new BackgroundSync(pcRootPath, mobileRootPath, allDiffs);
                worker.ShowDialog();
                runScanner();
            }
            else
            {
                MessageBox.Show("Nothing to sync!");
            }
            syncBtn.Enabled = true;
        }

        public List<string> GetDifferences(System.Windows.Forms.ListView listView1, System.Windows.Forms.ListView listView2)
        {
            string mediaSyncPathMobileStr = "";
            mediaSyncPathMobile.Invoke(new Action(() =>
            {
                mediaSyncPathMobileStr = mediaSyncPathMobile.SelectedItem.ToString();
            }));
            var paths1 = listView1.Items.Cast<ListViewItem>()
                .Select(item => item.SubItems[1].Text.Replace(mediaSyncPathPC.Text, "")) // Replace backslashes with forward slashes
                .ToHashSet();

            var paths2 = listView2.Items.Cast<ListViewItem>()
                .Select(item => item.SubItems[1].Text.Replace(mediaSyncPathMobileStr, "")) // Replace backslashes with forward slashes
                .ToHashSet();

            var in1NotIn2 = paths1.Except(paths2).ToList();
            //var in2NotIn1 = paths2.Except(paths1).ToList();

            var differences = new List<string>();
            differences.AddRange(in1NotIn2.Select(path => $"{path}"));
            //differences.AddRange(in2NotIn1.Select(path => $"On Mobile Only: {path}"));

            return differences;
        }
        private void mediaDetectionWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                mediaListPC.Invoke(new Action(() =>
                {
                    mediaListPC.Items.Clear();
                }));
                mediaListMobile.Invoke(new Action(() =>
                {
                    mediaListMobile.Items.Clear();
                }));
                string path = mediaSyncPathPC.Text;
                var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".m4a", ".wma" };
                mediaListPC.Invoke(new Action(() =>
                {
                    mediaListPC.BeginUpdate();
                }));
                foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                {
                    if (extensions.Contains(Path.GetExtension(file)))
                    {
                        fileCountPC++;
                        string fileName = Path.GetFileName(file);
                        string filePath = file;

                        // Update UI on main thread
                        mediaListPC.Invoke(new Action(() =>
                        {
                            var item = new ListViewItem(fileName);
                            item.SubItems.Add(filePath);
                            mediaListPC.Items.Add(item);
                        }));
                    }
                }
                mediaListPC.Invoke(new Action(() =>
                {
                    mediaListPC.EndUpdate();
                }));
                if (deviceId != null)
                {
                    mediaListMobile.Invoke(new Action(() =>
                    {
                        mediaListMobile.BeginUpdate();
                    }));
                    ScanMusicFolder();
                    mediaListMobile.Invoke(new Action(() =>
                    {
                        mediaListMobile.EndUpdate();
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mediaDetectionWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private async void mediaDetectionWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            generalProgressLabel.Text = "Media scanning completed! Found " + fileCountPC + " media files on PC && " + fileCountMobile + " on mobile.";
            syncBtn.Enabled = true;
            refreshBtn.Enabled = true;
            mediaSyncPathMobile.Enabled = true;
        }

        private void syncWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (deviceId != null)
            {
                syncFolders(mediaListPC, mediaListMobile);
            }
        }
        private void syncWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void syncWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }

        private void syncBtn_Click(object sender, EventArgs e)
        {
            syncFolders(mediaListPC, mediaListMobile);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mediaDetectionWorker.IsBusy || syncWorker.IsBusy)
            {
                e.Cancel = true;
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            runScanner();
        }

        private void mediaListPC_DoubleClick(object sender, EventArgs e)
        {
            ListViewHitTestInfo hit = mediaListPC.HitTest(mediaListPC.PointToClient(Cursor.Position));
            if (hit.Item != null)
            {
                // Get the main item
                ListViewItem item = hit.Item;
                currentlySelectedPC = item.Index;
                // Get the subitem if exists
                if (hit.SubItem != null)
                {
                    playAudio(hit.Item.SubItems[1].Text);
                }
            }
        }
        private void playAudio(string fileToPlay)
        {
            try
            {
                Track audioTrack = new Track(fileToPlay);
                string artist = audioTrack.Artist;
                string name = audioTrack.Title;

                artistTitle.Text = $"{artist}";
                songTitle.Text = $"{name}";
                //Start playing:
                if (outputDevice == null)
                {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlaybackStopped;
                }
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Stop();
                }
                bool loadedArt = LoadAlbumArtToPictureBox(albumArt, fileToPlay);
                if (!loadedArt)
                {
                    albumArt.Image = Resources.default_album;
                }
                audioFile = new AudioFileReader(fileToPlay);
                audioFile.Volume = 0.50f;
                currentSongTime = audioFile.TotalTime;
                outputDevice.Init(audioFile);
                float newVolume = volRocker.Value;
                outputDevice.Volume = (newVolume / 100.00f);
                outputDevice.Play();
                playBtn.Image = Properties.Resources.pause;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error trying to play file");
            }
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {

        }

        private void volRocker_Scroll(object sender, ScrollEventArgs e)
        {
            float newVolume = volRocker.Value;
            if (outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Volume = (newVolume / 100.00f);
                }

            }
            volLabel.Text = newVolume.ToString();
        }

        private void seekTimer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                TimeSpan ts = audioFile.CurrentTime;
                Double maxTime = currentSongTime.TotalSeconds;
                Int32 currentTime = ((int)ts.TotalSeconds);

                musicTimer.Maximum = ((int)maxTime);
                musicTimer.Value = currentTime;
                currentTimeStamp.Text = ts.ToString("mm':'ss");
                totalTimeStamp.Text = currentSongTime.ToString("mm':'ss");
            }
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            if (audioFile != null && outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Pause();
                    playBtn.Image = Properties.Resources.play;
                }
                else
                {
                    outputDevice.Play();
                    playBtn.Image = Properties.Resources.pause;
                }
            }
        }

        private void mediaListPC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void forwardBtn_Click(object sender, EventArgs e)
        {
            currentlySelectedPC++;
            // Check if we have a valid selected index
            if (currentlySelectedPC >= 0 && currentlySelectedPC < mediaListPC.Items.Count)
            {
                ListViewItem item = mediaListPC.Items[currentlySelectedPC];
                if (item.SubItems.Count > 1)
                {
                    string subItemText = item.SubItems[1].Text;
                    playAudio(subItemText);
                }
            }
        }

        private void rewindBtn_Click(object sender, EventArgs e)
        {
            currentlySelectedPC--;
            // Check if we have a valid selected index
            if (currentlySelectedPC >= 0 && currentlySelectedPC < mediaListPC.Items.Count)
            {
                ListViewItem item = mediaListPC.Items[currentlySelectedPC];
                if (item.SubItems.Count > 1)
                {
                    string subItemText = item.SubItems[1].Text;
                    playAudio(subItemText);
                }
            }
        }
    }
}
