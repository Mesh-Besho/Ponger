using Eto.Drawing;
using Eto.Forms;

namespace MeshBesho.Ponger.Editor
	{
	public class AboutDialog : Dialog
		{
		public AboutDialog()
			{
			Title = "About";
			MinimumSize = new Size(300, 200);
			Resizable = false;

			var Layout = new DynamicLayout
				{
				Padding = new Padding(20),
				Spacing = new Size(10, 10)
				};

			Layout.AddAutoSized(new Label
				{
				Text = "PongEdit",
				Font = SystemFonts.Bold(14f),
				TextAlignment = TextAlignment.Center
				}, centered: true);

			Layout.AddAutoSized(new Label
				{
				Text = "by allsorts46",
				TextAlignment = TextAlignment.Center
				}, centered: true);
			
			Layout.AddAutoSized(new Label
				{
				Text = "© 2026, Mesh Besho",
				TextAlignment = TextAlignment.Center
				}, centered: true);

			PositiveButtons.Add(DefaultButton = AbortButton = new Button((s, e) => Close()) { Text = "OK" });

			Content = Layout;
			}
		}
	}
