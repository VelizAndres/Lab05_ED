using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab05_Veliz_ED1.Models;
using Lab05_Veliz_ED1.Herramientas.Almacen;

namespace Lab05_Veliz_ED1.Controllers
{
    public class TareasController : Controller
    {
        public ActionResult Perfil()
        {
            if (Caja_Perfil.Instance.Heap_Perfil.raiz != null)
            {
                string titulo = Caja_Perfil.Instance.Heap_Perfil.raiz.Valor;
                mTarea tarea_raiz = Caja_Maestra.Instance.Tabla_Perfil.Buscar(titulo, mTarea.Obt_Titulo, mTarea.Del_CodeHash);
                ViewBag.Cantidad_tareas = Caja_Perfil.Instance.Heap_Perfil.Cant_Nodos;
                ViewBag.Desarrollador = tarea_raiz.Nombre_dev;
               string Fecha_text = string.Format("{0: dd/MM/yyyy}", tarea_raiz.Fecha);
                ViewBag.Fecha = Fecha_text;
                ViewBag.Titulo = tarea_raiz.Titulo;
                ViewBag.Descripción = tarea_raiz.Descripcion;
                ViewBag.Proyecto = tarea_raiz.Proyeto;
                string text_prioridad = "";
                int prio = tarea_raiz.Prioridad;
                if (prio == 0)
                {
                    text_prioridad = "Alta";
                }
                else if (prio == 10)
                {
                    text_prioridad = " Media Alta";
                }
                else if (prio == 20)
                {
                    text_prioridad = "Media";
                }
                else if (prio == 30)
                {
                    text_prioridad = " Media Baja ";
                }
                else if (prio == 40)
                {
                    text_prioridad = "Baja";
                }
                ViewBag.Prioridad = text_prioridad;
            }
            return View();
        }

        public ActionResult CrearTarea()
        {
            return View();
        }

        public ActionResult CompletarTarea()
            {
            string llave = Caja_Perfil.Instance.Heap_Perfil.Eliminar(mTarea.ComparPrioridad, mTarea.EsPrioritario_Hash);
            Caja_Maestra.Instance.Tabla_Perfil.Eliminar(llave,mTarea.Obt_Titulo, mTarea.Del_CodeHash);
            foreach(mPerfil perfil in Caja_Maestra.Instance.Perfiles_data)
            {
                if(perfil.nombre.Equals(Caja_Perfil.Instance.nombre))
                {
                    perfil.cola_prio = Caja_Perfil.Instance.Heap_Perfil;
                }
            }
            return RedirectToAction("Perfil", "Tareas");
            }




        // POST: Tareas/Create
        [HttpPost]
        public ActionResult CrearTarea(FormCollection collection)
        {
            try
            {
                DateTime fecha_aux = Convert.ToDateTime(collection["Fecha"]);
                string prioridad = collection["Prioridad"];
                int num_prio = 0;
                if (prioridad.Equals("Alta"))
                {
                    num_prio = 0;
                }
                else if (prioridad.Equals("Media Alta"))
                {
                    num_prio = 10;
                }
                else if (prioridad.Equals("Media"))
                {
                    num_prio = 20;
                }
                else if (prioridad.Equals("Media Baja"))
                {
                    num_prio = 30;

                }
                else if (prioridad.Equals("Baja"))
                {
                    num_prio = 40;
                }
                int day = DateTime.Today.Day;
                int month = DateTime.Today.Month;
                int year = DateTime.Today.Year;
                int prio = 0;
                day = fecha_aux.Day - day;
                month = (fecha_aux.Month - month) * 30;
                year = (fecha_aux.Year - year) * 365;
                prio = day + month + year;

                mTarea Nueva_Tarea = new mTarea
                {
                    Titulo = collection["Titulo"],
                    Descripcion = collection["Descripcion"],
                    Proyeto = collection["Proyeto"],
                    Fecha = Convert.ToDateTime(collection["Fecha"]),
                    Prioridad = num_prio,
                    Prioridad_real = prio + num_prio,
                    Nombre_dev = Caja_Perfil.Instance.nombre,
                };
                if(fecha_aux<DateTime.Now)
                {
                    ViewBag.Error = "Fecha incorrecta";
                    return View("CrearTarea");
                }
                if(Nueva_Tarea.Titulo=="")
                {
                    ViewBag.Error = "Titulo vacio";
                    return View("CrearTarea");
                }
                if(!Caja_Maestra.Instance.Tabla_Perfil.Guardar(Nueva_Tarea.Titulo,Nueva_Tarea,mTarea.Obt_Titulo,mTarea.Del_CodeHash))
                {
                    Caja_Perfil.Instance.Heap_Perfil.Agregar(Nueva_Tarea.Titulo, mTarea.EsPrioritario_Hash, mTarea.ComparPrioridad);
                    foreach (mPerfil perfil in Caja_Maestra.Instance.Perfiles_data)
                    {
                        if (perfil.nombre.Equals(Caja_Perfil.Instance.nombre))
                        {
                            perfil.cola_prio = Caja_Perfil.Instance.Heap_Perfil;
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Ya existe una tarea con ese titulo";
                    return View("CrearTarea");
                }

                return RedirectToAction("Perfil");
            }
            catch
            {
                ViewBag.Error = "Datos incorrectos";
                return View("CrearTarea");
            }
        }

      

    }
}
