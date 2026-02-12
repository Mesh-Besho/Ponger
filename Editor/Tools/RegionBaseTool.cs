using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal abstract class RegionBaseTool : BaseTool
		{
		private OverlayRectangle _PointOverlay;
		private OverlayLine? _CurrentOverlay;
		private readonly Stack<OverlayLine> _Overlays = new Stack<OverlayLine>();

		protected readonly Stack<PointF> _Points = new Stack<PointF>();

		protected RegionBaseTool(LevelEditor editor)
			: base(editor)
			{
			_PointOverlay = new OverlayRectangle(PointF.Empty, new SizeF(4, 4));
			}

		public override void OnActivated()
			{
			Editor.AddOverlay(_PointOverlay);
			}

		public override void OnDeactivated()
			{
			Editor.RemoveOverlay(_PointOverlay);
			Reset();
			}

		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			point = Editor.Snap(point);

			if (button == MouseButtons.Primary)
				{
				CommitPoint(point);
				return true;
				}

			if (button == MouseButtons.Alternate)
				{
				RollbackPoint();

				if (_CurrentOverlay != null)
					_CurrentOverlay.End = point;

				return true;
				}

			return false;
			}

		public override Boolean InvokeMouseMove(MouseButtons button, PointF point)
			{
			point = Editor.Snap(point);

			_PointOverlay.MoveCenterTo(point);

			if (_CurrentOverlay != null)
				{
				_CurrentOverlay.End = point;
				Editor.InvokeRedraw();

				return true;
				}

			return false;
			}

		protected virtual void CommitPoint(PointF point)
			{
			if (_Points.Count > 0)
				{
				if (point == _Points.Last())
					{
					OnRegionComplete(_Points.ToArray());
					Reset();
					return;
					}

				_CurrentOverlay!.End = point;
				_Overlays.Push(_CurrentOverlay);
				}

			_Points.Push(point);

			_CurrentOverlay = new OverlayLine(point, point);
			Editor.AddOverlay(_CurrentOverlay);
			Editor.InvokeRedraw();
			}

		private void RollbackPoint()
			{
			if (_Points.Count < 2)
				{
				Reset();
				return;
				}

			Editor.RemoveOverlay(_CurrentOverlay);
			Editor.InvokeRedraw();

			_Points.Pop();
			_CurrentOverlay = _Overlays.Pop();
			}

		protected abstract void OnRegionComplete(PointF[] points);

		protected void Reset()
			{
			foreach (var overlay in _Overlays)
				Editor.RemoveOverlay(overlay);

			if (_CurrentOverlay != null)
				Editor.RemoveOverlay(_CurrentOverlay);

			_Overlays.Clear();
			_CurrentOverlay = null;

			_Points.Clear();

			Editor.InvokeRedraw();
			}
		}
	}
