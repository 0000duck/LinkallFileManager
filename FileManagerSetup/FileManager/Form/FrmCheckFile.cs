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
using FileManager.DAL;

namespace FileManager
{
    public partial class FrmCheckFile : CCSkinMain
    {
        private string forderPath;
        private FileManager.Model.UserModel user;
        private int totalNum = 0;

        public FrmCheckFile(string forderPath,FileManager.Model.UserModel user)
        {
            this.forderPath = forderPath;
            this.user = user;
            InitializeComponent();
        }

        #region �ļ��ϴ����������������

        /// <summary>
        /// ������
        /// </summary>
        private static object _lockToActionFile = new object();

        /// <summary>
        /// ��ӻ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="file"></param>
        public static void AddFileToCache(string key, Model.FileModel file)
        {
            lock (_lockToActionFile)
            {
                if (!FileWinexploer.needAddOrMordifyFiles.ContainsKey(key))
                {
                    FileWinexploer.needAddOrMordifyFiles.Add(key, file);
                }
            }
        }

        public static void AddNeedAllUploadFilesToCache(string key, Model.FileModel file)
        {
            lock (_lockToActionFile)
            {
                if (!FileWinexploer.needAllUploadFiles.ContainsKey(key))
                {
                    FileWinexploer.needAllUploadFiles.Add(key, file);
                }
            }
        }

        private static void ClearCache()
        {
            FileWinexploer.needAllDownloadFiles.Clear();
            FileWinexploer.needAllUploadFiles.Clear();
            FileWinexploer.needAddOrMordifyFiles.Clear();
        }

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        private System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();

        /// <summary>
        /// ���ݿ�ִ������
        /// </summary>
        private System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();

