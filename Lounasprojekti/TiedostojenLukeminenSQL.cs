// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;

public class TiedostojenLukeminenSQL
{
    public static void LueKonsultit(string path)
    {
        LounasDBContext db = new LounasDBContext();
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            var käyttäjä = new Käyttäjä
            {
                Käyttäjänimi = line
            };
            db.Käyttäjäs.Add(käyttäjä);
            db.SaveChanges();
        }
    }
}


