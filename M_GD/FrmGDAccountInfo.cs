using System;
using System.Collections;
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
    [C_Global.CModuleAttribute("�û���Ϣ��ѯ", "FrmGDAccountInfo", "�û���Ϣ��ѯ", "GD Group")]
    public partial class FrmGDAccountInfo : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//��ҳҳ��

        int userID = 0;
        string serverIP = null;
        string userName = null;
        string userNick = null;
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 
        int selectItem = 0;

        string itemID = null;
        bool pageMixItems = false;
        bool pageUnits = false;
        bool pageCombat = false;
        bool pageOperator = false;
        bool pagePaint = false;
        bool pageSkill = false;
        bool pageSticker = false;
        bool pageUserUnitsDetail=false;
        bool pageStraightRanking = false;
        bool pageComplexRank = false;
        #endregion

        public FrmGDAccountInfo()
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
            FrmGDAccountInfo mModuleFrm = new FrmGDAccountInfo();
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

            //this.tpgMixTreeItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgMixTreeItems");
            //this.lblMixItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgUserUnits.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgUserUnits");
            //this.lblUnits.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgCombatItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCombatItems");
            //this.lblCombatItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgOperators.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgOperators");
            //this.lblOperators.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgSkillItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgSkillItems");
            //this.lblSkillItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgPaintItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgPaintItems");
            //this.lblPaintItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgStickItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgStickerItems");
            //this.lblStickerItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            tbcResult.Enabled = true;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDAccountInfo_Load(object sender, EventArgs e)
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
                userName = txtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();

                if (txtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
                {
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

        #region ��ѯ��ɫ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_Account_Query, (CEnum.Message_Body[])e.Argument);
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



        #region ������һ�����Ϣ���浱ǰ�к�
        private void GrdCharacter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//����ĳһ��
                {
                    selectChar = int.Parse(e.RowIndex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ˫����һ�����Ϣ�鿴���������Ϣ
        private void GrdCharacter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        tbcResult.SelectedTab = tpgMixTreeItems;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region �л�ѡ����в���
        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                    if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgMixTreeItems")))
                    {
                        MixTreeItems();//��ѯ�������
                    }
                    else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgUserUnits")))
                    {
                        UnitsItem();//��ѯ������Ϣ
                    }
                    else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgCombatItems")))
                    {
                        CombatItems();//��ѯս������
                    }
                    else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgOperators")))
                    {
                        OperaterInfo();//��ѯ������Ϣ
                    }
                    else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgPaintItems")))
                    {
                        PaintItems();//��ѯͿװ����
                    }
                    else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgSkillItems")))
                    {
                        SkillItems();//��ѯ���ܵ���
                    }
                    else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "UIC_UI_tpgStickerItems")))
                    {
                        StickerItems();//��ѯ��ǩ����
                    }
                
                    //else if (tbcResult.SelectedTab.Text.Equals("������ϸ��Ϣ"))
                    //{
                    //    UserUnitsDetail();//��ѯ������ϸ��Ϣ
                    
                    //}
                }
                    //else if(tbcResult.SelectedTab.Text.Equals("�ۺ�����"))
                    //{
                    //    ComplexRank();//��ѯ�ۺ�����
                    //}
                    //else if (tbcResult.SelectedTab.Text.Equals("��ʤ����"))
                    //{
                    //    StraightRanking();//��ѯ�ۺ�����
                    //}
                else
                {
                    GrdMixItems.DataSource = null;
                    pnlMixItems.Visible = false;

                    GrdUnits.DataSource = null;
                    pnlUnits.Visible = false;

                    GrdCombatItems.DataSource = null;
                    pnlCombatItems.Visible = false;

                    GrdOperators.DataSource = null;
                    pnlOperators.Visible = false;

                    GrdPaintItems.DataSource = null;
                    pnlPaintItems.Visible = false;

                    GrdSkillItems.DataSource = null;
                    pnlSkillItems.Visible = false;

                    GrdStickerItems.DataSource = null;
                    pnlStickerItems.Visible = false;

                    //GrdStraightRanking.DataSource = null;
                    //pnlStraightRanking.Visible = false;

                    //GrdComplexRank.DataSource = null;
                    //pnlComplexRank.Visible= false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion



        #region ��ѯ���������Ϣ
        private void MixTreeItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdMixItems.DataSource = null;

                this.pnlMixItems.Visible = false;
                this.cmbMixItems.Items.Clear();
                this.pageMixItems = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerMixTree.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerMixTree_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_MixTree_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerMixTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdMixItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlMixItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbMixItems.Items.Add(i + 1);
                }

                cmbMixItems.SelectedIndex = 0;
                pageMixItems = true;
                pnlMixItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ���������Ϣ
        private void cmbMixItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageMixItems)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdMixItems.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbMixItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReMixTree.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReMixTree_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_MixTree_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReMixTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdMixItems, out iPageCount);
        }
        #endregion



        #region ��ѯ������Ϣ
        private void UnitsItem()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdUnits.DataSource = null;

                this.pnlUnits.Visible = false;
                this.cmbUnits.Items.Clear();
                this.pageUnits = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerUnits.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerUnits_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerUnits_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUnits, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlUnits.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbUnits.Items.Add(i + 1);
                }

                cmbUnits.SelectedIndex = 0;
                pageUnits = true;
                pnlUnits.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ������Ϣ
        private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageUnits)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdUnits.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbUnits.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReUnits.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReUnits_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReUnits_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUnits, out iPageCount);
        }
        #endregion



        #region ��ѯս��������Ϣ
        private void CombatItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdCombatItems.DataSource = null;

                this.pnlCombatItems.Visible = false;
                this.cmbCombatItems.Items.Clear();
                this.pageCombat = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerCombatItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerCombatItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserCombatitems_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerCombatItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCombatItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlCombatItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbCombatItems.Items.Add(i + 1);
                }

                cmbCombatItems.SelectedIndex = 0;
                pageCombat = true;
                pnlCombatItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯս��������Ϣ
        private void cmbCombatItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageCombat)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdCombatItems.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbCombatItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReCombatItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReCombatItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserCombatitems_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReCombatItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCombatItems, out iPageCount);
        }
        #endregion



        #region ��ѯ������Ϣ
        private void OperaterInfo()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdOperators.DataSource = null;

                this.pnlOperators.Visible = false;
                this.cmbOperators.Items.Clear();
                this.pageOperator = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerOperators.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerOperators_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Operator_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerOperators_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdOperators, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlOperators.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbOperators.Items.Add(i + 1);
                }

                cmbOperators.SelectedIndex = 0;
                pageOperator = true;
                pnlOperators.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ������Ϣ
        private void cmbOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageOperator)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdOperators.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbOperators.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReOperators.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReOperators_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Operator_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReOperators_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdOperators, out iPageCount);
        }
        #endregion



        #region ��ѯͿװ����
        private void PaintItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdPaintItems.DataSource = null;

                this.pnlPaintItems.Visible = false;
                this.cmbPaintItems.Items.Clear();
                this.pagePaint = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerPaintItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerPaintItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Paint_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerPaintItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdPaintItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlPaintItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbPaintItems.Items.Add(i + 1);
                }

                cmbPaintItems.SelectedIndex = 0;
                pagePaint = true;
                pnlPaintItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯͿװ����
        private void cmbPaintItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pagePaint)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdPaintItems.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbPaintItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerRePaintItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerRePaintItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Paint_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerRePaintItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdPaintItems, out iPageCount);
        }
        #endregion



        #region ��ѯ���ܵ���
        private void SkillItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdSkillItems.DataSource = null;

                this.pnlSkillItems.Visible = false;
                this.cmbSkillItems.Items.Clear();
                this.pageSkill = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerSkillItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerSkillItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Skill_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerSkillItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdSkillItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlSkillItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbSkillItems.Items.Add(i + 1);
                }

                cmbSkillItems.SelectedIndex = 0;
                pageSkill = true;
                pnlSkillItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ���ܵ���
        private void cmbSkillItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageSkill)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdSkillItems.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbSkillItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReSkillItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerReSkillItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Skill_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerReSkillItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdSkillItems, out iPageCount);
        }
        #endregion



        #region ��ѯ��ǩ����
        private void StickerItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdStickerItems.DataSource = null;

                this.pnlStickerItems.Visible = false;
                this.cmbStickerItems.Items.Clear();
                this.pageSticker = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerStickerItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerStickerItems_DoWork(object sender, DoWorkEventArgs e)        
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerStickerItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdStickerItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlStickerItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbStickerItems.Items.Add(i + 1);
                }

                cmbStickerItems.SelectedIndex = 0;
                pageSticker = true;
                pnlStickerItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ��ǩ����
        private void cmbStickerItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageSticker)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdStickerItems.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbStickerItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReStickerItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerReStickerItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerReStickerItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdStickerItems, out iPageCount);
        }
        #endregion

       




        #region ����ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ��ѯ�ۺ�����backgroundWorker��Ϣ����
        private void backgroundWorkerComplexRank_DoWork(object sender, DoWorkEventArgs e)
        {
            //lock (typeof(C_Event.CSocketEvent))
            //{
            //    e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserRank_query, (CEnum.Message_Body[])e.Argument);
            //}
        }
        #endregion

        #region ��ѯ�ۺ�����backgroundWorker��Ϣ����
        private void backgroundWorkerComplexRank_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.tbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdComplexRank, out iPageCount);

            //if (iPageCount <= 1)
            //{
            //    this.pnlComplexRank.Visible = false;
            //}
            //else
            //{
            //    for (int i = 0; i < iPageCount; i++)
            //    {
            //        this.cmbComplexRank.Items.Add(i + 1);
            //    }

            //    this.cmbComplexRank.SelectedIndex = 0;
            //    pageComplexRank = true;
            //    this.pnlComplexRank.Visible = true;
            //}
        }
        #endregion

        #region
        private void cmbComplexRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (pageComplexRank)
            //    {
            //        this.GrpSearch.Enabled = false;
            //        this.tbcResult.Enabled = false;
            //        this.Cursor = Cursors.WaitCursor;//�ı����״̬
            //        this.GrdComplexRank.DataSource = null;

            //        CEnum.Message_Body[] mContent = new CEnum.Message_Body[4];

            //        mContent[0].eName = CEnum.TagName.SD_ServerIP;
            //        mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            //        mContent[0].oContent = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text);

            //        //mContent[1].eName = CEnum.TagName.f_idx;
            //        //mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        //mContent[1].oContent = userID;

            //        //mContent[2].eName = CEnum.TagName.SD_UserName;
            //        //mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            //        //mContent[2].oContent = userName;

            //        mContent[1].eName = CEnum.TagName.Index;
            //        mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        mContent[1].oContent = int.Parse(this.cmbComplexRank.Text.Trim());

            //        mContent[2].eName = CEnum.TagName.PageSize;
            //        mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        mContent[2].oContent = Operation_GD.iPageSize;

            //        mContent[3].eName = CEnum.TagName.SD_Type;
            //        mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        mContent[3].oContent =1;
            //        this.backgroundWorkerReComplexRank.RunWorkerAsync(mContent);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        #endregion

        #region
        private void backgroundWorkerReComplexRank_DoWork(object sender, DoWorkEventArgs e)
        {
            //lock (typeof(C_Event.CSocketEvent))
            //{
            //    e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserRank_query, (CEnum.Message_Body[])e.Argument);
            //}
        }
        #endregion

        #region
        private void backgroundWorkerReComplexRank_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.tbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdComplexRank, out iPageCount);
        }
        #endregion

        #region
        private void UserUnitsDetail()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                //this.GrdUserUnitsDetail.DataSource = null;

                //this.pnlUserUnitsDetail.Visible = false;
                //this.cmbUserUnitsDetail.Items.Clear();
                this.pageUserUnitsDetail = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;

                backgroundWorkerMixTree.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        #endregion

        private void backgroundWorkerUserUnitsDetail_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerUserUnitsDetail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.tbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUserUnitsDetail, out iPageCount);

            //if (iPageCount <= 1)
            //{
            //    this.pnlUserUnitsDetail.Visible = false;
            //}
            //else
            //{
            //    for (int i = 0; i < iPageCount; i++)
            //    {
            //        this.cmbUserUnitsDetail.Items.Add(i + 1);
            //    }

            //    cmbUserUnitsDetail.SelectedIndex = 0;
            //    pageUserUnitsDetail = true;
            //    this.pnlUserUnitsDetail.Visible = true;
            //}
        }

        private void cmbUserUnitsDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (pageUserUnitsDetail)
            //    {
            //        this.GrpSearch.Enabled = false;
            //        this.tbcResult.Enabled = false;
            //        this.Cursor = Cursors.WaitCursor;//�ı����״̬
            //        this.GrdUserUnitsDetail.DataSource = null;

            //        CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            //        mContent[0].eName = CEnum.TagName.SD_ServerIP;
            //        mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            //        mContent[0].oContent = serverIP;

            //        mContent[1].eName = CEnum.TagName.f_idx;
            //        mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        mContent[1].oContent = userID;

            //        mContent[2].eName = CEnum.TagName.SD_UserName;
            //        mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            //        mContent[2].oContent = userName;

            //        mContent[3].eName = CEnum.TagName.Index;
            //        mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        mContent[3].oContent = int.Parse(cmbUnits.Text.Trim());

            //        mContent[4].eName = CEnum.TagName.PageSize;
            //        mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            //        mContent[4].oContent = Operation_GD.iPageSize;

            //        this.backgroundWorkerReUserUnitsDetail.RunWorkerAsync(mContent);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void backgroundWorkerReUserUnitsDetail_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReUserUnitsDetail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.tbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUserUnitsDetail, out iPageCount);

           
        }

    
        private void backgroundWorkerStraightRanking_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserRank_query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerStraightRanking_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.tbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdStraightRanking, out iPageCount);

            //if (iPageCount <= 1)
            //{
            //    this.pnlStraightRanking.Visible = false;
            //}
            //else
            //{
            //    for (int i = 0; i < iPageCount; i++)
            //    {
            //        this.cmbStraightRanking.Items.Add(i + 1);
            //    }

            //    this.cmbStraightRanking.SelectedIndex = 0;
            //    pageStraightRanking = true;
            //    this.pnlStraightRanking.Visible = true;
            //}
        }

        private void backgroundWorkerReStraightRanking_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserRank_query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReStraightRanking_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.tbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdStraightRanking, out iPageCount);

           
        }

        private void cmbStraightRanking_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void GrdUnits_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdUnits.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdUnits.DataSource;
                        itemID =sTable.Rows[selectItem][4].ToString();//��������ʺ�ID
                        //itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        //if (itemName == "�޵���")
                        //{
                        //    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                        //    return;
                        //}
                        if (MessageBox.Show("�Ƿ���ʾ��һ���װ����Ϣ��", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[4];

                            mContent[0].eName = CEnum.TagName.SD_ServerIP;
                            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[0].oContent = serverIP;

                            //mContent[1].eName = CEnum.TagName.UserByID;
                            //mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                            //mContent[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                            mContent[1].eName = CEnum.TagName.f_idx;
                            mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[1].oContent = userID;

                            mContent[2].eName = CEnum.TagName.SD_UserName;
                            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[2].oContent = userName;

                            //mContent[4].eName = CEnum.TagName.SD_ID;
                            //mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            //mContent[4].oContent = itemID;

                            mContent[3].eName = CEnum.TagName.SD_ItemID;
                            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[3].oContent =int.Parse(itemID);

                            //mContent[6].eName = CEnum.TagName.SD_Type;
                            //mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            //mContent[6].oContent = 1;

                            backgroundWorkerUnitsEquipment.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UnitsItem_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            //this.GrdUnitEquipment.DataSource = null;
            //this.tbcResult.SelectedTab = this.tpgUserUnitsEquipment;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUnitEquipment, out iPageCount);
        }

    }
}