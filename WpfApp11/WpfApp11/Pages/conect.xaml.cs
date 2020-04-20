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

namespace WpfApp11.Pages
{
    /// <summary>
    /// Логика взаимодействия для conect.xaml
    /// </summary>
    public partial class conect : Page
    {
        private string conString;
        public conect()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conString = "Data Source = " + ServerName.Text.ToString() + "; Initial Catalog = " + BDName.Text.ToString() + "; Integrated Security = true;";
            SqlConnection connection = new SqlConnection(@conString); 
            try
            {
                connection.Open();
                MessageBox.Show($"Сервер: {ServerName.Text.ToString()}; База данных: {BDName.Text.ToString()}; Состояние: подключено.");
                NavigationService?.Navigate(new Page1(conString));
            }
            catch (SqlException)
            {
                MessageBox.Show("Подключение не выполнено!");
            }

        }

    }
}
