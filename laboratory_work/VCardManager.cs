using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace laboratory_work
{
    public class VCardManager
    {
        public static List<VCard> LoadVCards(string path)
        {
            var ans = new List<VCard>();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                throw new Exception("Files not found.");
            }
            foreach (var contact in Directory.GetFiles(path))
                ans.Add(VCard.ParseText(File.ReadAllText(contact)));
            return ans;
        }


        public static void SaveVCards(List<VCard> vCards, string path)
        {
            try
            {
                foreach (var contact in Directory.GetFiles(path))
                    File.Delete(contact);

                for (var i = 0; i < vCards.Count; ++i)
                    File.WriteAllText(path + "/Contact_" + i + ".vcf", vCards[i].ToString(), Encoding.UTF8);
            }
            catch (Exception)
            {
               throw new Exception("An error occured writing information to files");
            }
        }
    }
}