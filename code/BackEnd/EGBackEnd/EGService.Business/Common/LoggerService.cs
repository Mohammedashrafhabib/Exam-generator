#region Using ...
using Framework.Common.Enums;
using Framework.Core.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
#endregion

/*


 */
namespace EGService.Business.Common
{
	/// <summary>
	/// Specify a functionality to 
	/// log any thing in a log.
	/// </summary>
	public class LoggerService : ILoggerService
	{
		#region Data Members
		private readonly IHttpContextAccessor _httpContext;
		private readonly string _rootPath = "logs";
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of 
		/// type LoggerService.
		/// </summary>
		public LoggerService(IHttpContextAccessor httpContext)
		{
			this._httpContext = httpContext;

		}
		#endregion

		#region ILoggerService
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="type"></param>
		/// <param name="customFileName"></param>
		public void Log(string content, LogType type, string customFileName = null)
		{
			DateTime now = DateTime.Now;

			try
			{
				if (Directory.Exists(this._rootPath) == false)
					Directory.CreateDirectory(this._rootPath);

				string yearFolderPath = $"{this._rootPath}\\{now.Year}";
				if (Directory.Exists(yearFolderPath) == false)
					Directory.CreateDirectory(yearFolderPath);

				string monthFolderPath = $"{this._rootPath}\\{now.Year}\\{now.Month}";
				if (Directory.Exists(monthFolderPath) == false)
					Directory.CreateDirectory(monthFolderPath);

				string dayFolderPath = $"{this._rootPath}\\{now.Year}\\{now.Month}\\{now.Day}";
				if (Directory.Exists(dayFolderPath) == false)
					Directory.CreateDirectory(dayFolderPath);

				//string filePath = $"{_rootPath}\\{now.Year}\\{now.Month}\\{now.Day}\\{now.ToLongTimeString().Replace(":", "-")}-{type.ToString()}.log";

				string filePath = $"{_rootPath}\\{now.Year}\\{now.Month}\\{now.Day}";
				string fileName = $"Logs.log";
				


				if (string.IsNullOrEmpty(customFileName) == false)
				{
					fileName = $"{customFileName}-{now.ToLongTimeString().Replace(":", "-")}-{type.ToString()}.log";
				}

				string fullPath = $"{filePath}\\{fileName}";

				string path = $"{filePath}\\{fileName}";
				if (!File.Exists(path))
				{ // Create a file to write to   
					using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
					{
						using (StreamWriter wr = new StreamWriter(fs))
						{
							wr.WriteLine("Start Loggging ");
							wr.Flush();
						}
					}
				} // Open the file to read from.  

				using (StreamWriter sw = File.AppendText(fullPath))
				{
					sw.WriteLine(content);
					sw.Flush();

				}

			}
			catch (Exception ex)
			{
				try
				{
					#region Log Exception in EventLog
					EventLog eventLog = new EventLog(this.GetType().FullName, System.Environment.MachineName);

					eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
					#endregion
				}
				catch { }
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		public void LogError(string content)
		{
			this.Log(content, LogType.Error);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		public void LogError(Exception ex)
		{
			this.Log(ex.ToString(), LogType.Error);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="customFileName"></param>
		public void LogInfo(string content, string customFileName = null)
		{
			this.Log(content, LogType.Information);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="customFileName"></param>
		public void LogText(string content, string customFileName = null)
		{
			this.Log(content, LogType.Text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="customFileName"></param>
		public void LogWarning(string content, string customFileName = null)
		{
			this.Log(content, LogType.Warning);
		}

		#endregion
	}
}
