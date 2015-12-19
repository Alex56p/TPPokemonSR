using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokemonSRSite.Controllers
{
    public class MovesController : Controller
    {
        public ActionResult Sort(String sortBy)
        {
            if (Session["Moves_SortBy"] == null)
            {
                Session["Moves_SortBy"] = sortBy;
                Session["Moves_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["Moves_SortBy"] == sortBy)
                {
                    if ((String)Session["Moves_sortOrder"] == "ASC")
                        Session["Moves_SortOrder"] = "DESC";
                    else
                        Session["Moves_SortOrder"] = "ASC";
                }
                else
                {
                    Session["Moves_SortBy"] = sortBy;
                    Session["Moves_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "Moves");
        }
        public ActionResult Lister()
        {
            
            Moves moves = new Moves(Session["Main_DB"]);

            String orderBy = "";
            if (Session["Moves_SortBy"] != null)
                orderBy = (String)Session["Moves_SortBy"] + " " + (String)Session["Moves_SortOrder"];

            moves.SelectAll(orderBy);
            return View(moves.ToList());
        }
        public ActionResult Ajouter()
        {
            return View(new Move());
        }

        [HttpPost]
        public ActionResult Ajouter(Move move)
        {
            if (ModelState.IsValid)
            {
                Moves moves = new Moves(Session["Main_DB"]);
                moves.move = move;
                moves.Insert();
                return RedirectToAction("Lister", "Moves");
            }
            return View(move);
        }

        public ActionResult Details(String Id)
        {
            Moves moves = new Moves(Session["Main_DB"]);
            if (moves.SelectByID(Id))
                return View(moves.move);
            else
                return RedirectToAction("Lister", "Moves");
        }

        public ActionResult Editer(String Id)
        {
            Moves moves = new Moves(Session["Main_DB"]);
            if (moves.SelectByID(Id))
                return View(moves.move);
            else
                return RedirectToAction("Lister", "Moves");
        }
        [HttpPost]
        public ActionResult Editer(Move move)
        {
            Moves moves = new Moves(Session["Main_DB"]);
            if (ModelState.IsValid)
            {
                if (moves.SelectByID(move.Id))
                {
                    moves.move = move;
                    moves.Update();
                    return RedirectToAction("Lister", "Moves");
                }
            }
            return View(move);
        }

        public ActionResult Effacer(String Id)
        {
            Moves moves = new Moves(Session["Main_DB"]);
            moves.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "Moves");
        }
    }
}