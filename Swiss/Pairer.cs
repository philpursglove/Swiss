namespace Swiss;

public class Pairer
{
    public List<Game> Pair(List<Player> players)
    {
        players = players.Where(p => !p.Dropped).ToList();

        List<Game> games = new List<Game>();

        if (players.TrueForAll(p => p.Points == 0))
        {
            games = RandomPairing(players);
        }
        else
        {
            games = PointsPairing(players);
        }

        return games;
    }

    private List<Game> PointsPairing(List<Player> players)
    {
        List<Game> games = new List<Game>();

        // TODO Add Distinct here
        IEnumerable<int> brackets = players.Select(p => p.Points);

        foreach (var bracket in brackets)
        {
            List<Player> bracketPlayers = players.Where(p => p.Points == bracket).ToList();

            while (bracketPlayers.Any())
            {
                Game newGame = new Game();
                newGame.Player1 = bracketPlayers.Random();
                bracketPlayers.Remove(newGame.Player1);

                if (!bracketPlayers.Any())
                {
                    newGame.Player2 = new Player() { Name = "BYE" };
                }
                else
                {
                    newGame.Player2 = bracketPlayers.Random();
                    bracketPlayers.Remove(newGame.Player2);
                }

                games.Add(newGame);
            }
        }

        return games;   
    }

    private List<Game> RandomPairing(List<Player> players)
    {
        List<Game> games = new List<Game>();

        while (players.Any())
        {
            Game newGame = new Game();
            newGame.Player1 = players.Random();
            players.Remove(newGame.Player1);

            if (!players.Any())
            {
                newGame.Player2 = new Player() {Name = "BYE"};
            }
            else
            {
                newGame.Player2 = players.Random();
                players.Remove(newGame.Player2);
            }

            games.Add(newGame);
        }

        return games;
    }
}
