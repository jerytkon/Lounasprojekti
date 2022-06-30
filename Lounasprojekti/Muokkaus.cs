// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
//b.IlmoittauduLounaalle(2, 2);

class Muokkaus
{

    LounasDBContext db = new LounasDBContext();

    public void LisääArvio(int ravintolaId, int käyttäjäId)
    {
        int arvosana;

        Console.Write($"Anna arvosana 1-5 ravintolalle:");
        var arvosanaOnOk = int.TryParse(Console.ReadLine(), out arvosana);
        if (!arvosanaOnOk || arvosana < 1 || arvosana > 5)
        {
            Console.WriteLine("Arviota ei lisätty - arvosanan tulee olla väliltä 1-5. Palaa takaisin painamalla enter");
            Console.ReadLine();
            return;
        }
        Console.Write("Anna kommentti ravintolalle:");
        var kommentti = Console.ReadLine();
        if (kommentti.Length >= 200)
        {
            Console.WriteLine("Kommentin pituus max 200 merkkiä, yritä uudelleen");
            Console.ReadLine();
            return;
        }

        var uusi = new Arvio()
        {
            RavintolaId = ravintolaId,
            KäyttäjäId = käyttäjäId,
            Päivämäärä = DateTime.Today,
            Arvosana = arvosana,
            Kommentti = kommentti,
        };


        db.Arvios.Add(uusi);
        db.SaveChanges();
        Console.Clear();
        Console.WriteLine("Arvio lisätty. Paina enter");
        Console.ReadLine();
    }



    public void AloitaLounastapahtuma(int ravintolaId, int käyttäjäId)
    {
        var uusi = new Lounastapahtuma()
        {
            RavintolaId = ravintolaId,
            Päivämäärä = DateTime.Today
        };

        db.Lounastapahtumas.Add(uusi);
        db.SaveChanges();

        var uusi2 = new Lounasseura()
        {
            LounastapahtumaId = (from i in db.Lounastapahtumas
                                 where i.RavintolaId == ravintolaId && i.Päivämäärä == DateTime.Today
                                 select i.LounastapahtumaId).First(),
            KäyttäjäId = käyttäjäId
        };

        db.Lounasseuras.Add(uusi2);
        db.SaveChanges();
    }

    public void LisääRavintola()
    {
        var uusi = new Ravintola();

        Console.Write("Ravintolan nimi: ");
        uusi.RavintolanNimi = Console.ReadLine();

        Console.Write("Osoite: ");
        uusi.Osoite = Console.ReadLine();

        Console.Write("Postitoimipaikka: ");
        uusi.Postitoimipaikka = Console.ReadLine();

        Console.Write("Postinumero: ");
        uusi.Postinumero = Console.ReadLine();

        Console.Write("Verkkosivu: ");
        uusi.Verkkosivu = Console.ReadLine();

        Console.Write("Kategoria: ");
        uusi.Kategoria = Console.ReadLine();

        db.Ravintolas.Add(uusi);
        db.SaveChanges();
        Console.WriteLine("Ravintola lisätty");
        Console.ReadLine();
    }

    public void IlmoittauduLounaalle(int ravintolaId, int käyttäjäId)
    {
        var valikot = new Valikot();
        var lounastapahtumaId = (from i in db.Lounastapahtumas
                                 where i.RavintolaId == ravintolaId && i.Päivämäärä == DateTime.Today
                                 select i.LounastapahtumaId).FirstOrDefault();




        if (lounastapahtumaId == 0)
        {
            AloitaLounastapahtuma(ravintolaId, käyttäjäId);
            Console.Clear();
            Console.WriteLine(valikot.appAscii);
            Console.WriteLine("Lounastapahtuma luotu. Paina enter");
            Console.ReadLine();

        }
        else
        {
            var uusi = new Lounasseura()
            {
                LounastapahtumaId = lounastapahtumaId,
                KäyttäjäId = käyttäjäId
            };

            var onJoIlmoittautunut = (from i in db.Lounasseuras
                                      where lounastapahtumaId == i.LounastapahtumaId
                                      select i.KäyttäjäId).ToList();

            if (onJoIlmoittautunut.Contains(käyttäjäId))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(valikot.appAscii);
                Console.WriteLine("Olet jo ilmoittautunut mukaan lounaalle. Paina enter jatkaaksesi.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                return;
            }

            db.Lounasseuras.Add(uusi);
            db.SaveChanges();
            Console.Clear();
            
            Console.WriteLine(valikot.appAscii);
            Console.WriteLine("Sinut on lisätty lounaalle. Paina enter");
            Console.ReadLine();
        }
    }

 
    public void LisääUusiKäyttäjä(string käyttäjänimi)
    {
        var uusi = new Käyttäjä()
        {
            Käyttäjänimi = käyttäjänimi
        };
        db.Käyttäjäs.Add(uusi);
        db.SaveChanges();
    }

    public void sensuroiKommentti()
    {
        var id = TietojenNäyttäminen.ArvioID;
        var kysely = (from i in db.Arvios
                     where i.ArvioId == id
                     select i).First();
        if (kysely != null)
        {
            kysely.Kommentti = "sensuroitu";
            EntityState tila = db.Entry<Arvio>(kysely).State;
            Debug.WriteLine(tila);
            db.SaveChanges();
        }
    }


}
