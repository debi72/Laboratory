using System;
using System.Globalization;

namespace laboratory_work
{
    class ContactConsoleRead
    {
        private VCard vCard;

        public ContactConsoleRead()
        {
            vCard = new VCard();
            Console.Clear();
            Console.WriteLine("Please, write down the information.\n* - necessary fields");
        }

        private void ReadName()
        {
            while (string.IsNullOrEmpty(vCard.GivenName))
            {
                Console.Write("Name*: ");
                vCard.GivenName = Console.ReadLine();
            }
        }

        void ReadSurname()
        {
            while (string.IsNullOrEmpty(vCard.Surname))
            {
                Console.Write("Surname*: ");
                vCard.Surname = Console.ReadLine();
            }
        }

        void ReadMiddleName()
        {
            Console.Write("Middle Name: ");
            vCard.MiddleName = Console.ReadLine();

        }

        void ReadPrefix()
        {
            Console.Write("Prefix: ");
            vCard.Prefix = Console.ReadLine();
        }

        void ReadSuffix()
        {
            Console.Write("Suffix: ");
            vCard.Suffix = Console.ReadLine();
        }

        void ReadNickname()
        {
            Console.Write("Nickname: ");
            vCard.Nickname = Console.ReadLine();
        }

        void ReadBirthday()
        {
            while (vCard.Birthday == DateTime.MinValue)
            {
                Console.Write("Birthday (dd.mm.yyyy)*: ");
                string[] expectedFormats = {"dd.MM.yyyy"};
                var s = Console.ReadLine();
                if (string.IsNullOrEmpty(s)) continue;
                try
                {
                    vCard.Birthday = DateTime.ParseExact(s, expectedFormats, null,
                        DateTimeStyles.AllowWhiteSpaces);
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong format of Date");
                }
            }
        }

        void ReadPhones()
        {
            var current = "1";
            while (!string.IsNullOrEmpty(current))
            {
                Console.Write("telephone(if you havn'vCard got phone, don'vCard write anything):\n");
                var phone = new PhoneNumber { Number = Console.ReadLine() };
                if (!String.IsNullOrEmpty(phone.Number))
                    vCard.Add_phone(phone);
                else current = "";
            }
        }

        void ReadUrls()
        {
            var current = "1";
            while (!string.IsNullOrEmpty(current))
            {
                Console.Write("URLs(if you havn'vCard got url, don'vCard write anything):\n");
                var url = new Url { Address = Console.ReadLine() };
                if (!String.IsNullOrEmpty(url.Address))
                    vCard.Add_url(url);
                else current = "";
            }
        }

        void ReadEmails()
        {
            var current = "1";
            while (!string.IsNullOrEmpty(current))
            {
                Console.Write("E-mail(if you havn't got e-mail address, don't write anything):\n");
                var email = new EmailAddress { Address  = Console.ReadLine() };
                if (!String.IsNullOrEmpty(email.Address))
                    vCard.Add_email(email);
                else current = "";
            }
        }

        void ReadNote()
        {
            Console.Write("Note: ");
            vCard.Note = Console.ReadLine();
        }


        public void Read()
        {
            ReadSurname();
            ReadName();
            ReadMiddleName();
            ReadPrefix();
            ReadSuffix();
            ReadNickname();
            ReadBirthday();
            ReadPhones();
            ReadEmails();
            ReadUrls();
            ReadNote();
            vCard.FormattedName = string.Join(",", new[] { vCard.Prefix, vCard.Suffix, vCard.Surname, vCard.GivenName, vCard.MiddleName });
        }

        public VCard Card
        {
            get { return vCard; }
        }
    }
}
