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

    public static void NäytäRavintolanRuokailijat() { }
}