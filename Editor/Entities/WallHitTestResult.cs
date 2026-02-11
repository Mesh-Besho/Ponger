namespace MeshBesho.Ponger.Editor.Ponger
	{
	public class WallHitTestResult : HitTestResult
		{
		public new Wall Entity => base.Entity as Wall;
		
		public Int32? PointIndex { get; set; }
		
		public WallHitTestResult(Wall wall, Int32? pointIndex = null)
			: base(wall)
			{
			PointIndex = pointIndex;
			}
		}
	}
