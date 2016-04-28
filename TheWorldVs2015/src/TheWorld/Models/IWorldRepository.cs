using System.Collections.Generic;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        void AddStop(string tripName, string username, Stop newStop);

        void AddTrip(Trip newTrip);

        IEnumerable<Trip> GetAllTrips();

        IEnumerable<Trip> GetAllTripsWithStops();

        Trip GetTripByName(string tripName, string username);

        IEnumerable<Trip> GetUserTripsWithStops(string name);

        bool SaveAll();
    }
}
