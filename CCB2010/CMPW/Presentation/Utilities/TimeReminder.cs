using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;

namespace JOYFUL.CMPW.Presentation
{
    class RemindingInfo
    {
        public DateTime Time;
        public string Description;
        public bool Reminded;
        public string FileName;
    }

    internal class TimeReminder
    {
        readonly string PATH = ConfigurationManager.AppSettings[ "RemindingFile" ];
        List<RemindingInfo> _list = new List<RemindingInfo>();
        XmlDocument _doc = null;

        public TimeReminder( )
        {
            _doc = new XmlDocument();
            _doc.Load( PATH );
            Init( _doc );
        }

        private void Init( XmlDocument doc )
        {
            _doc = doc;
            _list.Clear();
            foreach ( XmlNode node in
                _doc.SelectSingleNode( "Remindings" ).SelectNodes( "Reminding" ) )
            {
                RemindingInfo info = new RemindingInfo();
                info.Time = DateTime.Today +
                    TimeSpan.Parse( node.Attributes[ "time" ].Value );
                info.Description = node.Attributes[ "desc" ].Value;
                info.FileName = node.Attributes["file"].Value;
                info.Reminded = false;
                _list.Add( info );
            }
        }

        public bool TryGetRemindInfo( out string desc )
        {
            desc = string.Empty;
            DateTime now = DateTime.Now;
            foreach( RemindingInfo item in _list )
            {
                if( item.Reminded ) continue;
                int seconds = (int)(( now - item.Time ).TotalSeconds);
                if( seconds > 0 && seconds < 60 )
                {
                    desc = item.Description+"__"+item.FileName;
                    item.Reminded = true;
                    return true;
                }
            }
            return false;
        }

        List<string> fileList = new List<string>();

       public void Save( List<string> timeList, List<string> descList )
       {
           XmlNode node = _doc.SelectSingleNode( "Remindings" );
           XmlNode child = node.FirstChild;
           node.RemoveAll();
           for( int i = 0; i < timeList.Count; ++i )
           {
               var item = child.Clone();
               item.Attributes[ "time" ].Value = timeList[ i ];
               item.Attributes[ "desc" ].Value = descList[ i ];
               item.Attributes[ "file" ].Value = fileList[i];

               node.AppendChild( item );
           }
           Init( _doc );
           _doc.Save( PATH );
       }

        public void Get( out List<string> timeList, out List<string> descList )
        {
            timeList = new List<string>();
            descList = new List<string>();
            fileList.Clear();

            foreach( RemindingInfo item in _list )
            {
                timeList.Add( item.Time.ToString( "HH:mm" ) );
                descList.Add( item.Description );
                fileList.Add(item.FileName);
            }
        }
    }
}
