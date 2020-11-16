using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Dominio
{
    public class Processo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string NumeroProcesso { get; set; }

        [StringLength(100)]
        public string Classe { get; set; }

        [StringLength(100)]
        public string Area { get; set; }

        [StringLength(100)]
        public string Assunto { get; set; }

        public string Origem { get; set; }

        [StringLength(100)]
        public string Distribuicao { get; set; }

        [StringLength(100)]
        public string Relator { get; set; }

        public List<Movimentacao> Movimentacao { get; set; }

    }
}
