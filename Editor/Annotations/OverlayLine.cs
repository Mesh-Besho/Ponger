using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	internal class OverlayLine : IRenderable
		{
		public OverlayLine() { }

		public OverlayLine(PointF start, PointF end)
			{
			Start = start;
			End = end;
			}

		public PointF Start { get; set; }
		public PointF End { get; set; }

		public void Render(Graphics graphics, RenderFlags flags)
			{
			graphics.DrawLine(Colors.Black, Start, End);
			}
		}
	}
