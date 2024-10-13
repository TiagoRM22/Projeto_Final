using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MYEMAIL.Models
{
    public class EmailFormModel
    {
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [EmailAddress(ErrorMessage = "O endereço de email é inválido.")]
        public string Para { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Display(Name = "Seu Nome")]
        public string DeNome { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [EmailAddress(ErrorMessage = "O endereço de email é inválido.")]
        [Display(Name = "Seu Email")]
        public string DeEmail { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        public string Mensagem { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Display(Name = "Contacto")]
        public string Telefone { get; set; }
    }
}