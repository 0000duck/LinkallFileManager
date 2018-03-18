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

namespace FileManager
{
    public partial class FrmForderProperty : CCSkinMain
    {
        private int type;
        private Model.ForderModel forder;
        public FrmForderProperty(int actionType, int type, Model.ForderModel forder)
        {
            InitializeComponent();

            this.type = type;
            this.forder = forder;
            //�ļ���
            if (type == 1)
            {
                this.skinRadioButton_file.Enabled = false;
                this.skinRadioButton_forder.Select();
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
                this.btnOK.Enabled = false;
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
                    if (this.forder != null && !string.IsNullOrEmpty(this.forder.UserName) && item.UserName == this.forder.UserName)
                    {
                        this.skinComboBox_createUser.SelectedItem = new ListItem(showText, item);
                        isHaveUser = true;
                    }
                }
                if (!isHaveUser)
                {
                    this.skinComboBox_createUser.SelectedIndex =0;
                }
            }
        }

        /// <summary>
        /// ��ʼ��UI
        /// </summary>
        /// <param name="file"></param>
        private void InitUiView()
        {
            this.skinTextBox_addTime.SkinTxt.Text = forder.Add_Time.ToString();
            this.skinTextBox_modifyTime.SkinTxt.Text = new DateTime(forder.Forder_Modify_Time).ToString();
            this.skinTextBox_name.SkinTxt.Text = forder.Title;
            this.skinTextBox_clientPath.SkinTxt.Text = forder.ClientPath;
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
                string addTimeStr = this.skinTextBox_addTime.SkinTxt.Text.Trim();
                string modifyTimeStr = this.skinTextBox_modifyTime.SkinTxt.Text.Trim();
                string name = this.skinTextBox_name.SkinTxt.Text.Trim();

                ListItem selectItem = (ListItem)this.skinComboBox_createUser.SelectedItem;
                Model.UserModel createUser = (Model.UserModel)selectItem.Value;

                DateTime addTime = DateTime.Now;
                DateTime modifyTime = DateTime.Now;

                if (createUser == null || string.IsNullOrEmpty(createUser.UserName))
                {
                    MessageBoxEx.Show("��ѡ�û���Ϣ����ȷ��");
                    return;
                }

                if (string.IsNullOrEmpty(name))
                {
                    MessageBoxEx.Show("���Ʋ���Ϊ�գ�");
                    return;
                }

                if (string.IsNullOrEmpty(addTimeStr) || !DateTime.TryParse(addTimeStr, out addTime))
                {
                    MessageBoxEx.Show("���ʱ�䲻��ȷ��");
                    return;
                }

                if (string.IsNullOrEmpty(modifyTimeStr) || !DateTime.TryParse(modifyTimeStr, out modifyTime))
                {
                    MessageBoxEx.Show("�޸�ʱ�䲻��ȷ��");
                    return;
                }

                this.forder.Add_Time = addTime;
                this.forder.Forder_Modify_Time = modifyTime.Ticks;
                this.forder.UserName = createUser.UserName;
                this.forder.Title = name;

                BLL.ForderBll bll = new BLL.ForderBll();
                bll.Update(this.forder);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                MessageBoxEx.Show("�޸ĳɹ���");

            }
            catch (Exception ee)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBoxEx.Show("�޸�ʧ�ܣ�" + ee.Message);
            }
        }
    }
}
