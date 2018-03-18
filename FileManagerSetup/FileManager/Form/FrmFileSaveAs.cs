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
using FileManager.Model;
using FileManager.BLL;

namespace FileManager
{
    public partial class FrmFileSaveAs : CCSkinMain
    {
        private int type;
        private Model.FileLogModel fileLog;
        private Model.FileModel file;
        public FrmFileSaveAs(int actionType, int type, Model.FileLogModel fileLog)
        {
            InitializeComponent();
            this.type = type;
            this.fileLog = fileLog;
            this.file = new BLL.FileBll().GetModel(fileLog.FileID);
            //�ļ���
            if (type == 1)
            {
                this.skinRadioButton_file.Enabled = false;
                this.skinRadioButton_forder.Select();
                this.skinTextBox_size.Visible = false;
            }
            else
            {
                this.skinRadioButton_forder.Enabled = false;
                this.skinRadioButton_file.Select();
            }

            //��ʼ����Ա��Ϣ
            InitCreateUser();
            InitUiView();

            ///��������Բ鿴����ֹ���п��޸Ŀؼ�
            if (actionType == 0)
            {
                //this.btnOK.Enabled = false;
                this.skinTextBox_addTime.SkinTxt.ReadOnly = true;
                this.skinTextBox_modifyTime.SkinTxt.ReadOnly = true;
                this.skinTextBox_name.SkinTxt.ReadOnly = true;
                this.skinComboBox_createUser.Enabled = false;
                this.skinTextBox_clientPath.SkinTxt.ReadOnly = true;
            }
        }

        /// <summary>
        /// ��ʼ����Ա
        /// </summary>
        private void InitCreateUser()
        {
            //��ȡ�����û�
            BLL.ManagerBll mBll = new BLL.ManagerBll();
            List<Model.UserModel> users = mBll.GetModelList(1000, " isLock = 0 ", " RoleType desc ");

            if (users != null && users.Count > 0)
            {
                bool isHaveUser = false;
                foreach (var item in users)
                {
                    string showText = string.Format("{0}({1})", item.UserName, item.RealName);
                    this.skinComboBox_createUser.Items.Add(new ListItem(showText, item));
                    if (this.file != null && !string.IsNullOrEmpty(this.file.UserName) && item.UserName == this.file.UserName)
                    {
                        this.skinComboBox_createUser.SelectedItem = new ListItem(showText, item);
                        isHaveUser = true;
                    }
                }
                if (!isHaveUser)
                {
                    this.skinComboBox_createUser.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// ��ʼ��UI
        /// </summary>
        /// <param name="file"></param>
        private void InitUiView()
        {
            this.skinTextBox_addTime.SkinTxt.Text = file.Add_Time.ToString();
            this.skinTextBox_modifyTime.SkinTxt.Text = new DateTime(file.File_Modify_Time).ToString();
            this.skinTextBox_name.SkinTxt.Text = file.File_Name;
            this.skinTextBox_size.SkinTxt.Text = file.File_Size.ToString();
            this.skinTextBox_clientPath.SkinTxt.Text = file.ClientPath;
            this.skinTextBox_lastVer.SkinTxt.Text = file.File_LastVersion.ToString();
            this.skinTextBox_logDetail.SkinTxt.Text = fileLog.Remark == null ? string.Empty : fileLog.Remark;
        }

        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                FileBll fileBll = new FileBll();
                Model.FileModel fileModel = fileBll.GetModel(this.file.ID);

                if (fileModel == null || string.IsNullOrEmpty(fileModel.File_Name))
                {
                    MessageBoxEx.Show("���ļ�����Ϣ�쳣��");
                    return;
                }

                FileVersionBll verBll = new FileVersionBll();

                var fileVer = new BLL.FileVersionBll().GetModel(this.fileLog.FileVerID);
                var content = verBll.GetContent(fileVer.ID);

                // ��ȡ�ļ���Ϣ
                this.saveFileDialog1.Filter = string.Format("*{0}|*.*", fileModel.File_Ext); ;
                this.saveFileDialog1.ShowDialog();
                string fileSavePath = this.saveFileDialog1.FileName;

                if (string.IsNullOrEmpty(fileSavePath))
                {
                    return;
                }

                //�ļ����·��
                string newPath = fileSavePath.EndsWith(fileModel.File_Ext) ? fileSavePath : fileSavePath + fileModel.File_Ext;
                FileWinexploer.CreateFile(content, newPath, fileVer.File_Modify_Time);
                MessageBoxEx.Show("����ɹ�!");
            }
            catch (Exception ee)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBoxEx.Show("�洢ʧ�ܣ�" + ee.Message);
            }
        }
    }
}
