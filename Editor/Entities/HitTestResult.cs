namespace MeshBesho.Ponger.Editor.Ponger
	{
	public class HitTestResult
		{
		public static HitTestResult None { get; } = new HitTestResult(null);
		
		public HitTestResult(EditorEntity entity)
			{
			Entity = entity;
			}
		
		public EditorEntity? Entity { get; }
		}
	}
