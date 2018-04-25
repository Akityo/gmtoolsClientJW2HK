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
    [C_Global.CModuleAttribute("�����־��Ϣ", "FrmJW2PlayerLog", "�����־��Ϣ", "JW2 Group")]
    public partial class FrmJW2PlayerLog : Form
    {

        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CEnum.Message_Body[,] mResult = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int selectChar;
        private int currDgSelectRow;
        private string userId = null;
        private int iPageCount = 0;//��ҳҳ��
        private bool bFirst;
        private string userAccount;
        private int selectAttri = 0;//��ѯ���Ե���


        private string FromUserID = null;
        private string ToUserID = null;

        private string FromUserName = null;
        private string ToUserName = null;

        private string FromPetID = null;
        private string ToPetID = null;

        private string FromPetName = null;
        private string ToPetName = null;

        private string begintime = null;
        private string logtime = null;

        private string bigType = null;
        private string smallType = null;
        #endregion

        public FrmJW2PlayerLog()
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
            FrmJW2PlayerLog mModuleFrm = new FrmJW2PlayerLog();
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
            this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            this.lblAccount.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            this.lblNick.Text = config.ReadConfigValue("MSD", "UIC_UI_lblNick");
            this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");

            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2PlayerLog");

            this.TpgCharacter.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmBugleSendLogtpgCharacter");
            this.tabPage1.Text = config.ReadConfigValue("MJW2", "NEW_UI_LogQuery");
            label1.Text = config.ReadConfigValue("MJW2", "NEW_UI_BigClass");
            label2.Text = config.ReadConfigValue("MJW2", "NEW_UI_SmallClass");
            LblDate.Text = config.ReadConfigValue("MJW2", "GI_UI_lblStartTime");
            LblLink.Text = config.ReadConfigValue("MJW2", "GI_UI_lblEndTime");

            LblPage.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");

            button1.Text = config.ReadConfigValue("MJW2", "NEW_UI_QueryLog");
           
            //this.chbSelect.Visible = false;
            //this.chbSelect.Checked = false;
        }


        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmJW2PlayerLog_Load(object sender, EventArgs e)
        {
            try
            {
                IntiFontLib();
                //TbcResult.Enabled = false;

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

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_JW2.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbServer = Operation_JW2.BuildCombox(mServerInfo, cmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region ��������ť��ѯ��ҵĻ�����Ϣ
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MMagic", "UIA_Hint_selectServer"));
                    this.txtAccount.Text = "";
                    this.txtNick.Text = "";
                    this.dataGridView1.DataSource = null;
                    return;
                }
                //����ؼ�
                this.TbcResult.SelectedTab = TpgCharacter;//ѡ���ɫ��Ϣѡ�
                this.dataGridView1.DataSource = null;
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

        #region ��ѯ���������Ϣ
        private void PartInfo()
        {
            this.btnSearch.Enabled = false;//����������ť
            //this.TbcResult.Enabled = false;
            this.Cursor = Cursors.WaitCursor;//�ı����״̬
            this.dataGridView1.DataSource = null;

            CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];
            //��ѯ��ҽ�ɫ��Ϣ
            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

            mContent[1].eName = CEnum.TagName.JW2_ACCOUNT;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = this.txtAccount.Text.ToString();

            mContent[2].eName = CEnum.TagName.JW2_UserNick;
            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[2].oContent = this.txtNick.Text.ToString();

            backgroundWorkerSearch.RunWorkerAsync(mContent);

        }
        #endregion

        #region ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.MAGIC_Account_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.btnSearch.Enabled = true;//��ѯ��ťʹ��
                this.TbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, this.dataGridView1, out iPageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion



        #region ������һ�����Ϣ���浱ǰ�к�
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currDgSelectRow = e.RowIndex;
        }
        #endregion

        #region ˫����һ�����Ϣ��ѯ��־��Ϣ
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
           
            currDgSelectRow = e.RowIndex;
            this.TbcResult.SelectedIndex = 1;
            this.dataGridView2.DataSource = null;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        }
        #endregion

        private void TbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                cmbBClass.Items.Clear();
                this.CmbPage.Visible = false;
                this.LblPage.Visible = false;
                if (this.TbcResult.SelectedIndex == 1)
                {
                    //cmbBClass.Items.Add("�����ʽ��־");
                    cmbBClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_PicUploadPass"));
                    //maple add
                    cmbBClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_GardenLogQuery"));
                
                    cmbBClass.SelectedIndex = 0;
                    if (this.dataGridView1.DataSource != null)
                    {
                        DataTable mTable = (DataTable)dataGridView1.DataSource;
                        userAccount = mTable.Rows[currDgSelectRow][1].ToString();//��������ʺ�id
                        userId = mTable.Rows[currDgSelectRow][0].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmbBClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (this.cmbBClass.Text.ToString() == "�����ʽ��־")
                //{
                //    this.cmbSClass.Items.Clear();
                //    this.cmbSClass.Items.Add("����Ԥ��");
                //    this.cmbSClass.Items.Add("�������");
                //}

                if (this.cmbBClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_PicUploadPass"))
                {
                    this.cmbSClass.Items.Clear();
                    this.cmbSClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_PassUser"));
                }
                else if (this.cmbBClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_GardenLogQuery"))
                {
                    this.cmbSClass.Items.Clear();
                    this.cmbSClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_Boutiqueitemstobuy"));
                    this.cmbSClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_Fieldpurchase"));
                    this.cmbSClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_Fertilizerpurchase"));
                    this.cmbSClass.Items.Add(config.ReadConfigValue("MJW2", "NEW_UI_Seedpurchased"));
                }

                this.cmbSClass.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void backgroundWorkerPlayLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(this.m_ClientEvent, CEnum.ServiceKey.JW2_PicCard_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerPlayLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
           
            this.button1.Enabled = true;//����������ť
            this.TbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }
            else
            {
                this.dataGridView2.DataSource = null;
                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, dataGridView2, out iPageCount);

                if (iPageCount <= 1)
                {
                    CmbPage.Visible = false;
                    LblPage.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        CmbPage.Items.Add(i + 1);
                    }

                    CmbPage.SelectedIndex = 0;
                    bFirst = true;
                    CmbPage.Visible = true;
                    LblPage.Visible = true;
                }
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        }

        private void backgroundWorkerRePlayLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(this.m_ClientEvent, CEnum.ServiceKey.JW2_PicCard_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerRePlayLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.button1.Enabled = true;//����������ť
                this.TbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {
                    this.dataGridView2.DataSource = null;
                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, dataGridView2, out iPageCount);

                 
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.LblPage.Visible = false;
                this.CmbPage.Visible = false;
                this.CmbPage.Items.Clear();
                bFirst = false;
                if (this.cmbBClass.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MJW2", "NEW_UI_PleaseSelectPlayerEmailBigClass"));
                    return;
                }
                if (this.cmbSClass.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MJW2", "NEW_UI_PleaseSelectPlayerEmailSmallClass"));
                    return;
                }

                this.button1.Enabled = false;//����������ť

                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.dataGridView2.DataSource = null;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];
                //��ѯ��ҽ�ɫ��Ϣ
                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

                mContent[1].eName = CEnum.TagName.BeginTime;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent =DtpBegin.Text.ToString();

                mContent[2].eName = CEnum.TagName.EndTime;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = DtpEnd.Text.ToString();

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.JW2_UserSN;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = int.Parse(userId);

                mContent[5].eName = CEnum.TagName.PageSize;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = Operation_JW2.iPageSize;

                if (this.cmbBClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_PicUploadPass"))
                {
                    mContent[6].eName = CEnum.TagName.Magic_category;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = 1;
                }
                else if (this.cmbBClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_GardenLogQuery"))
                {
                    mContent[6].eName = CEnum.TagName.Magic_category;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = 2;
                }

                if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_Boutiqueitemstobuy"))
                {
                    mContent[7].eName = CEnum.TagName.Magic_action;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 1;
                }
                else if (this.cmbSClass.Text.ToString() ==config.ReadConfigValue("MJW2", "NEW_UI_Fieldpurchase"))
                {
                    mContent[7].eName = CEnum.TagName.Magic_action;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 2;
                }
                else if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_Fertilizerpurchase"))
                {
                    mContent[7].eName = CEnum.TagName.Magic_action;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 3;
                }
                else if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_Seedpurchased"))
                {
                    mContent[7].eName = CEnum.TagName.Magic_action;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 4;
                }
                else if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_PassUser"))
                {
                    mContent[7].eName = CEnum.TagName.Magic_action;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 5;
                }


                this.backgroundWorkerPlayLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
             if(bFirst)
             {

            
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];
            //��ѯ��ҽ�ɫ��Ϣ
            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

            mContent[1].eName = CEnum.TagName.BeginTime;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = DtpBegin.Text.ToString();

            mContent[2].eName = CEnum.TagName.EndTime;
            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[2].oContent = DtpEnd.Text.ToString();

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = int.Parse(this.CmbPage.Text.ToString());

            mContent[4].eName = CEnum.TagName.JW2_UserSN;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = int.Parse(userId);

            mContent[5].eName = CEnum.TagName.PageSize;
            mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[5].oContent =Operation_JW2.iPageSize;

            if (this.cmbBClass.Text.ToString() ==config.ReadConfigValue("MJW2", "NEW_UI_PicUploadPass"))
            {
                mContent[6].eName = CEnum.TagName.Magic_category;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = 1;
            }
            else if (this.cmbBClass.Text.ToString() ==config.ReadConfigValue("MJW2", "NEW_UI_GardenLogQuery"))
            {
                mContent[6].eName = CEnum.TagName.Magic_category;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = 2;
            }

            if (this.cmbSClass.Text.ToString() ==config.ReadConfigValue("MJW2", "NEW_UI_Boutiqueitemstobuy"))
            {
                mContent[7].eName = CEnum.TagName.Magic_action;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 1;
            }
            else if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_Fieldpurchase"))
            {
                mContent[7].eName = CEnum.TagName.Magic_action;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 2;
            }
            else if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_Fertilizerpurchase"))
            {
                mContent[7].eName = CEnum.TagName.Magic_action;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 3;
            }
            else if (this.cmbSClass.Text.ToString() == config.ReadConfigValue("MJW2", "NEW_UI_Seedpurchased"))
            {
                mContent[7].eName = CEnum.TagName.Magic_action;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 4;
            }


            this.backgroundWorkerRePlayLog.RunWorkerAsync(mContent);
        }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
                
        }

      
    }
}