using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Lab05_Veliz_ED1.Herramientas.Estructuras;
using Lab05_Veliz_ED1.Herramientas.Almacen;

namespace Lab05_Veliz_ED1.Models
{
    public class mTarea
    {
        string titulo;
        string descripcion;
        string proyeto;
        int prioridad;
        int prioridad_real;
        string nombre_dev;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-mm-yy}",ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        
        public DateTime Fecha { get; set; }

        public string Titulo { get => titulo; set => titulo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Proyeto { get => proyeto; set => proyeto = value; }
        public int Prioridad { get => prioridad; set => prioridad = value; }
        public int Prioridad_real { get => prioridad_real; set => prioridad_real = value; }
        public string Nombre_dev { get => nombre_dev; set => nombre_dev = value; }


        // Delegados
        public static Func<mTarea, string> Obt_Titulo = delegate ( mTarea Tarea)
        {
            return Tarea.titulo;
        };

        public static Func<string, string, TablaHash<mTarea>, bool> EsPrioritario_Hash = delegate (string tarea_padre, string tarea_hijo, TablaHash<mTarea> Tareas_Hash)
        {
            mTarea tarea1 = Tareas_Hash.Buscar(tarea_padre, mTarea.Obt_Titulo, mTarea.Del_CodeHash);
            mTarea tarea2 = Tareas_Hash.Buscar(tarea_hijo, mTarea.Obt_Titulo, mTarea.Del_CodeHash);

            return (tarea1.Prioridad_real > tarea2.Prioridad_real);
        };

        public static Func<string, string, int> ComparPrioridad = delegate (string tarea1, string tarea2)
        {
            mTarea tarea_1 = Caja_Maestra.Instance.Tabla_Perfil.Buscar(tarea1, mTarea.Obt_Titulo, mTarea.Del_CodeHash);
            mTarea tarea_2 = Caja_Maestra.Instance.Tabla_Perfil.Buscar(tarea2, mTarea.Obt_Titulo, mTarea.Del_CodeHash);
            int tarea1_prio = tarea_1.Prioridad_real;
            int tarea2_prio = tarea_2.Prioridad_real;
            return tarea1_prio > tarea2_prio ? 1 : tarea1_prio < tarea2_prio ? -1 : 0;
        };

        public static Func<string, int> Del_CodeHash = delegate (string llave)
         {
             return Obt_CodeHash(llave);
         };
        
        public static Comparison<mTarea> Comparar_Prio = delegate (mTarea tarea1, mTarea tarea2)
        {
            int tarea1_prio = tarea1.Prioridad_real;
            int tarea2_prio = tarea2.Prioridad_real;
            return tarea1_prio > tarea2_prio ? 1 : tarea1_prio < tarea2_prio ? -1 : 0;
        };
        //Fin delegados


        public static int Obt_CodeHash(string llave)
        {
            int code = 0;
            int code_2 = 0;
            for (int i = 0; i < llave.Length; i++)
            {
                code += Convert.ToInt16(llave[i]);
            }

            foreach (char caract in llave)
            {
                code_2 += Convert.ToInt16(caract);
            }
            code_2 = code_2 % 11;
            code = code % 11;
            return code;
        }



    }
}