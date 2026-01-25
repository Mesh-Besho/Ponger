using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor.Ponger
	{
	public class Wall : EditorEntity, IRenderable
		{
		private static readonly Color _SelectHighlight = new Color(Colors.White, 0.5f);

		public Wall(IEnumerable<PointF>? points = null)
			{
			if (points != null)
				Points.AddRange(points);
			}

		public List<PointF> Points { get; } = new List<PointF>();
		public Color Color { get; set; }

		public void Render(Graphics graphics, RenderFlags flags)
			{
			graphics.FillPolygon(Color, Points.ToArray());

			if (flags.HasFlag(RenderFlags.Selected))
				{
				graphics.FillPolygon(_SelectHighlight, Points.ToArray());
				}
			}

		public RectangleF GetBoundingRectangle()
			{
			var MinX = Single.MaxValue;
			var MinY = Single.MaxValue;
			var MaxX = Single.MinValue;
			var MaxY = Single.MinValue;

			foreach (var point in Points)
				{
				if (point.X < MinX) MinX = point.X;
				if (point.Y < MinY) MinY = point.Y;
				if (point.X > MaxX) MaxX = point.X;
				if (point.Y > MaxY) MaxY = point.Y;
				}

			return new RectangleF(MinX, MinY, MaxX - MinX, MaxY - MinY);
			}

		public Boolean HitTest(PointF point, out HitTestResult result)
			{
			// Try to hit any of the points first

			for (var i = 0; i < Points.Count; i++)
				if (HitTestPoint(i, point, out result))
					return true;

			// Otherwise, check if the point is inside the polygon

			return HitTestInside(point, out result);
			}

		private Boolean HitTestPoint(Int32 pointIndex, PointF testPoint, out HitTestResult result)
			{
			result = HitTestResult.None;

			if ((UInt32)pointIndex >= (UInt32)Points.Count)
				return false;

			var wallPoint = Points[pointIndex];

			const Single radius = 10f;
			const Single radiusSquared = radius * radius;

			var dx = testPoint.X - wallPoint.X;
			var dy = testPoint.Y - wallPoint.Y;

			if ((dx * dx) + (dy * dy) <= radiusSquared)
				{
				result = new WallHitTestResult(this, pointIndex);
				return true;
				}

			return false;
			}

		private Boolean HitTestInside(PointF point, out HitTestResult result)
			{
			result = HitTestResult.None;

			// Need at least a triangle for a closed polygon area
			if (Points.Count < 3)
				return false;

			// Quick reject using bounding rectangle
			var bounds = GetBoundingRectangle();
			if (!bounds.Contains(point))
				return false;

			// Ray-casting point-in-polygon test (even/odd rule).
			// Treats Points as the polygon vertices; the polygon is implicitly closed.
			var inside = false;

			for (Int32 i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
				{
				var pi = Points[i];
				var pj = Points[j];

				// Check whether the horizontal ray to the right from 'point' crosses edge (pj -> pi)
				var intersects =
					((pi.Y > point.Y) != (pj.Y > point.Y)) &&
					(point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y) + pi.X);

				if (intersects)
					inside = !inside;
				}

			if (inside)
				{
				result = new WallHitTestResult(this);
				return true;
				}

			return false;
			}

		public static Wall FromJson(JsonObject json)
			{
			var Wall = new Wall();
			
			//Wall.Type = ParseWallType(json["Type"].GetValue<String>());
			Wall.Color = FormatHelper.ParseColor(json["Color"].GetValue<String>());

			var VerticesSection = json["Vertices"].AsArray();
			
			foreach(var vertex in VerticesSection)
				Wall.Points.Add(FormatHelper.VertexFromJson(vertex.AsObject()));

			return Wall;
			}
		
		public Wall Clone()
			{
			var Wall = new Wall
				{
				Color = Color
				};

			foreach(var point in Points)
				Wall.Points.Add(point);
			
			return Wall;
			}
		}
	}
