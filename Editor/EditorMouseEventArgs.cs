using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class EditorMouseEventArgs
		{
		public Camera Camera { get; }
		public PointF ScreenPosition { get; }
		public PointF WorldPosition { get; }
		public MouseButtons Buttons { get; }

		public EditorMouseEventArgs(Camera camera, PointF screenPosition, MouseButtons buttons)
			{
			Camera = camera;
			ScreenPosition = screenPosition;
			WorldPosition = camera.TransformInverse(screenPosition);
			Buttons = buttons;
			}
		}
	}