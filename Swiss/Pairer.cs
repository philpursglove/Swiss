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

        return games;
    }

    private static List<Game> RandomPairing(List<Player> players)
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