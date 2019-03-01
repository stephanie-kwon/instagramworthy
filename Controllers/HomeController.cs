using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InstagramWorthy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Hosting;


namespace InstagramWorthy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment he;
        private YourContext dbContext;
    
        // here we can "inject" our context service into the constructor
        public HomeController(YourContext context, IHostingEnvironment e)
        {
            dbContext = context;
            he = e;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Signup")]
        public ViewResult Signup()
        {
            return View();
        }
        
        [HttpPost]
        [Route("Signup")]
        public IActionResult Signup(User user)
        {
            if(ModelState.IsValid)
            {
                //Users came from DB table name. 
                if(dbContext.users.Any(u => u.Email == user.Email))
                {
                ModelState.AddModelError("Email", "Email already in use.");
                return View("Signup");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                if(!IsValidContentType(user.ImageFile.ContentType))
                {
                    User user1 = new User();
                    ViewBag.user = user1;
                    List<User> alluser1 = dbContext.users.ToList();
                    ViewBag.allUsers = user1;
                    ViewBag.id = HttpContext.Session.GetInt32("UserId");
                    ViewBag.Error = "Only JPG, JPEG, or PNG";    
                    return View("Signup");
                }

                string filename = Path.Combine(he.WebRootPath,user.ImageFile.FileName);
                user.ImageFile.CopyTo(new FileStream(filename, FileMode.Create));
                string filepath = "/" + Path.GetFileName(user.ImageFile.FileName);
        

                User registerUser = new User 
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Location = user.Location,
                    ProfileUrl = filepath
                }; 
                dbContext.Add(registerUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("id", registerUser.UserId);
                HttpContext.Session.SetString("Name", registerUser.FirstName);
                int? id_registered = HttpContext.Session.GetInt32("id");
                return RedirectToAction("Profile");
            }
            return View("Signup");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login (string login_Email, string login_Password)
        {
            LoginUser bob = new LoginUser
            {
                login_Email = login_Email,
                login_Password = login_Password
            };
            if(TryValidateModel(bob))
            {
                var userinDB = dbContext.users.FirstOrDefault(u => u.Email == login_Email);
                
                if(userinDB == null)
                {
                    // ModelState.AddModelError("login_Email", "Invalid Email");
                    ViewBag.EmailError = "Invalid Email";
                    return View("login");
                }

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(userinDB, userinDB.Password, login_Password);
                // if(userinDB.Password != login_Password)
                if(result == 0)
                {
                    System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$");
                    // ModelState.AddModelError("login_Password", "Invalid Email");
                    ViewBag.PasswordError = "Invalid Password";
                    return View("login");
                }                

            HttpContext.Session.SetInt32("id", userinDB.UserId);
            HttpContext.Session.SetString("Name", userinDB.FirstName);
            int? logged_in_id = HttpContext.Session.GetInt32("id");

            System.Console.WriteLine("@#$#@$$@#$@");
            System.Console.WriteLine(logged_in_id);
            

            ViewBag.userID = (int)logged_in_id;


            System.Console.WriteLine("!!!!!!!!!");
            System.Console.WriteLine(ViewBag.userID);
            return RedirectToAction("profile");

            }
            else 
            {
                System.Console.WriteLine("its working?");
        
            
                return View("login"); 
            }
        }

        [HttpGet]
        [Route("Login")]
        public ViewResult login()
        {
            return View();
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("Profile")]
        public IActionResult profile(User user)
        {
            Place place2 = new Place();
            ViewBag.place = place2;
            List<Place> allPlaces = dbContext.places.ToList();
            ViewBag.allPlaces = allPlaces;

            List<User> returnUser = dbContext.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).ToList();

            ViewBag.returnUser = returnUser;

            User userInfo = new User();
            ViewBag.userLoggedin = userInfo;

            return View("profile");
        }

        [HttpGet]
        [Route("UserReviews")]
        public IActionResult UserReviews()
        {
            Place place2 = new Place();
            ViewBag.place = place2;
            List<Place> allPlaces = dbContext.places.Include(x => x.Uploader).ToList();
            ViewBag.allPlaces = allPlaces;

            List<User> returnUser = dbContext.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).ToList();
            
            ViewBag.returnUser = returnUser;

            User userInfo = new User();
            ViewBag.userLoggedin = userInfo;

            
            return View("UserReviews");
        }


        [HttpGet]
        [Route("Place/{PlaceId}")]
        public ViewResult Place(int PlaceId)
        {
            // Place place2 = new Place();
            // ViewBag.place = place2;
            // List<Place> allPlaces = dbContext.places.Include(x => x.Uploader).ToList();
            // ViewBag.allPlaces = allPlaces;


            // Place placeAssociation = dbContext.places.Include(x => x.Uploader).Include(y => y.Liked).ThenInclude(b => b.place).FirstOrDefault(x => x.PlaceId == PlaceId);
            // ViewBag.placeAssociation = placeAssociation;

            Place onePlace = dbContext.places
                .Include(y => y.Liked)
                .Include(x => x.Uploader)
                .FirstOrDefault(x => x.PlaceId == PlaceId);

            System.Console.WriteLine("!!!!");
            System.Console.WriteLine(onePlace.Location);

            // var likedPost = dbContext.places.Include(x => x.Uploader).Include(like => like.Liked).ToList();
            // ViewBag.likedPost = likedPost;
        
            return View("Place", onePlace);
        }

        [HttpGet]
        [Route("AddPlace")]
        public ViewResult AddPlace()
        {
            int? logged_in_id = HttpContext.Session.GetInt32("id");
            ViewBag.userId = logged_in_id;
            return View();
        }

        private bool IsValidContentType(string ContentType)
        {
            return ContentType.Equals("image/png") || ContentType.Equals("image/jpg") || ContentType.Equals("image/jpeg");
        }
        private bool IsValidContentType(int contentLength)
        {
            return ((contentLength / 1024 / 1024)< 1); //1MB
        }
        [HttpPost]
        [Route("AddPlace")]
        public IActionResult updatingPlace(Place place)
        {   

            if(ModelState.IsValid)
            {
                
                if(!IsValidContentType(place.ImageFile.ContentType))
                {
                    Place place1 = new Place();
                        ViewBag.place = place1;
                        List<Place> allPlaces1 = dbContext.places.ToList();
                        ViewBag.allPlaces = allPlaces1;
                        ViewBag.id = HttpContext.Session.GetInt32("UserId");
                        ViewBag.Error = "Only JPG, JPEG, or PNG";
                        return View("AddPlace");
                }

                System.Console.WriteLine("hiiiiii?");

                string filename = Path.Combine(he.WebRootPath,place.ImageFile.FileName);
                place.ImageFile.CopyTo(new FileStream(filename, FileMode.Create));
                string filepath = "/" + Path.GetFileName(place.ImageFile.FileName);
        
                Place newPlace = new Place 
                {
                    Name = place.Name,
                    Location = place.Location,
                    Description = place.Description, 
                    UserId = (int)HttpContext.Session.GetInt32("id"),
                    PicUrl = filepath
                };
                dbContext.Add(newPlace);
                dbContext.SaveChanges();
                return Redirect($"/Place/{newPlace.PlaceId}");

            }
                System.Console.WriteLine("meow?");

                
                Place place2 = new Place();
                ViewBag.place = place2;
                List<Place> allPlaces = dbContext.places.ToList();
                ViewBag.allPlaces = allPlaces;
                ViewBag.id = HttpContext.Session.GetInt32("UserId");
                return View("AddPlace");
        }
    
        [HttpGet]
        [Route("{PlaceId}")]
        public IActionResult like(int PlaceId) 
        {
            System.Console.WriteLine("hello in like");
            Like newLike = new Like
            {
                UserId = (int)HttpContext.Session.GetInt32("id"),
                PlaceId = PlaceId
            };
            dbContext.Add(newLike);
            dbContext.SaveChanges();
            return Redirect($"/Place/{newLike.PlaceId}");
        }

        [HttpGet]
        [Route("Settings/{UserId}")]
        public ViewResult Setting(int UserId)
        {
            System.Console.WriteLine(UserId);
            System.Console.WriteLine("!!!!");
            System.Console.WriteLine(UserId);
            User retrievedUser = dbContext.users.FirstOrDefault(x => x.UserId == UserId);
            return View();
        }

        // [HttpPost]
        // [Route("Settings/{UserId}")]
        // public IActionResult updateProfile(int UserId, User user) {
        
        // string filename = Path.Combine(he.WebRootPath,user.ImageFile.FileName);
        // user.ImageFile.CopyTo(new FileStream(filename, FileMode.Create));
        // string filepath = "/" + Path.GetFileName(user.ImageFile.FileName);

        // System.Console.WriteLine(UserId);
        // System.Console.WriteLine("!!!!!!");
        

        // User retrievedUser = dbContext.users.FirstOrDefault(x => x.UserId == UserId);
        // retrievedUser.FirstName = user.FirstName;
        // retrievedUser.LastName = user.LastName;
        // retrievedUser.ProfileUrl = user.ProfileUrl;
        // dbContext.SaveChanges();

        // return View("profile");

        // }

    
    
    }

    

}
