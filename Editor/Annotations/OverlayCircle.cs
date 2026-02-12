using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public class OverlayCircle : IRenderable
		{
		public OverlayCircle() { }
		
		public OverlayCircle(PointF center, Single radius)
			{
			Bounds = new RectangleF(center.X - radius, center.Y - radius, radius * 2, radius * 2);	
			}

		public RectangleF Bounds { get; private set; }
		
		public void Render(Graphics graphics, RenderFlags flags)
			{
			graphics.DrawEllipse(Colors.Black, Bounds);
			}
		
		/// <summary>
		/// Move the overlay so that the center of the rectangle is at the given point.
		/// </summary>
		public void MoveCenter(PointF point)
			{
			var Delta = point - Bounds.Center;
			Bounds += Delta;
			}
		}
	}
