/******************************************
 * 
 * 模块名称：数据库资源配置实体类集合
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
 * 
 ******************************************/

using System;
using System.Collections;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// 数据库资源配置实体类集合。
	/// </summary>
	internal class DatabaseSettings:DictionaryBase
	{
		/// <summary>
		/// <para>默认构造函数。</para>
		/// </summary>
		internal DatabaseSettings()
		{
		}

		/// <summary>
		/// 向集合中增加一项
		/// </summary>
		/// <param name="data"></param>
		internal void Add( DatabaseSettingData data )
		{
			this.Dictionary.Add( data.DatabaseKey,data );
		}

		/// <summary>
		/// 获取集合中与指定 <paramref name="databaseKey"/> 相对应的项。
		/// 在 C# 中，该属性为 DatabaseSettings 类的索引器。
		/// </summary>
		/// <param name="databaseKey">数据库对象的键值。</param>
		/// <value>一个 <see cref="DatabaseSettingData"/> 项。</value>
		internal DatabaseSettingData this[ string databaseKey ]
		{
			get
			{
				return this.Dictionary[ databaseKey ] as DatabaseSettingData;
			}
		}
	}
}
