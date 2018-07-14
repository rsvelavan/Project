using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossSolar.Controllers
{
    [Route("api/[controller]")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        private readonly IPanelRepository _panelRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository, IPanelRepository panelRepository)
        {
            _analyticsRepository = analyticsRepository;
            _panelRepository = panelRepository;

        }



        // GET panel/XXXX1111YYYY2222/analytics
        //[HttpGet("{banelId}/[controller]")]
        [HttpGet("{panelId}")]
        public async Task<IActionResult> Get(string panelId)
        {
            var analytics = await _analyticsRepository.Query()
                .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();

            var result = new OneHourElectricityListModel
            {
                OneHourElectricitys = analytics.Select(c => new OneHourElectricityModel
                {
                    Id = c.Id,
                    KiloWatt = c.KiloWatt,
                    DateTime = c.DateTime
                }).ToList()
            };
            return Ok(result);
        }

        // GET panel/XXXX1111YYYY2222/analytics/day
        //[HttpGet("{panelId}/[controller]/day")]
        [HttpGet]
        [Route("DayResults/{pDate}")]
        public async Task<IActionResult> DayResults(string pDate)
        {
            DateTime giveDate = Convert.ToDateTime(pDate);
            
            var analytics = await _analyticsRepository.Query()
                                    .Where(x => x.DateTime.Year == giveDate.Year
                                    && x.DateTime.Month == giveDate.Month
                                    && x.DateTime.Day == giveDate.Day).ToListAsync();
            var result = analytics.AsQueryable()
                         .GroupBy(k => k.DateTime.ToShortDateString())
                         .Select(g => new
                         {
                             DateTime = g.Key.ToString(),
                             Sum = g.Sum(i => i.KiloWatt),
                             Average = g.Average(i => i.KiloWatt),
                             Minimum = g.Min(i => i.KiloWatt),
                             Maximum = g.Max(i => i.KiloWatt)
                         }).ToList();


            return Ok(result);
        }

        // POST panel/XXXX1111YYYY2222/analytics
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OneHourElectricityInsert value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var oneHourElectricityContent = new OneHourElectricity
            {
                KiloWatt = value.KiloWatt,
                DateTime = DateTime.UtcNow,
                PanelId = value.PanelId
            };

            await _analyticsRepository.InsertAsync(oneHourElectricityContent);

            var result = new OneHourElectricityModel
            {
                Id = oneHourElectricityContent.Id,
                KiloWatt = oneHourElectricityContent.KiloWatt,
                DateTime = oneHourElectricityContent.DateTime,
                PanelId = oneHourElectricityContent.PanelId
            };

            return Created($"panel/{result.PanelId}/analytics/{result.Id}", result);

        }
    }
}