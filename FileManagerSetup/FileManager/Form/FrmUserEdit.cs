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
    public partial class FrmUserEdit : CCSkinMain
    {
        private int userId;
        private int actionType;
        public FrmUserEdit(int userId, int actionType)
        {
            InitializeComponent();
            this.userId = userId;
            this.actionType = actionType;
            InitUiData();
        }

        /// <summary>
        /// ��ʼ������չʾ
        /// </summary>
        private void InitUiData()
        {
            if (userId <= 0)
            {
                this.skinTextBox_addTime.SkinTxt.Text = DateTime.Now.ToString();
                return;
            }
            if (userId > 0)
            {
                BLL.ManagerBll managerBll = new BLL.ManagerBll();
                Model.UserModel userModel = managerBll.GetModel(userId);
                if (userModel == null)
                {
                    this.skinTextBox_addTime.SkinTxt.Text = DateTime.Now.ToString();
                    return;
                }

                if (userModel != null)
                {
                    this.skinTextBox_userName.SkinTxt.Text = userModel.UserName;
                    this.skinTextBox_realName.SkinTxt.Text = userModel.RealName;
                    this.skinTextBox_addTime.SkinTxt.Text = userModel.AddTime.ToString();
                    this.skinRadioButton_lock.Checked = userModel.IsLock == 1;
                    this.skinRadioButton_unLock.Checked = userModel.IsLock == 0;
                    this.skinComboBox_role.SelectedIndex = userModel.RoleType;

                    //���̳�ʼ��
                    if (userModel.Projects != null && userModel.Projects.Count > 0)
                    {
                        foreach (var item in userModel.Projects)
                        {
                            int index = this.skinDataGridView_projects.Rows.Add(1);
                            DataGridViewRow gridRow = this.skinDataGridView_projects.Rows[index];
                            gridRow.Cells[0].Value = item.ID;
                            gridRow.Cells[1].Value = item.ProjectName;
                            gridRow.Cells[2].Value = item.MonitoringSoftwareName;
                            gridRow.Cells[3].Value = item.MonitoringPath;
                            //gridRow.Cells[4].Value = item.UserName;
                            //gridRow.Cells[5].Value = item.IsLock;
                            //gridRow.Cells[6].Value = item.ClientIp;
                            //gridRow.Cells[7].Value = item.AddTime;
                            gridRow.Tag = item;
                        }

                        #region ��ʱɾ��
                        //DataTable dt = new DataTable();
                        //dt.Columns.Add("ID");
                        //dt.Columns.Add("ProjectName");
                        //dt.Columns.Add("MonitoringSoftwareName");
                        //dt.Columns.Add("MonitoringPath");
                        //dt.Columns.Add("UserName");
                        //dt.Columns.Add("IsLock");
                        //dt.Columns.Add("ClientIp");
                        //dt.Columns.Add("AddTime");

                        //foreach (var item in userModel.Projects)
                        //{
                        //    DataRow row = dt.NewRow();
                        //    row[0] = item.ID;
                        //    row[1] = item.ProjectName;
                        //    row[2] = item.MonitoringSoftwareName;
                        //    row[3] = item.MonitoringPath;
                        //    row[4] = item.UserName;
                        //    row[5] = item.IsLock;
                        //    row[6] = item.ClientIp;
                        //    row[7] = item.AddTime;
                        //    dt.Rows.Add(row);

                        //    int index = this.skinDataGridView_projects.Rows.Add(1);
                        //    DataGridViewRow gridRow = this.skinDataGridView_projects.Rows[index];
                        //    gridRow.Cells[0].Value = item.ID;
                        //    gridRow.Cells[1].Value = item.ProjectName;
                        //    gridRow.Cells[2].Value = item.MonitoringSoftwareName;
                        //    gridRow.Cells[3].Value = item.MonitoringPath;
                        //    gridRow.Cells[4].Value = item.UserName;
                        //    gridRow.Cells[5].Value = item.IsLock;
                        //    gridRow.Cells[6].Value = item.ClientIp;
                        //    gridRow.Cells[7].Value = item.AddTime;
                        //    gridRow.Tag = item;
                        //}
                        //this.skinDataGridView_projects.DataSource = dt;
                        //this.skinDataGridView_projects.DataSource = userModel.Projects;

                        #endregion
                    }
                }
            }
        }

        #region �����¼�
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton_add_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton_edit_Click(object sender, EventArgs e)
        {
            EditProject();
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton_del_Click(object sender, EventArgs e)
        {
            DelProject();
        }

        private void skinDataGridView_projects_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditProject();
        }

        /// <summary>
        /// ȷ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            Model.UserModel user = new Model.UserModel();
            BLL.ManagerBll managerBll = new BLL.ManagerBll();

            try
            {
                #region ��ϢУ��

                //У��
                int isLock = this.skinRadioButton_lock.Checked ? 1 : 0; ;
                string userName = this.skinTextBox_userName.SkinTxt.Text;
                string realName = this.skinTextBox_realName.SkinTxt.Text;
                int roleType = this.skinComboBox_role.SelectedIndex;
                string addTimeStr = this.skinTextBox_addTime.SkinTxt.Text.Trim();

                DateTime addTime = DateTime.Now;
                if (string.IsNullOrEmpty(userName))
                {
                    MessageBoxEx.Show("�˺Ų���Ϊ�գ�");
                    return;
                }

                if (string.IsNullOrEmpty(realName))
                {
                    MessageBoxEx.Show("��������Ϊ�գ�");
                    return;
                }

                if (roleType != 0 && roleType != 1)
                {
                    MessageBoxEx.Show("����ϵ����Ա���û���ɫ���쳣��");
                    return;
                }

                if (string.IsNullOrEmpty(addTimeStr) || !DateTime.TryParse(addTimeStr, out addTime))
                {
                    MessageBoxEx.Show("���ʱ�䲻��ȷ��");
                    return;
                }

                //�����û�У���û��Ƿ����
                if (userId <= 0)
                {
                    bool isExidted = managerBll.Exists(userName);
                    if (isExidted)
                    {
                        MessageBoxEx.Show(string.Format("�˺�: {0}�Ѿ����ڣ�", userName));
                        return;
                    }
                    //��ʼ�޸�ֵ
                }

                //�޸��û�
                if (userId > 0)
                {
                    user = managerBll.GetModel(userId);
                    //��ʼ�޸�ֵ
                }

                #endregion

                #region  ������Ϣ
                user.IsLock = isLock;
                user.UserName = userName;
                user.RealName = realName;
                user.RoleType = roleType;
                user.RoleName = this.skinComboBox_role.SelectedItem.ToString(); ;
                user.AddTime = addTime;

                //������Ϣ����
                user.Projects = new List<Model.UserProjectModel>();
                foreach (DataGridViewRow item in this.skinDataGridView_projects.Rows)
                {
                    Model.UserProjectModel projectModel = (Model.UserProjectModel)item.Tag;
                    if (projectModel == null)
                    {
                        continue;
                    }
                    user.Projects.Add(projectModel);
                }

                #endregion

                #region ������˺��ж�
                // ���˺���֤
                if (!AuthDomin(realName, userName))
                {
                    return;
                }
                #endregion

                //�޸��û�
                if (userId > 0)
                {
                    if (managerBll.Update(user))
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBoxEx.Show("�����û���Ϣ�쳣��");
                    }
                }
                else
                {
                    if (managerBll.Add(user) > 0)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBoxEx.Show("�����û���Ϣ�쳣��");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("�����û���Ϣ�쳣��" + ex.Message);
            }
        }

        private bool AuthDomin(string realName, string userName)
        {
            try
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
                        if (!Bll.ADHelper.UserExists(userName))
                        {
                            if (MessageBox.Show(
                             string.Format("�˺�: {0}����{1}�в����ڣ��Ƿ��������˺�", userName, config.Domin),
                            "�������˺�",
                             MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                FrmUserDomin frm = new FrmUserDomin(userName, realName);
                                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show(userName + "�����˺��Ѿ����ڣ�");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Error", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show("���ִ��� �� " + ex.Message);
                return true;
            }
        }

        #endregion

        #region �Ҽ��¼�
        /// <summary>
        /// �Ҽ��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinDataGridView_projects_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //��������ѡ��״̬�Ͳ��ٽ�������
                    if (this.skinDataGridView_projects.Rows[e.RowIndex].Selected == false)
                    {
                        this.skinDataGridView_projects.ClearSelection();
                        this.skinDataGridView_projects.Rows[e.RowIndex].Selected = true;
                    }
                    //ֻѡ��һ��ʱ���û��Ԫ��
                    if (this.skinDataGridView_projects.SelectedRows.Count == 1)
                    {
                        this.skinDataGridView_projects.CurrentCell = this.skinDataGridView_projects.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //���������˵�
                    this.skinContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void ToolStripMenuItem_Add_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        private void ToolStripMenuItem_Edit_Click(object sender, EventArgs e)
        {
            EditProject();
        }

        private void ToolStripMenuItem_Del_Click(object sender, EventArgs e)
        {
            DelProject();
        }

        #endregion

        public Model.UserProjectModel modifyOrAddItem;

        #region �¼�����ʵ��

        private void AddProject()
        {
            FrmUserProject frm = new FrmUserProject(null);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (modifyOrAddItem == null) return;
                //��������
                int index = this.skinDataGridView_projects.Rows.Add(1);
                DataGridViewRow gridRow = this.skinDataGridView_projects.Rows[index];
                gridRow.Cells[0].Value = modifyOrAddItem.ID;
                gridRow.Cells[1].Value = modifyOrAddItem.ProjectName;
                gridRow.Cells[2].Value = modifyOrAddItem.MonitoringSoftwareName;
                gridRow.Cells[3].Value = modifyOrAddItem.MonitoringPath;
                gridRow.Tag = modifyOrAddItem;
            }
        }

        private void EditProject()
        {
            var rows = this.skinDataGridView_projects.SelectedRows;
            if (rows == null || rows.Count == 0) return;
            var row = rows[0];
            if (row != null && row.Tag != null)
            {
                FrmUserProject frm = new FrmUserProject((Model.UserProjectModel)row.Tag);
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (modifyOrAddItem == null) return;
                    //�޸Ĺ���
                    //foreach (DataGridViewRow item in this.skinDataGridView_projects.Rows)
                    //{
                    //    if (item.Cells[0].Value!= null && item.Cells[0].Value.ToString() == modifyOrAddItem.ID.ToString())
                    //    {
                    DataGridViewRow gridRow = row;
                    gridRow.Cells[0].Value = modifyOrAddItem.ID;
                    gridRow.Cells[1].Value = modifyOrAddItem.ProjectName;
                    gridRow.Cells[2].Value = modifyOrAddItem.MonitoringSoftwareName;
                    gridRow.Cells[3].Value = modifyOrAddItem.MonitoringPath;
                    gridRow.Tag = modifyOrAddItem;
                    //    }
                    //}
                }
            }
        }

        private void DelProject()
        {
            var rows = this.skinDataGridView_projects.SelectedRows;
            if (rows == null || rows.Count == 0) return;
            var row = rows[0];
            if (row != null && row.Tag != null)
            {
                this.skinDataGridView_projects.Rows.Remove(row);
            }
        }
        #endregion
    }
}
