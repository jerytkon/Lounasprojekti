// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using Lounasprojekti.Models;

class TietojenNäyttäminen
{
    public string RavintolaNimi { get; set; }
    public int RavintolaID { get; set; }

    LounasDBContext db = new LounasDBContext();

    public void NäytäRavintolat(Dictionary<string, int> a)
    {
        
        foreach (var item in a)
            Console.WriteLine(item.Key + " " + item.Value);
        Console.ReadLine();
    }

    public void NäytäRavintolanTiedot(List<string> a)
    {
        PäivitäRavintolaId(a[0]);
        RavintolaNimi = a[0];
        foreach (var item in a)
            Console.WriteLine(item);
        Console.ReadLine();
    }


    public void NäytäRavintolanTiedot(List<string> a, ConsoleMenu con)
    {
        PäivitäRavintolaId(a[0]);
        RavintolaNimi = a[0];
        Console.Clear();
        foreach (var item in a)
            Console.WriteLine(item);
        Console.ReadLine();
        con.Show();
    }

    public void PäivitäRavintolaId(string ravintolanNimi)
    {
        var kysely = (from i in db.Ravintolas
                      where i.RavintolanNimi == ravintolanNimi
                      select i.RavintolaId).FirstOrDefault();

        RavintolaID = kysely;

    }
}