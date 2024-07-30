using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CongratulatorWebAppAPI.DataBase;
using CongratulatorWebAppAPI.BuisnesObjects;

namespace CongratulatorWebAppAPI.Controllers
{
    public class UserImageController : Controller
    {
        private readonly CongratulationDbContext _context;

        public UserImageController()
        {
            _context = new CongratulationDbContext();
        }

        public UserImageController(CongratulationDbContext context)
        {
            _context = context;
        }

        public User GetUserImageById(Guid? id)
        {
            if (id == null || !_context.Users.Any())
            {
                return new User();
            }

            var user = _context.Users.FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return new User();
            }

            return user;
        }

        public async Task<IActionResult> CreateAsync(UserImage userImage, User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userImage);
        }

        public IActionResult Create(UserImage userImage)
        {
            if (ModelState.IsValid)
            {
                _context.Database.EnsureCreated();
                userImage.Id = Guid.NewGuid();
                _context.Add(userImage);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(userImage);
        }

        public async Task<IActionResult> UpdateAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'CongratulationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserImageExists(Guid id)
        {
            return false;
            //return (_context.UserImages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