        /// <summary>
        /// ���һ���ϴ�֮ǰ����ļ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CheckFolderUploadToDb(string forderPath, FileManager.Model.UserModel user)
        {
            try
            {
                string dirPath = forderPath;
                if (!Directory.Exists(dirPath))
                {
                    MessageBox.Show("����������·������ȷ������ϵ����Ա����");
                    return;
                }

                ClearCache();

                // ��ȡ������Ϣ
                Model.SystemConfig config = SystemConfigBll.GetConfig();

                // �������ݿ�����
                string conn = string.Format("server={0};uid={1};pwd={2};database={3};", config.DbAddress, config.DbUser, config.DbPassword, config.DbName);
                // ������
                using (connection = new System.Data.SqlClient.SqlConnection(conn))
                {
                    // �˴���ѯ�������ļ�����
                    DirectoryInfo rootDirInfo = new DirectoryInfo(dirPath);
                    this.textBox1.AppendText("\r\n�ļ��У�" + dirPath);

                    CheckFolderUploadAllToDb(rootDirInfo, 0, user.UserName, user.CurProject.ID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message, ex);
            }
        }

        /// <summary>
        /// �ݹ����ļ��к��ļ�
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <param name="parentId"></param>
        /// <param name="user"></param>
        private void CheckFolderUploadAllToDb(DirectoryInfo dirInfo, int parentId, string userName, int projectId)
        {
            // �������������ϴ������ݿ�
            ForderDalEx forderDal = new ForderDalEx("FM_");
            // �ϴ�֮ǰ�ж��Ƿ��Ѿ�����
            int curForderId = forderDal.GetForderId(this.connection, parentId, dirInfo.Name, userName, projectId);

            if (curForderId < 0)
            {
                //�ļ��в����ڣ�ȫ������
                List<Model.FileModel> fileLists = GetAllFiles(dirInfo);
                foreach (var item in fileLists)
                {
                    AddFileToCache(item.ClientPath, item);
                    AddNeedAllUploadFilesToCache(item.ClientPath, item);
                }
                return;
            }

            var files = dirInfo.GetFiles();
            //�ļ��ϴ�
            foreach (var item in files)
            {
                var file = CheckFileUploadToDb(item, curForderId, userName, projectId);
                if (file != null)
                {
                    AddFileToCache(file.ClientPath, file);
                }
                totalNum++;
                this.textBox1.AppendText("\r\n�ļ���" + item.FullName);
                Application.DoEvents();
            }
            var suvDirs = dirInfo.GetDirectories();
            if (suvDirs != null && suvDirs.Length > 0)
            {
                foreach (var item in suvDirs)
                {
                    //�ݹ����ļ���
                    CheckFolderUploadAllToDb(item, curForderId, userName, projectId);
                }
            }
        }

        /// <summary>
        /// ��ȡ�ļ����������ļ�Ϊ�����ʽ
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        private List<Model.FileModel> GetAllFiles(DirectoryInfo dirInfo)
        {
            List<Model.FileModel> fileModels = new List<Model.FileModel>();
            var files = dirInfo.GetFiles();
            //�ļ��ϴ�
            foreach (var item in files)
            {
                totalNum++;
                this.textBox1.AppendText("\r\n�ļ���" + item.FullName);
                Application.DoEvents();
                Model.FileModel f = new Model.FileModel()
                {
                    ActionNum = 1,
                    ClientPath = item.FullName,
                    File_Ext = item.Extension,
                    File_Size = (int)item.Length,
                    File_Name = item.Name,
                    File_Modify_Time = item.LastAccessTime.Ticks
                };
                fileModels.Add(f);
            }
            var suvDirs = dirInfo.GetDirectories();
            if (suvDirs != null && suvDirs.Length > 0)
            {
                foreach (var item in suvDirs)
                {
                    fileModels.AddRange(GetAllFiles(item));
                }
            }
            return fileModels;
        }

        /// <summary>
        /// �����ļ����
        /// </summary>
        /// <param name="item"></param>
        private Model.FileModel CheckFileUploadToDb(FileInfo item, int forderId, string userName, int projectId)
        {
            //string md5 = GetMD5HashFromFile(item.FullName);
            string md5 = "";

            //�ϴ��ļ�֮ǰ�ж��ļ��Ƿ��Ѿ�����
            FileDalEx fileDal = new FileDalEx("FM_");
            int fileID = fileDal.GetFileID(this.connection, forderId, item.Name, md5);
            //int fileID = 1;
            FileVersionDalEx fVerDal = new FileVersionDalEx("FM_");

            //�����ļ���Ϣ
            Model.FileModel file = null;
            //�ļ���Ϣ�����ڣ����½�һ��
            if (fileID > 0)
            {
                //�ж��Ƿ���Ҫ�����汾
                //int curVer = fileBll.GetFileLastVer(fileID, item.LastWriteTime);
                int curVer = 0;
                var lastFileVer = fileDal.GetFileLastVer(this.connection, fileID);
                if (lastFileVer != null)
                {
                    curVer = lastFileVer.Ver;
                }

                //����ļ��Ѿ�������ͬ�ļ��İ汾��������ϴ�
                if (curVer <= 0 || (lastFileVer != null && lastFileVer.File_Modify_Time < item.LastWriteTime.Ticks))
                {
                    file = new Model.FileModel()
                    {
                        ActionNum = 2,
                        ClientPath = item.FullName,
                        File_Ext = item.Extension,
                        File_Size = (int)item.Length,
                        File_Name = item.Name,
                        File_Modify_Time = item.LastAccessTime.Ticks
                    };
                    AddNeedAllUploadFilesToCache(item.FullName, file);
                }
            }
            //�ļ���Ϣ�����ڣ����½�һ��
            else
            {
                file = new Model.FileModel()
                {
                    ActionNum = 1,
                    ClientPath = item.FullName,
                    File_Ext = item.Extension,
                    File_Size = (int)item.Length,
                    File_Name = item.Name,
                    File_Modify_Time = item.LastAccessTime.Ticks
                };

                AddNeedAllUploadFilesToCache(item.FullName, file);
            }
            return file;
        }

        #endregion

        private void FrmCheckFile_Load(object sender, EventArgs e)
        {
        }
        private void FrmCheckFile_Shown(object sender, EventArgs e)
        {
            //CheckFolderUploadToDb(this.forderPath, this.user);
        }

        /// <summary>
        /// ������.�����������ڵ�ʱ����,���ص���ʱ���������ڲ�ľ���ֵ.
        /// </summary>
        /// <param name="DateTime1">��һ�����ں�ʱ��</param>
        /// <param name="DateTime2">�ڶ������ں�ʱ��</param>
        /// <returns></returns>
        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "��"
                        + ts.Hours.ToString() + "Сʱ"
                        + ts.Minutes.ToString() + "����"
                        + ts.Seconds.ToString() + "��";
            }
            catch
            {

            }
            return dateDiff;
        }

        /// <summary>
        /// ������.����һ��ʱ���뵱ǰ�������ں�ʱ���ʱ����,���ص���ʱ���������ڲ�ľ���ֵ.
        /// </summary>
        /// <param name="DateTime1">һ�����ں�ʱ��</param>
        /// <returns></returns>
        private string DateDiff(DateTime DateTime1)
        {
            return this.DateDiff(DateTime1, DateTime.Now);
        }
    }
}
