using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	internal class Camera
		{
		private IMatrix _Matrix;
		private IMatrix _Inverse;

		public PointF Origin
			{
			get => _Origin;
			set
				{
				_Origin = value;
				UpdateMatrix();
				}
			} private PointF _Origin;
		
		public Single Scale
			{
			get => _Scale;
			set
				{
				_Scale = value;
				UpdateMatrix();
				}
			} private Single _Scale;
		
		private void UpdateMatrix()
			{
			var New = Matrix.Create();

			New.Scale(Scale);
			New.Translate(-Origin.X, -Origin.Y);

			_Matrix = New;
			_Inverse = Matrix.Inverse(New);
			}

		public Camera()
			{
			_Origin = PointF.Empty;
			_Scale = 1;
			
			UpdateMatrix();
			}
		
		public IMatrix GetMatrix() => _Matrix;
		
		public RectangleF Transform(RectangleF rectangle) => _Matrix.TransformRectangle(rectangle);
		public PointF Transform(PointF point) => _Matrix.TransformPoint(point);

		public RectangleF TransformInverse(RectangleF rectangle) => _Inverse.TransformRectangle(rectangle);
		public PointF TransformInverse(PointF point) => _Inverse.TransformPoint(point);
		}
	}
