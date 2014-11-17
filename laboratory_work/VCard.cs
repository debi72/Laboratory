using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace laboratory_work
{
    //structs - information about person
    public struct PhoneNumber
    {
        public string Number { get; set; }
    }

    public struct EmailAddress
    {
        public string Address { get; set; }
    }

    public struct Url
    {
        public string Address { get; set; }
    }

    //the main class worked with VCard
    public class VCard
    {
        readonly char[] _lineBreak = { '\n', '\r' }; //char array of control characters: line feed and carriege return 
        DateTime _bday; //birthday
        List<EmailAddress> _emails = new List<EmailAddress>(); 
        string _formattedName;
        string _givenName;
        string _middleName;
        string _note;
        List<PhoneNumber> _phones = new List<PhoneNumber>();
        string _prefix;
        string _suffix;
        string _surname;
        List<Url> _urls = new List<Url>();

        public void Add_email(EmailAddress x)
        {
           _emails.Add(x);    
        }

        public void Add_url(Url x)
        {
            _urls.Add(x);
        }

        public void Add_phone(PhoneNumber x)
        {
            _phones.Add(x);
        }


        public string FormattedName
        {
            get { return _formattedName; }
            set { _formattedName = value.TrimEnd(_lineBreak); }
        }

        public string Mailer { get; set; }

        public string Surname
        {
            get { return _surname; }
            set { _surname = value.TrimEnd(_lineBreak); }
         
        }

        public string Nickname { get; set; }

        public string GivenName
        {
            get { return _givenName; }
            set { _givenName = value.TrimEnd(_lineBreak); }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value.TrimEnd(_lineBreak); }
        }

        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value.TrimEnd(_lineBreak); }
        }

        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value.TrimEnd(_lineBreak); }
        }

        public DateTime Birthday
        {
            get { return _bday; }
            set { _bday = value; }
        }


        public string Note
        {
            get { return _note; }
            set { _note = value.TrimEnd(_lineBreak); }
        }

        public IEnumerable<PhoneNumber> Phones
        {
            get { return _phones; }
        }

        public IEnumerable<EmailAddress> Emails
        {
            get { return _emails; }
        }

        public IEnumerable<Url> Urls
        {
            get { return _urls; }
        }
        // regular expression to parse format *.vcf
        private const string RegxLine =
            @"((?<strElement>[^\;^:]*) (:(?<strValue> (([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) )))";

        private const string RegxN =
            @"(?<strElement>(N))(:(?<strSurname>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))(;(?<strGivenName>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))? (;(?<strMidName>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))? (;(?<strPrefix>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))? (;(?<strSuffix>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))?";

        private const string RegxFn = @"(?<strElement>(FN)) (:(?<strValue>(([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) ))";

        private const string RegxMlr =
            @"(?<strElement>(MAILER)) (:(?<strValue>(([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) ))";

        private const string RegxNote =
            @"((?<strElement>(NOTE))(:(?<strValue> (([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) )))";

        private const string RegxUrl = @"((?<strElement>(URL)) (:(?<strUrl>[^\n\r]*)))";

        private const string RegxTel = @"((?<strElement>(TEL)) (:(?<strTel>[^\n\r]*)))";

        private const string RegxNick = @"((?<strElement>(NICKNAME)) (:(?<strValue>[^\n\r]*)))";

        private const string RegxMail = @"((?<strElement>(EMAIL)) (:(?<strEmail>[^\n\r]*)))";

        private const string RegxBday = @"(?<strElement>(BDAY)) (:(?<strBDay>[^\n\r]*))";

        private const RegexOptions RegxOptions =
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;

        private enum StrElement
        {
            Fn,
            N,
            Nickname,
            Bday,
            Email,
            Tel,
            Note,
            Url,
            Mailer
        };

        private static readonly Dictionary<string, StrElement> Elements = new Dictionary<string, StrElement>
        {
            {"FN", StrElement.Fn},
            {"N", StrElement.N},
            {"Nickname", StrElement.Nickname},
            {"BDAY", StrElement.Bday},
            {"EMAIL", StrElement.Email},
            {"TEL", StrElement.Tel},
            {"Note", StrElement.Note},
            {"URL", StrElement.Url},
            {"Mailer", StrElement.Mailer}
        };

        //method parse the text in format .vcf to VCard variable
        public static VCard ParseText(string vCardText)
        {
            var v = new VCard();
            var regex = new Regex(RegxLine, RegxOptions);
            var matches = regex.Matches(vCardText);

            foreach (Match match in matches)
            {
                var vCardLine = match.Value;
                var t = match.Groups["strElement"].Value;
                if (!Elements.ContainsKey(t)) continue;
                switch (Elements[t])
                {
                    case StrElement.Fn:
                        v.FormattedName = GetString(vCardLine,RegxFn);
                        break;
                    case StrElement.N:
                        var x = GetNames(vCardLine);
                        v.Surname = x.Surname;
                        v.GivenName = x.GivenName;
                        v.MiddleName = x.MiddleName;
                        v.Prefix = x.Prefix;
                        v.Suffix = x.Suffix;
                        break;
                    case StrElement.Nickname:
                        v.Nickname = GetString(vCardLine,RegxNick);
                        break;
                    case StrElement.Bday:
                        v.Birthday = GetBirthday(vCardLine);
                        break;
                    case StrElement.Email:
                        v._emails = GetEmails(vCardLine);
                        break;
                    case StrElement.Tel:
                        v._phones = GetPhones(vCardLine);
                        break;
                    case StrElement.Note:
                        v.Note = GetString(vCardLine, RegxNote);
                        break;
                    case StrElement.Url:
                        v._urls = GetUrls(vCardLine);
                        break;
                    case StrElement.Mailer:
                        v.Mailer = GetString(vCardLine,RegxMlr);
                        break;
                }
            }
            return v;
        }

        static VCard GetNames(string vCardLine)
        {
            var v = new VCard();
            var regex = new Regex(RegxN, RegxOptions);
            var m = regex.Match(vCardLine);
            if (!m.Success) return v;
            v.Surname = m.Groups["strSurname"].Value;
            v.GivenName = m.Groups["strGivenName"].Value;
            v.MiddleName = m.Groups["strMidName"].Value;
            v.Prefix = m.Groups["strPrefix"].Value;
            v.Suffix = m.Groups["strSuffix"].Value;
            return v;
        }

        static DateTime GetBirthday(string vCardLine)
        {
            var regex = new Regex(RegxBday, RegxOptions);
            var m = regex.Match(vCardLine);
            if (!m.Success) return DateTime.MinValue;
            var bdayStr = m.Groups["strBDay"].Value;
            if (String.IsNullOrEmpty(bdayStr)) return DateTime.MinValue;
            string[] expectedFormats =
            {
                "yyyyMMdd", "yyMMdd", "yyyy-MM-dd", "dd.MM.yyyy",
                "dd/MM/yyyy"
            };
            return DateTime.ParseExact(bdayStr, expectedFormats, null,
                DateTimeStyles.AllowWhiteSpaces);
        }

        static string GetString(string vCardLine, string regx)
        {
            var regex = new Regex(regx, RegxOptions);
            var m = regex.Match(vCardLine);
            return m.Success ? m.Groups["strValue"].Value : null;
        }
        static List<EmailAddress> GetEmails(string vCardLine)
        {
            var emails = new List<EmailAddress>();
            var regex = new Regex(RegxMail, RegxOptions);
                var mc = regex.Matches(vCardLine);
                if (mc.Count > 0)
                {
                    for (var i = 0; i < mc.Count; i++)
                    {
                        var email = new EmailAddress { Address = mc[i].Groups["strValue"].Value };
                        emails.Add(email);
                    }
                }
            return emails;
        }

        static List<PhoneNumber> GetPhones(string vCardLine)
        {
            var phones = new List<PhoneNumber>();
            var regex = new Regex(RegxTel, RegxOptions);
            var mc = regex.Matches(vCardLine);
            if (mc.Count > 0)
                for (var i = 0; i < mc.Count; i++)
                    phones.Add(new PhoneNumber {Number = mc[i].Groups["strValue"].Value});
            return phones;
        }


        static List<Url> GetUrls(string vCardLine)
        {
            var urls = new List<Url>();
            var regex = new Regex(RegxUrl, RegxOptions);
            var mc = regex.Matches(vCardLine);
            if (mc.Count <= 0) return urls;
            for (var i = 0; i < mc.Count; i++)
                urls.Add(new Url{Address = mc[i].Groups["strValue"].Value});
            return urls;
        }

        //convert from VCard variable to .vcf format
        public override string ToString()
        {
            return new VCardBuilder(this).ToString();
        }
    }       
}