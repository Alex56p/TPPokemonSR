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
            Players p = new Players(Session["Main_DB"]);
            if (p.SelectByUsername(Id))
            {
                PlayersPokemons playerspokemons = new PlayersPokemons(p.player,Session["Main_DB"]);
            
                PlayersMoves pkmn = new PlayersMoves(playerspokemons.playerspokemon, Session["Main_DB"]);

                String orderBy = "";
                if (Session["PlayersMove_SortBy"] != null)
                    orderBy = (String)Session["PlayersMove_SortBy"] + " " + (String)Session["PlayersMove_SortOrder"];

                pkmn.SelectAll(orderBy);

                return View(pkmn.ToList());
            }
            else
            {
                return RedirectToAction("Lister", "Player");
            }
        }
    }
}