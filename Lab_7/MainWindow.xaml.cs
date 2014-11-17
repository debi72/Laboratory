using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using laboratory_work;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Lab_7
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const Visibility Hide = Visibility.Hidden;
        private const Visibility Show = Visibility.Visible;

        private readonly string Path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                                       @"\Contacts";

        public int I_phone = 0, I_email = 0, I_url = 0;
        private int _pos;
        private List<VCard> contacts;

        public MainWindow()
        {
            InitializeComponent();
            HideAll();
            if (Directory.Exists(Path))
            {
                contacts = VCardManager.LoadVCards(Path);
                foreach (var t in contacts)
                {
                    Contacts.Items.Add(t.FormattedName);
                }
            }
            else
            {
                MessageBox.Show("Directory is not exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Directory.CreateDirectory(Path);
            }
        }


        private void Contacts_Click(object sender, MouseButtonEventArgs e)
        {
            Info.Visibility = Show;
            _pos = Contacts.SelectedIndex;
            StateAll(false);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SavebBtn_Click(object sender, RoutedEventArgs e)
        {
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
            Name.IsEnabled = state;
            Surname.IsEnabled = state;
            MiddleName.IsEnabled = state;
            Prefix.IsEnabled = state;
            Suffix.IsEnabled = state;
            BDay.IsEnabled = state;
            Note.IsEnabled = state;
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
                    txt.IsEnabled = hide;
                }
                catch (Exception)
                {
                }
            }
        }


        private void Add_template(Grid grid, Button btn, string name, bool del_btn_show, ref int I, string content = "")
        {
            var r = new TextBox
            {
                Text = content,
                Name = name + I,
                Margin = new Thickness(0, 30*I, 20, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            if (del_btn_show) btn.Visibility = Show;
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
            foreach (var email in contact.Emails)
            {
                Add_template(GEmail, Delete_email, "email_", false, ref I_email, email.Address);
            }
            foreach (var url in contact.Urls)
            {
                Add_template(GEmail, Delete_email, "url_", false, ref I_url, url.Address);
            }

        }

        private void Init()
        {
            Name.Text = "";
            Surname.Text = "";
            Contacts.SelectedIndex = -1;
            BDay.Text = DateTime.Now.ToShortDateString();
        }

        private void BDay_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = false;
        }
    }
}