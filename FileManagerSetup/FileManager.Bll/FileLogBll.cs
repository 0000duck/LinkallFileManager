using System;
using System.Data;
using System.Collections.Generic;
using FileManager.Common;

namespace FileManager.BLL
{
    /// <summary>
    /// �û�������־��Ϣ��
    /// </summary>
    public partial class FileLogBll
    {
        private readonly DAL.FileLogDal dal;
        public FileLogBll()
        {
            dal = new DAL.FileLogDal("FM_");
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
        public int Add(Model.FileLogModel model)
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
        public Model.FileLogModel GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// ͨ�����������ȡ
        /// </summary>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        public Model.FileLogModel GetModel(string actionCode)
        {
            return dal.GetModel(actionCode);
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <returns></returns>
        public DataSet GetAllInfoList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetAllInfoList(Top, strWhere, filedOrder);
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
        public List<Model.FileLogModel> GetModelList(int Top, string strWhere, string filedOrder)
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
        public Model.FileLogModel GetModel(string user_name, int top_num, string action_type)
        {
            return dal.GetModel(user_name, top_num, action_type);
        }

        /// <summary>
        /// ����Ҫ�ж�����
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="top_num"></param>
        /// <returns></returns>
        public Model.FileLogModel GetModel(string user_name, int top_num)
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