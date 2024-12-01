using System;

using System.Windows;
using System.IO;

using System.Windows.Markup;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Remoting;
namespace login_registrarion
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPasswordVisible = false;
        public MainWindow()
        {
            InitializeComponent();

        }


        private void btn_register2_Click(object sender, RoutedEventArgs e)
        {
            string name = TB_name.Text;
            string familiya = TB_Familiya.Text;
            string otchestvo = TB_otchestvo.Text;
            string login = TB_login1.Text;
            string password = TB_password1.Text;
            string double_password = TB_password2.Text;

            // Проверка на заполненность полей
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(familiya) ||
                string.IsNullOrWhiteSpace(otchestvo) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(double_password))
            {
                MessageBox.Show("Заполните все данные в полях!");
                return; // Завершаем выполнение метода, если поля не заполнены
            }

            // Проверка совпадения паролей
            if (password != double_password)
            {
                MessageBox.Show("Пароли не соответствуют!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Завершаем выполнение метода, если пароли не совпадают
            }

            // Если все проверки пройдены, сохраняем данные
            SaveToFile(name, familiya, otchestvo, login, password);
            MessageBox.Show("Вы прошли регистрацию!");
            tabControl.SelectedItem = LOGIN;

        }

        private void SaveToFile(string name, string familiya, string otchestvo, string login, string password)
        {
            string filePath = "passwords.txt"; // Укажите путь к файлу, например, "output.txt"

            try
            {
                // Записываем данные в файл
                using (StreamWriter writer = new StreamWriter(filePath, true)) // true для добавления в конец файла
                {
                    // Объединяем данные в одну строку
                    string data = $"{familiya}, {name}, {otchestvo}, {login}, {password}";
                    writer.WriteLine(data); // Записываем строку в файл
                }
                MessageBox.Show("Вы успешно зарегистрировались!");
            }
            catch 
            {
                MessageBox.Show($"Ошибка ");
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            TB_name.Clear();
            TB_Familiya.Clear();
            TB_otchestvo.Clear();
            TB_login1.Clear();
            TB_password1.Clear();
            TB_password2.Clear();
        }

        private void btn_hiden_password_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                // Скрыть текст пароля
                TB_password.Visibility = Visibility.Collapsed; // Скрываем TextBox
                PB_password.Visibility = Visibility.Visible; // Показываем PasswordBox

            }
            else
            {
                // Показать текст пароля
                TB_password.Text = PB_password.Password; // Устанавливаем текст из PasswordBox в TextBox
                TB_password.Visibility = Visibility.Visible; // Показываем TextBox
                PB_password.Visibility = Visibility.Collapsed; // Скрываем PasswordBox

            }

            isPasswordVisible = !isPasswordVisible; // Переключаем состояние видимости

        }

        private void btn_register_Click_2(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = REGISTRATION; 
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {


            string login = TB_login.Text;
            string password = PB_password.Password;

            string[] lines = File.ReadAllLines("passwords.txt");

            // Переменная для проверки успешного входа
            bool isAuthenticated = false;

            // Проверка каждой строки на соответствие логину и паролю
            foreach (string line in lines)
            {
                string[] userData = line.Split(',');

                if (userData.Length == 5) // Убедимся, что в строке 5 элементов
                {
                    string fileLogin = userData[3].Trim(); // Логин из файла
                    string filePassword = userData[4].Trim(); // Пароль из файла

                    if (fileLogin == login && filePassword == password)
                    {
                        isAuthenticated = true;
                        break; // Выходим из цикла, если нашли совпадение
                    }
                }
            }

            // Выводим сообщение в зависимости от результата проверки
            if (isAuthenticated)
            {
                MessageBox.Show("Вы успешно авторизовались!");
                Window2xaml window2 = new Window2xaml();
                window2.Show();
                this.Close();
            }
            else
            {
                TB_capcha.Visibility = Visibility.Visible;
                LB_error.Visibility = Visibility.Visible;
                LB_error.Visibility = Visibility.Visible;

                String allowchar = " ";
                allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,";
                allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
                allowchar += "1,2,3,4,5,6,7,8,9,0";
                char[] a = { ',' };

                String[] ar = allowchar.Split(a);

                String pwd = " ";
                String temp = " ";
                Random r = new Random();

                int kol = 6;

                for (int i = 0; i < kol; i++)
                {
                    temp = ar[(r.Next(0, ar.Length))];
                    pwd += temp;
                }

                LB_capcha.Content = pwd;

                string capcha = TB_capcha.Text;

                if (capcha != pwd) Capcha(); 
            }
        }
        private void Capcha()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };

            String[] ar = allowchar.Split(a);

            String pwd = " ";
            String temp = " ";
            Random r = new Random();

            int kol = 6;

            for (int i = 0; i < kol; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }

            LB_capcha.Content = pwd;
        }
    }
}

