using Eto.Drawing;

namespace MeshBesho.Ponger.Editor
	{
	internal class Camera
		{
		private IMatrix _Matrix;
		private IMatrix _Inverse;

		/// <summary>
		/// Origin point, in world coordinates.
		/// </summary>
		public PointF Origin
			{
			get => _Origin;
			set
				{
				_Origin = value;
				UpdateMatrix();
				}
			} private PointF _Origin;
		
		/// <summary>
		/// Viewport size, in screen coordinates.
		/// </summary>
		public SizeF Size
			{
			get => _Size;
			set
				{
				_Size = value;
				UpdateMatrix();
				}
			} private SizeF _Size;

		/// <summary>
		/// World to screen scale factor.
		/// </summary>
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

			New.Translate(_Size.Width / 2, _Size.Height / 2);
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
		
		/// <summary>World to screen.</summary>
		public RectangleF Transform(RectangleF rectangle) => _Matrix.TransformRectangle(rectangle);

		/// <summary>World to screen.</summary>
		public PointF Transform(PointF point) => _Matrix.TransformPoint(point);

		/// <summary>World to screen.</summary>
		public SizeF Transform(SizeF size) => _Matrix.TransformSize(size);

		/// <summary>Screen to world.</summary>
		public RectangleF TransformInverse(RectangleF rectangle) => _Inverse.TransformRectangle(rectangle);

		/// <summary>Screen to world.</summary>
		public PointF TransformInverse(PointF point) => _Inverse.TransformPoint(point);

		/// <summary>Screen to world.</summary>
		public SizeF TransformInverse(SizeF size) => _Inverse.TransformSize(size);

		/// <summary>
		/// Get the viewport rectangle in world coordinates.
		/// </summary>
		public RectangleF GetWorldViewport()
			{
			var Viewport = GetScreenViewport();
			return TransformInverse(Viewport);
			}

		/// <summary>
		/// Get the viewport rectangle in screen coordinates.
		/// </summary>
		public RectangleF GetScreenViewport()
			{
			var ScreenOrigin = Transform(Origin);
			return new RectangleF(ScreenOrigin, Size) - new SizeF(Size.Width / 2, Size.Height / 2);
			}
		}
	}
