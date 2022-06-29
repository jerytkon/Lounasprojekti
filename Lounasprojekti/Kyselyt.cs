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
        //Selaa ravintolat ja näytä ilmoittautuneet syömään
        // lisätään where ehto näyttämään vain tälle päivälle

        var ruokailijatLkm = palautaRuokailijatLkm();

        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();       
        var kysely2 = from i in db.Ravintolas
                     select i;
        
        foreach (var item in kysely2)
        {
            var ruokailijat = ruokailijatLkm[item.RavintolanNimi] > 0 ? $"{ruokailijatLkm[item.RavintolanNimi]} ruokailija{(ruokailijatLkm[item.RavintolanNimi] == 1 ? "" : "a")} tänään" : "";
            var valikkoNimi = $"{item.RavintolanNimi.PadRight(50)}{ruokailijat}";
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(HaeRavintolanTiedot(item.RavintolaId), con)));
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

    public Dictionary<string, int> palautaRuokailijatLkm()
    {
        Dictionary<string, int> ruokailijatLkm = new Dictionary<string, int>();
        var newDateTime = DateTime.Today.Date.ToString("yyyy-MM-dd");
        var kysely = (from i in db.VSyömäänRekisteröityneets
                      where i.Päivämäärä.ToString() == newDateTime || i.Päivämäärä == null
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