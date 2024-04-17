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
    /// Логика взаимодействия для InformationRequest.xaml
    /// </summary>
    public partial class InformationRequest : Window
    {
        DataContext _context = new DataContext();
        int numberValue;
        public InformationRequest(int numberValue)
        {
            InitializeComponent();
            this.numberValue = numberValue;
            PreLoad();
            LoadStatusHistory();
        }

        public void PreLoad()
        {
            var result = _context.RequestTs.Include(x => x.TypeFailNavigation).Where(x => x.Number == numberValue);

            ListInfo.Items.Clear();

            foreach (var item in result)
            {
                ListInfo.Items.Add(item);
            }

            ListInfo.Items.Refresh();
        }

        public void LoadStatusHistory()
        {
            var result = _context.StatusChangeTs.Include(x => x.RequestStatusNavigation).Where(x => x.RequestId == numberValue);

            ListStatus.Items.Clear();
            foreach(var item in result)
            {
                if (item.Comment == "" || item.Comment == null)
                {
                    item.Comment = "Без комментария";
                }
                ListStatus.Items.Add(item);
            }
            ListStatus.Items.Refresh();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OutViewInfo outViewInfo = new OutViewInfo();
            outViewInfo.Show();
            this.Close();
        }
    }
}
