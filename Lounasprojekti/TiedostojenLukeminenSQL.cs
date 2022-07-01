// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;

public class TiedostojenLukeminenSQL
{
    public static void LueKonsultit(string path)
    {
        LounasDBContext db = new LounasDBContext();

        var käyttäjänimet = (from i in db.Käyttäjäs
                             select i.Käyttäjänimi).ToList();

        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            var käyttäjä = new Käyttäjä
            {
                Käyttäjänimi = line
            };
            if (käyttäjänimet.Contains(line))
                continue;
            db.Käyttäjäs.Add(käyttäjä);
            db.SaveChanges();
        }
    }
}


