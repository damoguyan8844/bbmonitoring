using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Drawing.Imaging;


namespace JOYFULL.CMPW.Presentation
{
    internal static class ThreadingExtensions
    {
        static readonly log4net.ILog log =
          log4net.LogManager.GetLogger(typeof(ThreadingExtensions));

        /// <summary>
        /// Simple helper extension method to marshall to correct
        /// thread if its required
        /// </summary>
        /// <param name="control">The source control</param>
        /// <param name="methodcall">The method to call</param>
        /// <param name="priorityForCall">The thread priority</param>
        public static void InvokeIfRequired(this DispatcherObject control, 
            Action methodcall, DispatcherPriority priorityForCall)
        {
            //see if we need to Invoke call to Dispatcher thread  
            if (control.Dispatcher.Thread != Thread.CurrentThread)
                control.Dispatcher.Invoke(priorityForCall, methodcall);
            else
                methodcall();
        }

        /// <summary>
        /// Gets the Visibility of a UIElement in a thread safe way
        /// </summary>
        /// <param name="uie">The UIElement</param>
        /// <returns>The Visibility of the UIElement</returns>
        public static Visibility GetVisibilityThreadSafe(this UIElement uie)
        {
            Visibility vis = Visibility.Hidden;
            InvokeIfRequired(uie, () => { vis = uie.Visibility; }, DispatcherPriority.Background);
            return vis;
        }

        /// <summary>
        /// Sets the Visibility of a UIElement in a thread safe way
        /// </summary>
        /// <param name="uie">The UIElement</param>
        /// <param name="vis">The required Visibility</param>
        public static void SetVisibilityThreadSafe(this UIElement uie, Visibility vis)
        {
            InvokeIfRequired(uie, () => { uie.Visibility = vis; }, DispatcherPriority.Background);
        }        

        /// <summary>
        /// Sets the IsEnabled value in a thread safe way
        /// </summary>
        /// <param name="uie">Thr UIElement</param>
        /// <param name="enabled">The value for the IsEnabled property</param>
        public static void SetIsEnabledThreadSafe(this UIElement uie, bool enabled)
        {
            InvokeIfRequired(uie, () => { uie.IsEnabled = enabled; }, DispatcherPriority.Background);
        }

        /// <summary>
        /// Sets the text of a TextBlock in a thread safe way
        /// </summary>
        /// <param name="tb">The TextBlock</param>
        /// <param name="s">The string for the Text property</param>
        public static void SetTextThreadSafe(this TextBlock tb, string s)
        {
            InvokeIfRequired(tb, () => { tb.Text = s; }, DispatcherPriority.Background);
        }

        public static string GetTextThreadSafe( this TextBlock tb )
        {
            string s = string.Empty;
            InvokeIfRequired( tb, () => { s = tb.Text; }, DispatcherPriority.Background );
            return s;
        }
       
        /// <summary>
        /// Gets the Text of a TextBox in a thread safe way
        /// </summary>
        /// <param name="tb">The TextBox</param>
        /// <returns>A string containing the Text of the TextBox</returns>
        public static string SetTextThreadSafe(this TextBox tb)
        {
            string s = "";
            InvokeIfRequired(tb, () => { s = tb.Text; }, DispatcherPriority.Background);
            return s;
        }

        /// <summary>
        /// Sets the text of a TextBox in a thread safe way
        /// </summary>
        /// <param name="tb">The TextBox</param>
        /// <param name="s">The string for the Text property</param>
        public static void SetTextThreadSafe(this TextBox tb, string s)
        {
            InvokeIfRequired(tb, () => { tb.Text = s; }, DispatcherPriority.Background);
        }

        /// <summary>
        /// Gets the Content of a ContentControl in a thread safe way
        /// </summary>
        /// <param name="cc">The ContentControl</param>
        /// <returns>A string containing the Content of the ContentControl</returns>
        public static string GetContentThreadSafe(this ContentControl cc)
        {
            string s = "";
            InvokeIfRequired(cc, () => { s = cc.Content.ToString(); }, DispatcherPriority.Background);
            return s;
        }

        /// <summary>
        /// Sets the Content of a ContentControl in a thread safe way
        /// </summary>
        /// <param name="cc">The ContentControl</param>
        /// <param name="s">The string to use as the Content of the ContentControl</param>
        public static void SetContentThreadSafe(this ContentControl cc, string s)
        {
            InvokeIfRequired(cc, () => { cc.Content = s; }, DispatcherPriority.Background);
        }

        /// <summary>
        /// Gets the value of the IsChecked property of a CheckBox in a thread safe way
        /// </summary>
        /// <param name="cb">The CheckBox</param>
        /// <returns>A bool containing the value if the IsChecked property</returns>
        public static bool GetIsCheckedThreadSafe(this CheckBox cb)
        {
            bool? val = null;
            InvokeIfRequired(cb, () => { val = cb.IsChecked; }, DispatcherPriority.Background);
            return (bool)val;
        }

