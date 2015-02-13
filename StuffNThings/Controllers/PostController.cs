using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuffNThings.Models;
using StuffNThings.Repository.Models;
using StuffNThings.Repository.Repositories;

namespace StuffNThings.Controllers
{
    public class PostController : Controller
    {
		private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		
        //
        // GET: /Post/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Post/Details/5

        public ActionResult Details(int id)
        {
			var postRepository = new PostRepository(_connectionString);
			var model = postRepository.GetPostById(id);
            return View(model);
        }

        //
        // GET: /Post/Create
		[Authorize(Roles = "SystemAdministrator,RegionalAdministrator,Member")]
        public ActionResult Create()
        {
			//if (User != null && User.Identity.IsAuthenticated)
				return View();
			//else
				//return RedirectToAction("Login", "Account");
        }

        //
        // POST: /Post/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
				var postRepository = new PostRepository(_connectionString);

				// TODO: check for validation & probably create more validation
				var post = new Post {Title = collection.GetValue("Title").AttemptedValue,
									Description = collection.GetValue("Description").AttemptedValue,
									Price = Convert.ToDecimal(collection.GetValue("Price").AttemptedValue),
									Location = collection.GetValue("Location").AttemptedValue,
									TypeOfPostId = Convert.ToInt32(collection.GetValue("SelectedTypeOfPostId").AttemptedValue)};

				// TODO: will need to loop through the actual selected regions (whenever a control is created to select multiple regions) when done.
					//currently a region is created based on the single selection from the control and a hard coded region for sanity checking.
				post.Regions = new[] {new Region { Id = Convert.ToInt32(collection.GetValue("SelectedRegionId").AttemptedValue) },
															new Region {Id = 3}};

				post.Id = postRepository.AddPost(post);

                return RedirectToAction("Details", new {id = post.Id});
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Post/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Post/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Post/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Post/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		#region Comment Methods 
		//Move this to its own CommentController?
		[HttpPost]
		public string PersistComment(int postId, string bodyContent)
		{
			var commentRepository = new CommentRepository(_connectionString);

			// TODO: check for validation & probably create more validation
			var comment = new Comment
			{
				PostId = postId,
				Body = bodyContent
			};

			commentRepository.AddComment(comment);

			return "Your comment has been added.";
		}

		public JsonResult GetPostComments(int postId)
		{
			var commentRepository = new CommentRepository(_connectionString);

			var comments = (from c in commentRepository.GetPostsComments(postId)
							select new CommentTemp{Body = c.Body, CreatedDate = ((DateTime)c.CreatedDate).ToShortDateString()}).ToList();
							
			return Json(comments, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region PartialView Actions

		public ActionResult TypeOfPostSelector()
		{
			return PartialView("_TypeOfPostSelector", new TypeOfPostViewModel { TypeOfPosts = GetTypeOfPosts()});
		}

		public ActionResult RegionSelector()
		{
			return PartialView("_RegionSelector", new RegionViewModel {Regions = GetRegions()});
		}

		#endregion

		#region Internal Methods

		private IEnumerable<SelectListItem> GetTypeOfPosts()
		{
			var typeOfPostRepository = new TypeOfPostRepository(_connectionString);
			var typeOfPosts = typeOfPostRepository.GetAllTypeOfPosts().Select(top => new SelectListItem { Text = top.Name, Value = top.Id.ToString() });
			
			return new SelectList(typeOfPosts, "Value", "Text");
		}

		private IEnumerable<SelectListItem> GetRegions()
		{
			var locationRepository = new LocationRepository(_connectionString);
			// will need to figure a way of passing the correct state (for now it'll remain hard coded to Ohio)
			var regions = locationRepository.GetAllRegionsByState(2).Select(reg => new SelectListItem { Text = reg.Name, Value = reg.Id.ToString() });

			return new SelectList(regions, "Value", "Text");
		}

		#endregion
    }
}
 
public partial class CommentTemp
{
	public string Body { get; set; }
	public string CreatedDate { get; set; }
}