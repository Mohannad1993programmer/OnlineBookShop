using System;
using System.Data;
using Gtk;
using Microsoft.Data.Sqlite;

namespace OnlineBookShop
{
    public partial class Category : Gtk.Window
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

        public Category() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            showall();
        }

        private void showall()
        {
            TreeViewColumn title = new TreeViewColumn();

            TreeViewColumn CategoryID = new TreeViewColumn();
            CategoryID.Title = "CatId";

            TreeViewColumn CategoryName = new TreeViewColumn();
            CategoryName.Title = "CatName";

            TreeViewColumn CategoryDescription = new TreeViewColumn();
            CategoryDescription.Title = "CatDescription";
            if (count == 0)
            {
                treeview1.AppendColumn(CategoryID);
                treeview1.AppendColumn(CategoryName);
                treeview1.AppendColumn(CategoryDescription);
                count++;
            }
            musicListStore = new ListStore(typeof(string), typeof(string), typeof(string));

            CellRendererText CIDCell = new CellRendererText();
            CategoryID.PackStart(CIDCell, true);
            CellRendererText CNameCell = new CellRendererText();
            CategoryName.PackStart(CNameCell, true);
            CellRendererText CDescriptionCell = new CellRendererText();
            CategoryDescription.PackStart(CDescriptionCell, true);


            CategoryID.AddAttribute(CIDCell, "text", 0);
            CategoryName.AddAttribute(CNameCell, "text", 1);
            CategoryDescription.AddAttribute(CDescriptionCell, "text", 2);

            query = "SELECT * FROM Category";
            executequery(query);
            sqlconn.Open();
            using (var reader = sqlcmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var Cid = reader.GetString(0);
                    var Cname = reader.GetString(1);
                    var Cdescription = reader.GetString(2);

                    musicListStore.AppendValues(Cid, Cname, Cdescription);
                }
            }
            treeview1.Model = musicListStore;

        }

        private void createtable()
        {
            query = "CREATE TABLE IF NOT EXISTS Category(CategoryID integer primary key autoincrement,CategoryName varchar(50),CategoryDescription VARCHAR(50))";
            executequery(query);
        }

        private void cleardata()
        {

            entry3.Text = "";
            entry4.Text = "";
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
            query = "update Category set CategoryName='" + entry3.Text + "',CategoryDescription='" + entry4.Text + "' where CategoryID='" + musicListStore.GetValue(selected, 0) + "'";
            executequery(query);
            showall();
            cleardata();
        }

        protected void Save(object sender, EventArgs e)
        {
            createtable();
            query = "INSERT INTO Category(CategoryName,CategoryDescription) VALUES('" + entry3.Text + "','" + entry4.Text + "')";
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
                query = "delete from Category where CategoryID='" + musicListStore.GetValue(selected, 0) + "'";
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
                entry3.Text = (string)musicListStore.GetValue(selected, 1);
                entry4.Text = (string)musicListStore.GetValue(selected, 2);
            }
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

        protected void Authors(object sender, EventArgs e)
        {
            this.Hide();
            Author author = new Author();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            author.ModifyBg(StateType.Normal, col);
            author.Show();
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

