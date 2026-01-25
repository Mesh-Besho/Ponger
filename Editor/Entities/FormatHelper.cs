using System.Text.Json.Nodes;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor.Ponger
	{
	public static class FormatHelper
		{
		public static Color ParseColor(String value)
			{
			if (String.Equals(value, "RED", StringComparison.OrdinalIgnoreCase))
				return Colors.Red;

			return Colors.Lime;
			}
		
		public static PointF VertexFromJson(JsonObject json)
			{
			var X = json["X"].GetValue<Single>();
			var Y = json["Y"].GetValue<Single>();
			
			return new PointF(X, Y);
			}
		}
	}
