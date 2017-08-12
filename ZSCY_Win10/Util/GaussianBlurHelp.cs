using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace ZSCY_Win10.Util
{
    class GaussianBlurHelp
    {
        public static void InitializeBlur(UIElement uiElement)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(uiElement);
            Compositor compositor = hostVisual.Compositor;
            string name = "backdropBrush";
            GaussianBlurEffect gausianBlur = new GaussianBlurEffect()
            {
                BlurAmount = 30f,
                BorderMode = EffectBorderMode.Hard,
                Source = new ArithmeticCompositeEffect()
                {
                    MultiplyAmount = 0,
                    Source1Amount = 1f,
                    Source1 = new CompositionEffectSourceParameter(name),
                    Source2Amount = 0f,
                    Source2 = new ColorSourceEffect() { Color = Color.FromArgb(255, 245, 245, 245) }
                }
            };
            CompositionEffectFactory effectFactory = compositor.CreateEffectFactory(gausianBlur);
            CompositionBackdropBrush backdropBrush = compositor.CreateBackdropBrush();
            CompositionEffectBrush effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter(name, backdropBrush);
            SpriteVisual glassVisual = compositor.CreateSpriteVisual();
            glassVisual.Brush = effectBrush;
            ElementCompositionPreview.SetElementChildVisual(uiElement, glassVisual);
            ExpressionAnimation bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            glassVisual.StartAnimation("Size", bindSizeAnimation);

            effectFactory.Dispose();
            gausianBlur.Dispose();
            backdropBrush.Dispose();

        }
    }
}
