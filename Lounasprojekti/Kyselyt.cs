// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using LounasprojektiLib.Models;
using System.Globalization;

public class Kyselyt
{
    LounasDBContext db = new LounasDBContext();
    CultureInfo culture = CultureInfo.InvariantCulture;
    
    public List<Tuple<string, Action>> LuoRavintolatValikko(ConsoleMenu con)
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
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(r.RavintolaId, con)));
        }

        return map;
    }

    public List<string> HaeRuokailijatTänään(int ravintolaID)
    {
        var pvmMuokattu = DateTime.Today.Date.ToString("yyyy-MM-dd");
        var kysely = (from i in db.Lounastapahtumas
                      join i2 in db.Lounasseuras on i.LounastapahtumaId equals i2.LounastapahtumaId
                      join i3 in db.Käyttäjäs on i2.KäyttäjäId equals i3.KäyttäjäId
                      where i.RavintolaId == ravintolaID && i.Päivämäärä.Date.ToString() == pvmMuokattu
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

    public List<Tuple<string, Action>> SelaaTop3RavintolatValikko(ConsoleMenu con)
    {
        // Luodaan sanakirja, jossa ravintolat ja päivän ruokailijoiden lukumäärä
        var ruokailijatLkm = HaeRuokailijatLkm();

        // Haetaan ravintoloiden arvosteluiden keskiarvot
        var kysely = (from i in db.Ravintolas
                      select new
                      {
                          RavintolanNimi = i.RavintolanNimi,
                          RavintolaId = i.RavintolaId,
                          Keskiarvo = (from j in db.Arvios
                                       where j.RavintolaId == i.RavintolaId
                                       select j.Arvosana).Average()
                      });

        var järjestetty = (from i in kysely
                           orderby i.Keskiarvo descending
                           select i).Take(3);


        // Luodaan Tuple-lista valikon AddRange()-metodia varten:
        //      valikon alaotsikko (string): Ravintolan nimi, ruokailijoiden lukumäärä sekä keskiarvo arvosteluista
        //      Action: Näyttää ravintolan tiedot
        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        foreach (var r in järjestetty)
        {
            // Luodaan string, jossa ravintolan nimi ja ruokailijoiden lukumäärä
            var ruokailijat = ruokailijatLkm[r.RavintolanNimi] > 0 ? $"{ruokailijatLkm[r.RavintolanNimi]} ruokailija{(ruokailijatLkm[r.RavintolanNimi] == 1 ? "" : "a")} tänään" : "";
            var valikkoNimi = $"{r.RavintolanNimi.PadRight(30)} Keskiarvo: {string.Format("{0:F1}", r.Keskiarvo).PadRight(10)} {ruokailijat}";

            // Lisätään listaan valikon alaotsikko ja Action
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(r.RavintolaId, con)));
        }

        return map;
    }
}
