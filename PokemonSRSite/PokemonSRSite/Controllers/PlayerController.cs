using PokemonSRSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlayerSRSite.Controllers
{
    public class PlayerController : Controller
    {
        public ActionResult Sort(String sortBy)
        {
            if (Session["Player_SortBy"] == null)
            {
                Session["Player_SortBy"] = sortBy;
                Session["Player_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["Player_SortBy"] == sortBy)
                {
                    if ((String)Session["Player_sortOrder"] == "ASC")
                        Session["Player_SortOrder"] = "DESC";
                    else
                        Session["Player_SortOrder"] = "ASC";
                }
                else
                {
                    Session["Player_SortBy"] = sortBy;
                    Session["Player_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "Player");
        }
        public ActionResult Lister()
        {
            Players players = new Players(Session["Main_DB"]);

            String orderBy = "";
            if (Session["Player_SortBy"] != null)
                orderBy = (String)Session["Player_SortBy"] + " " + (String)Session["Player_SortOrder"];

            players.SelectAll(orderBy);
            return View(players.ToList());
        }
        public ActionResult Ajouter()
        {
            return View(new Player());
        }

        [HttpPost]
        public ActionResult Ajouter(Player player)
        {
            if (ModelState.IsValid)
            {
                Players players = new Players(Session["Main_DB"]);
                players.player = player;
                players.InsertPlayer();
                return RedirectToAction("Lister", "Player");
            }
            return View(player);
        }

        public ActionResult Details(String Id)
        {
            Players players = new Players(Session["Main_DB"]);
            if (players.SelectByUsername(Id))
                return View(players.player);
            else
                return RedirectToAction("Lister", "Player");
        }

        public ActionResult Editer(String Id)
        {
            Players players = new Players(Session["Main_DB"]);
            if (players.SelectByUsername(Id))
                return View(players.player);
            else
                return RedirectToAction("Lister", "Player");
        }
        [HttpPost]
        public ActionResult Editer(Player player)
        {
            Players players = new Players(Session["Main_DB"]);
            if (ModelState.IsValid)
            {
                if (players.SelectByUsername(player.Username))
                {
                    players.player = player;
                    players.Update();
                    return RedirectToAction("Lister", "Player");
                }
            }
            return View(player);
        }

        public ActionResult Effacer(String Id)
        {
            Players players = new Players(Session["Main_DB"]);
            players.DeleteRecordByUsername(Id);
            return RedirectToAction("Lister", "Player");
        }
    }
}