        /// <summary>
        /// Gets the value of the IsChecked property of a RadioButton in a thread safe way
        /// </summary>
        /// <param name="rb">The RadioButton</param>
        /// <returns>A bool containing the value if the IsChecked property</returns>
        public static bool GetIsCheckedThreadSafe(this RadioButton rb)
        {
            bool? val = null;
            InvokeIfRequired(rb, () => { val = rb.IsChecked; }, DispatcherPriority.Background);
            return (bool)val;
        }

        /// <summary>
        /// Sets the value of the IsChecked property of a RadioButton in a thread safe way
        /// </summary>
        /// <param name="rb">The RadioButton</param>
        /// <param name="val">The value to set the IsChecked property to</param>
        public static void SetIsCheckedThreadSafe(this RadioButton rb, bool? val)
        {
            InvokeIfRequired(rb, () => { rb.IsChecked = val; }, DispatcherPriority.Background);
        }

        /// <summary>
        /// Sets the value of the IsChecked property of a CheckBox in a thread safe way
        /// </summary>
        /// <param name="cb">The CheckBox</param>
        /// <param name="val">The value to set the IsChecked property to</param>
        public static void SetIsCheckedThreadSafe(this CheckBox cb, bool? val)
        {
            InvokeIfRequired(cb, () => { cb.IsChecked = val; }, DispatcherPriority.Background);
        }        

        /// <summary>
        /// Gets the Value property of a ProgressBar in a thread safe way
        /// </summary>
        /// <param name="pb">The ProgressBar</param>
        /// <returns>a double containing the Value property of the ProgressBar</returns>
        public static double GetValueThreadSafe(this ProgressBar pb)
        {
            double val = 0;
            InvokeIfRequired(pb, () => { val = pb.Value; }, DispatcherPriority.Background);
            return val;
        } 

        /// <summary>
        /// Sets the Value property of a ProgressBar in a thread safe way
        /// </summary>
        /// <param name="pb">the ProgressBar</param>
        /// <param name="val">The value to Set the Value property to</param>
        public static void SetValueThreadSafe(this ProgressBar pb, double val)
        {
            InvokeIfRequired( pb, ( ) => { pb.Value = val; }, DispatcherPriority.Background );
        }                

        /// <summary>
        /// Sets the Source property of an Image Control in a thread safe way
        /// </summary>
        /// <param name="img"></param>
        /// <param name="src"></param>
        public static void SetSourceThreadSafe( this System.Windows.Controls.Image img, string srcPath )
        {
            InvokeIfRequired( img, ( ) => {
                try
                {
                    if (!System.IO.File.Exists(srcPath))
                    {
                        img.Source = null;
                        return;
                    }

                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(srcPath, UriKind.Absolute);
                    bi.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(
                        System.Net.Cache.RequestCacheLevel.BypassCache);
                    bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bi.EndInit();
                    img.Source = bi;
                }
                catch (Exception e)
                {
                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                }
            }, 
                DispatcherPriority.Background );
        }

        //public static void SetSourceThreadSafe(this System.Windows.Controls.Image img,
        //    System.IO.MemoryStream ms )
        //{
        //    InvokeIfRequired(img, () =>
        //    {
        //        if( ms.Length == 0 )
        //        {
        //            img.Source = null;
        //            return;
        //        }

        //        BitmapImage bi = new BitmapImage();
        //        bi.BeginInit();
        //        //bi.CacheOption = BitmapCacheOption.OnLoad;
        //        bi.StreamSource = ms;
        //        //bi.UriSource = new Uri(srcPath, UriKind.Absolute);
        //        //bi.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(
        //        //    System.Net.Cache.RequestCacheLevel.BypassCache);
        //        //bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
        //        bi.EndInit();
        //        img.Source = bi;
        //    },
        //        DispatcherPriority.Background);
        //}

        public static void SetSourceThreadSafe(this System.Windows.Controls.Image img,
            System.Drawing.Bitmap bmp )
        {
            InvokeIfRequired(img, () =>
            {
                try
                {
                    if (bmp == null)
                    {
                        img.Source = null;
                        return;
                    }

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Flush();
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.EndInit();
                    img.Source = bi;
                    //ms.Dispose(); // dispose would destroy the memory stream thus bitmap image has no source
                }
                catch(Exception e)
                {
                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                }
            }, DispatcherPriority.Background);
        }


        public static ImageSource GetSourceThreadSafe( this System.Windows.Controls.Image img )
        {
            ImageSource src = null;
            InvokeIfRequired( img, ( ) => { src = img.Source; }, DispatcherPriority.Background );
            return src;
        }

