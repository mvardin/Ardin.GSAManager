using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Ardin.GSAManager
{
    public partial class frmMain : Form
    {
        public bool SerDownloadCompleted { get; set; }
        public bool CBDownloadCompleted { get; set; }
        public frmMain()
        {
            InitializeComponent();
            _timer = new Timer();
            _timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["StopStartInterval"]);
            _timer.Tick += _timer_Tick;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    if (cbRestart.Checked)
                    {
                        Log("Start");

                        #region Kill processes
                        var ser = Process.GetProcessesByName("Search_Engine_Ranker").FirstOrDefault();
                        if (ser != null)
                        {
                            try
                            {
                                ser.Kill();
                                Log("Ser process killed");
                            }
                            catch (Exception ex)
                            {
                                if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                {
                                    Log("Continue " + ex.Message);
                                }
                            }
                        }
                        var cb = Process.GetProcessesByName("GSA_CapBreak").FirstOrDefault();
                        if (cb != null)
                        {
                            try
                            {
                                cb.Kill();
                                Log("CB killed");
                            }
                            catch (Exception ex)
                            {
                                if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                {
                                    Log("Continue " + ex.Message);
                                }
                            }
                        }
                        #endregion

                        if (cbCBDownload.Checked)
                        {
                            //remove app data
                            var cbAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GSA Captcha Breaker");
                            if (Directory.Exists(cbAppDataPath))
                            {
                                try
                                {
                                    Directory.Delete(cbAppDataPath, true);
                                    Log("CB app data files deleted");
                                }
                                catch (Exception ex)
                                {
                                    if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                    {
                                        Log("Continue " + ex.Message);
                                    }
                                }
                            }
                            Log("CB App data deleted");

                            //remove program files
                            var cbProgramfilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "GSA Captcha Breaker");
                            if (Directory.Exists(cbProgramfilesPath))
                            {
                                try
                                {
                                    Directory.Delete(cbProgramfilesPath, true);
                                    Log("CB program files deleted");
                                }
                                catch (Exception ex)
                                {
                                    if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                    {
                                        Log("Continue " + ex.Message);
                                    }
                                }
                            }
                            Log("CB program files deleted");

                            //download
                            Log("CB Start downloading");
                            string cBFile = @"C:\GSA\captcha_breaker.exe";
                            if (File.Exists(cBFile))
                                File.Delete(cBFile);
                            CBDownloadCompleted = false;
                            WebClient webClientCb = new WebClient();
                            webClientCb.DownloadProgressChanged += WebClientSb_DownloadProgressChanged;
                            webClientCb.DownloadFileCompleted += WebClientCb_DownloadFileCompleted;
                            webClientCb.DownloadFileAsync(new Uri("https://www.gsa-online.de/download/captcha_breaker.exe"), cBFile);
                            while (!CBDownloadCompleted)
                            {
                                Thread.Sleep(1000);
                            }
                            Log("CB finish downloading");

                            //install
                            Log("CB starting");
                            ProcessStartInfo startInfo = new ProcessStartInfo()
                            {
                                FileName = cBFile
                            };
                            Process.Start(startInfo);
                            Log("CB started");
                        }
                        else
                        {
                            Log("CB starting");
                            Process.Start(@"C:\Program Files (x86)\GSA Captcha Breaker\GSA_CapBreak.exe");
                            Log("Cb started");
                        }

                        if (cbSERDownload.Checked)
                        {
                            //remove app data
                            var serAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GSA Search Engine Ranker");
                            if (Directory.Exists(serAppDataPath))
                            {
                                try
                                {
                                    Directory.Delete(serAppDataPath, true);
                                    Log("Ser app data files deleted");
                                }
                                catch (Exception ex)
                                {
                                    if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                    {
                                        Log("Continue " + ex.Message);
                                    }
                                }
                            }
                            Log("Ser app data deleted");

                            //remove program files
                            var serProgramfilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "GSA Search Engine Ranker");
                            if (Directory.Exists(serProgramfilesPath))
                            {
                                try
                                {
                                    Directory.Delete(serProgramfilesPath, true);
                                    Log("Ser program files deleted");
                                }
                                catch (Exception ex)
                                {
                                    if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                    {
                                        Log("Continue " + ex.Message);
                                    }
                                }
                            }
                            Log("Ser program files deleted");

                            //download
                            string serZipFile = @"C:\GSA\ser_setup.zip";
                            string serExtractedFile = @"C:\GSA\ser_setup.exe";

                            if (File.Exists(serZipFile))
                                File.Delete(serZipFile);
                            if (File.Exists(serExtractedFile))
                                File.Delete(serExtractedFile);

                            Log("Ser start downloading");
                            SerDownloadCompleted = false;
                            WebClient webClientSer = new WebClient();
                            webClientSer.DownloadProgressChanged += WebClientSer_DownloadProgressChanged;
                            webClientSer.DownloadFileCompleted += WebClientSer_DownloadFileCompleted;
                            webClientSer.DownloadFileAsync(new Uri("https://www.gsa-online.de/download/ser_setup.zip"), serZipFile);
                            while (!SerDownloadCompleted)
                            {
                                Thread.Sleep(1000);
                            }
                            Log("Ser downloaded");

                            //install
                            System.IO.Compression.ZipFile.ExtractToDirectory(@"C:\GSA\ser_setup.zip", @"C:\GSA");
                            string zipFile = @"C:\GSA\ser_setup.zip";
                            if (File.Exists(zipFile))
                            {
                                try
                                {
                                    File.Delete(zipFile);
                                }
                                catch (Exception ex)
                                {
                                    if (MessageBox.Show(ex.Message) == DialogResult.OK)
                                    {
                                        Log("Continue " + ex.Message);
                                    }
                                }
                            }
                            Log("Ser unzipped");

                            ProcessStartInfo startInfo = new ProcessStartInfo()
                            {
                                FileName = "C:\\GSA\\ser_setup.exe"
                            };
                            Process.Start(startInfo);
                            Log("Ser finished");
                        }
                        else
                        {
                            Log("Ser starting");
                            Process.Start(@"C:\Program Files (x86)\GSA Search Engine Ranker\Search_Engine_Ranker.exe");
                            Log("Ser started");
                        }
                    }
                    else
                    {
                        Log("Nothing");
                    }
                    Log("Finish");
                }
                catch (Exception ex)
                {
                    Log("main", ex);
                }
            });
        }

        private void WebClientCb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            CBDownloadCompleted = true;
        }
        private void WebClientSb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblCbStatus.Invoke((MethodInvoker)delegate
            {
                lblCbStatus.Text = e.ProgressPercentage + "%";
            });
        }
        private void WebClientSer_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SerDownloadCompleted = true;
        }
        private void WebClientSer_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblSerStatus.Invoke((MethodInvoker)delegate
            {
                lblSerStatus.Text = e.ProgressPercentage + "%";
            });
        }

        private void Log(string message, Exception ex = null)
        {
            if (ex != null)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                message += " | " + ex.Message;
            }
            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke((MethodInvoker)delegate
                {
                    lbLog.Items.Insert(0, DateTime.Now.ToString() + "\t" + message);
                });
            }
            else
            {
                lbLog.Items.Insert(0, DateTime.Now.ToString() + "\t" + message);
            }
        }
        public Point Cursor_StopStart { get; set; }
        public Point Cursor_Licence { get; set; }
        public Timer _timer { get; set; }
        private void btnSetMouse_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("put the mouse cursor on okey licence button and then press ok") == DialogResult.OK)
            {
                Cursor_Licence = CursorPosition.GetCursorPosition();
                Log("okey licence position is X:" + Cursor_Licence.X + " and Y:" + Cursor_Licence.Y);
            }
            if (MessageBox.Show("put the mouse cursor on stop and start button and then press ok") == DialogResult.OK)
            {
                Cursor_StopStart = CursorPosition.GetCursorPosition();
                Log("Start and stop position is X:" + Cursor_StopStart.X + " and Y:" + Cursor_StopStart.Y);
            }

        }
        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                Log("Auto click started");
                btnStart.Text = "Stop";
                _timer.Start();
            }
            else
            {
                Log("Auto click stoped");
                btnStart.Text = "Start";
                _timer.Stop();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            ClickOnPointTool.ClickOnPoint(IntPtr.Zero, Cursor_Licence);
            Thread.Sleep(5 * 1000);
            ClickOnPointTool.ClickOnPoint(IntPtr.Zero, Cursor_StopStart);
            Thread.Sleep(5 * 1000);
            ClickOnPointTool.ClickOnPoint(IntPtr.Zero, Cursor_StopStart);
        }
    }
}
