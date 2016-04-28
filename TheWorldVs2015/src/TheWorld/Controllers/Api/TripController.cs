using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips")]
    public class TripController : Controller
    {
        private ILogger<TripController> _logger;
        private IWorldRepository _repository;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var trips = _repository.GetUserTripsWithStops(User.Identity.Name);
            var results = Mapper.Map<IEnumerable<TripViewModel>>(trips);
            return Json(results);
        }

        [HttpPost]
        public JsonResult Post([FromBody] TripViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(vm);
                    newTrip.UserName = User.Identity.Name;
                    //todo: save in database
                    _logger.LogInformation("Attempting to save a new trip");
                    _repository.AddTrip(newTrip);

                    if(_repository.SaveAll())
                    {
                        var resultObj = Mapper.Map<TripViewModel>(newTrip);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(resultObj);
                    }
                }
            }
            catch(System.Exception ex)
            {
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
