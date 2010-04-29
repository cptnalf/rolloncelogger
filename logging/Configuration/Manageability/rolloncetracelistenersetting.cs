
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Igt.Adv.Patron.Logging.Configuration.Manageability
{
	using TraceListenerSetting = Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners.TraceListenerSetting;
	
	// instance of the data as in instrumentation class.
	[ManagementEntity]
	public class RollOnceTraceListenerSetting : TraceListenerSetting
	{
		private string _filename;
		private string _header;
		private string _footer;
		private string _formatter;
		private int _maxLogs;
		
		public RollOnceTraceListenerSetting(
																				RollOnceTraceListenerData sourceElem,
																				string name,
																				string filename,
																				string header,
																				string footer,
																				string formatter,
																				int maxLogs,
																				string traceOutputOptions,
																				string filter)
			: base(sourceElem, name, traceOutputOptions, filter)
		{
			this._filename = filename;
			this._header = header;
			this._footer = footer;
			this._formatter = formatter;
			this._maxLogs = maxLogs;
		}
		
		[ManagementConfiguration]
		public string Filename
		{
			get { return _filename; }
			set { _filename = value; }
		}
		
		[ManagementConfiguration]
		public string Header
		{
			get { return _header; }
			set { _header = value; }
		}
		
		[ManagementConfiguration]
		public string Footer
		{
			get { return _footer; }
			set { _footer = value; }
		}

		[ManagementConfiguration]
		public string Formatter
		{
			get { return _formatter; }
			set { _formatter = value; }
		}
		
		[ManagementConfiguration]
		public int MaxLogs
		{
			get { return _maxLogs; }
			set { _maxLogs = value; }
		}
		
		[ManagementBind]
		public static RollOnceTraceListenerSetting BindInstance(string appName,
																														string sectionName,
																														string name)
		{ return BindInstance<RollOnceTraceListenerSetting>(appName, sectionName, name); }
		
		[ManagementEnumerator]
		public static IEnumerable<RollOnceTraceListenerSetting> GetInstances()
		{ return GetInstances<RollOnceTraceListenerSetting>(); }
		
		protected override bool SaveChanges(ConfigurationElement sourceElem)
		{
			return RollOnceTraceListenerDataWmiMapper.SaveChanges(this, sourceElem);
		}
	}
}
