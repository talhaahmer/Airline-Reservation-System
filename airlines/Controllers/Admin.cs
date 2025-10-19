using airlines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace airlines.Controllers
{
    public class Admin : Controller
   
        {
        AirlinesContext db = new AirlinesContext();
        
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Showcities()
            {
                return View(db.Cities.ToList());
            }


        [HttpGet]
        public IActionResult Addcities()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Addcities(City abc)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(abc);
                db.SaveChanges();
             
                return RedirectToAction("Showcities");
            }
            return View(abc);
        }

        [HttpGet]
        public IActionResult EditCities(int id)
        {
            var abc = db.Cities.Find(id);

            return View(abc);
        }
        [HttpPost]
        public IActionResult EditCities(City abc)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Update(abc);
                db.SaveChanges();
                return RedirectToAction("Showcities");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteCities(int id)
        {
            var abc = db.Cities.Find(id);

            return View(abc);
        }
        [HttpPost]
        public IActionResult DeleteCities(City abc)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Remove(abc);
                db.SaveChanges();
                return RedirectToAction("Showcities");
            }
            return View();
        }

        ///////////////////////////////////
        ///
        public IActionResult ShowClasses()
        {
            return View(db.Classes.ToList());
        }

        [HttpGet]
        public IActionResult AddClasses()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClasses(Class abc)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(abc);
                db.SaveChanges();
               
                return RedirectToAction("ShowClasses");
            }
            return View(abc);
        }

        [HttpGet]
        public IActionResult EditClasses(int id)
        {
            var abc = db.Classes.Find(id);

            return View(abc);
        }
        [HttpPost]
        public IActionResult EditClasses(Class abc)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Update(abc);
                db.SaveChanges();
                return RedirectToAction("ShowClasses");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteClasses(int Id)
        {
            var abc = db.Classes.Find(Id);

            return View(abc);
        }
        [HttpPost]
        public IActionResult DeleteClasses(Class abc)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Remove(abc);
                db.SaveChanges();
                return RedirectToAction("ShowClasses");
            }
            return View();
        }



        ///////////////////////
        ///
        [HttpGet]

        public IActionResult ShowFlight()
        {
            var myflightContext = db.Flights.Include(p => p.OriginCityNavigation).Include(p => p.DestinationCityNavigation);

            return View(myflightContext.ToList());
        }



        //[HttpGet]
        //public IActionResult AddFlight()
        //{
        //    ViewData["Id"] = new SelectList(db.Cities, "Id", "Namee");

        //    return View();
        //}

        [HttpGet]
        public IActionResult AddFlight()
        {
            var cities = db.Cities.ToList();
            ViewBag.OriginCity = new SelectList(cities, "Id", "Namee");
            ViewBag.DestinationCity = new SelectList(cities, "Id", "Namee");
            return View();
        }

        [HttpPost]
        public IActionResult AddFlight(Flight ab)
        {
            if (ModelState.IsValid)
            {
                db.Flights.Add(ab);
                db.SaveChanges();
                return RedirectToAction("ShowFlight");
            }

           
            return View();
        }



        [HttpGet]
        public IActionResult EditFlights(int id)
        {
            var cities = db.Cities.ToList();
            ViewBag.OriginCity = new SelectList(cities, "Id", "Namee");
            ViewBag.DestinationCity = new SelectList(cities, "Id", "Namee");
            var ab = db.Flights.Find(id);

            return View(ab);
        }
        [HttpPost]
        public IActionResult EditFlights(Flight ab)
        {
            if (ModelState.IsValid)
            {
                db.Flights.Update(ab);
                db.SaveChanges();
                return RedirectToAction("ShowFlight");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteFlights(int Id)
        {
            var ab = db.Flights.Find(Id);

            return View(ab);
        }
        [HttpPost]
        public IActionResult DeleteFlights(Flight ab)
        {
            if (ModelState.IsValid)
            {
                db.Flights.Remove(ab);
                db.SaveChanges();
                return RedirectToAction("ShowFlights");
            }
            return View();
        }


        /////////
        ///
        public IActionResult ShowUsers()
        {

            var users = db.Logins.Include(u => u.Role).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult DeleteUsers(int Id)
        {
            var ab = db.Logins.Find(Id);

            return View(ab);
        }
        [HttpPost]
        public IActionResult DeleteUsers(Login ab)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Remove(ab);
                db.SaveChanges();
                return RedirectToAction("ShowFlights");
            }
            return View();
        }

    


      public IActionResult adminShowBookings()
        {
        
            var bookings = (from r in db.Reservations
                            join f in db.Flights on r.Fid equals f.Id
                        
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



    }
    }