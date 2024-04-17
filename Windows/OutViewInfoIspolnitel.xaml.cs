using Castle.Components.DictionaryAdapter.Xml;
using demo2024.Model;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для OutViewInfoIspolnitel.xaml
    /// </summary>
    public partial class OutViewInfoIspolnitel : Window
    {
        DataContext _context = new DataContext();
        public OutViewInfoIspolnitel()
        {
            InitializeComponent();
            ReadDB();
        }

        public void ReadDB()
        {
            UserT UserGet = (UserT)Application.Current.Resources["UserInfo"];

            List<RequestT> result = _context.RequestTs.Where(x => x.ResponsibleTs.Any(x => x.UserId == UserGet.Id)).Include(x => x.TypeFailNavigation).ToList();

            if (TextSearth.Text != "" || TextSearth.Text != " " || TextSearth.Text != null)
            {
                result = result.Where(x => x.DeviceName.Contains(TextSearth.Text)).ToList();
            }

            ListInfo.Items.Clear();
            foreach (var item in result)
            {
                ListInfo.Items.Add(item);
            }
            ListInfo.Items.Refresh();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Authorizaction AW = new Authorizaction();
            AW.Show();
            this.Close();
        }

        private void BtnEditRequest_Click(object sender, RoutedEventArgs e)
        {
            if (ListInfo.SelectedItem != null)
            {
                RequestT selectedData = (RequestT)ListInfo.SelectedItem;

                int numberValue = selectedData.Number;

                EditRequestIspol ERI = new EditRequestIspol(numberValue);
                ERI.Show();
                this.Close();
            }
        }

        private void TextSearth_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReadDB();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            RequestT request = (RequestT)grid.DataContext;

            int RequestNumber = request.Number;


            List<UserT> UserGet = _context.ResponsibleTs.Where(x => x.RequestId == RequestNumber).Select(x => x.User).ToList();

            string FIO = "";
            foreach (var item in UserGet)
            {
                FIO += item.Name + " " + item.Surname + " " + item.Patronomic + "\n";
            }

            FIOISPOL.Text = FIO;
        }

        private void InfoRequest_Click(object sender, RoutedEventArgs e)
        {
            if (ListInfo.SelectedItem != null)
            {
                RequestT selectedData = (RequestT)ListInfo.SelectedItem;

                int numberValue = selectedData.Number;

                InformationRequest IR = new InformationRequest(numberValue);
                IR.Show();
                this.Close();
            }
        }
    }
}
