using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.Resources;
using Assimp;
using Bonsai.Shaders.Configuration;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class SceneResources : ResourceLoader
{
    [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
    public string FileName { get; set; }

    protected override IEnumerable<IResourceConfiguration> GetResources()
    {
        using (var context = new AssimpContext())
        {
            var scene = context.ImportFile(FileName);
            foreach (var mesh in scene.Meshes)
            {
                yield return new AssimpMeshConfiguration { Name = mesh.Name, Mesh = mesh };
            }

            foreach (var texture in scene.Textures)
            {
            }

            foreach (var camera in scene.Cameras)
            {
            }

            // yield return new AssimpNodeConfiguration { Name = scene.RootNode.Name };
        }
    }
}

public class AssimpMeshConfiguration : MeshConfiguration
{
    public Assimp.Mesh Mesh { get; set; }

    public override Bonsai.Shaders.Mesh CreateResource(ResourceManager resourceManager)
    {
        // convert to bonsai mesh
        return base.CreateResource(resourceManager);
    }
}

// public class AssimpNodeConfiguration : ResourceConfiguration<Node>
// {
//     public override Node CreateResource(ResourceManager resourceManager)
//     {
//         throw new NotImplementedException();
//     }
// }