﻿using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RowController : Controller
    {
        private readonly itDBContext dbContext;
        public RowController(itDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetRow()
        {
            return Ok(dbContext.Row.ToList());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRow([FromRoute] Guid id)
        {
            var row = await dbContext.Row.FindAsync(id);

            if (row == null)
            {
                return NotFound();
            }
            return Ok(row);
        }

        [HttpPost]
        public async Task<IActionResult> PostrOW(AddRowRequest addRowRequest)
        {
            var row = new Row()
            {
                Id = Guid.NewGuid(),
                TableId= addRowRequest.TableId
            };

            await dbContext.Row.AddAsync(row);
            await dbContext.SaveChangesAsync();

            return Ok(row);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRow([FromRoute] Guid id, UpdateRowRequest updateRowRequest)
        {
            var row = await dbContext.Row.FindAsync(id);
            if (row != null)
            {
                row.TableId = updateRowRequest.TableId;

                await dbContext.SaveChangesAsync();

                return Ok(row);
            }

            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRow([FromRoute] Guid id)
        {
            var row = await dbContext.Row.FindAsync(id);

            if (row != null)
            {
                dbContext.Remove(row);
                await dbContext.SaveChangesAsync();
                return Ok(row);
            }
            return NotFound();
        }
    }
}
