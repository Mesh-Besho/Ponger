using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	internal class PortalPropertiesDialog : PropertiesDialog<Portal>
		{
		private readonly TextBox _NameTextBox;
		private readonly DropDown _DestinationDropDown;
		
		public PortalPropertiesDialog(LevelEditor editor, Portal portal)
			: base(editor, portal)
			{
			AddProperty("Name", _NameTextBox = new TextBox());
			AddProperty("Destination", _DestinationDropDown = new DropDown { DataStore = GetDestinationItems(editor) });
			}

		private IEnumerable<Object> GetDestinationItems(LevelEditor editor)
			{
			var Items = new List<ListItem>();

			Items.Add(new ListItem { Text = "NONE", Key = "" });
			
			foreach (var portal in editor.Level.Portals)
				{
				if (portal == Target)
					continue;

				Items.Add(new ListItem { Text = portal.Name, Key = portal.Name, Tag = portal });
				}

			return Items;
			}

		protected override void Populate()
			{
			_NameTextBox.Text = Target.Name;
			_DestinationDropDown.SelectedKey = Target.Destination?.Name ?? String.Empty;
			}

		protected override IEnumerable<String> Validate()
			{
			if (String.IsNullOrWhiteSpace(_NameTextBox.Text))
				yield return "Name cannot be empty";
			}

		protected override void Commit()
			{
			Target.Name = _NameTextBox.Text;
			Target.Destination = Editor.Level.Portals.FirstOrDefault(p => p.Name == _DestinationDropDown.SelectedKey);
			}
		}
	}
