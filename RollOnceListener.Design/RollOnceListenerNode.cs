
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using System.Diagnostics;
using System;

namespace Igt.Adv.Patron.RollOnceListener.Configuration.Design
{
	using TraceListenerNode = Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners.TraceListenerNode;
	using TraceListenerData = Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData;
	using RollOnceTraceListenerData = Igt.Adv.Patron.Logging.Configuration.RollOnceTraceListenerData;
	
	public class RollOnceTraceListenerNode : TraceListenerNode
	{
		private FormatterNode _formatterNode;
		private string _formatterName;
		private string _filename;
		private string _header;
		private string _footer;
		private int _maxLogs;
	
	
		/// Initialize a new instance of the <see cref="RollOnceTraceListenerNode"/> class.
		/// </summary>
		public RollOnceTraceListenerNode()
			: this(new RollOnceTraceListenerData(Resources.RollOnceTraceListenerNode, 
																					 DefaultValues.FileName, 
																					 DefaultValues.Header, 
																					 DefaultValues.Footer, 
																					 DefaultValues.MaxLogs,
																					 string.Empty,
																					 TraceOptions.None))
			{
			}
	
		/// <summary>
		/// Initialize a new instance of the <see cref="RollOnceTraceListenerNode"/> class with a <see cref="RollOnceTraceListenerData"/> instance.
		/// </summary>
		/// <param name="traceListenerData">A <see cref="RollOnceTraceListenerData"/> instance.</param>
		public RollOnceTraceListenerNode(RollOnceTraceListenerData traceListenerData)
		{
			if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");
			
			Rename(traceListenerData.Name);
			TraceOutputOptions = traceListenerData.TraceOutputOptions;
			this._formatterName = traceListenerData.Formatter;
			this._filename = traceListenerData.FileName;
			this._header = traceListenerData.Header;
			this._footer = traceListenerData.Footer;
			this._maxLogs = traceListenerData.MaxLogs;
		}        
		
		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		/// <value>
		/// The file name.
		/// </value>
		[Required]
		[Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
		[FilteredFileNameEditor(typeof(Resources), "RollOnceTraceListenerFileDialogFilter")]
		[SRDescription("RollOnceTraceListenerFileName", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Filename
		{
			get { return _filename; }
			set { _filename = value; }
		}
		
		/// <summary>
		/// Gets or sets the header for the file.
		/// </summary>
		/// <value>
		/// The header for the file.
		/// </value>
		[SRDescription("RollOnceTraceListenerHeader", typeof(Resources))]
			[SRCategory("CategoryGeneral", typeof(Resources))]
			public string Header
			{
				get { return _header; }
				set { _header = value; }
			}

		/// <summary>
		/// Gets or sets the footer for the file.
		/// </summary>
		/// <value>
		/// The footer for the file.
		/// </value>
		[SRDescription("RollOnceTraceListenerFooter", typeof(Resources))]
			[SRCategory("CategoryGeneral", typeof(Resources))]
			public string Footer
			{
				get { return _footer; }
				set { _footer = value; }
			}

		/// <summary>
		/// Gets or sets the formatter for the file.
		/// </summary>
		/// <value>
		/// The formatter for the file.
		/// </value>
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
			[ReferenceType(typeof(FormatterNode))]
			[SRDescription("FormatDescription", typeof(Resources))]
			[SRCategory("CategoryGeneral", typeof(Resources))]
			public FormatterNode Formatter
			{
				get { return _formatterNode; }
				set
					{
						_formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(_formatterNode,
																																					value,
																																					OnFormatterNodeRemoved,
																																					OnFormatterNodeRenamed);

						_formatterName = _formatterNode == null ? string.Empty : _formatterNode.Name;
					}
			}

		/// <summary>
		/// Gets the <see cref="RollOnceTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="RollOnceTraceListenerData"/> this node represents.
		/// </value>
		public override TraceListenerData TraceListenerData
		{
			get
				{
					RollOnceTraceListenerData data = 
						new RollOnceTraceListenerData(Name, _filename, _header, _footer, _maxLogs, _formatterName, TraceOutputOptions);
					data.TraceOutputOptions = TraceOutputOptions;
					return data;
				}
		}

		/// <summary>
		/// Sets the formatter to use for this listener.
		/// </summary>
		/// <param name="formatterNodeReference">
		/// A <see cref="FormatterNode"/> reference or <see langword="null"/> if no formatter is defined.
		/// </param>
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (_formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
		}		

		private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
		{
			_formatterNode = null;
		}

		private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
		{
			_formatterName = e.Node.Name;
		}
	}
}
