using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Codeplex.Data;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Plugins;
using Fiddler;

namespace LocalCacher
{
    public class LocalCacher : ObserverPlugin
    {
		public override string MenuTitle => "LocalCacher";

		private const string PLUGIN_SETTINGS = @"Settings\LocalCacher.json";
	    public Settings settings;
	    private CacheCore Cache;

		private JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();

		public override string Version
		{
			get { return "<BUILD_VERSION>"; }
		}

	    public LocalCacher()
	    {
		    settings = LoadSettings();
			Cache = new CacheCore(settings);
		    ModifyConfiguration.Instance.LoadSettings();
	    }

	    private ConcurrentBag<string> CheckedFiles = new ConcurrentBag<string>();

	    private void MakaiOnBeforeResponse(Session oSession)
	    {
		    if (oSession.fullUrl.Contains("/kcsapi/api_start2"))
		    {
			    string api_start2full = oSession.GetResponseBodyAsString();

			    var mod = ModifyConfiguration.Instance;
			    bool changed = false;
			    string api_start2_json = api_start2full.Substring(7);
			    Dictionary<string, object> api_start2 =
				    JavaScriptSerializer.DeserializeObject(api_start2_json) as Dictionary<string, object>;
			    try
			    {
				    var api_data = api_start2["api_data"] as Dictionary<string, object>;
				    var api_mst_ship = api_data["api_mst_ship"] as object[];
				    var api_mst_shipgraph = api_data["api_mst_shipgraph"] as object[];

				    string shipCache = Path.Combine(settings.CacheFolder, @"kcs\resources\swf\ships");
				    //for debug//shipCache = Path.Combine(Application.StartupPath, "Settings");
				    foreach (var shipgraph_data_obj in api_mst_shipgraph)
				    {
					    var shipgraph_data = shipgraph_data_obj as Dictionary<string, object>;
					    if (shipgraph_data["api_sortno"].ToString() == "0")
						    continue;
					    string shipid = shipgraph_data["api_id"].ToString();
					    string api_filename = shipgraph_data["api_filename"].ToString();
					    var ship_data =
						    api_mst_ship.FirstOrDefault(e => (e as Dictionary<string, object>)["api_id"].ToString() == shipid) as
							    Dictionary<string, object>;

					    string configFile = null;
					    //if (Configuration.Config.CacheSettings.CacheEnabled)
					    {
						    configFile = Path.Combine(shipCache, api_filename + ".config.ini");
					    }
					    if (File.Exists(configFile)) //岛风GO格式
					    {
						    IniFile iniFile = new IniFile(configFile);
						    ModifyConfigurationIniNode IniNode = new ModifyConfigurationIniNode();
						    IniNode.api_filename = api_filename;
						    IniNode.api_name = iniFile.ReadString("info", "ship_name", null);
						    IniNode.api_getmes = iniFile.ReadString("info", "getmes", null);
						    //IniNode.api_info = iniFile.ReadString("info", "sinfo", null);
						    IniNode.api_config_parameter = iniFile.ReadSectionValues("graph");

						    bool flag = ModifyIt("api_boko_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_boko_d", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_kaisyu_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_kaisyu_d", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_kaizo_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_kaizo_d", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_map_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_map_d", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_ensyuf_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_ensyuf_d", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_ensyue_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_battle_n", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_battle_d", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_weda", shipgraph_data, IniNode);
						    flag |= ModifyIt("api_wedb", shipgraph_data, IniNode);

						    if (flag)
						    {
							    changed = true;
						    }

						    // 魔改名称
						    if (!string.IsNullOrEmpty(IniNode.api_name))
						    {
							    ship_data["api_name"] = IniNode.api_name;
							    flag = true;
							    changed = true;
						    }
						    // 魔改获得信息
						    if (!string.IsNullOrEmpty(IniNode.api_getmes))
						    {
							    ship_data["api_getmes"] = IniNode.api_getmes;
							    flag = true;
							    changed = true;
						    }

						    if (flag)
						    {
							    ElectronicObserver.Utility.Logger.Add(2, string.Format("应用魔改: {0} → {1}", IniNode.api_filename, IniNode.api_name));
						    }
					    }
					    else //ApiModify.json格式
					    {
						    var ModifyNode = ModifyConfiguration.Instance.GetModifyNode(api_filename);
						    if (ModifyNode == null)
							    continue;

						    // 魔改立绘坐标
						    bool flag = ModifyIt("api_boko_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_boko_d", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_kaisyu_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_kaisyu_d", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_kaizo_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_kaizo_d", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_map_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_map_d", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_ensyuf_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_ensyuf_d", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_ensyue_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_battle_n", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_battle_d", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_weda", shipgraph_data, ModifyNode.api_parameter);
						    flag |= ModifyIt("api_wedb", shipgraph_data, ModifyNode.api_parameter);

						    if (flag)
						    {
							    changed = true;
						    }

						    // 魔改名称
						    if (!string.IsNullOrEmpty(ModifyNode.api_name))
						    {
							    ship_data["api_name"] = ModifyNode.api_name;
							    flag = true;
							    changed = true;
						    }
						    if (flag)
						    {
							    ElectronicObserver.Utility.Logger.Add(2, string.Format("应用魔改: {0} → {1}", ModifyNode.api_filename, ModifyNode.api_name));
						    }
					    }

				    }

				    // 如果有变动
				    if (changed)
				    {
					    oSession.utilSetResponseBody("svdata=" + JavaScriptSerializer.Serialize(api_start2));
						// Nekoxy defaults to ASCII so we need to manually add this
						oSession.oResponse.headers["Content-Type"] += ";charset=UTF-8";
				    }
			    }
			    catch (Exception e)
			    {
					ElectronicObserver.Utility.Logger.Add(3, "应用魔改过程中出现错误:" + e.Message + Environment.NewLine + e.StackTrace);
			    }
		    }
	    }

		private bool ModifyIt(string parameter, Dictionary<string, object> source, Dictionary<string, object> dest)
		{
			try
			{
				if (dest.ContainsKey(parameter))
				{
					var ModifyData = source[parameter] as object[];
					var NewModifyData = dest[parameter] as object[];
					ModifyData[0] = NewModifyData[0];
					ModifyData[1] = NewModifyData[1];
					return true;
				}
			}
			catch (Exception e)
			{
				ErrorReporter.SendErrorReport(e, string.Format("魔改参数错误，{0}.{1}", source["api_filename"], parameter));
			}

			return false;
		}

		private bool ModifyIt(string parameter, Dictionary<string, object> source, ModifyConfigurationIniNode iniNode)
		{
			try
			{
				var ModifyData = source[parameter] as object[];
				string strLeft = parameter.Substring(4) + "_left";
				string strTop = parameter.Substring(4) + "_top";
				int Left, Top;
				bool Modified = false;
				if (int.TryParse(iniNode.Get(strLeft), out Left))
				{
					ModifyData[0] = Left;
					Modified = true;
				}
				if (int.TryParse(iniNode.Get(strTop), out Top))
				{
					ModifyData[1] = Top;
					Modified = true;
				}
				return Modified;
			}
			catch (Exception e)
			{
				ErrorReporter.SendErrorReport(e, string.Format("魔改参数错误，{0}.{1}", source["api_filename"], parameter));
			}

			return false;
		}

		public override bool OnBeforeRequest(Session oSession)
		{
			if (oSession.fullUrl.Contains("/kcsapi/api_start2"))
			{
				oSession.bBufferResponse = true;
			}

			if (settings.CacheEnabled && oSession.fullUrl.Contains("/kcs/"))
			{

				// = KanColleCacher =
				string filepath;
				var direction = Cache.GotNewRequest(oSession.fullUrl, out filepath);

				if (direction == Direction.Return_LocalFile
					|| direction == Direction.NoCache_LocalFile)
				{

					//返回本地文件
					oSession.utilCreateResponseAndBypassServer();
					oSession.oResponse.headers["Server"] = "nginx";
					oSession.oResponse.headers["Date"] = GMTHelper.ToGMTString(DateTime.Now);

					filepath = filepath.ToLower();
					if (filepath.EndsWith(".swf"))
						oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";
					else if (filepath.EndsWith(".mp3"))
						oSession.oResponse.headers["Content-Type"] = "audio/mpeg";
					else if (filepath.EndsWith(".png"))
						oSession.oResponse.headers["Content-Type"] = "image/png";

					oSession.oResponse.headers["Last-Modified"] = _GetModifiedTime(filepath);
					oSession.oResponse.headers["Connection"] = "close";
					if (direction == Direction.NoCache_LocalFile)
					{
						oSession.oResponse.headers["Pragma"] = "no-cache";
						oSession.oResponse.headers["Cache-Control"] = "no-cache";
					}
					else
					{
						oSession.oResponse.headers["Pragma"] = "public";
						oSession.oResponse.headers["Cache-Control"] = "max-age=18000, public";
					}
					oSession.oResponse.headers["Accept-Ranges"] = "bytes";

					byte[] file;
					using (var fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
					    file = new byte[fs.Length];
					    fs.Read(file, 0, (int)fs.Length);
					}
					oSession.ResponseBody = file;

					//Debug.WriteLine("CACHR> 【返回本地】" + result);

				}
				else if (direction == Direction.Verify_LocalFile)
				{

					//请求服务器验证文件
					oSession.oRequest.headers["If-Modified-Since"] = _GetModifiedTime(filepath);
					oSession.bBufferResponse = true;

					//Debug.WriteLine("CACHR> 【验证文件】" + oSession.PathAndQuery);

				}
				else if (settings.ShowCacheLog && (settings.ShowMainD2Link || !oSession.fullUrl.Contains("mainD2.swf")))
				{

					//下载文件
					ElectronicObserver.Utility.Logger.Add(2, string.Format("重新下载缓存文件: {0}", oSession.fullUrl));
				}


				return true;
			}
			return false;
		}

	    public override bool OnBeforeResponse(Session oSession)
	    {
			MakaiOnBeforeResponse(oSession);

			if (settings.CacheEnabled && oSession.PathAndQuery.StartsWith("/kcs/"))
			{

				if (oSession.responseCode == 304)
				{

					string filepath = TaskRecord.GetAndRemove(oSession.fullUrl);
					//只有TaskRecord中有记录的文件才是验证的文件，才需要修改Header
					if (!string.IsNullOrEmpty(filepath))
					{
						CheckedFiles.Add(filepath);

						//服务器返回304，文件没有修改 -> 返回本地文件
						byte[] file;
						using (var fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
						{
						    file = new byte[fs.Length];
						    fs.Read(file, 0, (int)fs.Length);
						}
						oSession.ResponseBody = file;
						oSession.oResponse.headers.HTTPResponseCode = 200;
						oSession.oResponse.headers.HTTPResponseStatus = "200 OK";
						oSession.oResponse.headers["Last-Modified"] = oSession.oRequest.headers["If-Modified-Since"];
						oSession.oResponse.headers["Accept-Ranges"] = "bytes";
						oSession.oResponse.headers.Remove("If-Modified-Since");
						oSession.oRequest.headers.Remove("If-Modified-Since");
						if (filepath.EndsWith(".swf"))
							oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";
					}

					return true;
				}
				else if (oSession.responseCode == 200)
				{

					// 由服务器下载所得
					if (oSession.PathAndQuery.StartsWith("/kcs/sound") && oSession.PathAndQuery.IndexOf("titlecall/") < 0)
					{

						oSession.oResponse.headers["Pragma"] = "no-cache";
						oSession.oResponse.headers["Cache-Control"] = "no-cache";
					}
					return true;
				}

			}

			return false;
		}

	    public override bool OnAfterSessionComplete(Session oSession)
	    {
			if (settings.CacheEnabled && oSession.responseCode == 200)
			{

				string filepath = TaskRecord.GetAndRemove(oSession.fullUrl);
				if (!(string.IsNullOrEmpty(filepath) || CheckedFiles.Contains(filepath)))
				{
					if (File.Exists(filepath))
						File.Delete(filepath);

					//保存下载文件并记录Modified-Time
					try
					{

						if (settings.ShowCacheLog)
						{

							ElectronicObserver.Utility.Logger.Add(2, string.Format("更新缓存文件： {0}.", filepath));
						}

						oSession.SaveResponseBody(filepath);
						_SaveModifiedTime(filepath, oSession.oResponse.headers["Last-Modified"]);
						//Debug.WriteLine("CACHR> 【下载文件】" + oSession.PathAndQuery);
					}
					catch (Exception ex)
					{
						ElectronicObserver.Utility.ErrorReporter.SendErrorReport(ex, "会话结束时，保存返回文件时发生异常：" + oSession.fullUrl);
					}
				}

				return true;
			}

			return false;
		}

		private string _GetModifiedTime(string filepath)
		{
			FileInfo fi;
			DateTime dt = default(DateTime);
			try
			{
				fi = new FileInfo(filepath);
				dt = fi.LastWriteTime;
				return GMTHelper.ToGMTString(dt);
			}
			catch (Exception ex)
			{
				ElectronicObserver.Utility.ErrorReporter.SendErrorReport(ex, "在读取文件修改时间时发生异常：" + dt);
				return "";
			}
		}

		private void _SaveModifiedTime(string filepath, string gmTime)
		{
			FileInfo fi;
			try
			{
				fi = new FileInfo(filepath);
				DateTime dt = GMTHelper.GMT2Local(gmTime);
				if (dt.Year > 1900)
				{
					fi.LastWriteTime = dt;
				}
			}
			catch (Exception ex)
			{
				ElectronicObserver.Utility.ErrorReporter.SendErrorReport(ex, string.Format("在保存文件修改时间时发生异常。filepath: {0}, gmTime: {1}", filepath, gmTime));
			}
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
