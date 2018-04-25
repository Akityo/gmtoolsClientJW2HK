using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using C_Global;
using C_Event;
using Language;
using System.IO;
using System.Collections;

namespace M_RC
{
   /// <summary>
    /// 
    /// </summary>
    [C_Global.CModuleAttribute("�ϳɵ���??", "FrmBingoCard", " �ϳɵ���??", "FJ_Group")]
    public partial class FrmBingoCard : Form
    {
        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;
        private int iPageCount = 2;
        private bool bFirst = false;
        private string _ServerIP;
        string userAccount = null; //��ҽ�ɫId
        string userAccount2 = null;//��ҽ�ɫ����

        int currDgSelectRow = 0;    //�����Ϣdatagrid �е�ǰѡ�е���
        private CEnum.Message_Body[,] mType = null;


        struct itemEx
        {
            public string Tag;
            public string Text;
            public itemEx(string tag, string text)
            {
                this.Tag = tag;
                this.Text = text;
            }
            public override string ToString()
            {
                return this.Text;
            }
        }

        public FrmBingoCard()
        {
            InitializeComponent();
            FrmBingoCard.CheckForIllegalCrossThreadCalls = false;
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
            FrmBingoCard mModuleFrm = new FrmBingoCard();
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

        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmPlayerTradeLog_Load(object sender, EventArgs e)
        {
            IntiFontLib();

            CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];
            mContent[0].eName = CEnum.TagName.ServerInfo_GameDBID;
            mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[0].oContent = 1;

            mContent[1].eName = CEnum.TagName.ServerInfo_GameID;
            mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[1].oContent = m_ClientEvent.GetInfo("GameID_RC");

            this.backgroundWorkerServerLoad.RunWorkerAsync(mContent);

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
            //this.Text = config.ReadConfigValue("MRC", "FRC_UI_FrmBingoCard");
            //this.LblServer.Text = config.ReadConfigValue("MRC", "FRC_UI_LblServer");
            //this.LblAccount.Text = config.ReadConfigValue("MRC", "FRC_UI_LblPlayAccount");
            //this.BtnSearch.Text = config.ReadConfigValue("MRC", "FRC_UI_BtnSearch");
            //this.BtnClose.Text = config.ReadConfigValue("MRC", "FRC_UI_button1");
            //this.TpgCharacter.Text = config.ReadConfigValue("MRC", "FRC_UI_TpgCharacter");
            //this.TpgItemList.Text = config.ReadConfigValue("MRC", "FRC_UI_TpgItemList");
            //this.LblPlayId.Text = config.ReadConfigValue("MRC", "FRC_UI_LblPlayId");
            //this.LblState.Text = config.ReadConfigValue("MRC", "FRC_UI_LblState");
            //this.LblRoleName.Text = config.ReadConfigValue("MRC", "FRC_UI_LblRoleName");
            //this.LblStartTime.Text = config.ReadConfigValue("MRC", "FRC_UI_LblStartTime");
            //this.LblEndTime.Text = config.ReadConfigValue("MRC", "FRC_UI_LblEndTime");
            //this.BtnSerch.Text = config.ReadConfigValue("MRC", "FRC_UI_BtnSerch");

        }


        #endregion

        #region ����������б�backgroundWorker��Ϣ����
        private void backgroundWorkerServerLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_RC.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ����������б�backgroundWorker��Ϣ����
        private void backgroundWorkerServerLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CmbServer.Items.Clear();
            for (int i = 0; i < mServerInfo.GetLength(0); i++)
            {
                CmbServer.Items.Add(mServerInfo[i, 1].oContent);
            }

