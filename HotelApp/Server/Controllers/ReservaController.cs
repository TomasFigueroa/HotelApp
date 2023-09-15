using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;
using System.Diagnostics.Eventing.Reader;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Reservar")]
    //
    public class ReservaController : ControllerBase
    {
        private readonly Context context;

        public ReservaController(Context context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> Get()
        {
            return await context.Reservas.ToListAsync();
        }

        [HttpGet("int:NroReserva")]
        public async Task<ActionResult<Reserva>> GetDniPersona(int nroReserva)
        {
            var buscar = await context.Reservas.FirstOrDefaultAsync(c => c.NroReserva == nroReserva);

            if (buscar is null)
            {
                return BadRequest($"No se encontro la reserva de nro: {nroReserva}");
            }

            return buscar;
        }
        [HttpPost]
        public async Task<IActionResult> Post(ReservaDTO reservaDTO)
        {
            var responseApi = new ResponseAPI<int>(); 

            var entidad = await context.Reservas.FirstOrDefaultAsync(x => x.NroReserva == reservaDTO.NroReserva);

            if (entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe una reserva");
            }


            try
            {
                List<Habitacion> listahab = new List<Habitacion>();
                List<Huesped> listahues = new List<Huesped>();
                var mdReserva = new Reserva
                {
                    NroReserva = reservaDTO.NroReserva,
                    Fecha_inicio = reservaDTO.Fecha_inicio,
                    Fecha_fin = reservaDTO.Fecha_fin,
                    Dni = reservaDTO.Dni,
                    nhabs = reservaDTO.Nhabs
                };

                foreach (var huespedDTO in reservaDTO.Huespedes)
                {
                    var huesped = await context.Huespedes.FirstOrDefaultAsync(c => c.Dni == reservaDTO.DniHuesped);
                    if (huesped != null) { mdReserva.Huespedes.Add(huesped); }
                    else { responseApi.EsCorrecto = false;
                        responseApi.Mensaje += " falta el huesped con dni " + reservaDTO.DniHuesped;
                    }
                }
                foreach (var habitacionDTO in reservaDTO.Habitaciones)
                {
                    var habitacion = await context.Habitaciones.FirstOrDefaultAsync(c => c.Nhab == reservaDTO.Nhabs);
                    if (habitacion != null)
                    {
                        mdReserva.Habitaciones.Add(habitacion);
                    } else { responseApi.EsCorrecto = false; responseApi.Mensaje += "fallo en la habitacion nro: " + reservaDTO.Nhabs; }
                }
                context.Reservas.Add(mdReserva);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex); }
        }
    }
}
