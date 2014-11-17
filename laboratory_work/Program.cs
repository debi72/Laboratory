using System;
using System.IO;
using System.Text;

namespace laboratory_work
{
    enum MainMenuChoices { All = 1, Search, New, Exit };
    enum SeacrhMenuChoices { Back, Name, Surname, NameAndSurname, Phone, Email };
    public class Program
    {
        public static void Main()
        {
            var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent;
            if (directoryInfo == null) return;
            if (directoryInfo.Parent == null) return;
            if (directoryInfo.Parent.Parent == null) return;
            var path = directoryInfo.Parent.Parent.FullName + @"\Contacts";
            var vCards = VCardManager.LoadVCards(path);
            var contact = new Contact(vCards);
            VCardManager.SaveVCards(vCards, path);
            while (true)
            {
                Console.Clear();
                Menu.Pos = 1;
                var code = (MainMenuChoices)Menu.MainMenu(); //choice of user in main menu
                switch (code) //switch the choice of user in main menu
                {
                    case MainMenuChoices.All:
                        Console.Clear();
                        Console.Write(contact.ViewAllContacts(Console.WindowWidth));
                        break;
                    case MainMenuChoices.Search:
                        Search:
                        Console.Clear();
                        var cd = (SeacrhMenuChoices)Menu.SearchMenu(); // choice of user in search menu
                        switch (cd) //switch the choice of user in search menu
                        {
                            case SeacrhMenuChoices.Name:
                                Console.WriteLine("Please enter the name\n");
                                Console.WriteLine(Print(contact.SearchByName(Console.ReadLine())));
                                break;

                            case SeacrhMenuChoices.Surname:
                                Console.WriteLine("Please enter the Surnamename\n");
                                Console.WriteLine(Print(contact.SearchBySurname(Console.ReadLine())));
                                break;

                            case SeacrhMenuChoices.NameAndSurname:
                                Console.WriteLine("Please enter the name and surname via the gap\n");
                                var readLine = Console.ReadLine();
                                if (readLine != null)
                                {
                                    var s = readLine.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                                    Console.WriteLine(Print(contact.SearchByNameSurname(s[0], s[1])));
                                }
                                break;

                            case SeacrhMenuChoices.Phone:
                                Console.WriteLine("Please enter the Phone\n");
                                Console.WriteLine(Print(contact.SearchByPhone(Console.ReadLine())));
                                break;

                            case SeacrhMenuChoices.Email:
                                Console.WriteLine("Please enter the e-mail\n");
                                Console.WriteLine(Print(contact.SearchByEmail(Console.ReadLine())));
                                break;

                            case SeacrhMenuChoices.Back:
                                goto Search;
                        }
                        break;
                    case MainMenuChoices.New:
                        var ccr = new ContactConsoleRead();
                        ccr.Read();
                        vCards.Add(ccr.Card);
                        VCardManager.SaveVCards(vCards, path);
                        Console.WriteLine("Contact created.");
                        break;
                    case MainMenuChoices.Exit:
                        Menu.ExitShowDialog();
                        break;
                }
                Console.ReadKey();
            }
        }

        public static string Print(VCard v)
        {
            if (v == null)
                return "No such contact";

            var r = new StringBuilder();
            r.AppendLine("Name: " + v.GivenName);
            r.AppendLine("Surname: " + v.Surname);
            if (!string.IsNullOrEmpty(v.MiddleName))
                r.AppendLine("Middle name: " + v.MiddleName);
            if (!string.IsNullOrEmpty(v.Prefix))
                r.AppendLine("Prefix: " + v.Prefix);
            if (!string.IsNullOrEmpty(v.Suffix))
                r.AppendLine("Suffix: " + v.Suffix);
            if (!string.IsNullOrEmpty(v.Nickname))
                r.AppendLine("Nickname: " + v.Nickname);
            if (v.Birthday > DateTime.MinValue)
                r.AppendLine("Birthday: " + v.Birthday.ToShortDateString());

            var email = "Email: ";
            foreach (var t in v.Emails)
            {
                email += t.Address + ", ";
            }
            if (email != "Email: ")
            {
                r.AppendLine(email.Substring(0, email.Length - 2));
            }

            var phone = "Telephone: ";
            foreach (var t in v.Phones)
            {
                phone += t.Number + ", ";
            }
            if (phone != "Telephone: ")
            {
                r.AppendLine(phone.Substring(0, phone.Length - 2));
            }

            var url = "URL: ";
            foreach (var t in v.Urls)
            {
                url += t.Address + ", ";
            }
            if (url != "URL: ")
            {
                r.AppendLine(url.Substring(0, url.Length - 2));
            }

            if (!String.IsNullOrEmpty(v.Note))
                r.AppendLine("Note:" + v.Note);

            return r.ToString();
        }
    }
}