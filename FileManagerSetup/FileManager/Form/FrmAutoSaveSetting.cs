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
    public partial class FrmAutoSaveSetting : CCSkinMain
    {
        private Model.SystemConfig config;
        public FrmAutoSaveSetting()
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
                int autoUpdateInterval = config.AutoUpdateInterval;
                skinTextBox_setValue.SkinTxt.Text = autoUpdateInterval.ToString();
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
                string setValue = skinTextBox_setValue.SkinTxt.Text;
                int autoSaveSetValue = 0;
                if (string.IsNullOrEmpty(setValue))
                {
                    MessageBox.Show("�������ȷ����");
                    return;
                }
                if (!int.TryParse(setValue, out autoSaveSetValue) || autoSaveSetValue <= 0)
                {
                    MessageBox.Show("�������������");
                    return;
                }

                if (autoSaveSetValue < 120)
                {
                    MessageBox.Show("�����120��������");
                    return;
                }
                config.AutoUpdateInterval = autoSaveSetValue;

                new SystemConfigBll().saveConifg(config);

                // �ص�timer,����
                FrmMain mainForm = FrmMainSingle.MainForm;

                if (mainForm != null)
                {
                    mainForm.InitAutoUpdate();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܷ������쳣 : ", ex.Message);
                return;
            }

            MessageBox.Show("���óɹ���");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
