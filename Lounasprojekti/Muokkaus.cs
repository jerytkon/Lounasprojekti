// See https://aka.ms/new-console-template for more information
using Lounasprojekti.Models;
//b.IlmoittauduLounaalle(2, 2);

class Muokkaus
{

    LounasDBContext db = new LounasDBContext();

    public void LisääArvio(int ravintolaId, int käyttäjäId, int arvosana, string kommentti = "")
    {
        // lisää tarkistukset: arvosana 1-5, kommentti.Length < 200

        var uusi = new Arvio()
        {
            RavintolaId = ravintolaId,
            KäyttäjäId = käyttäjäId,
            Päivämäärä = DateTime.Today,
            Arvosana = arvosana,
            Kommentti = kommentti,
        };

        // lisää try catch

        db.Arvios.Add(uusi);
        db.SaveChanges();
    }

    public void LisääArvio(int ravintolaId, int käyttäjäId)
    {
        // lisää tarkistukset: arvosana 1-5, kommentti.Length < 200
        Console.Write($"Anna arvosana 1-5 ravintolalle:");
        var arvosana = int.Parse(Console.ReadLine());
        Console.Write("Anna kommentti ravintolalle:");
        var kommentti = Console.ReadLine();


        var uusi = new Arvio()
        {
            RavintolaId = ravintolaId,
            KäyttäjäId = käyttäjäId,
            Päivämäärä = DateTime.Today,
            Arvosana = arvosana,
            Kommentti = kommentti,
        };

        // lisää try catch

        db.Arvios.Add(uusi);
        db.SaveChanges();
    }

    public void AloitaLounastapahtuma(int käyttäjäId, int ravintolaId)
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

    public void AloitaLounastapahtuma(int käyttäjäId, int ravintolaId, DateTime pvm)
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

    public void IlmoittauduLounaalle(int ravintolaId, int käyttäjäId)
    {
        var lounastapahtumaId = (from i in db.Lounastapahtumas
                                 where i.RavintolaId == ravintolaId && i.Päivämäärä == DateTime.Today
                                 select i.LounastapahtumaId).FirstOrDefault();

        if (lounastapahtumaId == 0)
        {
            AloitaLounastapahtuma(ravintolaId, käyttäjäId);
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
        }

    }

    public void IlmoittauduLounaalle(string ravintolanNimi, int käyttäjäId)
    {
        var kysely = (from i in db.Ravintolas
                     where i.RavintolanNimi == ravintolanNimi
                     select i.RavintolaId).FirstOrDefault();

        IlmoittauduLounaalle(kysely, käyttäjäId);

}
}