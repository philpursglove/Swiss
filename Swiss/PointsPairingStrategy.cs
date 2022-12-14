namespace Swiss;

public class PointsPairingStrategy : IPairingStrategy
{
    public IEnumerable<Game> Pair(List<Player> players)
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
                if (bracketPlayers.Any(p => p.Bye))
                {
                    newGame.Player1 = bracketPlayers.Where(p => p.Bye).Random();
                }
                else
                {
                    newGame.Player1 = bracketPlayers.Random();
                }
                bracketPlayers.Remove(newGame.Player1);

                if (!bracketPlayers.Any())
                {
                    newGame.Player2 = new Player() { Name = "BYE" };
                }
                else
                {
                    if (bracketPlayers.Any(p => p.Bye))
                    {
                        newGame.Player2 = bracketPlayers.Where(p => p.Bye).Random();
                    }
                    else
                    {
                        newGame.Player2 = bracketPlayers.Random();
                    }
                    bracketPlayers.Remove(newGame.Player2);
                }

                games.Add(newGame);
            }
        }

        return games;

    }
}