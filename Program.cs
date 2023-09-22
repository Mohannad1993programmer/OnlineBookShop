using System;
using Gtk;

namespace OnlineBookShop
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            win.ModifyBg(StateType.Normal, col);
            win.Show();
            Application.Run();
          
        }
    }
}
