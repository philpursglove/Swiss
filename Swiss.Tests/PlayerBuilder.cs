namespace Swiss;

public class PlayerBuilder
{
    Player _player = new Player();

    public static implicit operator Player(PlayerBuilder builder)
    {
        return builder.Build();
    }

    public PlayerBuilder WithName(string name)
    {
        _player.Name = name;
        return this;
    }

    public PlayerBuilder WithDropped(bool dropped)
    {
        _player.Dropped = dropped;
        return this;
    }

    public PlayerBuilder WithPoints(int points)
    {
        _player.Points = points;
        return this;
    }

    public PlayerBuilder WithBye(bool bye)
    {
        _player.Bye = bye;
        return this;
    }

    public Player Build()
    {
        return _player;
    }
}