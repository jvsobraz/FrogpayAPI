using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrogPayAPI.Models;
using FrogPayAPI.Data;

namespace FrogPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly FrogPayContext _context;

        public PessoaController(FrogPayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            return await _context.Pessoas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return pessoa;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Pessoas
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.IdPessoa }, pessoa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.IdPessoa)
            {
                return BadRequest();
            }
            _context.Entry(pessoa).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PessoaExists(int id)
        {
            return _context.Pessoas.Any(e => e.IdPessoa == id);
        }

         [HttpGet("{id}/dados-bancarios-endereco")]
        public async Task<ActionResult<Pessoa>> GetDadosBancariosEndereco(int id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.DadosBancarios)
                .Include(p => p.Enderecos)
                .FirstOrDefaultAsync(p => p.IdPessoa == id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoasPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {       
            var pessoas = await _context.Pessoas
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return pessoas;
}
        }

    }
    }

