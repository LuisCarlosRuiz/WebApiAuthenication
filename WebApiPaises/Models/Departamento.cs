using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiPaises.Models
{
	public class Departamento
	{
		public int Id { get; set; }

		[StringLength(50)]
		public string Nombre { get; set; }

		public int PaisId { get; set; }

		[ForeignKey("PaisId")]
		public Pais pais { get; set; }
	}
}
