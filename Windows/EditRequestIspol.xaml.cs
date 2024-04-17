using demo2024.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Логика взаимодействия для EditRequestIspol.xaml
    /// </summary>
    public partial class EditRequestIspol : Window
    {
        DataContext _context = new DataContext();
        int numberValue;
        public EditRequestIspol(int numberValue)
        {
            InitializeComponent();
            this.numberValue = numberValue;
            PreLoad();
        }

        public void PreLoad()
        {
            var GetStatus = _context.RequestStatusTs;

            ComboRequestStatus.Items.Add("Не выбрано");
            foreach (var item in GetStatus)
            {
                ComboRequestStatus.Items.Add(item.StatusName);
            }
            ComboRequestStatus.SelectedIndex = 0;
        }

        public void UpdateStatus()
        {
            string currentDateTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime? currentDateTime = null;

            if (DateTime.TryParseExact(currentDateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                currentDateTime = parsedDateTime; // Если парсинг прошел успешно, присваиваем значение переменной
            }

            StatusChangeT newStatus = new StatusChangeT
            {
                RequestId = numberValue,
                RequestStatus = ComboRequestStatus.SelectedIndex,
                ChangeDate = currentDateTime,
                Comment = TextComment.Text
            };

            _context.StatusChangeTs.Add(newStatus);
            _context.SaveChanges();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OutViewInfoIspolnitel OVII = new OutViewInfoIspolnitel();
            OVII.Show();
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TextComment.Text == null ||  TextComment.Text == "" || TextComment.Text == " " || ComboRequestStatus.SelectedIndex == 0)
            {
                MessageBox.Show("Заполниете данные");
            }
            else
            {
                UpdateStatus();
                MessageBox.Show("Успешно");
            }
        }
    }
}
