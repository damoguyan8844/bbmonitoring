/******************************************
 * 
 * ģ�����ƣ����ÿ�����
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
 * 
 ******************************************/

using System;
using System.Xml;
using System.Configuration;


namespace JOYFULL.CMPW.Data
{

	/// <summary>
	/// Summary of DataConfigHandler
	/// </summary>

	public class DataConfigHandler : IConfigurationSectionHandler	
	{
		object IConfigurationSectionHandler.Create ( object parent, object context,	XmlNode section )
		{
			if( Object.Equals(section, null))
			{

				throw(new ArgumentNullException());
			}
			return section;				
		}
	}

}

