// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;

class ValikkojenPäivitys
{
    LounasDBContext db = new LounasDBContext();
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

    public void PäivitäKäyttäjienHallintaValikko(ConsoleMenu con)
    {

        var valikkoLista = LuoKäyttäjienHallintaLista();
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
   public List<string> LuoKäyttäjienHallintaLista()
    {
        var lista = new List<string>();
        var kysely = (from i in db.Käyttäjäs
                      select i).ToList();
        foreach (var item in kysely)
        {
            lista.Add($"{item.Käyttäjänimi.PadRight(20)}ID: {item.KäyttäjäId.ToString()}");
        }
        return lista;
    }


}