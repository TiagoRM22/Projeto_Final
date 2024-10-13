using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Projeto_Final.Models;

namespace Projeto_Final.Controllers
{
    public class ClientesController : Controller
    {

        [HttpPost]
        public ActionResult CriarCliente(Cliente novo, HttpPostedFileBase fich)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    bd.Cliente.Add(novo);
                    if (fich != null && fich.ContentLength > 0 && fich.ContentType.Contains("image"))
                    {
                        string caminho = novo.NUM_CC.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        novo.Foto = caminho;
                        caminho = Server.MapPath("~/fotos/Clientes/" + caminho);
                        fich.SaveAs(caminho);
                    }
                    bd.SaveChanges();

                    return RedirectToAction("ListarClientes", new { msg = "Criado com sucesso" });
                }
            }
            catch (Exception erro)
            {

                return RedirectToAction("ListarClientes", new { msg = erro.Message });
            }
        }

        [HttpGet]
        public ActionResult CriarCliente()
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    Cliente novo = new Cliente();
                    return View(novo);
                }
            }
            catch (Exception erro)
            {

                return RedirectToAction("ListarClientes", new { msg = erro.Message });
            }
        }



        [HttpPost, ActionName("ApagarCliente")]
        public ActionResult DeleteCliente(int? id, int? page)
        {
            using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
            {
                Cliente apagado = bd.Cliente.Find(id ?? -1);
                if (apagado != null)
                {
                    bd.Cliente.Remove(apagado);
                    bd.SaveChanges();
                    return RedirectToAction("ListarClientes", new { msg = "Cliente eliminado com sucesso", page = page ?? 1 });
                }
                else
                {
                    return RedirectToAction("ListarClientes", new { msg = "Cliente não encontrado", page = page ?? 1 });
                }
            }
        }

        [HttpGet]
        public ActionResult ApagarCliente(int? id, int? page)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    Cliente apagado = bd.Cliente.Find(id ?? -1);
                    if (apagado != null)
                    {
                        ViewBag.pagina = page ?? 1;
                        return View(apagado);
                    }
                    else
                    {
                        return RedirectToAction("ListarClientes", new { msg = "Cliente não encontrado", page = page ?? 1 });
                    }
                }

            }
            catch (Exception erro)
            {

                return RedirectToAction("ListarClientes", new { msg = erro.Message });
            }
        }

        [HttpPost]
        public ActionResult EliminaCliente(int? id)
        {
            using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
            {
                Cliente cliente = bd.Cliente.Find(id);
                if (cliente != null)
                {
                    bd.Cliente.Remove(cliente);
                    bd.SaveChanges();
                    return Json(new { msg = "Apagado" });
                }
                else
                {
                    return Json(new { msg = "Cliente não Existe" });
                }
            }
        }



        [HttpPost]
        public ActionResult EditarCliente(Cliente cliente, HttpPostedFileBase fich)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    Cliente clienteExistente = bd.Cliente.Find(cliente.NUM_CC);
                    if (clienteExistente != null)
                    {
                        clienteExistente.NomeProprio = cliente.NomeProprio;
                        clienteExistente.NomeApelido = cliente.NomeApelido;
                        clienteExistente.Email = cliente.Email;
                        clienteExistente.Contacto = cliente.Contacto;
                        clienteExistente.DataNascimento = cliente.DataNascimento;
                        clienteExistente.Morada = cliente.Morada;

                        if (fich != null && fich.ContentLength > 0 && fich.ContentType.Contains("image"))
                        {
                            string caminho = clienteExistente.NUM_CC.ToString() + System.IO.Path.GetExtension(fich.FileName);
                            clienteExistente.Foto = caminho;
                            caminho = Server.MapPath("~/fotos/Clientes/" + caminho);
                            fich.SaveAs(caminho);
                        }

                        bd.SaveChanges();

                        return RedirectToAction("ListarClientes", new { msg = "Cliente atualizado com sucesso" });
                    }
                    else
                    {
                        return RedirectToAction("ListarClientes", new { msg = "Cliente não encontrado" });
                    }
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarClientes", new { msg = erro.Message });
            }
        }
        public ActionResult EditarCliente(int? id)
        {
            using (BD_ProjetoFinalEntities db = new BD_ProjetoFinalEntities())
            {
                Cliente este = db.Cliente.Find(id ?? -1);
                if (este != null)
                {
                    return View(este);
                }
                else
                {
                    return View("Error");
                }
            }
        }



        public ActionResult DetalheCliente(int? id)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    Cliente este = bd.Cliente.Find(id ?? -1);
                    if (este != null)
                    {
                        return View(este);
                    }
                    else
                    {
                        return RedirectToAction("ListarClientes", new { msg = "Cliente não existe" });
                    }
                }
            }
            catch (Exception erro)
            {

                return RedirectToAction("ListarClientes", new { msg = erro.Message });
            }
        }


        public ActionResult ListarClientes()
        {
            return View();
        }

        public ActionResult GetClientes()
        {
            using (BD_ProjetoFinalEntities db = new BD_ProjetoFinalEntities())
            {
                var qry = db.Cliente;
                List<Cliente> clientes = new List<Cliente>();
                foreach (var c in qry)
                {
                    clientes.Add(new Cliente
                    {
                        NUM_CC = c.NUM_CC,
                        NomeProprio = c.NomeProprio,
                        NomeApelido = c.NomeApelido,
                        Email = c.Email,
                        Contacto = c.Contacto,
                        DataNascimento = c.DataNascimento,
                        Morada = c.Morada,
                        Foto = c.Foto
                    });
                }

                return Json(new { data = clientes }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}