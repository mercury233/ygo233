namespace YGO233
{
    partial class frmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdate));
            this.labelUpdate = new System.Windows.Forms.Label();
            this.textUpdateDetails = new System.Windows.Forms.TextBox();
            this.btnStartUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressUpdate = new System.Windows.Forms.ProgressBar();
            this.btnFinish = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUpdate
            // 
            this.labelUpdate.AutoSize = true;
            this.labelUpdate.Location = new System.Drawing.Point(11, 15);
            this.labelUpdate.Name = "labelUpdate";
            this.labelUpdate.Size = new System.Drawing.Size(95, 12);
            this.labelUpdate.TabIndex = 0;
            this.labelUpdate.Text = "正在检查更新...";
            // 
            // textUpdateDetails
            // 
            this.textUpdateDetails.BackColor = System.Drawing.SystemColors.Window;
            this.textUpdateDetails.Location = new System.Drawing.Point(12, 67);
            this.textUpdateDetails.Multiline = true;
            this.textUpdateDetails.Name = "textUpdateDetails";
            this.textUpdateDetails.ReadOnly = true;
            this.textUpdateDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textUpdateDetails.Size = new System.Drawing.Size(429, 173);
            this.textUpdateDetails.TabIndex = 1;
            this.textUpdateDetails.TabStop = false;
            // 
            // btnStartUpdate
            // 
            this.btnStartUpdate.Location = new System.Drawing.Point(194, 38);
            this.btnStartUpdate.Name = "btnStartUpdate";
            this.btnStartUpdate.Size = new System.Drawing.Size(166, 23);
            this.btnStartUpdate.TabIndex = 2;
            this.btnStartUpdate.Text = "关闭 YGOPro 并下载安装";
            this.btnStartUpdate.UseVisualStyleBackColor = true;
            this.btnStartUpdate.Click += new System.EventHandler(this.btnStartUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(366, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressUpdate
            // 
            this.progressUpdate.Location = new System.Drawing.Point(12, 37);
            this.progressUpdate.Name = "progressUpdate";
            this.progressUpdate.Size = new System.Drawing.Size(429, 24);
            this.progressUpdate.TabIndex = 4;
            this.progressUpdate.Visible = false;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(366, 37);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Text = "完成";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // frmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 252);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStartUpdate);
            this.Controls.Add(this.textUpdateDetails);
            this.Controls.Add(this.labelUpdate);
            this.Controls.Add(this.progressUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YGOPro 更新";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUpdate;
        private System.Windows.Forms.TextBox textUpdateDetails;
        private System.Windows.Forms.Button btnStartUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressUpdate;
        private System.Windows.Forms.Button btnFinish;
    }
}