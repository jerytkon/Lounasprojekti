// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;

class Kyselyt
{
    LounasDBContext db = new LounasDBContext();


    //Selaa ravintoloita eri parametreilla
    public Dictionary<string, int> SelaaRavintolat()
    {
        //Selaa ravintolat ja näytä ilmoittautuneet syömään
        // lisätään where ehto näyttämään vain tälle päivälle
        Dictionary<string, int> map = new Dictionary<string, int>();
        var kysely = (from i in db.Ravintolas
                      join i2 in db.Lounastapahtumas on i.RavintolaId equals i2.RavintolaId
                      join i3 in db.Lounasseuras on i2.LounastapahtumaId equals i3.LounastapahtumaId
                      group i by i.RavintolanNimi into g
                      select new { Ravintolanimi = g.Key, Count = g.Count() }).ToList();
        foreach (var item in kysely)
        {
            map.Add(item.Ravintolanimi, item.Count);
        }
        return map;
    }
    public List<Tuple<string, Action>> SelaaRavintolatValikko()
    {
        //var a = new TietojenNäyttäminen();
        //Selaa ravintolat ja näytä ilmoittautuneet syömään
        // lisätään where ehto näyttämään vain tälle päivälle
        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        var kysely = from i in db.Ravintolas
                      select i;
        foreach (var item in kysely)
        {
            map.Add(Tuple.Create<string, Action>(item.RavintolanNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(HaeRavintolanTiedot(item.RavintolaId))));
        }
        return map;
    }

    public List<Tuple<string, Action>> SelaaRavintolatValikko(ConsoleMenu con)
    {
        //var a = new TietojenNäyttäminen();
        //Selaa ravintolat ja näytä ilmoittautuneet syömään
        // lisätään where ehto näyttämään vain tälle päivälle
        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        var kysely = from i in db.Ravintolas
                     select i;
        foreach (var item in kysely)
        {
            map.Add(Tuple.Create<string, Action>(item.RavintolanNimi, () => TietojenNäyttäminen.NäytäRavintolanTiedot(HaeRavintolanTiedot(item.RavintolaId), con)));
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


}