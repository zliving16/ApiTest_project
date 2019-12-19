using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace ApiTest.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context){
            dbContext = context;
        }
            // [Authorize]
        // public IActionResult Index(){

        //     return View();
        // }
        
        [Authorize]
        [HttpGet("/")]
        public IActionResult Success(){
            
            // Get;
            Chilkat.PublicKey pubKey = new Chilkat.PublicKey();
            bool success = pubKey.LoadFromFile("qa_data/pem/rsa_public.pem");

            Chilkat.Jwt jwt = new Chilkat.Jwt();

            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            //  First verify the signature.
            bool sigVerified = jwt.VerifyJwtPk(token,pubKey);
            Console.WriteLine("verified: " + Convert.ToString(sigVerified));

            int leeway = 60;
            bool bTimeValid = jwt.IsTimeValid(token,leeway);
            Console.WriteLine("time constraints valid: " + Convert.ToString(bTimeValid));

            //  Now let's recover the original claims JSON (the payload).
            string payload = jwt.GetPayload(token);
            //  The payload will likely be in compact form:
            Console.WriteLine(payload);

            //  We can format for human viewing by loading it into Chilkat's JSON object
            //  and emit.
            Chilkat.JsonObject json = new Chilkat.JsonObject();
            success = json.Load(payload);
            json.EmitCompact = false;
            Console.WriteLine(json.Emit());

            //  We can recover the original JOSE header in the same way:
            string joseHeader = jwt.GetHeader(token);
            //  The payload will likely be in compact form:
            Console.WriteLine(joseHeader);

            //  We can format for human viewing by loading it into Chilkat's JSON object
            //  and emit.
            success = json.Load(joseHeader);
            json.EmitCompact = false;
            Console.WriteLine(json.Emit());
// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // JwtDecode Test1=new JwtDecode();
            // var x=Test1.GetName("eyJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJJU1MiLCJzY29wZSI6Imh0dHBzOi8vbGFyaW0uZG5zY2UuZG91YW5lL2NpZWxzZXJ2aWNlL3dzIiwiYXVkIjoiaHR0cHM6Ly9kb3VhbmUuZmluYW5jZXMuZ291di5mci9vYXV0aDIvdjEiLCJpYXQiOiJcL0RhdGUoMTQ2ODM2MjU5Mzc4NClcLyJ9");
            // // var x= Test1.TestJwtSecurityTokenHandler("CfDJ8JJa5feBk79Hq8LMUg4HHXfoWI4CgZu7vOhujmhfwtEi7rYOFiVzoUBGec1HXm2aOD69Q8AEqARSHvCmyUJAw_opdjsSfIhJS3v-Dbe0MsLw8QvdMMOuNeqEZvB93lH03TM62plyHreR5_D_G7kkvJvg5vaMUOd_GgZMRFkLMlrrcrPM8l2jOVOgD1r4WIEr0oCm9KB_T0Bt5vZ37CnPJJt7r6_yVM5yuAZU3aI92S5EYodHHhVe_OjRDqg1nALC2a2KzHbGnBKfO-7FbyocHU0QRdkl5F5VBRJVsHHNMOZJ3jUhtfflSggP_b5Imk0qNcs39rEUUC8ajLUv3zaFxaCTX6yyj-kqYg2JKYFSw7OXQZl_XiNlB2mY9cdb1xjjpbLM4SxWzB8k1rMMkXETK1ZLFpu9DUiDgoKmTqICaduGdETRjvBsVA4fa2H2ztmNWCEL7huxL41rXjYb4jXSosJ6wLpjFp2j9f1oLymvPVKeeZ_aKaxFHIJF_dUnG-nIuPUtCSAexfp4NLiWaR3ctWvQ4NKeRv-UwaBoSrSt_gSJ3QAqafrmu-vTdwIek6xPb9AWTUAC7TWCgIHgz3pQqJKnPteVWgXXsPSizg2FlcWc5kYTa0PV2kf8g7JYKHPbYPkbjDKUAiYoOG4AkFdV5z6febxVZiC4XQZLM3tZHCBUdTaFHJOwUSOX8aWna-BJBA");
            // // var x= Test1.TestJwtSecurityTokenHandler();
            // System.Console.WriteLine(x);
            // var jwt = "eyJhbGciOiJQUzM4NCIsImtpZCI6ImJpbGJvLmJhZ2dpbnNAaG9iYml0b24uZXhhbXBsZSJ9.SXTigJlzIGEgZGFuZ2Vyb3VzIGJ1c2luZXNzLCBGcm9kbywgZ29pbmcgb3V0IHlvdXIgZG9vci4gWW91IHN0ZXAgb250byB0aGUgcm9hZCwgYW5kIGlmIHlvdSBkb24ndCBrZWVwIHlvdXIgZmVldCwgdGhlcmXigJlzIG5vIGtub3dpbmcgd2hlcmUgeW91IG1pZ2h0IGJlIHN3ZXB0IG9mZiB0by4.cu22eBqkYDKgIlTpzDXGvaFfz6WGoz7fUDcfT0kkOy42miAh2qyBzk1xEsnk2IpN6-tPid6VrklHkqsGqDqHCdP6O8TTB5dDDItllVo6_1OLPpcbUrhiUSMxbbXUvdvWXzg-UD8biiReQFlfz28zGWVsdiNAUf8ZnyPEgVFn442ZdNqiVJRmBqrYRXe8P_ijQ7p8Vdz0TTrxUeT3lm8d9shnr2lfJT8ImUjvAA2Xez2Mlp8cBE5awDzT0qI0n6uiP1aCN_2_jLAeQTlqRHtfa64QQSUmFAAjVKPbByi7xho0uTOcbH510a6GYmJUAfmWjwZ6oD4ifKo8DYM-X72Eaw";
            // var handler = new JwtSecurityTokenHandler();
            // var token = handler.ReadJwtToken(jwt);
            // System.Console.WriteLine(token);

            System.Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            // int? y=HttpContext.Session.GetInt32("userid");
            CryptoEngine Encrypter=new CryptoEngine();
            // if (y==null){
            //     return RedirectToAction("Index");
            // }
            // bool Exists=dbContext.users.Any(e=>e.UserId==(int)y);
            // if(Exists==false){
            //     return RedirectToAction("Index");
            // }
            // ViewBag.UserId=(int)y;
            ViewBag.UserId=5;

            List<Photos> Allphoto=dbContext.photos.ToList();
            foreach(var photo in Allphoto){
                photo.Desc=Encrypter.Decrypt(photo.Desc);
                photo.PhotoPath=Encrypter.Decrypt(photo.PhotoPath);
            }
            ViewBag.AllPhotos=Allphoto;
            return View();
        }

        
        [Authorize]
        [HttpPost("addphoto")]
        public IActionResult AddPhoto(Photos NewPhoto){
            // int? y=HttpContext.Session.GetInt32("userid");
            // if (y==null){
            //     return RedirectToAction("Index");
            // }
            // bool Exists=dbContext.users.Any(e=>e.UserId==(int)y);
            // if(Exists==false){
            //     return RedirectToAction("Index");
            // }
            if(ModelState.IsValid){
                CryptoEngine Encrypter=new CryptoEngine();
                NewPhoto.Desc=Encrypter.Encrypt(NewPhoto.Desc);
                NewPhoto.PhotoPath =Encrypter.Encrypt(NewPhoto.PhotoPath);
                // NewPhoto.CreatorId=Encrypter.Encrypt((string)NewPhoto.CreatorId);
                dbContext.photos.Add(NewPhoto);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            else{
                // ViewBag.UserId=(int)y;
                ViewBag.UserId=5;

                List<Photos> Allphoto=dbContext.photos.ToList();
                foreach(var photo in Allphoto){
                    CryptoEngine Encrypter=new CryptoEngine();
                    photo.Desc=Encrypter.Decrypt(photo.Desc);
                    photo.PhotoPath=Encrypter.Decrypt(photo.PhotoPath);
                }
                ViewBag.AllPhotos=Allphoto;
                return View("Success");
                }
                    
        }
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");

        }

        [HttpPost("login")]
        public IActionResult Login(UsersWrapper user){
            
            LoginUsers SubUser=user.LoginUser;
            CryptoEngine Encrypter=new CryptoEngine();
            if(ModelState.IsValid)
            {
                SubUser.Email=Encrypter.Encrypt(SubUser.Email);
                Users userInDb = dbContext.users.FirstOrDefault(u => u.Email == SubUser.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginUser.Email", "Invalid Email");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUsers>();
                var result = hasher.VerifyHashedPassword(SubUser, userInDb.Password, SubUser.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginUser.Password", "Password is wrong");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userid",userInDb.UserId);
                return RedirectToAction("Success");
                // SubUser.Email=Users.Encrypt(SubUser.Email);
                // List<Users> AllUsers=dbContext.users.ToList();
                // foreach(Users item in AllUsers){
                //     item.Email=Encrypter.Decrypt(item.Email);
                //     if(item.Email==user.LoginUser.Email){
                //         Users userInDb = dbContext.users.FirstOrDefault(u => u.UserId == item.UserId);
                //         HttpContext.Session.SetInt32("userid",userInDb.UserId);
                //         var hasher = new PasswordHasher<LoginUsers>();
                //         var result = hasher.VerifyHashedPassword(SubUser, userInDb.Password, SubUser.Password);
                //         if(result == 0){
                //             ModelState.AddModelError("LoginUser.Password", "Password is wrong");
                //             return View("Index");
                //         }
                //         return RedirectToAction("Success");
                //     }
                // }
                // ModelState.AddModelError("LoginUser.Email", "Invalid Email");
                // return View("Index");
                // return
                // if(userInDb == null)
                // {
                // }

                
            }
            else{
                return View("Index");
            }
        }
            
        [HttpPost("register")]
        public IActionResult Register(UsersWrapper user){
            Users SubUser=user.NewUser;
            CryptoEngine Encrypter=new CryptoEngine();
            if( ModelState.IsValid){
                bool ExUser=dbContext.users.Any(t=>t.Email==SubUser.Email);
                if(ExUser==true){
                    ModelState.AddModelError("NewUser.Email","email already exists");
                    return View("Index");
                }
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                SubUser.Password = Hasher.HashPassword(SubUser, SubUser.Password);
                SubUser.Email=Encrypter.Encrypt(SubUser.Email);
                SubUser.UserName=Encrypter.Encrypt(SubUser.UserName);
                dbContext.users.Add(SubUser);
                dbContext.SaveChanges();
                Users CurUser=dbContext.users.Last();
                HttpContext.Session.SetInt32("userid",CurUser.UserId);
                return RedirectToAction("Success");
            }
            else{
                return View("Index");
            }          
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
