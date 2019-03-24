using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
    [Route("api/Pais/{paisId}/Departamento")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
		private readonly AplicationDbContext ctx;

		public DepartamentoController(AplicationDbContext context) => this.ctx = context;

		[HttpGet]
		public IEnumerable<Departamento> Get(int paisId)
		{
			return ctx.departamento.Where(q => q.PaisId == paisId).ToList();
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var departamento = ctx.departamento.FirstOrDefault(q => q.Id == id);

			if (departamento != null)
				return Ok(departamento);
			else
				return NotFound();
		}

		[HttpPost]
		public IActionResult Post([FromBody] Departamento departamento)
		{
			if (ModelState.IsValid)
			{
				ctx.departamento.Add(departamento);
				ctx.SaveChanges();
				return Ok();
			}
			else
				return BadRequest();
		}

		[HttpPut("{id}")]
		public IActionResult Put([FromBody] Departamento departamento, int id)
		{
			if (departamento.Id != id)
				return BadRequest();

			ctx.Entry(departamento).State = EntityState.Modified;
			ctx.SaveChanges();
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var departamento = ctx.departamento.FirstOrDefault(q => q.Id == id);

			if (departamento == null)
				return NotFound();

			ctx.departamento.Remove(departamento);
			ctx.SaveChanges();
			return Ok();
		}
    }
}