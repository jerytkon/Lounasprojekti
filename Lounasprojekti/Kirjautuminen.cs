// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;

class Kirjautuminen
{
    LounasDBContext db = new LounasDBContext();
    public int KäyttäjäId { get; set; } = 0;
    public string KäyttäjäNimi { get; set; }

    public int OnAdmin { get; set; }

    Muokkaus muokkausObjekti = new Muokkaus();
    public void Kirjaudu()
    {
        while (KäyttäjäId == 0)
        {
            if (KäyttäjäNimi == null)
            {
                Console.Write("Anna käyttäjänimi");
                KäyttäjäNimi = Console.ReadLine();
            }

            KäyttäjäId = (from i in db.Käyttäjäs
                          where i.Käyttäjänimi == KäyttäjäNimi
                          select i.KäyttäjäId).FirstOrDefault();
            if (KäyttäjäId != 0)
                OnAdmin = Convert.ToInt32(db.Käyttäjäs.Find(KäyttäjäId).OnAdmin);

            if (KäyttäjäId == 0)
            {
                Console.WriteLine($"Luodaanko uusi käyttäjä nimellä{KäyttäjäNimi}");
                Console.WriteLine("Vastaa Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                    muokkausObjekti.LisääUusiKäyttäjä(KäyttäjäNimi);
                if (Console.ReadKey().Key == ConsoleKey.N)
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Olet nyt kirjautunut. Paina enter");
                    Console.ReadLine();
                }
            }
        }
    }
}


