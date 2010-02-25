/******************************************
 * ģ�����ƣ�UpdateBehavior ö��
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
  ******************************************/

using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// �� Database.UpdateDataSet ������ʹ�ã�ָ���� DataAdapter �ĸ�����������ʧ��ʱ�Ĳ�����
	/// </summary>
	public enum UpdateBehavior
	{
		/// <summary>
		/// ���� DataAdapter �ĸ��������Ԥ���������ʧ�ܣ����»�ֹͣ��DataTable�е�ʣ���в�����¡�
		/// </summary>
		Standard,
		/// <summary>
		/// ��� DataAdapter �ĸ�����������ʧ�ܣ����»��������������᳢�Ը���ʣ��������С� 
		/// </summary>
		Continue,
		/// <summary>
		/// ��� DataAdapter �ĸ�����������ʧ��, ���еĸ��²�����ع���
		/// </summary>
		Transactional
	}
}
