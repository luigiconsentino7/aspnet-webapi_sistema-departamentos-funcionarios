using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace aspnet_evosystem.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public bool Ativo { get; set; }

        public List<Funcionario> Funcionarios { get; set; }

        public Departamento()
        {
            Nome = String.Empty;
            Sigla = String.Empty;
            Ativo = true;
            Funcionarios = new List<Funcionario>();
        }

        public void DisableDepartamento()
        {
            Ativo = false;
        }
    }
}
