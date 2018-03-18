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

namespace FileManager.ServerSetup
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
                if (!LinkManagerTest() || !LinkContentTest())
                {
                    MessageBox.Show("�������ݿ������Ƿ�������");
                    return;
                }

                config.DbUser = this.skinTextBox_managerAccount.SkinTxt.Text.Trim();
                config.DbAddress = this.skinTextBox_managerAddress.SkinTxt.Text.Trim();
                config.DbName = this.skinTextBox_managerDbName.SkinTxt.Text.Trim();
                config.DbPassword = this.skinTextBox_managerPwd.SkinTxt.Text.Trim();

                config.DbFileContentUser = this.skinTextBox_contentAccount.SkinTxt.Text.Trim();
                config.DbFileContentAddress = this.skinTextBox_contentAddress.SkinTxt.Text.Trim();
                config.DbFileContentName = this.skinTextBox_contentDbName.SkinTxt.Text.Trim();
                config.DbFileContentPassword = this.skinTextBox_contentPwd.SkinTxt.Text.Trim();
                //new SystemConfigBll().saveConifg(config);

                // ��ʼ��װ
                FrmWait frmManager = new FrmWait(config.DbName, 0, config.DbAddress, config.DbUser, config.DbPassword);
                DialogResult resultManager = frmManager.ShowDialog();

                if (resultManager == System.Windows.Forms.DialogResult.OK)
                {
                    FrmWait frmContent = new FrmWait(config.DbFileContentName, 1, config.DbFileContentAddress, config.DbFileContentUser, config.DbFileContentPassword);
                    DialogResult resultContent = frmContent.ShowDialog();

                    if (resultContent == System.Windows.Forms.DialogResult.OK)
                    {
                        MessageBox.Show("��װ�ɹ���");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("��װʧ�ܷ������쳣 : ", ex.Message);
                return;
            }


        }

        /// <summary>
        /// ���ݿ����Ӳ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton_dbLinkTest_Click(object sender, EventArgs e)
        {
            if (LinkManagerTest())
            {
                if (LinkContentTest())
                {
                    MessageBox.Show("���Ӳ���������������װ������");
                }
            }
        }

        private void Test(int type)
        {
            int retValue = 0;
            if (type == 0)
            {
                ����LinkManagerTest();
            }
            else
            {
                �� LinkContentTest();
            }

            if (retValue == 1)
            {
                MessageBox.Show("���ݿ��Ѿ����ڣ�����ȡ����");
                return;
            }
            else if (retValue == 2)
            {
                MessageBox.Show("���ݿⲻ���ڣ�������װ������");
                return;
            }
            else if (retValue == -1)
            {
                MessageBox.Show("���ݿ����ʧ�ܣ�" + "\n\n" + "�������ֶ���������");
                System.Diagnostics.Process.Start(System.Environment.CurrentDirectory);//�򿪰�װĿ¼
            }
            else
            {

            }
        }

        private bool LinkManagerTest()
        {
            string dbUser = this.skinTextBox_managerAccount.SkinTxt.Text.Trim();
            string dbAddress = this.skinTextBox_managerAddress.SkinTxt.Text.Trim();
            string dbName = this.skinTextBox_managerDbName.SkinTxt.Text.Trim();
            string dbPassword = this.skinTextBox_managerPwd.SkinTxt.Text.Trim();

            //string conn = string.Format("server={0};uid={1};pwd={2};database={3};Connect Timeout=3", dbAddress, dbUser, dbPassword, dbName);
            string strSql = "Server=" + dbAddress + ";User Id=" + dbUser + ";Password=" + dbPassword + ";Database=master;";//�������ݿ��ַ���
            int retNum = new InstallDB().CheckDataBase(strSql, dbName, System.Environment.CurrentDirectory);
            if (retNum <= 0)
            {
                MessageBox.Show("�ļ����������������Ϣ�����ӵ�ַ�����˺��������,���������ã�");
                return false;
            }
            else if (retNum == 1)
            {
                MessageBox.Show( string.Format("�ļ����������������Ϣ���ݿ����ƣ�{0} �Ѿ����ڣ��������������ƣ�",dbName));
                return false;
            }
            else if (retNum == 2)
            {
                return true;
            }

            return false;
        }

        private bool LinkContentTest()
        {
            string dbUser = this.skinTextBox_contentAccount.SkinTxt.Text.Trim();
            string dbAddress = this.skinTextBox_contentAddress.SkinTxt.Text.Trim();
            string dbName = this.skinTextBox_contentDbName.SkinTxt.Text.Trim();
            string dbPassword = this.skinTextBox_contentPwd.SkinTxt.Text.Trim();
            string strSql = "Server=" + dbAddress + ";User Id=" + dbUser + ";Password=" + dbPassword + ";Database=master;";//�������ݿ��ַ���
            int retNum = new InstallDB().CheckDataBase(strSql, dbName, System.Environment.CurrentDirectory);

            if (retNum <= 0)
            {
                MessageBox.Show("�ļ����ݷ�����������Ϣ�����ӵ�ַ�����˺��������,���������ã�");
                return false;
            }
            else if (retNum == 1)
            {
                MessageBox.Show(string.Format("�ļ����ݷ�����������Ϣ���ݿ����ƣ�{0} �Ѿ����ڣ��������������ƣ�", dbName));
                return false;
            }
            else if (retNum == 2)
            {
                return true;
            }

            return false;
        }

        private void skinButton_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
