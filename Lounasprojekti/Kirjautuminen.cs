// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;

class Kirjautuminen
{
    LounasDBContext db = new LounasDBContext();
    public int KäyttäjäId { get; set; } = 0;
    public string KäyttäjäNimi { get; set; }

    public int OnAdmin { get; set; }

    Muokkaus b = new Muokkaus();
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

            OnAdmin = Convert.ToInt32(db.Käyttäjäs.Find(KäyttäjäId).OnAdmin);

            if (KäyttäjäId == 0)
            {
                
                Console.WriteLine($"Luodaanko uusi käyttäjä nimellä{KäyttäjäNimi}");
                Console.WriteLine("Paina Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                    b.LisääUusiKäyttäjä(KäyttäjäNimi);
                if (Console.ReadKey().Key == ConsoleKey.N)
                {
                    continue;
                }


                //var kylläei = new ConsoleMenu();
                
                //kylläei.Add("Kyllä", () => b.LisääUusiKäyttäjä(KäyttäjäNimi));
                //kylläei.Add("Ei", () => KäyttäjäNimi = null)
                //.Configure(config =>
                // {
                //     config.Selector = "--> ";
                //     config.EnableFilter = false;
                //     config.Title = "Main menu";
                //     config.EnableWriteTitle = true;
                //     config.EnableBreadcrumb = true;
                // });
                //kylläei.Show();
                //if (KäyttäjäNimi == null)
                //{
                //    continue;
                //}


            }
        }
    }

    //public bool OnAdmin(int käyttäjäId)
    //{
    //    var admin = from i in 

    //}
}


