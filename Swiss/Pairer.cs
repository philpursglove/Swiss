using System.Threading.Tasks.Dataflow;

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
        List<Bracket> brackets = new List<Bracket>();

        IEnumerable<int> bracketPoints = players.Select(p => p.Points).Distinct().OrderByDescending(p => p);

        foreach (var bracketPoint in bracketPoints)
        {
            Bracket bracket = new Bracket()
            { Points = bracketPoint, Players = players.Where(p => p.Points == bracketPoint).ToList() };
            brackets.Add(bracket);
        }

        for (int i = 0; i < brackets.Count; i++)
        {
            List<Player> bracketPlayers = brackets[i].Players;
            if (bracketPlayers.Count % 2 == 1)
            {
                if (i < brackets.Count - 1)
                {
                    Player pairedUp = brackets[i + 1].Players.Random();
                    bracketPlayers.Add(pairedUp);
                    brackets[i + 1].Players.Remove(pairedUp);
                }
            }

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
                newGame.Player2 = new Player() { Name = "BYE" };
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