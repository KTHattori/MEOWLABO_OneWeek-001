using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField]
    private bool isInteractable = false;
    public bool IsInteractable { get => isInteractable; }

    Renderer _renderer = null;
    Material copiedMaterial = null;

    void Reset()
    {
        _renderer = GetComponent<Renderer>();
        copiedMaterial = null;
    }

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        copiedMaterial = null;
    }

    public void SetHighlighted(bool flag)
    {
        SetLayerMask(flag);
        SetMaterialProperty(PropManager.instance.MaterialPropertyName, flag);
    }

    void SetLayerMask(bool flag)
    {
        if(flag)
        {
            gameObject.layer = LayerMask.NameToLayer(PropManager.instance.HighlightedPropLayerName);
        }
        else
        {
            if(PropManager.IsProp(gameObject))
            {
                gameObject.layer = LayerMask.NameToLayer(PropManager.instance.PropLayerName);
            }
            else if(PropManager.IsStaticProp(gameObject))
            {
                gameObject.layer = LayerMask.NameToLayer(PropManager.instance.StaticPropLayerName);
            }
        }
    }

    void SetMaterialProperty(string propertyName, float value)
    {
        // Check if the material has the property
        if(_renderer.sharedMaterial.HasProperty(propertyName))
        {
            // Destroy the copied material if it exists
            if(copiedMaterial != null)
            {
                Destroy(copiedMaterial);
                copiedMaterial = null;
            }

            // Create a copy of the material
            copiedMaterial = Instantiate(_renderer.sharedMaterial);

            // Set the property
            copiedMaterial.SetFloat(propertyName, value);
        }
    }

    void SetMaterialProperty(string propertyName, bool flag)
    {
        // Check if the material has the property
        if(_renderer.sharedMaterial.HasProperty(propertyName))
        {
            // Destroy the copied material if it exists
            if(copiedMaterial != null)
            {
                Destroy(copiedMaterial);
                copiedMaterial = null;
            }

            // Create a copy of the material
            copiedMaterial = Instantiate(_renderer.sharedMaterial);

            // Set the property
            copiedMaterial.SetFloat(propertyName, flag ? 1.0f : 0.0f);

            // Set the material
            _renderer.sharedMaterial = copiedMaterial;
        }
    }

    private void OnDestroy()
    {
        if (copiedMaterial != null)
        {
            Destroy(copiedMaterial);
            copiedMaterial = null;
        }
    }
}
