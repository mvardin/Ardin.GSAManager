
namespace Ardin.GSAManager
{
    partial class frmMain
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
            this.btnStart = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.aaa = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblSerStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCbStatus = new System.Windows.Forms.Label();
            this.cbCBDownload = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 15;
            this.lbLog.Location = new System.Drawing.Point(12, 71);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(776, 364);
            this.lbLog.TabIndex = 1;
            // 
            // aaa
            // 
            this.aaa.AutoSize = true;
            this.aaa.Location = new System.Drawing.Point(131, 16);
            this.aaa.Name = "aaa";
            this.aaa.Size = new System.Drawing.Size(80, 15);
            this.aaa.TabIndex = 2;
            this.aaa.Text = "SerDownload:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(713, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblSerStatus
            // 
            this.lblSerStatus.AutoSize = true;
            this.lblSerStatus.Location = new System.Drawing.Point(217, 16);
            this.lblSerStatus.Name = "lblSerStatus";
            this.lblSerStatus.Size = new System.Drawing.Size(13, 15);
            this.lblSerStatus.TabIndex = 2;
            this.lblSerStatus.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(288, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "CBDownload:";
            // 
            // lblCbStatus
            // 
            this.lblCbStatus.AutoSize = true;
            this.lblCbStatus.Location = new System.Drawing.Point(374, 16);
            this.lblCbStatus.Name = "lblCbStatus";
            this.lblCbStatus.Size = new System.Drawing.Size(13, 15);
            this.lblCbStatus.TabIndex = 2;
            this.lblCbStatus.Text = "0";
            // 
            // cbCBDownload
            // 
            this.cbCBDownload.AutoSize = true;
            this.cbCBDownload.Location = new System.Drawing.Point(12, 46);
            this.cbCBDownload.Name = "cbCBDownload";
            this.cbCBDownload.Size = new System.Drawing.Size(98, 19);
            this.cbCBDownload.TabIndex = 4;
            this.cbCBDownload.Text = "Download CB";
            this.cbCBDownload.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbCBDownload);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblCbStatus);
            this.Controls.Add(this.lblSerStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.aaa);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMain";
            this.Text = "GSA Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Label aaa;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblSerStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCbStatus;
        private System.Windows.Forms.CheckBox cbCBDownload;
    }
}

