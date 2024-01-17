using System.Diagnostics;
using System.IO.Compression;

namespace Mist_GUI_Booster
{
	partial class Form_Mist_GUI_Booster
	{
		partial class EnvManager
		{
			private void WriteFileBytes(string rootDirectory, string fileName, byte[] content)
			{
				var fullFileName = Path.Join(rootDirectory, fileName);
				File.WriteAllBytes(fullFileName, content);
			}

			private int RunProcess(
				string rootDirectory,
				string batFileName,
				bool useShellExecute,
				params string[] commands
			)
			{
				var rootDirectoryFull = Path.GetFullPath(rootDirectory);
				var fullBatFileName = Path.Join(rootDirectoryFull, batFileName);

				using (StreamWriter file = new StreamWriter(fullBatFileName))
				{
					foreach (string command in commands)
					{
						file.WriteLine(command);
					}
				}

				ProcessStartInfo processStartInfo = new ProcessStartInfo(fullBatFileName);
				processStartInfo.WorkingDirectory = rootDirectoryFull;
				processStartInfo.UseShellExecute = useShellExecute;
				processStartInfo.CreateNoWindow = true;
				processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

				Process p = new Process();
				p.StartInfo = processStartInfo;
				p.Start();
				p.WaitForExit();

				File.Delete(fullBatFileName);

				return p.ExitCode;
			}

			private void RunFile(string rootDirectory, string fileName, byte[] content)
			{
				var rootDirectoryFull = Path.GetFullPath(rootDirectory);
				var fullFileName = Path.Join(rootDirectoryFull, fileName);
				File.WriteAllBytes(fullFileName, content);

				ProcessStartInfo processStartInfo = new ProcessStartInfo(fullFileName);
				processStartInfo.WorkingDirectory = rootDirectoryFull;

				Process p = new Process();
				p.StartInfo = processStartInfo;
				p.Start();
				p.WaitForExit();

				File.Delete(fullFileName);
			}

			private bool RunBat(string rootDirectory, string fileName)
			{
				var rootDirectoryFull = Path.GetFullPath(rootDirectory);
				var fullFileName = Path.Join(rootDirectoryFull, fileName);
				if (!File.Exists(fullFileName))
				{
					return false;
				}
				
				ProcessStartInfo processStartInfo = new ProcessStartInfo(fullFileName);
				processStartInfo.WorkingDirectory = rootDirectoryFull;

				Process p = new Process();
				p.StartInfo = processStartInfo;
				p.Start();
				p.WaitForExit();

				return true;
			}

			private void ExtractZipFile(string rootDirectory, string dirName, byte[] content)
			{
				var tmpFileName = Path.Join(rootDirectory, $"{dirName}.zip");
				File.WriteAllBytes(tmpFileName, content);
				ZipFile.ExtractToDirectory(tmpFileName, rootDirectory);
				File.Delete(tmpFileName);
			}

			private void DeleteDirectoryIfExists(string filePath)
			{
				if (Directory.Exists(filePath))
				{
					Directory.Delete(filePath, true);
				}
			}

			internal void OnLoad()
			{
				Directory.CreateDirectory(scriptDir);
				pysetterPath = Path.Join(scriptDir, pysetterDir);
				tmpPath = Path.Join(pysetterPath, tmpDir);
				venvPath = Path.Join(pysetterPath, venv);
				pythonPath = Path.Join(pysetterPath, pythonDir);

				if (!Directory.Exists(pysetterPath))
				{
					ExtractZipFile(scriptDir, pysetterDir, Properties.Resources.Pysetter);
				}
				Directory.CreateDirectory(tmpPath);
				WriteFileBytes(scriptDir, "run_mist.bat", Properties.Resources.run_mist);
			}

			internal void RunMist(Form_Mist_GUI_Booster form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Running Mist...");

					form_Mist_GUI.DisableButtons();

					if (!Directory.Exists(venvPath))
					{
						form_Mist_GUI.Log("Environment not set!");
						MessageBox.Show("环境没有配置！", "启动Mist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						form_Mist_GUI.EnableButtons();
						return;
					}

					bool success = RunBat(scriptDir, "run_mist.bat");

					if (success)
					{
						form_Mist_GUI.Log($"Running Mist succeed!");
					}
					else
					{
						form_Mist_GUI.Log($"Running Mist failed!");
						MessageBox.Show("启动Mist失败！", "启动Mist", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}
		}
	}
}
