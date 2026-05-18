#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

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
    /// Metadata about the type of the game var.
    /// </summary>
    public JsonTypeInfo<T> TypeInfo { get; }

    /// <summary>
    /// Creates a new interface to a game var in a <see cref="GameVarCollection"/>.
    /// </summary>
    public GameVarSlim(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory, JsonTypeInfo<T> TypeInfo) {
        this.Collection = Collection;
        this.Name = Name;
        this.DefaultValueFactory = DefaultValueFactory;
        this.TypeInfo = TypeInfo;
    }
    /// <inheritdoc cref="GameVar{T}(GameVarCollection, string, Func{T}, JsonTypeInfo{T})"/>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public GameVarSlim(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory)
        : this(Collection, Name, DefaultValueFactory, JsonTypeInfo.CreateJsonTypeInfo<T>(GameVarCollection.DefaultJsonOptions)) {
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <see cref="DefaultValueFactory"/>.
    /// </summary>
    public T Get() {
        return Collection.GetGameVar(Name, DefaultValueFactory, TypeInfo);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void Set(T Value) {
        Collection.SetGameVar(Name, Value, TypeInfo);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void Set(T Value, JsonTypeInfo<T> TypeInfo) {
        Collection.SetGameVar(Name, Value, TypeInfo);
    }
}