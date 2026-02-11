using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class LevelRenderPanel : Drawable
		{
		public LevelEditor Editor { get; set; }

		private PointF _Center;
		private PointF? _MouseWorldPosition;
		private PointF _DragCameraOrigin;
		private PointF? _DragMouseOrigin;
		
		public Camera ActiveCamera
			{
			get => _ActiveCamera;
			set
				{
				_ActiveCamera = value;
				Invalidate();
				}
			} private Camera _ActiveCamera;

		public LevelRenderPanel()
			{
			_ActiveCamera = new Camera();
			}

		protected override void OnSizeChanged(EventArgs e)
			{
			base.OnSizeChanged(e);

			_Center = new PointF(Width / 2f, Height / 2f);
			}

		protected override void OnMouseDown(MouseEventArgs e)
			{
			base.OnMouseDown(e);

			if (e.Buttons == MouseButtons.Middle)
				{
				_DragCameraOrigin = _ActiveCamera.Origin;
				_DragMouseOrigin = e.Location;

				e.Handled = true;
				return;
				}

			var MouseScreenPosition = e.Location - _Center;
			var WorldPoint = _ActiveCamera.TransformInverse(MouseScreenPosition);
			e.Handled = Editor?.InvokeMouseDown(e.Buttons, WorldPoint) ?? false;
			}

		protected override void OnMouseUp(MouseEventArgs e)
			{
			base.OnMouseUp(e);

			if (e.Buttons == MouseButtons.Middle)
				{
				_DragCameraOrigin = PointF.Empty;
				_DragMouseOrigin = null;
				
				e.Handled = true;
				return;
				}

			var MouseScreenPosition = e.Location - _Center;
			var WorldPoint = _ActiveCamera.TransformInverse(MouseScreenPosition);
			e.Handled = Editor?.InvokeMouseUp(e.Buttons, WorldPoint) ?? false;
			}
		
		protected override void OnMouseMove(MouseEventArgs e)
			{
			if(!e.Delta.IsZero)
				throw new Exception("Mouse movement should be handled by the mouse wheel.");
			
			base.OnMouseMove(e);

			var MouseScreenPosition = e.Location - _Center;
			_MouseWorldPosition = _ActiveCamera.TransformInverse(MouseScreenPosition);
			
			if (_DragMouseOrigin.HasValue)
				{
				var Delta = e.Location - _DragMouseOrigin.Value;
				var TransformedDelta = Delta / _ActiveCamera.Scale;
				_ActiveCamera.Origin = _DragCameraOrigin - TransformedDelta;
				Invalidate();
				
				e.Handled = true;
				return;
				}

			e.Handled = Editor?.InvokeMouseMove(e.Buttons, _MouseWorldPosition.Value) ?? false;
			Invalidate();
			}

		protected override void OnMouseLeave(MouseEventArgs e)
			{
			base.OnMouseLeave(e);

			_MouseWorldPosition = null;
			}

		protected override void OnMouseWheel(MouseEventArgs e)
			{
			base.OnMouseWheel(e);

			var ScreenPosition = e.Location - _Center;
			var OldMouseWorldPosition = _ActiveCamera.TransformInverse(ScreenPosition);
			
			_ActiveCamera.Scale = (Single)Math.Clamp(_ActiveCamera.Scale * Math.Pow(1.1f, e.Delta.Height), 0.1f, 10f);

			// Asymmetric zoom. Not sure if I like this better or not. Definitely feels better to zoom in on the mouse position,
			// but zooming out feels weird either way.
			
			if (e.Delta.Height > 0)
				{
				var NewMouseWorldPosition = _ActiveCamera.TransformInverse(ScreenPosition);
				_ActiveCamera.Origin += new PointF(OldMouseWorldPosition.X - NewMouseWorldPosition.X, OldMouseWorldPosition.Y - NewMouseWorldPosition.Y);
				}

			Invalidate();
			}

		protected override void OnPaint(PaintEventArgs e)
			{
			e.Graphics.Clear(Colors.White);
			
			e.Graphics.SaveTransform();
			e.Graphics.TranslateTransform(_Center);
			e.Graphics.MultiplyTransform(_ActiveCamera.GetMatrix());

			if (Program.Settings.Grid.Enabled && !Program.Settings.Grid.OnTop)
				DrawGrid(e.Graphics);
			
			Editor.Render(e.Graphics);

			if (Program.Settings.Grid.Enabled && Program.Settings.Grid.OnTop)
				DrawGrid(e.Graphics);
			
			e.Graphics.RestoreTransform();

			if (_MouseWorldPosition.HasValue)
				e.Graphics.DrawText(SystemFonts.Default(20), Brushes.Black, new PointF(10, 10), $"{_MouseWorldPosition.Value.X:F2}, {_MouseWorldPosition.Value.Y:F2}");
			}
		
		private void DrawGrid(Graphics graphics)
			{
			var GridSize = new Size(Program.Settings.Grid.Size, Program.Settings.Grid.Size);
			var VisibleBounds = _ActiveCamera.TransformInverse(new RectangleF(-_Center.X, -_Center.Y, Size.Width, Size.Height));

			var Pen = Pens.LightGrey;

			for (var x = VisibleBounds.Left - (VisibleBounds.Left % GridSize.Width); x <= VisibleBounds.Right; x += GridSize.Width)
				graphics.DrawLine(Pen, x, VisibleBounds.Top, x, VisibleBounds.Bottom);

			for (var y = VisibleBounds.Top - (VisibleBounds.Top % GridSize.Height); y <= VisibleBounds.Bottom; y += GridSize.Height)
				graphics.DrawLine(Pen, VisibleBounds.Left, y, VisibleBounds.Right, y);
			}
		}
	}
