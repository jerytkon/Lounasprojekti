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
        var valikot = new Valikot();
        Console.WriteLine(valikot.appAscii);
        while (KäyttäjäId == 0)
        {
            if (KäyttäjäNimi == null)
            {
                Console.Write("Anna käyttäjänimi: ");
                KäyttäjäNimi = Console.ReadLine();
                if (KäyttäjäNimi.Length < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Käyttäjänimen tulee olla vähintään 2 merkkiä pitkä");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Yritä uudelleen");
                    KäyttäjäNimi = null;
                    continue;

                }
                if (KäyttäjäNimi.Length > 20)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Käyttäjänimen tulee olla korkeintaan 20 merkkiä pitkä");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Yritä uudelleen");
                    KäyttäjäNimi = null;
                    continue;

                }
            }

            KäyttäjäId = (from i in db.Käyttäjäs
                          where i.Käyttäjänimi == KäyttäjäNimi
                          select i.KäyttäjäId).FirstOrDefault();
            if (KäyttäjäId != 0)
                OnAdmin = Convert.ToInt32(db.Käyttäjäs.Find(KäyttäjäId).OnAdmin);

            if (KäyttäjäId == 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Luodaanko uusi käyttäjä nimellä \"{KäyttäjäNimi}\"");
                Console.WriteLine("Vastaa Y/N");
                ConsoleKeyInfo näppäin;
                näppäin = Console.ReadKey();
                if (näppäin.Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    muokkausObjekti.LisääUusiKäyttäjä(KäyttäjäNimi);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Olet nyt kirjautunut. Paina enter");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
                if (näppäin.Key == ConsoleKey.N)
                {
                    KäyttäjäNimi = null;
                    Console.WriteLine();
                    continue;
                }
            }
        }
    }
}


