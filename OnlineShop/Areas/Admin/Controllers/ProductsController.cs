using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public ProductsController(ApplicationDbContext db,IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag).ToList());
        }
        //Get Create Method
        public IActionResult Create()
        {
            ViewData["ProductType"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagList"] = new SelectList(_db.SpecialTags.ToList(), "Id", "TagName");
            return View();
        }
        //Post Create Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == products.Name);
                if (searchProduct!=null)
                {
                    ViewBag.Message = "Product name already exists";
                    ViewData["ProductType"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagList"] = new SelectList(_db.SpecialTags.ToList(), "Id", "TagName");
                    return View(products);
                }
                if (image!=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    products.Image = "Images/No-image-found.jpg";
                }
                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                TempData["saved"] = "Product has been updated.";
                return RedirectToAction(nameof(Index));
            }

            return View(products);
        }
        //Edit Get Action Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var products = _db.Products.Include(c=>c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c=>c.Id==id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ProductType"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagList"] = new SelectList(_db.SpecialTags.ToList(), "Id", "TagName");

            return View(products);
        }
        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/img", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    products.Image = "Images/No-image-found.jpg";
                }
                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                TempData["saved"] = "Product has been saved.";
                return RedirectToAction(nameof(Index));
            }

            return View(products);
        }
        //Details Get Action Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        //Delete Get Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Products products)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != products.Id)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                _db.Remove(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
        [HttpPost]
        public IActionResult IsProductNameExists(string productName)
        {
            return Json(_db.Products.Any(x => x.Name == productName));

            //return Json(_db.Products.Any(x => x.Name == productName) ? "true" : string.Format("an account for address {0} already exists.", productName));

        }
    }
}