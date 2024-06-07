using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cstore.Data;
using cstore.Models;
using Microsoft.AspNetCore.Identity;
using cstore.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace cstore.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly cstoreContext _context;
        private readonly UserManager<cstoreUser> _userManager;


        public ReviewsController(cstoreContext context, UserManager<cstoreUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;

        }
        private Task<cstoreUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var cstoreContext = _context.Reviews.Include(r => r.Product);
            return View(await cstoreContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }
        public async Task<IActionResult> WriteReviewFromUserAsync(int? productid)
        {
            ViewData["ProductId"] = productid;
            var product = _context.Product.AsQueryable().Where(p => p.Id == productid).FirstOrDefault();
            ViewData["ProductName"] = product.Name;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WriteReviewFromUser([Bind("Id,AppUser,Comment,Rating,ProductId")] Reviews review)
        {
            cstoreUser user = await GetCurrentUserAsync();
            review.AppUser = user.UserName;
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = review.ProductId;
            return View(review);
        }
        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,AppUser,Comment,Rating")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", reviews.ProductId);
            return View(reviews);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", reviews.ProductId);
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,ProductId,AppUser,Comment,Rating")] Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewsExists(reviews.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", reviews.ProductId);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'cstoreContext.Reviews'  is null.");
            }
            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews != null)
            {
                _context.Reviews.Remove(reviews);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewsExists(int? id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
