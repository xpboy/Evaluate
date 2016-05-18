using Evaluate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Evaluate.Controllers
{
    public class EvaluationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evaluation/met/5
        public ActionResult met(int? Id)
        {
            return View("Index");
        }        
    }
}