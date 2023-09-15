using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Shared.DTO
{
	public class ReservaDTO 
	{
		public int NroReserva { get; set; }
		[Required(ErrorMessage = "La Fecha de inicio es Obligatoria")]
		public DateTime Fecha_inicio { get; set; }
		[Required(ErrorMessage = "La Fecha de fin es Obligatoria")]
		public DateTime Fecha_fin { get; set; }
		[Required(ErrorMessage = "El Dni del dueño de la reserva es obligatorio")]
		public int Dni { get; set; }
        [Required(ErrorMessage = "El Dni de los huespedes es obligatorio")]
        public string DniHuesped { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} es requerido")]
        public int Nhabs { get; set; }
        public List<HuespedDTO> Huespedes { get; set; } = new List<HuespedDTO>();

		public List<HabitacionDTO> Habitaciones { get; set; } = new List<HabitacionDTO>();
	}
}
