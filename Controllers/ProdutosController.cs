using Projeto_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Projeto_Final.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly WooCommerceApiClient _wooCommerceApiClient;

        public ProdutosController()
        {
            _wooCommerceApiClient = new WooCommerceApiClient("http://projeto-final.local/", "ck_20005a62b8f9faf2fd8930c170aacc3b762d8c32", "cs_b1c21a56b3ba31db62b0648a858479bbdc4b03fe");
        }

        // GET: Produtos

        [HttpPost]
        public async Task<ActionResult> CriarProduto(Produto novo, HttpPostedFileBase fich)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    bd.Produto.Add(novo);
                    bd.SaveChanges();

                    if (fich != null && fich.ContentLength > 0 && fich.ContentType.Contains("image"))
                    {
                        string caminho = novo.ID_PRODUTO.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        novo.Foto = caminho;
                        caminho = Server.MapPath("~/fotos/Produtos/" + caminho);
                        fich.SaveAs(caminho);
                    }

                    bd.SaveChanges();

                    // Cria produto no WooCommerce
                    var precoNaoNulo = novo.Preço ?? 0m; // Define um valor padrão se Preço for nulo
                    var response = await _wooCommerceApiClient.CreateProduct(novo.Modelo, novo.Caracteristicas, precoNaoNulo);

                    if (!response.IsSuccessful)
                    {
                        throw new Exception("Erro ao criar produto no WooCommerce: " + response.ErrorMessage);
                    }

                    return RedirectToAction("ListarProdutos", new { msg = "Criado com sucesso" });
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarProdutos", new { msg = erro.Message });
            }
        }

        [HttpPost, ActionName("ApagarProduto")]
        public async Task<ActionResult> DeleteProduto(int? id, int? page)
        {
            using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
            {
                Produto apagado = bd.Produto.Find(id ?? -1);
                if (apagado != null)
                {
                    bd.Produto.Remove(apagado);
                    bd.SaveChanges();

                    // Apaga produto no WooCommerce
                    var response = await _wooCommerceApiClient.DeleteProduct(apagado.ID_PRODUTO);

                    if (!response.IsSuccessful)
                    {
                        throw new Exception("Erro ao apagar produto no WooCommerce: " + response.ErrorMessage);
                    }

                    return RedirectToAction("ListarProdutos", "Produtos", new { msg = "Produto eliminado com sucesso", page = page ?? 1 });
                }
                else
                {
                    return RedirectToAction("ListarProdutos", "Produtos", new { msg = "Produto não encontrado", page = page ?? 1 });
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditarProduto(Produto produto, HttpPostedFileBase fich)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    Produto produtoExistente = bd.Produto.Find(produto.ID_PRODUTO);
                    if (produtoExistente != null)
                    {
                        produtoExistente.SKU = produto.SKU;
                        produtoExistente.Marca = produto.Marca;
                        produtoExistente.Modelo = produto.Modelo;
                        produtoExistente.Categoria = produto.Categoria;
                        produtoExistente.Quantidade = produto.Quantidade;
                        produtoExistente.Caracteristicas = produto.Caracteristicas;
                        produtoExistente.Preço = produto.Preço;

                        if (fich != null && fich.ContentLength > 0 && fich.ContentType.Contains("image"))
                        {
                            if (!string.IsNullOrEmpty(produtoExistente.Foto))
                            {
                                string oldImagePath = Server.MapPath("~/fotos/Produtos/" + produtoExistente.Foto);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }

                            string caminho = produtoExistente.ID_PRODUTO.ToString() + System.IO.Path.GetExtension(fich.FileName);
                            produtoExistente.Foto = caminho;
                            caminho = Server.MapPath("~/fotos/Produtos/" + caminho);
                            fich.SaveAs(caminho);
                        }

                        bd.SaveChanges();

                        // Atualiza produto no WooCommerce
                        var precoNaoNulo = produto.Preço ?? 0m; // Define um valor padrão se Preço for nulo
                        var response = await _wooCommerceApiClient.UpdateProduct(produto.ID_PRODUTO, produto.Modelo, produto.Caracteristicas, precoNaoNulo);

                        if (!response.IsSuccessful)
                        {
                            throw new Exception("Erro ao atualizar produto no WooCommerce: " + response.ErrorMessage);
                        }

                        return RedirectToAction("ListarProdutos", new { msg = "Produto atualizado com sucesso" });
                    }
                    else
                    {
                        return RedirectToAction("ListarProdutos", new { msg = "Produto não encontrado" });
                    }
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarProdutos", new { msg = erro.Message });
            }
        }

        [HttpGet]
        public ActionResult CriarProduto()
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    List<Categoria> cats = bd.Categoria.ToList();
                    SelectList lista = new SelectList(cats, "ID_Categoria", "Categoria1");
                    ViewBag.lista = lista;
                    Produto novo = new Produto();
                    return View(novo);
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarProdutos", new { msg = erro.Message });
            }
        }

        [HttpGet]
        public ActionResult ApagarProduto(int? id, int? page)
        {
            try
            {
                using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
                {
                    Produto apagado = bd.Produto.Find(id ?? -1);
                    if (apagado != null)
                    {
                        ViewBag.pagina = page ?? 1;
                        return View(apagado);
                    }
                    else
                    {
                        return RedirectToAction("ListarProdutos", new { msg = "Produto não encontrado", page = page ?? 1 });
                    }
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListarProdutos", new { msg = erro.Message });
            }
        }

        [HttpPost]
        public ActionResult EliminaProduto(int? id)
        {
            using (BD_ProjetoFinalEntities bd = new BD_ProjetoFinalEntities())
            {
                Produto Produto = bd.Produto.Find(id);
                if (Produto != null)
                {
                    bd.Produto.Remove(Produto);
                    bd.SaveChanges();
                    return Json(new { msg = "Apagado" });
                }
                else
                {
                    return Json(new { msg = "Produto não Existe" });
                }
            }
        }

        [HttpGet]
        public ActionResult EditarProduto(int? id)
        {
            using (BD_ProjetoFinalEntities db = new BD_ProjetoFinalEntities())
            {
                Produto este = db.Produto.Find(id ?? -1);
                if (este != null)
                {
                    List<Categoria> cats = db.Categoria.ToList();
                    SelectList lista = new SelectList(cats, "ID_Categoria", "Categoria1");
                    ViewBag.lista = lista;
                    ViewBag.Preço = este.Preço;
                    return View(este);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult DetalheProduto(int? id)
        {
            using (BD_ProjetoFinalEntities db = new BD_ProjetoFinalEntities())
            {
                Produto este = db.Produto.Find(id ?? -1);
                if (este != null)
                {
                    ViewBag.Preço = este.Preço;
                    return View(este);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult ListarProdutos()
        {
            return View();
        }

        public ActionResult GetProdutos()
        {
            using (BD_ProjetoFinalEntities db = new BD_ProjetoFinalEntities())
            {
                var qry = from p in db.Produto
                          join c in db.Categoria on p.Categoria equals c.ID_Categoria
                          select new
                          {
                              p.ID_PRODUTO,
                              p.SKU,
                              p.Marca,
                              p.Modelo,
                              p.Categoria,
                              Categoria1 = c.Categoria1, // Aqui estamos obtendo o nome da categoria através do join
                              p.Quantidade,
                              p.Caracteristicas,
                              p.Foto,
                              p.Preço
                          };

                return Json(new { data = qry.ToList() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}