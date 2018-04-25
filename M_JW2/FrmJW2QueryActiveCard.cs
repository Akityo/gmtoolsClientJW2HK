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
    [C_Global.CModuleAttribute("���Ͷ���/�ʼ������ѯ", "FrmJW2QueryActiveCard", "���Ͷ���/�ʼ������ѯ", "Audition Group")]
    public partial class FrmJW2QueryActiveCard : Form
    {

        #region ����
        private CSocketEvent m_ClientEvent = null;
        int type = 0;

        C_Global.CEnum.Message_Body[,] modiInfoResult = null;

        CEnum.Message_Body[,] mResult = null;
        CEnum.Message_Body[,] stopResult = null;
        private int iPageCount = 0;//��ҳҳ��
        bool pageRoleView = false;

        #endregion

        public FrmJW2QueryActiveCard()
        {
            InitializeComponent();
        }

        #region ��������
        /// <summary>
        /// ��������еĴ���
        /// </summary>
        /// <param name="oParent">MDI ����ĸ�����</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>����еĴ���</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //������¼����
            FrmJW2QueryActiveCard mModuleFrm = new FrmJW2QueryActiveCard();
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
            //GrpSearch.Text = config.ReadConfigValue("MAU", "GHP_UI_GrpSearch");
            //BtnSearch.Text = config.ReadConfigValue("MAU", "GHP_UI_BtnSearch");
            //LblUser.Text = config.ReadConfigValue("MAU", "GHP_UI_LblUser");
            //GrpResult.Text = config.ReadConfigValue("MAU", "GHP_UI_GrpResult");
            //this.Text = config.ReadConfigValue("MAU", "GHP_UI_GetHistoryPWD");

            this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            this.BtnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");


            lblCondition.Text = config.ReadConfigValue("MJW2", "NEW_UI_AccountNumber");
            label1.Text = config.ReadConfigValue("MJW2", "NEW_UI_CardNumber");

            this.tpgCharacter.Text = config.ReadConfigValue("MJW2", "NEW_UI_InfoQuery");
            lblRoleView.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");

            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2QueryActiveCard");
            BuildCombox();

        }

        private void BuildCombox()
        {
            try
            {
                string gameList = config.ReadConfigValue("MAU", "GHP_UI_gameList");
                string gameID = config.ReadConfigValue("MAU", "GHP_UI_gameID");
                string[] games = gameList.Split(',');
                string[] gameIDs = gameID.Split(',');

            }
            catch
            {

            }
        }



        #endregion

        #region ��½�������
        private void FrmJW2QueryActiveCard_Load(object sender, EventArgs e)
        {
            IntiFontLib();
        }
        #endregion



        #region ��ѯ�û���ʷ����
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            this.dgvResult.DataSource = null;
            //if (TxtID.Text.Trim() == "")
            //{
            //    MessageBox.Show(config.ReadConfigValue("MAU", "GHP_CODE_ErrMsg1"));
            //    return;
            //}


            //��ѯ��ʷ����

            BtnSearch.Enabled = false;
            Cursor = Cursors.WaitCursor;

            CEnum.Message_Body[] messageBody = new CEnum.Message_Body[4];

            messageBody[0].eTag = C_Global.CEnum.TagFormat.TLV_STRING;
            messageBody[0].eName = C_Global.CEnum.TagName.CARD_username;
            messageBody[0].oContent = TxtID.Text.Trim();

            messageBody[1].eTag = C_Global.CEnum.TagFormat.TLV_STRING;
            messageBody[1].eName = C_Global.CEnum.TagName.CARD_id;
            messageBody[1].oContent = textBox1.Text.Trim();

            messageBody[2].eTag = C_Global.CEnum.TagFormat.TLV_INTEGER;
            messageBody[2].eName = C_Global.CEnum.TagName.Index;
            messageBody[2].oContent =1;

            messageBody[3].eTag = C_Global.CEnum.TagFormat.TLV_INTEGER;
            messageBody[3].eName = C_Global.CEnum.TagName.PageSize;
            messageBody[3].oContent = Operation_JW2.iPageSize;
            this.backgroundWorkerSearch.RunWorkerAsync(messageBody);

        }
        #endregion
        #region ��ѯ�û���ʷ����backgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
          
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_Act_Card_Query, (CEnum.Message_Body[])e.Argument);
            }

        }
        #endregion
        #region ��ѯ�û���ʷ����backgroundWorker��Ϣ����
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;

                this.Cursor = Cursors.Default;//�ı����״̬
                this.BtnSearch.Enabled = true;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, dgvResult, out iPageCount);

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

        #region ���������ʷ�����б�
        //���������ʷ�����б�
        private void BuildPWDTable(CEnum.Message_Body[,] mResult)
        {
            try
            {
                DataTable mTable = new DataTable();
                mTable.Columns.AddRange(new DataColumn[] { new DataColumn(config.ReadConfigValue("MAU", "GHP_CODE_Pwd")), 
                    new DataColumn(config.ReadConfigValue("MAU", "GHP_CODE_ModiDate")), 
                    new DataColumn(config.ReadConfigValue("MAU", "GHP_CODE_Desc")),
                    new DataColumn(config.ReadConfigValue("MAU", "GHP_CODE_IP"))});
                for (int i = 0; i < mResult.GetLength(0); i++)
                {
                    DataRow row = mTable.NewRow();
                    for (int j = 0; j < mResult.GetLength(1); j++)
                    {
                        row[j] = mResult[i, j].oContent.ToString();
                    }
                    mTable.Rows.Add(row);
                }
                this.dgvResult.DataSource = mTable;
            }
            catch
            {
                this.dgvResult.DataSource = null;
            }
        }
        #endregion

        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_Act_Card_Query, (CEnum.Message_Body[])e.Argument);
            }

        }

        private void backgroundWorkerReSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;

                this.Cursor = Cursors.Default;//�ı����״̬

                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, dgvResult, out iPageCount);

             
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void cmbRoleView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pageRoleView)
            {

           
            CEnum.Message_Body[] messageBody = new CEnum.Message_Body[4];

            messageBody[0].eTag = C_Global.CEnum.TagFormat.TLV_STRING;
            messageBody[0].eName = C_Global.CEnum.TagName.CARD_username;
            messageBody[0].oContent = TxtID.Text.Trim();

            messageBody[1].eTag = C_Global.CEnum.TagFormat.TLV_STRING;
            messageBody[1].eName = C_Global.CEnum.TagName.CARD_id;
            messageBody[1].oContent = TxtID.Text.Trim();

            messageBody[2].eTag = C_Global.CEnum.TagFormat.TLV_INTEGER;
            messageBody[2].eName = C_Global.CEnum.TagName.Index;
            messageBody[2].oContent = 1;

            messageBody[3].eTag = C_Global.CEnum.TagFormat.TLV_INTEGER;
            messageBody[3].eName = C_Global.CEnum.TagName.PageSize;
            messageBody[3].oContent = Operation_JW2.iPageSize;
            this.backgroundWorkerReSearch.RunWorkerAsync(messageBody);
        }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       





    }
}