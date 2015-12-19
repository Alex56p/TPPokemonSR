using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokemonSRSite.Controllers
{
    public class PlayersPokemonController : Controller
    {
        public ActionResult Sort(String sortBy)
        {
            if (Session["PlayersPokemon_SortBy"] == null)
            {
                Session["PlayersPokemon_SortBy"] = sortBy;
                Session["PlayersPokemon_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["PlayersPokemon_SortBy"] == sortBy)
                {
                    if ((String)Session["PlayersPokemon_sortOrder"] == "ASC")
                        Session["PlayersPokemon_SortOrder"] = "DESC";
                    else
                        Session["PlayersPokemon_SortOrder"] = "ASC";
                }
                else
                {
                    Session["PlayersPokemon_SortBy"] = sortBy;
                    Session["PlayersPokemon_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "PlayersPokemonPlayer");
        }

        public ActionResult Lister(String Id)
        {
            Players players = new Players(Session["Main_DB"]);
            if (players.SelectByUsername(Id))
            {
                PlayersPokemons pkmn = new PlayersPokemons(players.player, Session["Main_DB"]);

                String orderBy = "";
                if (Session["PlayersPokemon_SortBy"] != null)
                    orderBy = (String)Session["PlayersPokemon_SortBy"] + " " + (String)Session["PlayersPokemon_SortOrder"];

                pkmn.SelectAll(orderBy);
            
                return View(pkmn.ToList());
            }
            else
            {
                return RedirectToAction("Lister", "Player");
            }
        }
        public ActionResult Ajouter()
        {
            return View(new PlayersPokemon());
        }
    }
}