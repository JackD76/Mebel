using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using WpfApp11.Pages.Role;
using System.Configuration;

namespace WpfApp11.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private readonly string conString;
        public Page1()
        {
            InitializeComponent();
            this.conString = ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Register(conString));
        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            if (Login.Text.Length < 1 || Password.Text.Length < 1)
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                SqlConnection connection = new SqlConnection(conString);
                try
                {
                    connection.Open();
                    string query = "SELECT role.role_id FROM dbo.users inner join role on users.role = role.role_id where users.login=@login and users.password= @password";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@login", Login.Text);
                    command.Parameters.AddWithValue("@password", Password.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        switch (reader.GetInt32(0))
                        {
                            case 1:
                                NavigationService?.Navigate(new Manager());
                                break;
                            case 2:
                                NavigationService?.Navigate(new Substituent());
                                break;
                            case 3:
                                NavigationService?.Navigate(new Customer());
                                break;
                            case 5:
                                NavigationService?.Navigate(new Director());
                                break;
                            case 4:
                                NavigationService?.Navigate(new Master());
                                break;
                        }
                    }
                    else MessageBox.Show("Неверный логин или пароль");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }
        
    }
}
