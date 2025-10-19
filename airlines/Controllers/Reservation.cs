using airlines.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace airlines.Controllers
{

    public class Reservation : Controller
    {
        AirlinesContext db = new AirlinesContext();
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View(db.Reservations.ToList());
        }
        [HttpGet]
        public IActionResult SearchFlight(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
                return View(new List<Flight>());

            var flights = db.Flights
                            .Where(f => f.FlightNumber.Contains(flightNumber))
                            .ToList();

            return View(flights);
        }


        [HttpGet]
        public ActionResult Search()
        {
            return View(new FlightSearchModel());
        }


        [HttpPost]
        public ActionResult Search(FlightSearchModel model, string c1, string c2)
        {
            var results = new List<string>();

            var sourceCity = db.Cities.FirstOrDefault(c => c.Namee.Trim().ToLower() == c1.Trim().ToLower());
            var destCity = db.Cities.FirstOrDefault(c => c.Namee.Trim().ToLower() == c2.Trim().ToLower());

            if (sourceCity == null || destCity == null)
            {
                results.Add("Invalid source or destination city.");
                model.Results = results;
                return View(model);
            }

            // 1. Direct Flight
            var directFlight = db.Flights
                .Include(f => f.DestinationCityNavigation)
                .FirstOrDefault(f => f.OriginCity == sourceCity.Id && f.DestinationCity == destCity.Id);

            if (directFlight != null)
            {
                results.Add($"✈️ Direct Flight Found: {sourceCity.Namee} → {destCity.Namee}");
                results.Add($"Duration: {directFlight.Duration} mins | Price: {directFlight.BasePrice}");

                ViewBag.direct = directFlight.Id;
                ViewBag.data = sourceCity.Namee;
                ViewBag.data1 = destCity.Namee;
            }
            else
            {
                // 2. Nearest cities
                var nearestSource = GetNearestCity(sourceCity.Namee);
                var nearestDest = GetNearestCity(destCity.Namee);

                // Option 1: source → nearestSource → dest
                var alt1 = db.Flights
                    .Include(f => f.DestinationCityNavigation)
                    .FirstOrDefault(f => f.OriginCity == sourceCity.Id && f.DestinationCity == nearestSource.Id);
                var alt2 = db.Flights
                    .Include(f => f.DestinationCityNavigation)
                    .FirstOrDefault(f => f.OriginCity == nearestSource.Id && f.DestinationCity == destCity.Id);

                // Option 2: source → nearestDest → dest
                var alt3 = db.Flights
                    .Include(f => f.DestinationCityNavigation)
                    .FirstOrDefault(f => f.OriginCity == sourceCity.Id && f.DestinationCity == nearestDest.Id);
                var alt4 = db.Flights
                    .Include(f => f.DestinationCityNavigation)
                    .FirstOrDefault(f => f.OriginCity == nearestDest.Id && f.DestinationCity == destCity.Id);

                if (alt1 != null && alt2 != null)
                {
                    int duration = (alt1.Duration ?? 0) + (alt2.Duration ?? 0);
                    int price = (alt1.BasePrice ?? 0) + (alt2.BasePrice ?? 0);
                    results.Add("✅ Suggested route (via nearest source):");
                    results.Add($"{sourceCity.Namee} → {alt1.DestinationCityNavigation?.Namee} → {destCity.Namee}");
                    results.Add($"Total Duration: {duration} mins | Total Price: {price}");
                    ViewBag.alt1 = alt1.Id;
                    ViewBag.alt2 = alt2.Id;
                    ViewBag.data = sourceCity.Namee;
                    ViewBag.data1 = destCity.Namee;

                    ViewBag.stopover1 = alt1.DestinationCityNavigation?.Namee;
                }
                else if (alt3 != null && alt4 != null)
                {
                    int duration = (alt3.Duration ?? 0) + (alt4.Duration ?? 0);
                    int price = (alt3.BasePrice ?? 0) + (alt4.BasePrice ?? 0);
                    results.Add("✅ Suggested route (via nearest destination):");
                    results.Add($"{sourceCity.Namee} → {alt3.DestinationCityNavigation?.Namee} → {destCity.Namee}");
                    results.Add($"Total Duration: {duration} mins | Total Price: {price}");
                    ViewBag.alt3 = alt3.Id;
                    ViewBag.alt4 = alt4.Id;
                    ViewBag.data = sourceCity.Namee;
                    ViewBag.data1 = destCity.Namee;
                    ViewBag.stopover2 = alt3.DestinationCityNavigation?.Namee;
                }
                else
                {
                    results.Add("❌ No suitable flight route found.");
                }



            }

            model.Results = results;
            return View(model);
        }

        private City GetNearestCity(string cityName)
        {
            var inputCity = db.Cities
                .FirstOrDefault(c => c.Namee.Trim().ToLower() == cityName.Trim().ToLower());

            if (inputCity == null || inputCity.Latitude == null || inputCity.Longitude == null)
                return null;

            var inputLat = (double)inputCity.Latitude;
            var inputLon = (double)inputCity.Longitude;

            return db.Cities
                .Where(c => c.Id != inputCity.Id && c.Latitude != null && c.Longitude != null)
                .AsEnumerable()
                .OrderBy(c =>
                    Math.Sqrt(
                        Math.Pow((double)c.Latitude - inputLat, 2) +
                        Math.Pow((double)c.Longitude - inputLon, 2)
                    )
                )
                .FirstOrDefault();
        }





        [HttpPost]
        public IActionResult Booking(string SelectedRouteIds)
        {
            ViewBag.Class = new SelectList(db.Classes.ToList(), "Id", "Namee");
            if (string.IsNullOrEmpty(SelectedRouteIds))
            {
                TempData["Error"] = "Please select a route to continue booking.";
                return RedirectToAction("Search", "Flights");
            }

            var routeIds = SelectedRouteIds.Split(',').Select(int.Parse).ToList();

            var flights = db.Flights
                .Include(f => f.OriginCityNavigation)
                .Include(f => f.DestinationCityNavigation)
                .Where(f => routeIds.Contains(f.Id))
                .ToList();

            if (flights.Count == 0)
            {
                TempData["Error"] = "Flights not found for selected route.";
                return RedirectToAction("Search", "Flights");
            }

            var itinerary = new TripItineraryViewModel
            {
                Flights = flights.OrderBy(f => f.DepartureTime).ToList(),
                TotalDuration = flights.Sum(f => f.Duration ?? 0),
                TotalPrice = flights.Sum(f => f.BasePrice ?? 0)
            };

            
            return View("ConfirmBooking", itinerary);
        }


        [HttpPost]
        public JsonResult CheckSeatAvailability(int totalSeatsRequested, string selectedRouteIds)
        {
            if (string.IsNullOrEmpty(selectedRouteIds))
            {
                return Json(new { success = false, message = "No flight selected." });
            }

            int selectedFlightId;
            if (!int.TryParse(selectedRouteIds, out selectedFlightId))
            {
                return Json(new { success = false, message = "Invalid flight ID." });
            }

            var flight = db.Flights.FirstOrDefault(f => f.Id == selectedFlightId);

            if (flight == null)
            {
                return Json(new { success = false, message = "Flight not found." });
            }

            if (flight.AvailableSeats < totalSeatsRequested)
            {
                return Json(new
                {
                    success = false,
                    insufficient = new[]
                    {
                new {
                    FlightNumber = flight.FlightNumber,
                    availableSeats = flight.AvailableSeats,
                }
            }
                });
            }

            return Json(new
            {

                success = true,
                flightId = flight.Id,
                availableSeats = flight.AvailableSeats,
                basePrice = flight.BasePrice,
                FlightNumber = flight.FlightNumber,
            });

        }

        [HttpPost]
        public IActionResult ConfirmFinalBooking(string SelectedRouteId, int PassengerClass, string TripType, DateTime DepartureDate, DateTime? ReturnDate, int Adults, int Children, int Seniors,
     string CreditCardUsed, string ExpiryDate, string CVV)
        {
            int totalPassengers = Adults + Children + Seniors;
            int flightId = int.Parse(SelectedRouteId);
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;
            int userId = Convert.ToInt32(abc);

            var flight = db.Flights.FirstOrDefault(f => f.Id == flightId);
            if (flight == null)
                return BadRequest("Flight not found");

            if (flight.AvailableSeats < totalPassengers)
                return BadRequest("Not enough seats available");


            int basePrice = (int)flight.BasePrice;


            double total = ((Adults * basePrice) + (Children * basePrice * 0.5) + (Seniors * basePrice * 0.7));
            int totalPrice = (int)Math.Round(total);


            flight.AvailableSeats -= totalPassengers;
            db.SaveChanges();



            Models.Reservation reservation = new Models.Reservation
            {
                Userid = userId,
                Cstatus = "Confirmed",
                TotalPrice = totalPrice,
                ReservationDate = DateTime.Now,
                CreditCardUsed = CreditCardUsed,
                Fid = flightId,
                PesCount = totalPassengers,
                TripType = TripType,
                DepartureDate = DepartureDate,
                ReturnDate = ReturnDate.HasValue ? ReturnDate.Value : (DateTime?)null,
                ClassId = PassengerClass
            };

            db.Reservations.Add(reservation);
            db.SaveChanges();

            return Json(new { success = true });
        }

  


        [HttpPost]
        public IActionResult BlockFlightSeats(int SelectedRouteId, int PassengerClass, string TripType,
    DateTime DepartureDate, DateTime? ReturnDate, int Adults, int Children, int Seniors)
        {
            int totalSeats = Adults + Children + Seniors;
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;
            int userId = Convert.ToInt32(abc);

            var flight = db.Flights.FirstOrDefault(f => f.Id == SelectedRouteId);
            if (flight == null)
                return Json(new { success = false, message = "Flight not found." });

            if (flight.AvailableSeats < totalSeats)
                return Json(new { success = false, message = "Not enough available seats." });

            // Subtract seats
            flight.AvailableSeats -= totalSeats;
            db.SaveChanges();


            int basePrice = (int)flight.BasePrice;


            double total = ((Adults * basePrice) + (Children * basePrice * 0.5) + (Seniors * basePrice * 0.7));
            int totalPrice = (int)Math.Round(total);
            // Create reservation
            var reservation = new Models.Reservation
            {
                Userid = userId,
                Cstatus = "Blocked",
                TotalPrice = totalPrice,
                ReservationDate = DateTime.Now,
                CreditCardUsed = null,
                Fid = flight.Id,
                PesCount = totalSeats,
                TripType = TripType,
                DepartureDate = DepartureDate,
                ReturnDate = ReturnDate.HasValue ? ReturnDate.Value : (DateTime?)null,
                ClassId = PassengerClass
            };

            db.Reservations.Add(reservation);
            db.SaveChanges();

            return Json(new { success = true, blockingNumber = reservation.Id });
        }



        public IActionResult MyBookings()
        {
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;
            int userId = Convert.ToInt32(abc);
            var bookings = (from r in db.Reservations
                            join f in db.Flights on r.Fid equals f.Id
                            where r.Userid == userId
                            select new airlines.Models.BookingViewModell
                            {
                                ReservationId = r.Id,
                                FlightNumber = f.FlightNumber,
                                Origin = f.OriginCityNavigation.Namee,
                                Destination = f.DestinationCityNavigation.Namee,
                                DepartureTime = f.DepartureTime,
                                ArrivalTime = f.ArrivalTime,
                                TotalPrice = r.TotalPrice ?? 0,
                                Status = r.Cstatus,
                                TripType = r.TripType,
                                ReservationDate = r.ReservationDate
                            }).ToList();

            return View(bookings);



        }
        [HttpPost]
        public IActionResult CancelReservation(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation != null && reservation.Cstatus != "Cancelled")
            {
                reservation.Cstatus = "Cancelled";

                var flight = db.Flights.FirstOrDefault(f => f.Id == reservation.Fid);
                if (flight != null)
                {
                    int seatsToReturn = reservation.PesCount ?? 0;
                    flight.AvailableSeats += seatsToReturn;
                }

                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public IActionResult ConfirmReservation(int reservationId, int PassengerClass, string TripType,
            DateTime DepartureDate, DateTime? ReturnDate, int Adults, int Children, int Seniors,
            string CreditCardUsed, string ExpiryDate, string CVV)
        {
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;
            int userId =Convert.ToInt32( abc);
            int a, b;
            var reservation = db.Reservations.FirstOrDefault(r => r.Id == reservationId && r.Cstatus == "Blocked");
            if (reservation == null)
                return BadRequest("No valid blocked reservation found.");

            else
            {
                a = (int)reservation.PesCount;
                b = (int)reservation.TotalPrice;

            }
            var flight = db.Flights.FirstOrDefault(f => f.Id == reservation.Fid);
            if (flight == null)
                return BadRequest("Flight not found.");
            
          

            // ✅ Update existing reservation
            reservation.Userid = userId;
            reservation.Cstatus = "Confirmed";
            reservation.TotalPrice = b;
            reservation.ReservationDate = DateTime.Now;
            reservation.CreditCardUsed = "112";
            reservation.PesCount = a;
            reservation.TripType = "OneWay";
            reservation.DepartureDate = DepartureDate;
            ReturnDate = ReturnDate.HasValue ? ReturnDate.Value : (DateTime?)null;
            reservation.ClassId = 1;

            db.SaveChanges();

            return Json(new { success = true, message = "Booking confirmed!" });
        }

    }



    }






