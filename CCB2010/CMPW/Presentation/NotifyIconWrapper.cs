using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JOYFULL.CMPW.Presentation
{
    public partial class NotifyIconWrapper : Component
    {
        public NotifyIconWrapper()
        {
            InitializeComponent();
            string path = "Images/telescope.png";
            Bitmap bmp = new Bitmap( path );
            NotifyIco.Icon = Icon.FromHandle( bmp.GetHicon() );
        }

        public NotifyIconWrapper( IContainer container )
        {
            container.Add( this );

            InitializeComponent();

        }
    }
}
