using Eto.Drawing;
using Eto.Forms;
using MeshBesho.Ponger.Editor.Ponger;

namespace MeshBesho.Ponger.Editor
	{
	internal class MouseTool : BaseTool
		{
		private PointF _MouseDownPosition;
		private PointF _LastDelta;
		private HitTestResult MouseDownEntity;
		private OverlayPolygon _MoveWallOverlay;
		
		public MouseTool(LevelEditor editor)
			: base(editor)
			{
			}

		public override ToolType Type => ToolType.Mouse;

		public override IEnumerable<ToolItem> GetToolbarItems() => [new ButtonToolItem(InvokeDelete) { Text = "Delete" }];
		
		private void InvokeDelete(Object? sender, EventArgs e)
			{
			throw new NotImplementedException();
			}

		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			if (button == MouseButtons.Primary)
				{
				_MouseDownPosition = point;
				_LastDelta = PointF.Empty;
				MouseDownEntity = Editor.HitTest(point, out var hit) ? hit : null;
				Editor.SelectedEntity = MouseDownEntity?.Entity;

				if (hit is WallHitTestResult wallHit)
					{
					_MoveWallOverlay = OverlayPolygon.FromWall(wallHit.Entity);
					Editor.AddOverlay(_MoveWallOverlay);
					}
				
				return true;
				}
			
			return false;
			}

		public override Boolean InvokeMouseUp(MouseButtons button, PointF point)
			{
			if (_MoveWallOverlay == null)
				return false;
			
			Editor.RemoveOverlay(_MoveWallOverlay);

			if (MouseDownEntity is WallHitTestResult wallHit)
				{
				for (var index = 0; index < wallHit.Entity.Points.Count; index++)
					wallHit.Entity.Points[index] += _LastDelta;
				}
			
			Editor.InvokeRedraw();

			return true;
			}

		public override Boolean InvokeMouseMove(MouseButtons button, PointF point)
			{
			if (MouseDownEntity == null)
				return false;

			var Delta = point - _MouseDownPosition;

			foreach (var line in _MoveWallOverlay.Lines)
				{
				line.Start = line.Start - _LastDelta + Delta;
				line.End = line.End - _LastDelta + Delta;
				}
			
			_LastDelta = Delta;
			
			return true;

			}
		}
	}
