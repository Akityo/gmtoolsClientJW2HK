using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using C_Global;
using C_Event;
using Language;

namespace M_RC
{
    [C_Global.CModuleAttribute("����[���", "FrmAddMoney", "����[���", "raycity")]        
    public partial class FrmAddMoney : Form
    {
        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;
        private bool bFirst = false;
        private int iPageCount = 0;
        public FrmAddMoney()
        {
            InitializeComponent();
        }

        #region �Զ�������¼�
        /// <summary>
        /// ��������еĴ���
        /// </summary>
        /// <param name="oParent">MDI ����ĸ�����</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>����еĴ���</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //������¼����
            FrmAddMoney mModuleFrm = new FrmAddMoney();
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

        #region ���Կ�
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
            //this.Text = config.ReadConfigValue("MRC", "FRC_CODE_FrmAddMoney");
            //this.GrpSearch.Text = config.ReadConfigValue("MRC", "FRC_CODE_GrpSearch");
            //this.LblServer.Text = config.ReadConfigValue("MRC", "FRC_UI_LblServer");
            //this.LblAccount.Text = config.ReadConfigValue("MRC", "FRC_UI_LblPlayAccount");
            //this.label9.Text = config.ReadConfigValue("MRC", "FRC_UI_LblPlayNickName");
            //this.tabPage1.Text = config.ReadConfigValue("MRC", "QueryPlayerAccont");
            //this.tabPage2.Text = config.ReadConfigValue("MRC", "FRC_CODE_FrmAddMoney");
            //this.btnAdd.Text = config.ReadConfigValue("MRC", "FRC_UI_FrmQueryRaceStateAdd");
            //BtnClear.Text = config.ReadConfigValue("MRC", "FRC_UI_BtnClear");
            //LblPage.Text = config.ReadConfigValue("MRC", "FRC_UI_LblSelectedPage");
            //this.label6.Text = config.ReadConfigValue("MRC", "FRC_CODE_Tip1");
       
        }


        #endregion

        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmQueryPlayerID_Load(object sender, EventArgs e)
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
                mContent[1].oContent = m_ClientEvent.GetInfo("GameID_RC");

