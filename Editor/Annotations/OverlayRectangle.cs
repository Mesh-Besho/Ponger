using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public class OverlayRectangle : IRenderable
		{
		public OverlayRectangle() { }

		public OverlayRectangle(RectangleF bounds)
			{
			Bounds = bounds;	
			}
		
		public OverlayRectangle(PointF topLeft, PointF bottomRight)
			{
			Bounds = new RectangleF(topLeft, bottomRight - topLeft);	
			}

		public OverlayRectangle(PointF origin, SizeF size, Boolean center = false)
			{
			var NewBounds = new RectangleF(origin, size);
			if (center) NewBounds.Center = origin;
			Bounds = NewBounds;
			}
		
		public RectangleF Bounds { get; set; }
		
		public void Render(Graphics graphics, RenderFlags flags)
			{
			graphics.DrawRectangle(Colors.Black, Bounds);
			}
		
		/// <summary>
		/// Move the overlay so that the center of the rectangle is at the given point.
		/// </summary>
		public void MoveCenterTo(PointF point)
			{
			var Delta = point - Bounds.Center;
			Bounds += Delta;
			}
		
		public void Move(PointF delta)
			{
			Bounds += delta;
			}
		}
	}
