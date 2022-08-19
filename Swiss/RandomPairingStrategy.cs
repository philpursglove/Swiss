namespace Swiss;

public class RandomPairingStrategy : IPairingStrategy
{
    public IEnumerable<Game> Pair(List<Player> players)
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