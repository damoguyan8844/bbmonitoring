using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Effects;

namespace JOYFULL.CMPW.Presentation
{
    internal static class Highlight
    {
        static List<UIElement> _elements = new List<UIElement>();
        static UIElement _enabled;
        const double OPACITY_NORMAL = 0.8;
        const double OPACITY_HIGHLIGHT = 1.0;
        

        public static void Fill( UIElement element )
        {
            if( !_elements.Contains( element ) )
            {
                _elements.Add( element );
                element.Opacity = OPACITY_NORMAL;
                element.MouseEnter += 
                    new System.Windows.Input.MouseEventHandler( element_MouseEnter );
            }
        }
        public static void Remove( UIElement element )
        {
            if( _elements.Contains( element ) )
            {
                _elements.Remove( element );
                element.Opacity = OPACITY_HIGHLIGHT;
                element.MouseEnter -= element_MouseEnter;
            }
        }
        public static void Enable( UIElement element )
        {
            _enabled = element;
            SetHighlight( element );
        }
        public static void Disable( UIElement element )
        {
            if( element == _enabled )
                _enabled = null;
            UnsetHighlight( element );
        }
        static void element_MouseEnter( object sender, System.Windows.Input.MouseEventArgs e )
        {
            UIElement element = sender as UIElement;
            SetHighlight( element );
            element.MouseLeave += 
                new System.Windows.Input.MouseEventHandler( element_MouseLeave );
        }

        static void element_MouseLeave( object sender, System.Windows.Input.MouseEventArgs e )
        {
            UIElement element = sender as UIElement;
            if( element != _enabled )
                UnsetHighlight( element );
            element.MouseLeave -= element_MouseLeave;
        }

        static void SetHighlight( UIElement element )
        {
            element.SetOpacityThreadSafe( OPACITY_HIGHLIGHT );
            element.SetDropShadowBitmapEffectThreadSafe( 0.8 );
        }

        static void UnsetHighlight( UIElement element )
        {
            element.SetOpacityThreadSafe( OPACITY_NORMAL );
            element.UnSetDropShadowBitmapEffectThreadSafe();

        }
    }
}
