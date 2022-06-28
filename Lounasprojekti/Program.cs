// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lounasprojekti;
using ConsoleTools;


var a = new Kyselyt();
var b = new Muokkaus();
var c = new TietojenNäyttäminen();

//var demo = a.SelaaRavintolat();
//foreach (var item in demo)
//    Console.WriteLine(item.Key + " " + item.Value);
//a.HaeRuokailijat(1);

//b.LisääArvio(2, 1, 4, "Olipas hyvää.");

var subMenu2 = new ConsoleMenu(args, level: 2)
    .Add("Ilmoittaudu lounasseuraksi", () => b.IlmoittauduLounaalle(c.RavintolaNimi, 1))
    .Add("Arvioi lounasravintola", () => b.LisääArvio(c.RavintolaID, 1))
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

var subMenu = new ConsoleMenu(args, level: 1)
    .AddRange(a.SelaaRavintolatValikko(subMenu2))
   
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

var menu = new ConsoleMenu(args, level: 0)
  .Add("Näytä ravintolat", subMenu.Show)
  .Add("Sub", subMenu.Show)
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

menu.Show();



