using System.Collections.Generic;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        void AddStop(string tripName, Stop newStop);

        void AddTrip(Trip newTrip);

        IEnumerable<Trip> GetAllTrips();

        IEnumerable<Trip> GetAllTripsWithStops();

        Trip GetTripByName(string tripName);

        bool SaveAll();
    }
}
