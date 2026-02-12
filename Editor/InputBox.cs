using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	public class InputBox : Dialog<DialogResult>
		{
		public static DialogResult ShowDialog(Control parent, String prompt, ref String value)
			{
			var Dialog = new InputBox(prompt) { Value = value };
			
			var Result = Dialog.ShowModal(parent);

			if (Result == DialogResult.Ok)
				value = Dialog.Value;

			return Result;
			}

		private readonly TextBox _InputTextBox;
		
		public String Value
			{
			get => _InputTextBox.Text;
			set => _InputTextBox.Text = value;
			}
		
		private InputBox(String prompt)
			{
			_InputTextBox = new TextBox();
			
			Content = new StackLayout
				{
				Orientation = Orientation.Vertical,
				HorizontalContentAlignment = HorizontalAlignment.Stretch,
				Items =
					{
					prompt,
					_InputTextBox
					}
				};

			Padding = new Padding(8);
			
			PositiveButtons.Add(DefaultButton = new Button((s, e) => Close(DialogResult.Ok)) { Text = "OK" });
			NegativeButtons.Add(AbortButton = new Button((s, e) => Close(DialogResult.Cancel)) { Text = "Cancel" });
			}

		protected override void OnLoadComplete(EventArgs e)
			{
			base.OnLoadComplete(e);
			
			_InputTextBox.SelectAll();
			_InputTextBox.Focus();
			}
		}
	}
