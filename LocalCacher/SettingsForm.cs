using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Plugins;

namespace LocalCacher
{
	public partial class SettingsForm : PluginSettingControl
	{
		private LocalCacher plugin;
		public SettingsForm(LocalCacher plugin)
		{
			this.plugin = plugin;
			InitializeComponent();
		}

		public override bool Save()
		{
			if (checkBoxEnabled.Checked)
			{
				if (!plugin.settings.CacheEnabled || plugin.settings.CacheFolder != textBoxCacheFolder.Text)
				{
					Logger.Add(2, string.Format("缓存设置更新。“{0}”", textBoxCacheFolder.Text));
				}
			}
			else if (plugin.settings.CacheEnabled)
			{
				Logger.Add(2, string.Format("缓存已关闭。"));
			}

			plugin.settings.CacheEnabled = checkBoxEnabled.Checked;
			plugin.settings.CacheFolder = textBoxCacheFolder.Text;

			plugin.SaveSettings();
			return true;
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			checkBoxEnabled.Checked = plugin.settings.CacheEnabled;
			textBoxCacheFolder.Text = plugin.settings.CacheFolder;
		}

		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			textBoxCacheFolder.Text = PathHelper.ProcessFolderBrowserDialog(textBoxCacheFolder.Text, folderBrowser);
		}
	}
}
