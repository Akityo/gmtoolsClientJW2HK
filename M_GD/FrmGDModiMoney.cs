using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using C_Controls.LabelTextBox;
using C_Global;
using C_Event;
using Language;
namespace M_GD
{
    [C_Global.CModuleAttribute("�޸Ľ�Ǯ", "FrmGDModiMoney", "�޸Ľ�Ǯ", "GD Group")]
    public partial class FrmGDModiMoney : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//��ҳҳ��

        int userID = 0;
        string serverIP = null;
        string serverName = null;
        string userName = null;
        string userNick = null;
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 
        private string userMoney = null;

        #endregion

        public FrmGDModiMoney()
        {
            InitializeComponent();
        }

        #region ��������еĴ���
        /// <summary>
        /// ��������еĴ���
        /// </summary>
        /// <param name="oParent">MDI ����ĸ�����</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>����еĴ���</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //������¼����
            FrmGDModiMoney mModuleFrm = new FrmGDModiMoney();
            mModuleFrm.m_ClientEvent = (CSocketEvent)oEvent;
            if (oParent != null)
            {
                mModuleFrm.MdiParent = (Form)oParent;
                mModuleFrm.Show();
            }
            else
            {
                mModuleFrm.ShowDialog();
            }

            return mModuleFrm;
        }
        #endregion

        #region ��ʼ�����Կ�
        /// <summary>
        ///�����ֿ�
        /// </summary>
        ConfigValue config = null;

        /// <summary>
        /// ��ʼ�����������Կ�
        /// </summary>
        private void IntiFontLib()
        {
            config = (ConfigValue)m_ClientEvent.GetInfo("INI");
            IntiUI();
        }

        private void IntiUI()
        {
            //this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            //this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            //this.lblAccount.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblNick.Text = config.ReadConfigValue("MSD", "UIC_UI_lblNick");
            //this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            //this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");

            //this.tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCharacter");

            //this.tpgModiLvl.Text = config.ReadConfigValue("MSD", "AM_UI_tpgModiLvl");
            //this.lblCharLvl.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblNewLvl.Text = config.ReadConfigValue("MSD", "AM_UI_lblNewLvl");
            //this.btnModiLvl.Text = config.ReadConfigValue("MSD", "AM_UI_btnModiLvl");
            //this.btnResetLvl.Text = config.ReadConfigValue("MSD", "BP_UI_btnReset");

            //this.tpgAddMoney.Text = config.ReadConfigValue("MSD", "AM_UI_tpgAddMoney");
            //this.lblCharMoney.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblMoney.Text = config.ReadConfigValue("MSD", "AM_UI_lblMoney");
            //this.btnAddMoney.Text = config.ReadConfigValue("MSD", "AM_UI_btnAddMoney");
            //this.btnResetMoney.Text = config.ReadConfigValue("MSD", "BP_UI_btnReset");

            //this.tpgModiPwd.Text = config.ReadConfigValue("MSD", "AM_UI_tpgModiPwd");
            //this.lblCharPwd.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblTempPwd.Text = config.ReadConfigValue("MSD", "AM_UI_lblTempPwd");
            //this.btnModiPwd.Text = config.ReadConfigValue("MSD", "AM_UI_btnModiPwd");
            //this.btnRecoverPwd.Text = config.ReadConfigValue("MSD", "AM_UI_btnRecoverPwd");
            //this.btnTempPwd.Text = config.ReadConfigValue("MSD", "AM_UI_btnTempPwd");

            tbcResult.Enabled = false;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDModiMoney_Load(object sender, EventArgs e)
        {
            try
            {
                IntiFontLib();

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];
                mContent[0].eName = CEnum.TagName.ServerInfo_GameDBID;
                mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[0].oContent = 1;

                mContent[1].eName = CEnum.TagName.ServerInfo_GameID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = m_ClientEvent.GetInfo("GameID_SD");

                this.backgroundWorkerFormLoad.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_GD.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbServer = Operation_GD.BuildCombox(mServerInfo, cmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region �л���ͬ����Ϸ������
        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region ��ѯ���������Ϣ
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                tbcResult.SelectedTab = tpgCharacter;//ѡ���ɫ��Ϣѡ�
                this.GrdCharacter.DataSource = null;

                if (cmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "UIA_Hint_selectServer"));
                    return;
                }

                serverIP = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text);
                serverName = cmbServer.Text.Trim();
                userName = txtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();

                if (txtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
                {
                    PartInfo();
                }
                else
                {
                    MessageBox.Show(config.ReadConfigValue("MMagic", "UIA_Hint_inPutAccont"));
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��ɫ������Ϣ
        private void PartInfo()
        {
            try
            {
                tbcResult.SelectedTab = tpgCharacter;//ѡ���ɫ��Ϣѡ�
                this.GrdCharacter.DataSource = null;

                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];
                //��ѯ��ҽ�ɫ��Ϣ
                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.SD_UserName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = userName;

                mContent[2].eName = CEnum.TagName.f_pilot;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userNick;

                backgroundWorkerSearch.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��ɫ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Account_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��ɫ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCharacter, out iPageCount);
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnModiMoney_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCharMoney.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AM_Hint_inputName"));
                    return;
                }
                if (txtContent.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_contentNull"));
                    return;
                }

                string itemComb = null;
                itemComb = "1070001 " + NudMoney.Value.ToString().Trim() + " G��|";

                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.UserByID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[2].eName = CEnum.TagName.f_idx;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = userID;

                mContent[3].eName = CEnum.TagName.SD_UserName;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = userName;

                mContent[4].eName = CEnum.TagName.SD_GC;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = int.Parse(this.NudMoney.Value.ToString());

                mContent[7].eName = CEnum.TagName.SD_Money_Old;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = int.Parse(this.txtCharMoney.Text.ToString());

                mContent[5].eName = CEnum.TagName.SD_Title;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = txtTitle.Text.Trim();

                mContent[6].eName = CEnum.TagName.SD_Content;
                mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[6].oContent = txtContent.Text.Trim();

           

                backgroundWorkerModiMoney.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerModiMoney_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UpdateMoney_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerModiMoney_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);

            PartInfo();
            this.txtCharMoney.Text = "";
            this.NudMoney.Value = 1;
            this.txtTitle.Text = "";
            this.txtContent.Text = "";
        }

        private void btnResetMoney_Click(object sender, EventArgs e)
        {
            this.NudMoney.Value = 1;
            this.txtTitle.Text = "";
            this.txtContent.Text="";

        }

        private void GrdCharacter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        tbcResult.SelectedTab = this.tpgModiMoney;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                    userNick = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_pilot")].ToString();
                    userMoney = mTable.Rows[selectChar]["G��"].ToString();
                    if (tbcResult.SelectedTab.Text.Equals("�޸Ľ��X"))
                    {
                        txtCharMoney.Text = userMoney;
                    }
                  
                }
                else
                {
                
                    txtCharMoney.Text = "";
                    NudMoney.Value = 1;
                    txtTitle.Text = "";
                    txtContent.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}