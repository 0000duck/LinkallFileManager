using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using FileManager.BLL;
using FileManager.Common;

namespace FileManager
{
    public partial class FrmUserDomin : CCSkinMain
    {
        public FrmUserDomin(string userName, string realName)
        {
            InitializeComponent();
            this.skinTextBox_userName.SkinTxt.Text = userName;
            this.skinTextBox_realName.SkinTxt.Text = realName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string userName = this.skinTextBox_userName.SkinTxt.Text;
            string realName = this.skinTextBox_realName.SkinTxt.Text;
            string userPwd = this.skinTextBox_userPwd.SkinTxt.Text;
            string confirmPwd = this.skinTextBox_confirmPwd.SkinTxt.Text;

            if (string.IsNullOrEmpty(userName))
            {
                MessageBoxEx.Show("���˺Ų���Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(realName))
            {
                MessageBoxEx.Show("���˺���������Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(userPwd))
            {
                MessageBoxEx.Show("���˺����벻��Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(confirmPwd))
            {
                MessageBoxEx.Show("����ȷ�ϲ���Ϊ�գ�");
                return;
            }

            if (userPwd != confirmPwd)
            {
                MessageBoxEx.Show("�����������벻һ��,���������룡");
                return;
            }

            try
            {
                if (!AuthDomin(realName, userName, userPwd))
                {
                    MessageBox.Show("������˺�ʧ�ܣ�");
                    return;
                }
                MessageBox.Show("������˺ųɹ���");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Error", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show("���ִ��� �� " + ex.Message);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool AuthDomin(string realName, string userName, string userPwd)
        {
            // ��ȡ������Ϣ
            var config = BLL.SystemConfigBll.GetConfig();
            if (config != null && !string.IsNullOrEmpty(config.Domin) && !string.IsNullOrEmpty(config.DominAdminName))
            {
                // ��ȡ����Ա��Ϣ
                var adminUserInfo = new ManagerBll().GetModel(config.DominAdminName);
                if (adminUserInfo != null && !string.IsNullOrEmpty(adminUserInfo.UserName) && !string.IsNullOrEmpty(adminUserInfo.Password))
                {
                    Bll.ADHelper.SetADValue(config.Domin, adminUserInfo.UserName, adminUserInfo.Password);
                    return Bll.ADHelper.AddNewUser(realName, userName, userPwd);
                }
            }

            return false;
        }
    }
}
