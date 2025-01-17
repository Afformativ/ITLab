﻿using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataBaseController : Controller
    {
        private readonly itDBContext dbContext;
        public DataBaseController(itDBContext dbContext)
        {
            this.dbContext= dbContext;
        }


        [HttpGet]
        public IActionResult GetDataBase()
        {
            return Ok(dbContext.DataBase.ToList());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetDataBase([FromRoute] Guid id)
        {
            var dataBase = await dbContext.DataBase.FindAsync(id);

            if (dataBase == null)
            {
                return NotFound();
            }
            return Ok(dataBase);
        }

        [HttpPost]
        public async Task<IActionResult> PostDataBase(AddDataBaseRequest addDataBaseRequest)
        {
            var dataBase = new DataBase()
            {
                Id = Guid.NewGuid(),
                Name = addDataBaseRequest.Name
            };
            
           await dbContext.DataBase.AddAsync(dataBase);
           await dbContext.SaveChangesAsync();

           return Ok(dataBase);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateDataBase([FromRoute] Guid id, UpdateDataBaseRequest updateDataBaseRequest)
        {
            var dataBase = await dbContext.DataBase.FindAsync(id);
            if (dataBase != null)
            {
                dataBase.Name = updateDataBaseRequest.Name;

                await dbContext.SaveChangesAsync();

                return Ok(dataBase);
            }

            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteDataBase([FromRoute] Guid id)
        {
            var dataBase = await dbContext.DataBase.FindAsync(id);

            if (dataBase != null)
            {
                dbContext.Remove(dataBase);
                await dbContext.SaveChangesAsync();
                return Ok(dataBase);
            }
            return NotFound();
        }
    }
}
