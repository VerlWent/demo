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
    /// Логика взаимодействия для AddRequestWindow.xaml
    /// </summary>
    public partial class AddRequestWindow : Window
    {
        DataContext _context = new DataContext();

        List<string> ids = new List<string>();
        public AddRequestWindow()
        {
            InitializeComponent();
            PreLoad();
        }
        public void PreLoad()
        {
            var GetUser = _context.UserTs.Where(x => x.Role == 2);

            ComboResponsible.Items.Add("Не выбрано");
            foreach (var item in GetUser)
            {
                string FIO = item.Surname + " " + item.Name + " " + item.Patronomic;

                ComboResponsible.Items.Add("Id:" + item.Id + ", " + FIO);
            }
            ComboResponsible.SelectedIndex = 0;

            var GetTypeFail = _context.TypeFailTs;

            ComboTypeFail.Items.Add("Не выбрано");
            foreach (var item in GetTypeFail)
            {
                ComboTypeFail.Items.Add(item.FailName);
            }
            ComboTypeFail.SelectedIndex = 0;


            var GetStatus = _context.RequestStatusTs;

            ComboRequestStatus.Items.Add("Не выбрано");
            foreach(var item in GetStatus)
            {
                ComboRequestStatus.Items.Add(item.StatusName);
            }
            ComboRequestStatus.SelectedIndex = 0;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OutViewInfo OVI = new OutViewInfo();
            OVI.Show();
            this.Close();
        }

        int IdRequest = 0;

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string currentDateTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime? currentDateTime = null;

            if (DateTime.TryParseExact(currentDateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                currentDateTime = parsedDateTime; // Если парсинг прошел успешно, присваиваем значение переменной
            }

            if (NameDebice.Text == "" || DescriptionFail.Text == "" || ClientFIO.Text == "" || ComboTypeFail.SelectedIndex == 0 || ComboRequestStatus.SelectedIndex == 0 || ListResponsibleAdded.Items.Count == 0)
            {
                MessageBox.Show("Заполните данные");
            }
            else
            {
                await AddRequest(currentDateTime);

                await AddResponsible();

                await AddStatus(currentDateTime);

                _context.SaveChanges();
                MessageBox.Show("Успешно");
            }
        }

        public async Task AddRequest(DateTime? currentDateTime)
        {
            IdRequest = _context.RequestTs.Max(x => x.Number) + 1;
            RequestT newRequest = new RequestT
            {
                Number = IdRequest,
                DateAdded = currentDateTime,
                DeviceName = NameDebice.Text,
                DescriptionFail = DescriptionFail.Text,
                ClientFio = ClientFIO.Text,
                TypeFail = ComboTypeFail.SelectedIndex
            };

            _context.RequestTs.Add(newRequest);
        }

        public async Task AddResponsible()
        {
            foreach (Person person in ListResponsibleAdded.Items)
            {
                ResponsibleT newResponsible = new ResponsibleT
                {
                    RequestId = IdRequest,
                    UserId = Convert.ToInt32(person.Id)
                };

                _context.ResponsibleTs.Add(newResponsible);
            }
        }

        public async Task AddStatus(DateTime? currentDateTime)
        {
            StatusChangeT newStatusChange = new StatusChangeT
            {
                RequestId = IdRequest,
                RequestStatus = ComboRequestStatus.SelectedIndex,
                ChangeDate = currentDateTime
            };

            _context.StatusChangeTs.Add(newStatusChange);
        }

        public class Person
        {
            public string Id { get; set; }
            public string FIO { get; set;}
        }

        private void BtnAddResponsible_Click(object sender, RoutedEventArgs e)
        {
            if (ComboResponsible.SelectedIndex == 0)
            {
                LabelInfo.Background = Brushes.Red;
            }
            else
            {
                string input = ComboResponsible.SelectedItem.ToString();

                int startIndex = input.IndexOf(":") + 1;
                int endIndex = input.IndexOf(",");

                string id = input.Substring(startIndex, endIndex - startIndex).Trim();

                int startIndexFIO = input.IndexOf(",") + 1;

                string fio = input.Substring(startIndexFIO).Trim();

                bool idExists = false;
                foreach (Person person in ListResponsibleAdded.Items)
                {
                    if (person.Id == id)
                    {
                        idExists = true;
                        break;
                    }
                }

                if (idExists)
                {
                    LabelInfo.Background = Brushes.Red;
                }
                else
                {
                    Person newPerson = new Person
                    {
                        Id = id,
                        FIO = fio
                    };

                    ListResponsibleAdded.Items.Add(newPerson);
                    ListResponsibleAdded.Items.Refresh();
                    LabelInfo.Background = Brushes.Green;
                }
            }
        }

        private void DeleteResponsible_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            Person person = button.DataContext as Person;

            if (person != null)
            {
                ListResponsibleAdded.Items.Remove(person);
            }
        }
    }
}
