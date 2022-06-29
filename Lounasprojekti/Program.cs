// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lounasprojekti;
using ConsoleTools;

var a = DateTime.Today.Date.ToString();
var b = DateTime.Today.Date.ToString("yyyy-MM-dd");
var valikot = new Valikot();
var kirjautuminen = new Kirjautuminen();
var kommenttiMenu = valikot.kommenttiMenu(args);
var kommentitMenu = valikot.kommentitMenu(args, kommenttiMenu);
var ravintolatSubMenu = valikot.ravintolaSubMenu(args, kirjautuminen);
var ravintolatMenu = valikot.ravintolatMenu(args, kirjautuminen, ravintolatSubMenu);



kirjautuminen.Kirjaudu();

if (kirjautuminen.OnAdmin == 0)
{
    var käyttäjäMenu = valikot.käyttäjäMenu(args, ravintolatMenu, kommentitMenu);
    käyttäjäMenu.Show();
}

if (kirjautuminen.OnAdmin == 1)
{
    var adminMenu = valikot.adminMenu(args, ravintolatMenu, kommentitMenu);
    adminMenu.Show();
}


