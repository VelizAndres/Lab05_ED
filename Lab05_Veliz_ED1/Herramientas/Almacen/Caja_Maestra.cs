using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab05_Veliz_ED1.Herramientas.Estructuras;
using Lab05_Veliz_ED1.Models;


namespace Lab05_Veliz_ED1.Herramientas.Almacen
{
    public class Caja_Maestra
    {
        private static Caja_Maestra _instance = null;

        public static Caja_Maestra Instance
        {
            get
            {
                if (_instance == null) _instance = new Caja_Maestra();
                return _instance;
            }
        }
        public TablaHash<mTarea> Tabla_Perfil = new TablaHash<mTarea>();
        public List<mPerfil> Perfiles_data = new List<mPerfil>();
        public bool DatosGuardado;
    }
}