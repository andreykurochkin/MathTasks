using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MathTasks.Data;
using MathTasks.Models;
using MathTasks.ViewModels;
using Amazon.S3;

namespace MathTasks.Controllers
{
    public class MathTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAmazonS3 _amazonS3Client;

        public MathTasksController(ApplicationDbContext context, IAmazonS3 amazonS3Client)
        {
            _context = context;
            _amazonS3Client = amazonS3Client;
        }

        // GET: MathTasks
        public async Task<IActionResult> Index()
        {
            //var testController = new AwsController(_amazonS3Client);
            ////await testController.CreateFolder("mathtasksbucket", "Monday", "demo");
            //await testController.UploadFile(@"c:\notes\nick-chapsas-refactoring.md", prefix: "Demo");
            return View(await _context.MathTasks.ToListAsync());
        }

        // GET: MathTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mathTask = await _context.MathTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mathTask == null)
            {
                return NotFound();
            }

            return View(mathTask);
        }

        // GET: MathTasks/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Topic = CreateSelectListWithData(await GetTopics());
            return View();
        }

        private async Task<IEnumerable<Topic>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        private SelectList CreateSelectListWithData(IEnumerable<Topic> topics)
        {
            var selectList = new SelectList(
                items: topics,
                dataValueField: nameof(Topic.Id),
                dataTextField: nameof(Topic.Content));
            return selectList;
        }

        // POST: MathTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content,TopicId")] MathTask mathTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mathTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mathTask);
        }

        private async Task<Topic> GetTopicById(int id)
        {
            var topics = await GetTopics();
            return topics.FirstOrDefault(t => t.Id.Equals(id));
        }

        // GET: MathTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mathTask = await _context.MathTasks.FindAsync(id);
            if (mathTask == null)
            {
                return NotFound();
            }
            return View(mathTask);
        }

        // POST: MathTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Theme,Subject")] MathTask mathTask)
        {
            if (id != mathTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mathTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MathTaskExists(mathTask.Id))
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
            return View(mathTask);
        }

        // GET: MathTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mathTask = await _context.MathTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mathTask == null)
            {
                return NotFound();
            }

            return View(mathTask);
        }

        // POST: MathTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mathTask = await _context.MathTasks.FindAsync(id);
            _context.MathTasks.Remove(mathTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MathTaskExists(int id)
        {
            return _context.MathTasks.Any(e => e.Id == id);
        }

        // GET: MathTasks/ShowSearchForm
        [HttpGet]
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // POST: MathTasks/ShowSearchResults
        [HttpPost]
        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            return View("Index", await _context.MathTasks
                // todo change to full text search
                .Where(mt => mt.Name.Contains(searchPhrase) || mt.Topic.Content.Contains(searchPhrase) || mt.Content.Contains(searchPhrase))
                .ToListAsync());
        }
    }
}
