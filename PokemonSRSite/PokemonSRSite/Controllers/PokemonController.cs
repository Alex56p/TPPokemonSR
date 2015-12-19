using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokemonSRSite.Controllers
{
    public class PokemonController : Controller
    {
        public ActionResult Sort(String sortBy)
        {
            if (Session["Pokemon_SortBy"] == null)
            {
                Session["Pokemon_SortBy"] = sortBy;
                Session["Pokemon_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["Pokemon_SortBy"] == sortBy)
                {
                    if ((String)Session["Pokemon_sortOrder"] == "ASC")
                        Session["Pokemon_SortOrder"] = "DESC";
                    else
                        Session["Pokemon_SortOrder"] = "ASC";
                }
                else
                {
                    Session["Pokemon_SortBy"] = sortBy;
                    Session["Pokemon_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "Pokemon");
        }
        public ActionResult Lister()
        {
            Pokemons pokemons = new Pokemons(Session["Main_DB"]);

            String orderBy = "";
            if (Session["Pokemon_SortBy"] != null)
                orderBy = (String)Session["Pokemon_SortBy"] + " " + (String)Session["Pokemon_SortOrder"];

            pokemons.SelectAll(orderBy);
            return View(pokemons.ToList());
        }
        public ActionResult Ajouter()
        {
            return View(new Pokemon());
        }

        [HttpPost]
        public ActionResult Ajouter(Pokemon pokemon)
        {
            if (ModelState.IsValid)
            {
                Pokemons pokemons = new Pokemons(Session["Main_DB"]);
                pokemons.pokemon = pokemon;;
                pokemons.pokemon.UpLoadPoster(Request);
                pokemons.Insert();
                return RedirectToAction("Lister", "Pokemon");
            }
            return View(pokemon);
        }

        public ActionResult Details(String Id)
        {
            Pokemons pokemons = new Pokemons(Session["Main_DB"]);
            if (pokemons.SelectByID(Id))
                return View(pokemons.pokemon);
            else
                return RedirectToAction("Lister", "Pokemon");
        }

        public ActionResult Editer(String Id)
        {
            Pokemons pokemons = new Pokemons(Session["Main_DB"]);
            if (pokemons.SelectByID(Id))
                return View(pokemons.pokemon);
            else
                return RedirectToAction("Lister", "Pokemon");
        }
        [HttpPost]
        public ActionResult Editer(Pokemon pokemon)
        {
            Pokemons pokemons = new Pokemons(Session["Main_DB"]);
            if (ModelState.IsValid)
            {
                if (pokemons.SelectByID(pokemon.Id))
                {
                    pokemons.pokemon = pokemon;
                    pokemons.pokemon.UpLoadPoster(Request);
                    pokemons.Update();
                    return RedirectToAction("Lister", "Pokemon");
                }
            }
            return View(pokemon);
        }

        public ActionResult Effacer(String Id)
        {
            Pokemons pokemons = new Pokemons(Session["Main_DB"]);
            pokemons.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "Pokemon");
        }
    }
}