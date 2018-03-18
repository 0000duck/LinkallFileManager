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
using FileManager.Bll;

namespace FileManager
{
    public partial class FrmDominSetting : CCSkinMain
    {
        private Model.SystemConfig config;
        public FrmDominSetting()
        {
            InitializeComponent();
            InitUiData();
        }

        private void InitUiData()
        {
            // ��ȡ������Ϣ
            config = BLL.SystemConfigBll.GetConfig();
            if (config != null)
            {
                skinTextBox_chemPath.SkinTxt.Text = config.Chem32Path;
                skinTextBox_dominName.SkinTxt.Text = config.Domin;
                skinTextBox_softwareName.SkinTxt.Text = config.SoftwareName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (config == null)
                {
                    MessageBox.Show("�������ô���! ����������");
                    return;
                }
                string chemPath = skinTextBox_chemPath.SkinTxt.Text;
                string domin = skinTextBox_dominName.SkinTxt.Text;
                string softwareName = skinTextBox_softwareName.SkinTxt.Text;

                if (string.IsNullOrEmpty(chemPath))
                {
                    MessageBox.Show("��ѡ����ȷ��·��");
                    return;
                }

                if (string.IsNullOrEmpty(softwareName))
                {
                    MessageBox.Show("��ѡ����ȷ�Ľ�������");
                    return;
                }

                if (string.IsNullOrEmpty(domin))
                {
                    MessageBox.Show("����д��ȷ������");
                    return;
                }
                config.Chem32Path = chemPath;
                config.Domin = domin;
                config.SoftwareName = softwareName;
                new SystemConfigBll().saveConifg(config);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܷ������쳣 : ", ex.Message);
                return;
            }

            MessageBox.Show("���óɹ���");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void skinButton_selectPath_Click(object sender, EventArgs e)
        {
            DialogResult selectDialog = this.openFileDialog1.ShowDialog();
            if (selectDialog == System.Windows.Forms.DialogResult.OK)
            {
                this.skinTextBox_chemPath.SkinTxt.Text = this.openFileDialog1.FileName;
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            FrmSystemSetting frm = new FrmSystemSetting(0);
            frm.ShowDialog();
        }
    }
}
