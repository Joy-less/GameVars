using System.Text.Json;
using System.Text.Json.Nodes;

namespace GameVars.Tests;

public class UnitTest1 {
    [Fact]
    public void ReadmeTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVar("cat", "meow");
        Collection.SetGameVar("dog", "woof");
        Collection.SetGameVar("animals", 2);
        JsonSerializer.Serialize(Collection.GetGameVars()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
            ["cat"] = "meow",
            ["dog"] = "woof",
            ["animals"] = 2,
        }));

        GameVar<string> Cat = new(Collection, "cat", () => "");
        Cat.Get().ShouldBe("meow");
        Cat.Set("nyan");
        Cat.Get().ShouldBe("nyan");

        GameVarSlim<int> Animals = new(Collection, "animals", () => 0);
        Animals.Get().ShouldBe(2);

        GameVar<string> Bird = new(Collection, "bird", () => "");
        Bird.Set("chirp");
        Collection.GetGameVar("bird", () => "").ShouldBe("chirp");
        Animals.Increment();
        Animals.Get().ShouldBe(3);
    }
    [Fact]
    public void GameVarCollectionGetGameVarsTest() {
        GameVarCollection Collection = new();
        Collection.GetGameVars().ShouldBe([]);
    }
    [Fact]
    public void GameVarCollectionGetGameVarTest() {
        GameVarCollection Collection = new();
        Collection.GetGameVar("a", 5).ShouldBe(5);
        Collection.GetGameVar("a", () => 5).ShouldBe(5);
    }
    [Fact]
    public void GameVarCollectionSetGameVarTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVar("a", 3);
        Collection.GetGameVar("a", 5).ShouldBe(3);
        Collection.GetGameVar("a", () => 5).ShouldBe(3);
    }
    [Fact]
    public void GameVarCollectionSetGameVarsTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVars(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        JsonSerializer.Serialize(Collection.GetGameVars()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        }));
        Collection.GetGameVar("a", 7).ShouldBe(1);
        Collection.GetGameVar("b", 7).ShouldBe(2);
    }
    [Fact]
    public void GameVarGetSetTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVars(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVar<int> GameVarA = new(Collection, "a", () => 0);
        GameVarA.Get().ShouldBe(1);
        GameVarA.Set(3);
        GameVarA.Get().ShouldBe(3);
    }
    [Fact]
    public void GameVarSlimGetSetTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVars(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVarSlim<int> GameVarSlimA = new(Collection, "a", () => 0);
        GameVarSlimA.Get().ShouldBe(1);
        GameVarSlimA.Set(3);
        GameVarSlimA.Get().ShouldBe(3);
    }
    [Fact]
    public void GameVarCacheTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVars(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVar<int> GameVarA = new(Collection, "a", () => 0);
        Collection.SetGameVar("a", 3);
        GameVarA.Get().ShouldBe(3);
        GameVarA.Set(5);
        GameVarA.Get().ShouldBe(5);
    }
    [Fact]
    public void GameVarSlimCacheTest() {
        GameVarCollection Collection = new();
        Collection.SetGameVars(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVarSlim<int> GameVarSlimA = new(Collection, "a", () => 0);
        Collection.SetGameVar("a", 3);
        GameVarSlimA.Get().ShouldBe(3);
        GameVarSlimA.Set(5);
        GameVarSlimA.Get().ShouldBe(5);
    }
    [Fact]
    public void GameVarIncrementTest() {
        GameVarCollection Collection = new(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVar<int> GameVarA = new(Collection, "a", () => 0);
        GameVarA.Increment();
        Collection.GetGameVar("a", 0).ShouldBe(2);
        GameVarA.Get().ShouldBe(2);
    }
}
