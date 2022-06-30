﻿// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
//b.IlmoittauduLounaalle(2, 2);

class Muokkaus
{

    LounasDBContext db = new LounasDBContext();

    public List<string> LuoRavintolatValikkoLista()
    {
        var Kysely = new Kyselyt();
        var valikkoLista = new List<string>();
        var ruokailijatLkm = Kysely.HaeRuokailijatLkm();

        var kysely = from i in db.Ravintolas
                     select i;

        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        foreach (var r in kysely)
        {
            var ruokailijat = ruokailijatLkm[r.RavintolanNimi] > 0 ? $"{ruokailijatLkm[r.RavintolanNimi]} ruokailija{(ruokailijatLkm[r.RavintolanNimi] == 1 ? "" : "a")} tänään" : "";
            var valikkoNimi = $"{r.RavintolanNimi.PadRight(50)}{ruokailijat}";
            valikkoLista.Add(valikkoNimi);
        }
        return valikkoLista;
    }

    public void PäivitäRavintolatValikko(ConsoleMenu con)
    {
        var valikkoLista = LuoRavintolatValikkoLista();
        var index = 0;
        foreach (var item in con.Items)
        {
            try
            {
                item.Name = valikkoLista[index];
                index++;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                item.Name = item.Name;
                index++;
            }
        }
    }

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

    public List<string> LuoKommentitValikkoLista()
    {
        var valikkoLista = new List<string>();
        var kysely = from i in db.Arvios
                     join i2 in db.Käyttäjäs on i.KäyttäjäId equals i2.KäyttäjäId
                     orderby i.Päivämäärä descending
                     select new { käyttäjänimi = i2.Käyttäjänimi, kommentti = i.Kommentti, päivämäärä = i.Päivämäärä };

        foreach (var item in kysely)
        {
            var valikkoNimi = $"{item.käyttäjänimi.PadRight(20)}{item.päivämäärä.ToString().PadRight(30)}{item.kommentti}";
            valikkoLista.Add(valikkoNimi);
        }
        return valikkoLista;
    }

    public void PäivitäKommenttiValikko(ConsoleMenu con)
    {
        var valikkoLista = LuoKommentitValikkoLista();
        var index = 0;
        foreach (var item in con.Items)
        {
            try
            {
                item.Name = valikkoLista[index];
                index++;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                item.Name = item.Name;
                index++;
            }
        }
    }
}