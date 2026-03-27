using ClubDeportivo.Data.Contratos;
using ClubDeportivo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClubDeportivo.Controllers
{
    public class MiembroController : Controller
    {
        private readonly IMiembro miembroDB;

        public MiembroController(IMiembro miembroData)
        {
            miembroDB = miembroData;
        }

        public ActionResult Index()
        {
            List<Miembro> lista = miembroDB.listar();
            return View(lista);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            Miembro miembro = miembroDB.ObtenerPorId(id);
            if (miembro == null)
                return NotFound();
            return View(miembro);
        }

        public ActionResult Edit(int id) 
        {
            var miembro = miembroDB.ObtenerPorId(id);
            if (miembro == null)
                return NotFound();
            return View(miembro);
        }

        public ActionResult Delete(int id) 
        {
            var miembro = miembroDB.ObtenerPorId(id);
            return View(miembro);
        }

        /* Post */
        [HttpPost]
        public IActionResult Create(Miembro miembro) 
        {
            if (!ModelState.IsValid)
                return View(miembro);

            miembroDB.Registrar(miembro);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Miembro miembro) 
        {
            if (!ModelState.IsValid)
                return View(miembro);
            miembroDB.Actualizar(miembro);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int idMiembro) 
        {
            miembroDB.Eliminar(idMiembro);
            return RedirectToAction("Index");
        }


    }
}
