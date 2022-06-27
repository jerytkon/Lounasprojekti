// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Lounasprojekti.Models;
using Lounasprojekti;

Kyselyt a = new Kyselyt();

var demo = a.SelaaRavintolat();
foreach (var item in demo)
    Console.WriteLine(item.Key + " " + item.Value);
Console.WriteLine(demo[]);

class Kyselyt
{
    LounasDBContext db = new LounasDBContext();


    //Selaa ravintoloita eri parametreilla
    public Dictionary<string, int> SelaaRavintolat()
    {
        //Selaa ravintolat ja näytä ilmoittautuneet syömään
        // lisätään where ehto näyttämään vain tälle päivälle
        Dictionary<string, int> map = new Dictionary<string, int>();
        var kysely = (from i in db.Ravintolas
                     join i2 in db.Lounastapahtumas on i.RavintolaId equals i2.RavintolaId
                     join i3 in db.Lounasseuras on i2.LounastapahtumaId equals i3.LounastapahtumaId
                     group i by i.RavintolanNimi into g
                     select new { Ravintolanimi = g.Key, Count = g.Count() }).ToList();
        foreach (var item in kysely)
        {
            map.Add(item.Ravintolanimi, item.Count);
        }
        return map;
    }


}