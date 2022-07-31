namespace Swiss.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Players_Are_Matched_Randomly()
        {
            Player alice = new Player() { Name = "Alice" };
            Player bob = new Player() { Name = "Bob" };
            Player charlie = new Player() { Name = "Charlie" };
            Player dave = new Player() { Name = "Dave" };

            List<Player> players = new List<Player>() { alice, bob, charlie, dave };

            Pairer pairer = new Pairer();

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

        [Test]
        public void For_An_Odd_Number_Of_Players_The_Last_Player_Is_A_Bye()
        {
            Player alice = new Player() { Name = "Alice" };
            Player bob = new Player() { Name = "Bob" };
            Player charlie = new Player() { Name = "Charlie" };

            List<Player> players = new List<Player>() { alice, bob, charlie };

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            var game = result.Last();

            Assert.That(game.Player2.Name, Is.EqualTo("BYE"));
        }

        [TestCase(2, 1)]
        [TestCase(3, 2)]
        [TestCase(4, 2)]
        [TestCase(5, 3)]
        public void Number_Of_Games_Generated(int numberOfPlayers, int expectedNumberOfGames)
        {
            List<Player> players = new List<Player>();

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                Player player = new Player() { Name = $"Player{i}" };
                players.Add(player);
            }

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            Assert.That(result.Count, Is.EqualTo(expectedNumberOfGames));
        }

        [Test]
        public void A_Dropped_Player_Is_Not_Paired()
        {
            Player alice = new Player() { Name = "Alice" };
            Player bob = new Player() { Name = "Bob" };
            Player charlie = new Player() { Name = "Charlie" };
            Player dave = new Player() { Name = "Dave", Dropped = true };

            List<Player> players = new List<Player>() { alice, bob, charlie, dave };

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            var game = result.Last();

            Assert.That(game.Player2.Name, Is.EqualTo("BYE"));
        }
    }
}