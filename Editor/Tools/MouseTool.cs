using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class MouseTool : BaseTool
		{
		private EditorEntity MouseDownEntity;

		public MouseTool(LevelEditor editor)
			: base(editor)
			{
			}

		public override ToolType Type => ToolType.Mouse;

		public override IEnumerable<ToolItem> GetToolbarItems() =>
				[new ButtonToolItem { Text = "Delete" }];

		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			if (button == MouseButtons.Primary)
				{
				MouseDownEntity = Editor.HitTest(point);
				Editor.SelectedEntity = MouseDownEntity;
				return true;
				}
			
			return false;
			}
		}
	}
