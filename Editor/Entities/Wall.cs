using Eto.Drawing;

namespace MeshBesho.Ponger.Editor.Ponger
	{
	public class Wall : EditorEntity, IRenderable
		{
		private static readonly Color _SelectHighlight = new Color(Colors.White, 0.5f);

		public Wall(IEnumerable<PointF> points = null)
			{
			if (points != null)
				Points = new List<PointF>(points);
			}
		public List<PointF> Points { get; }
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
		}
	}
