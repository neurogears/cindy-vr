using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Assimp;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Combinator)]
public class SceneIterator
{
    IEnumerable<Node> GetNodes(Node root)
    {
        yield return root;
        foreach (var child in root.Children)
        {
            foreach (var node in GetNodes(child))
            {
                yield return node;
            }
        }
    }

    public IObservable<Node> Process(IObservable<Scene> source)
    {
        return source.SelectMany(scene => GetNodes(scene.RootNode));
    }
}
