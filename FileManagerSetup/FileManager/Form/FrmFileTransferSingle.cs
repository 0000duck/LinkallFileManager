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

namespace FileManager
{
    public partial class FrmFileTransferSingle : CCSkinMain
    {
        private int actionType;
        public FrmFileTransferSingle(int actionType)
        {
            InitializeComponent();
            this.actionType = actionType;
            this.skinDataGridView10.AutoGenerateColumns = false;
            FrmFileUp();
        }

        private Color _baseColor = Color.FromArgb(0, 0, 64);//Color.FromArgb(0, 50, 90);
        private Color _borderColor = Color.FromArgb(64, 64, 0);
        private Color _progressBarBarColor = Color.Gold;
        private Color _progressBarBorderColor = Color.FromArgb(0, 95, 147);
        private Color _progressBarTextColor = Color.FromArgb(0, 95, 147);

        private readonly int SPEED_MAX = 3024 * 3224;
        private SkinFileTransfersItem item;
        private long totalSize;
        public void FrmFileUp()
        {
            var cacheToActionFiles = FileWinexploer.NeedAddOrMordifyFiles;
            string actionName = actionType == 0 ? "�ϴ��ļ���������" : "�����ļ����ͻ��˱���";
            FileTransfersItemStyle style = actionType == 0 ? FileTransfersItemStyle.Send : FileTransfersItemStyle.Receive;
            if (cacheToActionFiles != null && cacheToActionFiles.Count > 0)
            {
                //item = fileTansfersContainer1.AddItem(
                //    actionName,
                //    "",
                //    Resources._14,
                //    totalSize,
                //    style);
                //item.BaseColor = _baseColor;
                //item.BorderColor = _borderColor;
                //item.ProgressBarBarColor = _progressBarBarColor;
                //item.ProgressBarBorderColor = _progressBarBorderColor;
                //item.ProgressBarTextColor = _progressBarTextColor;
                //item.CancelButtonClick += new EventHandler(item_CancelButtonClick);

                this.skinFileTransfersItem1.Text = actionName;
                this.skinFileTransfersItem1.FileName = "sssssssssss";
                this.skinFileTransfersItem1.FileSize = 0;
                this.skinFileTransfersItem1.Text = actionName;
                this.skinFileTransfersItem1.Image = Resources._14;
                this.skinFileTransfersItem1.Style = style;

                //this.skinFileTransfersItem1.BaseColor = _baseColor;
                //this.skinFileTransfersItem1.BorderColor = _borderColor;
                this.skinFileTransfersItem1.ProgressBarBarColor = _progressBarBarColor;
                this.skinFileTransfersItem1.ProgressBarBorderColor = _progressBarBorderColor;
                this.skinFileTransfersItem1.ProgressBarTextColor = _progressBarTextColor;
                this.skinFileTransfersItem1.CancelButtonClick += new EventHandler(item_CancelButtonClick);
                this.skinFileTransfersItem1.Start();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("key");
            dt.Columns.Add("file_type");
            dt.Columns.Add("file_name");
            dt.Columns.Add("file_size");
            dt.Columns.Add("state");

            foreach (var item in cacheToActionFiles)
            {
                Model.FileModel file = item.Value;
                DataRow row = dt.NewRow();
                row[0] = file.ClientPath;
                row[1] = "�ļ�";
                row[2] = file.File_Name;
                row[3] = file.File_Size;
                row[4] = GetActionName(file.ActionNum);
                dt.Rows.Add(row);
                totalSize += file.File_Size;
            }
            this.skinDataGridView10.DataSource = dt;
        }
        private string GetActionName(int type)
        {
            switch (type)
            {
                case -1:
                    return "�ϴ��쳣";
                case 0:
                    return "������";
                case 1:
                    return "����";
                case 2:
                    return "�޸�";
                case 10:
                    return "�����";
                default:
                    return "�����";
            }
        }

        /// <summary>
        /// �ܾ��ļ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_RefuseButtonClick(object sender, EventArgs e)
        {
            SkinFileTransfersItem item = sender as SkinFileTransfersItem;
            MessageBox.Show(string.Format(
               "����� {0} - {1}���ܾ������ļ���",
               item.Text,
               item.FileName));
        }

        void item_SaveToButtonClick(object sender, EventArgs e)
        {
            SkinFileTransfersItem item = sender as SkinFileTransfersItem;
            MessageBox.Show(string.Format(
               "����� {0} - {1}�������ļ���...��",
               item.Text,
               item.FileName));
        }

        /// <summary>
        /// ȡ���ļ��ϴ����ļ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_CancelButtonClick(object sender, EventArgs e)
        {
            SkinFileTransfersItem item = sender as SkinFileTransfersItem;
            MessageBox.Show(string.Format(
                "ȡ���� {0} - {1}��ȡ�������ļ���",
                item.Text,
                item.FileName));
        }

        public static bool IsActionEnd(string key)
        {
            ///Ϊ��ֹ����������Ĭ�϶������״̬
            if (FileWinexploer.NeedAddOrMordifyFiles == null || FileWinexploer.NeedAddOrMordifyFiles.Count == 0) return true;
            Model.FileModel model;
            if (FileWinexploer.NeedAddOrMordifyFiles.TryGetValue(key, out model))
            {
                if (model == null) return true;
                return model.ActionNum == 10 || model.ActionNum == -1;
            }
            return true;
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
        /// ��ʱˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region ����
            int length = SPEED_MAX;
            if (this.skinFileTransfersItem1.TotalTransfersSize + length > skinFileTransfersItem1.FileSize)
            {
                this.skinFileTransfersItem1.TotalTransfersSize = 0;
                //�����ļ�
                Model.FileModel fileModel = LastActionFileName();
                if (fileModel != null)
                {
                    skinFileTransfersItem1.FileName = fileModel.File_Name;
                    skinFileTransfersItem1.FileSize = fileModel.File_Size;
                }
            }
            else
            {
                this.skinFileTransfersItem1.TotalTransfersSize += length;
            }
            #endregion

            #region ˢ������б�
            bool thisTimeHaveFile = false;
            foreach (DataGridViewRow row in this.skinDataGridView10.Rows)
            {
                if (row.Cells[4].Value.ToString() == "�����")
                {
                    continue;
                }
                string key = row.Cells[0].Value.ToString();
                if (IsActionEnd(key))
                {
                    row.Cells[4].Value = "�����";
                    row.Cells[4].Style.ForeColor = Color.Red;
                }
                else
                {
                    thisTimeHaveFile = true;
                }
            }

            if (!thisTimeHaveFile)
            {

                this.skinFileTransfersItem1.Enabled = false;
                this.skinFileTransfersItem1.Visible = false;

                if (fileTansfersContainer1.Controls.Count > 0)
                {
                    SkinFileTransfersItem item = (SkinFileTransfersItem)fileTansfersContainer1.Controls[fileTansfersContainer1.Controls.Count - 1];

                    fileTansfersContainer1.RemoveItem(item);
                }
                this.fileTansfersContainer1.Visible = false;
                this.skinPictureBox1.Visible = true;
                this.skinLabel19.Visible = true;
            }

            #endregion
        }

        private void FrmFileTransferNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            // ע���жϹر��¼�reason��Դ�ڴ��尴ť�������ò˵��˳�ʱ�޷��˳�!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //ȡ��"�رմ���"�¼�
                e.Cancel = true;
                this.skinFileTransfersItem1.Enabled = false;
                this.skinFileTransfersItem1.Visible = false;

                if (fileTansfersContainer1.Controls.Count > 0)
                {
                    SkinFileTransfersItem item = (SkinFileTransfersItem)fileTansfersContainer1.Controls[fileTansfersContainer1.Controls.Count - 1];
                    fileTansfersContainer1.RemoveItem(item);
                }
                this.fileTansfersContainer1.Visible = false;
                this.skinPictureBox1.Visible = true;
                this.skinLabel19.Visible = true;

                this.Dispose(true);
                return;
            }
        }
    }
}
