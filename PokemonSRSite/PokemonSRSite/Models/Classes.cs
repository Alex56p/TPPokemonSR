using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PokemonSRSite
{
    public partial class Player
    {
        public string Username { get; set; }
        public int Money { get; set; }
        public string City { get; set; }
        public string Division { get; set; }

        public Player()
        {
            Username = "";
            Money = 0;
            City = "";
            Division = "";
        }
    }

    public class Players : SqlExpressUtilities.SqlExpressWrapper
    {
        public Player player { get; set; }

        public Players(object cs)
            : base(cs)
        {
            player = new Player();
        }

        public Players()
        {
            player = new Player();
        }

        public List<Player> ToList()
        {
            List<object> list = this.RecordsList();
            List<PokemonSRSite.Player> players_list = new List<Player>();
            foreach (Player player in list)
            {
                players_list.Add(player);
            }

            return players_list;
        }

        public void InsertPlayer()
        {
            NonQuerySQL("INSERT INTO Players VALUES('" + player.Username + "', " + player.Money + ", '" + player.City + "', '" + player.Division + "')");
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                base.DeleteRecordByID(ID);
            }
        }

        public override void DeleteRecordByUsername(String ID)
        {
            if (this.SelectByUsername(ID))
            {
                base.DeleteRecordByUsername(ID);
            }
        }
    }

    public partial class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public string Image { get; set; }

        private ImageGUIDReference ImageReference;

        public Pokemon()
        {
            Id = 0;
            Name = "";
            Type = "";
            Attack = 0;
            Defense = 0;
            Speed = 0;
            Image = "";
            ImageReference = new ImageGUIDReference(@"/Images/Films/", @"UnknownPoster.png");
        }

        public String GetPosterURL()
        {
            return ImageReference.GetImageURL(Image);
        }

        public void UpLoadPoster(HttpRequestBase Request)
        {
            Image = ImageReference.UpLoadImage(Request, Image);
        }

        public void RemovePoster()
        {
            ImageReference.Remove(Image);
        }
    }

    public class Pokemons : SqlExpressUtilities.SqlExpressWrapper
    {
        public Pokemon pokemon { get; set; }

        public Pokemons(object cs) : base(cs)
        {
            pokemon = new Pokemon();
        }

        public Pokemons()
        {
            pokemon = new Pokemon();
        }

        public List<Pokemon> ToList()
        {
            List<object> list = this.RecordsList();
            List<PokemonSRSite.Pokemon> pokemons_list = new List<Pokemon>();
            foreach(Pokemon pokemon in list)
            {
                pokemons_list.Add(pokemon);
            }

            return pokemons_list;
        }

        public override void DeleteRecordByID(String ID)
        {
            if(this.SelectByID(ID))
            {
                this.pokemon.RemovePoster();
                base.DeleteRecordByID(ID);
            }
        }
    }

    public partial class Move
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Damage { get; set; }
        public int Accuracy { get; set; }

        public Move()
        {
            Id = 0;
            Name = "";
            Type = "";
            Damage = 0;
            Accuracy = 0;
        }
    }

    public class Moves : SqlExpressUtilities.SqlExpressWrapper
    {
        public Move move { get; set; }

        public Moves(object cs)
            : base(cs)
        {
            move = new Move();
        }

        public Moves()
        {
            move = new Move();
        }

        public List<Move> ToList()
        {
            List<object> list = this.RecordsList();
            List<PokemonSRSite.Move> moves_list = new List<Move>();
            foreach (Move move in list)
            {
                moves_list.Add(move);
            }

            return moves_list;
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                base.DeleteRecordByID(ID);
            }
        }
    }

    public partial class PlayersMove
    {
        public int IdPlayersMoves { get; set; }
        public int IdPlayersPokemon { get; set; }
        public int IdMove { get; set; }

        public PlayersMove()
        {
            IdPlayersMoves = 0;
            IdPlayersPokemon = 0;
            IdMove = 0;
        }
    }

    public class PlayersMoves : SqlExpressUtilities.SqlExpressWrapper
    {
        public PlayersMove playersmove{ get; set; }

        public PlayersMoves(PlayersPokemon pp, object cs)
            : base(cs)
        {
            playersmove = new PlayersMove();
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                base.DeleteRecordByID(ID);
            }
        }

        public override void SelectAll(string orderBy = "")
        {
            string sql = "SELECT * " +
                            "FROM Players" +
                            " INNER JOIN PlayersPokemons ON Players.Username = PlayersPokemons.Username" +
                            " INNER JOIN Pokemons ON PlayersPokemons.PokemonID = Pokemons.Id" +
                            " WHERE Players.Username = " + SqlExpressUtilities.SQLHelper.ConvertValueFromMemberToSQL(playerspokemon.player.Username);

            if (orderBy != "")
                sql += " ORDER BY " + orderBy;

            QuerySQL(sql);
        }

        public List<PlayersPokemon> ToList()
        {
            List<object> list = this.RecordsList();
            List<PokemonSRSite.PlayersPokemon> playerspokemon_list = new List<PlayersPokemon>();
            foreach (PlayersPokemon pp in list)
            {
                pp.PokemonName = getNameByID();
                playerspokemon_list.Add(pp);
            }

            return playerspokemon_list;
        }

        public String getNameByID()
        {
            QuerySQL("SELECT Name FROM Pokemons P INNER JOIN PlayersPokemons PP ON PP.PokemonID = P.Id Where ID = " + playerspokemon.PokemonID);
            String name = "";
            if (reader != null && reader.Read())
            {
                name = reader.GetString(0);
                EndQuerySQL();
            }
            return name;
        }
    }

    public partial class PlayersPokemon
    {
        public int Id{ get; set; }
        public int PokemonID { get; set; }
        [Display(Name = "Pokemon Name")]
        public string PokemonName { get; set; }
        public Player player { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

        public PlayersPokemon()
        {
            Id = 0;
            PokemonID = 0;
            player = new Player();
            Level = 0;
            Exp = 0;
        }

    }

    public class PlayersPokemons : SqlExpressUtilities.SqlExpressWrapper
    {
        public PlayersPokemon playerspokemon { get; set; }

        public PlayersPokemons(Player p, object cs) :base(cs)
        {
            playerspokemon = new PlayersPokemon();
            playerspokemon.player = p;
        }

        public PlayersPokemons(object cs)
            : base(cs)
        {
            playerspokemon = new PlayersPokemon();
        }

        public PlayersPokemons()
        {
            playerspokemon = new PlayersPokemon();
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                base.DeleteRecordByID(ID);
            }
        }

        public override void SelectAll(string orderBy = "")
        {
            string sql = "SELECT * " +
                            "FROM Players" +
                            " INNER JOIN PlayersPokemons ON Players.Username = PlayersPokemons.Username" +
                            " INNER JOIN Pokemons ON PlayersPokemons.PokemonID = Pokemons.Id" +
                            " WHERE Players.Username = " + SqlExpressUtilities.SQLHelper.ConvertValueFromMemberToSQL(playerspokemon.player.Username);

            if (orderBy != "")
                sql += " ORDER BY " + orderBy;

            QuerySQL(sql);
        }

        public List<PlayersPokemon> ToList()
        {
            List<object> list = this.RecordsList();
            List<PokemonSRSite.PlayersPokemon> playerspokemon_list = new List<PlayersPokemon>();
            foreach (PlayersPokemon pp in list)
            {
                pp.PokemonName = getNameByID();
                playerspokemon_list.Add(pp);
            }

            return playerspokemon_list;
        }

        public String getNameByID()
        {
            QuerySQL("SELECT Name FROM Pokemons Where ID = " + playerspokemon.PokemonID);
            String name = "";
            if(reader != null && reader.Read())
            {
                name = reader.GetString(0);
                EndQuerySQL();
            }
            return name;
        }
    }   
}