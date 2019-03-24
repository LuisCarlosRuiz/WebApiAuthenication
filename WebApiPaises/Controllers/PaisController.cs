using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class PaisController : ControllerBase
	{
		private readonly AplicationDbContext context;

		public PaisController(AplicationDbContext ctx) =>
			this.context = ctx;

		[HttpGet]
		public IEnumerable<Pais> Get()
		{
			return context.Pais.Include(q => q.Departamento).ToList();
		}

		[HttpGet("{id}", Name = "PaisCreado")]
		public IActionResult GetById(int id)
		{
			var pais = context.Pais.FirstOrDefault(q => q.Id == id);

			if (pais == null)
				return NotFound();
			else
				return Ok(pais);
		}

		[HttpPost]
		public IActionResult Post([FromBody] Pais pais)
		{
			if (ModelState.IsValid)
			{
				context.Pais.Add(pais);
				context.SaveChanges();
				return new CreatedAtRouteResult("PaisCreado", new { Id = pais.Id }, pais);
			}
			else
				return BadRequest();
		}

		[HttpPut("{id}")]
		public IActionResult Put([FromBody] Pais pais, int id)
		{
			if (pais.Id != id)
				return BadRequest();

			context.Entry(pais).State = EntityState.Modified;
			context.SaveChanges();
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var pais = context.Pais.FirstOrDefault(q => q.Id == id);

			if (pais == null)
				return NotFound();

			context.Pais.Remove(pais);
			context.SaveChanges();
			return Ok(pais);
		}
	}
}