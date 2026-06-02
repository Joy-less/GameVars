#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using System.Text.Json;

namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>, with no caching and no event handlers.
/// </summary>
#if NET
[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
public sealed class GameVarSlim<T> : IGameVar<T> {
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
    /// Custom JSON options used when serializing/deserializing the type.
    /// </summary>
    public JsonSerializerOptions? JsonOptions { get; }

    /// <summary>
    /// Creates a new interface to a game var in a <see cref="GameVarCollection"/>.
    /// </summary>
    public GameVarSlim(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory, JsonSerializerOptions? JsonOptions = null) {
        this.Collection = Collection;
        this.Name = Name;
        this.DefaultValueFactory = DefaultValueFactory;
        this.JsonOptions = JsonOptions;
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <see cref="DefaultValueFactory"/>.
    /// </summary>
    public T Get() {
        return Collection.Get(Name, DefaultValueFactory, JsonOptions);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void Set(T Value) {
        Collection.Set(Name, Value, JsonOptions);
    }
}