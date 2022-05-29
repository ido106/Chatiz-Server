﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Repository;
using Microsoft.AspNetCore.Authorization;
using Services;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UsersController : ControllerBase
    {
        //private readonly WebAppContext _context;
        private IUserService _service;
        private IConfiguration _config;

        public UsersController(IUserService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }


        // GET: Users
        [HttpGet("contacts")]
        [Authorize]

        public async Task<IActionResult> GetContacts()
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null) return NotFound();
            if (await _service.Get(username) == null) return NotFound();

            return Ok(await _service.GetContacts(username));
        }

        [HttpPost("contacts")]
        [Authorize]

        public async Task<IActionResult> AddContact([FromBody] JsonElement json)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null || await _service.Get(username) == null) return NotFound();

            string contact_username = json.GetProperty("id").ToString();
            if (contact_username == null || await _service.Get(contact_username) == null) return NotFound();
            
            await _service.AddContact(username, contact_username);

            return Ok();
        }




        [HttpGet("contacts/{id}")]
        [Authorize]
        public async Task<IActionResult> ContactID(string contact)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null || await _service.Get(username) == null) return NotFound();

            if (contact == null) return NotFound();

            Contact c = await _service.GetContact(username, contact);

            if (c == null) return NotFound();

            return Ok(c);
        }

        [HttpPut("contacts/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeContactID(string contact, [FromBody] JsonElement json)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null || await _service.Get(username) == null) return NotFound();
            if (contact == null) return NotFound();
            Contact c = await _service.GetContact(username, contact);
            if (c == null) return NotFound();

            // TODO do i have to change directly the contact's user? (it will change to the original user also)
            await _service.ChangeContact(username, contact, json.GetProperty("name").ToString(), json.GetProperty("server").ToString());

            return Ok();
        }

        [HttpDelete("contacts/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteContactID(string contact)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null || await _service.Get(username) == null) return NotFound();
            if (contact == null || await _service.GetContact(username, contact) == null) return NotFound();
       

            await _service.DeleteContact(username, contact);

            return Ok();
        }

        [HttpGet("contacts/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> GetMessages(string contact)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null || await _service.Get(username) == null) return NotFound();

            if (contact == null || await _service.GetContact(username, contact)== null) return NotFound();


            return Ok(await _service.GetMessages(username, contact));
        }

        [HttpPost("contacts/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> AddMessage(string contact, [FromBody] JsonElement json)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null || await _service.Get(username) == null) return NotFound();

            if (contact == null || await _service.GetContact(username, contact) == null) return NotFound();

            await _service.AddMessage(username, contact, json.GetProperty("content").ToString());
            return Ok();
        }

        [HttpGet("contacts/{id}/messages/{id2}")]
        [Authorize]
        public async Task<IActionResult> GetMessageID(string contact, string id)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (id == null || username == null || await _service.Get(username) == null) return NotFound();

            if (contact == null || await _service.GetContact(username, contact) == null) return NotFound();

            if (! int.TryParse(id, out var id2)) return BadRequest();

            Message message = await _service.GetMessageID(username, contact, id2);
            if (message == null) return NotFound();

            return Ok(message);
        }

        [HttpPut("contacts/{id}/messages/{id2}")]
        [Authorize]
        public async Task<IActionResult> EditMessageID(string contact, string id, [FromBody] JsonElement json)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (id == null || username == null || await _service.Get(username) == null) return NotFound();

            if (contact == null || await _service.GetContact(username, contact) == null) return NotFound();

            if (!int.TryParse(id, out var id2)) return BadRequest();

            Message message = await _service.GetMessageID(username, contact, id2);
            if (message == null) return NotFound();

            await _service.UpdateMessage(contact, id2, username, json.GetProperty("content").ToString());

            return Ok();
        }

        [HttpDelete("contacts/{id}/messages/{id2}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessageID(string contact, string id)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (id == null || username == null || await _service.Get(username) == null) return NotFound();

            if (contact == null || await _service.GetContact(username, contact) == null) return NotFound();

            if (!int.TryParse(id, out var id2)) return BadRequest();

            Message message = await _service.GetMessageID(username, contact, id2);
            if (message == null) return NotFound();

           await _service.DeleteMessage(username, contact, id2);

            return Ok();
        }

        [HttpPost("invitations")]

        // *****************

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] JsonElement json)
        {
            string username = json.GetProperty("username").ToString();
            string password = json.GetProperty("password").ToString();
            if (await _service.Get(username) != null && (await _service.Get(username)).Password.Equals(password))
            {

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["JWTParams:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("username", username)
                };


                var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties() { ExpiresUtc = DateTime.UtcNow.AddDays(7) };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTParams:SecretKey"]));
                var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_config["JWTParams:Issuer"], _config["JWTParams:Audience"], claims, expires: DateTime.UtcNow.AddDays(7), signingCredentials: mac);
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });

                /*
                                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTParams:SecretKey"]));
                                var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                                var token = new JwtSecurityToken(
                                        _config["JWTParams:Issuer"],
                                        _config["JWTParams:Audience"],
                                        claims,
                                        expires: DateTime.UtcNow.AddMinutes(60),
                                        signingCredentials: mac);

                                return Ok(new JwtSecurityTokenHandler().WriteToken(token));*/

                /**
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("username", loginDetails.Username),
                };

                var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties() { ExpiresUtc = DateTime.UtcNow.AddDays(7) };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTParams:SecretKey"]));
                var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(configuration["JWTParams:Issuer"], configuration["JWTParams:Audience"], claims, expires: DateTime.UtcNow.AddDays(7), signingCredentials: mac);
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), user });
                **/
            }
            return BadRequest();
        }

        /*************************************************************************************************************************************************************************
         */

        //TODO: not sure why the function allways return status 500, try to updtae the db and it might work, the db didnt work for me.
        //if it doesnt help it might be a general error with the server
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] JsonElement json)
        {
            if (ModelState.IsValid)
            {
                //User user = new(username, nickName, password);
                User user = new();
                user.Username = json.GetProperty("username").ToString();
                user.Nickname = json.GetProperty("nickName").ToString();
                user.Password = json.GetProperty("password").ToString();

                var q = await _service.Get(user.Username);
                // user is already exist
                if (q != null) return BadRequest();

                user.LastSeen = DateTime.Now;
                user.Server = "https://localhost:7092";

                await _service.Add(user);
                return Ok();
            }
            return BadRequest();
        }

        // api/contacts/:id/messages

        /*[HttpGet("{id}/{messages}")]
        public async Task<IActionResult> GetContactMessagesAsync(string id, string messages)
        {
            string username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            if (username == null) return NotFound();
            if (messages == null || messages != "messages")
            {
                return NotFound();
            }
            string result = "";
            var allMessages = await _service.GetContactMsgs(username, id);
            if (allMessages == null)
            {
                return NotFound();
            }
            bool firstTime = true;
            result += "[";

            foreach (var message in allMessages)
            {
                if (!firstTime)
                {
                    result += ",";
                }
                result += "{";
                result += "\"id\":\"" + message.Id + "\",";
                result += "\"content\":\"" + message.Data + "\",";
                result += "\"created\":\"" + message.TimeSent.ToString("yyyy-MM-ddTHH:mm:ss.fffffff") + "\",";
                if (message.IsMine)
                {
                    result += "\"sent\":\"" + username.ToLower() + "\"";
                }
                else
                {
                    result += "\"sent\":\"" + id.ToLower() + "\"";
                }
                result += "}";
                firstTime = false;
            }
            result += "]";

            return Ok(result);
        }*/
    }
}


