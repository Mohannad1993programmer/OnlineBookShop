using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Gtk;

namespace OnlineBookShop
{
    public partial class Billings : Gtk.Window
    {
        private ListStore musicListStore, musicListStore1;
        private int count,count1 = 0;
        private SqliteConnection sqlconn;
        private SqliteCommand sqlcmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        string query;
        TreeIter selected;

        public Billings() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            showall(); 
            showall1();
        }

        private void showall()
        {
            TreeViewColumn title = new TreeViewColumn();

            TreeViewColumn BCode = new TreeViewColumn();
            BCode.Title = "Code";

            TreeViewColumn BName = new TreeViewColumn();
            BName.Title = "Name";

            TreeViewColumn BStock = new TreeViewColumn();
            BStock.Title = "Stock";

            TreeViewColumn BPrice = new TreeViewColumn();
            BPrice.Title = "Price";

            if (count == 0)
            {
                treeview1.AppendColumn(BCode);
                treeview1.AppendColumn(BName);
                treeview1.AppendColumn(BStock);
                treeview1.AppendColumn(BPrice);
                count++;
            }

            musicListStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

            CellRendererText BCodeCell = new CellRendererText();
            BCode.PackStart(BCodeCell, true);
            CellRendererText BNameCell = new CellRendererText();
            BName.PackStart(BNameCell, true);
            CellRendererText BQuantityCell = new CellRendererText();
            BStock.PackStart(BQuantityCell, true);
            CellRendererText BPriceCell = new CellRendererText();
            BPrice.PackStart(BPriceCell, true);

            BCode.AddAttribute(BCodeCell, "text", 0);
            BName.AddAttribute(BNameCell, "text", 1);
            BStock.AddAttribute(BQuantityCell, "text", 2);
            BPrice.AddAttribute(BPriceCell, "text", 3);
       
            query = "SELECT * FROM Book";
            executequery(query);
            sqlconn.Open();
            using (var reader = sqlcmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var Bid = reader.GetString(0);
                    var Bname = reader.GetString(1);
                    var Bquantity = reader.GetString(4);
                    var Bprice = reader.GetString(5);

                    musicListStore.AppendValues(Bid, Bname, Bquantity, Bprice);
                }
            }
            treeview1.Model = musicListStore;
        }

        private void showall1()
        {
            TreeViewColumn title = new TreeViewColumn();

            TreeViewColumn BookID = new TreeViewColumn();
            BookID.Title = "ID";

            TreeViewColumn Bookname = new TreeViewColumn();
            Bookname.Title = "Book";

            TreeViewColumn BookQuantity = new TreeViewColumn();
            BookQuantity.Title = "Quantity";

            TreeViewColumn BookPrice = new TreeViewColumn();
            BookPrice.Title = "Price";

            TreeViewColumn TotalPrice = new TreeViewColumn();
            TotalPrice.Title = "Total";


            if (count1 == 0)
            {
                treeview2.AppendColumn(BookID);
                treeview2.AppendColumn(Bookname);
                treeview2.AppendColumn(BookQuantity);
                treeview2.AppendColumn(BookPrice);
                treeview2.AppendColumn(TotalPrice);
                count1++;
            }

            musicListStore1 = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

            CellRendererText BIDCell = new CellRendererText();
            BookID.PackStart(BIDCell, true);
            CellRendererText BNameCell = new CellRendererText();
            Bookname.PackStart(BNameCell, true);
            CellRendererText BQuantityCell = new CellRendererText();
            BookQuantity.PackStart(BQuantityCell, true);
            CellRendererText BPriceCell = new CellRendererText();
            BookPrice.PackStart(BPriceCell, true);
            CellRendererText TPriceCell = new CellRendererText();
            TotalPrice.PackStart(TPriceCell, true);

            BookID.AddAttribute(BIDCell, "text", 0);
            Bookname.AddAttribute(BNameCell, "text", 1);
            BookQuantity.AddAttribute(BQuantityCell, "text", 2);
            BookPrice.AddAttribute(BPriceCell, "text", 3);
            TotalPrice.AddAttribute(TPriceCell, "text", 4);

            query = "SELECT * FROM TotalPrice";
            executequery(query);
            sqlconn.Open();
            using (var reader = sqlcmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var Bid = reader.GetString(0);
                    var Bname = reader.GetString(1);
                    var Bquantity = reader.GetString(2);
                    var Bprice = reader.GetString(3);
                    var Tprice = reader.GetString(4);

                    musicListStore1.AppendValues(Bid, Bname, Bquantity, Bprice,Tprice);
                }
            }
            treeview2.Model = musicListStore1;
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


        private void createtable()
        {
            query = "CREATE TABLE IF NOT EXISTS TotalPrice(BookID integer primary key autoincrement,BookName varchar(50),BookPrice VARCHAR(50),BookQuantity varchar(50),TotalPrice varchar(50))";
            executequery(query);
        }

        protected void Click(object sender, EventArgs e)
        {
            if (treeview1.Selection.GetSelected(out selected))
            {

                entry1.Text = (string)musicListStore.GetValue(selected, 1);
               entry2.Text = (string)musicListStore.GetValue(selected, 3);
            
            }
        }

        protected void AddtoBill(object sender, EventArgs e)
        {
            createtable();
            var n1 = Convert.ToInt32(entry2.Text);
            var n2 = Convert.ToInt32(entry3.Text);
            var n3 = (Convert.ToInt32(musicListStore.GetValue(selected, 2)))-n2 ;
            
            query = "INSERT INTO TotalPrice(BookName,BookPrice,BookQuantity,TotalPrice) VALUES('" + entry1.Text + "','" + entry2.Text + "','" + entry3.Text + "','" + n1*n2 +"')";
            executequery(query);
            query = "update Book set BookQuantity='" + n3 + "' where BookID='" + musicListStore.GetValue(selected, 0) + "'";
            executequery(query);
            showall();
            showall1();
            cleardata();
        }

        private void cleardata()
        {

            entry1.Text = "";
            entry2.Text = "";
            entry3.Text = "";
            entry4.Text = "";
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
    }
}
