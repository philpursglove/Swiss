namespace Swiss.Tests;

public class RandomStrategyTests
{
    private Pairer pairer;

    [SetUp]
    public void SetUp()
    {
        pairer = new Pairer(new RandomPairingStrategy());
    }

    [Test]
    public void Players_Are_Matched_Randomly()
    {
        Player alice = new PlayerBuilder().WithName("Alice");
        Player bob = new PlayerBuilder().WithName("Bob");
        Player charlie = new PlayerBuilder().WithName("Charlie");
        Player dave = new PlayerBuilder().WithName("Dave");

        List<Player> players = new List<Player>() { alice, bob, charlie, dave };

        var result = pairer.Pair(players);

        List<Player> seenPlayers = new List<Player>();

        foreach (Game game in result)
        {
            Assert.That(players.Except(seenPlayers).Contains(game.Player1));
            seenPlayers.Add(game.Player1);
            Assert.That(players.Except(seenPlayers).Contains(game.Player2));
            seenPlayers.Add(game.Player2);
        }
    }
}