using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private ILogger _logger;
        private IWorldRepository _repository;

        public StopController(IWorldRepository repository, ILogger<StopController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);
                if(trip == null)
                {
                    return Json(null);
                }

                var stops = trip.Stops.OrderBy(s => s.Order);
                var stopsView = Mapper.Map<IEnumerable<StopViewModel>>(stops);

                return Json(stopsView);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get stops for trip {tripName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding trip name");
            }
        }

        public JsonResult Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    // Map to the Entity
                    var newStop = Mapper.Map<Stop>(vm);

                    // Looking up Geocoordinates

                    // Save to the DataBase 
                    _repository.AddStop(tripName, newStop);

                    if(_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        var stopView = Mapper.Map<StopViewModel>(newStop);
                        return Json(stopView);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to save new stop", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new stop");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");
        }
    }
}
