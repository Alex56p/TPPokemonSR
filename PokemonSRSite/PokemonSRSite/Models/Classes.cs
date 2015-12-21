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
        public int Id { get; set; }
        public int IdPlayersPokemon { get; set; }
        public int IdMove { get; set; }
        public string Username { get; set; }
        public string MoveName { get; set; }
        public string PokemonName { get; set; }
        public static List<PlayersPokemon> AllPlayersPokemon { get; set; }
        public static List<Move> AllMovesNames { get; set; }

        public PlayersMove()
        {
            Id = 0;
            IdPlayersPokemon = 0;
            IdMove = 0;
        }
    }

    public class PlayersMoves : SqlExpressUtilities.SqlExpressWrapper
    {
        public PlayersMove playersmove{ get; set; }

        public PlayersMoves( object cs)
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

        public void SelectFromId(string Id,string orderBy = "")
        {
            string sql = "SELECT PlayersMoves.Id, Players.Username, Moves.Name, Pokemons.Name" +
                            " FROM Players" +
                            " INNER JOIN PlayersPokemons ON Players.Username = PlayersPokemons.Username" +
                            " INNER JOIN Pokemons ON PlayersPokemons.PokemonID = Pokemons.Id" +
                            " INNER JOIN PlayersMoves ON PlayersMoves.IdPlayersPokemon = PlayersPokemons.ID" +
                            " INNER JOIN Moves ON Moves.Id = PlayersMoves.IdMove" +
                            " WHERE PlayersPokemons.Id = " + Id;

            if (orderBy != "")
                sql += " ORDER BY " + orderBy;

            QuerySQL(sql);
        }

        public List<PlayersMove> ToList()
        {
            List<PokemonSRSite.PlayersMove> playerspokemon_list = new List<PlayersMove>();

            if(reader != null)
            {
                while(reader.Read())
                {
                    PlayersMove pm = new PlayersMove();
                    pm.Id = reader.GetInt32(0);
                    pm.Username = reader.GetString(1);
                    pm.MoveName = reader.GetString(2);
                    pm.PokemonName = reader.GetString(3);

                    playerspokemon_list.Add(pm);
                }
            }

            PlayersMove.AllPlayersPokemon = getAllPlayersPokemon();
            PlayersMove.AllMovesNames = getAllMoves();
            return playerspokemon_list;
        }

        public List<Move> getAllMoves()
        {
            QuerySQL("SELECT Id, Name FROM MOVES");
            List<Move> list = new List<Move>();
            Move m = new Move();
            
            if(reader != null)
            {
                while(reader.Read())
                {
                    m = new Move();
                    m.Id = reader.GetInt32(0);
                    m.Name = reader.GetString(1);

                    list.Add(m);
                }
            }

            EndQuerySQL();
            return list;
        }

        public List<PlayersPokemon> getAllPlayersPokemon()
        {
            QuerySQL("SELECT PlayersPokemons.Id, PokemonID, Username, Pokemons.Name FROM PlayersPokemons INNER JOIN Pokemons ON PlayersPokemons.PokemonID = Pokemons.ID");
            PlayersPokemon pp = new PlayersPokemon();
            List<PlayersPokemon> list = new List<PlayersPokemon>();
            if (reader != null)
            {
                while(reader.Read())
                {
                    pp = new PlayersPokemon();
                    pp.Id = reader.GetInt32(0);
                    pp.PokemonID = reader.GetInt32(1);
                    pp.Username = reader.GetString(2);
                    pp.PokemonName = reader.GetString(3);

                    list.Add(pp);
                }
            }

            EndQuerySQL();

            return list;
        }

        public string getMoveName(string id)
        {
            QuerySQL("SELECT Name FROM Moves Where ID = " + id);
            String name = "";
            if (reader != null && reader.Read())
            {
                name = reader.GetString(0);
                EndQuerySQL();
            }
            return name;
        }

        public String getPokemonNameByID(string id)
        {
            string sql = "SELECT Name FROM Pokemons Where ID = " + getPokemonID(id);
            QuerySQL(sql);
            String name = "";
            if (reader != null && reader.Read())
            {
                name = reader.GetString(0);
                EndQuerySQL();
            }
            return name;
        }

        public string getUsername(string id)
        {
            QuerySQL("SELECT Username FROM PlayersPokemons WHERE Id = " + id);
            String name = "";
            if (reader != null && reader.Read())
            {
                name = reader.GetString(0);
                EndQuerySQL();
            }
            return name;
        }

        public string getPokemonID(string id)
        {
            reader = null;
            QuerySQL("SELECT PokemonID FROM PlayersPokemons WHERE Id = " + id);
            String name = "";
            if (reader != null && reader.Read())
            {
                name = reader.GetInt32(0).ToString();
                EndQuerySQL();
            }
            return name;
        }

        public void AddPlayersMove()
        {
            String sql = "INSERT INTO PlayersMoves VALUES(" + playersmove.IdPlayersPokemon + ", " + playersmove.IdMove + ")";
            NonQuerySQL(sql);
        }

        public void UdpatePlayersMove()
        {
            String sql = "UPDATE PlayersMoves SET [IdPlayersPokemon] = " + playersmove.IdPlayersPokemon +
                ", [IdMove] = " + playersmove.IdMove + "WHERE ID = " + playersmove.Id;

            NonQuerySQL(sql);
        }
    }

    public partial class PlayersPokemon
    {
        public int Id{ get; set; }
        public int PokemonID { get; set; }
        [Display(Name = "Pokemon Name")]
        public string PokemonName { get; set; }
        public String Username { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public static List<string> AllPokemons { get; set; }
        public static List<string> AllIdPokemons { get; set; }
        public static List<string> AllUsers { get; set; }


        public PlayersPokemon()
        {
            Id = 0;
            PokemonID = 0;
            PokemonName = "";
            Username = "";
            Level = 0;
            Exp = 0;
        }

    }

    public class PlayersPokemons : SqlExpressUtilities.SqlExpressWrapper
    {
        public PlayersPokemon playerspokemon { get; set; }

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
                            " WHERE Players.Username = '" + playerspokemon.Username + "'";

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
                pp.PokemonName = getNameByID(pp.PokemonID.ToString());

                
                playerspokemon_list.Add(pp);
            }
            PlayersPokemon.AllUsers = getAllUsers();
            PlayersPokemon.AllPokemons = getlistPokemons();
            PlayersPokemon.AllIdPokemons = getlistIDPokemon();
            return playerspokemon_list;
        }

        public List<string> getAllUsers()
        {
            List<string> list = new List<string>();
            string sql = "SELECT Username FROM Players";
            QuerySQL(sql);

            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }

            reader.Close();

            return list;
        }

        public List<string> getlistIDPokemon()
        {
            List<string> list = new List<string>();
            string sql = "SELECT Id FROM Pokemons";
            QuerySQL(sql);

            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(reader.GetInt32(0).ToString());
                }
            }

            reader.Close();

            return list;
        }

        public List<string> getlistPokemons()
        {
            List<string> list = new List<string>();
            string sql = "SELECT Name FROM Pokemons";
            QuerySQL(sql);

            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }

            reader.Close();

            return list;
        }

        public String getNameByID(string id)
        {
            QuerySQL("SELECT Name FROM Pokemons Where ID = " + id);
            String name = "";
            if(reader != null && reader.Read())
            {
                name = reader.GetString(0);
                EndQuerySQL();
            }
            return name;
        }

        public void UdpatePlayersPokemon()
        {
            String sql = "UPDATE PlayersPokemons SET [PokemonID] = "+ playerspokemon.PokemonID + 
                ", [Username] = '" + playerspokemon.Username + 
                "', [Level] = " + playerspokemon.Level + 
                ", [Exp] = "+ playerspokemon.Exp + " WHERE [Id] = " + playerspokemon.Id;

            NonQuerySQL(sql);
        }

        public void AddPlayersPokemon()
        {
            String sql = "INSERT INTO PlayersPokemons VALUES(" + playerspokemon.PokemonID + ", '" + playerspokemon.Username + "', " + playerspokemon.Level + ", " + playerspokemon.Exp + ")";
            NonQuerySQL(sql);
        }
    }   
}