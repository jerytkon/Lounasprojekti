// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using LounasprojektiLib.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
/** <summary>
 * Luokalla tulostetaan konsolille tietoa.
 * </summary> */
public static class TietojenNäyttäminen
{
    public static string RavintolaNimi { get; set; }
    public static int RavintolaID { get; set; }
    public static int ArvioID { get; set; }
    public static int KäyttäjäID { get; set; }
    public static string Verkkosivu { get; set; }
    public static string RuokalistaVerkkosivu { get; set; }


    static LounasDBContext db = new LounasDBContext();

    public static void NäytäRavintolanTiedot(int ravintolanId, ConsoleMenu con)
    {
        var ravintola = db.Ravintolas.Find(ravintolanId);

        var kysely1 = (from i in db.Arvios
                       where i.RavintolaId == ravintolanId
                       select i.Arvosana).Average();

        var valikot = new Valikot();
        PäivitäRavintolaId(ravintola.RavintolanNimi);
        Console.Clear();
        Console.WriteLine(valikot.appAscii);
        Console.WriteLine("".PadRight(9) + ravintola.RavintolanNimi);
        Console.WriteLine();
        Console.WriteLine("Kategoria:".PadRight(15) + ravintola.Kategoria);
        Console.WriteLine("Keskiarvo:".PadRight(15) + string.Format("{0:F1}", kysely1));
        Console.WriteLine("Osoite:".PadRight(15) + ravintola.Osoite);
        Console.WriteLine("".PadRight(15) + ravintola.Postinumero);
        Console.WriteLine("".PadRight(15) + ravintola.Postitoimipaikka);
        Console.WriteLine("Verkkosivu:".PadRight(15) + ravintola.Verkkosivu);
        Console.WriteLine();
        Console.WriteLine("***** Sulje tiedot painamalla enter *****");
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

    public static void PäivitäKommenttiId(string kommentti)
    {
        var kysely = (from i in db.Arvios
                      where i.Kommentti == kommentti
                      select i.ArvioId).First();

        ArvioID = kysely;

    }

    public static void PäivitäKäyttäjäId(int käyttäjäId)
    {
        var kysely = (from i in db.Käyttäjäs
                      where i.KäyttäjäId == käyttäjäId
                      select i.KäyttäjäId).First();

        KäyttäjäID = kysely;

    }

    public static void NäytäRavintolanRuokailijat(int RavintolaID)
    {
        Console.Clear();
        var valikot = new Valikot();
        Console.WriteLine(valikot.appAscii);
        Kyselyt kysely = new Kyselyt();
        var Ruokailijat = kysely.HaeRuokailijatTänään(RavintolaID);
        if (Ruokailijat.Count == 0)
            Console.WriteLine($"Ravintolassa {RavintolaNimi} ei ruokailijoita tänään");
        else
            Console.WriteLine($"Ravintolassa {RavintolaNimi} tänään syömässä:" + Environment.NewLine);
            foreach (var ruokailija in Ruokailijat)
            {
                Console.WriteLine(ruokailija);
            }
        Console.WriteLine(Environment.NewLine + "***** Palaa takaisin painamalla enter *****");
        Console.ReadLine();
    }

    public static List<Tuple<string, Action>> NäytäKommentitValikko(ConsoleMenu con)
    {

        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        var kysely = from i in db.Arvios
                     join i2 in db.Käyttäjäs on i.KäyttäjäId equals i2.KäyttäjäId
                     orderby i.Päivämäärä descending
                     select new { käyttäjänimi = i2.Käyttäjänimi, kommentti = i.Kommentti, päivämäärä = i.Päivämäärä };

        foreach (var item in kysely)
        {
            var valikkoNimi = $"{item.käyttäjänimi.PadRight(20)}{item.päivämäärä.ToString().PadRight(30)}{item.kommentti}";
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () =>
            {
                TietojenNäyttäminen.PäivitäKommenttiId(item.kommentti);
                con.Show();
            }));

        }

        return map;
    }

    public static void NäytäRavintolanArvostelut(int ravintolaId)
    {
        var kommenttiLista = new List<string>();
        kommenttiLista.Add("*************************ARVOSTELUT*************************");

        var kysely = (from i in db.Ravintolas
                      where i.RavintolaId == ravintolaId
                      join i2 in db.Arvios on i.RavintolaId equals i2.RavintolaId
                      join i3 in db.Käyttäjäs on i2.KäyttäjäId equals i3.KäyttäjäId
                      orderby i2.Päivämäärä descending
                      select new
                      {
                          Pvm = i2.Päivämäärä,
                          Arvosana = i2.Arvosana,
                          Kommentti = i2.Kommentti,
                          Nimi = i3.Käyttäjänimi
                      });

        foreach (var item in kysely)
        {
            var kommentti = item.Kommentti;
            if (item.Kommentti.Length > 60)
            {
                kommentti = item.Kommentti.Substring(0, 60) + Environment.NewLine + item.Kommentti.Substring(60);
            }
            var sb = new StringBuilder();
            sb.AppendLine(String.Format("{0, 0}{1, 40}", item.Pvm.ToString(), item.Nimi));
            sb.Append("Arvosana: " + item.Arvosana.ToString() + "/5" + Environment.NewLine);
            sb.Append(String.Format("{0, 40}", kommentti + Environment.NewLine));
            sb.AppendLine("************************************************************");
            kommenttiLista.Add(sb.ToString());
        }
        Console.Clear();
        var valikot = new Valikot();
        Console.WriteLine(valikot.appAscii);
        Console.WriteLine("                       " + RavintolaNimi + Environment.NewLine);
        foreach (var item in kommenttiLista)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine(Environment.NewLine + "***** Palaa takaisin painamalla enter *****");
        Console.ReadLine();
    }

    public static List<Tuple<string, Action>> ListaaKäyttäjät(ConsoleMenu con)
    {
        List<Tuple<string, Action>> map = new List<Tuple<string, Action>>();
        var kysely = (from i in db.Käyttäjäs
                      select i).ToList();

        foreach (var item in kysely)
        {
            var valikkoNimi = $"{item.Käyttäjänimi.PadRight(20)}ID: {item.KäyttäjäId.ToString()}";
            map.Add(Tuple.Create<string, Action>(valikkoNimi, () =>
            {
                TietojenNäyttäminen.PäivitäKäyttäjäId(item.KäyttäjäId);
                con.Show();
            }));
        }

        return map;
    }


    public static void PäivitäRuokalistaVerkkosivu()
    {
        var kysely = (from i in db.Ravintolas
                     where i.RavintolaId == RavintolaID
                     select i.MenuUrl).First();
        RuokalistaVerkkosivu = kysely;
    }

   public static void AvaaRuokalistaUrl()
    {
        PäivitäRuokalistaVerkkosivu();
        var url = RuokalistaVerkkosivu;
        if (String.IsNullOrEmpty(url))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ravintolalla ei tallennettua menu url osoitetta. Paina enter palataksesi.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            return;
        }
        try
        {
            Process.Start(url);
        }
        catch
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }

        }
    }

}