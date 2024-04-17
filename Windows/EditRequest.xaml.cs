using demo2024.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime;
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
using static demo2024.Windows.AddRequestWindow;

namespace demo2024.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditRequest.xaml
    /// </summary>
    public partial class EditRequest : Window
    {
        DataContext _context = new DataContext();
        int numberRequest;
        List<Person> GetPerson = new List<Person>();


        public EditRequest(int numberRequest)
        {
            InitializeComponent();
            this.numberRequest = numberRequest;
            PreLoad();
            ReadDB();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (NameDebice.Text == "" || DescriptionFail.Text == "" || ClientFIO.Text == "" || ComboTypeFail.SelectedIndex == 0 || ComboRequestStatus.SelectedIndex == 0 || ListResponsibleAdded.Items.Count == 0)
            {
                MessageBox.Show("Заполните данные");
            }
            else
            {
                UpdateRequest();
                UpdateStatus();
                UpdateResponsible();
                MessageBox.Show("Успешно");
            } 
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
            foreach (var item in GetStatus)
            {
                ComboRequestStatus.Items.Add(item.StatusName);
            }
            ComboRequestStatus.SelectedIndex = 0;
        }

        public void ReadDB()
        {
            List<RequestT> result = Model.DataContext.context.RequestTs.Where(x => x.Number == numberRequest).Include(x => x.TypeFailNavigation).ToList();

            NameDebice.Text = result[0].DeviceName;
            DescriptionFail.Text = result[0].DescriptionFail;
            ClientFIO.Text = result[0].ClientFio;
            ComboTypeFail.SelectedIndex = Convert.ToInt32(result[0].TypeFail);
            var number = _context.StatusChangeTs.Where(x => x.RequestId == numberRequest).OrderByDescending(x => x.Id).LastOrDefault();
            ComboRequestStatus.SelectedIndex = Convert.ToInt32(number.RequestStatus);

            var GetResposnible = _context.ResponsibleTs.Where(x => x.RequestId == numberRequest).Include(x => x.User);

           

            foreach (var item in GetResposnible)
            {
                GetPerson.Add(new Person()
                {
                    Id = item.User.Id.ToString(),
                    FIO = item.User.Surname + " " + item.User.Name + " " + item.User.Patronomic
                });
            }

            ListResponsibleAdded.Items.Clear();
            ListResponsibleAdded.ItemsSource = GetPerson;
            ListResponsibleAdded.Items.Refresh();

        }

        public void UpdateRequest()
        {
            var GetRequest = _context.RequestTs.FirstOrDefault(x => x.Number == numberRequest);

            GetRequest.DeviceName = NameDebice.Text;
            GetRequest.DescriptionFail = DescriptionFail.Text;
            GetRequest.ClientFio = ClientFIO.Text;
            GetRequest.TypeFail = ComboTypeFail.SelectedIndex;

            _context.SaveChanges();
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
                RequestId = numberRequest,
                RequestStatus = ComboRequestStatus.SelectedIndex,
                ChangeDate = currentDateTime
            };
            _context.StatusChangeTs.Add(newStatus);
            _context.SaveChanges();
        }

        public void UpdateResponsible()
        {
            var responsiblesToRemove = _context.ResponsibleTs.Where(x => x.RequestId == numberRequest).ToList();

            _context.ResponsibleTs.RemoveRange(responsiblesToRemove);

            foreach (Person person in ListResponsibleAdded.Items)
            {
                ResponsibleT newResponsible = new ResponsibleT
                {
                    RequestId = numberRequest,
                    UserId = Convert.ToInt32(person.Id)
                };

                _context.ResponsibleTs.Add(newResponsible);
                _context.SaveChanges();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OutViewInfo OVI = new OutViewInfo();
            OVI.Show();
            this.Close();
        }

        public class Person
        {
            public string Id { get; set; }
            public string FIO { get; set; }
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

                bool idExists = GetPerson.Any(person => person.Id == id);

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

                    GetPerson.Add(newPerson);

                    ListResponsibleAdded.ItemsSource = null;
                    ListResponsibleAdded.ItemsSource = GetPerson;
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
                GetPerson.Remove(person);

                ListResponsibleAdded.ItemsSource = null;
                ListResponsibleAdded.ItemsSource = GetPerson;
                ListResponsibleAdded.Items.Refresh();
            }
        }
    }
}
