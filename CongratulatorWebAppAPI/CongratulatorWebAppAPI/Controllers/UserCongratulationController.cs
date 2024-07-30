using CongratulatorWebAppAPI.BuisnesObjects;
using CongratulatorWebAppAPI.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorWebAppAPI.Controllers
{
    public class UserCongratulationController : Controller
    {
        #region Fields

        private readonly CongratulationDbContext _context;

        #endregion

        #region Constructors

        public UserCongratulationController()
        {
            _context = new CongratulationDbContext();
        }

        public UserCongratulationController(CongratulationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Async CRUD operations

        /// <summary>
        /// Асинхронное добавление
        /// </summary>
        public async Task<IActionResult> CreateAsync([Bind("Id, Message, Type")] UserCongratulation userCongratulation)
        {
            if (ModelState.IsValid)
            {
                //_context.Database.EnsureCreated();
                userCongratulation.Id = Guid.NewGuid();
                _context.Add(userCongratulation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCongratulation);
        }

        /// <summary>
        /// Асинхронное получение списка
        /// </summary>
        public async Task<IActionResult> GetUserListAsync()
        {
            return _context.UserCongratulations != null ?
                        View(await _context.UserCongratulations.ToListAsync()) :
                        //View(await _context.Users.ToList().Select(s => s.ToString()).ToList()) :
                        Problem("Entity set 'CongratulationDbContext.UserCongratulations'  is null.");
        }

        /// <summary>
        /// Асинхронное получение по Id
        /// </summary>
        public async Task<IActionResult> GetUserDetailsAsync(Guid? id)
        {
            if (id == null || !_context.UserCongratulations.Any())
            {
                return NotFound();
            }

            var user = await _context.UserCongratulations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Асинхронное изменение
        /// </summary>
        public async Task<IActionResult> UpdateAsync(Guid id, [Bind("Id, Message, Type")] UserCongratulation userCongratulation)
        {
            if (id != userCongratulation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCongratulation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userCongratulation.Id))
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
            return View(userCongratulation);
        }

        /// <summary>
        /// Асинхронное удаление по Id
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.UserCongratulations == null)
            {
                return NotFound();
            }

            var userCongratulation = await _context.UserCongratulations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCongratulation == null)
            {
                return NotFound();
            }

            return View(userCongratulation);
        }

        #endregion

        #region Sync CRUD operations

        /// <summary>
        /// Синхронное добавление
        /// </summary>
        public IActionResult Create([Bind("Id, Message, Type")] UserCongratulation userCongratulation)
        {
            if (ModelState.IsValid)
            {
                _context.Database.EnsureCreated();
                userCongratulation.Id = Guid.NewGuid();
                _context.Add(userCongratulation);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(userCongratulation);
        }

        /// <summary>
        /// Синхронное получение списка
        /// </summary>
        public IEnumerable<UserCongratulation> GetUserList()
        {
            //return _context.Users != null && _context.Users.Any() ? _context.Users.Select(s => JsonSerializer.Serialize(s)).ToList() : new List<User>() { };
            return _context.UserCongratulations != null && _context.UserCongratulations.Any() ? _context.UserCongratulations.ToList() : new List<UserCongratulation>() { };
        }

        /// <summary>
        /// Cинхронное получение по Id
        /// </summary>
        public UserCongratulation GetUserDetails(Guid? id)
        {
            if (id == null || !_context.Users.Any())
            {
                return new UserCongratulation();
            }

            var userCongratulation = _context.UserCongratulations.FirstOrDefault(m => m.Id == id);

            if (userCongratulation == null)
            {
                return new UserCongratulation();
            }

            return userCongratulation;
        }

        /// <summary>
        /// Удаление по Id
        /// </summary>
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.UserCongratulations == null)
            {
                return Problem("Entity set 'CongratulationDbContext.Users'  is null.");
            }
            var userCongratulation = await _context.UserCongratulations.FindAsync(id);
            if (userCongratulation != null)
            {
                _context.UserCongratulations.Remove(userCongratulation);
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
            return (_context.UserCongratulations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion
    }
}
