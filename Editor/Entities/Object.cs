using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public class Pobject : EditorEntity, IRenderable
		{
		public static Pobject[] Templates { get; }

		static Pobject()
			{
			var ShedKeyTemplate = new Pobject
				{
				Id = "ShedKey",
				Type = "key",
				Texture = "shed_key.png",
				Color = FormatHelper.KnownColors["GOLD"]
				};

			var HouseKeyTemplate = new Pobject(ShedKeyTemplate)
				{
				Id = "HouseKey",
				Texture = "house_key.png"
				};

			var MagnetPowerUp = new Pobject()
				{
				Id = "MagnetPowerUp",
				Type = "MouseMagnetPowerup",
				Texture = "mouse_magnet_powerup.png",
				Color = FormatHelper.KnownColors["WHITE"]
				};
			
			Templates = new Pobject[]
				{
				ShedKeyTemplate, HouseKeyTemplate,
				MagnetPowerUp
				};
			}

		public Pobject()
			{
			}
		
		public Pobject(Pobject? template)
			{
			if (template == null)
				return;
			
			Id = template.Id;
			Type = template.Type;
			Position = template.Position;
			Texture = template.Texture;
			Color = template.Color;
			}
		
		public String Id { get; set; }
		public String Type { get; set; }
		public String Texture { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }

		public void Render(Graphics graphics, RenderFlags flags)
			{
			var ActualTexture = Textures.GetEmbeddedImage(Texture);

			// TODO: Supposed to be the image tinted by the color, but don't know how to do that in Eto.
			// So just draw the color and then the texture on top.
			
			graphics.FillRectangle(Brushes.Cached(Color), GetBoundingRectangle());
			
			if (ActualTexture != null)
				graphics.DrawImage(ActualTexture, GetBoundingRectangle());
			
			graphics.DrawText(SystemFonts.Default(), Brushes.Black, Position, Id);
			}
		
		internal static Pobject FromJson(JsonObject json)
			{
			var Pobject = new Pobject();

			Pobject.Id = json["obj_id"].GetValue<String>();
			Pobject.Type = json["type"].GetValue<String>();
			Pobject.Position = FormatHelper.VertexFromJson(json["pos"].AsObject());
			Pobject.Texture = json["texture"].GetValue<String>();
			Pobject.Color = FormatHelper.ParseColor(json["color"].GetValue<String>());
			
			return Pobject;
			}
		
		public JsonNode? ToJson()
			{
			var Data = new JsonObject();

			Data["obj_id"] = Id;
			Data["type"] = Type;
			Data["pos"] = FormatHelper.VertexToJson(Position);
			Data["texture"] = Texture;
			Data["color"] =  FormatHelper.FormatColor(Color);
			
			return Data;
			}
		
		public RectangleF GetBoundingRectangle(Single radius = 10)
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
