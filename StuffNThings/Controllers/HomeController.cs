using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuffNThings.Repository.Repositories;
using StuffNThings.Repository.Models;
using StuffNThings.Filters;

namespace StuffNThings.Controllers
{
	[InitializeSimpleMembership]
	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			PostRepository postRepository = new PostRepository(ConfigurationManager.ConnectionStrings [ "DefaultConnection" ].ConnectionString);
			var model = postRepository.GetAllPosts();

			return View(model);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
