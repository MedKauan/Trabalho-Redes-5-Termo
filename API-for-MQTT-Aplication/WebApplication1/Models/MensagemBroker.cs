using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MensagemBroker
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo é obrigatório")]
        public string Mensagem { get; set; }
    }
}
