// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;

static class TietojenNäyttäminen
{
    public static string RavintolaNimi { get; set; }
    public static int RavintolaID { get; set; }

    static LounasDBContext db = new LounasDBContext();

    public static void NäytäRavintolanTiedot(List<string> ravintolanTiedot)
    {
        PäivitäRavintolaId(ravintolanTiedot[0]);
        RavintolaNimi = ravintolanTiedot[0];
        foreach (var item in ravintolanTiedot)
            Console.WriteLine(item);
        Console.ReadLine();
    }


    public static void NäytäRavintolanTiedot(List<string> ravintolanTiedot, ConsoleMenu con)
    {
        PäivitäRavintolaId(ravintolanTiedot[0]);
        RavintolaNimi = ravintolanTiedot[0];
        Console.Clear();
        foreach (var item in ravintolanTiedot)
            Console.WriteLine(item);
        Console.ReadLine();
        con.Show();
    }

    public static void PäivitäRavintolaId(string ravintolanNimi)
    {
        var kysely = (from i in db.Ravintolas
                      where i.RavintolanNimi == ravintolanNimi
                      select i.RavintolaId).First();

        RavintolaID = kysely;

    }

    public static void NäytäRavintolanRuokailijat(int RavintolaID)
    {
        Kyselyt kysely = new Kyselyt();
        var Ruokailijat = kysely.HaeRuokailijatTänään(RavintolaID);
        if (Ruokailijat.Count == 0)
            Console.WriteLine("Ei ruokailijoita");
        else
            foreach (var ruokailija in Ruokailijat)
            {
                Console.WriteLine(ruokailija);
            }
        Console.ReadLine();
    }

    public static List<Tuple<string, Action>> NäytäKommentitValikko( ConsoleMenu con)
    {

        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        var kysely = from i in db.Arvios
                     join i2 in db.Käyttäjäs on i.KäyttäjäId equals i2.KäyttäjäId
                     orderby i.Päivämäärä descending
                     select new { käyttäjänimi = i2.Käyttäjänimi, kommentti = i.Kommentti, päivämäärä = i.Päivämäärä };

        foreach (var item in kysely)
        {
            var valikkoNimi = $"{item.käyttäjänimi.PadRight(20)}{item.päivämäärä.ToString().PadRight(30)}{item.kommentti}";
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () => con.Show()));

        }

        return map;
    }


}