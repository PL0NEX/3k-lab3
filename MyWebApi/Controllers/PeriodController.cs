using MyWebApi.Domain.Entities;
using MyWebApi.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("period")]
    public class PeriodsController : ControllerBase
    {
        private readonly ILogger<PeriodsController> logger;
        private readonly IRepository<Period> periodRepository;

        public PeriodsController(ILogger<PeriodsController> logger, IRepository<Period> periodRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.periodRepository = periodRepository ?? throw new ArgumentNullException(nameof(periodRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Period>>> Get()
        {
            logger.LogInformation("Get all periods");
            return Ok(await periodRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Period>> Get(Guid id)
        {
            var period = await periodRepository.GetByIdAsync(id);
            if (period == null) return NotFound($"Период с ID {id} не найден.");

            return Ok(period);
        }

        [HttpPost]
        public async Task<ActionResult> Insert([FromBody] Period period)
        {
            var validationResult = ValidatePeriod(period);
            if (!string.IsNullOrWhiteSpace(validationResult))
                return BadRequest(validationResult);

            if (!period.Id.HasValue)
                period.Id = Guid.NewGuid();

            return Ok(await periodRepository.InsertAsync(period));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] Period period)
        {
            var validationResult = ValidatePeriod(period);
            if (!string.IsNullOrWhiteSpace(validationResult))
                return BadRequest(validationResult);

            if (await periodRepository.GetByIdAsync(id) == null)
                return NotFound($"Период с ID {id} не существует.");

            await periodRepository.UpdateAsync(id, period);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            if (await periodRepository.GetByIdAsync(id) == null)
                return NotFound($"Период с ID {id} не существует.");

            await periodRepository.DeleteAsync(id);
            return Ok();
        }

        private string ValidatePeriod(Period period)
        {
            if (period == null) return "Период не может быть пустым.";
            if (period.StartDate >= period.EndDate) return "Дата начала должна быть раньше даты окончания.";

            return string.Empty;
        }
    }
}
