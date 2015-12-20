using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokemonSRSite.Controllers
{
    public class PlayersMoveController : Controller
    {
        public ActionResult Sort(String sortBy)
        {
            if (Session["PlayersMove_SortBy"] == null)
            {
                Session["PlayersMove_SortBy"] = sortBy;
                Session["PlayersMove_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["PlayersMove_SortBy"] == sortBy)
                {
                    if ((String)Session["PlayersMove_sortOrder"] == "ASC")
                        Session["PlayersMove_SortOrder"] = "DESC";
                    else
                        Session["PlayersMove_SortOrder"] = "ASC";
                }
                else
                {
                    Session["PlayersMove_SortBy"] = sortBy;
                    Session["PlayersMove_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "PlayersMovePlayer");
        }

        public ActionResult Lister(String Id)
        {
            PlayersMoves pkmn = new PlayersMoves(Session["Main_DB"]);

            String orderBy = "";
            if (Session["PlayersPokemon_SortBy"] != null)
                orderBy = (String)Session["PlayersPokemon_SortBy"] + " " + (String)Session["PlayersPokemon_SortOrder"];

            pkmn.playersmove.Username = Id;
            pkmn.SelectAll(orderBy);

            return View(pkmn.ToList());
        }

        public ActionResult Effacer(String Id)
        {
            PlayersMoves pp = new PlayersMoves(Session["Main_DB"]);
            pp.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "PlayersMove");
        }
    }
}