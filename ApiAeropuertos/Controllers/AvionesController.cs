using ApiAeropuertos.Models;
using ApiAeropuertos.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ApiAeropuertos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvionesController : Controller
    {

        

     
        public Repository<Partidas> repository;
        private readonly sistem21_avionesafContext context;
        public AvionesController(sistem21_avionesafContext context)
        {
           
            this.context = context;
            repository = new(context);
           

      
        }
        
        //public async Task filtrar()
        //{

        //    var data2 =  repository.Get().OrderBy(x => x.Tiempo).ToList();

        //    DateTime fechaactual = DateTime.UtcNow;

        //    foreach (var item in data2)
        //    {
               

        //        if (item.Tiempo.Date == fechaactual.Date && item.Status.ToLower() == "on time")
        //        {
        //            if (((item.Tiempo.TimeOfDay - fechaactual.TimeOfDay).TotalMinutes) < 10)
        //            {
        //                item.Status = "On Boarding";
        //                await repository.Update(item);
        //            }



        //        }
        //        else if (item.Tiempo.Date < fechaactual.Date && item.Status.ToLower() == "on time")
        //        {
        //            item.Status = "On Boarding";
        //            await repository.Update(item);
        //        }



        //    }
        //}

        [HttpGet]
        public IActionResult Get()
        {
           
            //await filtrar();
            var datosrepo = repository.Get().OrderBy(x => x.Tiempo);
            return Ok(datosrepo.Select(x => new Partidas { Id = x.Id, Vuelo = x.Vuelo, Destino = x.Destino, Status = x.Status, Puerta = x.Puerta, Tiempo = x.Tiempo }));


        }

       

        [HttpPost]
        public IActionResult Post(Partidas v)
        {
            if (v == null)
            {
                return BadRequest("Debe enviar un vuelo.");
            }

            if (Validate(v, out List<string> errores))
            {
                //no hay errores
                Partidas vuelo = new()
                {
                    
                      Destino=v.Destino,
                      Status=v.Status,
                      Puerta=v.Puerta,
                      Tiempo=v.Tiempo,
                      Vuelo=v.Vuelo
                      
                };
                repository.Insert(vuelo);
                return Ok();
            }
            else //tiene errores
            {
                return BadRequest(errores);
            }

        }
      
        public IActionResult Index()
        {
            return View();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var vuelo = repository.GetById(id);
            if (vuelo == null)
            {
                return NotFound();
            }

            repository.Delete(vuelo);
            return Ok();
        }
        [HttpPut]
        public IActionResult Put(Partidas v)
        {
            if (v == null)
            {
                return BadRequest("No se indico el vuelo a editar.");
            }

            if (ValidateUpdate(v, out List<string> errores))
            {
                var vuelo = repository.GetById(v.Id);

                if (vuelo == null)
                {
                    return NotFound();
                }

                vuelo.Puerta = v.Puerta;
                vuelo.Status = v.Status;
                vuelo.Tiempo = v.Tiempo;
                vuelo.Destino = v.Destino;
                vuelo.Vuelo = v.Vuelo;
                repository.Update(vuelo);
                return Ok();

            }
            else
            {
                return BadRequest(errores);
            }
        }

        private bool Validate(Partidas v, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(v.Destino))
            {
                errors.Add("Especifique el destino del vuelo.");
            }

            if (string.IsNullOrWhiteSpace(v.Vuelo))
            {
                errors.Add("Ingrese una clave de vuelo.");
            }
            if (string.IsNullOrWhiteSpace(v.Puerta))
            {
                errors.Add("Seleccione la puerta de salida.");
            }

            if (repository.Get().Any(x => x.Vuelo.ToLower() == v.Vuelo.ToLower() && x.Id != v.Id))
            {
                errors.Add("Ya existe un vuelo con la misma clave.");
            }



            if (repository.Get().Any(x => x.Puerta == v.Puerta && x.Tiempo.Date == v.Tiempo.Date && (x.Status.ToLower() != "cancelado" || x.Status.ToLower() != "en vuelo")) )
            {
                var lista = repository.Get().Select(x=>x).Where(x => x.Puerta == v.Puerta && x.Tiempo.Date == v.Tiempo.Date && (x.Status.ToLower() != "cancelado" || x.Status.ToLower() != "en vuelo"));
                foreach (var item in lista)
                {
                    if (diff(item.Tiempo, v.Tiempo))
                        {
                        errors.Add("La puerta elegida esta ocupada a la hora ingresada.");
                        break;
                    }
                 
                   
                }
          
            }

            if (errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool diff(DateTime x, DateTime v)
        {
            var diferencia = x.TimeOfDay.Subtract(v.TimeOfDay).TotalMinutes;
            if (Math.Abs(diferencia) < 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateUpdate(Partidas v, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(v.Destino))
            {
                errors.Add("Especifique el destino del vuelo.");
            }

            if (string.IsNullOrWhiteSpace(v.Vuelo))
            {
                errors.Add("Ingrese una clave de vuelo.");
            }
            if (string.IsNullOrWhiteSpace(v.Puerta))
            {
                errors.Add("Seleccione la puerta de salida.");
            }

            if (repository.Get().Any(x => x.Vuelo.ToLower() == v.Vuelo.ToLower() && x.Id != v.Id))
            {
                errors.Add("Ya existe un vuelo con la misma clave.");
            }

            if (errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

         
        
     

    }
}
