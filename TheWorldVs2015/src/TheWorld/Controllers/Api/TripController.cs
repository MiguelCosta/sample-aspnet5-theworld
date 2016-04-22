using AutoMapper;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Net;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private IWorldRepository _repository;

        public TripController(IWorldRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var trips = _repository.GetAllTripsWithStops();
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

                    //todo: save in database

                    var resultObj = Mapper.Map<TripViewModel>(newTrip);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(resultObj);
                }
            }
            catch(System.Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
