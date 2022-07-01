// See https://aka.ms/new-console-template for more information
using LounasprojektiLib.Models;

/** <summary>
 * Luokalla lisätään tiedostosta tietokantaan tietoa
 * </summary> */
public class TiedostojenLukeminenSQL
{
    public static void LueKonsultit(string polku)
    {
        LounasDBContext db = new LounasDBContext();

        var käyttäjänimet = (from i in db.Käyttäjäs
                             select i.Käyttäjänimi).ToList();

        string[] rivit = File.ReadAllLines(polku);
        foreach (string rivi in rivit)
        {
            var käyttäjä = new Käyttäjä
            {
                Käyttäjänimi = rivi
            };
            if (käyttäjänimet.Contains(rivi))
                continue;
            db.Käyttäjäs.Add(käyttäjä);
            db.SaveChanges();
        }
    }

    public static void LueRavintolat(string polku)
    {
        LounasDBContext db = new LounasDBContext();

        var ravintolaNimet = (from i in db.Ravintolas
                             select i.RavintolanNimi).ToList();

        string[] rivit = File.ReadAllLines(polku);
        foreach (string rivi in rivit)
        {
            var osat = rivi.Split(';');
            var ravintola = new Ravintola
            {
                RavintolanNimi = osat[0],
                Osoite = osat[1],
                Postinumero = osat[2],
                Postitoimipaikka = osat[3],
                Verkkosivu = osat[4],   
                MenuUrl = osat[5],
                Kategoria = osat[6]
            };
            if (ravintolaNimet.Contains(ravintola.RavintolanNimi))
                continue;
            db.Ravintolas.Add(ravintola);
            db.SaveChanges();
        }
    }
}