                this.backgroundWorkerFormLoad.RunWorkerAsync(mContent);
            }
            catch
            { }
        }
        #endregion

        #region ������ɫ��Ϣ
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (TxtAccount.Text.Length > 0 || TxtNick.Text.Length > 0)
            {
                this.BtnSearch.Enabled = false;
                this.RoleInfoView.DataSource = null;
                this.CmbPage.Items.Clear();
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.RayCity_NyUserID;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = TxtAccount.Text;

                mContent[1].eName = CEnum.TagName.RayCity_ServerIP;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = Operation_RCode.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[2].eName = CEnum.TagName.RayCity_NyNickName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = TxtNick.Text;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_RCode.iPageSize;

                backgroundWorkerSerch.RunWorkerAsync(mContent);
            }
            else
            {
                MessageBox.Show(config.ReadConfigValue("MRC", "FQP_Code_inputid"));
            }
        }
        #endregion

        #region ����ر�
        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ����������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_RCode.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ����������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CmbServer = Operation_RCode.BuildCombox(mServerInfo, CmbServer);
        }
        #endregion

        #region ��ѯ��ɫ��Ϣbackgroundworker��Ϣ����
        private void backgroundWorkerSerch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_RCode.GetResult(tmp_ClientEvent, CEnum.ServiceKey.RayCity_BasicAccount_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��ɫ��Ϣbackgroundworker��Ϣ����
        private void backgroundWorkerSerch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.BtnSearch.Enabled = true;
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_RCode.BuildDataTable(this.m_ClientEvent, mResult, RoleInfoView, out iPageCount);


            if (iPageCount <= 1)
            {
                PnlPage.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    CmbPage.Items.Add(i + 1);
                }

                CmbPage.SelectedIndex = 0;
                bFirst = true;
                PnlPage.Visible = true;
            }
        }
        #endregion

        #region ˫����ɫ��Ϣ���������Ϸ��
        private void RoleInfoView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && RoleInfoView.DataSource != null)
            {
                using (DataTable dt = (DataTable)RoleInfoView.DataSource)
                {

                    txtId.Text = dt.Rows[e.RowIndex][2].ToString();
                    textBox1.Text = dt.Rows[e.RowIndex][3].ToString();
                    tabControl1.SelectedIndex = 1;
                    txtMoney.Focus();
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        #region �����Ϸ��backgoundworker��Ϣ���ͺͽ���
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("����ID���ର��");
                return;
            }

            try
            {
                int.Parse(txtMoney.Text);
            }
            catch
            {
                MessageBox.Show("�K�[������ �榡�����T");
                return;
            }

            if (txtTitle.Text == "")
            {
                MessageBox.Show("���D���ର��");
                return;
            }


            if (txtMessage.Text == "")
            {
                MessageBox.Show("�ذe��T���ର��");
                return;
            }

            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

            mContent[0].eName = CEnum.TagName.RayCity_CharacterID;
            mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[0].oContent = int.Parse(txtId.Text);

            mContent[1].eName = CEnum.TagName.RayCity_CharacterMoney;
            mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[1].oContent = int.Parse(txtMoney.Text);

            mContent[2].eName = CEnum.TagName.RayCity_ServerIP;
            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[2].oContent = Operation_RCode.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[3].eName = CEnum.TagName.UserByID;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

            mContent[4].eName = CEnum.TagName.RayCity_Title;
            mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[4].oContent = txtTitle.Text;

            mContent[5].eName = CEnum.TagName.RayCity_Message;
            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[5].oContent = txtMessage.Text;

            mContent[6].eName = CEnum.TagName.RayCity_NyUserID;
            mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[6].oContent = TxtAccount.Text;

            CEnum.Message_Body[,] result = Operation_RCode.GetResult(tmp_ClientEvent, CEnum.ServiceKey.RayCity_AddMoney, mContent);

            if (result[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(result[0, 0].oContent.ToString());
                return;
            }
            else if (result[0, 0].eName == CEnum.TagName.Status && result[0, 0].oContent.ToString() == "SUCCESS")
            {
                MessageBox.Show("�o�e���� ���\");
                txtMoney.Text = "";
                txtTitle.Text = "";
                txtMessage.Text = "";
            }
            else
            {
                MessageBox.Show("�o�e���� ����");
            }
        }
        #endregion

        #region �������������Ϸ�������
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMoney.Text = "";
            txtTitle.Text = "";
            txtMessage.Text = "";
        }
        #endregion

        #region ������ʷC_Event
        private void CmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_RCode.GetItemAddr(mServerInfo, CmbServer.Text));
        }
        #endregion

        #region ��ѯ��ɫ��Ϣ��ҳ
        private void CmbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bFirst)
            {
                this.CmbPage.Enabled = false;
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];
                CEnum.Message_Body[,] mResult = null;
                mContent[0].eName = CEnum.TagName.RayCity_NyUserID;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = TxtAccount.Text;

                mContent[1].eName = CEnum.TagName.RayCity_ServerIP;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = Operation_RCode.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[2].eName = CEnum.TagName.RayCity_NyNickName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = TxtNick.Text;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(this.CmbPage.Text.ToString());


                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_RCode.iPageSize;

                lock (typeof(C_Event.CSocketEvent))
                {
                    mResult = Operation_RCode.GetResult(tmp_ClientEvent, CEnum.ServiceKey.RayCity_BasicAccount_Query, mContent);
                }
                CmbPage.Enabled = true;

                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {
                    Operation_RCode.BuildDataTable(this.m_ClientEvent, mResult, RoleInfoView, out iPageCount);

                }
            }
        }
        #endregion

    }
}