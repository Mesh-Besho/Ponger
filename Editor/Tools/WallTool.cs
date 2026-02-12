using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class WallTool : RegionBaseTool
		{
		private Color _NewColor = Colors.Red;

		public WallTool(LevelEditor editor)
			: base(editor)
			{
			}

		public override ToolType Type => ToolType.Wall;

		public override IEnumerable<ToolItem> GetToolbarItems() => [CreateColorDropDown()];

		private DropDownToolItem CreateColorDropDown()
			{
			var Item = new DropDownToolItem { Text = "RED" };

			foreach (var kvp in FormatHelper.KnownColors)
				Item.Items.Add(new RadioMenuItem(new RadioCommand((s, e) =>
					{
						OnColorChosen(kvp.Value);
						Item.Text = kvp.Key;
					}), (RadioMenuItem)Item.Items.FirstOrDefault()) { Text = kvp.Key, Checked = kvp.Value == _NewColor });

			if (FormatHelper.KnownColorsReversed.TryGetValue(_NewColor, out var colorName))
				Item.Text = colorName;

			return Item;
			}

		private void OnColorChosen(Color color)
			{
			_NewColor = color;
			}

		protected override void OnRegionComplete(PointF[] points)
			{
			var Wall = new Wall(points)
				{
				Color = _NewColor
				};

			Editor.Level.Walls.Add(Wall);
			Editor.InvokeRedraw();

			Reset();
			}
		}
	}
