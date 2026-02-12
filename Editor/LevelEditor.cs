using System.Collections.ObjectModel;
using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class LevelEditor
		{
		private readonly Dictionary<ToolType, BaseTool> _Tools = new Dictionary<ToolType, BaseTool>();

		private ToolType _Mode;

		private readonly List<IRenderable> _Overlays = new List<IRenderable>();

		private EditorEntity? _SelectedEntity;
		
		public LevelEditor(Level level)
			{
			Level = level;
			ToolbarItems = new ObservableCollection<ToolItem>();

			OnModeChanged(ToolType.Mouse);
			}

		public Level Level { get; }

		public ObservableCollection<ToolItem> ToolbarItems { get; }

		public EditorEntity? SelectedEntity
			{
			get => _SelectedEntity;
			set
				{
				if (Equals(_SelectedEntity, value))
					return;

				_SelectedEntity = value;

				InvokeRedraw();
				}
			}

		public ToolType Mode
			{
			get => _Mode;
			set
				{
				if (_Mode == value)
					return;

				_Mode = value;

				OnModeChanged(value);
				}
			}

		public BaseTool Tool { get; private set; }

		public event Action RedrawNeeded;
		public event Action<String> Error;		

		private void OnModeChanged(ToolType value)
			{
			// Reset whatever the old tool was doing
			Tool?.OnDeactivated();

			if (!_Tools.TryGetValue(value, out var tool))
				{
				tool = CreateTool(value);
				_Tools.Add(value, tool);
				}

			Tool = tool;
			Tool.OnActivated();

			ModeChanged?.Invoke();
			}

		public event Action ModeChanged;

		private BaseTool? CreateTool(ToolType type)
			{
			if (type == ToolType.Mouse)
				return new MouseTool(this);

			if (type == ToolType.Wall)
				return new WallTool(this);

			if (type == ToolType.WinZone)
				return new WinZoneTool(this);

			throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown tool type");
			}
		
		public void ShowToolError(String message)
			{
			Error?.Invoke(message);
			}
		
		public void AddOverlay(IRenderable overlay)
			{
			_Overlays.Add(overlay);
			InvokeRedraw();
			}

		public void RemoveOverlay(IRenderable overlay)
			{
			_Overlays.Remove(overlay);
			InvokeRedraw();
			}

		public Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			return Tool?.InvokeMouseDown(button, point) ?? false;
			}

		public Boolean InvokeMouseUp(MouseButtons button, PointF point)
			{
			return Tool?.InvokeMouseUp(button, point) ?? false;
			}

		public Boolean InvokeMouseMove(MouseButtons button, PointF point)
			{
			return Tool?.InvokeMouseMove(button, point) ?? false;
			}

		public void InvokeRedraw()
			{
			RedrawNeeded?.Invoke();
			}

		public void Render(Graphics graphics)
			{
			var Renderables = Level?.GetRenderables();

			foreach (var renderable in Renderables)
				{
				var Flags = RenderFlags.None;

				if (Equals(renderable, SelectedEntity))
					Flags |= RenderFlags.Selected;

				renderable.Render(graphics, Flags);
				}

			foreach (var overlay in _Overlays)
				overlay.Render(graphics, RenderFlags.None);
			}

		public Boolean HitTest(PointF point, out HitTestResult result) => Level.HitTest(point, out result);

		public PointF Snap(PointF point)
			{
			if (Keyboard.Modifiers.HasFlag(Keys.Control))
				return point;
			
			var Whole = Program.Settings.Grid.Size;
			var Half = Whole / 2f;
			var Fake = new PointF(point.X + (point.X > 0 ? Half : -Half), point.Y + (point.Y > 0 ? Half : -Half));
			var Snapped = new PointF(Fake.X - (Fake.X % Whole), Fake.Y - (Fake.Y % Whole));
			return Snapped;
			}
		}
	}
