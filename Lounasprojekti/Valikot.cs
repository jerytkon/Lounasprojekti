// See https://aka.ms/new-console-template for more information
using ConsoleTools;

class Valikot
{

    public ConsoleMenu ravintolaSubMenu(string[] args, Kirjautuminen kirjautuminen)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var ravintolaSubMenu = new ConsoleMenu(args, level: 2)
            .Add("Ilmoittaudu lounasseuraksi", () => muokkausObjekti.IlmoittauduLounaalle(TietojenNäyttäminen.RavintolaID, kirjautuminen.KäyttäjäId, DateTime.Today.AddDays(1)))
            .Add("Arvioi lounasravintola", () => muokkausObjekti.LisääArvio(TietojenNäyttäminen.RavintolaID, kirjautuminen.KäyttäjäId))
            .Add("Sub_Close", ConsoleMenu.Close)
            .Configure(config =>
    {
        config.Selector = "--> ";
        config.EnableFilter = false;
        config.Title = "Submenu";
        config.EnableBreadcrumb = true;
        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
    });
        return ravintolaSubMenu;
    }

    public ConsoleMenu ravintolatMenu(string[] args, Kirjautuminen kirjautuminen, ConsoleMenu ravintolatSubMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var ravintolatMenu = new ConsoleMenu(args, level: 1)
          .AddRange(kyselyObjekti.SelaaRavintolatValikko(ravintolatSubMenu))
          .Add("Sub_Close", ConsoleMenu.Close)
          .Configure(config =>
      {
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.Title = "Submenu";
          config.EnableBreadcrumb = true;
          config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
      });
        return ravintolatMenu;
    }
    public ConsoleMenu käyttäjäMenu(string[] args, ConsoleMenu ravintolatMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var käyttäjäMenu = new ConsoleMenu(args, level: 0)
          .Add("Näytä ravintolat", ravintolatMenu.Show)
          .Add("Change me", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
          //.Add("Close", ConsoleMenu.Close)
          .Add("Exit", () => Environment.Exit(0))
      .Configure(config =>
      {
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.Title = "Main menu";
          config.EnableWriteTitle = true;
          config.EnableBreadcrumb = true;
      }); 
        return käyttäjäMenu;
    }
    public ConsoleMenu adminMenu(string[] args, ConsoleMenu ravintolatMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var adminMenu = new ConsoleMenu(args, level: 0)
            .Add("Näytä ravintolat", ravintolatMenu.Show)
            .Add("Lisää ravintola", () => muokkausObjekti.LisääRavintola())
            .Add("Change me", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
            .Add("Exit", () => Environment.Exit(0))
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = "Main menu";
      config.EnableWriteTitle = true;
      config.EnableBreadcrumb = true;
  });
        return adminMenu;
    }
}

