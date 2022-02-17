using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Ardin.GSAManager
{
    public partial class frmMain : Form
    {
        public bool SerDownloadCompleted { get; set; }
        public bool CBDownloadCompleted { get; set; }
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Log("Start");
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
                        if(MessageBox.Show(ex.Message) == DialogResult.OK)
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
                        Log("Cb killed");
                    }
                    catch (Exception ex)
                    {
                        if (MessageBox.Show(ex.Message) == DialogResult.OK)
                        {
                            Log("Continue " + ex.Message);
                        }
                    }
                }
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

                if (!cbCBDownload.Checked)
                {
                    Log("Try to run Cb");
                    Process.Start(@"C:\Program Files (x86)\GSA Captcha Breaker\GSA_CapBreak.exe");
                    Log("Cb started");
                }
                else
                {
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
                }

                Download();
                while (!SerDownloadCompleted || !CBDownloadCompleted)
                {
                    Thread.Sleep(1000);
                }

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
                Log("New version of Ser " + (cbCBDownload.Checked ? "and CB " : "") + "downloaded and extracted");
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "C:\\GSA\\ser_setup.exe"
                };
                Process.Start(startInfo);
                if (cbCBDownload.Checked)
                {
                    startInfo = new ProcessStartInfo()
                    {
                        FileName = "C:\\GSA\\captcha_breaker.exe"
                    };
                    Process.Start(startInfo);
                }
                if (MessageBox.Show("Compelete setup and then press okey", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Process.Start(@"C:\Program Files (x86)\GSA Search Engine Ranker\Search_Engine_Ranker.exe");
                    if (cbCBDownload.Checked)
                        Process.Start(@"C:\Program Files (x86)\GSA Captcha Breaker\GSA_CapBreak.exe");
                }
                Log("Finish");
            });
        }

        private void Download()
        {
            try
            {
                string serZipFile = @"C:\GSA\ser_setup.zip";
                string cBExtractedFile = @"C:\GSA\captcha_breaker.exe";
                string serExtractedFile = @"C:\GSA\ser_setup.exe";

                if (File.Exists(serZipFile))
                    File.Delete(serZipFile);
                if (File.Exists(serExtractedFile))
                    File.Delete(serExtractedFile);
                if (cbCBDownload.Checked)
                {
                    if (File.Exists(cBExtractedFile))
                        File.Delete(cBExtractedFile);
                }
                Log("Start downloading new version of Ser " + (cbCBDownload.Checked ? " and Cb" : ""));
                WebClient webClientSer = new WebClient();
                webClientSer.DownloadProgressChanged += WebClientSer_DownloadProgressChanged;
                webClientSer.DownloadFileCompleted += WebClientSer_DownloadFileCompleted;
                webClientSer.DownloadFileAsync(new Uri("https://www.gsa-online.de/download/ser_setup.zip"), serZipFile);

                if (cbCBDownload.Checked)
                {
                    WebClient webClientCb = new WebClient();
                    webClientCb.DownloadProgressChanged += WebClientSb_DownloadProgressChanged;
                    webClientCb.DownloadFileCompleted += WebClientCb_DownloadFileCompleted;
                    webClientCb.DownloadFileAsync(new Uri("https://www.gsa-online.de/download/captcha_breaker.exe"), cBExtractedFile);
                }
                else CBDownloadCompleted = true;
            }
            catch (Exception ex)
            {
                Log("Download ser failed", ex);
            }
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
        private void button1_Click(object sender, EventArgs e)
        {
            var hanle = (IntPtr)0x002C0752;
            var pos = Win32.GetPosition(hanle);
            Win32.Click(hanle, pos.Left + 50, pos.Top + 385);
        }
    }
}
