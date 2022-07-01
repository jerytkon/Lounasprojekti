// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LounasprojektiLib;
using ConsoleTools;

TiedostojenLukeminenSQL.LueKonsultit("..\\..\\..\\konsultit.txt");
TiedostojenLukeminenSQL.LueRavintolat("..\\..\\..\\ravintolat.txt");

var valikot = new Valikot();
var kirjautuminen = new Kirjautuminen();
kirjautuminen.Kirjaudu();

var kommenttiMenu = valikot.KommenttiMenu(args);
var kommentitMenu = valikot.KommentitMenu(args, kommenttiMenu);
var ravintolatSubMenu = valikot.RavintolaSubMenu(args, kirjautuminen);
var ravintolatMenu = valikot.RavintolatMenu(args, kirjautuminen, ravintolatSubMenu);
var top3Menu = valikot.Top3Menu(args, kirjautuminen, ravintolatSubMenu);
var käyttäjänPoistoMenu = valikot.KäyttäjänMuokkausMenu(args);
var listaaKäyttäjätMenu = valikot.ListaaKäyttäjätMenu(args, käyttäjänPoistoMenu);

if (kirjautuminen.OnAdmin == 0)
{
    var käyttäjäMenu = valikot.KäyttäjäMenu(args, ravintolatMenu, kommentitMenu, top3Menu);
    käyttäjäMenu.Show();
}

if (kirjautuminen.OnAdmin == 1)
{
    var adminMenu = valikot.AdminMenu(args, ravintolatMenu, kommentitMenu, listaaKäyttäjätMenu);
    adminMenu.Show();
}


