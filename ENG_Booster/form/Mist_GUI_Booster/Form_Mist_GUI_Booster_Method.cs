using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mist_GUI_Booster
{
	partial class Form_Mist_GUI_Booster
	{
		internal Form_Mist_GUI_Booster Init(string[] args)
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
