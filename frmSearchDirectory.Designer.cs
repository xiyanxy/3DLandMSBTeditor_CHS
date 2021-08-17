namespace MsbtEditor
{
	partial class frmSearchDirectory
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtFindText = new System.Windows.Forms.TextBox();
            this.btnFindText = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearchDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkSearchSubfolders = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "查找内容:";
            // 
            // txtFindText
            // 
            this.txtFindText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFindText.Location = new System.Drawing.Point(111, 59);
            this.txtFindText.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.txtFindText.Name = "txtFindText";
            this.txtFindText.Size = new System.Drawing.Size(478, 27);
            this.txtFindText.TabIndex = 2;
            // 
            // btnFindText
            // 
            this.btnFindText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindText.Location = new System.Drawing.Point(603, 56);
            this.btnFindText.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.btnFindText.Name = "btnFindText";
            this.btnFindText.Size = new System.Drawing.Size(112, 36);
            this.btnFindText.TabIndex = 3;
            this.btnFindText.Text = "查找";
            this.btnFindText.UseVisualStyleBackColor = true;
            this.btnFindText.Click += new System.EventHandler(this.btnFindText_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(603, 103);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 36);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(22, 109);
            this.chkMatchCase.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(106, 24);
            this.chkMatchCase.TabIndex = 4;
            this.chkMatchCase.Text = "区分大小写";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            this.chkMatchCase.CheckedChanged += new System.EventHandler(this.chkMatchCase_CheckedChanged);
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.IntegralHeight = false;
            this.lstResults.ItemHeight = 20;
            this.lstResults.Location = new System.Drawing.Point(19, 151);
            this.lstResults.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(685, 415);
            this.lstResults.TabIndex = 7;
            this.lstResults.DoubleClick += new System.EventHandler(this.lstResults_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "查找目录:";
            // 
            // txtSearchDirectory
            // 
            this.txtSearchDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchDirectory.BackColor = System.Drawing.SystemColors.Window;
            this.txtSearchDirectory.Location = new System.Drawing.Point(111, 16);
            this.txtSearchDirectory.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.txtSearchDirectory.Name = "txtSearchDirectory";
            this.txtSearchDirectory.ReadOnly = true;
            this.txtSearchDirectory.Size = new System.Drawing.Size(478, 27);
            this.txtSearchDirectory.TabIndex = 0;
            this.txtSearchDirectory.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(603, 12);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(112, 36);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkSearchSubfolders
            // 
            this.chkSearchSubfolders.AutoSize = true;
            this.chkSearchSubfolders.Location = new System.Drawing.Point(158, 109);
            this.chkSearchSubfolders.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.chkSearchSubfolders.Name = "chkSearchSubfolders";
            this.chkSearchSubfolders.Size = new System.Drawing.Size(121, 24);
            this.chkSearchSubfolders.TabIndex = 5;
            this.chkSearchSubfolders.Text = "搜索子文件夹";
            this.chkSearchSubfolders.UseVisualStyleBackColor = true;
            this.chkSearchSubfolders.CheckedChanged += new System.EventHandler(this.chkSearchSubfolders_CheckedChanged);
            // 
            // frmSearchDirectory
            // 
            this.AcceptButton = this.btnFindText;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(726, 588);
            this.Controls.Add(this.chkSearchSubfolders);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSearchDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFindText);
            this.Controls.Add(this.txtFindText);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSearchDirectory";
            this.Text = "目录内查找";
            this.Load += new System.EventHandler(this.frmSearchDirectory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFindText;
		private System.Windows.Forms.Button btnFindText;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkMatchCase;
		private System.Windows.Forms.ListBox lstResults;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtSearchDirectory;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.CheckBox chkSearchSubfolders;
	}
}