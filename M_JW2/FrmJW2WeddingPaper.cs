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


namespace M_JW2
{
    [C_Global.CModuleAttribute("情侣派对卡", "FrmJW2WeddingPaper", "情侣派对卡", "JW2 Group")]
    public partial class FrmJW2WeddingPaper : Form
    {
        #region 变量

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//翻页页数

        int userID = 0;
        string serverIP = null;
        string userName = null;
        string userNick = null;
        int selectChar = 0;   //GrdCharacter中当前选中的行 
        int selectItem = 0;

        string itemID = null;
        bool pageRoleView = false;
        bool pageWeddingPaper = false;
      

        #endregion

        public FrmJW2WeddingPaper()
        {
            InitializeComponent();
        }

        #region 创建类库中的窗体
        /// <summary>
        /// 创建类库中的窗体
        /// </summary>
        /// <param name="oParent">MDI 程序的父窗体</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>类库中的窗体</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //创建登录窗体
            FrmJW2WeddingPaper mModuleFrm = new FrmJW2WeddingPaper();
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

        #region 初始化语言库
        /// <summary>
        ///　文字库
        /// </summary>
        ConfigValue config = null;

        /// <summary>
        /// 初始化华文字语言库
        /// </summary>
        private void IntiFontLib()
        {
            config = (ConfigValue)m_ClientEvent.GetInfo("INI");
            IntiUI();
        }

        private void IntiUI()
        {
            this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            this.lblAccount.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            this.lblNick.Text = config.ReadConfigValue("MSD", "UIC_UI_lblNick");
            this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");
            //this.tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCharacter");
            //this.lblStoryState.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2WeddingPaper");

            this.tpgCharacter.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmBugleSendLogtpgCharacter");
            this.tpgWeddingPaper.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgWeddingPaper");

            lblRoleView.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");
            label1.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");



            tbcResult.Enabled = true;
        }
        #endregion



        #region 登陆窗体加载(得到游戏服务器列表)
        private void FrmJW2WeddingPaper_Load(object sender, EventArgs e)
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
                mContent[1].oContent = m_ClientEvent.GetInfo("GameID_JW2");

                this.backgroundWorkerFormLoad.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 加载游戏服务器列表backgroundWorker消息发送
        private void backgroundWorkerFormLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_JW2.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region 加载游戏服务器列表backgroundWorker消息接收
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbServer = Operation_JW2.BuildCombox(mServerInfo, cmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region 切换不同的游戏服务器
        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion

        #region
        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdRoleView.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdRoleView.DataSource;
                    serverIP = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "JW2_UserSN")].ToString());//保存玩家帐号ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "JW2_UserID")].ToString();
                    if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2WeddingPaper")))
                    {
                        WeddingPaperInfo();//查询情侣信息
                    }

                }
                else
                {


                    GrdRoleView.DataSource = null;
                    GrdRoleView.Visible = false;




                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region
        private void GrdRoleView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.tbcResult.SelectedIndex = 1;
        }
        #endregion

        #region
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_ACCOUNT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//改变鼠标状态
                this.tbcResult.SelectedIndex = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdRoleView, out iPageCount);

                if (iPageCount <= 1)
                {
                    this.pnlRoleView.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbRoleView.Items.Add(i + 1);
                    }

                    this.cmbRoleView.SelectedIndex = 0;
                    this.pageRoleView = true;
                    this.pnlRoleView.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerWeddingPaper_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_Wedding_Paper, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerWeddingPaper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//改变鼠标状态
                this.tbcResult.SelectedIndex = 1;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdWeddingPaper, out iPageCount);

                if (iPageCount <= 1)
                {
                    this.pnlWeddingPaper.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbWeddingPaper.Items.Add(i + 1);
                    }

                    this.cmbWeddingPaper.SelectedIndex = 0;
                    this.pageWeddingPaper = true;
                    this.pnlWeddingPaper.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_ACCOUNT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//改变鼠标状态
                this.tbcResult.SelectedIndex = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdWeddingPaper, out iPageCount);

              
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerReWeddingPaper_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_Wedding_Paper, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReWeddingPaper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.btnSearch.Enabled = true;
                Cursor = Cursors.Default;
                this.tbcResult.SelectedIndex = 1;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdWeddingPaper, out iPageCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region
        private void WeddingPaperInfo()
        {
            try
            {

                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//改变鼠标状态
              
                this.pageWeddingPaper = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.JW2_UserSN;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.JW2_UserID;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerWeddingPaper.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        #endregion

        #region
        private void cmbWeddingPaper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageWeddingPaper)
            {
                
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.JW2_UserSN;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.JW2_UserID;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(this.cmbWeddingPaper.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReWeddingPaper.RunWorkerAsync(mContent);
            }
        }
        #endregion

        #region
        private void cmbRoleView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageRoleView)
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//改变鼠标状态
              
                this.pageRoleView = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = cmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_ACCOUNT;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.txtAccount.Text.ToString();


                mContent[3].eName = CEnum.TagName.JW2_UserNick;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = txtNick.Text.ToString();

                mContent[4].eName = CEnum.TagName.Index;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = int.Parse(this.cmbRoleView.Text.ToString());

                mContent[5].eName = CEnum.TagName.PageSize;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = Operation_JW2.iPageSize;

                backgroundWorkerReSearch.RunWorkerAsync(mContent);
            }
        }
        #endregion

        #region
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//改变鼠标状态
             
                this.pageRoleView = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = cmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_ACCOUNT;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.txtAccount.Text.ToString();


                mContent[3].eName = CEnum.TagName.JW2_UserNick;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = txtNick.Text.ToString();

                mContent[4].eName = CEnum.TagName.Index;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = 1;

                mContent[5].eName = CEnum.TagName.PageSize;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = Operation_JW2.iPageSize;

                backgroundWorkerSearch.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}