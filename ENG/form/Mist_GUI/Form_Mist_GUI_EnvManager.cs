using System.Diagnostics;
using System.IO.Compression;

namespace Mist_GUI
{
	partial class Form_Mist_GUI
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

			internal void InstallPython(Form_Mist_GUI form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Installing Python...");

					form_Mist_GUI.DisableButtons();

					if (Directory.Exists(pythonPath))
					{
						form_Mist_GUI.Log("Python is installed!");
						MessageBox.Show("Python is installed!", "Install Python 3.10.11", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						ExtractZipFile(pysetterPath, pythonDir, Properties.Resources.Python310);
						form_Mist_GUI.Log("Python installation succeed!");
						MessageBox.Show("Python installation succeed!", "Install Python 3.10.11", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}

			internal void InstallGit(Form_Mist_GUI form_Mist_GUI)
			{
				form_Mist_GUI.Log("Installing Git...");
				RunFile(tmpPath, "git_installer.exe", Properties.Resources.git_installer);
			}

			internal void ChangeSource(Form_Mist_GUI form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Changing source...");

					form_Mist_GUI.DisableButtons();

					if (!Directory.Exists(pythonPath))
					{
						form_Mist_GUI.Log("Python is not installed!");
						MessageBox.Show("Python is not installed!", "Change Source", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						form_Mist_GUI.EnableButtons();
						return;
					}

					RunProcess(scriptDir, "change_source.bat", false,
						$"@echo off",
						$"cd {pysetterDir}",
						$"\"{pythonDir}/python\" -m pip config set global.index-url https://pypi.tuna.tsinghua.edu.cn/simple"
					);
					MessageBox.Show("Installation source changed!", "Change Source", MessageBoxButtons.OK, MessageBoxIcon.Information);

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}

			internal void ResetSource(Form_Mist_GUI form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Resetting source...");

					form_Mist_GUI.DisableButtons();

					if (!Directory.Exists(pythonPath))
					{
						form_Mist_GUI.Log("Python is not installed!");
						MessageBox.Show("Python is not installed!", "Reset Source", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						form_Mist_GUI.EnableButtons();
						return;
					}

					RunProcess(scriptDir, "reset_source.bat", false,
						$"@echo off",
						$"cd {pysetterDir}",
						$"\"{pythonDir}/python\" -m pip config unset global.index-url"
					);
					MessageBox.Show("Installation source resetted!", "Reset Source", MessageBoxButtons.OK, MessageBoxIcon.Information);

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}

			internal void PrepareEnvironment(Form_Mist_GUI form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Preparing pytorch environment...");

					form_Mist_GUI.DisableButtons();

					if (!Directory.Exists(pythonPath))
					{
						form_Mist_GUI.Log("Python is not installed!");
						MessageBox.Show("Python is not installed!", "Prepare Environment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						form_Mist_GUI.EnableButtons();
						return;
					}

					bool success = RunBat(pysetterPath, "build.bat");

					if (success)
					{
						form_Mist_GUI.Log($"Environment preparing succeed!");
					}
					else
					{
						form_Mist_GUI.Log($"Environment preparing failed!");
						MessageBox.Show("Environment preparing failed!", "Prepare Environment", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}

			internal void TestEnvironment(Form_Mist_GUI form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Testing Environment...");

					form_Mist_GUI.DisableButtons();

					if (!Directory.Exists(venvPath))
					{
						form_Mist_GUI.Log("Environment not set!");
						MessageBox.Show("Environment not set!", "Test Environment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						form_Mist_GUI.EnableButtons();
						return;
					}

					bool success = RunBat(pysetterPath, "env_test.bat");

					if (success)
					{
						form_Mist_GUI.Log($"Environment testing succeed!");
					}
					else
					{
						form_Mist_GUI.Log($"Environment testing failed!");
						MessageBox.Show("Environment testing failed!", "Test Environment", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}

			internal void ClearEnvironment(Form_Mist_GUI form_Mist_GUI)
			{
				DeleteDirectoryIfExists(venvPath);
				form_Mist_GUI.Log($"Environment cleared!");
				MessageBox.Show("Environment cleared!", "Clear Environment", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			internal void RunMist(Form_Mist_GUI form_Mist_GUI)
			{
				Thread thread = new Thread(() =>
				{
					form_Mist_GUI.Log("Running Mist...");

					form_Mist_GUI.DisableButtons();

					if (!Directory.Exists(venvPath))
					{
						form_Mist_GUI.Log("Environment not set!");
						MessageBox.Show("Environment not set!", "Run Mist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
						MessageBox.Show("Running Mist failed!", "Run Mist", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					form_Mist_GUI.EnableButtons();
				});

				thread.Start();
			}
		}
	}
}
