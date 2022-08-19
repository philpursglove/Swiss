namespace Swiss;

public interface IPairingStrategy
{
    IEnumerable<Game> Pair(List<Player> players);
}