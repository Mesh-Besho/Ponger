using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class ObjectTool : BaseTool
		{
		private OverlayCircle _PointOverlay;
		private Pobject? _NewTemplate;
		
		public ObjectTool(LevelEditor editor)
			: base(editor)
			{
			_PointOverlay = new OverlayCircle(PointF.Empty, Portal.Radius);
			}

		public override ToolType Type => ToolType.Portal;

		public override IEnumerable<ToolItem> GetToolbarItems() => [CreateObjectDropdown()];

		private DropDownToolItem CreateObjectDropdown()
			{
			var Item = new DropDownToolItem();

			foreach (var template in Pobject.Templates)
				{
				if (_NewTemplate == null)
					_NewTemplate = template;
				
				var IsSelected = Equals(template.Id, _NewTemplate?.Id);
				
				Item.Items.Add(new RadioMenuItem(new RadioCommand((s, e) =>
					{
					OnTemplateChosen(template);
					Item.Text = template.Id;
					}), (RadioMenuItem)Item.Items.FirstOrDefault()) { Text = template.Id, Checked =IsSelected });

				if (IsSelected)
					Item.Text = template.Id;
				}

			return Item;
			}

		private void OnTemplateChosen(Pobject template)
			{
			_NewTemplate = template;
			}
		
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

			Editor.InvokeRedraw();
			
			return false;
			}
		
		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			if (button != MouseButtons.Primary)
				return false;
			
			point = Editor.Snap(point);

			Application.Instance.AsyncInvoke(() =>
				{
				var Pobject = new Pobject(_NewTemplate) { Position = point };
				
				Pobject.Id = Editor.RequestString("Enter object ID:");

				if (Pobject.Id == null)
					return;

				Editor.Level.Objects.Add(Pobject);
				
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
