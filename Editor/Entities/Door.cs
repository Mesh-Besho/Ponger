using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	internal class Door : EditorEntity, IRenderable
		{
		public PointF Hinge { get; set; }
		public List<Wall> Walls { get; } = new List<Wall>();
		//public List<Doer> Doers { get; } = new List<Doer>();

		public static Door FromJson(JsonObject json)
			{
			var Door = new Door();

			Door.Hinge = FormatHelper.VertexFromJson(json["hinge"].AsObject());

			var WallsSection = json["walls"].AsArray();

			foreach (var wall in WallsSection)
				Door.Walls.Add(Wall.FromJson(wall.AsObject()));

			return Door;
			}
		
		public JsonNode? ToJson()
			{
			var Data = new JsonObject();

			Data["hinge"] = FormatHelper.VertexToJson(Hinge);
			
			var WallsSection = new JsonArray();
			Data["walls"] = WallsSection;
			
			foreach (var wall in Walls)
				WallsSection.Add(wall.ToJson());

			return Data;
			}
		
		public void Render(Graphics graphics, RenderFlags flags)
			{
			foreach (var wall in Walls)
				{
				var MovedWall = wall.Clone();
				
				for (var index = 0; index < wall.Points.Count; index++)
					MovedWall.Points[index] += Hinge;

				MovedWall.Render(graphics, flags);
				}
			
			graphics.FillEllipse(Brushes.Black, new RectangleF(Hinge - new PointF(2, 2), new SizeF(4, 4)));
			}
		
		public Boolean HitTest(PointF point, out HitTestResult result)
			{
			var Bounds = GetBoundingRectangle();
			Bounds.Inflate(5, 5);

			if (Bounds.Contains(point))
				{
				result = new HitTestResult(this);
				return true;
				}
			
			result = HitTestResult.None;
			return false;
			}

		public RectangleF GetBoundingRectangle()
			{
			var First = true;
			var Bounds = new RectangleF();

			foreach (var wall in Walls)
				foreach (var point in wall.Points)
					{
					if (First || point.X < Bounds.Left)
						Bounds.Left = point.X;

					if (First || point.X > Bounds.Right)
						Bounds.Right = point.X;

					if (First || point.Y < Bounds.Top)
						Bounds.Top = point.Y;

					if (First || point.Y > Bounds.Bottom)
						Bounds.Bottom = point.Y;

					First = false;
					}

			Bounds.Offset(Hinge);
			
			return Bounds;
			}
		}
	}