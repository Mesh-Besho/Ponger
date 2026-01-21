using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class LevelRenderPanel : Drawable
		{
		public LevelEditor Editor { get; set; }

		protected override void OnPaint(PaintEventArgs e)
			{
			e.Graphics.Clear(Colors.White);

			Editor.Render(e.Graphics);
			}
		}
	}
