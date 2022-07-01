// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lounasprojekti;
using ConsoleTools;

var valikot = new Valikot();
var kirjautuminen = new Kirjautuminen();
kirjautuminen.Kirjaudu();

var kommenttiMenu = valikot.kommenttiMenu(args);
var kommentitMenu = valikot.kommentitMenu(args, kommenttiMenu);
var ravintolatSubMenu = valikot.ravintolaSubMenu(args, kirjautuminen);
var ravintolatMenu = valikot.ravintolatMenu(args, kirjautuminen, ravintolatSubMenu);
var top3Menu = valikot.top3Menu(args, kirjautuminen, ravintolatSubMenu);
var käyttäjänPoistoMenu = valikot.KäyttäjänMuokkausMenu(args);
var listaaKäyttäjätMenu = valikot.ListaaKäyttäjätMenu(args, käyttäjänPoistoMenu);

if (kirjautuminen.OnAdmin == 0)
{
    var käyttäjäMenu = valikot.käyttäjäMenu(args, ravintolatMenu, kommentitMenu, top3Menu);
    käyttäjäMenu.Show();
}

if (kirjautuminen.OnAdmin == 1)
{
    var adminMenu = valikot.adminMenu(args, ravintolatMenu, kommentitMenu, listaaKäyttäjätMenu);
    adminMenu.Show();
}


