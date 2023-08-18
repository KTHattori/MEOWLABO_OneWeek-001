using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropManager : MonoSingleton<PropManager>
{
    [SerializeField]
    private string propLayerName = "Prop";
    public string PropLayerName { get => propLayerName; }
    public LayerMask PropLayerMask { get => LayerMask.NameToLayer(propLayerName); }
    [SerializeField]
    private string staticPropLayerName = "StaticProp";
    public string StaticPropLayerName { get => staticPropLayerName; }
    public LayerMask StaticPropLayerMask { get => LayerMask.NameToLayer(staticPropLayerName); }
    [SerializeField]
    private string highlightedPropLayerName = "Highlighted";
    public string HighlightedPropLayerName { get => highlightedPropLayerName; }
    public LayerMask HighlightedPropLayerMask { get => LayerMask.NameToLayer(highlightedPropLayerName); }
    public int SelectableLayerMask { get => 1 << PropLayerMask | 1 << StaticPropLayerMask | 1 << HighlightedPropLayerMask; }
    [SerializeField]
    private string materialPropertyName = "Highlighted";
    public string MaterialPropertyName { get => materialPropertyName; }
    [SerializeField]
    private TextMeshProUGUI textField = null;
    public TextMeshProUGUI TextField { get => textField; }
    
    static private Prop highlightedProp = null;
    static public Prop HighlightedProp { get => highlightedProp; }

    public GameObject highlightedObject = null;

    public List<GameObject> propList = new List<GameObject>();
    public List<GameObject> staticPropList = new List<GameObject>();
    public Material propMaterial;
    public Material staticPropMaterial;

    [ContextMenu("Fetch By Layer")]
    public void FetchByLayer()
    {
        // レイヤーがPropに設定されているオブジェクトを取得し、リストに格納する
        propList.Clear();
        staticPropList.Clear();
        foreach(var obj in GameObjectUtility.FindGameObjectsWithLayerMask(PropLayerMask))
        {
            propList.Add(obj);
        }
        // レイヤーがStaticPropに設定されているオブジェクトを取得し、リストに格納する
        foreach(var obj in GameObjectUtility.FindGameObjectsWithLayerMask(StaticPropLayerMask))
        {
            staticPropList.Add(obj);
        }
    }

    [ContextMenu("Fetch By Tag")]
    public void FetchByTag()
    {
        // タグがPropに設定されているオブジェクトを取得し、リストに格納する
        propList.Clear();
        staticPropList.Clear();
        foreach(var obj in GameObject.FindGameObjectsWithTag("Prop"))
        {
            propList.Add(obj);
        }
        // タグがStaticPropに設定されているオブジェクトを取得し、リストに格納する
        foreach(var obj in GameObject.FindGameObjectsWithTag("StaticProp"))
        {
            staticPropList.Add(obj);
        }
    }

    [ContextMenu("Set Tag")]
    public void SetTag()
    {
        foreach(var obj in propList)
        {
            obj.tag = "Prop";
        }
        foreach(var obj in staticPropList)
        {
            obj.tag = "StaticProp";
        }
    }

    [ContextMenu("Set Layer")]
    

    [ContextMenu("AddPropComponent")]
    public void AddPropComponent()
    {
        foreach(var obj in propList)
        {
            if(obj.GetComponent<Prop>() == null)
            {
                obj.AddComponent<Prop>();
            }
        }

        foreach(var obj in staticPropList)
        {
            if(obj.GetComponent<Prop>() == null)
            {
                obj.AddComponent<Prop>();
            }
        }
    }

    [ContextMenu("RemovePropComponent")]
    public void RemovePropComponent()
    {
        foreach(var obj in propList)
        {
            if(obj.GetComponent<Prop>() != null)
            {
                DestroyImmediate(obj.GetComponent<Prop>());
            }
        }

        foreach(var obj in staticPropList)
        {
            if(obj.GetComponent<Prop>() != null)
            {
                DestroyImmediate(obj.GetComponent<Prop>());
            }
        }
    }

    [ContextMenu("AddCollider")]
    public void AddCollider()
    {
        foreach(var obj in propList)
        {
            if(obj.GetComponent<Collider>() == null && obj.GetComponent<MeshFilter>() != null)
            {
                Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                MeshCollider collider = obj.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
                collider.convex = true;
            }
        }

        foreach(var obj in staticPropList)
        {
            if(obj.GetComponent<Collider>() == null && obj.GetComponent<MeshFilter>() != null)
            {
                Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                MeshCollider collider = obj.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
                collider.convex = true;
            }
        }
    }

    [ContextMenu("RemoveCollider")]
    public void RemoveCollider()
    {
        foreach(var obj in propList)
        {
            if(obj.GetComponent<Collider>() != null)
            {
                DestroyImmediate(obj.GetComponent<Collider>());
            }
        }

        foreach(var obj in staticPropList)
        {
            if(obj.GetComponent<Collider>() != null)
            {
                DestroyImmediate(obj.GetComponent<Collider>());
            }
        }
    }

    [ContextMenu("SetMaterial")]
    public void SetMaterial()
    {
        foreach(var obj in propList)
        {
            if(obj.GetComponent<MeshRenderer>() != null)
            {
                foreach(Material mat in obj.GetComponent<MeshRenderer>().sharedMaterials)
                {
                    obj.GetComponent<MeshRenderer>().sharedMaterial = propMaterial;
                }
            }
        }

        foreach(var obj in staticPropList)
        {
            if(obj.GetComponent<MeshRenderer>() != null)
            {
                foreach(Material mat in obj.GetComponent<MeshRenderer>().sharedMaterials)
                {
                    obj.GetComponent<MeshRenderer>().sharedMaterial = staticPropMaterial;
                }
            }
        }
    }


    static public bool IsProp(GameObject obj)
    {
        return obj.CompareTag("Prop");
    }

    static public bool IsStaticProp(GameObject obj)
    {
        return obj.CompareTag("StaticProp");
    }

    static public bool IsHighlighted(GameObject obj)
    {
        return obj.layer == LayerMask.NameToLayer(instance.HighlightedPropLayerName);
    }

    static public void UpdateHighlighted(GameObject obj)
    {
        if(obj == null)
        {
            if(highlightedProp != null)
            {
                highlightedProp.SetHighlighted(false);
                highlightedProp = null;
            }
            else
            {
                return;
            }
        }
        else if(highlightedProp == null)
        {
            // --- new highlighted prop ---
            highlightedProp = obj.GetComponent<Prop>();
            highlightedProp.gameObject.layer = LayerMask.NameToLayer(instance.HighlightedPropLayerName);
            highlightedProp.SetHighlighted(true);
        }
        else if(highlightedProp.gameObject != obj)
        {
            // --- change highlighted prop ---
            highlightedProp.SetHighlighted(false);
            highlightedProp = obj.GetComponent<Prop>();
            highlightedProp.gameObject.layer = LayerMask.NameToLayer(instance.HighlightedPropLayerName);
            highlightedProp.SetHighlighted(true);
        }

        instance.highlightedObject = obj;
    }
}