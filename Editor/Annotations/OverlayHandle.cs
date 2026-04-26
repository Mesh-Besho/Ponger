using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public class OverlayHandle
		{
		public const Single Radius = 10f;

		public OverlayHandle(PointF center, HitTestResult hit)
			{
			Point = center;
			Hit = hit;
			}

		public PointF Point { get; private set; }
		public HitTestResult Hit { get; }

		public void Render(Graphics graphics, PointF point)
			{
			var Bounds = new RectangleF(point.X - Radius, point.Y - Radius, Radius * 2, Radius * 2);
			graphics.FillEllipse(Colors.Pink, Bounds);
			}

		public void MoveBy(PointF point)
			{
			var Delta = point - Point;
			Point += Delta;
			}

		public void MoveTo(PointF point)
			{
			Point = point;
			}
		}
	}
