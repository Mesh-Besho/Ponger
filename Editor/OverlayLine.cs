using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	internal class OverlayLine
		{
		public OverlayLine(PointF start, PointF end)
			{
			Start = start;
			End = end;
			}

		public PointF Start { get; set; }
		public PointF End { get; set; }

		public void Render(Graphics graphics)
			{
			graphics.DrawLine(Colors.Black, Start, End);
			}
		}
	}
