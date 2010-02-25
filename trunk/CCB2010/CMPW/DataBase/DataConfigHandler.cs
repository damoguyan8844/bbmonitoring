/******************************************
 * 
 * 模块名称：配置控制类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
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

