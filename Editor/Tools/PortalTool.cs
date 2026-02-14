using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class PortalTool : BaseTool
		{
		private OverlayCircle _PointOverlay;
		private OverlayLine _LinkOverlay;

		private Portal? _FirstPortal;
		
		public PortalTool(LevelEditor editor)
			: base(editor)
			{
			_PointOverlay = new OverlayCircle(PointF.Empty, Portal.Radius);
			_LinkOverlay = new OverlayLine(PointF.Empty, PointF.Empty);
			}

		public override ToolType Type => ToolType.Portal;

		public override void OnActivated()
			{
			Editor.AddOverlay(_PointOverlay);
			}

		public override void OnDeactivated()
			{
			Editor.RemoveOverlay(_PointOverlay);
			//Reset();
			}
		
		public override Boolean InvokeMouseMove(MouseButtons button, PointF point)
			{
			point = Editor.Snap(point);

			_PointOverlay.MoveCenter(point);

			if (_FirstPortal != null)
				_LinkOverlay.End = point;

			Editor.InvokeRedraw();
			
			return false;
			}
		
		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			if (button == MouseButtons.Alternate)
				{
				if (_FirstPortal != null)
					{
					_FirstPortal = null;
					Editor.RemoveOverlay(_LinkOverlay);
					return true;
					}

				return false;
				}

			if (button != MouseButtons.Primary)
				return false;
			
			point = Editor.Snap(point);

			Application.Instance.AsyncInvoke(() =>
				{
				var Portal = new Portal { Position = point };
				
				using(var dialog = new PortalPropertiesDialog(Editor, Portal))
					if (dialog.ShowModal() == DialogResult.Cancel)
						return;
				
				Editor.Level.Portals.Add(Portal);

				if (Portal.Destination == null)
					{
					if (_FirstPortal == null)
						{
						_FirstPortal = Portal;
						_LinkOverlay.Start = _LinkOverlay.End = point;
						Editor.AddOverlay(_LinkOverlay);
						}

					else
						{
						_FirstPortal.Destination = Portal;
						_FirstPortal = null;
						Editor.RemoveOverlay(_LinkOverlay);
						}
					}

				Editor.InvokeRedraw();
				});

			return true;
			}

		public override Boolean InvokeMouseUp(MouseButtons button, PointF point)
			{
			if (button != MouseButtons.Primary)
				return false;
			
			return true;
			}
		}
	}
