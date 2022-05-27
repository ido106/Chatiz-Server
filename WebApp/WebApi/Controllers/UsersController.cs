using System;
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

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UsersController : ControllerBase
    {
        //private readonly WebAppContext _context;
        private UserService _service;
        private IConfiguration _config;

        public UsersController(UserService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }



        [HttpPost("signIn")]

        public async Task<IActionResult> SignIn(string username, string password)
        {
            if (await _service.Get(username) != null && (await _service.Get(username)).Password.Equals(password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["JWTParams:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", username)
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTParams:SecretKey"]));
                var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        _config["JWTParams:Issuer"],
                        _config["JWTParams:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: mac);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username, Nickname, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _service.Get(user.Username);
                // user is already exist
                if (q != null) return BadRequest();

                user.LastSeen = DateTime.Now;
                user.Server = "https://localhost:7092";

                _service.Add(user);
            }
            return BadRequest();
        }
    }

    // GET: Users/Details/5
    [Authorize]
    public async Task<IActionResult> Details(string id)
    {
        if (id == null || _context.User == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var user = await _context.User
            .FirstOrDefaultAsync(m => m.Username == id);
        if (user == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return View(user);
    }

    // GET: Users/Create

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    /**
    // POST: Users/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Username, Nickname, Password")] User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }
    **/

    // GET: Users/Edit/5

    [Authorize]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.User == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var user = await _context.User.FindAsync(id);
        if (user == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        return View(user);
    }

    // POST: Users/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]

    [Authorize]
    public async Task<IActionResult> Edit(string id, [Bind("Username,Nickname,Password")] User user)
    {
        if (id != user.Username)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Username))
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }


    // GET: Users/Delete/5
    /**
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.User == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var user = await _context.User
            .FirstOrDefaultAsync(m => m.Username == id);
        if (user == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return View(user);
    }
    **/

    // POST: Users/Delete/5
    /**
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.User == null)
        {
            return Problem("Entity set 'WebAppContext.User'  is null.");
        }
        var user = await _context.User.FindAsync(id);
        if (user != null)
        {
            _context.User.Remove(user);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    **/
}

