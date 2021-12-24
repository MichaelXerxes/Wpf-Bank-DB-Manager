using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Wpf_BankDBManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["Wpf_BankDBManager.Properties.Settings.DataBaseSampleOneConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowBranches();
            
        }
        private void ShowBranches()
        {
            try
            {
                string query = "select * from BankBranch";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable branchTable = new DataTable();

                    sqlDataAdapter.Fill(branchTable);

                    BranchesList.DisplayMemberPath = "Location";
                    BranchesList.SelectedValuePath = "Id";
                    BranchesList.ItemsSource = branchTable.DefaultView;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
