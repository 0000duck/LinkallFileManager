using System;
using System.Collections.Generic;

namespace FileManager.Model
{
    /// <summary>
    /// �����ɫ:ʵ����[��ʱ����]
    /// </summary>
    [Serializable]
    public partial class UserRoleModel
    {
        public UserRoleModel()  { }

        #region Model

        /// <summary>
        /// ����ID
        /// </summary>
        public int ID;

        /// <summary>
        /// ��ɫ����
        /// </summary>
        public string RoleName;

        /// <summary>
        /// ��ɫ����
        /// </summary>
        public int RoleType;
      
        /// <summary>
        /// �Ƿ�ϵͳĬ��0��1��
        /// </summary>
        public int IsSys;
         
        #endregion Model
    }
}