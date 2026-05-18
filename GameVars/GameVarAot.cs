using System.Text.Json.Serialization.Metadata;

namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>, with caching and event handlers.<br/>
/// Unlike <see cref="GameVar{T}"/>, <see cref="GameVarAot{T}"/> is trimmable due to using <see cref="JsonTypeInfo{T}"/>.
/// </summary>
public sealed class GameVarAot<T> : IGameVar<T> {
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
    /// Metadata about the type of the game var.
    /// </summary>
    public JsonTypeInfo<T> TypeInfo { get; }

    private T CachedValue;

    /// <summary>
    /// Invoked whenever the game var is changed.<br/>
    /// Note: The value may be the same as before.
    /// </summary>
    public event OnChangedEventHandler? OnChanged;
    /// <summary>
    /// A handler for <see cref="OnChanged"/>.
    /// </summary>
    public delegate void OnChangedEventHandler();

    /// <summary>
    /// Creates a new interface to a game var in a <see cref="GameVarCollection"/>,
    /// with a cached value of the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
    public GameVarAot(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory, JsonTypeInfo<T> TypeInfo) {
        this.Collection = Collection;
        this.Name = Name;
        this.DefaultValueFactory = DefaultValueFactory;
        this.TypeInfo = TypeInfo;

        CachedValue = Collection.GetGameVar(Name, DefaultValueFactory, TypeInfo);

        Collection.OnGameVarChanged += OnGameVarChangedHandler;
    }
    /// <summary>
    /// Clears the event handlers from the collection.
    /// </summary>
    ~GameVarAot() {
        Collection.OnGameVarChanged -= OnGameVarChangedHandler;
    }
    /// <summary>
    /// Gets the cached value of the given game var.
    /// </summary>
    public T Get() {
        return CachedValue;
    }
    /// <summary>
    /// Sets the value and cached value of the given game var.
    /// </summary>
    public void Set(T Value) {
        CachedValue = Value;

        Collection.SetGameVar(Name, Value, TypeInfo);
    }

    private void OnGameVarChangedHandler(string GameVarName) {
        if (GameVarName == Name) {
            CachedValue = Collection.GetGameVar(Name, DefaultValueFactory, TypeInfo);
            OnChanged?.Invoke();
        }
    }
}