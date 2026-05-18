#if NET
using System.Diagnostics.CodeAnalysis;
#endif

namespace GameVars;

/// <summary>
/// An interface to a game var in a <see cref="GameVarCollection"/>.
/// </summary>
public interface IGameVar<T> {
    /// <summary>
    /// The collection of game vars containing the game var.
    /// </summary>
    public GameVarCollection Collection { get; }
    /// <summary>
    /// The name (key) of the game var.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the value of the game var, or a default value.
    /// </summary>
    public T Get();
    /// <summary>
    /// Sets the value of the game var.
    /// </summary>
    public void Set(T Value);
}