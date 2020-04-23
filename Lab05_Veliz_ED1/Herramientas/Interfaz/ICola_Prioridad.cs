using System;

namespace Lab05_Veliz_ED1.Herramientas.Interfaz
{
    interface ICola_Prioridad <T>
    {
        void Agregar(T valor, Delegate Det_Prioridad, Delegate Comparar);
        T Eliminar(Delegate Comparar, Delegate Det_prioridad);
        T Buscar(T valor, Delegate comparar);
    }
}