/*****************************************************************************************************************************************************************************************/


//    public async Task<IActionResult> Details(string id)
//    {

//    }

//    // GET: Users/Create

//    [Authorize]
//    public IActionResult Create()
//    {
//        return View();
//    }

//    /**
//    // POST: Users/Create
//    // To protect from overposting attacks, enable the specific properties you want to bind to.
//    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]        
//    [Authorize]

//    public async Task<IActionResult> Create([Bind("Username, Nickname, Password")] User user)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.Add(user);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//        return View(user);
//    }
//    **/

//    // GET: Users/Edit/5

//    [Authorize]
//    public async Task<IActionResult> Edit(string id)
//    {
//        if (id == null || _context.User == null)
//        {
//            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//        }

//        var user = await _context.User.FindAsync(id);
//        if (user == null)
//        {
//            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//        }
//        return View(user);
//    }

//    // POST: Users/Edit/5
//    // To protect from overposting attacks, enable the specific properties you want to bind to.
//    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]

//    [Authorize]
//    public async Task<IActionResult> Edit(string id, [Bind("Username,Nickname,Password")] User user)
//    {
//        if (id != user.Username)
//        {
//            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//        }

//        if (ModelState.IsValid)
//        {
//            try
//            {
//                _context.Update(user);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!UserExists(user.Username))
//                {
//                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction(nameof(Index));
//        }
//        return View(user);
//    }


//    // GET: Users/Delete/5
///**
//public async Task<IActionResult> Delete(string id)
//{
//    if (id == null || _context.User == null)
//    {
//        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//    }

//    var user = await _context.User
//        .FirstOrDefaultAsync(m => m.Username == id);
//    if (user == null)
//    {
//        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//    }

//    return View(user);
//}
//**/

//// POST: Users/Delete/5
///**
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> DeleteConfirmed(string id)
//{
//    if (_context.User == null)
//    {
//        return Problem("Entity set 'WebAppContext.User'  is null.");
//    }
//    var user = await _context.User.FindAsync(id);
//    if (user != null)
//    {
//        _context.User.Remove(user);
//    }

//    await _context.SaveChangesAsync();
//    return RedirectToAction(nameof(Index));
//}
//**/

