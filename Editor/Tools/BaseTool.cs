using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal abstract class BaseTool
		{
		public BaseTool(LevelEditor editor)
			{
			Editor = editor;
			}
		
		public LevelEditor Editor { get; }
		
		public abstract ToolType Type { get; }

		public virtual Boolean InvokeMouseUp(MouseButtons button, PointF point) => false;

		public virtual Boolean InvokeMouseDown(MouseButtons button, PointF point) => false;

		public virtual Boolean InvokeMouseMove(MouseButtons button, PointF point) => false;

		public virtual void OnActivated() { }
		
		public virtual void OnDeactivated() { }

		public virtual IEnumerable<ToolItem> GetToolbarItems() => Enumerable.Empty<ToolItem>();
		}
	}
