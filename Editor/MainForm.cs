using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	public class MainForm : Form
		{
		private readonly ToolItem[] _ToolToolItems;
		private readonly LevelRenderPanel _Renderer;

		private readonly ToolBar MainToolBar;
		private readonly RadioCommand ModeMouseCommand;
		private readonly RadioCommand ModeWallCommand;
		private readonly Command OpenCommand;
		private readonly Command SaveCommand;

		private LevelEditor _Editor;
		
		public MainForm()
			{
			OpenCommand = new Command(InvokeOpen) { MenuText = "Open" };
			SaveCommand = new Command(InvokeSave) { MenuText = "Save" };

			ModeMouseCommand = new RadioCommand((s, e) => SetMode(ToolType.Mouse)) { ToolBarText = "Mouse" };
			ModeWallCommand = new RadioCommand((s, e) => SetMode(ToolType.Wall)) { ToolBarText = "Wall", Controller = ModeMouseCommand };

			_ToolToolItems =
				[
				ModeMouseCommand.CreateToolItem(),
				ModeWallCommand.CreateToolItem()
				];

			Menu = new MenuBar
				{
				Items =
					{
					new ButtonMenuItem
						{
						Text = "File",
						Items = { OpenCommand, SaveCommand }
						}
					}
				};
			
			ToolBar = MainToolBar = new ToolBar();
			
			_Renderer = new LevelRenderPanel();

			Open(new Level());
			
			Size = new Size(800, 600);
			Content = _Renderer;

			RebuildToolbar();
			}

		private void RebuildToolbar()
			{
			var Items = new List<ToolItem>(_ToolToolItems);

			var ToolItems = _Editor.Tool?.GetToolbarItems().ToArray() ?? Array.Empty<ToolItem>();

			if (ToolItems.Length > 0)
				{
				Items.Add(new SeparatorToolItem());
				Items.AddRange(ToolItems);
				}

			MainToolBar.Items.Clear();
			MainToolBar.Items.AddRange(Items);
			}

		private void InvokeSave(Object? sender, EventArgs e)
			{
			using var Picker = new SaveFileDialog
				{
				Filters = { new FileFilter("Level files", "*.ponger") }
				};

			if (Picker.ShowDialog(this) == DialogResult.Cancel)
				return;

			Save(Picker.FileName, _Editor.Level);
			}
		
		private void Save(String fileName, Level level)
			{
			File.WriteAllText(fileName, level.ToJson().ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
			}

		private void InvokeOpen(Object? sender, EventArgs e)
			{
			using var Picker = new OpenFileDialog
				{
				CheckFileExists = true,
				Filters = { new FileFilter("Level files", "*.ponger") }
				};

			if (Picker.ShowDialog(this) == DialogResult.Cancel)
				return;

			Open(Picker.FileName);
			}
		
		private void Open(String fileName)
			{
			var Text = File.ReadAllText(fileName);
			var Json = JsonObject.Parse(Text).AsObject();

			Open(Level.FromJson(Json));
			}

		private void Open(Level level)
			{
			_Editor = new LevelEditor(level);
			_Editor.RedrawNeeded += () => _Renderer?.Invalidate();
			_Editor.ModeChanged += () => RebuildToolbar();

			_Renderer.Editor = _Editor;
			
			RebuildToolbar();
			}
		
		protected override void OnLoadComplete(EventArgs e)
			{
			base.OnLoadComplete(e);
			}

		private void SetMode(ToolType mode)
			{
			_Editor.Mode = mode;
			}
		}
	}
