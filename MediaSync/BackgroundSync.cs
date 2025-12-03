using MediaDevices;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MediaSync
{
    public partial class BackgroundSync : Form
    {
        private RichTextBox progressBox;
        private BackgroundWorker backgroundWorker;
        public static MediaDevice mDevice;
        private string deviceId;
        private string pcPath;
        private string mobilePath;
        private List<string> filesToSync;

        public BackgroundSync(string pcPathf, string mobilePathf, List<string> filesToSyncf)
        {
            InitializeComponent();
            pcPath = pcPathf;
            mobilePath = mobilePathf;
            filesToSync = filesToSyncf;
            mDevice = Form1.mDevice;
            progressBox = new RichTextBox();
            backgroundWorker = new BackgroundWorker();

            // progressBox
            progressBox.Dock = DockStyle.Fill;
            progressBox.ReadOnly = true;

            // backgroundWorker
            backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            backgroundWorker.WorkerSupportsCancellation = true;

            // Form
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.progressBox);
            this.Text = "Sync Job Pending...";
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.Size = new System.Drawing.Size(800, 400);

            StartWorker();
        }

        private void StartWorker()
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (mDevice != null)
                {
                    mDevice.Connect();
                    int counter = 1;
                    int titleCounter = filesToSync.Count();
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.Text = $"Syncing {counter} / {titleCounter}";
                        }));
                    }
                    else
                    {
                        this.Text = $"Syncing {counter} / {titleCounter}";
                    }
                    foreach (string file in filesToSync)
                    {
                        if (backgroundWorker.CancellationPending)
                        {
                            mDevice.Disconnect();
                            e.Cancel = true;
                            return;
                        }
                        try
                        {
                            string pcFile = $"{pcPath}{file}";
                            string mobileFile = $"{mobilePath}{file}";
                            string mobileFolder = Path.GetDirectoryName(mobileFile);
                            string textUpdate = $"Syncing {file}";

                            writeProgress("MSG1", textUpdate);
                            if (mobileFolder != mobilePath && !mDevice.DirectoryExists(mobileFolder))
                            {
                                writeProgress("MSG1", $"Creating directory {mobileFolder}");
                                mDevice.CreateDirectory(mobileFolder);
                            }

                            try
                            {
                                if (!mDevice.FileExists(mobileFile))
                                {
                                    mDevice.UploadFile(pcFile, mobileFile);
                                    writeProgress("MSG", $"Synced {file}");
                                    counter++;
                                }
                                else
                                {
                                    writeProgress("MSG", $"File already exists: {file}");
                                    counter++;
                                }

                            }
                            catch (Exception ex)
                            {
                                writeProgress("ERROR", $"There was an error writing the file: {e.ToString()}");
                            }

                        }
                        catch (Exception ex)
                        {
                            writeProgress("ERROR", $"There was an error writing the file: {ex.Message}");
                        }
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                this.Text = $"Syncing {counter} / {titleCounter}";
                            }));
                        }
                        else
                        {
                            this.Text = $"Syncing {counter} / {titleCounter}";
                        }
                    }
                    mDevice.Disconnect();
                }
            }catch(Exception ex)
            {
                writeProgress("ERROR", $"There was an error sync'ing the device:\n\n{ex.Message}");
            }
        }
        private void writeProgress(string type = "MSG", string text = "")
        {
            try
            {
                if (progressBox.InvokeRequired)
                {
                    progressBox.Invoke(new Action(() =>
                    {
                        if (type == "ERROR")
                        {
                            progressBox.SelectionStart = progressBox.TextLength;
                            progressBox.SelectionColor = Color.Red;
                        }
                        else if (type == "MSG")
                        {
                            progressBox.SelectionStart = progressBox.TextLength;
                            progressBox.SelectionColor = Color.Green;
                        }
                        else
                        {
                            progressBox.SelectionStart = progressBox.TextLength;
                            progressBox.SelectionColor = progressBox.ForeColor;
                        }
                        progressBox.AppendText($"{text}\n");
                        progressBox.SelectionStart = progressBox.TextLength;
                        progressBox.ScrollToCaret();
                    }));
                }
                else
                {
                    if (type == "ERROR")
                    {
                        progressBox.SelectionStart = progressBox.TextLength;
                        progressBox.SelectionColor = Color.Red;
                    }
                    else if (type == "MSG")
                    {
                        progressBox.SelectionStart = progressBox.TextLength;
                        progressBox.SelectionColor = Color.Green;
                    }
                    else
                    {
                        progressBox.SelectionStart = progressBox.TextLength;
                        progressBox.SelectionColor = progressBox.ForeColor;
                    }
                    progressBox.AppendText($"{text}\n");
                    progressBox.SelectionStart = progressBox.TextLength;
                    progressBox.ScrollToCaret();
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Text = "Sync Job CANCELLED!";
                writeProgress("ERROR", "\nSync Job CANCELLED!");
            }
            else if (e.Error != null)
            {
                // Handle error
            }
            else
            {
                this.Text = "Sync Job Completed!";
                writeProgress("MSG", "\nSync Job Completed!");
            }

        }

        private void BackgroundSync_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                writeProgress("MSG1", "Cancelling Sync, please wait...");
                e.Cancel = true;
            }
        }

        private void BackgroundSync_Load(object sender, EventArgs e)
        {

        }
    }
}