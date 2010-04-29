
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Igt.Adv.Patron.Logging.Configuration.Design
{
	public class PatronLoggingConfigManager : ConfigurationDesignManager
	{
		public override void Register(System.IServiceProvider serviceProvider)
		{
			LoggingMapNodeRegistrar logReg = new LoggingMapNodeRegistrar(serviceProvider);
			logReg.Register();
		}
	}
}
