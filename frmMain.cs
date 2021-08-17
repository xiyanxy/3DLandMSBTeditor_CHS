using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MsbtEditor.Properties;
using Be.Windows.Forms;
using System.Text.RegularExpressions;

namespace MsbtEditor
{
	public partial class frmMain : Form
	{
		private MSBT _msbt = null;
		private bool _fileOpen = false;
		private bool _hasChanges = false;

		public frmMain(string[] args)
		{
			InitializeComponent();
			this.Icon = Resources.msbteditor;

			if (args.Length > 0 && File.Exists(args[0]))
				OpenFile(args[0]);
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			slbAddress.Text = string.Empty;
			slbActions.Text = string.Empty;
			slbStringCount.Text = string.Empty;
			UpdateForm();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dr = DialogResult.No;

			if (_fileOpen && _hasChanges)
				dr = MessageBox.Show("你有未保存的更改在 " + FileName() + "中. 退出前保存更改?", "未保存的更改", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

			switch (dr)
			{
				case DialogResult.Yes:
					SaveFile();
					break;
				case DialogResult.Cancel:
					e.Cancel = true;
					break;
			}

			Settings.Default.Save();
		}

		private void frmMain_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Copy;
		}

		private void frmMain_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (files.Length > 0 && File.Exists(files[0]))
				ConfirmOpenFile(files[0]);
		}

