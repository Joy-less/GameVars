namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>, with no caching and no event handlers.
/// </summary>
public readonly struct GameVarSlim<T> : IGameVar<T> {
    /// <summary>
    /// The collection of game vars containing the game var.
    /// </summary>
    public GameVarCollection Collection { get; }
    /// <summary>
    /// The name (key) of the game var.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// A function returning a default value for the game var.
    /// </summary>
    public Func<T> DefaultValueFactory { get; }

    /// <summary>
    /// Creates a new interface to a game var in a <see cref="GameVarCollection"/>.
    /// </summary>
    public GameVarSlim(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory) {
        this.Collection = Collection;
        this.Name = Name;
        this.DefaultValueFactory = DefaultValueFactory;
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <see cref="DefaultValueFactory"/>.
    /// </summary>
    public T Get() {
        return Collection.GetGameVar(Name, DefaultValueFactory);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void Set(T Value) {
        Collection.SetGameVar(Name, Value);
    }
}