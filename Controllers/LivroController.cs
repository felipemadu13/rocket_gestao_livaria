using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace livrariaapi.Controllers;

[ApiController]
[Route("[controller]")]
public class LivroController : ControllerBase
{

    private readonly ILogger<LivroController> _logger;
    private readonly AppDbContext _context;

    public LivroController(ILogger<LivroController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get() {
        var livros = await _context.Livro.ToListAsync();
        if (livros == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }

        return Ok(livros);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult Get([FromRoute] int id) {

        var livro = _context.Livro.FirstOrDefault(p => p.id == id);
        if (livro == null)
            return NotFound(new { message = "Livro não encontrado." });

        return Ok(livro);
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Create(Livro livro) {

        if (livro is null)
            return BadRequest(new { message = "Erro de cadastro. Dados inválidos" });
        _context.Livro.Add(livro);
        _context.SaveChanges();

        return Created("~/livro",new { message = "Usuário cadastrado com sucesso." });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, Livro livroAtualizado) {
        if (livroAtualizado == null || id != livroAtualizado.id)
            return BadRequest(new { message = "Dados inválidos." });

        var livro = _context.Livro.FirstOrDefault(p => p.id == id);
        if (livro == null)
            return NotFound(new { message = "Livro não encontrado." });

        livro.titulo = livroAtualizado.titulo;
        livro.autor = livroAtualizado.autor;
        livro.genero = livroAtualizado.genero;
        livro.preco = livroAtualizado.preco;
        livro.QuantidadeEstoque = livroAtualizado.QuantidadeEstoque;

        _context.Livro.Update(livro);
        _context.SaveChanges();

        return Ok(new { message = "Livro atualizado com sucesso." });
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) {
        var livro = _context.Livro.FirstOrDefault(p => p.id == id);
        if (livro == null)
            return NotFound(new { message = "Livro não encontrado." });

        _context.Livro.Remove(livro);
        _context.SaveChanges();

        return Ok(new { message = "Livro deletado com sucesso." });
    }

    
}
