using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab05_Veliz_ED1.Models;

namespace Lab05_Veliz_ED1.Herramientas.Estructuras
{
    public class TablaHash <T>
    {
        List<T>[] TPendientes_List = new List<T>[11];
        public bool Guardar(string llave, T tarea_nueva, Delegate Obt_Titulo, Delegate CodeHash)
        {
            int posicion = (int)CodeHash.DynamicInvoke(llave);
            if (TPendientes_List[posicion] == null)
            {
                TPendientes_List[posicion] = new List<T>();
                TPendientes_List[posicion].Add(tarea_nueva);
                return false;
            }
            else
            {
                bool existe_tit = false;
                string titulo = "";
                foreach (T tarea in TPendientes_List[posicion])
                {
                    titulo = (string)Obt_Titulo.DynamicInvoke(tarea);
                    if(titulo==llave)    { existe_tit = true;}
                }
                if (existe_tit)
                {
                    //"Ya existe la tarea "
                    return true;
                }
                else
                {
                    TPendientes_List[posicion].Add(tarea_nueva);
                    return false;
                }
            }
        }

        public T Buscar(string llave, Delegate Obt_Titulo, Delegate CodeHash)
        {
            int posicion = (int)CodeHash.DynamicInvoke(llave);
            T Elemento_Buscado = default(T);
            foreach (T Buscado in TPendientes_List[posicion])
            {
                string llave_buscado = (string)Obt_Titulo.DynamicInvoke(Buscado);
                if (llave.Equals(llave_buscado))
                {
                    Elemento_Buscado = Buscado;
                }
            }
            return Elemento_Buscado;
        }

        public List<T>[] Retorna_Tabla()
        {
            return TPendientes_List;
        }
        
        public void Eliminar(string llave, Delegate Obt_Titulo, Delegate CodeHash)
        {
            int posicion = (int)CodeHash.DynamicInvoke(llave);
            TPendientes_List[posicion].Remove(Buscar(llave, Obt_Titulo, CodeHash));
        }
    }
}