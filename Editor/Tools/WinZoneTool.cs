using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class WinZoneTool : RegionBaseTool
		{
		public WinZoneTool(LevelEditor editor)
			: base(editor)
			{
			}

		public override ToolType Type => ToolType.Wall;

		protected override void CommitPoint(PointF point)
			{
			base.CommitPoint(point);

			// Win zones have 4 points, so if we now have 4 then just complete the shape.
			if (_Points.Count == 4)
				CommitPoint(_Points.Last());
			}

		protected override void OnRegionComplete(PointF[] points)
			{
			if (points.Length != 4)
				{
				Editor.ShowToolError("Win zones must have exactly 4 points.");
				Reset();
				return;
				}
			
			var Zone = new WinZone(points);

			Editor.Level.WinZones.Add(Zone);
			Editor.InvokeRedraw();

			Reset();
			}
		}
	}
