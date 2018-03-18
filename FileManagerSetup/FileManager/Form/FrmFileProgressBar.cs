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
using CCWin.SkinControl;
using FileManager.Properties;
using System.Threading;

namespace FileManager
{
    public partial class FrmFileProgressBar : CCSkinMain
    {
        private int actionType;
        public FrmFileProgressBar(int actionType)
        {
            InitializeComponent();
            this.actionType = actionType;
            FrmFileUp();
        }

        private SkinFileTransfersItem item;
        private long totalSize;
        public void FrmFileUp()
        {
            var cacheToActionFiles = FileWinexploer.NeedAddOrMordifyFiles;
            string actionName = actionType == 0 ? "�ϴ��ļ���������" : "�����ļ����ͻ��˱���";
            FileTransfersItemStyle style = actionType == 0 ? FileTransfersItemStyle.Send : FileTransfersItemStyle.Receive;

            if (cacheToActionFiles != null && cacheToActionFiles.Count > 0)
            {
                this.skinProgressBar1.Maximum = cacheToActionFiles.Count;
            }
        }

        public static int IsActionEnd(string key)
        {
            ///Ϊ��ֹ����������Ĭ�϶������״̬
            if (FileWinexploer.NeedAddOrMordifyFiles == null || FileWinexploer.NeedAddOrMordifyFiles.Count == 0) return 1;
            Model.FileModel model;
            if (FileWinexploer.NeedAddOrMordifyFiles.TryGetValue(key, out model))
            {
                if (model == null) return 1;
                if (model.ActionNum == 10)
                {
                    return 1;
                }
                else if (model.ActionNum == -1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            return 1;
        }

        public static Model.FileModel LastActionFileName()
        {
            ///Ϊ��ֹ����������Ĭ�϶������״̬
            if (FileWinexploer.NeedAddOrMordifyFiles == null || FileWinexploer.NeedAddOrMordifyFiles.Count == 0) return null;
            foreach (var item in FileWinexploer.NeedAddOrMordifyFiles.Keys)
            {
                Model.FileModel mode = FileWinexploer.NeedAddOrMordifyFiles[item];
                if (mode != null && mode.ActionNum != 10) return mode;
            }
            return null;
        }

        /// <summary>
        /// ��ʱ�洢
        /// </summary>
        private Dictionary<string, Model.FileModel> DoneAddOrMordifyFiles = new Dictionary<string, Model.FileModel>();
        private Dictionary<string, string> DoneNotSuccessFiles = new Dictionary<string, string>();

        /// <summary>
        /// ��ȡĿǰ�������ļ�����
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Model.FileModel> GetAddFiles()
        {
            Dictionary<string, Model.FileModel> retDics = new Dictionary<string, Model.FileModel>();
            if (FileWinexploer.NeedAddOrMordifyFiles == null || FileWinexploer.NeedAddOrMordifyFiles.Count == 0)
            {
                return retDics;
            }

            foreach (var item in FileWinexploer.NeedAddOrMordifyFiles.Keys)
            {
                if (string.IsNullOrEmpty(item)) continue;

                Model.FileModel mode = FileWinexploer.NeedAddOrMordifyFiles[item];
                if (DoneAddOrMordifyFiles.ContainsKey(item))
                {
                    continue;
                }
                if (mode == null || mode.ActionNum != 10)
                {
                    continue;
                }
                DoneAddOrMordifyFiles.Add(item, mode);
                retDics.Add(item, mode);
            }
            return retDics;
        }

        /// <summary>
        /// ��ȡĿǰ�������ļ�����
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetNotSuccessFiles()
        {
            Dictionary<string, string> retDics = new Dictionary<string, string>();
            if (FileWinexploer.NotSuccessFiles == null || FileWinexploer.NotSuccessFiles.Count == 0)
            {
                return retDics;
            }

            foreach (string item in FileWinexploer.NotSuccessFiles.Keys)
            {
                if (string.IsNullOrEmpty(item)) continue;

                if (DoneNotSuccessFiles.ContainsKey(item))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                DoneNotSuccessFiles.Add(item, item);
                retDics.Add(item, item);
            }
            return retDics;
        }

        /// <summary>
        /// ��ʱˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            int addCount = GetNotEndCount();
            int totalCount = FileWinexploer.NeedAddOrMordifyFiles.Count;

            // �����Ѿ��ϴ����ļ�
            var addItems = GetAddFiles();
            if (addItems.Count > 0)
            {
                foreach (var item in addItems.Keys)
                {
                    Model.FileModel mode = addItems[item];

                    if (!FileWinexploer.NotSuccessFiles.ContainsKey(item))
                    {
                        this.textBox1.AppendText(string.Format("\r\n�ļ�{0}�ɹ����ͻ���·��{1}", actionType == 1 ? "����" : "�ϴ�", item));
                        Application.DoEvents();
                    }
                }
            }

            if (addCount > 0)
            {
                this.skinProgressBar1.Value = FileWinexploer.NeedAddOrMordifyFiles.Count - addCount;
                this.skinLabel1.Text = string.Format("�ܹ���Ҫ{2}{0}���ļ���Ŀǰ��{2}{1}���ļ�;��", totalCount, this.skinProgressBar1.Value, actionType == 1 ? "����" : "�ϴ�");
                Application.DoEvents();
            }
            else
            {
                this.skinProgressBar1.Value = FileWinexploer.NeedAddOrMordifyFiles.Count - addCount;
                this.skinLabel1.Text = string.Format("�ܹ���Ҫ{2}{0}���ļ���Ŀǰ��{2}{1}���ļ�;�Ѿ���ɣ�", totalCount, this.skinProgressBar1.Value, actionType == 1 ? "����" : "�ϴ�");
                Application.DoEvents();

                this.skinLabel_tip.Visible = true;
                this.skinLabel_tip.Text = string.Format("��ϲ����{0}��ɣ�", actionType == 1 ? "����" : "�ϴ�");
                Application.DoEvents();

                timer1.Stop();
            }

            // �����Ѿ��ϴ����ļ�
            var addNotSuccessItems = GetNotSuccessFiles();
            if (addNotSuccessItems.Count > 0)
            {
                foreach (var item in addNotSuccessItems.Keys)
                {
                    this.textBox2.AppendText(string.Format("\r\n�ļ�{0}û�гɹ����ͻ���·��{1}", actionType == 1 ? "����" : "�ϴ�", item));
                    Application.DoEvents();
                }
            }

            this.skinLabel_success.Text = string.Format("�ɹ�{1}�ļ���{0}��", (this.skinProgressBar1.Value - DoneNotSuccessFiles.Count).ToString(), actionType == 1 ? "����" : "�ϴ�");
            this.skinLabel_fail.Text = string.Format("δ�ɹ�{1}�ļ���{0}��", DoneNotSuccessFiles.Count, actionType == 1 ? "����" : "�ϴ�");
            Application.DoEvents();
        }

        private void FrmFileTransferNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ע���жϹر��¼�reason��Դ�ڴ��尴ť�������ò˵��˳�ʱ�޷��˳�!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //ȡ��"�رմ���"�¼�
                e.Cancel = true;
                try
                {
                    this.timer1.Stop();
                    Thread.Sleep(1000);
                    this.Dispose(true);
                }
                catch (Exception ex)
                {
                }
                return;
            }
        }

        /// <summary>
        /// ��ȡδ�������
        /// </summary>
        /// <returns></returns>
        private int GetNotEndCount()
        {
            int totalCount = 0;
            if (FileWinexploer.NeedAddOrMordifyFiles != null && FileWinexploer.NeedAddOrMordifyFiles.Count > 0)
            {
                foreach (var item in FileWinexploer.NeedAddOrMordifyFiles.Keys)
                {
                    Model.FileModel mode = FileWinexploer.NeedAddOrMordifyFiles[item];
                    if (mode != null && mode.ActionNum != 10)
                    {
                        totalCount++;
                    }
                }
            }
            return totalCount;
        }
    }
}
