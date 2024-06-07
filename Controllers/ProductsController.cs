using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cstore.Data;
using cstore.Models;
using cstore.ViewModels;

using cstore.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace cstore.Controllers
{


    public class ProductsController : Controller
    {
        private readonly cstoreContext _context;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;

        public ProductsController(cstoreContext context, IBufferedFileUploadService bufferedFileUploadService)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchcategory,string searchprice)
        {
            IQueryable<Product> products = _context.Product.AsQueryable();
           
            if (!string.IsNullOrEmpty(searchcategory))
            {
                products = products.Where(s => s.Name.Contains(searchcategory));
            }
            if (!string.IsNullOrEmpty(searchprice))
            {
                products = products.Where(s => s.Size.ToString().Contains(searchprice));
            }


            //products = products.Include(m => m.Name).Include(m => m.Brand);
            var productVM = new ProductCategoryViewModel {
                Products = await products.ToListAsync()
            };
        
            //var cstoreContext = _context.Product.Include(p => p.Brand);
            return View(productVM);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Id");
            //return View();

            var brands = _context.Brand.AsEnumerable();

            CreateProductViewModel model = new CreateProductViewModel();

            model.BrandList = new SelectList(brands, "Id", "Name");

            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductViewModel viewModel, IFormFile downloadUrl, IFormFile ImageURL)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(product);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Id", product.BrandId);
            //return View(product);

            Product product = new Product();

           
                product.Name=viewModel.Name;
                product.Price= viewModel.Price; 
                product.Description=viewModel.Description;  
                product.Color= viewModel.Color; 
                product.Size= viewModel.Size;
                product.IsAvailable= viewModel.IsAvailable;
                product.BrandId = viewModel.SelectedBrandId;
       

               

                await _bufferedFileUploadService.UploadFile(downloadUrl);
                product.DownloadUrl = "images/" + downloadUrl.FileName;

                await _bufferedFileUploadService.UploadFile(ImageURL);
                product.ImageURL = "images/" + ImageURL.FileName;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            

            return View();


        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new EditProductViewModel
            {
                Color = product.Color,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Size = product.Size,
                IsAvailable = product.IsAvailable,
                SelectedBrandId = product.BrandId,
                BrandList = new SelectList(_context.Brand, "Id", "Name", product.BrandId),
               

            };
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Id", product.BrandId);
            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int id, EditProductViewModel viewModel)
        //{
        //    if (id != viewModel.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Id", product.BrandId);
        //    return View(product);
        //}
        public async Task<IActionResult> Edit(int id, EditProductViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = await _context.Product.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                product.Name = viewModel.Name;
                // Update other properties...
                product.Price= viewModel.Price;
                product.Color= viewModel.Color;
                product.Size= viewModel.Size;
                product.IsAvailable= viewModel.IsAvailable;
                product.Description= viewModel.Description;
                product.BrandId = viewModel.SelectedBrandId;


                if (viewModel.NewDownloadUrl != null)
                {
                    await _bufferedFileUploadService.UploadFile(viewModel.NewDownloadUrl);
                    product.DownloadUrl = "/images/" + viewModel.NewDownloadUrl.FileName;
                }

                if (viewModel.NewImageURL != null)
                {
                    await _bufferedFileUploadService.UploadFile(viewModel.NewImageURL);
                    product.ImageURL = "/images/" + viewModel.NewImageURL.FileName;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Load BrandList if needed...

            return View(viewModel);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'cstoreContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
