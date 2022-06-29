// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lounasprojekti;
using ConsoleTools;


var kirjautuminen = new Kirjautuminen();
kirjautuminen.Kirjaudu();


var a = new Kyselyt();
var b = new Muokkaus();
//var c = new TietojenNäyttäminen();

//var demo = a.SelaaRavintolat();
//foreach (var item in demo)
//    Console.WriteLine(item.Key + " " + item.Value);
//a.HaeRuokailijat(1);

//b.LisääArvio(2, 1, 4, "Olipas hyvää.");

var ravintolaSubMenu = new ConsoleMenu(args, level: 2)
    .Add("Ilmoittaudu lounasseuraksi", () => b.IlmoittauduLounaalle(TietojenNäyttäminen.RavintolaID, kirjautuminen.KäyttäjäId, DateTime.Today.AddDays(1)))
    .Add("Arvioi lounasravintola", () => b.LisääArvio(TietojenNäyttäminen.RavintolaID, kirjautuminen.KäyttäjäId))
  //.Add("Sub_One",() => NäytäRavintolat(a.SelaaRavintolat()))
  .Add("Sub_Close", ConsoleMenu.Close)
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = "Submenu";
      config.EnableBreadcrumb = true;
      config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
  });

var ravintolatMenu = new ConsoleMenu(args, level: 1)
    .AddRange(a.SelaaRavintolatValikko(ravintolaSubMenu))

  //.Add("Sub_One",() => NäytäRavintolat(a.SelaaRavintolat()))
  .Add("Sub_Close", ConsoleMenu.Close)
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = "Submenu";
      config.EnableBreadcrumb = true;
      config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
  });

var käyttäjäMenu = new ConsoleMenu(args, level: 0)
  .Add("Näytä ravintolat", ravintolatMenu.Show)
  .Add("Change me", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
  .Add("Close", ConsoleMenu.Close)
  //.Add("Action then Close", (thisMenu) => { SomeAction("Close"); thisMenu.CloseMenu(); })
  .Add("Exit", () => Environment.Exit(0))
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = "Main menu";
      config.EnableWriteTitle = true;
      config.EnableBreadcrumb = true;
  });

var adminMenu = new ConsoleMenu(args, level: 0)
  .Add("Näytä ravintolat", ravintolatMenu.Show)
  .Add("Lisää ravintola", () => b.LisääRavintola())
  .Add("Change me", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
  .Add("Close", ConsoleMenu.Close)
  //.Add("Action then Close", (thisMenu) => { SomeAction("Close"); thisMenu.CloseMenu(); })
  .Add("Exit", () => Environment.Exit(0))
  .Configure(config =>
  {
      config.Selector = "--> ";
      config.EnableFilter = false;
      config.Title = "Main menu";
      config.EnableWriteTitle = true;
      config.EnableBreadcrumb = true;
  });



if (kirjautuminen.OnAdmin == 0)
{
käyttäjäMenu.Show();
}

if (kirjautuminen.OnAdmin == 1)
{
adminMenu.Show();
}


