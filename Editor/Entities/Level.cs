using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using Eto.Drawing;
using MeshBesho.Ponger.Editor;

namespace MeshBesho.Ponger.Editor
	{
	internal class Level
		{
		public List<Wall> Walls { get; } = new List<Wall>();
		public List<Door> Doors { get; } = new List<Door>();
		public List<WinZone> WinZones { get; } = new List<WinZone>();

		public IEnumerable<IRenderable> GetRenderables()
			{
			foreach (var wall in Walls)
				yield return wall;
			
			foreach (var door in Doors)
				yield return door;

			foreach (var winZone in WinZones)
				yield return winZone;
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
			
			foreach (var winZone in WinZones)
				if (winZone.HitTest(point, out result))
					return true;

			result = HitTestResult.None;
			return false;
			}

		public static Level FromJson(JsonObject json)
			{
			var Level = new Level();

			var WallsSection = json["Shapes"] as JsonArray;

			foreach (var wallElement in WallsSection)
				Level.Walls.Add(Wall.FromJson(wallElement.AsObject()));

			var DoorsSection = json["doors"] as JsonArray;

			foreach (var doorElement in DoorsSection)
				Level.Doors.Add(Door.FromJson(doorElement.AsObject()));
			
			var WinZonesSection = json["winzones"] as JsonArray;

			if (WinZonesSection != null)
				foreach (var winZoneElement in WinZonesSection)
					Level.WinZones.Add(WinZone.FromJson(winZoneElement.AsObject()));

			return Level;
			}

		public JsonObject? ToJson()
			{
			var Data = new JsonObject();

			var WallsSection = new JsonArray();
			Data["Shapes"] = WallsSection;

			foreach (var wall in Walls)
				WallsSection.Add(wall.ToJson());
			
			var DoorsSection = new JsonArray();
			Data["doors"] = DoorsSection;

			foreach (var door in Doors)
				DoorsSection.Add(door.ToJson());
			
			var WinZonesSection = new JsonArray();
			Data["winzones"] = WinZonesSection;

			foreach (var winZone in WinZones)
				WinZonesSection.Add(winZone.ToJson());
			
			return Data;
			}

		public IEnumerable<ValidationProblem> Validate(Boolean fix)
			{
			// Check walls
			
			var UselessWalls = new List<Wall>();

			foreach (var wall in Walls)
				{
				var WallBounds = wall.GetBoundingRectangle();

				if (WallBounds.Width == 0 || WallBounds.Height == 0)
					{
					UselessWalls.Add(wall);
					yield return new ValidationProblem("Wall is flat, should be removed", fix);

					continue;
					}

				foreach (var problem in wall.Validate(fix))
					yield return problem;
				}

			foreach (var wall in UselessWalls)
				Walls.Remove(wall);
			
			// Check win zones

			var UselessWinZones = new List<WinZone>();
			
			foreach (var winZone in WinZones)
				{
				var WinZoneBounds = winZone.GetBoundingRectangle();

				if (WinZoneBounds.Width == 0 || WinZoneBounds.Height == 0)
					{
					UselessWinZones.Add(winZone);
					yield return new ValidationProblem("WinZone is flat, should be removed", fix);

					continue;
					}

				foreach (var problem in winZone.Validate(fix))
					yield return problem;
				}
			
			foreach (var winZone in UselessWinZones)
				WinZones.Remove(winZone);
			
			if (WinZones.Count == 0)
				yield return new ValidationProblem("Level must have at least one win zone", false);
			}
		}
	}
