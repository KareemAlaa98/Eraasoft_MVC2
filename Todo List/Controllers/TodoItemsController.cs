using Microsoft.AspNetCore.Mvc;
using Todo_List.Data;
using Todo_List.Models;

namespace Todo_List.Controllers
{
    public class TodoItemsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Create()
        {
            if(Request.Method == "POST")
            {
                return View("Items");
            }
            return View();
        }
        public IActionResult Items(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTimeOffset.Now.AddDays(1);
                Response.Cookies.Append("username", userName, option);
            }

            ViewData["UserName"] = userName;

            var todos = context.TodoItems.ToList();
            return View(todos);
        }

        // // Creating New To-do item
        public IActionResult CreateNew()
        {
            return View();
        }
        public IActionResult SaveNew(string title, string desc, DateTime deadline)
        {
            var newTodo = new TodoItem { Title = title, Description = desc, Deadline = deadline };
            context.TodoItems.Add(newTodo);
            context.SaveChanges();

            var todos = context.TodoItems.ToList();

            TempData["SuccessMessage"] = $"{newTodo.Title} is created successfully!";
            return View("Items", todos);
        }


        // // Editing To-do item
        public IActionResult Edit(int id)
        {
            var item = context.TodoItems.Find(id);
            return View(item);
        }
        public IActionResult SaveEdit(int id, string title, string desc, DateTime deadline)
        {
            if (Request.Method == "POST")
            {
                var item = context.TodoItems.Find(id);

                item.Title = title;
                item.Description = desc;
                item.Deadline = deadline;
                context.SaveChanges();
            }
            var todos = context.TodoItems.ToList();
            return View("Items", todos);
        }

        // // Deleting To-do item
        public IActionResult Delete(int id)
        {
            var item = context.TodoItems.Find(id);
            context.TodoItems.Remove(item);
            context.SaveChanges();

            var todos = context.TodoItems.ToList();
            return View("Items", todos);
        }
    }
}
