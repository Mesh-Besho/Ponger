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
		
		/// <summary>
		/// Move the overlay so that the center of the rectangle is at the given point.
		/// </summary>
		public void MoveCenter(PointF point)
			{
			var Delta = point - Bounds.Center;
			Bounds += Delta;
			}
		}

	internal class OverlayPolygon : IRenderable
		{
		public List<OverlayLine> Lines { get; } = new List<OverlayLine>();

		public static OverlayPolygon FromWall(Wall wall)
			{
			return new OverlayPolygon(wall.Points.ToArray());
			}

		public OverlayPolygon(params IEnumerable<OverlayLine> lines)
			{
			Lines.AddRange(lines);
			}
		
		public OverlayPolygon(params IEnumerable<PointF> points)
			{
			var Points = points.ToArray();
			
			if (Points.Length < 2)
				throw new ArgumentException("Must have at least 2 points");

			for (var i = 0; i < Points.Length - 1; i++)
				Lines.Add(new OverlayLine(Points[i], Points[i + 1]));

			Lines.Add(new OverlayLine(Points[^1], Points[0]));
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
