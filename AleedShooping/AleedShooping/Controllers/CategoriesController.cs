using AleedShooping.Data;
using AleedShooping.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AleedShooping.Controllers
{
    public class CategoriesController:Controller
    {
      
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public CategoriesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una categoria con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(string.Empty, exception.Message);
                }

            }
            return View(category);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category catregory)
        {
            if (id != catregory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catregory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una categoria con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(string.Empty, exception.Message);
                }
            }
            return View(catregory);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories
                .FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories
                .FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro Borrado Exitosamente");
            return RedirectToAction(nameof(Index));
        }
    }
}
