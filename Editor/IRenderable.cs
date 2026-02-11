using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public interface IRenderable
		{
		public void Render(Graphics graphics, RenderFlags flags);
		}
	}
