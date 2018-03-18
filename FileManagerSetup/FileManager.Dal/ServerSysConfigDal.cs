using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using FileManager.DBUtility;
using FileManager.Common;

namespace FileManager.DAL
{
    /// <summary>
    /// ���ݷ�����:����Ա
    /// </summary>
    public partial class ServerSysConfigDal
    {
        private string databaseprefix; //���ݿ����ǰ׺
        public ServerSysConfigDal(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region ��������

        /// <summary>
        /// ��ѯ�Ƿ����
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "ServerSysConfig");
            return DbHelperSQL.Exists(strSql.ToString());
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.ServerSysConfig model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "ServerSysConfig set ");
            strSql.Append("ManagerEmail=@RoleId,");
            strSql.Append("ManagerTel=@RoleType,");
            strSql.Append("SystemLoginType=@RoleName,");
            SqlParameter[] parameters = {
					new SqlParameter("@ManagerEmail", SqlDbType.Int,4),
					new SqlParameter("@ManagerTel", SqlDbType.NVarChar,100),
					new SqlParameter("@SystemLoginType", SqlDbType.NVarChar,100),
               };
            parameters[0].Value = model.ManagerEmail;
            parameters[1].Value = model.ManagerTel;
            parameters[2].Value = model.SystemLoginType;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �޸�һ������
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "ServerSysConfig set " + strValue);
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.ServerSysConfig GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from " + databaseprefix + "ServerSysConfig ");

            Model.ServerSysConfig model = new Model.ServerSysConfig();
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ManagerEmail"].ToString() != "")
                {
                    model.ManagerEmail = ds.Tables[0].Rows[0]["ManagerEmail"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ManagerTel"].ToString() != "")
                {
                    model.ManagerTel =  ds.Tables[0].Rows[0]["ManagerTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SystemLoginType"].ToString() != "")
                {
                    model.SystemLoginType = int.Parse(ds.Tables[0].Rows[0]["SystemLoginType"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
    
        #endregion  Method
    }
}