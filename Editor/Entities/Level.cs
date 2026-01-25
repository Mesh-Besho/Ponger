using System.Text.Json;
using System.Text.Json.Nodes;
using Eto.Drawing;
using MeshBesho.Ponger.Editor.Ponger;

namespace MeshBesho.Ponger.Editor
	{
	internal class Level
		{
		public List<Wall> Walls { get; } = new List<Wall>();
		public List<Door> Doors { get; } = new List<Door>();

		public IEnumerable<IRenderable> GetRenderables()
			{
			foreach (var wall in Walls)
				yield return wall;
			
			foreach (var door in Doors)
				yield return door;
			}

		public void Render(Graphics graphics)
			{
			foreach (var wall in Walls)
				wall.Render(graphics, RenderFlags.None);
			}

		public Boolean HitTest(PointF point, out HitTestResult result)
			{
			foreach (var wall in Walls)
				if (wall.HitTest(point, out result))
					return true;

			result = HitTestResult.None;
			return false;
			}
		
		public static Level FromJson(JsonObject json)
			{
			var Level = new Level();

			var WallsSection = json["Shapes"] as JsonArray;
			
			foreach(var wallElement in WallsSection)
				Level.Walls.Add(Wall.FromJson(wallElement.AsObject()));
			
			var DoorsSection = json["doors"] as JsonArray;
			
			foreach(var doorElement in DoorsSection)
				Level.Doors.Add(Door.FromJson(doorElement.AsObject()));

			return Level;
			}
		}
	}
