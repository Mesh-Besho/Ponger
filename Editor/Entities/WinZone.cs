using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public class WinZone : EditorEntity, IRenderable
		{
		private static readonly Color _SelectHighlight = new Color(Colors.White, 0.5f);
		private static readonly Brush _RenderBrush = new SolidBrush(new Color(Colors.MediumPurple, 0.5f));

		public WinZone(IEnumerable<PointF>? points = null)
			{
			if (points != null)
				Points.AddRange(points);
			}

		public List<PointF> Points { get; } = new List<PointF>();

		public void Render(Graphics graphics, RenderFlags flags)
			{
			graphics.FillPolygon(_RenderBrush, Points.ToArray());

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

			var WallPoint = Points[pointIndex];

			const Single Radius = 10f;
			const Single RadiusSquared = Radius * Radius;

			var Dx = testPoint.X - WallPoint.X;
			var Dy = testPoint.Y - WallPoint.Y;

			if ((Dx * Dx) + (Dy * Dy) <= RadiusSquared)
				{
				result = new WinZoneHitTestResult(this, pointIndex);
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
				result = new WinZoneHitTestResult(this);
				return true;
				}

			return false;
			}

		public static WinZone FromJson(JsonObject json)
			{
			var Zone = new WinZone();

			Zone.Points.Add(FormatHelper.VertexFromJson(json["TLC"].AsObject()));
			Zone.Points.Add(FormatHelper.VertexFromJson(json["TRC"].AsObject()));
			Zone.Points.Add(FormatHelper.VertexFromJson(json["BRC"].AsObject()));
			Zone.Points.Add(FormatHelper.VertexFromJson(json["BLC"].AsObject()));
			
			return Zone;
			}

		public JsonObject ToJson()
			{
			var Data = new JsonObject();

			if(Points.Count != 4)
				throw new Exception("WinZone must have exactly 4 points");
			
			Data["TLC"] = FormatHelper.VertexToJson(Points[0]);
			Data["TRC"] = FormatHelper.VertexToJson(Points[1]);
			Data["BRC"] = FormatHelper.VertexToJson(Points[2]);
			Data["BLC"] = FormatHelper.VertexToJson(Points[3]);

			return Data;
			}
		
		public WinZone Clone()
			{
			var Zone = new WinZone();

			foreach (var point in Points)
				Zone.Points.Add(point);

			return Zone;
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

			return [new ValidationProblem("WinZone has wrong winding order", fix)];
			}
		}
	}
