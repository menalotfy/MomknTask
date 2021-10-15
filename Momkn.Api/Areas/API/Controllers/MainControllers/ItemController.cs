using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Momkn.API.Helpers;
using Momkn.Core.DTOs.MainEntitiesDTO;
using Momkn.Core.Enitities.MainEntity;
using Momkn.Core.Interfaces.MainInterface;

namespace Momkn.API.Areas.API.Controllers.MainControllers
{
    [Route("API/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _dbContext;

        public ItemController(IItemRepository ItemRepository)
        {
            _dbContext = ItemRepository;
            
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllItems()
        {
            List<Item> items = _dbContext.GetAll().ToList();

            return Ok(ResponseHelper.Success(data:items));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<Item> GetItem(int ID)
        {
            var Item = _dbContext.GetById(ID);

            if (Item == null)
            {
                return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));
            }

            return Ok(ResponseHelper.Success(data:Item));
        } 
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<Item> GetAllItemsStep(int StepID)
        {
            var items = _dbContext.GetAllItemsStep(StepID);

            if (items== null)
            {
                return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));
            }

            return Ok(ResponseHelper.Success(data:items));
        }

        // PUT: api/Item/5
        [HttpPost]
        public IActionResult UpdateItem([FromBody] ItemDTO ItemDto)
        {
            Item item = _dbContext.GetById(ItemDto.ID);
            try
            {
                item.Title = ItemDto.Title;
                item.Description = ItemDto.Description;
                item.StepID = ItemDto.StepID;
                _dbContext.Update(item);
            }
            catch (Exception ex)
            {
                if (!ItemExists(ItemDto.ID))
                {
                    return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));

                }
                else
                {
                    return BadRequest(ResponseHelper.Fail(message: ex.ToString()));
                }
            }

            return Ok(ResponseHelper.Success(data: item));
        }

        // POST: api/Item
        [HttpPost]
        public ActionResult<Item> AddItem([FromBody] ItemDTO ItemDto)
        {
            try
            {
                Item Item = new Item();

                Item.Title = ItemDto.Title;
                Item.Description = ItemDto.Description;
                Item.StepID = ItemDto.StepID;
                _dbContext.Add(Item);

                return Ok(ResponseHelper.Success(data: Item));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail(message: ex.ToString()));
            }
        }

        // DELETE: api/Item/5
        [HttpDelete]
        public ActionResult<Item> DeleteItem(int ID)
        {
            var item = _dbContext.GetById(ID);
            if (item == null)
            {
                return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));
            }

            _dbContext.Delete(item);
            return Ok(ResponseHelper.Success(data: item));
        }

        private bool ItemExists(int ID)
        {
            return _dbContext.Exists(ID);
        }
    }
}
