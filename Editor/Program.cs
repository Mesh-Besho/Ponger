// See https://aka.ms/new-console-template for more information

using MeshBesho.Ponger.Editor;
using Application = Eto.Forms.Application;

namespace MeshBesho.Ponger.Editor
	{
	public static class Program
		{
		[STAThread]
		public static void Main(String[] args)
			{
			new Application().Run(new MainForm());
			}
		}
	}
