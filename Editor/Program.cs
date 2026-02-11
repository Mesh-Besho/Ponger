using System.IO;
using Eto.Forms;
using MeshBesho.Ponger.Editor.Settings;

namespace MeshBesho.Ponger.Editor
	{
	public static class Program
		{
		private static String _SettingsPath;
		public static ProgramSettings Settings { get; private set; }

		[STAThread]
		public static void Main(String[] args)
			{
			_SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PongEdit", "settings.json");
			Settings = ProgramSettings.LoadFromFile(_SettingsPath);
			
			new Application().Run(new MainForm());
			}

		public static void SaveSettings()
			{
			Settings.SaveToFile(_SettingsPath);
			}
		}
	}
