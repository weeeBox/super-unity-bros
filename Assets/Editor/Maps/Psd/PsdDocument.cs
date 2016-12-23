using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PhotoshopFile;

public delegate bool PsdLayerFilter(PsdLayer layer);

public class PsdDocument
{
    public PsdDocument(PsdFile psdFile)
    {
        width = psdFile.ColumnCount;
        height = psdFile.RowCount;
        layers = ReadLayers(psdFile);
    }

    PsdLayer[] ReadLayers(PsdFile psdFile)
    {
        List<PsdLayer> result = new List<PsdLayer>();
        Stack<PsdLayerSet> layerSetStack = new Stack<PsdLayerSet>();

        PsdLayerSet parentSet = null;
        foreach (Layer layer in psdFile.Layers)
        {
            // Get the section info for this layer
            var secInfo = layer.AdditionalInfo
                .Where(info => info.GetType() == typeof(LayerSectionInfo))
                .ToArray();

            // Section info is basically layer group info
            bool isOpen = false;
            bool isGroup = false;
            bool closeGroup = false;
            if (secInfo.Any())
            {
                foreach (var layerSecInfo in secInfo)
                {
                    LayerSectionInfo info = (LayerSectionInfo)layerSecInfo;
                    isOpen = info.SectionType == LayerSectionType.OpenFolder;
                    isGroup = info.SectionType == LayerSectionType.ClosedFolder | isOpen;
                    closeGroup = info.SectionType == LayerSectionType.SectionDivider;
                    if (isGroup || closeGroup)
                        break;
                }
            }

            if (isGroup)
            {
                PsdLayerSet layerSet = layerSetStack.Pop();
                layerSet.name = layer.Name;
                parentSet = layerSetStack.Count > 0 ? layerSetStack.Peek() : null;
            }
            else if (closeGroup)
            {
                PsdLayerSet layerSet = new PsdLayerSet(this, "", layer.Rect);
                layerSet.visible = layer.Visible;
                if (parentSet != null)
                {
                    parentSet.AddLayer(layerSet);
                }
                else
                {
                    result.Add(layerSet);
                }
                layerSetStack.Push(layerSet);
                parentSet = layerSet;
            }
            else // art layer
            {
                PsdLayer psdLayer;

                PsdShape[] shapes = layer.GetShapes();
                if (shapes != null)
                {
                    psdLayer = new PsdShapeLayer(this, layer.Name, layer.Rect, shapes);
                }
                else
                {
                    psdLayer = new PsdArtLayer(this, layer.Name, layer.Rect, layer.ReadPixels());
                }

                psdLayer.visible = layer.Visible;
                if (parentSet != null)
                {
                    parentSet.AddLayer(psdLayer);
                }
                else
                {
                    result.Add(psdLayer);
                }
            }
        }

        return result.ToArray();
    }

    public PsdLayer FindLayer(string name)
    {
        return FindLayer(delegate(PsdLayer layer) { return layer.name == name; });
    }

    public PsdLayer FindLayer(PsdLayerFilter filter)
    {
        foreach (var layer in layers)
        {
            if (filter(layer))
            {
                return layer;
            }
        }
        return null;
    }

    public PsdLayer[] layers { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }
}

public class PsdLayer
{
    public PsdLayer(PsdDocument document, string name, Rect rect)
    {
        this.document = document;
        this.name = name;
        this.rect = rect;
    }

    public PsdDocument document { get; private set; }
    public string name { get; internal set; }
    public Rect rect { get; private set; }
    public int width { get { return (int)rect.width; } }
    public int height { get { return (int)rect.height; } }
    public bool visible { get; internal set; }
}

public class PsdArtLayer : PsdLayer
{
    public PsdArtLayer(PsdDocument document, string name, Rect rect, Color32[] pixels)
        : base(document, name, rect)
    {
        this.pixels = pixels;
    }

    public Texture2D CreateTexture(TextureFormat format = TextureFormat.ARGB32, bool mipmap = false)
    {
        Texture2D texture = new Texture2D(width, height, format, mipmap);
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }

    public Color32[] pixels { get; private set; }

    public override string ToString()
    {
        return string.Format("Layer: " + name);
    }
}

public class PsdShapeLayer : PsdLayer
{
    public PsdShapeLayer(PsdDocument document, string name, Rect rect, PsdShape[] shapes)
        : base(document, name, rect)
    {
        this.shapes = shapes;
    }

    public PsdShape[] shapes { get; private set; }
}

public class PsdLayerSet : PsdLayer
{
    public PsdLayerSet(PsdDocument document, string name, Rect rect)
        : base(document, name, rect)
    {
        layers = new List<PsdLayer>();
    }

    public void AddLayer(PsdLayer layer)
    {
        layers.Add(layer);
    }

    public override string ToString()
    {
        return string.Format("Group: " + name);
    }

    public List<PsdLayer> layers { get; private set; }

    public int layerCount { get { return layers.Count; } }
}

public class PsdShape
{
    public PsdShape(Vector2[] points, bool closed)
    {
        this.points = points;
        this.closed = closed;
    }

    public bool closed { get; private set; }
    public Vector2[] points { get; private set; }
}