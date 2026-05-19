using System.Text.Json.Serialization.Metadata;

namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>, with caching and event handlers.<br/>
/// Unlike <see cref="GameVar{T}"/>, <see cref="GameVarAot{T}"/> supports NativeAOT and trimming due to using <see cref="JsonTypeInfo{T}"/>.
/// </summary>
public sealed class GameVarAot<T> : IGameVar<T>, IDisposable {
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

        CachedValue = Collection.Get(Name, DefaultValueFactory, TypeInfo);

        Collection.OnGameVarChanged += OnGameVarChangedHandler;
    }
    /// <summary>
    /// Clears event handlers from the collection.<br/>
    /// Note: You don't need to call this method if you are about to discard the collection,
    /// since the event handlers will be discarded along with the collection.
    /// </summary>
    public void Dispose() {
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

        Collection.Set(Name, Value, TypeInfo);
    }

    private void OnGameVarChangedHandler(string GameVarName) {
        if (GameVarName == Name) {
            CachedValue = Collection.Get(Name, DefaultValueFactory, TypeInfo);
            OnChanged?.Invoke();
        }
    }
}