using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoshopFile
{
    public static class PsdFileExtensions
    {
        #region Layer

        public static Color32[] ReadPixels(this Layer layer)
        {
            if ((int)layer.Rect.width == 0 || (int)layer.Rect.height == 0)
            {
                return null;
            }

            int width = (int)layer.Rect.width;
            int height = (int)layer.Rect.height;

            Color32[] pixels = new Color32[width * height];

            Channel red = (from l in layer.Channels
                where l.ID == 0
                select l).First();
            Channel green = (from l in layer.Channels
                where l.ID == 1
                select l).First();
            Channel blue = (from l in layer.Channels
                where l.ID == 2
                select l).First();
            Channel alpha = layer.AlphaChannel;

            for (int i = 0; i < pixels.Length; i++)
            {
                byte r = red.ImageData[i];
                byte g = green.ImageData[i];
                byte b = blue.ImageData[i];
                byte a = 255;

                if (alpha != null)
                {
                    a = alpha.ImageData[i];
                }

                int mod = i % width;
                int n = ((width - mod - 1) + i) - mod;
                pixels[pixels.Length - n - 1] = new Color32(r, g, b, a);
            }

            return pixels;
        }

        public static VectorMaskSetting GetVectorMaskSetting(this Layer layer)
        {
            foreach (var layerInfo in layer.AdditionalInfo)
            {
                VectorMaskSetting vectorMaskSetting = layerInfo as VectorMaskSetting;
                if (vectorMaskSetting != null)
                {
                    return vectorMaskSetting;
                }
            }

            return null;
        }

        public static bool HasVectorMask(this Layer layer)
        {
            return layer.GetVectorMaskSetting() != null;
        }

        public static PsdShape[] GetShapes(this Layer layer)
        {
            VectorMaskSetting setting = layer.GetVectorMaskSetting();
            if (setting == null)
            {
                return null;
            }

            List<PsdShape> result = new List<PsdShape>();

            Vector2[] points = null;
            int pointIndex = 0;
            bool isClosed = false;

            foreach (var record in setting.PathDataRecords)
            {
                if (record is SubpathLengthRecord)
                {
                    SubpathLengthRecord lengthRecord = record as SubpathLengthRecord;
                    if (points != null)
                    {
                        throw new PsdInvalidException("Duplicate subpath length record");
                    }

                    points = new Vector2[lengthRecord.BezierKnotCount];
                    pointIndex = 0;
                    isClosed = lengthRecord.IsClosed;
                }
                else if (record is BezierKnotRecord)
                {
                    BezierKnotRecord knowRecord = record as BezierKnotRecord;
                    points[pointIndex++] = new Vector2(knowRecord.AnchorPoint.Horizontal, knowRecord.AnchorPoint.Vertical);
                    if (pointIndex == points.Length)
                    {
                        result.Add(new PsdShape(points, isClosed));
                        points = null;
                        pointIndex = 0;
                    }
                }
            }
            return result.ToArray();
        }

        #endregion
    }
}
