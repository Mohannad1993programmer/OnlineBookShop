using System;
using System.Data;
using Gtk;
using Microsoft.Data.Sqlite;

namespace OnlineBookShop
{
    public partial class Author : Gtk.Window
    {
        private ListStore musicListStore;
        private int count = 0;
        private SqliteConnection sqlconn;
        private SqliteCommand sqlcmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        MessageDialog md;
        string query;
        TreeIter selected;

        public Author() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            showall();
        }

        private void showall()
        {
            TreeViewColumn title = new TreeViewColumn();

            TreeViewColumn AuthorID = new TreeViewColumn();
            AuthorID.Title = "AutId";

            TreeViewColumn AuthorName = new TreeViewColumn();
            AuthorName.Title = "AutName";

            TreeViewColumn AuthorGender = new TreeViewColumn();
            AuthorGender.Title = "AutGender";

            TreeViewColumn AuthorCountry = new TreeViewColumn();
            AuthorCountry.Title = "AutCountry";


            if (count == 0)
            {
                treeview1.AppendColumn(AuthorID);
                treeview1.AppendColumn(AuthorName);
                treeview1.AppendColumn(AuthorGender);
                treeview1.AppendColumn(AuthorCountry);
                count++;
            }
            musicListStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

            CellRendererText AIDCell = new CellRendererText();
            AuthorID.PackStart(AIDCell, true);
            CellRendererText ANameCell = new CellRendererText();
            AuthorName.PackStart(ANameCell, true);
            CellRendererText AuthorGenderCell = new CellRendererText();
            AuthorGender.PackStart(AuthorGenderCell, true);
            CellRendererText AuthorCountryCell = new CellRendererText();
            AuthorCountry.PackStart(AuthorCountryCell, true);



            AuthorID.AddAttribute(AIDCell, "text", 0);
            AuthorName.AddAttribute(ANameCell, "text", 1);
            AuthorGender.AddAttribute(AuthorGenderCell, "text", 2);
            AuthorCountry.AddAttribute(AuthorCountryCell, "text", 3);


            query = "SELECT * FROM Author";
            executequery(query);
            sqlconn.Open();
            using (var reader = sqlcmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var Aid = reader.GetString(0);
                    var Aname = reader.GetString(1);
                    var Agender = reader.GetString(2);
                    var Acountry = reader.GetString(3);

                    musicListStore.AppendValues(Aid, Aname, Agender,Acountry);
                }
            }
            treeview1.Model = musicListStore;
        }


        protected void Logout(object sender, EventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            mainWindow.ModifyBg(StateType.Normal, col);
            mainWindow.Show();
        }

      
        protected void Books(object sender, EventArgs e)
        {
            this.Hide();
            Window1 window1 = new Window1();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            window1.ModifyBg(StateType.Normal, col);
            window1.Show();
        }

        protected void Categories(object sender, EventArgs e)
        {
            this.Hide();
            Category category = new Category();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            category.ModifyBg(StateType.Normal, col);
            category.Show();
        }


        private void createtable()
        {
            query = "CREATE TABLE IF NOT EXISTS Author(AuthorID integer primary key autoincrement,AuthorName varchar(50),AuthorGender VARCHAR(50),AuthorCountry VARCHAR(50))";
            executequery(query);
        }

        private void cleardata()
        {
            entry1.Text = "";
            entry2.Text = "";
            entry3.Text = "";
        }

        private void setconnection()
        {
            sqlconn = new SqliteConnection("data source=OnlineBookShop.db");
        }

        private void executequery(string txtquery)
        {
            setconnection();
            sqlconn.Open();
            sqlcmd = sqlconn.CreateCommand();
            sqlcmd.CommandText = txtquery;
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();
        }

        protected void Update(object sender, EventArgs e)
        {
            createtable();
            query = "update Author set AuthorName='" + entry1.Text + "',AuthorGender='" + entry2.Text + "',AuthorCountry='" + entry3.Text + "' where AuthorID='" + musicListStore.GetValue(selected, 0) + "'";
            executequery(query);
            showall();
            cleardata();
        }

        protected void Save(object sender, EventArgs e)
        {
            createtable();
            query = "INSERT INTO Author(AuthorName,AuthorGender,AuthorCountry) VALUES('" + entry1.Text + "','" + entry2.Text + "','"+ entry3.Text + "')";
            executequery(query);
            showall();
            cleardata();
        }

        protected void Delete(object sender, EventArgs e)
        {
            createtable();
            md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.YesNo, "هل حقا نريد جذف هذا العنصر؟");
            ResponseType response = (ResponseType)md.Run();
            md.Show();
            if (response == ResponseType.No)
                md.Destroy();
            else
            {
                query = "delete from Author where AuthorID='" + musicListStore.GetValue(selected, 0) + "'";
                executequery(query);
                md.Destroy();
            }

            showall();
            cleardata();
        }

        protected void clk(object sender, EventArgs e)
        {
            if (treeview1.Selection.GetSelected(out selected))
            {
                entry1.Text = (string)musicListStore.GetValue(selected, 1);
                entry2.Text = (string)musicListStore.GetValue(selected, 2);
                entry3.Text= (string)musicListStore.GetValue(selected, 3);
            }
        }

        protected void Sellers(object sender, EventArgs e)
        {
            this.Hide();
            Sellers sellers = new Sellers();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            sellers.ModifyBg(StateType.Normal, col);
            sellers.Show();
        }
    }
}
