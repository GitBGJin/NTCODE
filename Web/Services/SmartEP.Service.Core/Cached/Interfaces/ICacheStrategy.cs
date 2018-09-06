namespace SmartEP.Service.Core.Cached
{
    using System;
    using System.Text;

    /// <summary>
    /// ����������Խӿ�
    /// </summary>
    public interface ICacheStrategy
    {
        #region Properties

        /// <summary>
        /// ����ʱ��,��λ����
        /// </summary>
        int TimeOut
        {
            set;get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// ���ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        void AddObject(string objId, object o);

        /// <summary>
        /// ���ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        /// <param name="expire">����ʱ��,��λ:��</param>
        void AddObject(string objId, object o, int expire);

        /// <summary>
        /// ���ָ��ID�Ķ���(����ָ����ֵ��)
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        /// <param name="dependKey">������</param>
        void AddObjectWithDepend(string objId, object o, string[] dependKey);

        /// <summary>
        /// ���ָ��ID�Ķ���(����ָ���ļ���)
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        /// <param name="files">�������ļ���</param>
        void AddObjectWithFileChange(string objId, object o, string[] files);

        /// <summary>
        /// ��յ��л�������
        /// </summary>
        void FlushAll();

        /// <summary>
        /// �Ƿ����ӳɹ�
        /// </summary>
        /// <returns></returns>
        bool IsConnect();

        /// <summary>
        /// �Ƿ����ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <returns></returns>
        bool IsExistObject(string objId);

        /// <summary>
        /// �Ƴ�ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        void RemoveObject(string objId);

        /// <summary>
        /// ����ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <returns></returns>
        object RetrieveObject(string objId);

        #endregion Methods
    }
}