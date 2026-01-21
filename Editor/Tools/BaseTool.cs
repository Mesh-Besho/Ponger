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

		public virtual void InvokeMouseUp(MouseButtons button, PointF point) { }

		public virtual void InvokeMouseDown(MouseButtons button, PointF point) { }

		public virtual void InvokeMouseMove(MouseButtons button, PointF point) { }

		public virtual void Reset() { }

		public virtual IEnumerable<ToolItem> GetToolbarItems() => Enumerable.Empty<ToolItem>();
		}
	}
