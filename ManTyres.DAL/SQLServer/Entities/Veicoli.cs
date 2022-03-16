using ManTyres.DAL.SQLServer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
    public partial class Veicoli
    {
        public Veicoli()
        {
            Pneumaticis = new HashSet<Pneumatici>();
        }
        [Key]
        public int VeicoloId { get; set; }
        public string Targa { get; set; }
        public string Marca { get; set; }
        public string Modello { get; set; }
        public int? ClienteId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DataCreazione { get; set; }


        [ForeignKey("ClienteId")]
        public virtual Clienti Cliente { get; set; }
        public virtual ICollection<Pneumatici> Pneumaticis { get; set; }
    }
}
