
using Assessment.Data;
using Assessment.Services;
using Assessment.Services.Filters;
using Assessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SynicTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Assessment.Web.Controllers
{
    public class BlogController : SynicController
    {
        private readonly IBlogService BlogService;

        public BlogController(IBlogService blogService)
        {
            this.BlogService = blogService;
        }

        public IActionResult Index()
        {
            var blogs = BlogService.Get().ToList();

            return View(blogs);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        //Create Method
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Blog());
        }

        [HttpPost]
        public IActionResult Create(Blog model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blog = new Blog
            {
                Title = model.Title,
                Body = model.Body,
                Author = model.Author

            };

            BlogService.CreateUpdate(blog);

            return RedirectToAction("Index");
        }

        //Edit Method
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var blog = BlogService.GetById(id);

            var model = new Blog
            {
                Id = blog.Object.Id,
                Title = blog.Object.Title,
                Body = blog.Object.Body,
                Author = blog.Object.Author
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Blog model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blog = BlogService.GetById(model.Id);
            blog.Object.Title = model.Title;
            blog.Object.Body = model.Body;
            blog.Object.Author = model.Author;

            BlogService.CreateUpdate(blog.Object);

            return RedirectToAction("Index");
        }

        //Delete Method
        [HttpPost]
        public IActionResult Delete(int id)
        {
            BlogService.DeleteById(id);
            return RedirectToAction("Index");
        }
    }
}
