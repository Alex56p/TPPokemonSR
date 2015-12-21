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
            return RedirectToAction("Lister", "PlayersMove", new { Id = (String)Session["IdPlayersPokemon"] });
        }

        public ActionResult Lister(String Id)
        {
            PlayersMoves pkmn = new PlayersMoves(Session["Main_DB"]);

            Session["IdPlayersPokemon"] = Id;
            String orderBy = "";
            if (Session["PlayersMove_SortBy"] != null)
                orderBy = (String)Session["PlayersMove_SortBy"] + " " + (String)Session["PlayersMove_SortOrder"];

            pkmn.playersmove.Username = pkmn.getUsername(Id);
            pkmn.SelectFromId(Id,orderBy);

            List<PlayersMove> pm = pkmn.ToList();
            return View(pm);
        }

        public ActionResult Effacer(String Id)
        {
            PlayersMoves pp = new PlayersMoves(Session["Main_DB"]);
            pp.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "PlayersMove", new { Id = (String)Session["IdPlayersPokemon"] });
        }

        public ActionResult Ajouter()
        {
            return View(new PlayersMove());
        }

        [HttpPost]
        public ActionResult Ajouter(PlayersMove PMove)
        {
            if (ModelState.IsValid)
            {
                PlayersMoves moves = new PlayersMoves(Session["Main_DB"]);

                moves.playersmove = PMove;
                moves.playersmove.IdMove = int.Parse(Request["moves"]);
                moves.playersmove.IdPlayersPokemon = int.Parse(Request["playerspokemon"]);
                moves.AddPlayersMove();
                return RedirectToAction("Lister", "PlayersMove", new { Id = (String)Session["IdPlayersPokemon"] });
            }
            return View(PMove);
        }

        public ActionResult Editer(String Id)
        {
            PlayersMoves pp = new PlayersMoves(Session["Main_DB"]);
            if (pp.SelectByID(Id))
            {
                return View(pp.playersmove);
            }
            else
                return RedirectToAction("Lister", "PlayersMove", new { Id = (String)Session["IdPlayersPokemon"] });
        }

        [HttpPost]
        public ActionResult Editer(PlayersMove pp)
        {
            PlayersMoves players = new PlayersMoves(Session["Main_DB"]);
            if (ModelState.IsValid)
            {
                if (players.SelectByID(pp.Id))
                {
                    players.playersmove = pp;
                    players.playersmove.IdPlayersPokemon = int.Parse(Request["playerspokemons"]);
                    players.playersmove.IdMove = int.Parse(Request["move"]);
                    players.UdpatePlayersMove();
                    return RedirectToAction("Lister", "PlayersMove", new { Id = (String)Session["IdPlayersPokemon"] });
                }
            }
            return View(pp);
        }

        public ActionResult Details(String Id)
        {
            PlayersMoves pp = new PlayersMoves(Session["Main_DB"]);
            if (pp.SelectByID(Id))
            {
                pp.playersmove.PokemonName = pp.getPokemonNameByID(pp.playersmove.IdPlayersPokemon.ToString());
                pp.playersmove.MoveName = pp.getMoveName(pp.playersmove.IdMove.ToString());
                pp.playersmove.Username = pp.getUsername(pp.playersmove.IdPlayersPokemon.ToString());
                return View(pp.playersmove);
            }
            else
                return RedirectToAction("Lister", "PlayersMove", new { Id = (String)Session["IdPlayersPokemon"] });
        }
    }
}