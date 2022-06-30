// See https://aka.ms/new-console-template for more information
using ConsoleTools;

class Valikot
{
    public string appAscii { get; set; } = @"

     _                                                     
    | |                                                    
    | |     ___  _   _ _ __   __ _ ___    __ _ _ __  _ __  
    | |    / _ \| | | | '_ \ / _` / __|  / _` | '_ \| '_ \ 
    | |___| (_) | |_| | | | | (_| \__ \ | (_| | |_) | |_) |
    \_____/\___/ \__,_|_| |_|\__,_|___/  \__,_| .__/| .__/ 
                                              | |   | |    
                                              |_|   |_|    

";
    public ConsoleMenu kommenttiMenu(string[] args)
    {
        var muokkausObjekti = new Muokkaus();
        var kommenttiMenu = new ConsoleMenu(args, 4)
            .Add("Sensuroi kommentti", () => muokkausObjekti.sensuroiKommentti())
            .Add("Takaisin", ConsoleMenu.Close)
                    .Configure(config =>
                    {
                        config.Selector = "--> ";
                        config.EnableFilter = false;
                        config.Title = appAscii;
                        config.EnableBreadcrumb = true;
                        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                        config.WriteHeaderAction = () => Console.WriteLine("Valitse toiminto: ");
                    }
                    );
        return kommenttiMenu;
    }
    public ConsoleMenu kommentitMenu(string[] args, ConsoleMenu kommenttiMenu)
    {
        var valikkojenPäivitysObjekti = new ValikkojenPäivitys();
        var kommentitMenu = new ConsoleMenu(args, 3)
        .AddRange(TietojenNäyttäminen.NäytäKommentitValikko(kommenttiMenu))
        .Add("Päivitä kommentit", (thisMenu) => valikkojenPäivitysObjekti.PäivitäKommenttiValikko(thisMenu))
        .Add("Takaisin", ConsoleMenu.Close)
                    .Configure(config =>
                    {
                        config.Selector = "--> ";
                        config.EnableFilter = false;
                        config.Title = appAscii;
                        config.EnableBreadcrumb = true;
                        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                        config.WriteHeaderAction = () => Console.WriteLine("Valitse kommentti: ");
                    });
        return kommentitMenu;
    }
    public ConsoleMenu ravintolaSubMenu(string[] args, Kirjautuminen kirjautuminen)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var ravintolaSubMenu = new ConsoleMenu(args, level: 2)
            .Add("Ilmoittaudu lounasseuraksi", () => muokkausObjekti.IlmoittauduLounaalle(TietojenNäyttäminen.RavintolaID, kirjautuminen.KäyttäjäId))
            .Add("Arvioi lounasravintola", () => muokkausObjekti.LisääArvio(TietojenNäyttäminen.RavintolaID, kirjautuminen.KäyttäjäId))
            .Add("Näytä ruokailijat", () => TietojenNäyttäminen.NäytäRavintolanRuokailijat(TietojenNäyttäminen.RavintolaID))
            .Add("Näytä arvostelut", () => TietojenNäyttäminen.NäytäRavintolanArvostelut(TietojenNäyttäminen.RavintolaID))
            .Add(kirjautuminen.OnAdmin == 1 ? "Poista ravintola" : "Admin ominaisuus", 
                kirjautuminen.OnAdmin == 1 ? 
                () => muokkausObjekti.PoistaRavintola(TietojenNäyttäminen.RavintolaID) : 
                () => { Console.WriteLine("Et ole admin! Älä paina tästä. Enterillä voit poistua."); 
                Console.ReadLine(); })
            .Add("Takaisin", ConsoleMenu.Close)
            .Configure(config =>
    {
        config.Selector = "--> ";
        config.EnableFilter = false;
        config.Title = appAscii;
        config.EnableBreadcrumb = true;
        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
        config.WriteHeaderAction = () => Console.WriteLine("Valitse toiminto: ");
    });
        return ravintolaSubMenu;
    }

    public ConsoleMenu ravintolatMenu(string[] args, Kirjautuminen kirjautuminen, ConsoleMenu ravintolatSubMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var valikkojenPäivitysObjekti = new ValikkojenPäivitys();
        var ravintolatMenu = new ConsoleMenu(args, level: 1)
          .AddRange(kyselyObjekti.LuoRavintolatValikko(ravintolatSubMenu))
          .Add("Päivitä ravintolat", (thisMenu) => valikkojenPäivitysObjekti.PäivitäRavintolatValikko(thisMenu))
          .Add("Takaisin", ConsoleMenu.Close)
          .Configure(config =>
      {
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.Title = "";
          config.EnableBreadcrumb = true;
          config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
          config.WriteHeaderAction = () => Console.WriteLine("Valitse ravintola: ");
      });
        return ravintolatMenu;
    }

    public ConsoleMenu top3Menu(string[] args, Kirjautuminen kirjautuminen, ConsoleMenu ravintolatSubMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var top3Menu = new ConsoleMenu(args, level: 1)
          .AddRange(kyselyObjekti.SelaaTop3RavintolatValikko(ravintolatSubMenu))
          .Add("Takaisin", ConsoleMenu.Close)
          .Configure(config =>
          {
              config.Selector = "--> ";
              config.EnableFilter = false;
              config.Title = "";
              config.EnableBreadcrumb = true;
              config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              config.WriteHeaderAction = () => Console.WriteLine("Valitse ravintola: ");
          });
        return top3Menu;
    }

    public ConsoleMenu käyttäjäMenu(string[] args, ConsoleMenu ravintolatMenu, ConsoleMenu kommentitMenu, ConsoleMenu top3Menu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var käyttäjäMenu = new ConsoleMenu(args, level: 0)
          .Add("Näytä kaikki ravintolat", ravintolatMenu.Show)
          .Add("Näytä 3 parhaiten arvosteltua ravintolaa", top3Menu.Show)
          .Add("Sulje", () => Environment.Exit(0))
      .Configure(config =>
      {
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.Title = appAscii;
          config.EnableWriteTitle = false;
          config.EnableBreadcrumb = true;
          config.EnableWriteTitle = false;
          config.WriteHeaderAction = () => Console.WriteLine("Valitse toiminto: ");
      });
        return käyttäjäMenu;
    }
    public ConsoleMenu adminMenu(string[] args, ConsoleMenu ravintolatMenu, ConsoleMenu kommentitMenu, ConsoleMenu listaaKäyttäjätMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var adminMenu = new ConsoleMenu(args, level: 0)
            .Add("Näytä ravintolat", ravintolatMenu.Show)
            .Add("Näytä kommentit", () => kommentitMenu.Show())
            .Add("Käyttäjien hallinta", () => listaaKäyttäjätMenu.Show())
            .Add("Lisää ravintola", () => muokkausObjekti.LisääRavintola())
            .Add("Sulje", () => Environment.Exit(0))
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = appAscii;
      config.EnableWriteTitle = false;
      config.EnableBreadcrumb = true;
      config.WriteHeaderAction = () => Console.WriteLine("Valitse toiminto: ");
  });
        return adminMenu;
    }

    public ConsoleMenu KäyttäjänMuokkausMenu(string[] args)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var käyttäjänpoistoMenu = new ConsoleMenu(args, level: 2)
          .Add("Sensuroi käyttäjänimi", () => muokkausObjekti.SensuroiKäyttäjä())
          .Add("Takaisin", ConsoleMenu.Close)
      //.Add("Exit", () => Environment.Exit(0))
      .Configure(config =>
      {
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.Title = appAscii;
          config.EnableWriteTitle = false;
          config.EnableBreadcrumb = true;
          config.WriteHeaderAction = () => Console.WriteLine("Valitse toiminto: ");
      });
        return käyttäjänpoistoMenu;
    }



    public ConsoleMenu ListaaKäyttäjätMenu(string[] args, ConsoleMenu käyttäjänPoistoMenu)
    {
        var kyselyObjekti = new Kyselyt();
        var muokkausObjekti = new Muokkaus();
        var ListaaKäyttäjätMenu = new ConsoleMenu(args, level: 1)
            .AddRange(TietojenNäyttäminen.ListaaKäyttäjät(käyttäjänPoistoMenu))
            .Add("Takaisin", ConsoleMenu.Close)
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = appAscii;
      config.EnableWriteTitle = false;
      config.EnableBreadcrumb = true;
      config.WriteHeaderAction = () => Console.WriteLine("Valitse käyttäjä: ");
  });
        return ListaaKäyttäjätMenu;
    }




}

