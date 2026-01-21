using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	public class MainForm : Form
		{
		private readonly ToolItem[] _ToolToolItems;
		private readonly LevelEditor Editor;

		private readonly ToolBar MainToolBar;
		private readonly RadioCommand ModeMouseCommand;
		private readonly RadioCommand ModeWallCommand;
		private readonly Command OpenCommand;

		private readonly LevelRenderPanel Renderer;
		private readonly Command SaveCommand;

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

			Renderer = new LevelRenderPanel();
			Renderer.MouseUp += (s, e) => Editor.InvokeMouseUp(e.Buttons, e.Location);
			Renderer.MouseDown += (s, e) => Editor.InvokeMouseDown(e.Buttons, e.Location);
			Renderer.MouseMove += (s, e) => Editor.InvokeMouseMove(e.Buttons, e.Location);

			var Level = new Level();
			Editor = new LevelEditor(Level);
			Editor.RedrawNeeded += () => Renderer.Invalidate();
			Editor.ModeChanged += () => RebuildToolbar();

			Renderer.Editor = Editor;

			Size = new Size(800, 600);
			Content = Renderer;

			RebuildToolbar();
			}

		private void RebuildToolbar()
			{
			var Items = new List<ToolItem>(_ToolToolItems);

			var ToolItems = Editor.Tool?.GetToolbarItems().ToArray() ?? Array.Empty<ToolItem>();

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
			throw new NotImplementedException();
			}

		private void InvokeOpen(Object? sender, EventArgs e)
			{
			throw new NotImplementedException();
			}

		protected override void OnLoadComplete(EventArgs e)
			{
			base.OnLoadComplete(e);
			}

		private void SetMode(ToolType mode)
			{
			Editor.Mode = mode;
			}
		}
	}
