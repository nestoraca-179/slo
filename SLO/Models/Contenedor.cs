//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SLO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contenedor
    {
        public int ID { get; set; }
        public int id_bl { get; set; }
        public string num_cont { get; set; }
        public Nullable<int> num_paq { get; set; }
        public string tip_cont { get; set; }
        public Nullable<int> estado { get; set; }
        public string eq_inter_rec1 { get; set; }
        public string eq_inter_rec2 { get; set; }
        public string eq_inter_rec3 { get; set; }
        public string seal_party { get; set; }
        public Nullable<decimal> peso_neto { get; set; }
        public Nullable<decimal> peso_bruto { get; set; }
        public Nullable<int> tamanio { get; set; }
        public Nullable<decimal> temper { get; set; }
        public string imo { get; set; }
        public string num_un { get; set; }
        public string ventilac { get; set; }
        public string descrip_mer { get; set; }
        public string co_us_in { get; set; }
        public Nullable<System.DateTime> fe_us_in { get; set; }
        public string co_us_mo { get; set; }
        public Nullable<System.DateTime> fe_us_mo { get; set; }
    
        public virtual BL BL { get; set; }
    }
}
