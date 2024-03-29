﻿
namespace aspnet_evosystem.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string? Foto { get; set; }
        public string Rg { get; set; }
        public int DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; }
        public bool Ativo {  get; set; }

        public Funcionario() 
        {
            Nome = String.Empty;
            Sobrenome = String.Empty;
            Foto = String.Empty;
            Rg = String.Empty;
            Ativo = true;
        }

        public void DisableFuncionario()
        {
            Ativo = false;
        }

        public void EnableFuncionario()
        {
            Ativo = true;
        }
    }
}
