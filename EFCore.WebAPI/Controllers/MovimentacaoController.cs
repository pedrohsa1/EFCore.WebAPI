using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.Dominio;
using EFCore.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EFCore.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoController : ControllerBase
    {

        public readonly ApplicationDBContext _context;

        public MovimentacaoController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET api/movimentacao
        [HttpGet]
        public ActionResult Get()
        {
            var listMovimentacao = (from movimentacao in _context.Movimentacoes select movimentacao).ToList();
            return Ok(listMovimentacao);
        }

        // GET api/movimentacao
        [HttpGet("filtro/{numeroMovimentacao}")]
        public ActionResult GetFiltro(string numeroMovimentacao)
        {
            var listMovimentacoes = (from movimentacao in _context.Movimentacoes
                                 where movimentacao.Id.Equals(numeroMovimentacao)
                                 select movimentacao).ToList();
            if (listMovimentacoes.Count == 0)
            {
                return NotFound();
            }
            return Ok(listMovimentacoes);
        }

        [HttpPost]
        public ActionResult Post(Movimentacao movimentacao)
        {
            try
            {
                _context.Movimentacoes.Add(movimentacao);
                _context.SaveChanges();
                return Ok("Cadastrado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // PUT api/movimentacao/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, Movimentacao movimentacao)
        {
            try
            {
                if (_context.Movimentacoes.Find(id) != null)
                {
                    _context.Movimentacoes.Add(movimentacao);
                    _context.SaveChanges();
                    return Ok("Movimentacao Atualizado com Sucesso!");
                }
                return Ok("Não encontrado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/movimentacao/delete/5
        [HttpDelete("delete/{id}")]
        public void Delete(int id)
        {
            var movimentacao = _context.Movimentacoes.Where(x => x.Id == id).Single();
            _context.Remove(movimentacao);
            _context.SaveChanges();
        }
    }
}
