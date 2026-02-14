using System.ComponentModel;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal abstract class PropertiesDialog<T> : Dialog<DialogResult>
		{
		private readonly TableLayout _Layout;
		
		public LevelEditor Editor { get; }
		public T Target { get; }
		
		protected PropertiesDialog(LevelEditor editor, T target)
			{
			Editor = editor;
			Target = target;
			
			Content = _Layout = new TableLayout();
			
			PositiveButtons.Add(DefaultButton = new Button(InvokeOkay) { Text = "Okay" });
			NegativeButtons.Add(AbortButton = new Button((s, e) => Close(DialogResult.Cancel)) { Text = "Cancel" });
			}

		protected override void OnClosing(CancelEventArgs e)
			{
			// Eto.Gtk doesn't 'press' AbortButton if ESC is pressed.
			if(Result == DialogResult.None)
				Result = DialogResult.Cancel;
			
			base.OnClosing(e);
			}

		protected override void OnLoad(EventArgs e)
			{
			base.OnLoad(e);

			_Layout.Rows.Add(null);
			
			Populate();
			}
		
		protected abstract void Populate();

		private void InvokeOkay(Object? sender, EventArgs e)
			{
			var Errors = Validate()?.ToArray();
			
			if(Errors != null && Errors.Length > 0)
				{
				Application.Instance.AsyncInvoke(() => MessageBox.Show(this, String.Join(Environment.NewLine, Errors), "Error", MessageBoxButtons.OK));
				return;
				}

			Commit();
			Close(DialogResult.Ok);
			}
		
		protected virtual IEnumerable<String> Validate()
			{
			yield break;
			}

		protected abstract void Commit();

		protected void AddProperty(String label, Control control)
			{
			_Layout.Rows.Add(new TableRow(label, control));
			}
		}
	}
