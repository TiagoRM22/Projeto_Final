using Projeto_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_Final.Controllers
{
    public class CategoriaController : Controller
    {
        // GET: Categoria



        [HttpGet]
        public ActionResult CriarCategoria()
        {
            try
            {
                Categoria novaCategoria = new Categoria();
                return View(novaCategoria);
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarProdutos", "Produtos", new { msg = erro.Message });
            }
        }


        [HttpPost]
        public ActionResult CriarCategoria(Categoria novaCategoria)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    bd.Categoria.Add(novaCategoria);
                    bd.SaveChanges();

                    return RedirectToAction("ListarProdutos", "Produtos", new { msg = "Categoria criada com sucesso" });
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarProdutos", "Produtos", new { msg = erro.Message });
            }
        }






        public ActionResult Index()
        {
            return View();
        }
    }
}