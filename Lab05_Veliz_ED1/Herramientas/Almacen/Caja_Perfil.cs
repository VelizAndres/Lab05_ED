using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab05_Veliz_ED1.Herramientas.Estructuras;
using Lab05_Veliz_ED1.Models;

namespace Lab05_Veliz_ED1.Herramientas.Almacen
{
    public class Caja_Perfil
    {

        private static Caja_Perfil _instance = null;

        public static Caja_Perfil Instance
        {
            get
            {
                if (_instance == null) _instance = new Caja_Perfil();
                return _instance;
            }
        }

        public Heap<string> Heap_Perfil = new Heap<string>();
        public string nombre;
    }
}