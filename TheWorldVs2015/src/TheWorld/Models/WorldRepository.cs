using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var trip = GetTripByName(tripName);

            newStop.Order = trip.Stops.Any() ?
                              trip.Stops.Max(s => s.Order) + 1 : 1;
            _context.Add(newStop);
        }

        public void AddTrip(Trip newTrip)
        {
            newTrip.Created = DateTime.UtcNow;
            _context.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _context.Trips.OrderBy(t => t.Name);
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                throw;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name).ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not get stops from database", ex);
                throw;
            }
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips.Include(t => t.Stops)
                            .FirstOrDefault(t => t.Name == tripName);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Trip> TripFindBy(Expression<Func<Trip, bool>> predicate)
        {
            return _context.Trips.Where(predicate);
        }
    }
}