		#region Menu and Toolbar

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ConfirmOpenFile();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFile();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFile(true);
		}

		private void gBATempToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://gbatemp.net/threads/release-msbt-editor-reloaded.406208");
		}
		private void xiyanxy_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/xiyanxy/3DLandMSBTeditor_CHS");
		}

		private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/IcySon55/3DLandMSBTeditor");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show(Settings.Default.ApplicationName + "\r\n作者: IcySon55  汉化: 曦颜XY。", "关于 " + Settings.Default.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		private void ConfirmOpenFile(string filename = "")
		{
			DialogResult dr = DialogResult.No;

			if (_fileOpen && _hasChanges)
				dr = MessageBox.Show("你有未保存的更改在 " + FileName() + "中. 退出前保存更改?", "未保存的更改", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

			switch (dr)
			{
				case DialogResult.Yes:
					dr = SaveFile();
					if (dr == DialogResult.OK) OpenFile(filename);
					break;
				case DialogResult.No:
					OpenFile(filename);
					break;
			}
		}

		private void OpenFile(string filename = "")
		{
			ofdOpenFile.InitialDirectory = Settings.Default.InitialDirectory;
			DialogResult dr = DialogResult.OK;

			if (filename != string.Empty)
			{
				_msbt = new MSBT(filename);
				_fileOpen = true;
				_hasChanges = false;
				LoadFile();
				UpdateForm();
				Settings.Default.InitialDirectory = new FileInfo(filename).DirectoryName;
				Settings.Default.Save();
				Settings.Default.Reload();
			}
			else
			{
				dr = ofdOpenFile.ShowDialog();
				if (dr == DialogResult.OK)
				{
					try
					{
						_msbt = new MSBT(ofdOpenFile.FileName);
						_fileOpen = true;
						_hasChanges = false;
						LoadFile();
						Settings.Default.InitialDirectory = new FileInfo(ofdOpenFile.FileName).DirectoryName;
						Settings.Default.Save();
						Settings.Default.Reload();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK);
						_fileOpen = false;
						_hasChanges = false;
					}
					UpdateForm();
				}
			}
		}

		private void LoadFile()
		{
			lstStrings.Items.Clear();

			for (int i = 0; i < _msbt.TXT2.NumberOfStrings; i++)
			{
				if (_msbt.HasLabels)
				{
					lstStrings.Sorted = true;
					lstStrings.Items.Add(_msbt.LBL1.Labels[i]);
				}
				else
				{
					lstStrings.Sorted = false;
					lstStrings.Items.Add(_msbt.TXT2.Strings[i]);
				}
			}

			if (lstStrings.Items.Count > 0)
				lstStrings.SelectedIndex = 0;

			slbActions.Text = "打开成功 " + FileName();
			slbStringCount.Text = "此文件中的字符串: " + _msbt.TXT2.NumberOfStrings;
		}

		private DialogResult SaveFile(bool saveAs = false)
		{
			DialogResult dr = DialogResult.OK;

			if (_msbt.File == null || saveAs)
			{
				sfdSaveEntity.InitialDirectory = Settings.Default.InitialDirectory;
				dr = sfdSaveEntity.ShowDialog();
			}

			if ((_msbt.File == null || saveAs) && dr == DialogResult.OK)
			{
				_msbt.File = new FileInfo(sfdSaveEntity.FileName);
				Settings.Default.InitialDirectory = new FileInfo(sfdSaveEntity.FileName).DirectoryName;
				Settings.Default.Save();
				Settings.Default.Reload();
			}

			if (dr == DialogResult.OK)
			{
				_msbt.Save();
				_hasChanges = false;
				UpdateTextView();
				UpdateOriginalText();
				UpdateHexView();
				UpdateForm();
			}

			slbActions.Text = "保存成功 " + FileName();

			return dr;
		}

		private void lstStrings_SelectedIndexChanged(object sender, EventArgs e)
		{
			IEntry ent = (IEntry)lstStrings.SelectedItem;

			txtLabelName.Text = ent.ToString();

			UpdateTextView();
			UpdateOriginalText();
			UpdateHexView();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Shift))
			{
				if (keyData == (Keys.Tab | Keys.Shift))
					lstStrings.SelectedIndex -= (lstStrings.SelectedIndex - 1 >= 0 ? 1 : (lstStrings.Items.Count - 1) * -1);
				else
					lstStrings.SelectedIndex += (lstStrings.SelectedIndex + 1 < lstStrings.Items.Count ? 1 : (lstStrings.Items.Count - 1) * -1);

				return true;
			}
			else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		private void txtSelectAll_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox txtBox = (TextBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				txtBox.SelectAll();
				e.SuppressKeyPress = true;
			}
		}

		private void hbxSelectAll_KeyDown(object sender, KeyEventArgs e)
		{
			HexBox hbxBox = (HexBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				hbxBox.SelectAll();
				e.SuppressKeyPress = true;
			}
		}

		private void txtEdit_KeyUp(object sender, KeyEventArgs e)
		{
			string result = txtEdit.Text;

			IEntry ent = (IEntry)lstStrings.SelectedItem;
			ent.Value = _msbt.FileEncoding.GetBytes(result.Replace("\r\n", "\n").Replace(@"\0", "\0") + "\0");

			if (txtEdit.Text != txtOriginal.Text)
				_hasChanges = true;

			UpdateHexView();
			UpdateForm();
		}

		protected void hbxEdit_Changed(object sender, EventArgs e)
		{
			DynamicFileByteProvider dfbp = (DynamicFileByteProvider)sender;

			IEntry ent = (IEntry)lstStrings.SelectedItem;
			List<byte> bytes = new List<byte>();
			for (int i = 0; i < (int)dfbp.Length; i++)
				bytes.Add(dfbp.ReadByte(i));
			ent.Value = bytes.ToArray();

			UpdateTextView();
			UpdateForm();

			if (txtEdit.Text != txtOriginal.Text)
				_hasChanges = true;
		}

		private void UpdateTextView()
		{
			IEntry ent = (IEntry)lstStrings.SelectedItem;

			txtEdit.Text = _msbt.FileEncoding.GetString(ent.Value).Replace("\n", "\r\n").TrimEnd('\0').Replace("\0", @"\0") + "\0";

			slbAddress.Text = "字符串: " + (ent.Index + 1);
		}

		private void UpdateOriginalText()
		{
			IEntry ent = (IEntry)lstStrings.SelectedItem;

			txtOriginal.Text = _msbt.FileEncoding.GetString(_msbt.TXT2.OriginalStrings[(int)ent.Index].Value).Replace("\n", "\r\n").TrimEnd('\0').Replace("\0", @"\0") + "\0";
		}

		private void UpdateHexView()
		{
			DynamicFileByteProvider dfbp = null;

			try
			{
				IEntry ent = (IEntry)lstStrings.SelectedItem;
				MemoryStream strm = new MemoryStream(ent.Value);

				dfbp = new DynamicFileByteProvider(strm);
				dfbp.Changed += new EventHandler(hbxEdit_Changed);
			}
			catch (Exception)
			{ }

			hbxHexView.ByteProvider = dfbp;
		}

		// Utilities
		private void UpdateForm()
		{
			this.Text = Settings.Default.ApplicationName + (FileName() != string.Empty ? " - " + FileName() : string.Empty) + (_hasChanges ? "*" : string.Empty);

			saveToolStripMenuItem.Enabled = _fileOpen;
			saveAsToolStripMenuItem.Enabled = _fileOpen;
			findToolStripMenuItem.Enabled = _fileOpen;
			exportCSVToolStripMenuItem.Enabled = _fileOpen;
			exportXMSBTToolStripMenuItem.Enabled = _fileOpen;
			importXMSBTToolStripMenuItem.Enabled = _fileOpen;
			exportXMSBTModToolStripMenuItem.Enabled = _fileOpen;

			lstStrings.Enabled = _fileOpen;
			txtLabelName.Enabled = _fileOpen;
			btnSaveLabel.Enabled = _fileOpen && _msbt.HasLabels;
			btnAddLabel.Enabled = _fileOpen && _msbt.HasLabels;
			btnDeleteLabel.Enabled = _fileOpen && _msbt.HasLabels;
			txtEdit.Enabled = _fileOpen;
			txtOriginal.Enabled = _fileOpen;
			hbxHexView.Enabled = _fileOpen;
		}

		private string FileName()
		{
			return _msbt == null || _msbt.File == null ? string.Empty : _msbt.File.Name;
		}

		// Tools
		private void exportCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "导出到 CSV..。";
			sfd.Filter = "逗号分隔 (*.csv)|*.csv";
			sfd.InitialDirectory = Settings.Default.InitialDirectory;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				string result = _msbt.ExportToCSV(sfd.FileName);

				MessageBox.Show(result, "CSV导出结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void BG4ExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "选择BG4文件..。";
			ofd.Filter = "BG4数据文件 (*.dat)|*.dat|全部文件 (*.*)|*.*";
			ofd.InitialDirectory = Settings.Default.BG4OpenDirectory;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.BG4OpenDirectory = new FileInfo(ofd.FileName).DirectoryName;

				if (File.Exists(ofd.FileName))
				{
					FolderBrowserDialog fbd = new FolderBrowserDialog();
					fbd.Description = "选择要将文件导出到的目标目录。";
					fbd.SelectedPath = Settings.Default.BG4ExtractDirectory;

					if (fbd.ShowDialog() == DialogResult.OK)
					{
						Settings.Default.BG4ExtractDirectory = fbd.SelectedPath;

						if (Directory.Exists(fbd.SelectedPath))
						{
							bool overwrite = true;

							if (new DirectoryInfo(fbd.SelectedPath).GetFiles().Length > 0)
								overwrite = MessageBox.Show("是否可以覆盖目标目录中的文件?", "覆盖吗?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes ? true : false;

							string result = MsbtEditor.BG4.BG4.Extract(ofd.FileName, fbd.SelectedPath, overwrite);

							MessageBox.Show(result, "BG4导出结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				}
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void compressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			try
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					YATA.dsdecmp.Compress(ofd.FileName, ofd.FileName + ".lz");
					MessageBox.Show("完成", "LZ11 压缩");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "LZ11 压缩");
			}
		}

		private void decompressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			try
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					YATA.dsdecmp.Decompress(ofd.FileName, ofd.FileName + ".bin");
					MessageBox.Show("Done", "LZ11 解压");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "LZ11 解压");
			}
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmSearch search = new frmSearch();
			search.msbt = _msbt;
			search.StartPosition = FormStartPosition.CenterParent;
			search.ShowDialog();

			if (search.Return != null)
			{
				lstStrings.SelectedItem = search.Return;
			}
		}

		private void searchDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmSearchDirectory search = new frmSearchDirectory();
			search.StartPosition = FormStartPosition.CenterParent;
			search.ShowDialog();

			if (search.Return != null)
			{
				OpenFile(search.Return.Filename);

				if (lstStrings.Items.Count >= search.Return.Index)
					lstStrings.SelectedIndex = search.Return.Index;
			}
		}

		private void exportXMSBTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = _msbt.File.Name.Substring(0, _msbt.File.Name.Length - 4) + "xmsbt";
			sfd.Title = "XMSBT另存为...";
			sfd.Filter = "XMSBT文件 (*.xmsbt)|*.xmsbt";
			sfd.InitialDirectory = Settings.Default.XMSBTDirectory;
			sfd.AddExtension = true;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.XMSBTDirectory = new FileInfo(sfd.FileName).DirectoryName;

				string result = _msbt.ExportXMSBT(sfd.FileName);

				MessageBox.Show(result, "XMSBT导出结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void importXMSBTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "选择XMSBT文件...";
			ofd.Filter = "XMSBT文件 (*.xmsbt)|*.xmsbt";
			ofd.InitialDirectory = Settings.Default.XMSBTDirectory;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.XMSBTDirectory = new FileInfo(ofd.FileName).DirectoryName;

				if (File.Exists(ofd.FileName))
				{
					bool addLabels = MessageBox.Show("在XMSBT中添加MSBT文件中不存在的标签?", "添加吗?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

					string result = _msbt.ImportXMSBT(ofd.FileName, addLabels);

					MessageBox.Show(result, "XMSBT 导入结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			LoadFile();

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void batchExportXMSBTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "选择包含MSBT文件的目录:";
			fbd.ShowNewFolderButton = false;
			fbd.SelectedPath = Settings.Default.XMSBTDirectory;

			if (fbd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.XMSBTDirectory = fbd.SelectedPath;

				if (Directory.Exists(fbd.SelectedPath))
				{
					DirectoryInfo dir = new DirectoryInfo(fbd.SelectedPath);
					FileInfo[] files = dir.GetFiles("*.msbt");
					bool overwrite = true;
					string result = "";

					if (dir.GetFiles("*.xmsbt").Length > 0)
						overwrite = MessageBox.Show("是否可以覆盖目录中的XMSBT文件?", "覆盖吗?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

					if (files.Length > 0)
					{
						foreach (FileInfo file in files)
						{
							try
							{
								MSBT msbt = new MSBT(file.FullName);
								msbt.ExportXMSBT(file.FullName.Substring(0, file.FullName.Length - 4) + "xmsbt", overwrite);
							}
							catch (Exception ex)
							{
								result = ex.Message;
							}
						}

						if (result.Length == 0)
							result = "已成功批量将文件导出到XMSBT。";
					}
					else
						result = "所选目录中没有要导出的MSBT文件。";

					MessageBox.Show(result, "XMSBT批量导出结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void batchImportXMSBTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "选择包含同名MSBT和XMSBT文件的目录:";
			fbd.ShowNewFolderButton = false;
			fbd.SelectedPath = Settings.Default.XMSBTDirectory;

			if (fbd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.XMSBTDirectory = fbd.SelectedPath;

				if (Directory.Exists(fbd.SelectedPath))
				{
					DirectoryInfo dir = new DirectoryInfo(fbd.SelectedPath);
					FileInfo[] msbtFiles = dir.GetFiles("*.msbt");
					FileInfo[] xmsbtFiles = dir.GetFiles("*.xmsbt");
					string result = "";

					bool addLabels = MessageBox.Show("在XMSBT文件中添加MSBT文件中不存在的标签?", "添加吗?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

					Dictionary<string, FileInfo> matches = new Dictionary<string, FileInfo>();

					foreach (FileInfo file in msbtFiles)
					{
						string name = file.FullName.Substring(0, file.FullName.Length - 4);

						foreach (FileInfo xFile in xmsbtFiles)
						{
							if (name == xFile.FullName.Substring(0, xFile.FullName.Length - 5))
							{
								matches.Add(name, file);
								break;
							}
						}
					}

					if (matches.Count > 0)
					{
						foreach (string file in matches.Keys)
						{
							try
							{
								MSBT msbt = new MSBT(matches[file].FullName);
								msbt.ImportXMSBT(matches[file].FullName.Substring(0, matches[file].FullName.Length - 4) + "xmsbt", addLabels);
								msbt.Save();
							}
							catch (Exception ex)
							{
								result = ex.Message;
							}
						}

						if (result.Length == 0)
							result = "已成功从XMSBT批量导入。";
					}
					else
						result = "所选目录中没有与XMSBT文件匹配的MSBT文件。";

					MessageBox.Show(result, "XMSBT批量导入结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void exportXMSBTModToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "选择没有更改的源MSBT文件...";
			ofd.Filter = "MSBT文件 (*.msbt)|*.msbt";
			ofd.InitialDirectory = Settings.Default.InitialDirectory;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.FileName = _msbt.File.Name.Substring(0, _msbt.File.Name.Length - 4) + "xmsbt";
				sfd.Title = "XMSBT另存为..。";
				sfd.Filter = "XMSBT文件 (*.xmsbt)|*.xmsbt";
				sfd.InitialDirectory = Settings.Default.XMSBTDirectory;
				sfd.AddExtension = true;

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					Settings.Default.XMSBTDirectory = new FileInfo(sfd.FileName).DirectoryName;

					string result = _msbt.ExportXMSBTMod(sfd.FileName, ofd.FileName);

					MessageBox.Show(result, "XMSBT Mod导出结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void extractUMSBTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "选择UMSBT文件..。";
			ofd.Filter = "UMSBT数据文件 (*.umsbt)|*.umsbt";
			ofd.InitialDirectory = Settings.Default.BG4OpenDirectory;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.BG4OpenDirectory = new FileInfo(ofd.FileName).DirectoryName;

				if (File.Exists(ofd.FileName))
				{
					FolderBrowserDialog fbd = new FolderBrowserDialog();
					fbd.Description = "选择要将文件解压缩到的目标目录:";
					fbd.SelectedPath = Settings.Default.BG4ExtractDirectory;

					if (fbd.ShowDialog() == DialogResult.OK)
					{
						Settings.Default.BG4ExtractDirectory = fbd.SelectedPath;

						if (Directory.Exists(fbd.SelectedPath))
						{
							bool overwrite = true;

							if (new DirectoryInfo(fbd.SelectedPath).GetFiles("*.msbt").Length > 0)
								overwrite = MessageBox.Show("是否可以覆盖目标目录中的MSBT文件?", "覆盖吗?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

							string result = MsbtEditor.UMSBT.UMSBT.Extract(ofd.FileName, fbd.SelectedPath, overwrite);

							MessageBox.Show(result, "UMSBT解包结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				}
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void packUMSBTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "选择包含MSBT文件的源目录:";
			fbd.SelectedPath = Settings.Default.BG4ExtractDirectory;

			if (fbd.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.BG4ExtractDirectory = fbd.SelectedPath;

				if (Directory.Exists(fbd.SelectedPath))
				{
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.Title = "UMSBT数据文件另存为..。";
					sfd.Filter = "UMSBT数据文件 (*.umsbt)|*.umsbt";
					sfd.InitialDirectory = Settings.Default.BG4OpenDirectory;
					sfd.AddExtension = true;

					if (sfd.ShowDialog() == DialogResult.OK)
					{
						Settings.Default.BG4OpenDirectory = new FileInfo(sfd.FileName).DirectoryName;

						string result = MsbtEditor.UMSBT.UMSBT.Pack(sfd.FileName, fbd.SelectedPath);

						MessageBox.Show(result, "UMSBT打包结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
			}

			Settings.Default.Save();
			Settings.Default.Reload();
		}

		private void btnSaveLabel_Click(object sender, EventArgs e)
		{
			if (txtLabelName.Text.Trim().Length <= MSBT.LabelMaxLength && Regex.IsMatch(txtLabelName.Text.Trim(), MSBT.LabelFilter))
			{
				bool taken = false;

				foreach (Label lbl in _msbt.LBL1.Labels)
				{
					if (lbl.Name == txtLabelName.Text.Trim())
					{
						taken = true;
						break;
					}
				}

				if (!taken)
				{
					_msbt.RenameLabel((Label)lstStrings.SelectedItem, txtLabelName.Text.Trim());
					int selectedIndex = lstStrings.SelectedIndex;
					LoadFile();
					if (lstStrings.Items.Count > selectedIndex)
						lstStrings.SelectedIndex = selectedIndex;
				}
				else
					MessageBox.Show("输入的标签名称已存在。新标签名称必须是唯一的。", "无效的标签名", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
				MessageBox.Show("您输入的标签名称无效。只能使用字母数字值: a-z, A-Z, 0-9 and _ (下划线). 长度也限制为64个字符。", "无效的标签名", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void btnAddLabel_Click(object sender, EventArgs e)
		{
			if (txtLabelName.Text.Trim().Length <= MSBT.LabelMaxLength && Regex.IsMatch(txtLabelName.Text.Trim(), MSBT.LabelFilter))
			{
				bool taken = false;

				foreach (Label lbl in _msbt.LBL1.Labels)
				{
					if (lbl.Name == txtLabelName.Text.Trim())
					{
						taken = true;
						break;
					}
				}

				if (!taken)
				{
					Label lbl = _msbt.AddLabel(txtLabelName.Text.Trim());
					LoadFile();
					if (lstStrings.Items.Contains(lbl))
						lstStrings.SelectedIndex = lstStrings.Items.IndexOf(lbl);
				}
				else
					MessageBox.Show("输入的标签名称已存在。新标签名称必须是唯一的。", "无效的标签名", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
				MessageBox.Show("您输入的标签名称无效。只能使用字母数字值: a-z, A-Z, 0-9 and _ (下划线). 长度也限制为64个字符。", "无效的标签名", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void btnDeleteLabel_Click(object sender, EventArgs e)
		{
			Label lbl = (Label)lstStrings.SelectedItem;

			DialogResult dr = MessageBox.Show("是否要删除 '" + lbl.Name + "'?", "删除标签?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dr == DialogResult.Yes)
			{
				_msbt.RemoveLabel(lbl);
				int selectedIndex = lstStrings.SelectedIndex;
				LoadFile();
				lstStrings.SelectedIndex = lstStrings.Items.Count > selectedIndex ? selectedIndex : 0;
			}
		}
	}
}