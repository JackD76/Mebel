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
using System.IO;
using Microsoft.Win32;

namespace WpfApp11.Pages
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// DESKTOP-SMQ8FBI\MSSQLSERVERR
    /// </summary>
    public partial class Register : Page
    {
        private string conString;
        private FileInfo file;
        byte[] photo;
        bool capture = false;

        public Register(string conString)
        {
            InitializeComponent();
            this.conString = conString;
            SqlConnection connection = new SqlConnection(@conString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT DISTINCT Role FROM dbo.[User]", connection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> list = new List<string>();
            while (reader.Read())

            {

                list.Add(reader[0].ToString());

            }
            Roled.ItemsSource = list;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (capture == false || Login.Text.Length<1 || Roled.SelectedItem==null || Password.Text.Length < 1 || Dich.Source==null || NameP.Text.Length < 1 || Surname.Text.Length < 1 || Patronymic.Text.Length < 1)
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                try
                {
                    string Role=Roled.SelectedItem.ToString();
                    SqlConnection connection = new SqlConnection(@conString);
                    connection.Open();
                    string sqlExpression = "Registration";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@login", Login.Text);
                    command.Parameters.AddWithValue("@Password", Password.Text);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@Surname", Surname.Text);
                    command.Parameters.AddWithValue("@Name", NameP.Text);
                    command.Parameters.AddWithValue("@Patronymic", Patronymic.Text);
                    command.Parameters.AddWithValue("@Photo", photo);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Пользователь успешно добавлен");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

        public event System.EventHandler CaptchaRefreshed;
        private void CreateCaptcha()
        {
            string allowchar = string.Empty;
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            string[] ar = allowchar.Split(a);
            string pwd = string.Empty;
            string temp = string.Empty;
            System.Random r = new System.Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];

                pwd += temp;
            }

            CaptchaText.Text = pwd;
            CaptchaRefreshed?.Invoke(this, System.EventArgs.Empty);
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            CreateCaptcha();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //  Captcha
            if (Captcha.Text !=null)
            {
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
            else 
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
