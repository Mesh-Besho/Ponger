using Eto.Drawing;
using Eto.Forms;
using MeshBesho.Ponger.Editor.Ponger;

namespace MeshBesho.Ponger.Editor
	{
	internal class WallTool : BaseTool
		{
		private OverlayLine _CurrentOverlay;
		private readonly Stack<OverlayLine> _Overlays = new Stack<OverlayLine>();

		private readonly Stack<PointF> _Points = new Stack<PointF>();

		private Color _NewColor = Colors.Red;

		public WallTool(LevelEditor editor) : base(editor)
			{
			}
		
		public override ToolType Type => ToolType.Wall;
		
		public override IEnumerable<ToolItem> GetToolbarItems() => [CreateColorDropDown() ];
		
		private DropDownToolItem CreateColorDropDown()
			{
			var Item = new DropDownToolItem { Text = "RED" };

			foreach(var kvp in FormatHelper.KnownColors)
				Item.Items.Add(new RadioMenuItem(new RadioCommand((s, e) =>
					{
						OnColorChosen(kvp.Value);
						Item.Text = kvp.Key;
					}), (RadioMenuItem)Item.Items.FirstOrDefault()) { Text = kvp.Key, Checked = kvp.Value == _NewColor });
			
			if(FormatHelper.KnownColorsReversed.TryGetValue(_NewColor, out var colorName))
				Item.Text = colorName;
				
			return Item;
			}
		private void OnColorChosen(Color color)
			{
			_NewColor = color;
			}

		public override Boolean InvokeMouseDown(MouseButtons button, PointF point)
			{
			point = Snap(point);

			if (button == MouseButtons.Primary)
				{
				CommitPoint(point);
				return true;
				}

			if (button == MouseButtons.Alternate)
				{
				RollbackPoint();

				if (_CurrentOverlay != null)
					_CurrentOverlay.End = point;

				return true;
				}
			
			return false;
			}

		public override Boolean InvokeMouseMove(MouseButtons button, PointF point)
			{
			if (_CurrentOverlay != null)
				{
				point = Snap(point);
				_CurrentOverlay.End = point;
				Editor.InvokeRedraw();
				
				return true;
				}
			
			return false;
			}
		
		public override void Reset()
			{
			ClearOverlays();
			_Points.Clear();
			}

		private void CommitPoint(PointF point)
			{
			if (_Points.Count > 0)
				{
				if (point == _Points.Last())
					{
					OnWallComplete();
					return;
					}

				_CurrentOverlay.End = point;
				_Overlays.Push(_CurrentOverlay);
				}

			_Points.Push(point);

			_CurrentOverlay = new OverlayLine(point, point);
			Editor.AddOverlay(_CurrentOverlay);
			Editor.InvokeRedraw();
			}

		private void RollbackPoint()
			{
			if (_Points.Count < 2)
				{
				Reset();
				return;
				}

			Editor.RemoveOverlay(_CurrentOverlay);
			Editor.InvokeRedraw();

			_Points.Pop();
			_CurrentOverlay = _Overlays.Pop();
			}
		
		private PointF Snap(PointF point) => new PointF((Int32)(point.X / 50) * 50, (Int32)(point.Y / 50) * 50);

		private void OnWallComplete()
			{
			var Wall = new Wall(_Points)
				{
				Color = _NewColor
				};

			Editor.Level.Walls.Add(Wall);
			Editor.InvokeRedraw();

			Reset();
			}

		private void ClearOverlays()
			{
			foreach (var overlay in _Overlays)
				Editor.RemoveOverlay(overlay);

			if (_CurrentOverlay != null)
				Editor.RemoveOverlay(_CurrentOverlay);

			_Overlays.Clear();
			_CurrentOverlay = null;

			Editor.InvokeRedraw();
			}
		}
	}
