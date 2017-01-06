using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;
using ElectronicObserver.Window.Plugins;
using Fiddler;

namespace LocalCacher
{
    public class LocalCacher : ObserverPlugin
    {

		public override string MenuTitle => "LocalCacher";

		private const string PLUGIN_SETTINGS = @"Settings\LocalCacher.json";
		public Settings settings = LoadSettings();

		public override string Version
		{
			get { return "<BUILD_VERSION>"; }
		}

		public override bool OnBeforeRequest(Session oSession)
	    {
			return true;
		}

	    public override bool OnBeforeResponse(Session oSession)
	    {
			return true;
		}

	    public override bool OnAfterSessionComplete(Session oSession)
	    {
		    return true;
	    }

	    private static Settings LoadSettings()
	    {
			if (File.Exists(PLUGIN_SETTINGS))
				return DynamicJson.Parse(File.ReadAllText(PLUGIN_SETTINGS)).Deserialize<Settings>();
			else
				return new Settings();
		}

		public void SaveSettings()
		{
			if (settings == null)
			{
				settings = new Settings();
			}
			if (!Directory.Exists("Settings"))
			{
				Directory.CreateDirectory("Settings");
			}
			File.WriteAllText(PLUGIN_SETTINGS, DynamicJson.Serialize(settings));
		}

		public override PluginSettingControl GetSettings()
	    {
		    return new SettingsForm(this);
	    }
    }
}
