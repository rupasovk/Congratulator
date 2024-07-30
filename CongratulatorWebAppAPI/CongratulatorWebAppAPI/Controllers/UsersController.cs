using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CongratulatorWebAppAPI.DataBase;
using CongratulatorWebAppAPI.BuisnesObjects;

namespace CongratulatorWebAppAPI.Controllers
{
    public class UsersController : Controller
    {
        #region Fields

        private readonly CongratulationDbContext _context;

        #endregion

        #region Constructors

        public UsersController()
        {
            _context = new CongratulationDbContext();
        }
        
        public UsersController(CongratulationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Async CRUD operations

        /// <summary>
        /// Асинхронное добавление пользователя
        /// </summary>
        public async Task<IActionResult> CreateAsync([Bind("Id,Name,SurName,BirthDay,Role,Email,Country")] User user)
        {
            if (ModelState.IsValid)
            {
                //_context.Database.EnsureCreated();
                user.Id = Guid.NewGuid();
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        /// <summary>
        /// Асинхронное получение списка всех пользователей
        /// </summary>
        public async Task<IActionResult> GetUserListAsync()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        //View(await _context.Users.ToList().Select(s => s.ToString()).ToList()) :
                        Problem("Entity set 'CongratulationDbContext.Users'  is null.");
        }

        /// <summary>
        /// Асинхронное получение пользователя по Id
        /// </summary>
        public async Task<IActionResult> GetUserDetailsAsync(Guid? id)
        {
            if (id == null || !_context.Users.Any())
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Асинхронное изменение пользователя
        /// </summary>
        public async Task<IActionResult> UpdateAsync(Guid id, [Bind("Id,Name,SurName,BirthDay,Role,Email,Country")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
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
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
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

        /// <summary>
        /// Асинхронное удаление пользователя по Id
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        #endregion

        #region Sync CRUD operations

        /// <summary>
        /// Синхронное добавление пользователя
        /// </summary>
        public IActionResult Create([Bind("Id,Name,SurName,BirthDay,Role,Email,Country")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Database.EnsureCreated();
                user.Id = Guid.NewGuid();
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        /// <summary>
        /// Синхронное получение списка всех пользователей
        /// </summary>
        public IEnumerable<User> GetUserList()
        {
            //return _context.Users != null && _context.Users.Any() ? _context.Users.Select(s => JsonSerializer.Serialize(s)).ToList() : new List<User>() { };
            return _context.Users != null && _context.Users.Any() ? _context.Users.Include(u => u.UserImage).ToList() : new List<User>() { };
        }

        /// <summary>
        /// Cинхронное получение пользователя по Id
        /// </summary>
        public User GetUserDetails(Guid? id)
        {
            if (id == null || !_context.Users.Any())
            {
                return new User();
            }

            var user = _context.Users.Include(u => u.UserImage).FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return new User();
            }

            return user;
        }

        /// <summary>
        /// Удаление пользователя по Id
        /// </summary>
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

        #endregion

        /// <summary>
        /// Проверка существования пользователя
        /// </summary>
        private bool UserExists(Guid id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region Birthday operations

        /// <summary>
        /// Синхронное получение списка первых n пользователей с ближайшими днями рождения
        /// </summary>
        public IEnumerable<User> GetNearBirthdaysList(int n)
        {
            if (_context.Users == null || !_context.Users.Any())
                return new List<User>();

            var today = DateTime.Today;
            var users = _context.Users.Include(u => u.UserImage)
                                      .ToList()
                                      .Select(u =>
                                      {
                                          var nextBirthday = new DateTime(today.Year, u.BirthDay.Month, u.BirthDay.Day);
                                          if (nextBirthday < today)
                                              nextBirthday = nextBirthday.AddYears(1);
                                          return new { User = u, DaysTillBirthday = (nextBirthday - today).Days };
                                      })
                                      .OrderBy(x => x.DaysTillBirthday)
                                      .Take(n)
                                      .Select(x => x.User)
                                      .ToList();

            return users;
        }
        
        /// <summary>
        /// Синхронное получение списка пользователей-именинников
        /// </summary>
        public IEnumerable<User> GetTodayBirthdaysList()
        {
            if (_context.Users == null || !_context.Users.Any())
                return new List<User>();

            var today = DateTime.Today;
            var users = _context.Users.Include(u => u.UserImage).ToList()
                .Where(w => w.BirthDay.Month == today.Month && w.BirthDay.Day == today.Day)
                .OrderBy(x => x.BirthDay)
                .ToList();

            return users;
        }

        #endregion

        #endregion
    }
}
