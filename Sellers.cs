using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Gtk;
using System.Data.SQLite;
namespace OnlineBookShop
{
    public partial class Sellers : Gtk.Window
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

        public Sellers() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            showall();
        }

        private void showall()
        {
            TreeViewColumn title = new TreeViewColumn();

            TreeViewColumn SellerID = new TreeViewColumn();
            SellerID.Title = "SellId";

            TreeViewColumn SellerName = new TreeViewColumn();
            SellerName.Title = "SelName";

            TreeViewColumn SellerEmail = new TreeViewColumn();
            SellerEmail.Title = "SelEmail";

            TreeViewColumn SellerPhone = new TreeViewColumn();
            SellerPhone.Title = "SelPhone";

            TreeViewColumn SellerPassword = new TreeViewColumn();
            SellerPassword.Title = "SelPass";

            if (count == 0)
            {
                treeview2.AppendColumn(SellerID);
                treeview2.AppendColumn(SellerName);
                treeview2.AppendColumn(SellerEmail);
                treeview2.AppendColumn(SellerPhone);
                treeview2.AppendColumn(SellerPassword);
                count++;
            }
            musicListStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

            CellRendererText SIDCell = new CellRendererText();
            SellerID.PackStart(SIDCell, true);
            CellRendererText SNameCell = new CellRendererText();
            SellerName.PackStart(SNameCell, true);
            CellRendererText SEmailCell = new CellRendererText();
            SellerEmail.PackStart(SEmailCell, true);
            CellRendererText SellerPhoneCell = new CellRendererText();
            SellerPhone.PackStart(SellerPhoneCell, true);
            CellRendererText SellerPasswordCell = new CellRendererText();
            SellerPassword.PackStart(SellerPasswordCell, true);



            SellerID.AddAttribute(SIDCell, "text", 0);
            SellerName.AddAttribute(SNameCell, "text", 1);
            SellerEmail.AddAttribute(SEmailCell, "text", 2);
            SellerPhone.AddAttribute(SellerPhoneCell, "text", 3);
            SellerPassword.AddAttribute(SellerPasswordCell, "text", 4);


            query = "SELECT * FROM Sellers";
            executequery(query);
            sqlconn.Open();
            using (var reader = sqlcmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var Sid = reader.GetString(0);
                    var Sname = reader.GetString(1);
                    var Semail = reader.GetString(2);
                    var Sphone = reader.GetString(3);
                    var Spassword = reader.GetString(4);

                    musicListStore.AppendValues(Sid,Sname,Semail,Sphone,Spassword);
                }
            }
            treeview2.Model = musicListStore;
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

        protected void Catogries(object sender, EventArgs e)
        {
            this.Hide();
            Category category = new Category();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            category.ModifyBg(StateType.Normal, col);
            category.Show();
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

        protected void Bookd(object sender, EventArgs e)
        {
            this.Hide();
            Window1 window1 = new Window1();
            Gdk.Color col = new Gdk.Color();
            Gdk.Color.Parse("gray", ref col);
            window1.ModifyBg(StateType.Normal, col);
            window1.Show();
        }

        private void createtable()
        {
            query = "CREATE TABLE IF NOT EXISTS Sellers(SellerID integer primary key autoincrement,SellerName varchar(50),SellerEmail VARCHAR(50),SellerPhone VARCHAR(50),SellerPassword VARCHAR(50) )";
            executequery(query);
        }

        private void cleardata()
        {
            entry4.Text = "";
            entry5.Text = "";
            entry6.Text = "";
            entry7.Text = "";
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

        protected void savebtn(object sender, EventArgs e)
        {
            createtable();
            query = "INSERT INTO Sellers(SellerName,SellerEmail,SellerPhone,SellerPassword) VALUES('" + entry4.Text + "','" + entry5.Text + "','" + entry6.Text + "','" + entry7.Text+ "')"; 
            executequery(query);
            showall();
            cleardata();
        }

        protected void Updatedbtn(object sender, EventArgs e)
        {
            createtable();
            query = "update Sellers set SellerName='" + entry4.Text + "',SellerEmail='" + entry5.Text + "',SellerPhone='" + entry6.Text + "',SellerPassword='" + entry7.Text + "' where SellerID='" + musicListStore.GetValue(selected, 0) + "'";
            executequery(query);
            showall();
            cleardata();
        }

        protected void Deletebtn(object sender, EventArgs e)
        {
            createtable();
            md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.YesNo, "هل حقا نريد جذف هذا العنصر؟");
            ResponseType response = (ResponseType)md.Run();
            md.Show();
            if (response == ResponseType.No)
                md.Destroy();
            else
            {
                query = "delete from Sellers where SellerID='" + musicListStore.GetValue(selected, 0) + "'";
                executequery(query);
                md.Destroy();
            }

            showall();
            cleardata();
        }

        protected void clk(object sender, EventArgs e)
        {
            if (treeview2.Selection.GetSelected(out selected))
            {
                entry4.Text = (string)musicListStore.GetValue(selected, 1);
                entry5.Text = (string)musicListStore.GetValue(selected, 2);
                entry6.Text = (string)musicListStore.GetValue(selected, 3);
                entry7.Text = (string)musicListStore.GetValue(selected, 4);

            }
        }
    }
}
