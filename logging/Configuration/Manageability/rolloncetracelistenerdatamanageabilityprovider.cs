
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;


namespace Igt.Adv.Patron.Logging.Configuration.Manageability
{
	using TraceListenerDataManageabilityProvider = Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners.TraceListenerDataManageabilityProvider<RollOnceTraceListenerData>;
	
	public class RollOnceTraceListenerDataManageabilityProvider 
	  : TraceListenerDataManageabilityProvider
	{
		public const string FilenamePropertyName = "filename";
		
		public const string HeaderPropertyName = "header";
		public const string FooterPropertyName = "footer";
		public const string MaxLogsPropertyName = "maxLogs";
		
		public RollOnceTraceListenerDataManageabilityProvider()
		{
			RollOnceTraceListenerDataWmiMapper.RegisterWmiTypes();
		}
		
		
		// adds the admin parts that represent the properties of a
		// specific instance of the config element type managed by the reciever.
		//
		protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
																																	RollOnceTraceListenerData configObj,
																																	IConfigurationSource configSource,
																																	string elementPolicyKeyName)
		{
			/* uses resources for these parts... */
			contentBuilder.AddEditTextPart("Filename", 
																		 FilenamePropertyName,
																		 configObj.FileName,
																		 255,
																		 true);
			contentBuilder.AddEditTextPart("Header",
																		 HeaderPropertyName,
																		 configObj.Header,
																		 512,
																		 false);
			contentBuilder.AddEditTextPart("Footer",
																		 FooterPropertyName,
																		 configObj.Footer,
																		 512,
																		 false);
			contentBuilder.AddNumericPart("Max Logs",
																		MaxLogsPropertyName,
																		configObj.MaxLogs);
			
			AddTraceOptionsPart(contentBuilder, configObj.TraceOutputOptions);
			AddFilterPart(contentBuilder, configObj.Filter);
			AddFormattersPart(contentBuilder, configObj.Formatter, configSource);
		}
		
		protected override void GenerateWmiObjects(RollOnceTraceListenerData configObj,
																							 ICollection<ConfigurationSetting> wmiSettings)
		{
			RollOnceTraceListenerDataWmiMapper.GenerateWmiObjects(configObj, wmiSettings);
		}
		
		protected override void OverrideWithGroupPolicies(RollOnceTraceListenerData configObj,
																											IRegistryKey policyKey)
		{
			string filenameOverride = policyKey.GetStringValue(FilenamePropertyName);
			string headerOverride = policyKey.GetStringValue(HeaderPropertyName);
			string footerOverride = policyKey.GetStringValue(FooterPropertyName);
			int? maxLogs = policyKey.GetIntValue(MaxLogsPropertyName);
			
			string formatterOverride = GetFormatterPolicyOverride(policyKey);
			TraceOptions? traceOutputOptionsO = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
			
			configObj.FileName = filenameOverride;
			configObj.Header = headerOverride;
			configObj.Footer = footerOverride;
			configObj.Formatter = formatterOverride;
			configObj.MaxLogs = maxLogs.Value;
			configObj.TraceOutputOptions = traceOutputOptionsO.Value;
			configObj.Filter = filterOverride.Value;
		}
	}
}
