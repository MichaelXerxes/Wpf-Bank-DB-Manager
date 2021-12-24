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
    
    public partial class MainWindow : Window
        
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["Wpf_BankDBManager.Properties.Settings.DataBaseSampleOneConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowBranches();
            ShowAllCashiers();
           
            
            
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
        private void ShowAssociatedCahsiers()
        {
            try
            {
                string query = "select * from Cashier c inner join WorkPlaceCashierBankBranch wpbb" +
                    " on c.Id = wpbb.CashierId where wpbb.BankBranchId= @BankBranchId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@BankBranchId", BranchesList.SelectedValue);

                    DataTable cashierTable = new DataTable();

                    sqlDataAdapter.Fill(cashierTable);

                    ListAssociatedCashiers.DisplayMemberPath = "Name";
                    ListAssociatedCashiers.SelectedValuePath = "Id";
                    ListAssociatedCashiers.ItemsSource = cashierTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void ShowCahsierDetails()
        {
            try
            {
                string query = "select Cashier.* from Cashier where Id= @CashierId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@CashierId", ListAssociatedCashiers.SelectedValue);

                    DataTable cashierTable = new DataTable();

                    sqlDataAdapter.Fill(cashierTable);

                    ListCahsierDetails.DisplayMemberPath = "Name" + "Contact_Number" + "Earnings";
                    ListCahsierDetails.SelectedValuePath = "Id";
                   // ListCahsierDetails.DisplayMemberPath = "Contact_Number";
                    // ListCahsierDetails.DisplayMemberPath = "Earnings";
                   // ListCahsierDetails.SelectedValuePath = "Id";
                    ListCahsierDetails.ItemsSource = cashierTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void ShowAllCashiers()
        {
            try
            {
                string query = "select * from Cashier";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable cashierTable = new DataTable();
                    sqlDataAdapter.Fill(cashierTable);
                    ListAllCashiers.DisplayMemberPath = "Name";
                    ListAllCashiers.SelectedValuePath = "Id";
                    ListAllCashiers.ItemsSource = cashierTable.DefaultView;
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
           
        }
        private void BranchesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(ListAssociatedCashiers.SelectedValue.ToString(););
            //ListAssociatedCashiers.SelectedValue.ToString();
            ShowAssociatedCahsiers();
        }

        private void ListAssociatedCashiers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowCahsierDetails();
        }

        private void DeleteBranch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from BankBranch where id = @BranchId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@BranchId", BranchesList.SelectedValue);
                sqlCommand.ExecuteScalar();
                
                
            }
            catch(Exception ed)
            {
                MessageBox.Show(ed.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBranches();
            }
            
        }
        private void AddBranch_Click(object sender,RoutedEventArgs e)
        {
            try
            {
                string query = "insert into BankBranch values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", textBox.Text);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ed)
            {
                MessageBox.Show(ed.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBranches();
            }
        }
        private void AddCashierToBranch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into WorkPlaceCashierBankBranch values (@BankBranchId, @CashierId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@BankBranchId", BranchesList.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@CashierId", ListAllCashiers.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ed)
            {
                MessageBox.Show(ed.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedCahsiers();
            }
        }

        private void AddCashier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Cashier values (@Name, @Contact_Number, @Earnings)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", textCashierName.Text);
                sqlCommand.Parameters.AddWithValue("@Contact_Number", textContactNumber.Text);
                sqlCommand.Parameters.AddWithValue("@Earnings", textEarnings.Text);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ed)
            {
                MessageBox.Show(ed.ToString());
            }
            finally
            {
                sqlConnection.Close();

                ShowAllCashiers();
            }
        }

        private void DeleteCashier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Cashier where id = @CashierId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@CashierId", ListAllCashiers.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ed)
            {
                MessageBox.Show(ed.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllCashiers();
            }
        }
    }
}
