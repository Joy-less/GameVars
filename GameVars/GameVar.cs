#if NET
using System.Diagnostics.CodeAnalysis;
#endif

namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>, with caching and event handlers.
/// </summary>
#if NET
[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
public sealed class GameVar<T> : IGameVar<T>, IDisposable {
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
    public GameVar(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory) {
        this.Collection = Collection;
        this.Name = Name;
        this.DefaultValueFactory = DefaultValueFactory;

        CachedValue = Collection.Get(Name, DefaultValueFactory);

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

        Collection.Set(Name, Value);
    }

    private void OnGameVarChangedHandler(string GameVarName) {
        if (GameVarName == Name) {
            CachedValue = Collection.Get(Name, DefaultValueFactory);
            OnChanged?.Invoke();
        }
    }
}