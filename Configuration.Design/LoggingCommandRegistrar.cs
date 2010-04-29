
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Igt.Adv.Patron.Logging.Configuration.Design
{
	using TraceListenerCollectionNode = Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners.TraceListenerCollectionNode;
	
	public sealed class LoggingCommandRegistrar : CommandRegistrar
	{	
		public LoggingCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			_addRollOnceTraceListenerNodeCmd();
			AddDefaultCommands(typeof(RollOnceTraceListenerNode));
		}
		
		private void _addRollOnceTraceListenerNodeCmd()
		{
			AddMultipleChildNodeCommand(
																	Resources.RollOnceTraceListenerCmdNodeText,
																	Resources.RollOnceTraceListenerCmdNodeTextLong,
																	typeof(RollOnceTraceListenerNode),
																	typeof(TraceListenerCollectionNode));
		}
	}
}
