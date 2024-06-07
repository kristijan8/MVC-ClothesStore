using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cstore.Data;
using cstore.Models;
using Microsoft.AspNetCore.Identity;
using cstore.Areas.Identity.Data;

namespace cstore.Controllers
{
    public class UsersController : Controller
    {
        private readonly cstoreContext _context;
        private readonly UserManager<cstoreUser> _userManager;


        public UsersController(cstoreContext context, UserManager<cstoreUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;

        }
        private Task<cstoreUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var cstoreContext = _context.Users.Include(u => u.Product);
            return cstoreContext != null ?
                         View(await cstoreContext.ToListAsync()) :
                         Problem("Entity set 'MVCBookStoreContext.UserBook'  is null.");
        }

        // GET: Users/Details/5
         [Authorize(Roles = "User")]
        public async Task<IActionResult> AddProductBought(int? productid)
        {
            if (productid == null)
            {
                return NotFound();
            }
            var cstoreUserContext = _context.Users.Where(r => r.ProductId == productid).Include(p => p.Product).ThenInclude(p => p.Brand);
            var user = await GetCurrentUserAsync();

            if (ModelState.IsValid)
            {
                Users userproduct = new Users();
                userproduct.ProductId = (int)productid;
                userproduct.AppUser = user.UserName;
                _context.Users.Add(userproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyProductsList));
            }
            return cstoreUserContext != null ?
                          View(await cstoreUserContext.ToListAsync()) :
                          Problem("Entity set 'cstoreContext.Users'  is null.");
        }
         [Authorize(Roles = "User")]
        public async Task<IActionResult> MyProductsList()
        {
            var user = await GetCurrentUserAsync();
            var MVCUserProductContext = _context.Users.AsQueryable().Where(r => r.AppUser == user.UserName).Include(r => r.Product).ThenInclude(p => p.Brand);
            var products_ofcurrentuser = _context.Product.AsQueryable(); ;
            products_ofcurrentuser = MVCUserProductContext.Select(p => p.Product);
            return MVCUserProductContext != null ?
                          View("~/Views/Users/KupeniProdukti.cshtml", await products_ofcurrentuser.ToListAsync()) :
                          Problem("Entity set 'MVCBookStoreContext.UserBook'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,ProductId")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", users.ProductId);
            return View(users);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", users.ProductId);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,AppUser,ProductId")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", users.ProductId);
            return View(users);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOwnedProduct(int? productid)
        {
            if (productid == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var userProduct = await _context.Users.Include(p => p.Product).AsQueryable().FirstOrDefaultAsync(m => m.AppUser == user.UserName && m.ProductId == productid);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View("~/Views/Users/Delete.cshtml", userProduct);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'MVCBookStoreContext.UserBook'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int? id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
