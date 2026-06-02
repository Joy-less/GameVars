using System.Text.Json;
using System.Text.Json.Nodes;

namespace GameVars.Tests;

public class UnitTest1 {
    [Fact]
    public void ReadmeTest() {
        GameVarCollection Collection = new();
        Collection.Set("cat", "meow");
        Collection.Set("dog", "woof");
        Collection.Set("animals", 2);
        JsonSerializer.Serialize(Collection.GetAll()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
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
        Collection.Get("bird", () => "").ShouldBe("chirp");
        Animals.Increment();
        Animals.Get().ShouldBe(3);

        GameVar<HashSet<int>> Numbers = new(Collection, "numbers", () => [1, 2, 3]);
        Numbers.Get().ShouldBe([1, 2, 3]);
        Numbers.Set([1]);
        Numbers.Get().ShouldBe([1]);
    }
    [Fact]
    public void GameVarCollectionGetGameVarsTest() {
        GameVarCollection Collection = new();
        Collection.GetAll().ShouldBe([]);
    }
    [Fact]
    public void GameVarCollectionGetGameVarTest() {
        GameVarCollection Collection = new();
        Collection.Get("a", 5).ShouldBe(5);
        Collection.Get("a", () => 5).ShouldBe(5);
    }
    [Fact]
    public void GameVarCollectionSetGameVarTest() {
        GameVarCollection Collection = new();
        Collection.Set("a", 3);
        Collection.Get("a", 5).ShouldBe(3);
        Collection.Get("a", () => 5).ShouldBe(3);
    }
    [Fact]
    public void GameVarCollectionSetGameVarsTest() {
        GameVarCollection Collection = new();
        Collection.SetAll(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        JsonSerializer.Serialize(Collection.GetAll()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        }));
        Collection.Get("a", 7).ShouldBe(1);
        Collection.Get("b", 7).ShouldBe(2);
    }
    [Fact]
    public void GameVarCollectionCloneTest() {
        GameVarCollection Collection = new(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
            ["c"] = new JsonObject() {
                ["d"] = true,
            },
        });
        GameVarCollection Collection2 = Collection.Clone();
        JsonSerializer.Serialize(Collection2.GetAll()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
            ["c"] = new JsonObject() {
                ["d"] = true,
            },
        }));
    }
    [Fact]
    public void GameVarCollectionSerializeDeserializeTest() {
        GameVarCollection Collection = new(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
            ["c"] = new JsonObject() {
                ["d"] = true,
            },
        });
        Collection.Serialize().ShouldBe(JsonSerializer.Serialize(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
            ["c"] = new JsonObject() {
                ["d"] = true,
            },
        }));
        JsonSerializer.Serialize(GameVarCollection.Deserialize(Collection.Serialize()).GetAll()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
            ["c"] = new JsonObject() {
                ["d"] = true,
            },
        }));
    }
    [Fact]
    public void GameVarGetSetTest() {
        GameVarCollection Collection = new();
        Collection.SetAll(new JsonObject() {
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
        Collection.SetAll(new JsonObject() {
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
        Collection.SetAll(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVar<int> GameVarA = new(Collection, "a", () => 0);
        Collection.Set("a", 3);
        GameVarA.Get().ShouldBe(3);
        GameVarA.Set(5);
        GameVarA.Get().ShouldBe(5);
    }
    [Fact]
    public void GameVarSlimCacheTest() {
        GameVarCollection Collection = new();
        Collection.SetAll(new JsonObject() {
            ["a"] = 1,
            ["b"] = 2,
        });
        GameVarSlim<int> GameVarSlimA = new(Collection, "a", () => 0);
        Collection.Set("a", 3);
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
        Collection.Get("a", 0).ShouldBe(2);
        GameVarA.Get().ShouldBe(2);
    }
}
