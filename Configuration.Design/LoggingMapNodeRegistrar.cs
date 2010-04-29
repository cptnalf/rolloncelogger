
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Igt.Adv.Patron.Logging.Configuration.Design
{
	public class LoggingMapNodeRegistrar : NodeMapRegistrar
	{
		public LoggingMapNodeRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		
		public override void Register()
		{
			AddMultipleNodeMap("Roll Once Trace Listener", 
			                   typeof(RollOnceTraceListenerNode),
			                   typeof(Igt.Adv.Patron.Logging.Configuration.RollOnceTraceListenerData));
		}
	}
}
