﻿using System;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace Avalonia.Rendering
{
    public class RenderLayer
    {
        private readonly IRenderLayerFactory _factory;

        public RenderLayer(
            IRenderLayerFactory factory,
            Size size,
            IVisual layerRoot)
        {
            _factory = factory;
            Bitmap = factory.CreateLayer(layerRoot, size);
            Size = size;
            LayerRoot = layerRoot;
            Order = GetDistanceFromRenderRoot(layerRoot);
        }

        public IRenderTargetBitmapImpl Bitmap { get; private set; }
        public Size Size { get; private set; }
        public IVisual LayerRoot { get; }
        public int Order { get; }

        public void ResizeBitmap(Size size)
        {
            if (Size != size)
            {
                var resized = _factory.CreateLayer(LayerRoot, size);

                using (var context = resized.CreateDrawingContext())
                {
                    context.Clear(Colors.Black);
                    context.DrawImage(Bitmap, 1, new Rect(Size), new Rect(Size));
                    Bitmap.Dispose();
                    Bitmap = resized;
                    Size = size;
                }
            }
        }

        private static int GetDistanceFromRenderRoot(IVisual visual)
        {
            var root = visual as IRenderRoot;
            var result = 0;

            while (root == null)
            {
                ++result;
                visual = visual.VisualParent;

                if (visual == null)
                {
                    throw new AvaloniaInternalException("Visual is not rooted.");
                }

                root = visual as IRenderRoot;
            }

            return result;
        }
    }
}
