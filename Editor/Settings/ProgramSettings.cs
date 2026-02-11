using System.IO;
using System.Text.Json;

namespace MeshBesho.Ponger.Editor.Settings
	{
	public class ProgramSettings
		{
		public GridSettings Grid { get; set; } = new();
		public String DefaultColorName { get; set; } = "RED";

		#region Load/Save
		
		public static ProgramSettings LoadFromFile(String fileName)
			{
			try
				{
				if (File.Exists(fileName))
					{
					var Json = File.ReadAllText(fileName);
					return JsonSerializer.Deserialize<ProgramSettings>(Json, GetJsonOptions()) ?? new ProgramSettings();
					}
				}
			
			catch { /* Ignore */}
			
			return new ProgramSettings();
			}

		public void SaveToFile(String fileName)
			{
			String? Directory = Path.GetDirectoryName(fileName);

			if (Directory != null && !System.IO.Directory.Exists(Directory))
				System.IO.Directory.CreateDirectory(Directory);

			String Json = JsonSerializer.Serialize(this, GetJsonOptions());
			File.WriteAllText(fileName, Json);
			}

		private static JsonSerializerOptions GetJsonOptions()
			{
			return new JsonSerializerOptions
				{
				WriteIndented = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};
			}
		
		#endregion
		}
	}