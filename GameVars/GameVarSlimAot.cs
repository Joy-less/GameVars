using System.Text.Json.Serialization.Metadata;

namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>, with no caching and no event handlers.<br/>
/// Unlike <see cref="GameVarSlim{T}"/>, <see cref="GameVarSlimAot{T}"/> supports NativeAOT and trimming due to using <see cref="JsonTypeInfo{T}"/>.
/// </summary>
public sealed class GameVarSlimAot<T> : IGameVar<T> {
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
    public GameVarSlimAot(GameVarCollection Collection, string Name, Func<T> DefaultValueFactory, JsonTypeInfo<T> TypeInfo) {
        this.Collection = Collection;
        this.Name = Name;
        this.DefaultValueFactory = DefaultValueFactory;
        this.TypeInfo = TypeInfo;
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
}