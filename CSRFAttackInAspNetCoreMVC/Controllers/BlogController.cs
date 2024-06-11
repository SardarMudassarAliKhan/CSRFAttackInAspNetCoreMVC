using CSRFAttackInAspNetCoreMVC.Data;
using CSRFAttackInAspNetCoreMVC.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;

namespace CSRFAttackInAspNetCoreMVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var posts = _context.BlogPosts.ToList();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogPost model)
        {
            if (ModelState.IsValid)
            {
                // Sanitize inputs before saving to the database
                model.Title = Sanitizer.GetSafeHtmlFragment(model.Title);
                model.Content = Sanitizer.GetSafeHtmlFragment(model.Content);

                _context.BlogPosts.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
}
