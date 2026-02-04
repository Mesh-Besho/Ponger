using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor.Ponger
	{
	public static class FormatHelper
		{
		public static Dictionary<String, Color> KnownColors { get; } = new Dictionary<String, Color>
			{ 
				{ "RED", Colors.Red },
				{ "BROWN", Colors.Brown },
				{ "BLACK", Colors.Black },
				{ "WHITE", Colors.White },
				{ "GOLD", Colors.Gold },
			};
		
		public static Dictionary<Color, String> KnownColorsReversed { get; } = KnownColors.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

		public static Color ParseColor(String value)
			{
			if (KnownColors.TryGetValue(value, out var color))
				return color;
			
			return Colors.Lime;
			}
		
		public static String FormatColor(Color color)
			{
			if (KnownColorsReversed.TryGetValue(color, out var name))
				return name;
			
			return "SAUSAGEBISCUITS";
			}
		
		public static PointF VertexFromJson(JsonObject json)
			{
			var X = json["X"].GetValue<Single>();
			var Y = json["Y"].GetValue<Single>();
			
			return new PointF(X, Y);
			}

		public static JsonObject VertexToJson(PointF point)
			{
			var Data = new JsonObject();
			Data["X"] = point.X;
			Data["Y"] = point.Y;
			return Data;
			}
		}
	}
