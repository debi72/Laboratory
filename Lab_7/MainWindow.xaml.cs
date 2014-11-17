using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using laboratory_work;
//using Button = System.Windows.Controls.Button;
//using HorizontalAlignment = System.Windows.HorizontalAlignment;
//using KeyEventArgs = System.Windows.Input.KeyEventArgs;
//using MessageBox = System.Windows.MessageBox;
//using TextBox = System.Windows.Controls.TextBox;

namespace Lab_7
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private new const Visibility Hide = Visibility.Hidden;
        private new const Visibility Show = Visibility.Visible;

        private readonly string _path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                                       @"\Contacts";

        public int I_phone = 0, I_email = 0, I_url = 0;
        private int _pos;
        private List<VCard> contacts;

        public MainWindow()
        {
            InitializeComponent();
            HideAll();
            if (Directory.Exists(_path))
            {
                contacts = VCardManager.LoadVCards(_path);
                foreach (var t in contacts)
                {
                    Contacts.Items.Add(t.FormattedName);
                }
            }
            else
            {
                MessageBox.Show("Directory is not exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Directory.CreateDirectory(_path);
            }
        }


        private void Contacts_Click(object sender, MouseButtonEventArgs e)
        {
            Info.Visibility = Show;
            _pos = Contacts.SelectedIndex;
            StateAll(false);
            Fill(contacts[_pos]);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SavebBtn_Click(object sender, RoutedEventArgs e)
        {
            var phones = GetList<PhoneNumber>(GPhone);
            var emails = GetList<EmailAddress>(GEmail);
            var urls = GetList<Url>(GUrl);
            var newContact = new VCard(Name.Text, Surname.Text, BDay.DisplayDate, MiddleName.Text, NickName.Text, Prefix.Text, Suffix.Text, phones, emails, urls, Note.Text);
            contacts.Add(newContact);
            try
            {
                VCardManager.SaveVCards(contacts, _path);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            HideAll();
            if (Directory.Exists(_path))
            {
                contacts = VCardManager.LoadVCards(_path);
                foreach (var t in contacts)
                {
                    Contacts.Items.Add(t.FormattedName);
                }
            }
            else
            {
                MessageBox.Show("Directory is not exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Directory.CreateDirectory(_path);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
            StateAll(true);
            Add_buttons(Show);

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
            StateAll(true);
            Add_buttons(Show);
            Init();
        }

        private void Add_email_Click(object sender, RoutedEventArgs e)
        {
            Add_template(GEmail, Delete_email, "email_", true, ref I_email);
        }

        private void Delete_email_Click(object sender, RoutedEventArgs e)
        {
            Delete_template(GEmail, Delete_email, "email_", ref I_email);
        }

        private void Add_Url_Click(object sender, RoutedEventArgs e)
        {
            Add_template(GUrl, Delete_url, "url_", true, ref I_url);
        }

        private void Delete_url_Click(object sender, RoutedEventArgs e)
        {
            Delete_template(GUrl, Delete_url, "url_", ref I_url);
        }

        private void Add_phone_Click(object sender, RoutedEventArgs e)
        {
            Add_template(GPhone, Delete_phone, "phone_", true, ref I_phone);
        }

        private void Delete_phone_Click(object sender, RoutedEventArgs e)
        {
            Delete_template(GPhone, Delete_phone, "phone_", ref I_phone);
        }


        private void StateAll(bool state)
        {
            Name.Focusable = state;
            Surname.Focusable = state;
            MiddleName.Focusable = state;
            Prefix.Focusable = state;
            Suffix.Focusable = state;
            BDay.Focusable = false;
            BDay.IsEnabled = state;
            Note.Focusable = state;
            TxtBoxes(GEmail, state);
            TxtBoxes(GPhone, state);
            TxtBoxes(GUrl, state);
        }

       
        private void HideAll()
        {
            Info.Visibility = Hide;
            SavebBtn.Visibility = Hide;
            Delete_buttons(Hide);
            Add_buttons(Hide);

        }
        private void ShowAll()
        {
            Info.Visibility = Show;
            SavebBtn.Visibility = Show;
            Add_buttons(Show);
        }

        private void TxtBoxes(Grid grid, bool hide)
        {
            foreach (UIElement elem in grid.Children)
            {
                try
                {
                    var txt = (TextBox) elem;
                    txt.Focusable = hide;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private List<T> GetList<T>(Grid grid)
        {
            var ans = new List<T>();
            foreach (UIElement elem in grid.Children)
            {
                try
                {
                    var txt = (TextBox)elem;
                    if (typeof(T) == typeof (EmailAddress))
                    {
                        (ans as List<EmailAddress>).Add(new EmailAddress(){Address = txt.Text});
                    }
                    if (typeof(T) == typeof(PhoneNumber))
                    {
                        (ans as List<PhoneNumber>).Add(new PhoneNumber(){Number = txt.Text});
                    }
                    if (typeof(T) == typeof(Url))
                    {
                        (ans as List<Url>).Add(new Url() { Address = txt.Text });
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return ans;
        }

        private void Add_template(Grid grid, Button btn, string name, bool delBtnShow, ref int I, string content = "")
        {
            var r = new TextBox
            {
                Text = content,
                Name = name + I,
                Margin = new Thickness(0, 30*I, 20, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            if (delBtnShow) btn.Visibility = Show;
            ++I;
            grid.Children.Add(r);
        }

        private void Delete_template(Grid grid, Button btn, string name, ref int I)
        {
            if (grid.Children.Count <= 2) return;
            foreach (UIElement t in grid.Children)
            {
                try
                {
                    var x = (TextBox) t;
                    if (x.Name != name + (grid.Children.Count - 3)) continue;
                    grid.Children.Remove(t);
                    --I;
                    if (I == 0)
                        btn.Visibility = Hide;
                    return;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        void Add_buttons(Visibility state)
        {
            Add_Email.Visibility = state;
            Add_Url.Visibility = state;
            Add_phone.Visibility = state;
        }

        void Delete_buttons(Visibility state)
        {
            Delete_email.Visibility = state;
            Delete_url.Visibility = state;
            Delete_phone.Visibility = state;
        }

        void Fill(VCard contact)
        {
            Name.Text = contact.GivenName;
            Surname.Text = contact.Surname;
            MiddleName.Text = contact.MiddleName;
            NickName.Text = contact.Nickname;
            BDay.DisplayDate = contact.Birthday;
            BDay.Text = contact.Birthday.ToShortDateString();
            if (contact.Emails.Count != 0)
                foreach (var email in contact.Emails)
                {
                    Add_template(GEmail, Delete_email, "email_", false, ref I_email, email.Address);
                }
            if (contact.Urls.Count != 0)
                foreach (var url in contact.Urls)
                {
                    Add_template(GEmail, Delete_email, "url_", false, ref I_url, url.Address);
                }
            if (contact.Urls.Count != 0) 
                foreach (var phone in contact.Phones)
                {
                    Add_template(GPhone, Delete_phone, "phone_", false, ref I_phone, phone.Number);
                }
        }

        private void Init()
        {
            Name.Text = "";
            Surname.Text = "";
            MiddleName.Text = "";
            Prefix.Text = "";
            Suffix.Text = "";
            BDay.DisplayDate = DateTime.Now;
            Note.Text = "";
            while (GPhone.Children.Count > 0) Delete_template(GPhone, Delete_phone, "phone_", ref I_phone);
            while (GEmail.Children.Count > 0) Delete_template(GEmail, Delete_email, "email_", ref I_email);
            while (GUrl.Children.Count > 0) Delete_template(GUrl, Delete_url, "url_", ref I_url);
            Contacts.SelectedIndex = -1;
            BDay.Text = DateTime.Now.ToShortDateString();
        }
    }
}