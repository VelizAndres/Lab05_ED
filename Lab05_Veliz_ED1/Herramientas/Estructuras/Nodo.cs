using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab05_Veliz_ED1.Herramientas.Estructuras
{
    public class Nodo <T>
    {
        private Nodo<T> padre;
        private Nodo<T> hijoizq;
        private Nodo<T> hijoder;
        private T valor;

        public Nodo<T> Hijoizq { get => hijoizq; set => hijoizq = value; }
        public Nodo<T> Hijoder { get => hijoder; set => hijoder = value; }
        public T Valor { get => valor; set => valor = value; }
        public Nodo<T> Padre { get => padre; set => padre = value; }
    }
}