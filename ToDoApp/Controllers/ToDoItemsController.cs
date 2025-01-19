using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ToDoContext _context;

        public ToDoItemsController(ToDoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoItems.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

      

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ToDoItem toDoItem)
        {
            if (id != toDoItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ToDoItems.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }
        

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null) return NotFound();

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            // Toggle the completion status
            toDoItem.IsComplete = !toDoItem.IsComplete;

            _context.Update(toDoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
