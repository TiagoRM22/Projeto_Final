//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projeto_Final.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Produto
    {
        public int ID_PRODUTO { get; set; }
        public string SKU { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public Nullable<int> Categoria { get; set; }
        public Nullable<int> Quantidade { get; set; }
        public string Caracteristicas { get; set; }
        public string Foto { get; set; }
        public Nullable<decimal> Preço { get; set; }
    
        public virtual Categoria Categoria1 { get; set; }
    }
}
