using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField]
    private bool isInteractable = false;
    public bool IsInteractable { get => isInteractable; }

    [SerializeField]
    private bool hasDialogue = false;
    public bool HasDialogue { get => hasDialogue; }

    [SerializeField]
    List<Renderer> renderers = new List<Renderer>();
    [SerializeField]
    List<Material> copiedMaterials = new List<Material>();

    void Reset()
    {
        renderers.Clear();
        copiedMaterials.Clear();
        renderers.AddRange(GetComponentsInChildren<Renderer>());
        foreach(var renderer in renderers)
        {
            int mats = renderer.sharedMaterials.Length;
            for(int i = 0;i < mats;i++)
            {
                copiedMaterials.Add(null);
            }
        }
    }

    void Start()
    {
        renderers.Clear();
        copiedMaterials.Clear();
        renderers.AddRange(GetComponentsInChildren<Renderer>());
        foreach(var renderer in renderers)
        {
            int mats = renderer.sharedMaterials.Length;
            for(int i = 0;i < mats;i++)
            {
                copiedMaterials.Add(null);
            }
        }
    }

    public void SetHighlighted(bool flag)
    {
        SetLayerMask(flag);
        SetMaterialProperty(PropManager.instance.MaterialPropertyName, flag);
    }

    void SetLayerMask(bool flag)
    {
        foreach(var renderer in renderers)
        {
            if(flag)
            {
                renderer.gameObject.layer = LayerMask.NameToLayer(PropManager.instance.HighlightedPropLayerName);
            }
            else
            {
                if(PropManager.IsProp(renderer.gameObject))
                {
                    renderer.gameObject.layer = LayerMask.NameToLayer(PropManager.instance.PropLayerName);
                }
                else if(PropManager.IsStaticProp(renderer.gameObject))
                {
                    renderer.gameObject.layer = LayerMask.NameToLayer(PropManager.instance.StaticPropLayerName);
                }
            }
        }
    }

    void SetMaterialProperty(string propertyName, float value)
    {
        // Check if the material has the property
        for(int i = 0;i < renderers.Count;i++)
        {
            if(renderers[i].sharedMaterial.HasProperty(propertyName))
            {
                // Destroy the copied material if it exists
                if(copiedMaterials[i] != null)
                {
                    Destroy(copiedMaterials[i]);
                    copiedMaterials[i] = null;
                }

                // Create a copy of the material
                copiedMaterials[i] = Instantiate(renderers[i].sharedMaterial);

                // Set the property
                copiedMaterials[i].SetFloat(propertyName, value);

                // Set the material
                renderers[i].sharedMaterial = copiedMaterials[i];
            }
        }
    }

    void SetMaterialProperty(string propertyName, bool flag)
    {
        // Check if the material has the property
        for(int i = 0;i < renderers.Count;i++)
        {
            if(renderers[i].sharedMaterial.HasProperty(propertyName))
            {
                // Destroy the copied material if it exists
                if(copiedMaterials[i] != null)
                {
                    Destroy(copiedMaterials[i]);
                    copiedMaterials[i] = null;
                }

                // Create a copy of the material
                copiedMaterials[i] = Instantiate(renderers[i].sharedMaterial);

                // Set the property
                copiedMaterials[i].SetFloat(propertyName, flag ? 1.0f : 0.0f);

                // Set the material
                renderers[i].sharedMaterial = copiedMaterials[i];
            }
        }
    }

    private void OnDestroy()
    {
        foreach(var material in copiedMaterials)
        {
            if(material != null)
            {
                Destroy(material);
            }
        }
    }
}
