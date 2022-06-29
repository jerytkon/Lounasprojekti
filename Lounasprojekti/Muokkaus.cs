// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;
//b.IlmoittauduLounaalle(2, 2);

class Muokkaus
{

    LounasDBContext db = new LounasDBContext();

    public void LisääArvio(int ravintolaId, int käyttäjäId, int arvosana, string kommentti = "")
    {

        if (arvosana < 1 || arvosana > 5)
        {
            Console.WriteLine("Arvosanan tulee olla väliltä 1-5, yritä uudelleen");
            Console.ReadLine();
            return;
        }
        if (kommentti.Length > 200)
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
        Console.WriteLine("Arvio lisätty");
        Console.ReadLine();
    }

    public void LisääArvio(int ravintolaId, int käyttäjäId)
    {

        Console.Write($"Anna arvosana 1-5 ravintolalle:");
        var arvosana = int.Parse(Console.ReadLine());
        if (arvosana < 1 || arvosana > 5)
        {
            Console.WriteLine("Arvosanan tulee olla väliltä 1-5, yritä uudelleen");
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
        Console.WriteLine("Arvio lisätty");
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

    public void AloitaLounastapahtuma(int ravintolaId, int käyttäjäId, DateTime pvm)
    {
        var uusi = new Lounastapahtuma()
        {
            RavintolaId = ravintolaId,
            Päivämäärä = pvm
        };

        db.Lounastapahtumas.Add(uusi);
        db.SaveChanges();


        var uusi2 = new Lounasseura()
        {
            LounastapahtumaId = (from i in db.Lounastapahtumas
                                 where i.RavintolaId == ravintolaId && i.Päivämäärä == pvm
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
        var lounastapahtumaId = (from i in db.Lounastapahtumas
                                 where i.RavintolaId == ravintolaId && i.Päivämäärä == DateTime.Today
                                 select i.LounastapahtumaId).FirstOrDefault();

        if (lounastapahtumaId == 0)
        {
            AloitaLounastapahtuma(ravintolaId, käyttäjäId);
            Console.WriteLine("Lounastapahtuma luotu");
            Console.ReadLine();

        }
        else
        {
            var uusi = new Lounasseura()
            {
                LounastapahtumaId = lounastapahtumaId,
                KäyttäjäId = käyttäjäId
            };

            db.Lounasseuras.Add(uusi);
            db.SaveChanges();
            Console.WriteLine("Sinut on lisätty lounaalle");
            Console.ReadLine();
        }
    }

    public void IlmoittauduLounaalle(int ravintolaId, int käyttäjäId, DateTime pvm)
    {
        var lounastapahtumaId = (from i in db.Lounastapahtumas
                                 where i.RavintolaId == ravintolaId && i.Päivämäärä == pvm
                                 select i.LounastapahtumaId).FirstOrDefault();

        if (lounastapahtumaId == 0)
        {
            AloitaLounastapahtuma(ravintolaId, käyttäjäId, pvm);
            Console.WriteLine("Lounastapahtuma luotu");
            Console.ReadLine();
        }
        else
        {
            var uusi = new Lounasseura()
            {
                LounastapahtumaId = lounastapahtumaId,
                KäyttäjäId = käyttäjäId
            };

            db.Lounasseuras.Add(uusi);
            db.SaveChanges();
            Console.WriteLine("Sinut on lisätty lounaalle");
            Console.ReadLine();
        }

    }

    public void IlmoittauduLounaalle(string ravintolanNimi, int käyttäjäId)
    {
        var kysely = (from i in db.Ravintolas
                      where i.RavintolanNimi == ravintolanNimi
                      select i.RavintolaId).FirstOrDefault();

        IlmoittauduLounaalle(kysely, käyttäjäId);

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
}