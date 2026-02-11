using Eto.Drawing;
using MeshBesho.Ponger.Editor;

namespace MeshBesho.Ponger.Editor
	{
	internal class OverlayLine : IRenderable
		{
		public OverlayLine() { }

		public OverlayLine(PointF start, PointF end)
			{
			Start = start;
			End = end;
			}

		public PointF Start { get; set; }
		public PointF End { get; set; }

		public void Render(Graphics graphics, RenderFlags flags)
			{
			graphics.DrawLine(Colors.Black, Start, End);
			}
		}

	public class OverlayRectangle : IRenderable
		{
		public OverlayRectangle() { }
		
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
		}

	internal class OverlayPolygon : IRenderable
		{
		public List<OverlayLine> Lines { get; } = new List<OverlayLine>();

		public static OverlayPolygon FromWall(Wall wall)
			{
			var Polygon = new OverlayPolygon();

			if (wall.Points.Count < 2)
				throw new ArgumentException("Wall must have at least 2 points");

			for (var i = 0; i < wall.Points.Count - 1; i++)
				Polygon.Lines.Add(new OverlayLine(wall.Points[i], wall.Points[i + 1]));

			Polygon.Lines.Add(new OverlayLine(wall.Points[^1], wall.Points[0]));

			return Polygon;
			}

		public OverlayPolygon(params IEnumerable<OverlayLine> lines)
			{
			Lines.AddRange(lines);
			}

		public void Render(Graphics graphics, RenderFlags flags)
			{
			foreach(var line in Lines)
				line.Render(graphics, flags);
			}
		
		public void Move(PointF delta)
			{
			foreach (var line in Lines)
				{
				line.Start = line.Start + delta;
				line.End = line.End + delta;
				}
			}
		
		public void MovePoint(Int32 index, PointF delta)
			{
			var StartIndex = index;
			var EndIndex = index == 0 ? Lines.Count - 1 : index - 1;
			
			Lines[StartIndex].Start += delta;
			Lines[EndIndex].End += delta;
			}
		}
	
	}
