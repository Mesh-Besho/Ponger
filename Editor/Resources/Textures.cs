using System.Reflection;
using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	public static class Textures
		{
		public static Image ShedKey => GetEmbeddedImage("shed_key.png");
		public static Image HouseKey => GetEmbeddedImage("house_key.png");
		public static Image MagnetPowerUp => GetEmbeddedImage("mouse_magment_powerup.png");

		private static Dictionary<String, Image> _ImageCache = new Dictionary<String, Image>();
		
		public static Image GetEmbeddedImage(String name)
			{
			if (!_ImageCache.TryGetValue(name, out var Image))
				{
				try { _ImageCache.Add(name, Bitmap.FromResource("PongEdit.Resources." + name)); }
				catch
					{
					var ExistingNames = Assembly.GetEntryAssembly().GetManifestResourceNames();
					_ImageCache.Add(name, null);
					}
				}

			return Image;
			}
		}

	}