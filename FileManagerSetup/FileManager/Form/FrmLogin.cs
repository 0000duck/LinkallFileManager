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
using FileManager.Bll;

namespace FileManager
{
    public partial class FrmLogin : CCSkinMain
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private int errorLoginCount = 0;
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            this.skinLabel_errorMsg.Visible = false;
            this.skinLabel_errorMsg.Text = string.Empty;

            string account = this.skinTextBox_account.SkinTxt.Text.Trim();
            string pwd = this.skinTextBox_pwd.SkinTxt.Text.Trim();
            if (string.IsNullOrEmpty(account))
            {
                MessageBoxEx.Show("�˺���Ϣ����Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(pwd))
            {
                MessageBoxEx.Show("������Ϣ����Ϊ�գ�");
                return;
            }

            // ���˺ŵ�¼��֤
            try
            {
                this.skinLabel_errorMsg.Text = string.Format("���˺ţ�{0}�����ڵ�¼��,��Ⱥ�...", account);
                Application.DoEvents();

                UserLoginForDomainBll bll = new UserLoginForDomainBll();

                // ��ȡ������Ϣ
                var config = BLL.SystemConfigBll.GetConfig();

                if (this.errorLoginCount > 4)
                {
                    MessageBoxEx.Show(string.Format("���˺ţ�{0}�������������5��,�ѱ�����,����ϵ����Ա��", account));
                    this.skinLabel_errorMsg.Text = string.Format("���˺ţ�{0}�������������5��,�ѱ�����,����ϵ����Ա��", account);
                    this.skinLabel_errorMsg.Visible = true;

                    // �����˺�

                    return;
                }

                //if (!bll.ImpersonateValidUser(account, config.Domin, pwd))
                    if (!bll.ImpersonateValidUser2(account, config.Domin, pwd))
                //if (!bll.AuthDomainUserByAdHelper(account, config.Domin, pwd))
                {
                    this.errorLoginCount += 1;
                    MessageBoxEx.Show(string.Format("���˺ţ�{0}����¼��ʧ�� ���˺Ż����������", account));
                    this.skinLabel_errorMsg.Text = string.Format("���˺ţ�{0}����¼��ʧ�ܣ��˺Ż����������", account);
                    this.skinLabel_errorMsg.Visible = true;
                    return;
                }

                try
                {
                    //new UserLoginForDomainBll().AuthDomainUserAuth();
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(string.Format("���˺ţ�{0}����¼��ʧ�� ������ϵ����Ա����", account));
                this.skinLabel_errorMsg.Text = string.Format("���˺ţ�{0}����¼��ʧ�ܣ�", account);
                this.skinLabel_errorMsg.Visible = true;
                return;
            }

            //��ȡϵͳ�˺�
            Model.UserModel user = Bll.SystemBll.GetUserInfo(account);

            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.RealName))
            {
                MessageBoxEx.Show(string.Format("���˺ţ�{0}�������ڻ��������������ϵ����Ա����", account) + Bll.SystemBll.ManagerLinkInfo);
                this.skinLabel_errorMsg.Text = string.Format("���˺ţ�{0}�������ڻ����������", account);
                this.skinLabel_errorMsg.Visible = true;
                return;
            }

            if (user.IsLock == 1)
            {
                MessageBoxEx.Show(string.Format("���˺ţ�{0}���Ѿ������� ������ϵ����Ա����", account) + Bll.SystemBll.ManagerLinkInfo);
                this.skinLabel_errorMsg.Text = string.Format("���˺ţ�{0}��û��Ȩ�ޣ�", account);
                this.skinLabel_errorMsg.Visible = true;
                return;
            }

            Bll.SystemBll.UserInfo = user;

           

            // �첽��־
            SystemBll.ActionLogAsyn( 0,string.Empty,string.Empty, Model.ActionType.USERLOGIN);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void toolStripButton_systemSetting_Click(object sender, EventArgs e)
        {
            FrmDominSetting frm = new FrmDominSetting();
            frm.ShowDialog();
        }
    }
}
