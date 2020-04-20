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
using WpfApp11.Pages.Role;

namespace WpfApp11.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Register());
        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            if (Login.Text.Length < 1 || Password.Text.Length < 1)
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                string conString = "Data Source =VC-103-05 ; Initial Catalog = Mebel; Integrated Security = true;";
                SqlConnection connection = new SqlConnection(@conString);
                try
                {
                    connection.Open();
                    string query = "SELECT Login, Password, Role FROM dbo.[User] where login=@login and Password= @password";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@login", Login.Text);
                    command.Parameters.AddWithValue("@password", Password.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //  if (Login.Text == reader[0].ToString() && Password.Text == reader[1].ToString())
                        // {
                        MessageBox.Show("Вход выполнен");
                        MessageBox.Show("Ваша роль:" + reader[2].ToString());
                        switch (reader[2].ToString())
                        {
                            case "Менеджер":
                                NavigationService?.Navigate(new Manager());
                                break;
                            case "Заместитель":
                                NavigationService?.Navigate(new Substituent());
                                break;
                            case "Заказчик":
                                NavigationService?.Navigate(new Customer());
                                break;
                            case "Директор":
                                NavigationService?.Navigate(new Director());
                                break;
                            case "Мастер":
                                NavigationService?.Navigate(new Master());
                                break;
                        }
                        return;
                        //}
                    }
                    MessageBox.Show("Неверный логин или пароль");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }
        
    }
}
