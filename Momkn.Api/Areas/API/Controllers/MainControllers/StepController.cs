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
    public class StepController : ControllerBase
    {
        private readonly IStepRepository _dbContext;

        public StepController(IStepRepository StepRepository)
        {
            _dbContext = StepRepository;
            
        }
        [HttpGet]
      
        public IActionResult GetAllSteps()
        {
            List<Step> steps = _dbContext.GetAll().OrderBy(x => x.Number).ToList();

            return Ok(ResponseHelper.Success(data:steps));
        }

        [HttpGet]
       
        public ActionResult<Step> GetStep(int ID)
        {
            var step = _dbContext.GetById(ID);

            if (step == null)
            {
                return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));
            }

            return Ok(ResponseHelper.Success(data: step));
        }

        // PUT: api/Step/5
        [HttpPost]
        public IActionResult UpdateStep([FromBody] StepDTO StepDto)
        {
            Step step = _dbContext.GetById(StepDto.ID);
            try
            {
                step.Name = StepDto.Name;
                step.Number = StepDto.Number;
                _dbContext.Update(step);
            }
            catch (Exception ex)
            {
                if (!StepExists(StepDto.ID))
                {
                    return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));

                }
                else
                {
                    return BadRequest(ResponseHelper.Fail(message: ex.ToString()));
                }
            }

            return Ok(ResponseHelper.Success(data: step));
        }

        // POST: api/Step
        [HttpPost]
        public ActionResult<Step> AddStep([FromBody] StepDTO StepDto)
        {
            try
            {
                Step step = new Step();
                step.Name = StepDto.Name;
                step.Number = StepDto.Number;
                _dbContext.Add(step);

                return Ok(ResponseHelper.Success(data: step));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail(message: ex.ToString()));
            }
        }

        // DELETE: api/Step/5
        [HttpDelete]
        public ActionResult<Step> DeleteStep(int ID)
        {
            var step = _dbContext.GetById(ID);
            if (step == null)
            {
                return NotFound(ResponseHelper.Fail(message: "Record you look for not found"));
            }

            _dbContext.Delete(step);
            return Ok(ResponseHelper.Success(data: step));
        }

        private bool StepExists(int ID)
        {
            return _dbContext.Exists(ID);
        }
    }
}
