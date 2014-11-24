using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db4objects.Db4o.Linq;
using Db4oDoc.WebApp.Infrastructure;
using Db4oDoc.WebApp.Models;

namespace Db4oDoc.WebApp.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        // #example: Listing all pilots on the index
        public ActionResult Index()
        {
            IList<Pilot> allPilots = Db4oProvider.Database.Query<Pilot>();
            return View(allPilots);
        }
        // #end example

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Pilot newPilot)
        {
            if (Valid(newPilot))
            {
                Db4oProvider.Database.Store(newPilot);        
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(Guid id)
        {
            Pilot pilot = GetPilotById(id);
            Db4oProvider.Database.Delete(pilot);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            Pilot pilot = GetPilotById(id);
            return View(pilot);
        }

        // #example: update the object
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Guid id, Pilot editedPilot)
        {
            Pilot databasePilot = GetPilotById(id);
            databasePilot.Name = editedPilot.Name;
            databasePilot.Points = editedPilot.Points;
            Db4oProvider.Database.Store(databasePilot);

            return RedirectToAction("Index");
        }
        // #end example

        private Pilot GetPilotById(Guid id)
        {
            return (from Pilot p in Db4oProvider.Database
                    where p.ID.Equals(id)
                    select p).Single();
        }

        private bool Valid(Pilot pilot)
        {
            return pilot.Name.Length > 0;
        }
    }
}
