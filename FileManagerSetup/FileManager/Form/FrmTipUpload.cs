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
using CCWin.Win32;
using System.Runtime.Remoting.Messaging;
using FileManager.Model;

namespace FileManager
{
    public partial class FrmTipUpload : CCSkinMain
    {
        private string softwareName;

        public string SoftwareName
        {
            get { return softwareName; }
            set { softwareName = value; }
        }
        private Model.UserProjectModel curProject;
        public FrmTipUpload(string softwareName)
        {
            InitializeComponent();
            this.softwareName = softwareName;
        }

        public FrmTipUpload()
        {
            InitializeComponent();
            this.softwareName = string.Empty;
        }

        public const int AW_SLIDE = 262144;
        public const int AW_VER_NEGATIVE = 8;

        //���ڼ���ʱ
        private void FrmTipUpload_Load(object sender, EventArgs e)
        {
            //��ʼ�����ڳ���λ��
            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.PointToScreen(p);
            this.Location = p;
            this.Visible = true;
            NativeMethods.AnimateWindow(this.Handle, 130, AW_SLIDE + AW_VER_NEGATIVE);//��ʼ���嶯��
        }

        //����ʱ6��رյ�����
        private void timShow_Tick(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {

                Model.UserProjectModel curProject = null;
                Model.UserModel user = Bll.SystemBll.UserInfo;
                if (user == null || user.Projects == null || user.Projects.Count == 0)
                {
                    MessageBox.Show("�˺���Ϣ����ȷ������ϵ����Ա����");
                    return;
                }

                foreach (var item in user.Projects)
                {
                    if (item.MonitoringSoftwareName == softwareName)
                    {
                        curProject = item;
                        break;
                    }
                }

                if (curProject == null)
                {
                    MessageBox.Show("������Ϣ�󶨲���ȷ������ϵ����Ա����");
                    return;
                    //curProject = user.Projects[0];
                }

                if (curProject == null || string.IsNullOrEmpty(curProject.MonitoringPath))
                {
                    MessageBox.Show("����������·������ȷ������ϵ����Ա����");
                    return;
                }

                //��ذ�·��
                string dirPath = curProject.MonitoringPath;
                if (!Directory.Exists(dirPath))
                {
                    MessageBox.Show("����������·������ȷ������ϵ����Ա����");
                    return;
                }

                //�˴���ѯ�������ļ�����
                DirectoryInfo rootDirInfo = new DirectoryInfo(dirPath);
                FileWinexploer upload = new FileWinexploer();
               
                //�ϴ�֮ǰ�����ļ���Ӱ����
                user.CurProject = curProject;
                this.curProject = curProject;

                #region 20151104�޸��ϴ�����Ч��&�����ϴ������ļ���������--ɾ��
                //upload.CheckFolderUploadToDb(dirPath, user);
                //if (FileWinexploer.NeedAddOrMordifyFiles == null || FileWinexploer.NeedAddOrMordifyFiles.Count == 0)
                //{
                //    MessageBox.Show("û�пɸ����ļ�������Ҫ������");
                //    return;
                //}
                //else
                //{
                //    if (MessageBox.Show(
                //     string.Format("�˴��ϴ��ļ�����Ϊ:{0},�Ƿ�����ϴ���", FileWinexploer.NeedAddOrMordifyFiles.Count),
                //      "һ���ϴ��ļ�",
                //       MessageBoxButtons.OKCancel) == DialogResult.OK)
                //    {
                //        //���ͨ����ʼ�첽�ϴ�
                //        FolderUploadHandler handler = new FolderUploadHandler(upload.FolderUploadToDb);
                //        handler.BeginInvoke(dirPath, user, IAsyncMenthod, null);

                //        ///�첽��־
                //        ActionLog(string.Empty, ActionType.ALLUPLOAD);

                //        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                //        return;

                //        //��̬�ϴ�����
                //        FrmFileTransferNew frmTransfer = new FrmFileTransferNew(0);
                //        frmTransfer.Show();
                //    }
                //}
                #endregion

                #region 20151104�޸��ϴ�����Ч��&�����ϴ������ļ���������--�޸�

                // ��ʾ�Ƿ��ϴ�|ȡ��|�ϴ�����
                string tip = FileWinexploer.ConstructUploadTip();
                FrmMessageBox messageBox = new FrmMessageBox(tip, 0);
                var result = messageBox.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                // �滻������[8.5wjh]
                FrmWait frmCheckFile = new FrmWait(dirPath, user, 0);
                DialogResult frmCheckFileResult = frmCheckFile.ShowDialog();
                if (frmCheckFileResult != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                if (FileWinexploer.NeedAddOrMordifyFiles == null || FileWinexploer.NeedAddOrMordifyFiles.Count == 0)
                {
                    MessageBox.Show("�ϴ���ɣ�");
                    return;
                }
                else
                {
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        FileWinexploer.SetUploadCache();
                    }

                    // 0909 �ϴ��������޸�
                    //FrmFileProgressBar frmTransfer = new FrmFileProgressBar(0);
 
                    //���ͨ����ʼ�첽�ϴ�
                    FolderUploadHandler handler = new FolderUploadHandler(upload.FolderUploadToDb);
                    handler.BeginInvoke(dirPath, user, IAsyncMenthod, null);

                    ///�첽��־
                    ActionLog(string.Empty, ActionType.ALLUPLOAD);

                    //frmTransfer.Activate();
                    //frmTransfer.Show();
                }
                #endregion

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// �ϴ�����ί��
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public delegate void FolderUploadHandler(string a, FileManager.Model.UserModel b);

        /// <summary>
        /// �첽�ص�
        /// </summary>
        /// <param name="result"></param>
        void IAsyncMenthod(IAsyncResult result)
        {
            FolderUploadHandler handler = (FolderUploadHandler)((AsyncResult)result).AsyncDelegate;
            handler.EndInvoke(result);
        }

        /// <summary>
        /// ������־��¼
        /// </summary>
        /// <param name="clientPath"></param>
        /// <param name="action"></param>
        private void ActionLog(string clientPath, ActionType action)
        {
            if (curProject == null) return;

            if (string.IsNullOrEmpty(clientPath)) clientPath = curProject.MonitoringPath;

            Bll.SystemBll.ActionLogAsyn(curProject.ID, curProject.ProjectName, clientPath, action);
        }

        private void skinButton_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmTipUpload_Shown(object sender, EventArgs e)
        {

        }
    }
}
