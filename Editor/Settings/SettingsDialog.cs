using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor.Settings
	{
	internal class SettingsDialog : Dialog<DialogResult>
		{
		private readonly CheckBox _GridEnabledCheckBox;
		private readonly CheckBox _GridOnTopCheckBox;
		private readonly NumericStepper _GridSizePicker;
		
		public SettingsDialog()
			{
			Title = "Settings";
			Padding = new Padding(8);
			
			_GridEnabledCheckBox = new CheckBox { Text = "Enabled" };
			_GridOnTopCheckBox = new CheckBox { Text = "On top" };
			_GridSizePicker = new NumericStepper { MinValue = 2, MaxValue = 1000 };

			Content = new StackLayout
				{
				Orientation = Orientation.Vertical,
				HorizontalContentAlignment = HorizontalAlignment.Stretch,
				Spacing = 8,
				Items =
					{
					new GroupBox
						{
						Text = "Grid",
						Content = new TableLayout
							{
							Spacing = new Size(8, 8),
							Rows =
								{
								new TableRow(String.Empty, _GridEnabledCheckBox),
								new TableRow(String.Empty, _GridOnTopCheckBox),
								new TableRow("Size", _GridSizePicker)
								}
							}
						}
					}
				};
			
			PositiveButtons.Add(DefaultButton = new Button(HandleOkayButtonClick) { Text = "OK" });
			NegativeButtons.Add(AbortButton = new Button((s, e) => Close(DialogResult.Cancel)) { Text = "Cancel" });
			}

		protected override void OnLoad(EventArgs e)
			{
			base.OnLoad(e);
			Populate();
			}

		private void HandleOkayButtonClick(Object? sender, EventArgs e)
			{
			Commit();
			Close(DialogResult.Ok);
			}
		
		private void Populate()
			{
			_GridEnabledCheckBox.Checked = Program.Settings.Grid.Enabled;
			_GridOnTopCheckBox.Checked = Program.Settings.Grid.OnTop;
			_GridSizePicker.Value = Program.Settings.Grid.Size;
			}
		
		private void Commit()
			{
			Program.Settings.Grid.Enabled = _GridEnabledCheckBox.Checked ?? false;
			Program.Settings.Grid.OnTop = _GridOnTopCheckBox.Checked ?? false;
			Program.Settings.Grid.Size = (Int32)_GridSizePicker.Value;

			Program.SaveSettings();
			}
		}
	}