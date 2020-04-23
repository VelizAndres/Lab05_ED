using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab05_Veliz_ED1.Herramientas.Almacen;
using Lab05_Veliz_ED1.Herramientas.Estructuras;
using Lab05_Veliz_ED1.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab05_Veliz_ED1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!Caja_Maestra.Instance.DatosGuardado)
            {
                string text = Server.MapPath("~/Info/Tareas.csv");
                using (var archivo = new FileStream(text, FileMode.Open))
                {
                    using (var archivolec = new StreamReader(archivo))
                    {

                        string texto = archivolec.ReadLine();
                        while (texto != null)
                        {
                            int day = DateTime.Today.Day;
                            int month = DateTime.Today.Month;
                            int year = DateTime.Today.Year;
                            int prio = 0;
                            string descri = "";
                            Regex regx = new Regex("," + "(?=(?:[^\']*\'[^\']*\')*(?![^\']*\'))");
                            string[] cajatext = regx.Split(texto);
                            for (int i = 1; i < (cajatext[1].Length - 2); i++)
                            {
                                descri += cajatext[1][i];
                            }
                            DateTime Fecha_obt = Convert.ToDateTime(cajatext[4]);
                            day = Fecha_obt.Day - day;
                            month = (Fecha_obt.Month - month) * 30;
                            year = (Fecha_obt.Year - year) * 365;
                            prio = day + month + year;
                            mTarea nueva_tarea = new mTarea
                            {
                                Titulo = cajatext[0],
                                Descripcion = descri,
                                Proyeto = cajatext[2],
                                Prioridad = int.Parse(cajatext[3]),
                                Prioridad_real = prio + int.Parse(cajatext[3]),
                                Fecha = Fecha_obt,
                                Nombre_dev = cajatext[5]
                            };
                            mPerfil nuevoperfil = new mPerfil { nombre = nueva_tarea.Nombre_dev, cola_prio = new Heap<string>() };

                            Caja_Maestra.Instance.Tabla_Perfil.Guardar(nueva_tarea.Titulo, nueva_tarea, mTarea.Obt_Titulo, mTarea.Del_CodeHash);

                            if (Caja_Maestra.Instance.Perfiles_data != null)
                            {
                                bool guardado = false;
                                foreach (mPerfil perfil in Caja_Maestra.Instance.Perfiles_data)
                                {
                                    if (perfil.nombre == nuevoperfil.nombre)
                                    {
                                        perfil.cola_prio.Agregar(nueva_tarea.Titulo, mTarea.EsPrioritario_Hash, mTarea.ComparPrioridad);
                                        guardado = true;
                                    }
                                }
                                if (!guardado)
                                {
                                    nuevoperfil.cola_prio.Agregar(nueva_tarea.Titulo, mTarea.EsPrioritario_Hash, mTarea.ComparPrioridad);
                                    Caja_Maestra.Instance.Perfiles_data.Add(nuevoperfil);
                                }
                            }
                            texto = archivolec.ReadLine();
                        }
                    }
                }
                Caja_Maestra.Instance.DatosGuardado = true;
            }
            return View();
        }

        public ActionResult Manager()
        {
            List<mTarea> Total_Tareas = new List<mTarea>();
            foreach(var listado in Caja_Maestra.Instance.Tabla_Perfil.Retorna_Tabla())
            {
                if (listado != null)
                {
                    foreach (mTarea tarea in listado)
                    {
                        Total_Tareas.Add(tarea);
                    }
                }
            }
            Total_Tareas.Sort(mTarea.Comparar_Prio);
            return View(Total_Tareas);
        }

        public ActionResult User_Dev()
        {
            return View(Caja_Maestra.Instance.Perfiles_data);
        }

        public ActionResult Nuevo_Us()
        {
            return View();
        }

        public ActionResult CreateUser(string Texto)
        {
            mPerfil nuevoperfil = new mPerfil { nombre = Texto, cola_prio = new Heap<string>() };
                foreach (mPerfil perfil in Caja_Maestra.Instance.Perfiles_data)
                {
                    if (perfil.nombre == nuevoperfil.nombre)
                    {
                        ViewBag.Error = "Ya existe el usuario";
                        return View();
                    }
                }
               Caja_Maestra.Instance.Perfiles_data.Add(nuevoperfil);
              return RedirectToAction("User_Dev");
        }

        public ActionResult Guarda_Datos()
        {
            return View();
        }

        public ActionResult Cerrar_Secion()
        {
             List<mTarea> Total_Tareas = new List<mTarea>();
            foreach (var listado in Caja_Maestra.Instance.Tabla_Perfil.Retorna_Tabla())
            {
                if (listado != null)
                {
                    foreach (mTarea tarea in listado)
                    {
                        Total_Tareas.Add(tarea);
                    }
                }
            }

            string info = "";
            foreach(mTarea tarea in Total_Tareas)
            {
                info += tarea.Titulo + ",'" + tarea.Descripcion + "'," + tarea.Proyeto +"," + tarea.Prioridad + "," + string.Format("{0:dd/MM/yyyy}", tarea.Fecha) + "," + tarea.Nombre_dev;
                info += Environment.NewLine;
            }
            string text = Server.MapPath("~/Info/Tareas.csv");
            using (var archivo = new FileStream(text, FileMode.Open))
            {
                using (var escritor = new StreamWriter(archivo))
                {
                    escritor.Write(info);
                }
            }
            return View("Index");
        }




        [HttpPost]
        public ActionResult User_Dev(FormCollection collection)
        {
            string Nombre = collection["Usuario"];
            foreach(mPerfil perfil in Caja_Maestra.Instance.Perfiles_data)
            {
                if(Nombre.Equals(perfil.nombre))
                {
                    Caja_Perfil.Instance.nombre = Nombre;
                    Caja_Perfil.Instance.Heap_Perfil = perfil.cola_prio;
                }
            }
            return RedirectToAction("Perfil","Tareas");
        }
     

    }
}