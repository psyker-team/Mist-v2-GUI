namespace Mist_GUI_Booster
{
	partial class Form_Mist_GUI_Booster
	{
		internal static string officialWebsite { get; } = @"https://mist-project.github.io/index_en";

		private EnvManager envManager { get; } = new EnvManager();

		internal partial class EnvManager
		{
			internal static string scriptDir { get; } = @"mist-v2";

			internal static string tmpDir { get; } = @"tmp";

			internal static string pysetterDir { get; } = @"Pysetter";

			internal static string sourceDir { get; } = @"src";

			internal static string pythonDir { get; } = @"Python310";

			internal static string venv { get; } = @"venv";

			private string tmpPath { get; set; }

			private string pysetterPath { get; set; }

			private string venvPath { get; set; }

			private string pythonPath { get; set; }
		}
	}
}
