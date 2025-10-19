using airlines.Models;

namespace airlines.Services
{

    // Services/ReservationService.cs
    public class ReservationService
    {
        private readonly AirlinesContext _db = new AirlinesContext();

        public ReservationService(AirlinesContext db)
        {
            _db = db;
        }

        public void AutoUnblockExpiredReservations()
        {
            var cutoffTime = DateTime.Now.AddMinutes(-15);

            var expiredReservations = _db.Reservations
                .Where(r => r.Cstatus == "Blocked" && r.ReservationDate < cutoffTime)
                .ToList();

            foreach (var reservation in expiredReservations)
            {
                var flight = _db.Flights.FirstOrDefault(f => f.Id == reservation.Fid);
                if (flight != null)
                {
                    flight.AvailableSeats += reservation.PesCount ?? 0;
                }

                // Either delete or update status
                reservation.Cstatus = "Expired";
            }

            _db.SaveChanges();
        }
    }
}