using Eto.Drawing;
using Eto.Forms;

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
			if (Editor.SelectedEntity is Wall selectedWall)
				{
				Editor.Level.Walls.Remove(selectedWall);
				Editor.SelectedEntity = null;
				}

			Editor.InvokeRedraw();
			}

		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			point = Editor.Snap(point);

			if (button != MouseButtons.Primary)
				return false;

			_MouseDownPosition = point;
			_LastDelta = PointF.Empty;
			MouseDownEntity = Editor.HitTest(point, out var hit) ? hit : null;
			Editor.SelectedEntity = MouseDownEntity?.Entity;

			if (hit is WallHitTestResult wallHit)
				{
				_MoveWallOverlay = OverlayPolygon.FromWall(wallHit.Entity);
				Editor.AddOverlay(_MoveWallOverlay);
				}
			
			else if (hit is WinZoneHitTestResult winZoneHit)
				{
				_MoveWallOverlay = new OverlayPolygon(winZoneHit.Entity.Points);
				Editor.AddOverlay(_MoveWallOverlay);
				}

			return true;
			}

		public override Boolean InvokeMouseUp(MouseButtons button, PointF point)
			{
			if (_MoveWallOverlay == null)
				return false;
			
			if (button != MouseButtons.Primary)
				return false;
			
			Editor.RemoveOverlay(_MoveWallOverlay);
			
			if (MouseDownEntity is WallHitTestResult wallHit)
				{
				if (wallHit.PointIndex.HasValue)
					wallHit.Entity.Points[wallHit.PointIndex.Value] += _LastDelta;

				else
					{
					for (var index = 0; index < wallHit.Entity.Points.Count; index++)
						wallHit.Entity.Points[index] += _LastDelta;
					}
				}
			
			else if (MouseDownEntity is WinZoneHitTestResult winZoneHit)
				{
				if (winZoneHit.PointIndex.HasValue)
					winZoneHit.Entity.Points[winZoneHit.PointIndex.Value] += _LastDelta;

				else
					{
					for (var index = 0; index < winZoneHit.Entity.Points.Count; index++)
						winZoneHit.Entity.Points[index] += _LastDelta;
					}
				}
			
			Editor.InvokeRedraw();

			return true;
			}

		public override Boolean InvokeMouseMove(MouseButtons button, PointF point)
			{
			if (MouseDownEntity == null)
				return false;

			point = Editor.Snap(point);
			
			var Delta = point - _MouseDownPosition;

			if (MouseDownEntity is WallHitTestResult wallHit)
				{
				if (wallHit.PointIndex.HasValue)
					_MoveWallOverlay.MovePoint(wallHit.PointIndex.Value, PointF.Empty - _LastDelta + Delta);	
				
				else
					_MoveWallOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}
			
			if (MouseDownEntity is WinZoneHitTestResult winZoneHit)
				{
				if (winZoneHit.PointIndex.HasValue)
					_MoveWallOverlay.MovePoint(winZoneHit.PointIndex.Value, PointF.Empty - _LastDelta + Delta);	
				
				else
					_MoveWallOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}

			_LastDelta = Delta;
			
			return true;
			}
		}
	}
