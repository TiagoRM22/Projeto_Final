using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MYEMAIL.Models;

namespace Projeto_Final.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CriarColaborador()
        {
            ViewBag.Message = "Página de Criação de Novos Colaboradores.";
            return View();
        }

        public ActionResult Enviado()
        {
            return View();
        }

        public ActionResult Contacto()
        {
            EmailFormModel emailmsg = new EmailFormModel
            {
                Para = "ricardo.cavalheiro@my.istec.pt"
            };
            return View(emailmsg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contacto(EmailFormModel model, HttpPostedFileBase fich)
        {
            if (ModelState.IsValid)
            {
                var tomsg = model.Para;
                var body = "<p>Email From: {0} ({1})</p><p>Mensagem:</p><p>{2}</p>";
                var mensagem = new MailMessage();

                mensagem.To.Add(new MailAddress(tomsg));
                mensagem.From = new MailAddress("ricardo.cavalheiro@my.istec.pt");
                mensagem.Subject = "O assunto do seu email";
                mensagem.Body = string.Format(body, model.DeNome, model.DeEmail, model.Mensagem, model.Telefone);
                mensagem.IsBodyHtml = true;

                if (fich != null && fich.ContentLength > 0)
                {
                    mensagem.Attachments.Add(new Attachment(fich.InputStream, fich.FileName));
                }

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "ricardo.cavalheiro@my.istec.pt",
                        Password = "sssss"
                    };
                     smtp.Credentials = credential;
                     smtp.Host = "smtp.gmail.com";
                     smtp.Port = 25;//587
                     smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mensagem);
                    return RedirectToAction("Enviado");
                }
            }
            return View(model);
        }
    }
}
