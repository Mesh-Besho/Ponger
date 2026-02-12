namespace MeshBesho.Ponger.Editor
	{
	internal interface ILevelLoader
		{
		void AddFixup(Action<Level> fixup);
		}
	}
