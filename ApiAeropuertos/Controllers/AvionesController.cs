﻿using ApiAeropuertos.Models;
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

        


        Repository<Partidas> repository;
        private readonly sistem21_avionesafContext context;
        public AvionesController(sistem21_avionesafContext context)
        {
           
            this.context = context;
            repository = new(context);
      
        }


        [HttpGet]
        public IActionResult Get()
        {


            //DateTime fechaactual = DateTime.Now.Date;
            //TimeSpan horaactual = DateTime.Now.TimeOfDay;
            //var data = repository.Get();
            //var cambios = data.Select(x => new Partidas { Id = x.Id, Vuelo = x.Vuelo, Destino = x.Destino, Status = x.Status, Puerta = x.Puerta, Tiempo=x.Tiempo });

            //foreach (var item in cambios.ToList())
            //{


            //    if (item.Tiempo.Date < fechaactual)
            //    {

            //        item.Status = "On Boarding";
            //        repository.Update(item);


            //    }
            //    else if (item.Tiempo.Date == fechaactual)
            //    {
            //        if ((item.Tiempo.TimeOfDay - horaactual).TotalMinutes < 10)
            //        {
            //            item.Status = "On Boarding";
            //            repository.Update(item);
            //        }
            //    }

            //}

            var data2 = repository.Get().OrderBy(x => x.Tiempo);
            return Ok(data2.Select(x => new Partidas { Id = x.Id,  Vuelo = x.Vuelo, Destino=x.Destino,Status=x.Status,  Puerta=x.Puerta, Tiempo=x.Tiempo }));
       
        }

        [HttpPost]
        public IActionResult Post(Partidas p)
        {
            if (p == null)
            {
                return BadRequest("Debe proporcionar un producto");
            }

            if (Validate(p, out List<string> errores))
            {
                //no hay errores
                Partidas entidad = new()
                {
                    
                      Destino=p.Destino,
                      Status=p.Status,
                      Puerta=p.Puerta,
                      Tiempo=p.Tiempo,
                      Vuelo=p.Vuelo
                      
                };
                repository.Insert(entidad);
                return Ok();
            }
            else //tiene errores
            {
                return BadRequest(errores);
            }

        }
        private bool Validate(Partidas p, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(p.Destino))
            {
                errors.Add("Escriba el nombre del producto.");
            }

            if (string.IsNullOrWhiteSpace(p.Vuelo))
            {
                errors.Add("Seleccione la categoria del producto");
            }
            if (string.IsNullOrWhiteSpace(p.Puerta))
            {
                errors.Add("Seleccione la categoria del producto");
            }

            if (string.IsNullOrWhiteSpace(p.Status))
            {
                errors.Add("Seleccione la categoria del producto");
            }

          
            if (repository.Get().Any(x => x.Vuelo == p.Vuelo && x.Id != p.Id))
            {
                errors.Add("Ya existe un producto con el mismo nombre");
            }

            

            return errors.Count == 0;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var Vuelo = repository.Get().Where(x => x.Id == id)
                .Select(x => new Partidas
                {

                   
                    Id = x.Id,
                    Destino=x.Destino,
                    Vuelo=x.Vuelo,
                    Puerta=x.Puerta,
                    Status=x.Status,
                Tiempo=x.Tiempo
                    

                }).FirstOrDefault();
            return Ok(Vuelo);
        }

        [HttpPut]
        public IActionResult Put(Partidas p)
        {
            if (p == null)
            {
                return BadRequest("No se indico el producto a modificar.");
            }

            if (Validate(p, out List<string> errores))
            {
                var entidad = repository.GetById(p.Id);
                if (entidad == null)
                {
                    return NotFound();
                }

                entidad.Puerta = p.Puerta;
                entidad.Status = p.Status;
                entidad.Tiempo = p.Tiempo;
                entidad.Destino = p.Destino;
                entidad.Vuelo = p.Vuelo;
                repository.Update(entidad);
                return Ok();

            }
            else
            {
                return BadRequest(errores);
            }
        }

    }
}