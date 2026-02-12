using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public class Portal : EditorEntity, IRenderable
		{
		public const Single Radius = 10;
		
		public String Name { get; set; }
		public PointF Position { get; set; }
		public Portal Destination { get; set; }

		public void Render(Graphics graphics, RenderFlags flags)
			{
			var Pen = flags.HasFlag(RenderFlags.Selected) ? Pens.Red : Pens.Pink;

			for (var r = Radius; r > 0; r -= 2)
				graphics.DrawEllipse(Pen, GetBoundingRectangle(r));

			if (Destination != null)
				{
				graphics.DrawLine(Pens.DeepPink, Position, Destination.Position);
				}
			
			graphics.DrawText(SystemFonts.Default(), Brushes.Black, Position, Name);
			}
		
		internal static Portal FromJson(JsonObject json, ILevelLoader loader)
			{
			var Portal = new Portal();

			Portal.Name = json["name"].GetValue<String>();
			Portal.Position = FormatHelper.VertexFromJson(json["pos"].AsObject());

			// TODO: Radius

			// Destination is by name but we want the instance. Match it up later
			var DestinationName = json["destination"].GetValue<String>();
			loader.AddFixup(l => Portal.Destination = l.Portals.FirstOrDefault(p => String.Equals(p.Name, DestinationName, StringComparison.OrdinalIgnoreCase)));
			
			return Portal;
			}
		
		public JsonNode? ToJson()
			{
			var Data = new JsonObject();

			Data["name"] = Name;
			Data["pos"] = FormatHelper.VertexToJson(Position);
			Data["destination"] = Destination?.Name ?? String.Empty;
			
			// TODO: Radius

			return Data;
			}
		
		public RectangleF GetBoundingRectangle(Single radius = Radius)
			{
			return new RectangleF(Position.X - radius, Position.Y - radius, radius * 2, radius * 2);
			}
		
		public Boolean HitTest(PointF point, out HitTestResult result)
			{
			if (GetBoundingRectangle().Contains(point))
				{
				result = new HitTestResult(this);
				return true;
				}
			
			result = HitTestResult.None;
			return false;
			}
		}
	}
