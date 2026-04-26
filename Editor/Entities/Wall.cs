using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
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
			result = HitTestResult.None;

			// Need at least a triangle for a closed polygon area
			if (Points.Count < 3)
				return false;

			// Quick reject using bounding rectangle
			var Bounds = GetBoundingRectangle();
			if (!Bounds.Contains(point))
				return false;

			// Ray-casting point-in-polygon test (even/odd rule).
			// Treats Points as the polygon vertices; the polygon is implicitly closed.
			var Inside = false;

			for (Int32 I = 0, J = Points.Count - 1; I < Points.Count; J = I++)
				{
				var Pi = Points[I];
				var Pj = Points[J];

				// Check whether the horizontal ray to the right from 'point' crosses edge (pj -> pi)
				var Intersects =
					((Pi.Y > point.Y) != (Pj.Y > point.Y)) &&
					(point.X < (Pj.X - Pi.X) * (point.Y - Pi.Y) / (Pj.Y - Pi.Y) + Pi.X);

				if (Intersects)
					Inside = !Inside;
				}

			if (Inside)
				{
				result = new WallHitTestResult(this);
				return true;
				}

			return false;
			}

		public override OverlayHandle[] GetHandles()
			{
			return Points
				.Select((p, i) => new OverlayHandle(p, new WallHitTestResult(this, i)))
				.ToArray();
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

		public JsonObject ToJson()
			{
			var Data = new JsonObject();

			Data["Color"] = FormatHelper.FormatColor(Color);
			
			var VerticesSection = new JsonArray();
			Data["Vertices"] = VerticesSection;
			
			foreach (var point in Points)
				VerticesSection.Add(FormatHelper.VertexToJson(point));

			return Data;
			}
		
		public Wall Clone()
			{
			var Wall = new Wall
				{
				Color = Color
				};

			foreach (var point in Points)
				Wall.Points.Add(point);

			return Wall;
			}
		
		public IEnumerable<ValidationProblem> Validate(Boolean fix)
			{
			// Need at least a triangle for winding to be meaningful.
			if (Points.Count < 3)
				return Enumerable.Empty<ValidationProblem>();

			// Shoelace formula (signed area). In a standard Cartesian coordinate system:
			//  - area > 0 => counter-clockwise
			//  - area < 0 => clockwise
			Double SignedArea2 = 0;

			for (Int32 i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
				{
				var pj = Points[j];
				var pi = Points[i];
				SignedArea2 += (pj.X * pi.Y) - (pi.X * pj.Y);
				}

			var IsClockwise = SignedArea2 < 0;

			if (IsClockwise)
				return Enumerable.Empty<ValidationProblem>();

			if (fix)
				Points.Reverse();

			return [new ValidationProblem("Wall has wrong winding order", fix)];
			}
		}
	}
