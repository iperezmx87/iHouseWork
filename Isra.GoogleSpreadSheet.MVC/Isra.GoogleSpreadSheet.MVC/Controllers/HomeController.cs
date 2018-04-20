using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Isra.GoogleSpreadSheet.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // debes agregar tu id de cliente de google
            // lo configuras en console.developer.google.com
            // lo debes configurar para localhost y para tu servidor productivo
            // en este caso se toma el token del archivo web.config ;)
            // este token se ocupa en el helper 
            ViewBag.GoogleToken = ConfigurationManager.AppSettings["GoogleToken"].ToString();
            return View();
        }

        // generamos un metodo sencillo que develva unas filas de ejemplo para demostrar como
        // se debe devolver la informacion hacia el cliente de google 
        [HttpPost]
        public string GenerarInformacionGoogle()
        {
            try
            {
                // asi debes poner la informacion

                // encabezados de columnas opcionales
                // cargando datos en la peticion
                List<IList<object>> lstValues = new List<IList<object>>();

                // encabezado de columna, pueden ser opcionales bebe :)
                lstValues.Add(new List<object>()
                    {
                        "Columna 1", "Columna 2", "Columna 3", "Columna 4"
                    });

                // datos obligatorios
                // normalmente los tomas de un servicio o de algun rest
                // esto normalmente se hace en un for, o en un query de linq
                // depediendo del numero de filas que se tengan
                // tu decides :)
                // debes hacer una instruccion de estas por cada fila que ingreses 
                // y debe coincidir con el numero de columnas
                lstValues.Add(new List<object>()
                {
                    "A","B","C","D"
                });

                lstValues.Add(new List<object>()
                {
                    "1","2","3","4"
                });

                // debes devolver el objeto serializado, para eso
                // usas Newtonsoft :)
                return JsonConvert.SerializeObject(lstValues);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}