using Eto.Drawing;
using MeshBesho.Ponger.Editor.Ponger;

namespace MeshBesho.Ponger.Editor
	{
	internal class Level
		{
		public List<Wall> Walls { get; } = new List<Wall>();

		public IEnumerable<IRenderable> GetRenderables()
			{
			foreach (var wall in Walls)
				yield return wall;
			}

		public void Render(Graphics graphics)
			{
			foreach (var wall in Walls)
				wall.Render(graphics, RenderFlags.None);
			}

		public EditorEntity HitTest(PointF point)
			{
			foreach (var wall in Walls)
				if (wall.GetBoundingRectangle().Contains(point))
					return wall;

			return null;
			}
		}
	}
