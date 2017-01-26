using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalCacher
{
	static class TaskRecord
	{
		static ConcurrentDictionary<string, string> record = new ConcurrentDictionary<string, string>();
		//KEY: url, Value: filepath
		//只有在验证文件修改时间后，向客户端返回本地文件或者将文件保存到本地时才需要使用

		static public void Add( string url, string filepath )
		{
			record.AddOrUpdate( url, filepath, ( key, oldValue ) => filepath );
		}

		static public string GetAndRemove( string url )
		{
			string ret;
			if (url.Contains(".swf"))
			{
			    // 大咪咪在remodel中发包请求资源有问题，固将swf的record保留 by AtrisMio @ 7d960e0
			    // 同一个swf文件连续请求多次，第一次请求后记录被删除，影响后续请求 by Gizeta
			    record.TryGetValue(url, out ret);
			}
			else
			{
			    record.TryRemove(url, out ret);
			}
			return ret;
		}
		static public string Get( string url )
		{
			string ret;
			if ( record.TryGetValue( url, out ret ) )
				return ret;
			return "";
		}
	}
}
