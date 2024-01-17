using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mist启动器一键包
{
	partial class Form_Mist启动器一键包
	{
		internal Form_Mist启动器一键包 Init(string[] args)
		{
			envManager.OnLoad();
			Log("Program started.");
			return this;
		}

		private void Log(object message)
		{
			Console.WriteLine(message);
		}

		private void OpenUrl(string url)
		{
			var runUrl = url.Replace("&", "^&");
			var processStartInfo = new ProcessStartInfo(runUrl);
			processStartInfo.UseShellExecute = true;
			Process.Start(processStartInfo);
		}

		private void ShowLink()
		{
			Log("Show link...");
			OpenUrl(officialWebsite);
		}

		private void DisableButtons()
		{
			imageButtonRunMist.Enabled = false;
		}

		private void EnableButtons()
		{
			imageButtonRunMist.Enabled = true;
        }
	}
}
