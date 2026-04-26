using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class LevelRenderPanel : Drawable
		{
		public LevelEditor Editor { get; set; }

		private PointF? _MouseWorldPosition;
		private PointF _DragCameraOrigin;
		private PointF? _DragMouseOrigin;
		
		public Camera ActiveCamera
			{
			get => _ActiveCamera;
			private set
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

			_ActiveCamera.Size = Size;
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

			e.Handled = Editor?.InvokeMouseDown(Transform(e)) ?? false;
			}

		private EditorMouseEventArgs Transform(MouseEventArgs e)
			{
			return new EditorMouseEventArgs(_ActiveCamera, e.Location, e.Buttons);
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

			e.Handled = Editor?.InvokeMouseUp(Transform(e)) ?? false;
			}
		
		protected override void OnMouseMove(MouseEventArgs e)
			{
			base.OnMouseMove(e);

			_MouseWorldPosition = _ActiveCamera.TransformInverse(e.Location);
			
			if (_DragMouseOrigin.HasValue)
				{
				var Delta = e.Location - _DragMouseOrigin.Value;
				var TransformedDelta = Delta / _ActiveCamera.Scale;
				_ActiveCamera.Origin = _DragCameraOrigin - TransformedDelta;
				Invalidate();
				
				e.Handled = true;
				return;
				}

			e.Handled = Editor?.InvokeMouseMove(Transform(e)) ?? false;
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

			Zoom(e.Delta.Height, e.Location);
			}

		public void Zoom(Single delta, PointF? location = null)
			{
			var ScreenPosition = location ?? PointF.Empty;
			var OldMouseWorldPosition = _ActiveCamera.TransformInverse(ScreenPosition);
			
			_ActiveCamera.Scale = (Single)Math.Clamp(_ActiveCamera.Scale * Math.Pow(1.1f, delta), 0.1f, 10f);

			// Asymmetric zoom. Not sure if I like this better or not. Definitely feels better to zoom in on the mouse position,
			// but zooming out feels weird either way.
			
			if (delta > 0)
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
			Editor.Render(e.Graphics, _ActiveCamera);
			e.Graphics.RestoreTransform();

			if (_MouseWorldPosition.HasValue)
				e.Graphics.DrawText(SystemFonts.Default(20), Brushes.Black, new PointF(10, 10), $"{_MouseWorldPosition.Value.X:F2}, {_MouseWorldPosition.Value.Y:F2}");
			}
		}
	}
