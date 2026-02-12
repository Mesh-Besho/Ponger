namespace MeshBesho.Ponger.Editor
	{
	public class ValidationProblem
		{
		public ValidationProblem(String message, Boolean isFixed)
			{
			Message = message;
			Fixed = isFixed;
			}
		
		public String Message { get; }
		public Boolean Fixed { get; }
		}
	}
