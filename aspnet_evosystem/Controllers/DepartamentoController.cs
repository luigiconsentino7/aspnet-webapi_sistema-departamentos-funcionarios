using aspnet_evosystem.Data;
using aspnet_evosystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnet_evosystem.Controllers
{
    [Route("api/departamentos")]
    [ApiController]
    public class DepartamentoController : Controller
    {

        readonly AppDbContext _context;

        public DepartamentoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Listar todos os Departamentos
        /// </summary>
        /// <returns>Todos os Departamentos ativos</returns>
        /// <response code = "200">Sucesso</response>
        [HttpGet("GetAllDepartamentos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var departamento = _context.DepartamentosDb.ToList();

            return Ok(departamento);
        }

        /// <summary>
        /// Listar um Departamento específico por ID com a lista de Funcionários do Departamento
        /// </summary>
        /// <param name="id">Identificador do Departamento</param>
        /// <returns>Lista de Funcionários cadastrados no Departamento</returns>
        /// <reponse code="200">Sucesso</reponse>
        /// <response code="404">Departamento não encontrado</response>
        [HttpGet("GetDepartamento/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var departamento = _context.DepartamentosDb
                .Include(d => d.Funcionarios)
                .SingleOrDefault(d => d.Id == id);

            if (departamento == null)
            {
                return NotFound("Departamento não encontrado.");
            }
            return Ok(departamento);
        }



        /// <summary>
        /// Cadastrar um Departamento
        /// </summary>
        /// <remarks>
        ///
        /// ```json
        /// {
        ///     "nome": "string",
        ///     "sigla": "string"
        /// }
        /// ```
        /// 
        /// </remarks>
        /// <param name="departamento"></param>
        /// <returns>Departamento Criado</returns>
        /// <response code="201">Sucesso</response>

        [HttpPost("PostDepartamento")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.DepartamentosDb.Add(departamento);
                _context.SaveChanges();


                return CreatedAtAction(nameof(GetById), new { id = departamento.Id }, departamento);
            }

            return BadRequest();

        }

        /// <summary>
        /// Atualizar um Departamento por ID
        /// </summary>
        /// <param name="id">Identificador do Departamento</param>
        /// <param name="input">Dados do Departamento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("UpdateDepartamento/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(int id, Departamento input)
        {
            var departamento = _context.DepartamentosDb.SingleOrDefault(d => d.Id == id);

            if (departamento == null)
            {
                return NotFound("Departamento não encontrado.");
            }

            departamento.Nome = input.Nome;
            departamento.Sigla = input.Sigla;


            _context.DepartamentosDb.Update(departamento);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Desativar um Departamento por ID
        /// </summary>
        /// <param name="id">Identificador do Departamento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("DisableDepartamento/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Inactivate(int id)
        {
            var departamento = _context.DepartamentosDb.SingleOrDefault(d => d.Id == id);

            if (departamento == null)
            {
                return NotFound("Departamento não encontrado.");
            }

            departamento.DisableDepartamento();

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Ativar um Departamento por ID
        /// </summary>
        /// <param name="id">Identificador do Departamento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPost("EnableDepartamento/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Enable(int id)
        {
            var departamento = _context.DepartamentosDb.SingleOrDefault(d => d.Id == id);

            if (departamento == null)
            {
                return NotFound("Departamento não encontrado.");
            }

            departamento.EnableDepartamento();

            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletar um Departamento por ID
        /// </summary>
        /// <param name="id">Identificador do Departamento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("DeleteDepartamento/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(int id)
        {
            var departamento = _context.DepartamentosDb.SingleOrDefault(d => d.Id == id);

            if (departamento == null)
            {
                return NotFound("Departamento não encontrado.");
            }

            _context.DepartamentosDb.Remove(departamento);

            _context.SaveChanges();

            return NoContent();
        }

    }
}
