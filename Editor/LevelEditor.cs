using System.Collections.ObjectModel;
using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class LevelEditor
		{
		private readonly Dictionary<ToolType, BaseTool> _Tools = new Dictionary<ToolType, BaseTool>();

		private Pen? _GridMajorPen;
		private Pen? _GridMinorPen;

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
		public event Action<EditorValueRequest> ValueNeeded;
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

			if (type == ToolType.Portal)
				return new PortalTool(this);

			if (type == ToolType.Object)
				return new ObjectTool(this);
			
			throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown tool type");
			}
		
		public void ShowToolError(String message)
			{
			Error?.Invoke(message);
			}
		
		public void AddOverlay(IRenderable overlay)
			{
			if (overlay == null)
				throw new ArgumentNullException(nameof(overlay));
			
			_Overlays.Add(overlay);
			InvokeRedraw();
			}

		public void RemoveOverlay(IRenderable overlay)
			{
			_Overlays.Remove(overlay);
			InvokeRedraw();
			}

		public String RequestString(String prompt, Func<String, Boolean> validator = null)
			{
			var Request = new EditorValueRequest<String>(prompt, validator);
			ValueNeeded?.Invoke(Request);
			return Request.Result ? Request.Value : null;
			}

		public Boolean InvokeMouseDown(EditorMouseEventArgs e)
			{
			return Tool?.InvokeMouseDown(e) ?? false;
			}

		public Boolean InvokeMouseUp(EditorMouseEventArgs e)
			{
			return Tool?.InvokeMouseUp(e) ?? false;
			}

		public Boolean InvokeMouseMove(EditorMouseEventArgs e)
			{
			return Tool?.InvokeMouseMove(e) ?? false;
			}

		public void InvokeRedraw()
			{
			RedrawNeeded?.Invoke();
			}

		public Boolean HitTest(EditorMouseEventArgs e, out HitTestResult result)
			{
			foreach (var handle in Tool?.GetHandles() ?? Array.Empty<OverlayHandle>())
				{
				var Bounds = new RectangleF(handle.Point.X - OverlayHandle.Radius, handle.Point.Y - OverlayHandle.Radius, OverlayHandle.Radius * 2, OverlayHandle.Radius * 2);
				Bounds = e.Camera.Transform(Bounds);
				if (Bounds.Contains(e.ScreenPosition))
					{
					result = handle.Hit;
					return true;
					}
				}

			return Level.HitTest(e.WorldPosition, out result);
			}

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

		public void Render(Graphics graphics, Camera camera)
			{
			if (Program.Settings.Grid.Enabled && !Program.Settings.Grid.OnTop)
				DrawGrid(graphics, camera);

			graphics.SaveTransform();
			graphics.MultiplyTransform(camera.GetMatrix());

			var Renderables = Level?.GetRenderables();

			foreach (var renderable in Renderables)
				{
				var Flags = RenderFlags.None;

				if (Equals(renderable, SelectedEntity))
					Flags |= RenderFlags.Selected;

				renderable.Render(graphics, Flags);
				}

			if (Program.Settings.Grid.Enabled && Program.Settings.Grid.OnTop)
				{
				graphics.RestoreTransform();
				DrawGrid(graphics, camera);
				graphics.SaveTransform();
				graphics.MultiplyTransform(camera.GetMatrix());
				}

			foreach (var overlay in _Overlays)
				overlay.Render(graphics, RenderFlags.None);

			graphics.RestoreTransform();

			foreach (var handle in Tool?.GetHandles() ?? Array.Empty<OverlayHandle>())
				{
				var Position = camera.Transform(handle.Point);
				handle.Render(graphics, Position);
				}
			}

		private void DrawGrid(Graphics graphics, Camera camera)
			{
			_GridMajorPen ??= new Pen(Color.FromGrayscale(0.7f), 1.5f);
			_GridMinorPen ??= new Pen(Color.FromGrayscale(0.9f));
			
			// Expects to draw in screen space, without camera transform

			var GridSizeWorld = new SizeF(Program.Settings.Grid.Size, Program.Settings.Grid.Size);
			var GridSizeScreen = camera.Transform(GridSizeWorld);

			while (GridSizeScreen.Width < 15)
				{
				GridSizeWorld *= 2;
				GridSizeScreen *= 2;
				}

			while (GridSizeScreen.Width > 50)
				{
				GridSizeWorld /= 2;
				GridSizeScreen /= 2;
				}

			var OriginScreen = camera.Transform(PointF.Empty);

			var ViewportWorld = camera.GetWorldViewport();
			var ViewportScreen = camera.GetScreenViewport();

			var X = ViewportWorld.Left - (ViewportWorld.Left % GridSizeWorld.Width);
			var Y = ViewportWorld.Top - (ViewportWorld.Top % GridSizeWorld.Height);

			for(; X <=ViewportWorld.Right || Y <=ViewportWorld.Bottom; X += GridSizeWorld.Width, Y += GridSizeWorld.Height)
				{
				var Screen = camera.Transform(new PointF(X, Y));

				var MajorX = X % Program.Settings.Grid.Size == 0;
				var MajorY = Y % Program.Settings.Grid.Size == 0;

				var PenX = MajorX ? _GridMajorPen : _GridMinorPen;
				var PenY = MajorY ? _GridMajorPen : _GridMinorPen;

				if (Screen.X <= ViewportScreen.Right)
					graphics.DrawLine(PenX, Screen.X, ViewportScreen.Top, Screen.X, ViewportScreen.Bottom);

				if (Screen.Y <= ViewportScreen.Bottom)
					graphics.DrawLine(PenY, ViewportScreen.Left, Screen.Y, ViewportScreen.Right, Screen.Y);
				}

			graphics.DrawLine(Pens.Black, OriginScreen.X - GridSizeScreen.Width, OriginScreen.Y, OriginScreen.X + GridSizeScreen.Width, OriginScreen.Y);
			graphics.DrawLine(Pens.Black, OriginScreen.X, OriginScreen.Y - GridSizeScreen.Height, OriginScreen.X, OriginScreen.Y + GridSizeScreen.Height);
			}
		}
	}
