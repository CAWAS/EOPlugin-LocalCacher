namespace LocalCacher
{
	partial class SettingsForm
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxCacheFolder = new System.Windows.Forms.TextBox();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.checkBoxEnabled, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBoxCacheFolder, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonBrowse, 2, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 150);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// checkBoxEnabled
			// 
			this.checkBoxEnabled.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.checkBoxEnabled, 3);
			this.checkBoxEnabled.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkBoxEnabled.Location = new System.Drawing.Point(3, 3);
			this.checkBoxEnabled.Name = "checkBoxEnabled";
			this.checkBoxEnabled.Size = new System.Drawing.Size(144, 17);
			this.checkBoxEnabled.TabIndex = 0;
			this.checkBoxEnabled.Text = "Enable LocalCacher";
			this.checkBoxEnabled.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 29);
			this.label1.TabIndex = 1;
			this.label1.Text = "Cache Location";
			// 
			// textBoxCacheFolder
			// 
			this.textBoxCacheFolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxCacheFolder.Location = new System.Drawing.Point(91, 26);
			this.textBoxCacheFolder.Name = "textBoxCacheFolder";
			this.textBoxCacheFolder.Size = new System.Drawing.Size(1, 20);
			this.textBoxCacheFolder.TabIndex = 2;
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonBrowse.Location = new System.Drawing.Point(72, 26);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowse.TabIndex = 3;
			this.buttonBrowse.Text = "Browse";
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "SettingsForm";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox checkBoxEnabled;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxCacheFolder;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.FolderBrowserDialog folderBrowser;
	}
}
