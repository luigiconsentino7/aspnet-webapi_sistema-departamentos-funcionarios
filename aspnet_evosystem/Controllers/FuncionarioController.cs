using aspnet_evosystem.Data;
using aspnet_evosystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace aspnet_evosystem.Controllers
{

    [Route("api/funcionarios")]
    [ApiController]
    public class FuncionarioController : Controller
    {

        readonly AppDbContext  _context;

       public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Listar todos os Funcionários
        /// </summary>
        /// <returns>Todos os Funcionários ativos</returns>
        /// <response code = "200">Sucesso</response>
        [HttpGet("GetAllFuncionarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var funcionario = _context.FuncionariosDb
                .Include(d => d.Departamento)
                .Where(d => d.Ativo).ToList();

            return Ok(funcionario);
        }

        /// <summary>
        /// Listar um Funcionário específico
        /// </summary>
        /// <param name="id">Identificador do Funcionário</param>
        /// <returns>Funcionário específico</returns>
        /// <reponse code="200">Sucesso</reponse>
        /// <response code="404">Departamento não encontrado</response>
        [HttpGet("GetFuncionario {id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var funcionario = _context.FuncionariosDb
                .Include(d => d.Departamento)
                .SingleOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }
            return Ok(funcionario);
        }

        /// <summary>
        /// Cadastrar um Funcionário
        /// </summary>
        /// <remarks>
        ///
        /// ```json
        /// {
        ///     "nome": "string",
        ///     "sobrenome": "string",
        ///     "rg": "string",
        ///     "departamentoId": 0
        /// }
        /// ```
        /// 
        /// </remarks>
        /// <param name="funcionario"></param>
        /// <returns>Funcionário Criado</returns>
        /// <response code="201">Sucesso</response>

        [HttpPost("PostFuncionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostFuncionario(Funcionario funcionario)
        {
            var departamentoExistente = await _context.DepartamentosDb.FindAsync(funcionario.DepartamentoId);

            if (departamentoExistente == null || departamentoExistente.Ativo == false)
            {
                return NotFound("Departamento não encontrado.");
            }

            funcionario.Departamento = departamentoExistente;

            _context.FuncionariosDb.Add(funcionario);
            _context.SaveChanges();

            departamentoExistente.Funcionarios.Add(funcionario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = funcionario.Id }, funcionario);
        }

        /// <summary>
        /// Atualizar um Funcionário
        /// </summary>
        /// <param name="id">Identificador do Funcionário</param>
        /// <param name="input">Dados do Funcionário</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("UpdateFuncionario {id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(int id, Funcionario input)
        {
            var funcionario = _context.FuncionariosDb.SingleOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            funcionario.Nome = input.Nome;
            funcionario.Sobrenome = input.Sobrenome;
            funcionario.Rg = input.Rg;
            funcionario.DepartamentoId = input.DepartamentoId;

            _context.FuncionariosDb.Update(funcionario);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Desativar um Funcionário
        /// </summary>
        /// <param name="id">Identificador do Funcionário</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("DisableFuncionario {id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Disable(int id)
        {
            var funcionario = _context.FuncionariosDb.SingleOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            funcionario.DisableFuncionario();

            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletar um Funcionário
        /// </summary>
        /// <param name="id">Identificador do Funcionário</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("DeleteFuncionario {id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(int id)
        {
            var funcionario = _context.FuncionariosDb.SingleOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            _context.FuncionariosDb.Remove(funcionario);

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Upload de Foto de um Funcionário
        /// </summary>
        /// <param name="funcionarioId">Identificador do Funcionário</param>
        /// <param name="file">Arquivo</param>
        /// <returns>Botão para Dowload do Arquivo</returns>
        [HttpPost("UploadImagem/{funcionarioId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
        public async Task<ActionResult> UploadFoto(int funcionarioId, IFormFile file)
        {
            try
            {
                var funcionario = _context.FuncionariosDb.SingleOrDefault(f => f.Id == funcionarioId);

                if (funcionario == null)
                {
                    return NotFound("Funcionário não encontrado");
                }

                if (file == null || file.Length == 0)
                {
                    return BadRequest("Arquivo inválido ou ausente");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Converta os dados binários da imagem em uma representação Base64
                    var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                    // Gere uma URL temporária para a imagem
                    var imageUrl = $"data:image/png;base64,{base64Image}";

                    // Atualize a propriedade Foto do funcionário com a URL da imagem
                    funcionario.Foto = imageUrl;
                }

                await _context.SaveChangesAsync();

                // Retorne a URL da imagem como resultado
                return Ok(new { Link = funcionario.Foto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }



    }
}
