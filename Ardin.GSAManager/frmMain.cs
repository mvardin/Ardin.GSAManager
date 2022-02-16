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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ardin.GSAManager
{
    public partial class frmMain : Form
    {
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
                    ser.Kill();
                    Log("Ser process killed");
                }
                var cb = Process.GetProcessesByName("GSA_CapBreak").FirstOrDefault();
                if (cb != null)
                {
                    cb.Kill();
                    Log("Cb killed");
                }
                Log("Try to run Cb");
                Process.Start(@"C:\Program Files (x86)\GSA Captcha Breaker\GSA_CapBreak.exe");
                Log("Cb started");
                var serAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GSA Search Engine Ranker");
                if (Directory.Exists(serAppDataPath))
                {
                    Directory.Delete(serAppDataPath, true);
                    Log("Ser app data files deleted");
                }
                var serProgramfilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "GSA Search Engine Ranker");
                if (Directory.Exists(serProgramfilesPath))
                {
                    Directory.Delete(serProgramfilesPath, true);
                    Log("Ser program files deleted");
                }

                Download("ser");
            });
        }

        private void Download(string app)
        {
            try
            {
                string zipFile = @"C:\GSA\ser_setup.zip";
                if (File.Exists(zipFile))
                    File.Delete(zipFile);
                string extractedFile = @"C:\GSA\ser_setup.exe";
                if (File.Exists(extractedFile))
                    File.Delete(extractedFile);
                Log("Start downloading new version of Ser");
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                webClient.DownloadFileAsync(new Uri("https://www.gsa-online.de/download/ser_setup.zip"), @"C:\GSA\ser_setup.zip");
            }
            catch (Exception ex)
            {
                Log("Download ser failed", ex);
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(@"C:\GSA\ser_setup.zip", @"C:\GSA");
            string zipFile = @"C:\GSA\ser_setup.zip";
            if (File.Exists(zipFile))
                File.Delete(zipFile);
            Log("New version of Ser downloaded and extracted");
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "C:\\GSA\\ser_setup.exe"
            };
            Process.Start(startInfo);
            if (MessageBox.Show("Compelete setup and then press okey", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Log("Setup compeleted");
                Log("Run ser");
                Process.Start(@"C:\Program Files (x86)\GSA Search Engine Ranker\Search_Engine_Ranker.exe");

                Log("Ser started , try to restore config files");
            }
            Log("Finish");
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblStatus.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = e.ProgressPercentage + "%";
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
