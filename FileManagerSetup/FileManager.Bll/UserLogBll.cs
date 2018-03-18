using System;
using System.Data;
using System.Collections.Generic;
using FileManager.Common;

namespace FileManager.BLL
{
    /// <summary>
    /// �û�������־��Ϣ��
    /// </summary>
    public partial class UserLogBll
    {
        private readonly DAL.UserLogDal dal;
        public UserLogBll()
        {
            dal = new DAL.UserLogDal("FM_");
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
        /// ����������
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(Model.UserLogModel model)
        {
            return dal.Add(model);
        }
         
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.UserLogModel GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// ��ȡIP
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <returns></returns>
        public List<string> GetLogIps(int Top, string strWhere, string filedOrder)
        {
            return dal.GetLogIps(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// ��ȡʵ���б�
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <returns></returns>
        public List<Model.UserLogModel> GetModelList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetModelList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// �����û���������һ�ε�¼��¼
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="top_num"></param>
        /// <param name="action_type"></param>
        /// <returns></returns>
        public Model.UserLogModel GetModel(string user_name, int top_num, string action_type)
        {
            return dal.GetModel(user_name, top_num, action_type);
        }

        /// <summary>
        /// ����Ҫ�ж�����
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="top_num"></param>
        /// <returns></returns>
        public Model.UserLogModel GetModel(string user_name, int top_num)
        {
            return dal.GetModel(user_name, top_num);
        }

        /// <summary>
        /// ɾ��7��ǰ����־����
        /// </summary>
        /// <param name="dayCount"></param>
        /// <returns></returns>
        public int DeleteByDay(int dayCount)
        {
            return dal.DeleteByDay(dayCount);
        }

        /// <summary>
        /// ��ò�ѯ��ҳ����
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        #endregion  Method
    }
}