
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Igt.Adv.Patron.Logging.Configuration.Manageability
{
	public static class RollOnceTraceListenerDataWmiMapper
	{
		public static void GenerateWmiObjects(RollOnceTraceListenerData configObj,
																					ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new RollOnceTraceListenerSetting(configObj,
																											 configObj.Name,
																											 configObj.FileName,
																											 configObj.Header,
																											 configObj.Footer,
																											 configObj.Formatter,
																											 configObj.MaxLogs,
																											 configObj.TraceOutputOptions.ToString(),
																											 configObj.Filter.ToString()));
		}
		
		internal static bool SaveChanges(RollOnceTraceListenerSetting setting,
																		 ConfigurationElement sourceElem)
		{
			RollOnceTraceListenerData elem = sourceElem as RollOnceTraceListenerData;
			
			elem.FileName = setting.Filename;
			elem.Header = setting.Header;
			elem.Footer = setting.Footer;
			elem.Formatter = setting.Formatter;
			elem.MaxLogs = setting.MaxLogs;
			elem.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);
			
			SourceLevels filter;
			if (ParseHelper.TryParseEnum(setting.Filter, out filter))
				{
					elem.Filter = filter;
				}
			return true;
		}
		
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(RollOnceTraceListenerSetting));
		}
	}
}
