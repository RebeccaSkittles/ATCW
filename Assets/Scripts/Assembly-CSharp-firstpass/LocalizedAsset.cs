using UnityEngine;
using UnityEngine.UI; // Add this line

public class LocalizedAsset : MonoBehaviour
{
    public Object localizeTarget;

    public void Awake()
    {
        LocalizeAsset(localizeTarget);
    }

    public void LocalizeAsset()
    {
        LocalizeAsset(localizeTarget);
    }

    public static void LocalizeAsset(Object target)
    {
        if (target == null)
        {
            return;
        }

        if (target is Image)
        {
            Image image = (Image)target;
            if (image.sprite != null)
            {
                Sprite sprite = Language.GetAsset(image.sprite.name) as Sprite;
                if (sprite != null)
                {
                    image.sprite = sprite;
                }
            }
        }
        else if (target is Material)
        {
            Material material = (Material)target;
            if (material.mainTexture != null)
            {
                Texture texture = Language.GetAsset(material.mainTexture.name) as Texture;
                if (texture != null)
                {
                    material.mainTexture = texture;
                }
            }
        }
        else if (target is MeshRenderer)
        {
            MeshRenderer meshRenderer = (MeshRenderer)target;
            if (meshRenderer.material.mainTexture != null)
            {
                Texture texture = Language.GetAsset(meshRenderer.material.mainTexture.name) as Texture;
                if (texture != null)
                {
                    meshRenderer.material.mainTexture = texture;
                }
            }
        }
    }
}
