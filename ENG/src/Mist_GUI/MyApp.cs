namespace Mist_GUI
{
	internal static class MyApp
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form_Mist_GUI().Init(args));
		}
	}
}
