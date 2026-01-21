using System.Collections.ObjectModel;
using Eto.Drawing;
using Eto.Forms;
using MeshBesho.Ponger.Editor.Ponger;

namespace MeshBesho.Ponger.Editor
	{
	internal class LevelEditor
		{
		private readonly Dictionary<ToolType, BaseTool> _Tools = new Dictionary<ToolType, BaseTool>();

		private ToolType _Mode;

		private readonly List<OverlayLine> _Overlays = new List<OverlayLine>();

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

				RedrawNeeded?.Invoke();
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

		private void OnModeChanged(ToolType value)
			{
			// Reset whatever the old tool was doing
			Tool?.Reset();

			if (!_Tools.TryGetValue(value, out var tool))
				{
				tool = CreateTool(value);
				_Tools.Add(value, tool);
				}

			else
				tool.Reset();

			Tool = tool;

			ModeChanged?.Invoke();
			}

		public event Action ModeChanged;

		private BaseTool? CreateTool(ToolType type)
			{
			if (type == ToolType.Mouse)
				return new MouseTool(this);

			if (type == ToolType.Wall)
				return new WallTool(this);

			throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown tool type");
			}

		public void AddOverlay(OverlayLine overlay)
			{
			_Overlays.Add(overlay);
			}

		public void RemoveOverlay(OverlayLine overlay)
			{
			_Overlays.Remove(overlay);
			}

		public void InvokeMouseDown(MouseButtons button, PointF point)
			{
			Tool?.InvokeMouseDown(button, point);

			}

		public void InvokeMouseUp(MouseButtons button, PointF point)
			{
			Tool?.InvokeMouseUp(button, point);
			}

		public void InvokeMouseMove(MouseButtons button, PointF point)
			{
			Tool?.InvokeMouseMove(button, point);
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
				overlay.Render(graphics);
			}

		public EditorEntity HitTest(PointF point) => Level.HitTest(point);
		}
	}
