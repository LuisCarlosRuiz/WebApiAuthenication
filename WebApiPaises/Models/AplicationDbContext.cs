using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaises.Models
{
	public class AplicationDbContext : IdentityDbContext<AplicationUser>
	{
		public AplicationDbContext(DbContextOptions<AplicationDbContext> options):base(options)
		{

		}

		public DbSet<Pais> Pais { get; set; }		

		public DbSet<Departamento> departamento { get; set; }		
	}
}
