using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EFCore.Dominio;
using EFCore.Repo;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        public readonly ApplicationDBContext _context;

        public ProcessoController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET api/processo
        [HttpGet]
        public ActionResult Get()
        {
            var listProcessos = (from processo in _context.Processos select processo).ToList();
            return Ok(listProcessos);
        }

        // GET api/processo
        [HttpGet("filtro/{numeroProcesso}")]
        public ActionResult GetFiltro(string numeroProcesso)
        {
            Processo consultaProcesso = (from processo in _context.Processos
                                         where processo.NumeroProcesso.Equals(numeroProcesso)
                                         select processo).FirstOrDefault();
            
            
            if (consultaProcesso != null)
            {
                consultaProcesso.Movimentacao = new List<Movimentacao>();
                consultaProcesso.Movimentacao = (from movimentacao in _context.Movimentacoes
                                                 where movimentacao.ProcessoId.Equals(consultaProcesso.Id)
                                                 select movimentacao).ToList();
                return Ok(consultaProcesso);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Post(Processo processo)
        {
            try
            {
                _context.Processos.Add(processo);
                _context.SaveChanges();
                return Ok("Cadastrado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // PUT api/processo/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, Processo processo)
        {
            try
            {
                if (_context.Processos.Find(id) != null)
                {
                    _context.Processos.Add(processo);
                    _context.SaveChanges();
                    return Ok("Processo Atualizado com Sucesso!");
                }
                return Ok("Não encontrado");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // DELETE api/processo/delete/5
        [HttpDelete("delete/{id}")]
        public void Delete(int id)
        {
            var processo = _context.Processos.Where(x => x.Id == id).Single();
            _context.Remove(processo);
            _context.SaveChanges();
        }
    }
}