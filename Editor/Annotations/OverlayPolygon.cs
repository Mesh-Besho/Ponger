using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
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
