﻿using System;
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
            return RedirectToAction("Lister", "PlayersPokemon", new { Id = (String)Session["Username"] });
        }

        public ActionResult Lister(String Id)
        {
            Session["Username"] = Id;
                PlayersPokemons pkmn = new PlayersPokemons(Session["Main_DB"]);
                pkmn.playerspokemon.Username = Id;

                String orderBy = "";
                if (Session["PlayersPokemon_SortBy"] != null)
                    orderBy = (String)Session["PlayersPokemon_SortBy"] + " " + (String)Session["PlayersPokemon_SortOrder"];

                pkmn.SelectAll(orderBy);

                return View(pkmn.ToList());

        }
        public ActionResult Ajouter()
        {
            return View(new PlayersPokemon());
        }

        [HttpPost]
        public ActionResult Ajouter(PlayersPokemon player)
        {
            if (ModelState.IsValid)
            {
                PlayersPokemons players = new PlayersPokemons(Session["Main_DB"]);

                players.playerspokemon = player;
                players.playerspokemon.Username = Request["users"].ToString();
                players.playerspokemon.PokemonID = int.Parse(Request["pokemons"]);
                players.AddPlayersPokemon();
                return RedirectToAction("Lister", "PlayersPokemon", new { Id = (String)Session["Username"] });
            }
            return View(player);
        }


        public ActionResult Details(String Id)
        {
            PlayersPokemons pp = new PlayersPokemons(Session["Main_DB"]);
            if (pp.SelectByID(Id))
            {
                return View(pp.playerspokemon);
            }
            else
                return RedirectToAction("Lister", "PlayersPokemon", new { Id = (String)Session["Username"] });
        }

        public ActionResult Editer(String Id)
        {
            PlayersPokemons pp = new PlayersPokemons(Session["Main_DB"]);
            if (pp.SelectByID(Id))
            {
                return View(pp.playerspokemon);
            }
            else
                return RedirectToAction("Lister", "PlayersPokemon", new { Id = (String)Session["Username"] });
        }

        [HttpPost]
        public ActionResult Editer(PlayersPokemon pp)
        {
            PlayersPokemons players = new PlayersPokemons(Session["Main_DB"]);
            if (ModelState.IsValid)
            {
                if (players.SelectByID(pp.Id))
                {
                    players.playerspokemon = pp;
                    players.playerspokemon.PokemonID = int.Parse(Request["pokemons"]);
                    players.playerspokemon.Username = Request["users"].ToString();
                    players.UdpatePlayersPokemon();
                    return RedirectToAction("Lister", "PlayersPokemon", new { Id = (String)Session["Username"] });
                }
            }
            return View(pp);
        }

        public ActionResult Effacer(String Id)
        {
            PlayersPokemons pp = new PlayersPokemons(Session["Main_DB"]);
            pp.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "PlayersPokemon", new { Id = (String)Session["Username"] });
        }
    }
}