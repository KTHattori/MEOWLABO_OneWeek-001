using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    private PropAction action = null;
    private bool isHold = false;

    public bool IsHold { get { return isHold; } }


    [System.Serializable]
    public class RendererData
    {
        public Renderer renderer;
        public Material[] materials;

        public RendererData(Renderer renderer, Material[] materials)
        {
            this.renderer = renderer;
            this.materials = materials;
        }

        public void SetLayerMask(int layerMask)
        {
            renderer.gameObject.layer = layerMask;
        }
        public void SetMaterialProperty(string propertyName, float value)
        {
            foreach(var material in materials)
            {
                material.SetFloat(propertyName, value);
            }
        }

        public void SetMaterialProperty(string propertyName, bool value)
        {
            foreach(var material in materials)
            {
                material.SetInt(propertyName, value ? 1 : 0);
            }
        }

        public void ReleaseMaterials()
        {
            foreach(var material in materials)
            {
                Destroy(material);
            }
        }
    }

    [SerializeField]
    List<RendererData> rendererData = new List<RendererData>();

    void Reset()
    {
        for(var i = 0;i < rendererData.Count;i++)
        {
            rendererData[i].ReleaseMaterials();
        }
        rendererData.Clear();
        var renderers = GetComponentsInChildren<Renderer>();
        foreach(var renderer in renderers)
        {
            rendererData.Add(new RendererData(renderer, new Material[renderer.sharedMaterials.Length]));
        }
        action = GetComponent<PropAction>();
    }

    void Start()
    {
        if(rendererData.Count != GetComponentsInChildren<Renderer>().Length)
        {
            Reset();
        }

        action = GetComponent<PropAction>();

        // Fetch renderer data
        for(int i = 0;i < rendererData.Count;i++)
        {
            rendererData[i].materials = new Material[rendererData[i].renderer.sharedMaterials.Length];
            rendererData[i].materials = rendererData[i].renderer.materials;
        }
    }

    void Update()
    {

    }

    public void SetHighlighted(bool flag)
    {
        SetLayerMask(flag);
        SetMaterialProperty(PropManager.instance.MaterialPropertyName, flag);
    }

    void SetLayerMask(bool flag)
    {
        for(int i = 0;i < rendererData.Count;i++)
        {
            if(flag)
            {
                rendererData[i].SetLayerMask(LayerMask.NameToLayer(PropManager.instance.HighlightedPropLayerName));
            }
            else
            {
                if(PropManager.IsProp(rendererData[i].renderer.gameObject))
                {
                    rendererData[i].SetLayerMask(LayerMask.NameToLayer(PropManager.instance.PropLayerName));
                }
                else if(PropManager.IsStaticProp(rendererData[i].renderer.gameObject))
                {
                    rendererData[i].SetLayerMask(LayerMask.NameToLayer(PropManager.instance.StaticPropLayerName));
                }
            }
        }
    }

    public void SetHold(bool flag)
    {
        isHold = flag;
    }

    public void SetLayerHold(bool flag)
    {
        for(int i = 0;i < rendererData.Count;i++)
        {
            if(flag)
            {
                Debug.Log(PropManager.instance.HoldingPropLayerName);
                rendererData[i].SetLayerMask(LayerMask.NameToLayer(PropManager.instance.HoldingPropLayerName));
            }
            else
            {
                if(PropManager.IsProp(rendererData[i].renderer.gameObject))
                {
                    rendererData[i].SetLayerMask(LayerMask.NameToLayer(PropManager.instance.PropLayerName));
                }
                else if(PropManager.IsStaticProp(rendererData[i].renderer.gameObject))
                {
                    rendererData[i].SetLayerMask(LayerMask.NameToLayer(PropManager.instance.StaticPropLayerName));
                }
            }
        }
    }

    void SetMaterialProperty(string propertyName, float value)
    {
        for(int i = 0;i < rendererData.Count;i++)
        {
            for(int j = 0;j < rendererData[i].materials.Length;j++)
            {
                if(rendererData[i].materials[j].HasProperty(propertyName)) rendererData[i].materials[j].SetFloat(propertyName, value);
            }
            rendererData[i].renderer.sharedMaterials = rendererData[i].materials;
        }
    }

    void SetMaterialProperty(string propertyName, bool flag)
    {
        for(int i = 0;i < rendererData.Count;i++)
        {
            for(int j = 0;j < rendererData[i].materials.Length;j++)
            {
                if(rendererData[i].materials[j].HasProperty(propertyName)) rendererData[i].materials[j].SetFloat(propertyName, flag ? 1f : 0f);
            }
            rendererData[i].renderer.sharedMaterials = rendererData[i].materials;
        }
    }

    private void OnDestroy()
    {
        for(var i = 0;i < rendererData.Count;i++)
        {
            rendererData[i].ReleaseMaterials();
        }
        rendererData.Clear();
    }
}
