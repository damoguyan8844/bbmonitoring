/******************************************
 * 
 * ģ�����ƣ����ݿ���Դ����ʵ���༯��
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
 * 
 ******************************************/

using System;
using System.Collections;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// ���ݿ���Դ����ʵ���༯�ϡ�
	/// </summary>
	internal class DatabaseSettings:DictionaryBase
	{
		/// <summary>
		/// <para>Ĭ�Ϲ��캯����</para>
		/// </summary>
		internal DatabaseSettings()
		{
		}

		/// <summary>
		/// �򼯺�������һ��
		/// </summary>
		/// <param name="data"></param>
		internal void Add( DatabaseSettingData data )
		{
			this.Dictionary.Add( data.DatabaseKey,data );
		}

		/// <summary>
		/// ��ȡ��������ָ�� <paramref name="databaseKey"/> ���Ӧ���
		/// �� C# �У�������Ϊ DatabaseSettings �����������
		/// </summary>
		/// <param name="databaseKey">���ݿ����ļ�ֵ��</param>
		/// <value>һ�� <see cref="DatabaseSettingData"/> �</value>
		internal DatabaseSettingData this[ string databaseKey ]
		{
			get
			{
				return this.Dictionary[ databaseKey ] as DatabaseSettingData;
			}
		}
	}
}
