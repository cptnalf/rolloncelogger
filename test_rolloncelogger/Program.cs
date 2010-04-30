using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test_rolloncelogger
{
	using MSLog = Microsoft.Practices.EnterpriseLibrary.Logging.Logger;
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("begin");
			MSLog.Write("flarg", "General");
			
			MSLog.Write("a;lskdf;alksdjf", "General");
		}
	}
}
