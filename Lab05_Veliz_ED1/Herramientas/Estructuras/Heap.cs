using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab05_Veliz_ED1.Herramientas.Interfaz;
using Lab05_Veliz_ED1.Herramientas.Estructuras;
using Lab05_Veliz_ED1.Herramientas.Almacen;


namespace Lab05_Veliz_ED1.Herramientas.Estructuras
{
    public class Heap<T> : ICola_Prioridad<T>
    {
        public Nodo<T> raiz;
        public int Cant_Nodos;

/*-------------------------------------------------------------------------*/
        public void Agregar(T valor, Delegate Det_Prioridad, Delegate Comparar)
        {
            Nodo<T> nod_nuevo = new Nodo<T>();
            nod_nuevo.Valor = valor;
            Cant_Nodos++;
            if (raiz == null)
            {
                raiz = nod_nuevo;
                raiz.Padre = null;
            }
            else
            {
                Realizar_Recorrido(nod_nuevo);
                intercambiar_valor_padre(nod_nuevo, Det_Prioridad);
            }
        }

        private void Realizar_Recorrido(Nodo<T> Nuevo)
        {
            Nodo<T> Guia = raiz;
            string bin= Binario_Conver(Cant_Nodos);
            for(int i=1; i<bin.Length;i++)
            {
                if(Convert.ToString(bin[i]).Equals("0"))
                {
                    if(Guia.Hijoizq==null)
                    {
                        Guia.Hijoizq = Nuevo;
                        Nuevo.Padre = Guia;
                    }
                    else
                    {
                        Guia = Guia.Hijoizq;
                    }
                }
                else
                {
                    if(Guia.Hijoder==null)
                    {
                        Guia.Hijoder = Nuevo;
                        Nuevo.Padre = Guia;
                    }
                    else
                    {
                        Guia = Guia.Hijoder;
                    }
                }
            }
        }

        private string Binario_Conver(int total_nodo)
        {
            string cadena = "";
            if (total_nodo > 0)
            {
                while (total_nodo > 0)
                {
                    if (total_nodo % 2 == 0)
                    {
                        cadena = "0" + cadena;
                    }
                    else
                    {
                        cadena = "1" + cadena;
                    }
                    total_nodo = (total_nodo / 2);
                }
            }
            return cadena;
        }

        private void intercambiar_valor_padre(Nodo<T> agregado, Delegate Deter_Prioridad)
        {
            if (agregado.Padre != null)
            {
                T aux_val = agregado.Padre.Valor;
                if ((bool)Deter_Prioridad.DynamicInvoke(agregado.Padre.Valor, agregado.Valor, Caja_Maestra.Instance.Tabla_Perfil))
                {
                    agregado.Padre.Valor = agregado.Valor;
                    agregado.Valor = aux_val;
                    intercambiar_valor_padre(agregado.Padre, Deter_Prioridad);
                }
            }
        }

        /*-------------------------------------------------------------------------*/

        public T Buscar(T valor, Delegate Det_Prioridad)
        {
            throw new NotImplementedException();
        }


        public T Eliminar(Delegate Comparar, Delegate Det_prioridad)
        {
            T aux = raiz.Valor;
            Nodo<T> cola = Encontrar_ultimo_nodo();
            raiz.Valor = cola.Valor;
            borrar_ult_nodo(cola, Comparar);
            if (raiz != null) { intercambiar_valor_hijo(raiz, Comparar, Det_prioridad); }
            Cant_Nodos--;
            return aux;
        }

        private void borrar_ult_nodo(Nodo<T> ultimo,Delegate Comparador)
        {
            Nodo<T> aux_last = ultimo;
            if(ultimo.Padre!=null)
            {
                if (ultimo.Padre.Hijoder != null && ultimo.Padre.Hijoizq != null)
                {
                    if ((int)Comparador.DynamicInvoke(ultimo.Valor, ultimo.Padre.Hijoder.Valor) == 0)
                    {
                        ultimo.Padre.Hijoder = null;
                    }
                    else
                    {
                        ultimo.Padre.Hijoizq = null;
                    }
                }
                else if (ultimo.Padre.Hijoizq != null)
                {
                    ultimo.Padre.Hijoizq = null;
                }
                else if (ultimo.Padre.Hijoder != null)
                {
                    ultimo.Padre.Hijoder = null;
                }
            }
            else
            {
                raiz = null;
            }

        }

        private Nodo<T> Encontrar_ultimo_nodo()
        {
            Nodo<T> Guia = raiz;
            string bin = Binario_Conver(Cant_Nodos);
            for (int i = 1; i < bin.Length; i++)
            {
                if (Convert.ToString(bin[i]).Equals("0"))
                {
                    Guia = Guia.Hijoizq;
                }
                else
                {
                    Guia = Guia.Hijoder;
                }
            }
            return Guia;
        }

        private void intercambiar_valor_hijo(Nodo<T> movido, Delegate Comparador, Delegate Deter_Prior)
        {
            T aux_val = movido.Valor;
            if(movido.Hijoder!=null && movido.Hijoizq!=null)
            {
                if((int)Comparador.DynamicInvoke(movido.Hijoizq.Valor, movido.Hijoder.Valor) >0)
                {
                    if ((bool)Deter_Prior.DynamicInvoke(movido.Valor, movido.Hijoder.Valor, Caja_Maestra.Instance.Tabla_Perfil))
                    {
                        movido.Valor = movido.Hijoder.Valor;
                        movido.Hijoder.Valor = aux_val;
                        intercambiar_valor_hijo(movido.Hijoder, Comparador, Deter_Prior);
                    }
                }
                else
                {
                    if ((bool)Deter_Prior.DynamicInvoke(movido.Valor, movido.Hijoizq.Valor, Caja_Maestra.Instance.Tabla_Perfil))
                    {
                        movido.Valor = movido.Hijoizq.Valor;
                        movido.Hijoizq.Valor = aux_val;
                        intercambiar_valor_hijo(movido.Hijoizq, Comparador, Deter_Prior);
                    }
                }
            }
            else if(movido.Hijoder!=null)
            {
                if((bool)Deter_Prior.DynamicInvoke(movido.Valor,movido.Hijoder.Valor, Caja_Maestra.Instance.Tabla_Perfil))
                {
                    movido.Valor = movido.Hijoder.Valor;
                    movido.Hijoder.Valor = aux_val;
                    intercambiar_valor_hijo(movido.Hijoder, Comparador, Deter_Prior);
                }
            }
            else if(movido.Hijoizq!=null)
            {
                if ((bool)Deter_Prior.DynamicInvoke(movido.Valor, movido.Hijoizq.Valor, Caja_Maestra.Instance.Tabla_Perfil))
                {
                    movido.Valor = movido.Hijoizq.Valor;
                    movido.Hijoizq.Valor = aux_val;
                    intercambiar_valor_hijo(movido.Hijoizq, Comparador, Deter_Prior);
                }
            }
        }



    }
}