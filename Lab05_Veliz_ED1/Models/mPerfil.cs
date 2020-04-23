using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab05_Veliz_ED1.Herramientas.Estructuras;


namespace Lab05_Veliz_ED1.Models
{
    public class mPerfil
    {
          public string nombre;
          public Heap<string> cola_prio { get; set; }
    }
}