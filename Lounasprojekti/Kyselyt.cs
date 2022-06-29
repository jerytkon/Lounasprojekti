// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;
using System.Globalization;

class Kyselyt
{
    LounasDBContext db = new LounasDBContext();
    CultureInfo culture = CultureInfo.InvariantCulture;

    public List<Tuple<string, Action>> SelaaRavintolatValikko(ConsoleMenu con)
    {
        // Luodaan sanakirja, jossa ravintolat ja ruokailijoiden lukumäärä
        var ruokailijatLkm = HaeRuokailijatLkm();

        var kysely = from i in db.Ravintolas
                     select i;

        // Luodaan Tuple-lista valikon AddRange()-metodia varten:
        //       valikon alaotsikko (string): Ravintolan nimi ja ruokailijoiden lukumäärä
        //       Action: Näyttää ravintolan tiedot
        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        foreach (var r in kysely)
        {
            // Luodaan string, jossa ravintolan nimi ja ruokailijoiden lukumäärä
            var ruokailijat = ruokailijatLkm[r.RavintolanNimi] > 0 ? $"{ruokailijatLkm[r.RavintolanNimi]} ruokailija{(ruokailijatLkm[r.RavintolanNimi] == 1 ? "" : "a")} tänään" : "";
            var valikkoNimi = $"{r.RavintolanNimi.PadRight(50)}{ruokailijat}";

            // Lisätään listaan valikon alaotsikko ja Action
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(HaeRavintolanTiedot(r.RavintolaId), con)));
        }

        return map;
    }

    public List<Tuple<string, Action>> SelaaRavintolatValikko(ConsoleMenu con, DateTime pvm)
    {
        // Luodaan sanakirja, jossa ravintolat ja ruokailijoiden lukumäärä
        var ruokailijatLkm = HaeRuokailijatLkm(pvm);

        var kysely = from i in db.Ravintolas
                     select i;

        // Luodaan Tuple-lista valikon AddRange()-metodia varten:
        //      valikon alaotsikko (string): Ravintolan nimi ja ruokailijoiden lukumäärä
        //      Action: Näyttää ravintolan tiedot
        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        foreach (var r in kysely)
        {
            // Luodaan string, jossa ravintolan nimi ja ruokailijoiden lukumäärä
            var ruokailijat = ruokailijatLkm[r.RavintolanNimi] > 0 ? $"{ruokailijatLkm[r.RavintolanNimi]} ruokailija{(ruokailijatLkm[r.RavintolanNimi] == 1 ? "" : "a")} tänään" : "";
            var valikkoNimi = $"{r.RavintolanNimi.PadRight(50)}{ruokailijat}";

            // Lisätään listaan valikon alaotsikko ja Action
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(HaeRavintolanTiedot(r.RavintolaId), con)));
        }

        return map;
    }

    public List<string> HaeRavintolanTiedot(int ravintolaID)
    {
        // Lisää ravintolan kommentit listan loppuun
        var ravintola = db.Ravintolas.Find(ravintolaID);
        var kysely1 = (from i in db.Arvios
                       where i.RavintolaId == ravintolaID
                       select i.Arvosana).Average();

        var lista = new List<string>();
        lista.Add(ravintola.RavintolanNimi);
        lista.Add(ravintola.Kategoria);
        lista.Add(kysely1.ToString());
        lista.Add(ravintola.Osoite);
        lista.Add(ravintola.Postinumero);
        lista.Add(ravintola.Postitoimipaikka);
        lista.Add(ravintola.Verkkosivu);
        return lista;
    }
    public List<string> HaeRuokailijat(int ravintolaID)
    {
        var kysely = (from i in db.Lounastapahtumas
                      join i2 in db.Lounasseuras on i.LounastapahtumaId equals i2.LounastapahtumaId
                      join i3 in db.Käyttäjäs on i2.KäyttäjäId equals i3.KäyttäjäId
                      where i.RavintolaId == ravintolaID
                      select i3.Käyttäjänimi).ToList();
        return kysely;
    }

    public List<string> HaeRuokailijatTänään(int ravintolaID)
    {
        var newDateTime = DateTime.Today.Date.ToString("yyyy-MM-dd");
        var kysely = (from i in db.Lounastapahtumas
                      join i2 in db.Lounasseuras on i.LounastapahtumaId equals i2.LounastapahtumaId
                      join i3 in db.Käyttäjäs on i2.KäyttäjäId equals i3.KäyttäjäId
                      where i.RavintolaId == ravintolaID && i.Päivämäärä.Date.ToString() == newDateTime
                      select i3.Käyttäjänimi).ToList();
        return kysely;
    }

    public Dictionary<string, int> HaeRuokailijatLkm()
    {
        Dictionary<string, int> ruokailijatLkm = new Dictionary<string, int>();
        var pvmMuokattu = DateTime.Today.Date.ToString("yyyy-MM-dd");
        var kysely = (from i in db.VSyömäänRekisteröityneets
                      where i.Päivämäärä.ToString() == pvmMuokattu || i.Päivämäärä == null
                      select i);

        foreach (var item in kysely)
        {
            if (ruokailijatLkm.ContainsKey(item.RavintolanNimi))
                continue;
            ruokailijatLkm.Add(item.RavintolanNimi, Convert.ToInt32(item.SyömäänTulijat));
        }
        return ruokailijatLkm;
    }

    public Dictionary<string, int> HaeRuokailijatLkm(DateTime pvm)
    {
        Dictionary<string, int> ruokailijatLkm = new Dictionary<string, int>();
        var pvmMuokattu = pvm.Date.ToString("yyyy-MM-dd");
        var kysely = (from i in db.VSyömäänRekisteröityneets
                      where i.Päivämäärä.ToString() == pvmMuokattu || i.Päivämäärä == null
                      select i);

        foreach (var item in kysely)
        {
            if (ruokailijatLkm.ContainsKey(item.RavintolanNimi))
                continue;
            ruokailijatLkm.Add(item.RavintolanNimi, Convert.ToInt32(item.SyömäänTulijat));
        }
        return ruokailijatLkm;
    }
}
