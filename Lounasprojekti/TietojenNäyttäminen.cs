// See https://aka.ms/new-console-template for more information
class TietojenNäyttäminen
{

    public void NäytäRavintolat(Dictionary<string, int> a)
    {
        foreach (var item in a)
            Console.WriteLine(item.Key + " " + item.Value);
        Console.ReadLine();
    }

    public void NäytäRavintolanTiedot(List<string> a)
    {
        foreach (var item in a)
            Console.WriteLine(item);
        Console.ReadLine();
    }
}