
using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

using ConfigurationPropertyAttribute = System.Configuration.ConfigurationPropertyAttribute;
/* large portions of this were blantenly stolen from the enterprise logging application block
 * source code.
 * 
 * as a side note:
 * I'm not entirely sure how anyone would implement their own custom logger without
 * viewing Microsoft's code.
 * None what follows is entirely documented, or given in any example.
 * 
 */
namespace Igt.Adv.Patron.Logging.Configuration
{
	using TraceListenerAssembler = Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerAsssembler;
	using IBuilderContext = Microsoft.Practices.ObjectBuilder2.IBuilderContext;
		
	/** this tells the config tool what to do with itself when someone wants to create a RollOnceListner. */
	[Assembler(typeof(RollOnceTraceListenerAssembler))]
	public class RollOnceTraceListenerData : TraceListenerData
	{
		private const string _fileNameProp = "fileName";
		private const string _headerProp = "header";
		private const string _footerProp = "footer";
		private const string _formatterNameProp = "formatter";
		private const string _rollDirProp = "rollDirection";
		private const string _maxLogsProp = "maxLogs";
		
		public RollOnceTraceListenerData() { }
		
		public RollOnceTraceListenerData(string name, 
																string filename, 
																string header, 
																string footer, 
																int maxLogs,
																string formatterName,
																TraceOptions traceOutputOptions)
			: base(name, typeof(TraceListeners.RollOnceTraceListener), traceOutputOptions)
		{
			this.FileName = filename;
			this.Header = header;
			this.Footer = footer;
			this.MaxLogs = maxLogs;
			this.Formatter = formatterName;
		}
		
		public RollOnceTraceListenerData(string name, 
																string filename,
																string header, 
																string footer, 
																int maxLogs,
																string formatterName,
																SourceLevels filter)
			: base(name, typeof(TraceListeners.RollOnceTraceListener), TraceOptions.None, filter)
		{
			this.FileName = filename;
			this.Header = header;
			this.Footer = footer;
			this.MaxLogs = maxLogs;
			this.Formatter = formatterName;
		}
		
		[ConfigurationProperty(_fileNameProp, IsRequired = true)]
		public string FileName
		{
			get { return (string) base[_fileNameProp]; }
			set { base[_fileNameProp] = value; }
		}
		
		[ConfigurationProperty(_headerProp, IsRequired = false)]
		public string Header
		{
			get { return (string) base[_headerProp]; }
			set { base[_headerProp] = value; }
		}
		
		[ConfigurationProperty(_footerProp, IsRequired = false)]
		public string Footer
		{
			get { return (string) base[_footerProp]; }
			set { base[_footerProp] = value; }
		}
		
		[ConfigurationProperty(_formatterNameProp, IsRequired = false)]
		public string Formatter
		{
			get { return (string) base[_formatterNameProp]; }
			set { base[_formatterNameProp] = value; }
		}
		
		[ConfigurationProperty(_maxLogsProp, IsRequired = false)]
		public int MaxLogs
		{
			get { return (int) base[_maxLogsProp]; }
			set { base[_maxLogsProp] = value; }
		}
	}

	/** this creates the tracelistener from the configuration data. */
	public class RollOnceTraceListenerAssembler : TraceListenerAssembler
	{
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData configData, 
																					 IConfigurationSource configurationSource, 
																					 ConfigurationReflectionCache reflectionCache)
		{
			RollOnceTraceListenerData rolConfigData = configData as RollOnceTraceListenerData;
			
			ILogFormatter formatter = 
				GetFormatter(context, rolConfigData.Formatter, configurationSource, reflectionCache);
			
			TraceListener createdObject	= new Igt.Adv.Patron.Logging.TraceListeners.RollOnceTraceListener(
			                                                                  rolConfigData.Name,
																																				rolConfigData.FileName,
																																				rolConfigData.Header,
																																				rolConfigData.Footer,
																																				formatter,
																																				rolConfigData.MaxLogs
																																				);
			return createdObject;
		}
	}
}
