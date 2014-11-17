using System;
using System.Text;

namespace laboratory_work
{
    class VCardBuilder
    {
        readonly StringBuilder _r = new StringBuilder();
        private readonly VCard _v;

        public VCardBuilder(VCard x)
        {
            _v = x;
        }

         void Set_Name()
        {
            _r.AppendLine("N:" + string.Join(";", new[] { _v.Surname, _v.GivenName, _v.MiddleName, _v.Prefix, _v.Suffix }).Trim(new[] { ';' }));
        }

         void Set_FN()
        {
            _r.AppendLine("FN:" + string.Join(",", new[] { _v.Prefix, _v.Suffix, _v.Surname, _v.GivenName, _v.MiddleName }).Trim(','));
        }

         void Set_Birthday()
        {
            if (_v.Birthday > DateTime.MinValue)
                _r.AppendLine("BDAY:" + _v.Birthday.ToString("yyyy-MM-dd"));
        }

         void Add_Emails()
        {
             foreach (var email in _v.Emails)
                if (!String.IsNullOrEmpty(email.Address))
                    _r.AppendLine("EMAIL:" + email.Address);
        }

         void Add_Phones()
        {
             foreach (var phone in _v.Phones)
                if (!String.IsNullOrEmpty(phone.Number))
                    _r.AppendLine("TEL:" + phone.Number);
        }

         void Set_Nickname()
        {
            if (!String.IsNullOrEmpty(_v.Nickname))
                _r.AppendLine("NICKNAME:" + _v.Nickname);
        }

         void Add_Urls()
        {
             foreach (var url in _v.Urls)
                if (!String.IsNullOrEmpty(url.Address))
                    _r.AppendLine("URL:" + url.Address);
        }

         void Set_Note()
        {
            if (!String.IsNullOrEmpty(_v.Note))
                _r.AppendLine("NOTE:" + _v.Note);
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine("BEGIN:VCARD");
            str.AppendLine("VERSION:3.0");
            Set_FN();
            Set_Name();
            Set_Birthday();
            Set_Nickname();
            Add_Emails();
            Add_Phones();
            Add_Urls();
            Set_Note();
            str.Append(_r);
            str.AppendLine("END:VCARD");
            return str.ToString();
        }
    }
}
