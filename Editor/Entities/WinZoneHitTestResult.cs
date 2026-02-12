namespace MeshBesho.Ponger.Editor
	{
	public class WinZoneHitTestResult : HitTestResult
		{
		public new WinZone Entity => base.Entity as WinZone;
		
		public Int32? PointIndex { get; set; }
		
		public WinZoneHitTestResult(WinZone wall, Int32? pointIndex = null)
			: base(wall)
			{
			PointIndex = pointIndex;
			}
		}
	}
