// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lounasprojekti;
using ConsoleTools;

var valikot = new Valikot();
var kirjautuminen = new Kirjautuminen();
var ravintolatSubMenu = valikot.ravintolaSubMenu(args, kirjautuminen);
var ravintolatMenu = valikot.ravintolatMenu(args, kirjautuminen, ravintolatSubMenu);

kirjautuminen.Kirjaudu();

if (kirjautuminen.OnAdmin == 0)
{
    var käyttäjäMenu = valikot.käyttäjäMenu(args, ravintolatMenu);
    käyttäjäMenu.Show();
}

if (kirjautuminen.OnAdmin == 1)
{
    var adminMenu = valikot.adminMenu(args, ravintolatMenu);
    adminMenu.Show();
}


