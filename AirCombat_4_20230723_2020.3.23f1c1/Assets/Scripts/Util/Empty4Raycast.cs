using UnityEngine.UI;


/// <summary>

/// </summary>
public class Empty4Raycast : MaskableGraphic
{
    public Empty4Raycast()
    {
        useLegacyMeshGeneration = false;
    }

    protected override void OnPopulateMesh(VertexHelper vp)
    {
        vp.Clear();
    }
}