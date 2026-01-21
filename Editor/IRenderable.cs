using Eto.Drawing;

namespace MeshBesho.Ponger.Editor.Ponger
	{
	public interface IRenderable
		{
		public void Render(Graphics graphics, RenderFlags flags);
		}
	}
