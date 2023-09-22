using System;
using Gtk;
using Gdk;
using Microsoft.Data.Sqlite;
using OnlineBookShop;

public partial class MainWindow : Gtk.Window
{
    int flag;
    private SqliteConnection sqlconn;
    private SqliteCommand sqlcmd;
    MessageDialog md;
    string query,query1;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {

        Build();
        insertdatatodatabase();
    }

    private void setconnection()
    {
        var ConnectionStrongBuilder = new SqliteConnectionStringBuilder();
        ConnectionStrongBuilder.DataSource = "./OnlineBookShop.db";
        sqlconn = new SqliteConnection(ConnectionStrongBuilder.ConnectionString);
    }

    private void insertdatatodatabase()
    {
        query = "DROP TABLE IF EXISTS login";
        executequery(query);
        query = "CREATE TABLE if not exists login(ID integer primary key autoincrement,sno varchar(50),name VARCHAR(50),password varchar(50))";
        executequery(query);

        query = "INSERT INTO login(sno,name,password) VALUES('m1','mohannad','mm123456')";
        executequery(query);

        query = "INSERT INTO login(sno,name,password) VALUES('b1','bushra','bu123456')";
        executequery(query);

        query = "INSERT INTO login(sno,name,password) VALUES('a1','ayad','ay123456')";
        executequery(query);

        query = "INSERT INTO login(sno,name,password) VALUES('n1','noha','nn123456')";
        executequery(query);
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


    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }



    protected void insertbtn(object sender, EventArgs e)
    {
        query = "SELECT * FROM login";
        executequery(query);
        sqlconn.Open();
        using (var reader = sqlcmd.ExecuteReader())
        {
            while (reader.Read())
            {
                if (entry1.Text == reader.GetString(2) && entry2.Text == reader.GetString(3))
                    flag = 1;
            }
        }
        query1 = "SELECT * FROM Sellers";
        executequery(query1);
        sqlconn.Open();
        using (var reader = sqlcmd.ExecuteReader())
        {
            while (reader.Read())
            {
                if (entry1.Text == reader.GetString(2) && entry2.Text == reader.GetString(4))
                
                    flag = 2; 
                
            }
        }

        
        if (flag == 1)
            {
                md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "welcome " + entry1.Text);
                md.Run();
                md.Destroy();
                this.Hide();
                Window1 main = new Window1();
                Gdk.Color col = new Gdk.Color();
                Gdk.Color.Parse("gray", ref col);
                main.ModifyBg(StateType.Normal, col);
                main.Show();
            }
        else if (flag == 2)
         {
             md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "welcome " + entry1.Text);
             md.Run();
             md.Destroy();
             this.Hide();
             Billings billings = new Billings();
             Gdk.Color col = new Gdk.Color();
             Gdk.Color.Parse("gray", ref col);
             billings.ModifyBg(StateType.Normal, col);
             billings.Show();
         }
        else
        {
                md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "Invaild username or paassword");
                md.Run();
                md.Destroy();
            }
        }
    }


