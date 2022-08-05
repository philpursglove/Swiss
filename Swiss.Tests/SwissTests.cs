namespace Swiss.Tests
{
    [TestFixture]
    public class SwissTests
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

        [Test]
        public void Players_Get_Matched_With_Players_On_Equal_Points()
        {
            Player alice = new Player() { Name = "Alice", Points = 3 };
            Player bob = new Player() { Name = "Bob", Points = 3 };
            Player charlie = new Player() { Name = "Charlie", Points = 0 };
            Player dave = new Player() { Name = "Dave", Points = 0 };

            List<Player> players = new List<Player>() { alice, bob, charlie, dave };

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            var game = result.First(g => g.Player1 == alice || g.Player2 == alice);
            if (game.Player1 == alice)
            {
                Assert.That(game.Player2, Is.EqualTo(bob));
            }
            else
            {
                Assert.That(game.Player1, Is.EqualTo(bob));
            }
            game = result.First(g => g.Player1 == charlie || g.Player2 == charlie);
            if (game.Player1 == charlie)
            {
                Assert.That(game.Player2, Is.EqualTo(dave));
            }
            else
            {
                Assert.That(game.Player1, Is.EqualTo(dave));
            }
        }

        [Test]
        public void Where_A_Bracket_Has_An_Odd_Number_Of_Players_A_Random_Player_From_The_Next_Bracket_Is_Paired_Up()
        {
            List<Player> players = new List<Player>()
            {
                new Player() { Name = "Alice", Points = 3 },
                new Player() { Name = "Bob", Points = 1 },
                new Player() { Name = "Charlie", Points = 1 },
                new Player() { Name = "Dave", Points = 0 }
            };

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            // One of bob or charlie must be paired with alice
            // One of bob or charlie must be paired with dave

            IEnumerable<Player> middlePlayers = players.Where(p => p.Name == "Bob" || p.Name == "Charlie");

            Game firstGame = result.First(g => g.Player1.Name == "Alice" || g.Player2.Name == "Alice");
            if (firstGame.Player1.Name == "Alice")
            {
                Assert.That(middlePlayers.Contains(firstGame.Player2));
            }
            else
            {
                Assert.That(middlePlayers.Contains(firstGame.Player1));
            }
            Game lastGame = result.First(g => g.Player1.Name == "Dave" || g.Player2.Name == "Dave");
            if (lastGame.Player1.Name == "Dave")
            {
                Assert.That(middlePlayers.Contains(lastGame.Player2));
            }
            else
            {
                Assert.That(middlePlayers.Contains(lastGame.Player1));
            }
        }

        [Test]
        public void Where_Brackets_Finish_With_An_Odd_Number_In_The_Last_Group_The_Last_Game_Is_A_Bye()
        {
            List<Player> players = new List<Player>()
            {
                new Player() {Name = "Alice", Points = 3},
                new Player() {Name = "Bob", Points = 1},
                new Player() {Name = "Charlie", Points = 1},
                new Player() {Name = "Dave", Points = 0},
                new Player() {Name = "Ed", Points = 0},
                new Player() {Name = "Fred", Points = 0, Dropped = true}
            };

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            // One of bob or charlie must be paired with alice
            // One of bob or charlie must be paired with dave

            Game lastGame = result.Last();
            Assert.That(lastGame.Player2.Name, Is.EqualTo("BYE"));
        }

        [Test]
        public void In_The_Last_Bracket_Assign_The_Bye_To_A_Player_Who_Has_Not_Had_A_Bye()
        {
            List<Player> players = new List<Player>()
            {
                new PlayerBuilder().WithName("Alice").WithPoints(3).Build(),
                new PlayerBuilder().WithName("Bob").WithPoints(3).Build(),
                new PlayerBuilder().WithName("Charlie").WithPoints(1).Build(),
                new PlayerBuilder().WithName("Dave").WithPoints(1).Build(),
                new PlayerBuilder().WithName("Ed").WithPoints(0).WithBye(true).Build(),
                new PlayerBuilder().WithName("Fred").WithPoints(0).WithBye(true).Build(),
                new PlayerBuilder().WithName("George").WithPoints(0).WithBye(false).Build()
            };

            Pairer pairer = new Pairer();

            var result = pairer.Pair(players);

            Game lastGame = result.Last();
            Assert.That(lastGame.Player1.Name, Is.EqualTo("George"));
            Assert.That(lastGame.Player2.Name, Is.EqualTo("BYE"));
        }

        [Test]
        public void Avoid_Repeated_Games()
        {
            Assert.That(true, Is.False);
        }
    }
}