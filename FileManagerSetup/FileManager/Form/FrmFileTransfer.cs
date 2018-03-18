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
    public partial class FrmFileTransfer : CCSkinMain
    {
        private int actionType;
        public FrmFileTransfer(int actionType)
        {
            InitializeComponent();
            this.actionType = actionType;
            this.skinDataGridView10.AutoGenerateColumns = false;
            FrmFileUp();
        }

        private Random _random;
        private Color _baseColor = Color.FromArgb(0, 0, 64);//Color.FromArgb(0, 50, 90);
        private Color _borderColor = Color.FromArgb(64, 64, 0);
        private Color _progressBarBarColor = Color.Gold;
        private Color _progressBarBorderColor = Color.FromArgb(0, 95, 147);
        private Color _progressBarTextColor = Color.FromArgb(0, 95, 147);

        private readonly int SPEED_MAX = 1024 * 2224;
        public void FrmFileUp()
        {
            //propertyGrid1.SelectedObject = fileTansfersContainer1;
            //_random = new Random();

            var cacheToActionFiles = FileWinexploer.NeedAddOrMordifyFiles;
            //List<Model.FileModel> models = new List<Model.FileModel>();
            //foreach (var item in cacheToActionFiles.Values)
            //{
            //    if (item.ActionNum != 10)
            //    {
            //        models.Add(item);
            //    }
            //}

            string actionName = actionType == 0 ? "�ϴ��ļ���������" : "�����ļ����ͻ��˱���";
            FileTransfersItemStyle style = actionType == 0 ? FileTransfersItemStyle.Send : FileTransfersItemStyle.Receive;
            foreach (var fileItem in cacheToActionFiles)
            {
                Model.FileModel file = fileItem.Value;
                if (file.ActionNum != 10)
                {
                    SkinFileTransfersItem item;
                    item = fileTansfersContainer1.AddItem(
                        actionName,
                        file.File_Name,
                        Resources._14,
                        file.File_Size,
                        style);

                    item.Tag = file.ClientPath;

                    item.BaseColor = _baseColor;
                    item.BorderColor = _borderColor;
                    item.ProgressBarBarColor = _progressBarBarColor;
                    item.ProgressBarBorderColor = _progressBarBorderColor;
                    item.ProgressBarTextColor = _progressBarTextColor;
                    item.CancelButtonClick += new EventHandler(item_CancelButtonClick);
                }
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
            }
            this.skinDataGridView10.DataSource = dt;
        }

        private string GetActionName(int type)
        {
            switch (type)
            {
                case -1:
                    return "�����쳣";
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
                return model.ActionNum == 10;
            }
            return true;
        }

        /// <summary>
        /// ��ʱˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //int length = _random.Next(800, 1500) * 1024;

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
                    #region ����

                    //if (this.skinDataGridView10.Rows.Count > 100)
                    //{
                    //    if (fileTansfersContainer1.Controls.Count > 0)
                    //    {
                    //        SkinFileTransfersItem item = (SkinFileTransfersItem)fileTansfersContainer1.Controls[fileTansfersContainer1.Controls.Count - 1];
                            
                    //        int length = SPEED_MAX;

                    //        if (item.TotalTransfersSize + length > item.FileSize)
                    //        {
                    //            if (IsActionEnd(key))
                    //            {
                    //                item.Visible = false;
                    //                thisTimeHaveFile = false;
                    //                fileTansfersContainer1.RemoveItem(item);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            item.TotalTransfersSize += length;
                    //        }
                    //    }
                    //    return;
                    //}

                    #endregion

                    thisTimeHaveFile = true;
                }
            }

            if (!thisTimeHaveFile)
            {
                this.fileTansfersContainer1.Visible = false;
                this.skinPictureBox1.Visible = true;
                this.skinLabel19.Visible = true;
            }

            



            if (fileTansfersContainer1.Controls.Count > 0)
            {
                SkinFileTransfersItem item = (SkinFileTransfersItem)fileTansfersContainer1.Controls[fileTansfersContainer1.Controls.Count -1];
                string key = item.Tag.ToString();
                int length = SPEED_MAX;

                if (item.TotalTransfersSize + length > item.FileSize)
                {
                    //item.TotalTransfersSize = 0;
                    if (IsActionEnd(key))
                    {
                        item.Visible = false;
                        thisTimeHaveFile = false;
                        fileTansfersContainer1.RemoveItem(item);
                    }
                }
                else
                {
                    item.TotalTransfersSize += length;
                }
            }
            else
            {
                this.fileTansfersContainer1.Visible = false;
                this.skinPictureBox1.Visible = true;
                this.skinLabel19.Visible = true;
            }

            //foreach (SkinFileTransfersItem item in fileTansfersContainer1.Controls)
            //{
            //    if (item.Tag == null)
            //    {
            //        item.Visible = false;
            //    }

            //    string key = item.Tag.ToString();
            //    int length = SPEED_MAX;

            //    if (item.TotalTransfersSize + length > item.FileSize)
            //    {
            //        //item.TotalTransfersSize = 0;
            //        if (IsActionEnd(key))
            //        {
            //            item.Visible = false;
            //            thisTimeHaveFile = false;
            //        }
            //    }
            //    else
            //    {
            //        item.TotalTransfersSize += length;
            //    }
            //}
            //if (!thisTimeHaveFile)
            //{
            //    this.fileTansfersContainer1.Visible = false;
            //    this.skinPictureBox1.Visible = true;
            //    this.skinLabel19.Visible = true;
            //}

            
        }
    }
}
