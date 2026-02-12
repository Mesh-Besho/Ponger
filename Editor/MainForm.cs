using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Eto.Drawing;
using Eto.Forms;
using MeshBesho.Ponger.Editor.Settings;

namespace MeshBesho.Ponger.Editor
	{
	public class MainForm : Form
		{
		private readonly ToolItem[] _ToolToolItems;
		private readonly LevelRenderPanel _Renderer;

		private readonly ToolBar MainToolBar;
		private readonly Command NewCommand;
		private readonly Command OpenCommand;
		private readonly Command SaveCommand;

		private LevelEditor _Editor;
		
		public MainForm()
			{
			Title = "PongEdit";
			
			NewCommand = new Command(InvokeNew) { MenuText = "New" };
			OpenCommand = new Command(InvokeOpen) { MenuText = "Open" };
			SaveCommand = new Command(InvokeSave) { MenuText = "Save" };

			var Modes = new[] { ToolType.Mouse, ToolType.Wall, ToolType.Portal, ToolType.WinZone };

			var ModeCommands = new RadioCommand[Modes.Length];
			_ToolToolItems = new ToolItem[Modes.Length];

			for (var i = 0; i < Modes.Length; i++)
				{
				var Mode = Modes[i];
				ModeCommands[i] = new RadioCommand((_, _) => SetMode(Mode)) { ToolBarText = Modes[i].ToString(), Controller = i == 0 ? null : ModeCommands[i - 1] };
				_ToolToolItems[i] = new RadioToolItem(ModeCommands[i]) { Tag = Mode };
				}

			Menu = new MenuBar
				{
				Items =
					{
					new ButtonMenuItem
						{
						Text = "File",
						Items = { NewCommand, OpenCommand, SaveCommand }
						},
					new ButtonMenuItem
						{
						Text = "Tools",
						Items =
							{
							new ButtonMenuItem((s, e) => InvokeCheck()) { Text = "Check" },
							new SeparatorMenuItem(),
							new ButtonMenuItem((s, e) => new SettingsDialog().ShowModal(this)) { Text = "Settings" }
							}
						},
					new ButtonMenuItem
						{
						Text = "Help",
						Items = { new ButtonMenuItem((s, e) => new AboutDialog().ShowModal(this)) { Text = "About" } }
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

		private void InvokeNew(Object? sender, EventArgs e)
			{
			New();
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

		private void New()
			{
			Open(new Level());
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
			_Editor.ValueNeeded += r => RequestValue(r);
			_Editor.Error += ShowToolError;
			
			_Renderer.Editor = _Editor;

			RebuildToolbar();
			SetMode(ToolType.Mouse);
			_Renderer.Invalidate();
			}
		
		private void Save(String fileName, Level level)
			{
			File.WriteAllText(fileName, level.ToJson().ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
			}
		
		private void RequestValue(EditorValueRequest request)
			{
			var Text = String.Empty;
			request.Result = InputBox.ShowDialog(this, request.Prompt, ref Text) == DialogResult.Ok;
			request.SetResult(Text);
			}

		private void InvokeCheck()
			{
			var Results = _Editor.Level.Validate(true).ToArray();

			if (!Results.Any())
				{
				MessageBox.Show(this, "No problems detected", MessageBoxType.Information);
				return;
				}

			var Builder = new StringBuilder();

			foreach (var problem in Results)
				Builder.AppendLine(problem.Message + " " + (problem.Fixed ? "✔ Fixed!" : "✘ YOU FIX IT!"));
			
			MessageBox.Show(this, Builder.ToString(), MessageBoxType.Warning);
			}
		
		private void SetMode(ToolType mode)
			{
			_Editor.Mode = mode;

			foreach (var item in _ToolToolItems.OfType<RadioToolItem>())
				item.Checked = Equals(item.Tag, mode);
			}
		
		public void ShowToolError(String message)
			{
			Application.Instance.AsyncInvoke(() => MessageBox.Show(this, message, MessageBoxType.Error));
			}
		}
	}
