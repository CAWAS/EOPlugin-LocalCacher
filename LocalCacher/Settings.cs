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
	}
}
