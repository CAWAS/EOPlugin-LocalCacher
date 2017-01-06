using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalCacher
{
	public class Settings
	{
		public bool CacheEnabled { get; set; } = false;
		public string CacheFolder { get; set; } = "MyCache";
		public bool HackEnabled { get; set; } = true;
		public bool HackTitleEnabled { get; set; } = true;
		public int CacheEntryFiles { get; set; } = 2;
		public int CachePortFiles { get; set; } = 2;
		public int CacheSceneFiles { get; set; } = 2;
		public int CacheResourceFiles { get; set; } = 2;
		public int CacheSoundFiles { get; set; } = 2;
		public int CheckFiles { get; set; } = 1;

		public bool ShowCacheLog { get; set; } = true;
		public bool ShowMainD2Link { get; set; } = false;
	}
}
