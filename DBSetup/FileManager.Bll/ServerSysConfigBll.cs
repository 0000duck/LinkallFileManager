using System;
using System.Data;
using System.Collections.Generic;
using FileManager.Common;

namespace FileManager.BLL
{
    /// <summary>
    /// �û���Ϣ��
    /// </summary>
    public partial class ServerSysConfigBll
    {
        private readonly DAL.ServerSysConfigDal dal;
        public ServerSysConfigBll()
        {
            dal = new DAL.ServerSysConfigDal("FM_");
        }

        #region ��������
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.ServerSysConfig model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// ����һ�����ݵ��ֶ�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public void UpdateField(int id, string strValue)
        {
            dal.UpdateField(id, strValue);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.ServerSysConfig GetModel(int id)
        {
            return dal.GetModel(id);
        }

        #endregion  Method
    }
}