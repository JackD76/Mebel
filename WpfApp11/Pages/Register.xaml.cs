using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace WpfApp11.Pages
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// DESKTOP-SMQ8FBI\MSSQLSERVERR
    /// </summary>
    public partial class Register : Page
    {
        private readonly string conString;
        private FileInfo file;
        byte[] photo;
        bool capture;

        public Register(string conString)
        {
            InitializeComponent();
            this.conString = conString;
        }
        public static byte[] GetPhoto(string filePath)
        {
            FileStream stream = new FileStream(
            filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            byte[] photo = reader.ReadBytes((int)stream.Length);
            reader.Close();
            stream.Close();
            return photo;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {

            string query = "INSERT INTO users (login, password, role, first_name, second_name, last_name, photo) VALUES (@login, @password, @role, @name, @surname, @patronymic,@photo)";
            if (Login.Text == "" || Password.Text == ""|| NameP.Text == "" || Surname.Text == "" || Patronymic.Text == "")
            {

                MessageBox.Show("Все поля должны быть заполнены!"
                                + Environment.NewLine
                                + "Пожалуйста, заполниет все поля!", "Ошибка");
            }
            else
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasMiniMaxChars = new Regex(@".{8,15}");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[*&{}|+]");

                if (!hasLowerChar.IsMatch(Password.Text))
                {
                    MessageBox.Show("Пароль должен содержать букву в нижнем регистре");
                }
                else if (!hasMiniMaxChars.IsMatch(Password.Text))
                {
                    MessageBox.Show("Пароль доожжен быть от 8 до 16 символов");
                }
                else if (!hasNumber.IsMatch(Password.Text))
                {
                    MessageBox.Show("Пароль должен содержать цифру");
                }

                else if (!hasSymbols.IsMatch(Password.Text))
                {
                    MessageBox.Show("Пароль должен содержать спецсимвол");
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);

                        try
                        {
                            command.Parameters.AddWithValue("@login", Login.Text);
                            command.Parameters.AddWithValue("@password", Password.Text);
                            command.Parameters.AddWithValue("@role", 3);
                            command.Parameters.AddWithValue("@name", NameP.Text);
                            command.Parameters.AddWithValue("@surname", Surname.Text);
                            command.Parameters.AddWithValue("@patronymic", Patronymic.Text);
                            command.Parameters.AddWithValue("@photo", photo);
                            if (photo == null)
                            {
                                command.Parameters[6].DbType = DbType.Binary;
                                command.Parameters[6].Value = DBNull.Value;
                            }
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }

                        connection.Close();
                    }
                    NavigationService?.Navigate(new Page1());
                    MessageBox.Show("Пользователь успешно добавлен!", "Успешно");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " Фотографии(*.JPG; *.GIF; *.PNG)| *.JPG; *.GIF; *.PNG " ;
            if (openFileDialog.ShowDialog() == true)
            {
                this.file = new FileInfo(openFileDialog.FileName);
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(openFileDialog.FileName);
            bitmap.EndInit();
            Dich.Source = bitmap;
            photo = GetPhoto(openFileDialog.FileName);
        }

        public event EventHandler CaptchaRefreshed;
        private void CreateCaptcha()
        {
            string allowchar;
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            string[] ar = allowchar.Split(a);
            string pwd = string.Empty;
            string temp;
            var r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];

                pwd += temp;
            }

            CaptchaText.Text = pwd;
            CaptchaRefreshed?.Invoke(this, EventArgs.Empty);
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            CreateCaptcha();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //  Captcha
            if (Captcha.Text ==CaptchaText.Text)
            {
                MessageBox.Show("Проверка пройдена");
                capture = true;
            }
            else
            {
                MessageBox.Show("Проверка не пройдена!");
            }
        }
    }
}
