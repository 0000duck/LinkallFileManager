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
    public partial class FrmFileTransferOld : CCSkinMain
    {
        public FrmFileTransferOld()
        {
            InitializeComponent();
            FrmFileUp();
        }

        private Random _random;
        private Color _baseColor = Color.FromArgb(0, 0, 64);//Color.FromArgb(0, 50, 90);
        private Color _borderColor = Color.FromArgb(64, 64, 0);
        private Color _progressBarBarColor = Color.Gold;
        private Color _progressBarBorderColor = Color.FromArgb(0, 95, 147);
        private Color _progressBarTextColor = Color.FromArgb(0, 95, 147);

        public void FrmFileUp()
        {
            //propertyGrid1.SelectedObject = fileTansfersContainer1;
            _random = new Random();
            //SkinFileTransfersItem item;
            //item = fileTansfersContainer1.AddItem(
            //    "�����ļ�",
            //    "123.rar",
            //    Resources._14,
            //    1024 * 1024 * 20,
            //     FileTransfersItemStyle.Send);
            //item.BaseColor = _baseColor;
            //item.BorderColor = _borderColor;
            //item.ProgressBarBarColor = _progressBarBarColor;
            //item.ProgressBarBorderColor = _progressBarBorderColor;
            //item.ProgressBarTextColor = _progressBarTextColor;
            //item.CancelButtonClick += new EventHandler(item_CancelButtonClick);
            
            //item.Start();
            //item = fileTansfersContainer1.AddItem(
            //    "�����ļ�",
            //    "456.rar",
            //    Resources._14,
            //    1024 * 1024 * 10,
            //     FileTransfersItemStyle.Receive);
            //item.CancelButtonClick += new EventHandler(item_CancelButtonClick);
            //item.Start();

            //item = fileTansfersContainer1.AddItem(
            //   "�����ļ�",
            //   "789.rar",
            //   Resources._14 ,
            //   1024 * 1024 * 15,
            //    FileTransfersItemStyle.ReadyReceive);
            //item.SaveButtonClick += new EventHandler(item_SaveButtonClick);
            //item.SaveToButtonClick += new EventHandler(item_SaveToButtonClick);
            //item.RefuseButtonClick += new EventHandler(item_RefuseButtonClick);
            //item.Start();

            for (int i = 0; i < 100; i++)
            {
                //Create();
                SkinFileTransfersItem item;
                item = fileTansfersContainer1.AddItem(
                    "�����ļ�",
                    "123.rar",
                    Resources._14,
                    1024 * 1024 * 20,
                     FileTransfersItemStyle.Send);
                item.BaseColor = _baseColor;
                item.BorderColor = _borderColor;
                item.ProgressBarBarColor = _progressBarBarColor;
                item.ProgressBarBorderColor = _progressBarBorderColor;
                item.ProgressBarTextColor = _progressBarTextColor;
                item.CancelButtonClick += new EventHandler(item_CancelButtonClick);

                //item.Start();
            }
            DataTable dt = new DataTable();

            dt.Columns.Add("file_type");
            dt.Columns.Add("file_name");
            dt.Columns.Add("file_size");
            dt.Columns.Add("state");

            for (int i = 0; i < 1000; i++)
            {
                DataRow row = dt.NewRow();
                row[0] = "�ļ�";
                row[1] = "�ļ�";
                row[2] = "10M";
                row[3] = "������";
                dt.Rows.Add(row);
            }

            this.skinDataGridView10.DataSource = dt;
        }

        /// <summary>
        /// �����ļ����䶯̬Ч��
        /// </summary>
        private void Create()
        {
            SkinFileTransfersItem item;
            item = fileTansfersContainer1.AddItem(
                "�����ļ�",
                "123.rar",
                Resources._14,
                1024 * 1024 * 20,
                 FileTransfersItemStyle.Send);
            item.BaseColor = _baseColor;
            item.BorderColor = _borderColor;
            item.ProgressBarBarColor = _progressBarBarColor;
            item.ProgressBarBorderColor = _progressBarBorderColor;
            item.ProgressBarTextColor = _progressBarTextColor;
            item.CancelButtonClick += new EventHandler(item_CancelButtonClick);

            //item.Start();
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
        /// �ļ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_SaveButtonClick(object sender, EventArgs e)
        {
            SkinFileTransfersItem item = sender as SkinFileTransfersItem;
            MessageBox.Show(string.Format(
               "����� {0} - {1}�������ļ���",
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
                "����� {0} - {1}��ȡ�������ļ���",
                item.Text,
                item.FileName));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //int length = _random.Next(800, 1500) * 1024;

            bool thisTimeHaveFile = true;
            foreach (SkinFileTransfersItem item in fileTansfersContainer1.Controls)
            {
                int length = _random.Next(8, 150) * 1024;

                if (item.TotalTransfersSize + length >
                    item.FileSize)
                {
                    item.TotalTransfersSize = 0;
                    item.Visible = false;
                    thisTimeHaveFile = false;
                }
                else
                {
                    item.TotalTransfersSize += length;
                }
            }
            if (!thisTimeHaveFile)
            {
                this.fileTansfersContainer1.Visible = false;
                this.skinPictureBox1.Visible = true;
                this.skinLabel19.Visible = true;
            }

            foreach ( DataGridViewRow row in this.skinDataGridView10.Rows)
            {
                row.Cells[3].Value = "�Ѿ����";
                row.Cells[3].Style.ForeColor = Color.Red;
            }

        }
    }
}
