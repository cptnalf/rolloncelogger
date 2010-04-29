
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

/* large portions of this were blantenly stolen from the enterprise logging application block
 * source code.
 * 
 * as a side note:
 * I'm not entirely sure how anyone would implement their own custom logger without
 * viewing Microsoft's code.
 * None what follows is entirely documented, or given in any example.
 * 
 */

namespace Igt.Adv.Patron.Logging.TraceListeners
{
	using IntList = System.Collections.Generic.List<int>;
	//using EnvHelper = Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.EnvironmentHelper;
	
	[ConfigurationElementType(typeof(Configuration.RollOnceTraceListenerData))]
	public class RollOnceTraceListener : FlatFileTraceListener
	{
		private int _maxLogs = 10;
		private bool _rolled = false;
		
		public RollOnceTraceListener() : base() { }
		
		public RollOnceTraceListener(string name, string filename, string header, string footer, ILogFormatter formatter, int maxLogs)
			: base(filename, header, footer, formatter)
		{
			this.Name = name;
			_maxLogs = maxLogs;
		}
		
		public RollOnceTraceListener(string filename, ILogFormatter formatter)
			: base(filename)
			{ }
		
		private void _rollFile()
		{
			/* lots o problems here
			 * really need to not construct the log file until we've rolled all of the log files.
			 * this might require that this class use a different base constructor.
			 *
			 * 2nd:
			 *  we really don't want to roll log files unless we have to.
			 *  the whole point here is to move the file called 'filename' out of the way so that it is the latest
			 *  copy.
			 *  (i suppose this could be configurable...)
			 *
			 *  
			 */
			
			string[] files = null;
			string basename;
			string name;
			string pattern;
			
			/* so now i have 'foo.log' and 'c:\flarg\blarg\' */
			string currentName = ((FileStream)((StreamWriter)this.Writer).BaseStream).Name;
			name = Path.GetFileName(currentName);
			basename = currentName.Substring(0, currentName.Length - name.Length);
			
			/* find all files which have names we're concerned about. */
			pattern = name + "*";
			files = Directory.GetFiles(basename, pattern, SearchOption.TopDirectoryOnly);
			
			IntList ids = new IntList();
			
			/* get the list of files */
			foreach(string fullpath in files)
				{
					int id = 0;
					int idx = fullpath.IndexOf(currentName);
					if (idx == 0)
						{
							System.Console.WriteLine(fullpath);
							
							if (fullpath.Length != currentName.Length
							    && fullpath.Length > (currentName.Length + 1))
								{
									idx = currentName.Length + 1;
									string number = fullpath.Substring(idx);
									
									/* skip this file if the outermost extension is not a number. */
									if (! int.TryParse(number, out id)) { continue; }
								}
							else { id = -1; }
							
							ids.Add(id);
							System.Console.WriteLine(id);
						}
					else
						{
							/* umm ??? */
							continue;
						}
				}
			
			/* sort them, biggest first. */
			ids.Sort( (x, y) => y.CompareTo(x));
			
			{
				/* torch the open writer. */
				this.Writer.Close();
				this.Writer = null;
			}
			
			
			System.Console.WriteLine("[{0}", ids.Count);
			
			int idsIdx = -1;
			/* this will give us:
			 * filename + filename.0 -> filename.(maxLogs -1)
			 * eg:
			 * maxLogs = 5
			 * you will have:
			 * foo.log
			 * foo.log.0
			 * foo.log.1
			 * foo.log.2
			 * foo.log.3
			 * foo.log.4
			 * 
			 * anything past foo.log.4 will be deleted.
			 */
			if (ids.Count > 0 && ids.Count > (this._maxLogs + 1))
				{
					while((ids.Count - (idsIdx +1))  > _maxLogs)
						{
							++idsIdx;
							string destFile = string.Format("{0}{1}.{2}", basename, name, ids[idsIdx] );
							if (ids[idsIdx] == -1) { destFile = currentName; }
							
							System.Console.WriteLine("-- {0}", destFile);
							try{
								File.Delete(destFile);
							} catch(System.IO.IOException ex) { }
						}
				}
			
			if (idsIdx < 0) { idsIdx =0; }
			/* now move them around. 
			 * starting from where we left off deleting, or the beginning.
			 */
			for(; idsIdx < ids.Count; ++idsIdx)
				{
					string sourceFile;
					string destFile = string.Format("{0}{1}.{2}", basename, name, ids[idsIdx] +1 );
					
					if (ids[idsIdx] == -1) { sourceFile = currentName; }
					else { sourceFile = string.Format("{0}{1}.{2}", basename, name, ids[idsIdx]); }
					
					try
						{
							if (File.Exists(destFile)) 
								{
									/* if for some reason the dest file was created 
									 * after we listed the dir,
									 * move it out of the way.
									 */
									string newName = Path.GetRandomFileName();
									newName = Path.Combine(basename, newName);
									
									File.Move(destFile, newName);
								}
							
							if (File.Exists(sourceFile))
								{
									/* i don't want to do this in this case. 
									// take care of tunneling issues http://support.microsoft.com/kb/172190
									File.SetCreationTime(sourceFile, DateTime.Now);
									*/
									
									File.Move(sourceFile, destFile);
								}
						}
					catch(System.IO.IOException) { }
				}
			
			/* let the base class do the file-opening... */
			this.Writer = null;
			_rolled = true;
		}
		
		public override void TraceData(TraceEventCache eventCache, 
																	 string source, 
																	 TraceEventType eventType, 
																	 int id, 
																	 object data)
		{
			if (! _rolled) { _rollFile(); }
			
			base.TraceData(eventCache, source, eventType, id, data);
		}

		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			base.TraceData(eventCache, source, eventType, id, data);
		}
	}
}
