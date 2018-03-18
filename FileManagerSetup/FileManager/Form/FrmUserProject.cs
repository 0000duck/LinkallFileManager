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

namespace FileManager
{
    public partial class FrmUserProject : CCSkinMain
    {
        private Model.UserProjectModel  userProject;
        public FrmUserProject(Model.UserProjectModel userProject)
        {
            InitializeComponent();
            this.userProject = userProject;
            InitUiData();
        }

        /// <summary>
        /// �������ݰ�
        /// </summary>
        private void InitUiData()
        {
            if (userProject != null)
            {
                this.skinRadioButton_unLock.Checked = userProject.IsLock == 0;
                this.skinRadioButton_lock.Checked = userProject.IsLock == 1;

                this.skinTextBox_projectName.SkinTxt.Text = userProject.ProjectName;
                this.skinTextBox_monitoringPath.SkinTxt.Text = userProject.MonitoringPath;
                this.skinTextBox_monitoringSoftwareName.SkinTxt.Text = userProject.MonitoringSoftwareName;
                this.skinTextBox_addTime.SkinTxt.Text = userProject.AddTime.ToString();
            }
            else
            {
                this.skinTextBox_addTime.SkinTxt.Text =DateTime.Now.ToString();
            }
        }

        /// <summary>
        /// ·��ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton_selectPath_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            string path = this.folderBrowserDialog1.SelectedPath;
            this.skinTextBox_monitoringPath.SkinTxt.Text = path;
        }

        /// <summary>
        /// ȷ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (userProject == null)
            {
                userProject = new Model.UserProjectModel();
            }

            int isLock = this.skinRadioButton_lock.Checked ? 1 : 0; ;
            string projectName = this.skinTextBox_projectName.SkinTxt.Text;
            string monitoringPath = this.skinTextBox_monitoringPath.SkinTxt.Text;
            string monitoringSoftwareName = this.skinTextBox_monitoringSoftwareName.SkinTxt.Text;
            string addTimeStr = this.skinTextBox_addTime.SkinTxt.Text.Trim();

            DateTime addTime = DateTime.Now;
            if (string.IsNullOrEmpty(projectName))
            {
                MessageBoxEx.Show("������Ϣ����Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(monitoringPath))
            {
                MessageBoxEx.Show("����·������Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(monitoringSoftwareName))
            {
                MessageBoxEx.Show("���������Ʋ���Ϊ�գ�");
                return;
            }

            if (string.IsNullOrEmpty(addTimeStr) || !DateTime.TryParse(addTimeStr, out addTime))
            {
                MessageBoxEx.Show("���ʱ�䲻��ȷ��");
                return;
            }

            userProject.IsLock = isLock;
            userProject.ProjectName = projectName;
            userProject.MonitoringPath = monitoringPath;
            userProject.MonitoringSoftwareName = monitoringSoftwareName;
            userProject.AddTime = addTime;
            FrmUserEdit userForm = (FrmUserEdit)this.Owner;
            userForm.modifyOrAddItem = userProject;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