        public static void SetOpacityThreadSafe( this UIElement element, double opacity )
        {
            InvokeIfRequired( element, ( ) => { element.Opacity = opacity; }, DispatcherPriority.Background );
        }

        public static void SetDropShadowBitmapEffectThreadSafe( this UIElement element, double softness )
        {
            InvokeIfRequired( element, ( ) => { 
                DropShadowBitmapEffect effect = new DropShadowBitmapEffect();
                effect.Softness = 0.8;
                element.BitmapEffect = effect;}, DispatcherPriority.Background );
        }

        public static void UnSetDropShadowBitmapEffectThreadSafe( this UIElement element )
        {
            InvokeIfRequired( element, ( ) =>
            {
                element.BitmapEffect = null;
            }, DispatcherPriority.Background );
        }

        public static void TranslateAnimationThreadSafe( this UIElement element, 
            double xDistance, double yDistance, TimeSpan duration )
        {
            InvokeIfRequired( element, ( ) =>
            {
                Transform tr = element.RenderTransform;
                if( tr is TranslateTransform )
                {
                    TranslateAnimation( tr as TranslateTransform, xDistance, yDistance, duration );
                }
                else if( tr is TransformGroup )
                {
                    foreach( Transform item in (tr as TransformGroup).Children )
                    {
                        if( item is TranslateTransform )
                        {
                            TranslateAnimation( item as TranslateTransform, xDistance, yDistance, duration );
                            return;
                        }
                    }
                    TranslateTransform trtr = new TranslateTransform();
                    ( tr as TransformGroup ).Children.Add( trtr );
                    TranslateAnimation( trtr, xDistance, yDistance, duration );
                }
                else
                {
                    TransformGroup group = new TransformGroup();
                    if( tr != null ) group.Children.Add( tr );
                    TranslateTransform trtr = new TranslateTransform();
                    group.Children.Add( trtr );
                    element.RenderTransform = group;
                    TranslateAnimation( trtr, xDistance, yDistance, duration );
                }
            }, DispatcherPriority.Background );
        }

        private static void TranslateAnimation( TranslateTransform tr, double xDistance,
            double yDistance, TimeSpan duration )
        {
            if( xDistance != 0.0 )
            {
                DoubleAnimation xAnimation = new DoubleAnimation( tr.X,
                    tr.X + xDistance, duration );
                tr.BeginAnimation( TranslateTransform.XProperty, xAnimation );
            }
            if( yDistance != 0.0 )
            {
                DoubleAnimation yAnimation = new DoubleAnimation( tr.Y,
                    tr.Y + yDistance, duration );
                tr.BeginAnimation( TranslateTransform.YProperty, yAnimation );
            }
        }

        public static void SetOpacityAnimation( this UIElement element,
            double from, double to, TimeSpan duration )
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation(
                from, to, duration );
            InvokeIfRequired( element, ( ) =>
            {
                element.BeginAnimation( UIElement.OpacityProperty,
                    opacityAnimation );
            }, DispatcherPriority.Background );
        }
        //public static void SetFlashThreadSafe( this UIElement element,
        //    int t1, int t2, double ratio )
        //{
        //    double opacity = element.Opacity;
        //    DoubleAnimation opacityAnimation = 
        //        new DoubleAnimation( opacity, opacity * ratio, 
        //            TimeSpan.FromMilliseconds( t1 ) );
        //    element.BeginAnimation( UIElement.OpacityProperty, opacityAnimation );
        //    Thread.Sleep( t1 );
        //    opacityAnimation = new DoubleAnimation( opacity * ratio,
        //        opacity, TimeSpan.FromMilliseconds( t2 ) );
        //    element.BeginAnimation( UIElement.OpacityProperty, opacityAnimation );
        //}

        public static void HideThreadSafe( this Window w )
        {
            InvokeIfRequired(w, () =>
                {
                    w.Hide();
                }, DispatcherPriority.Background);
        }

        public static void ShowThreadSafe(this Window w)
        {
            InvokeIfRequired(w, () =>
            {
                w.Show();
            }, DispatcherPriority.Background);
        }

        public static void SetTopMostThreadSafe( this Window w, bool value )
        {
            InvokeIfRequired(w, () =>
            {
                w.Topmost = value;
            }, DispatcherPriority.Background);
        }

        public static void MaximizeThreadSafe(this Window w)
        {
            InvokeIfRequired(w, () =>
            {
                w.WindowState = WindowState.Maximized;
            }, DispatcherPriority.Background);
        }

        public static void ActivateThreadSafe( this Window w )
        {
            InvokeIfRequired(w, () =>
            {
                w.Activate();
            }, DispatcherPriority.Background);
        }

        public static string GetNameThreadSafe( this System.Windows.Controls.Image img )
        {
            string name = string.Empty;
            InvokeIfRequired(img, () =>
            {
                name = img.Name;
            }, DispatcherPriority.Background);
            return name;
        }
    }
}