            CmbServer.SelectedIndex = 0;
            bFirst = true;

            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_RCode.GetItemAddr(mServerInfo, CmbServer.Text));
        }
        #endregion

        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmBingoCard_Load(object sender, EventArgs e)
        {
            try
            {
                IntiFontLib();
                label8.Visible = false;
                comboBox1.Visible = false;
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];
                mContent[0].eName = CEnum.TagName.ServerInfo_GameDBID;
                mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[0].oContent = 1;

                mContent[1].eName = CEnum.TagName.ServerInfo_GameID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = m_ClientEvent.GetInfo("GameID_RC");

                this.backgroundWorkerServerLoad.RunWorkerAsync(mContent);
            }
            catch
            { }

        }
        #endregion

        #region ������ɫ��Ϣ�б���Ϣ���ͺͽ���
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (CmbServer.Text == "")
            {
                return;
            }
            //����ؼ�
            comboBox2.Items.Clear();
            tabControl1.SelectedTab = TpgCharacter;

            this.RoleInfoView.DataSource = null;

            if (TxtAccount.Text.Trim().Length > 0 || TxtNick.Text.Trim().Length > 0)
            {
                this.BtnSearch.Enabled = false;
                PartInfo();
            }
            else
            {
                MessageBox.Show(config.ReadConfigValue("MRC", "FQP_Code_inputid"));
                return;
            }
        }
        #endregion

        #region ������ɫ��Ϣ�б�backgroundworker��Ϣ���ͺͽ���
        private void PartInfo()
        {
            this.RoleInfoView.DataSource = null;
            CEnum.Message_Body[,] mResult = null;
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            mContent[0].eName = CEnum.TagName.RayCity_NyUserID;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = TxtAccount.Text;

            mContent[1].eName = CEnum.TagName.RayCity_ServerIP;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = Operation_RC.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[2].eName = CEnum.TagName.RayCity_NyNickName;
            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[2].oContent = TxtNick.Text;

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = 1;

            mContent[4].eName = CEnum.TagName.PageSize;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = Operation_RCode.iPageSize;

            this.backgroundWorkerSerch.RunWorkerAsync(mContent);

        }
        #endregion

        #region ������ɫ��Ϣbackgroundworker��Ϣ����
        private void backgroundWorkerSerch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_RCode.GetResult(tmp_ClientEvent, CEnum.ServiceKey.RayCity_BasicAccount_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ������ɫ��Ϣbackgroundworker��Ϣ����
        private void backgroundWorkerSerch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
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
                        comboBox2.Items.Add(i + 1);
                    }

                    comboBox2.SelectedIndex = 0;
                    bFirst = true;
                    PnlPage.Visible = true;
                }
            }
            catch
            {}
        }
        #endregion

        #region ˫����ɫ��Ϣ���кϳɵ���ѡ�
        private void RoleInfoView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.CmbState.Items.Clear();
                this.comboBox1.Items.Clear();
                if (RoleInfoView.DataSource != null)
                {
                    DataTable mTable = (DataTable)RoleInfoView.DataSource;
                    userAccount = mTable.Rows[e.RowIndex]["�b��ID"].ToString();
                    userAccount2 = mTable.Rows[e.RowIndex]["�b��"].ToString();
                    this.TxtCharinfo.Text = userAccount;
                    this.txtName.Text = userAccount2;
                }
                if (e.RowIndex >= 0 && RoleInfoView.DataSource != null)
                {
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region �ϳɵ���ѡ���ѡ���ʼ��
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (RoleInfoView.DataSource != null)
                {
                    DataTable mTable = (DataTable)RoleInfoView.DataSource;
                    userAccount = mTable.Rows[currDgSelectRow]["�b��ID"].ToString();
                    userAccount2 = mTable.Rows[currDgSelectRow]["�b��"].ToString();
                    this.textBox2.Text = userAccount;
                    this.textBox1.Text = userAccount2;
                }


                if (tabControl1.SelectedIndex == 1)
                {
                    //InitComboboxInfo();
                    this.CmbState.Items.Clear();
                    itemEx item1 = new itemEx("2", "�w�g���");
                    this.CmbState.Items.Add(item1);
                    itemEx item2 = new itemEx("0", "�X������");
                    this.CmbState.Items.Add(item2);
                    itemEx item3 = new itemEx("1", "�X�����\,���O�����");
                    this.CmbState.Items.Add(item3);
                    this.CmbState.Text = this.CmbState.Items[0].ToString();
                }
            }
            catch
            { }

        }
        #endregion

        #region �ϳɵ��߲�ѯ
        private void BtnSerch_Click(object sender, EventArgs e)
        {
            InitComboboxInfo();
        }
        #endregion

        #region ��ʼ��״̬�ͺϳɵ��߲�ѯbackgroundworker��Ϣ���ͺͽ���
        private void InitComboboxInfo()
        {
            try
            {

                comboBox1.Items.Clear();
                CEnum.Message_Body[,] mResult = null;
                CEnum.Message_Body[] mContent2 = new CEnum.Message_Body[7];


                mContent2[0].eName = CEnum.TagName.RayCity_ServerIP;
                mContent2[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent2[0].oContent = Operation_RC.GetItemAddr(mServerInfo, CmbServer.Text);


                mContent2[1].eName = CEnum.TagName.RayCity_CharacterID;
                mContent2[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[1].oContent = int.Parse(this.userAccount.ToString());

                mContent2[2].eName = CEnum.TagName.RayCity_BeginDate;
                mContent2[2].eTag = CEnum.TagFormat.TLV_DATE;
                mContent2[2].oContent = DtpBegin.Value;

                mContent2[3].eName = CEnum.TagName.RayCity_EndDate;
                mContent2[3].eTag = CEnum.TagFormat.TLV_DATE;
                mContent2[3].oContent = DtpEnd.Value;

                mContent2[4].eName = CEnum.TagName.Index;
                mContent2[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[4].oContent = 1;

                mContent2[5].eName = CEnum.TagName.PageSize;
                mContent2[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[5].oContent = Operation_RCode.iPageSize;

                itemEx item = (itemEx)this.CmbState.SelectedItem;

                mContent2[6].eName = CEnum.TagName.RayCity_BingoCardState;
                mContent2[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[6].oContent = int.Parse(item.Tag);

                this.backgroundWorkerInitCombobox.RunWorkerAsync(mContent2);


            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region ����ر�
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region �ϳɵ��߲�ѯ��ҳ
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bFirst)
            {
                CEnum.Message_Body[] mContent2 = new CEnum.Message_Body[7];

                mContent2[0].eName = CEnum.TagName.RayCity_ServerIP;
                mContent2[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent2[0].oContent = Operation_RC.GetItemAddr(mServerInfo, CmbServer.Text);


                mContent2[1].eName = CEnum.TagName.RayCity_CharacterID;
                mContent2[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[1].oContent = int.Parse(this.userAccount.ToString());

                mContent2[2].eName = CEnum.TagName.RayCity_BeginDate;
                mContent2[2].eTag = CEnum.TagFormat.TLV_DATE;
                mContent2[2].oContent = DtpBegin.Value;

                mContent2[3].eName = CEnum.TagName.RayCity_EndDate;
                mContent2[3].eTag = CEnum.TagFormat.TLV_DATE;
                mContent2[3].oContent = DtpEnd.Value;

                mContent2[4].eName = CEnum.TagName.Index;
                mContent2[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[4].oContent = int.Parse(this.comboBox1.Text.Trim());

                mContent2[5].eName = CEnum.TagName.PageSize;
                mContent2[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[5].oContent = Operation_RCode.iPageSize;

                itemEx item = (itemEx)this.CmbState.SelectedItem;

                mContent2[6].eName = CEnum.TagName.RayCity_BingoCardState;
                mContent2[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent2[6].oContent = int.Parse(item.Tag);

                this.backgroundWorkerPage.RunWorkerAsync(mContent2);

           }
       }
        #endregion

        #region ��ʼ���ϳɵ���backgroundworker��Ϣ����
        private void backgroundWorkerInitCombobox_DoWork(object sender, DoWorkEventArgs e)
         {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_RCode.GetResult(tmp_ClientEvent, CEnum.ServiceKey.RayCity_BingoCard_Query, (CEnum.Message_Body[])e.Argument);
            }
         }
        #endregion

         #region ��ʼ���ϳɵ���backgoundworker��Ϣ����
         private void backgroundWorkerInitCombobox_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
         {
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show("�X���˳ƵL�O��");
                return;
            }

            Operation_RCode.BuildDataTable(this.m_ClientEvent, mResult, TradeInfoView, out iPageCount);

            if (iPageCount <= 1)
            {
                label8.Visible = false;
                comboBox1.Visible = false;

            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    comboBox1.Items.Add(i + 1);
                }

                comboBox1.SelectedIndex = 0;
                bFirst = true;
                label8.Visible = true;
                comboBox1.Visible = true;
            }
        }
        #endregion

        #region 
        private void backgroundWorkerPage_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_RCode.GetResult(tmp_ClientEvent, CEnum.ServiceKey.RayCity_BingoCard_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerPage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }
            else
            {
                Operation_RCode.BuildDataTable(this.m_ClientEvent, mResult, TradeInfoView, out iPageCount);

            }
        }
        #endregion

        #region
        private void CmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_RCode.GetItemAddr(mServerInfo, CmbServer.Text));
        }
        #endregion

        #region
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
                    mContent[3].oContent = int.Parse(this.comboBox2.Text.ToString());


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
            catch
            {}

        }
        #endregion

    }
       


}