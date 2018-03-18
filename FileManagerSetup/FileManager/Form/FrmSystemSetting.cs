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
using FileManager.Bll;

namespace FileManager
{
    public partial class FrmSystemSetting : CCSkinMain
    {
        private int type;
        public FrmSystemSetting(int type)
        {
            InitializeComponent();
            this.type = type;
            InitUiData();
        }
        private Model.SystemConfig config = null;
        private void InitUiData()
        {
            // ��ȡ������Ϣ
            config = BLL.SystemConfigBll.GetConfig();
            if (config != null)
            {
                this.skinTextBox_managerAccount.SkinTxt.Text = config.DbUser;
                this.skinTextBox_managerAddress.SkinTxt.Text = config.DbAddress;
                this.skinTextBox_managerDbName.SkinTxt.Text = config.DbName;
                this.skinTextBox_managerPwd.SkinTxt.Text = config.DbPassword;

                this.skinTextBox_contentAccount.SkinTxt.Text = config.DbFileContentUser;
                this.skinTextBox_contentAddress.SkinTxt.Text = config.DbFileContentAddress;
                this.skinTextBox_contentDbName.SkinTxt.Text = config.DbFileContentName;
                this.skinTextBox_contentPwd.SkinTxt.Text = config.DbFileContentPassword;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LinkTest()) return;

                config.DbUser = this.skinTextBox_managerAccount.SkinTxt.Text.Trim();
                config.DbAddress = this.skinTextBox_managerAddress.SkinTxt.Text.Trim();
                config.DbName = this.skinTextBox_managerDbName.SkinTxt.Text.Trim();
                config.DbPassword = this.skinTextBox_managerPwd.SkinTxt.Text.Trim();

                config.DbFileContentUser = this.skinTextBox_contentAccount.SkinTxt.Text.Trim();
                config.DbFileContentAddress = this.skinTextBox_contentAddress.SkinTxt.Text.Trim();
                config.DbFileContentName = this.skinTextBox_contentDbName.SkinTxt.Text.Trim();
                config.DbFileContentPassword = this.skinTextBox_contentPwd.SkinTxt.Text.Trim();
                new SystemConfigBll().saveConifg(config);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܷ������쳣 : ", ex.Message);
                return;
            }

            MessageBox.Show("���óɹ���");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void skinButton_dbLinkTest_Click(object sender, EventArgs e)
        {
            if (LinkTest())
            {
                MessageBox.Show("���������ӵ�ַ��ȷ��");
                return;
            }
        }

        private bool LinkTest()
        {
            string dbUser = this.skinTextBox_managerAccount.SkinTxt.Text.Trim();
            string dbAddress = this.skinTextBox_managerAddress.SkinTxt.Text.Trim();
            string dbName = this.skinTextBox_managerDbName.SkinTxt.Text.Trim();
            string dbPassword = this.skinTextBox_managerPwd.SkinTxt.Text.Trim();

            string conn = string.Format("server={0};uid={1};pwd={2};database={3};Connect Timeout=3", dbAddress, dbUser, dbPassword, dbName);

            bool isLinkSuccess = SystemBll.SystemLinkCheck(conn);
            if (!isLinkSuccess)
            {
                MessageBox.Show("ϵͳ�����Ӵ���,���������÷��������ӵ�ַ��");
                return false;
            }
            return true;

        }
    }
}
