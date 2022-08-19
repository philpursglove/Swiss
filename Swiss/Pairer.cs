namespace Swiss;

public class Pairer
{
    public List<Game> Pair(List<Player> players)
    {
        players = players.Where(p => !p.Dropped).ToList();

        List<Game> games = new List<Game>();

        IPairingStrategy strategy = null;

        if (players.TrueForAll(p => p.Points == 0))
        {
            strategy = new RandomPairingStrategy();
        }
        else
        {
            strategy = new PointsPairingStrategy();
        }

        games = strategy.Pair(players).ToList();

        return games;
    }
}