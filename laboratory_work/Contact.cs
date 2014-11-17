using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace laboratory_work
{
    class Contact
    {
        List<VCard> _a = new List<VCard>();

        public Contact(List<VCard> t)
        {
            _a = t;
        }

        public VCard SearchByName(string name)
        {
            foreach (var t in _a)
            {
                if (t.GivenName == name)
                    return t;
            }
            return null;
        }

        public  VCard SearchBySurname(string surname)
        {
            foreach (var t in _a)
            {
                if (t.Surname == surname)
                    return t;
            }
            return null;
        }

        public  VCard SearchByNameSurname(string name, string surname)
        {
            foreach (var t in _a)
            {
                if (t.Surname == surname && t.GivenName == name)
                    return t;
            }
            return null;
        }

        public  VCard SearchByPhone(string phone)
        {
            var phn = new PhoneNumber { Number = phone };
            foreach (var t in _a)
            {
                if (t.Phones.Contains(phn))
                    return t;
            }
            return null;
        }

        public  VCard SearchByEmail(string email)
        {
            var mail = new EmailAddress { Address = email };
            foreach (var t in _a)
            {
                if (t.Emails.Contains(mail))
                    return t;
            }
            return null;
        }

        public  String ViewAllContacts(int length)
        {
            var str = new StringBuilder();
            for (var j = 0; j < _a.Count - 1; ++j)
            {
                str.Append(Program.Print(_a[j]));
                var tmp = "";
                for (var i = 0; i < length; ++i)
                    tmp += "_";
                str.AppendLine(tmp);
            }
            str.Append(Program.Print(_a[_a.Count - 1]));
            return str.ToString();
        }
    }
}
