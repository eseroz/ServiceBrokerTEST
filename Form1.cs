using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private string GetConnectionString()
        {
            return @"server=.;database=Panda2017;user id=sa;password=sapass!";
        }

        SqlConnection connection;

        public Form1()
        {
            InitializeComponent();

            connection = new SqlConnection(GetConnectionString());
            connection.Open();

            SomeMethod();

            CheckForIllegalCrossThreadCalls = false;


        }

        void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dep = sender as SqlDependency;

            dep.OnChange -= new OnChangeEventHandler(OnDependencyChange);
            SomeMethod();
        }

        void SomeMethod()
        {
            // Assume connection is an open SqlConnection.
            // Create a new SqlCommand object.
            using (SqlCommand cmd =
                new SqlCommand("SELECT  [Id] ,[Alan],[DosyaAdi] FROM dbo.Mesajlar where Mesaj='34'", connection))
            {
                // Create a dependency and associate it with the SqlCommand.
                SqlDependency dependency = new SqlDependency(cmd);

                SqlDependency.Start(GetConnectionString());

                dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

                // Execute the command.
                using (SqlDataReader dr = cmd.ExecuteReader())
                {


                    while (dr.Read())
                    {
                        var u = dr[1];
                        textBox1.Text +=  u.ToString();
                    }

                }
            }
        }
    }
}