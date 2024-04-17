using demo2024.Model;
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
using System.Windows.Shapes;

namespace demo2024.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorizaction.xaml
    /// </summary>
    public partial class Authorizaction : Window
    {
        DataContext _context = new DataContext();

        public Authorizaction()
        {
            InitializeComponent();
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e)
        {
            CheckUser();
        }

        public void CheckUser()
        {

            var UserGet = _context.UserTs.FirstOrDefault(x => x.Login == LoginInput.Text && x.Password == PasswordInput.Text);

            if (UserGet != null)
            {
                Application.Current.Resources["UserInfo"] = UserGet;
                if (UserGet.Role == 1)
                {
                    OutViewInfo OVI = new OutViewInfo();
                    OVI.Show();
                    this.Close();
                }
                else if (UserGet.Role == 2)
                {
                    OutViewInfoIspolnitel OVII = new OutViewInfoIspolnitel();
                    OVII.Show();
                    this.Close();
                }

                
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
    }
}
