using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class MouseTool : BaseTool
		{
		private PointF _MouseDownPosition;
		private PointF _LastDelta;
		private HitTestResult? MouseDownEntity;
		
		private OverlayPolygon _MoveWallOverlay;
		private OverlayRectangle _MoveObjectOverlay;
		
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
			
			else if (Editor.SelectedEntity is Door selectedDoor)
				{
				Editor.Level.Doors.Remove(selectedDoor);
				Editor.SelectedEntity = null;
				}
			
			else if (Editor.SelectedEntity is Portal selectedPortal)
				{
				foreach(var portal in Editor.Level.Portals)
					if (portal.Destination == selectedPortal)
						portal.Destination = null;
				
				Editor.Level.Portals.Remove(selectedPortal);
				Editor.SelectedEntity = null;
				}
			
			else if (Editor.SelectedEntity is Pobject selectedObject)
				{
				Editor.Level.Objects.Remove(selectedObject);
				Editor.SelectedEntity = null;
				}
			
			else if (Editor.SelectedEntity is WinZone selectedWinZone)
				{
				Editor.Level.WinZones.Remove(selectedWinZone);
				Editor.SelectedEntity = null;
				}

			Editor.InvokeRedraw();
			}

		public override Boolean InvokeMouseDown(EditorMouseEventArgs e)
			{
			var HitPositive = Editor.HitTest(e, out var hit);

			if (e.Buttons == MouseButtons.Alternate)
				{
				if(!HitPositive)
					return false;

				InvokeProperties(hit);
				Editor.InvokeRedraw();
				return true;
				}
			
			if (e.Buttons != MouseButtons.Primary)
				return false;
			
			MouseDownEntity = HitPositive ? hit : null;

			var point = Editor.Snap(e.WorldPosition);
			
			_MouseDownPosition = point;
			_LastDelta = PointF.Empty;
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

			else if (hit?.Entity is Door door)
				{
				_MoveObjectOverlay = new OverlayRectangle(door.GetBoundingRectangle());
				Editor.AddOverlay(_MoveObjectOverlay);
				}
			
			else if (hit?.Entity is Portal portal)
				{
				_MoveObjectOverlay = new OverlayRectangle(portal.GetBoundingRectangle());
				Editor.AddOverlay(_MoveObjectOverlay);
				}
			
			else if (hit?.Entity is Pobject pobject)
				{
				_MoveObjectOverlay = new OverlayRectangle(pobject.GetBoundingRectangle());
				Editor.AddOverlay(_MoveObjectOverlay);
				}
			
			return true;
			}
		
		private OverlayHandle HitTestHandles(PointF point)
			{
			foreach(var handle in GetHandles())
				{
				var HitRect = new RectangleF(handle.Point, new SizeF(50, 50) - new SizeF(25, 25));

				if(HitRect.Contains(point))
					return handle;
				}
			return null;
			}

		private void InvokeProperties(HitTestResult hit)
			{
			if (hit.Entity is Portal portal)
				using (var editor = new PortalPropertiesDialog(Editor, portal))
					editor.ShowModal();
			}

		public override Boolean InvokeMouseUp(EditorMouseEventArgs e)
			{
			if (e.Buttons != MouseButtons.Primary)
				return false;
			
			if (MouseDownEntity is WallHitTestResult wallHit)
				{
				Editor.RemoveOverlay(_MoveWallOverlay);
				
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
				Editor.RemoveOverlay(_MoveWallOverlay);
				
				if (winZoneHit.PointIndex.HasValue)
					winZoneHit.Entity.Points[winZoneHit.PointIndex.Value] += _LastDelta;

				else
					{
					for (var index = 0; index < winZoneHit.Entity.Points.Count; index++)
						winZoneHit.Entity.Points[index] += _LastDelta;
					}
				}
			
			else if (MouseDownEntity?.Entity is Door door)
				{
				Editor.RemoveOverlay(_MoveObjectOverlay);

				door.Hinge += _LastDelta;
				}
			
			else if (MouseDownEntity?.Entity is Portal portal)
				{
				Editor.RemoveOverlay(_MoveObjectOverlay);

				portal.Position += _LastDelta;
				}

			else if (MouseDownEntity?.Entity is Pobject pobject)
				{
				Editor.RemoveOverlay(_MoveObjectOverlay);

				pobject.Position += _LastDelta;
				}
			
			else
				return false;
			
			Editor.InvokeRedraw();

			return true;
			}

		public override Boolean InvokeMouseMove(EditorMouseEventArgs e)
			{
			if (MouseDownEntity == null)
				return false;

			var point = Editor.Snap(e.WorldPosition);
			
			var Delta = point - _MouseDownPosition;

			if (MouseDownEntity is WallHitTestResult wallHit)
				{
				if (wallHit.PointIndex.HasValue)
					_MoveWallOverlay.MovePoint(wallHit.PointIndex.Value, PointF.Empty - _LastDelta + Delta);	
				
				else
					_MoveWallOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}
			
			else if (MouseDownEntity is WinZoneHitTestResult winZoneHit)
				{
				if (winZoneHit.PointIndex.HasValue)
					_MoveWallOverlay.MovePoint(winZoneHit.PointIndex.Value, PointF.Empty - _LastDelta + Delta);	
				
				else
					_MoveWallOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}

			else if (MouseDownEntity?.Entity is Door door)
				{
				_MoveObjectOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}
			
			else if (MouseDownEntity?.Entity is Portal portal)
				{
				_MoveObjectOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}
			
			else if (MouseDownEntity?.Entity is Pobject pobject)
				{
				_MoveObjectOverlay.Move(PointF.Empty - _LastDelta + Delta);
				}
			
			_LastDelta = Delta;
			
			return true;
			}

		public override IEnumerable<OverlayHandle> GetHandles()
			{
			return Editor.SelectedEntity?.GetHandles() ?? base.GetHandles();
			}
		}
	}
