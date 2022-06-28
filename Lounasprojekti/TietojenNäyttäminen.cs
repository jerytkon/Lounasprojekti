// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;

static class TietojenNäyttäminen
{
    public static string RavintolaNimi { get; set; }
    public static int RavintolaID { get; set; }

    static LounasDBContext db = new LounasDBContext();

    public static void NäytäRavintolat(Dictionary<string, int> a)
    {
        
        foreach (var item in a)
            Console.WriteLine(item.Key + " " + item.Value);
        Console.ReadLine();
    }

    public static void NäytäRavintolanTiedot(List<string> a)
    {
        PäivitäRavintolaId(a[0]);
        RavintolaNimi = a[0];
        foreach (var item in a)
            Console.WriteLine(item);
        Console.ReadLine();
    }


    public static void NäytäRavintolanTiedot(List<string> a, ConsoleMenu con)
    {
        PäivitäRavintolaId(a[0]);
        RavintolaNimi = a[0];
        Console.Clear();
        foreach (var item in a)
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
}