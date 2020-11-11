using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Assimp;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Source)]
public class LoadScene
{
    [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
    public string FileName { get; set; }

    public IObservable<Scene> Process()
    {
        return Observable.Start(() =>
        {
            using (var context = new AssimpContext())
            {
                var scene = context.ImportFile(FileName);
                return scene;
            }
        });
    }
}
