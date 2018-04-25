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
    [C_Global.CModuleAttribute("�������", "FrmGDNoticeManage", "�������", "GD Group")]
    public partial class FrmGDNoticeManage : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        //private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//��ҳҳ��

        string serverIP = null;
        int typeSend = 1;
        int noticeID = 0;
        int selectNotice = 0;
        string content = null;

        private bool pageNotice = false;//��ҳ���ظ���Query
        bool isCheck = false;

        #endregion

        public FrmGDNoticeManage()
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
            FrmGDNoticeManage mModuleFrm = new FrmGDNoticeManage();
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

        #region ��ʼ��������Ϣ
        /// <summary>
        ///�����ֿ�
        /// </summary>
        ConfigValue config = null;

        /// <summary>
        /// ��ʼ��������Ϣ
        /// </summary>
        private void IntiFontLib()
        {
            config = (ConfigValue)m_ClientEvent.GetInfo("INI");
            IntiUI();
        }

        private void IntiUI()
        {
            //this.lblServerList.Text = config.ReadConfigValue("MSD", "NM_UI_lblServerList");
            //this.btnSelectAll.Text = config.ReadConfigValue("MSD", "NM_UI_btnSelectAll");

            //this.btnNoticeInfo.Text = config.ReadConfigValue("MSD", "NM_UI_btnNoticeInfo");
            //this.lblNotice.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.chbNoticeImmed.Text = config.ReadConfigValue("MSD", "NM_UI_chbNoticeImme");
            //this.lblStartTime.Text = config.ReadConfigValue("MSD", "GI_UI_lblStartTime");
            //this.lblEndTime.Text = config.ReadConfigValue("MSD", "NM_UI_lblEndTime");
            //this.lblTimeSpan.Text = config.ReadConfigValue("MSD", "NM_UI_lblTimeSpan");
            //this.lblType.Text = config.ReadConfigValue("MSD", "NM_UI_lblType");
            cmbType.Items.Clear();
            //cmbType.Items.Add(config.ReadConfigValue("MSD", "NM_UI_cmbNoticeCheck"));
            cmbType.Items.Add("�S�o����");
            cmbType.Items.Add("����");
            cmbType.Items.Add("��ӹ���");
            //this.lblContent.Text = config.ReadConfigValue("MSD", "NM_UI_lblContent");
            //this.btnAddNotice.Text = config.ReadConfigValue("MSD", "NM_UI_btnAddNotice");
            //this.btnReset.Text = config.ReadConfigValue("MSD", "BP_UI_btnReset");

            pnlNotice.Visible = false;

            btnEditNotice.Enabled = false;
            btnEditNotice.Visible = false;
        }

        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDNoticeManage_Load(object sender, EventArgs e)
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
            clbServer.Enabled = true;
            clbServer.Items.Clear();
            for (int i = 0; i < mServerInfo.GetLength(0); i++)
            {
                clbServer.Items.Add(mServerInfo[i, 1].oContent.ToString());
            }
            //tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region ѡ��������Ϸ������
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (isCheck == true)
            {
                clbServer.Enabled = true;
                for (int i = 0; i < clbServer.Items.Count; i++)
                {
                    clbServer.SetItemChecked(i, false);
                }
                isCheck = false;
            }
            else if (isCheck == false)
            {
                clbServer.Enabled = false;
                for (int i = 0; i < clbServer.Items.Count; i++)
                {
                    clbServer.SetItemChecked(i, true);
                }
                isCheck = true;
            }
        }
        #endregion



        #region ���ò�ѯ��ť��ѯ������Ϣ
        private void btnNoticeInfo_Click(object sender, EventArgs e)
        {
            try
            {
                NoticeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������Ϣ
        private void NoticeList()
        {
            try
            {
                this.GrdNotice.DataSource = null;
                pnlNotice.Visible = false;
                cmbNotice.Items.Clear();
                pageNotice = false;

                this.GrpServer.Enabled = false;
                this.GrpNotice.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];

                mContent[0].eName = CEnum.TagName.Index;
                mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[0].oContent = 1;

                mContent[1].eName = CEnum.TagName.PageSize;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = Operation_GD.iPageSize;

                backgroundWorkerSearch.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_SeacrhNotes_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpServer.Enabled = true;
            this.GrpNotice.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdNotice, out iPageCount);

            if (iPageCount <= 1)
            {
                pnlNotice.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    cmbNotice.Items.Add(i + 1);
                }

                cmbNotice.SelectedIndex = 0;
                pageNotice = true;
                pnlNotice.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ������Ϣ
        private void cmbNotice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageNotice)
                {
                    this.GrdNotice.DataSource = null;

                    this.GrpServer.Enabled = false;
                    this.GrpNotice.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];

                    mContent[0].eName = CEnum.TagName.Index;
                    mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[0].oContent = int.Parse(cmbNotice.Text.Trim());

                    mContent[1].eName = CEnum.TagName.PageSize;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReSearch.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_SeacrhNotes_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpServer.Enabled = true;
            this.GrpNotice.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdNotice, out iPageCount);
        }
        #endregion



        #region ˫��������Ϣ����ɾ��
        private void GrdNotice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectNotice = int.Parse(e.RowIndex.ToString());//������
                    if (GrdNotice.DataSource != null)
                    {
                        DataTable mTable = (DataTable)GrdNotice.DataSource;
                        noticeID = int.Parse(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());
                        //serverIP = mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_ServerIP")].ToString();

                        if (MessageBox.Show("�Ƿ�ȷ��ֹͣ���͹���?", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpServer.Enabled = false;
                            this.GrpNotice.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[9];

                            //mContent[0].eName = CEnum.TagName.SD_ServerIP;
                            //mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                            //mContent[0].oContent = serverIP;

                            mContent[0].eName = CEnum.TagName.UserByID;
                            mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[0].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                            mContent[1].eName = CEnum.TagName.SD_Limit;
                            mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[1].oContent = int.Parse(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_Limit")].ToString());

                            mContent[2].eName = CEnum.TagName.SD_Content;
                            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[2].oContent = mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_Content")].ToString();

                            mContent[3].eName = CEnum.TagName.SD_EndTime;
                            mContent[3].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                            mContent[3].oContent = Convert.ToDateTime(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_EndTime")].ToString());

                            mContent[4].eName = CEnum.TagName.SD_StartTime;
                            mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                            mContent[4].oContent = Convert.ToDateTime(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_StartTime")].ToString());

                            mContent[5].eName = CEnum.TagName.SD_Type;
                            mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[5].oContent = 2;

                            mContent[6].eName = CEnum.TagName.SD_SendType;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 1;

                            mContent[7].eName = CEnum.TagName.SD_Status;
                            mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[7].oContent = 1;

                            mContent[8].eName = CEnum.TagName.SD_ID;
                            mContent[8].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[8].oContent = noticeID;

                            backgroundWorkerDeleteNotice.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerDeleteNotice_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_TaskList_Update, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ɾ��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerDeleteNotice_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpServer.Enabled = true;
            this.GrpNotice.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);
            NoticeList();
        }
        #endregion



        #region ����������Ϣ���浱ǰ�к�
        private void GrdNotice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//����ĳһ��
                {
                    selectNotice = int.Parse(e.RowIndex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
            
        #region �Ҽ����������Ϣ���б༭
        private void cmsEditNotice_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdNotice.DataSource != null)
                {
                    btnAddNotice.Enabled = false;
                    btnAddNotice.Visible = false;

                    btnEditNotice.Enabled = true;
                    btnEditNotice.Visible = true;

                    DataTable mTable = (DataTable)GrdNotice.DataSource;
                    noticeID = int.Parse(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());

                    chbNoticeImmed.Checked = false;
                    DptStartTime.Value = Convert.ToDateTime(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_StartTime")].ToString());
                    DptEndTime.Value = Convert.ToDateTime(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_EndTime")].ToString());
                    nudTimeSpan.Value = int.Parse(mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_Limit")].ToString());
                    txtContent.Text = mTable.Rows[selectNotice][config.ReadConfigValue("GLOBAL", "SD_Content")].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnAddNotice.Enabled = true;
                btnAddNotice.Visible = true;

                btnEditNotice.Enabled = false;
                btnEditNotice.Visible = false;
            }
        }
        #endregion
        
        #region ѡ���Ƿ���������
        private void chbNoticeImmed_CheckedChanged(object sender, EventArgs e)
        {
            if (chbNoticeImmed.Checked)
            {
                DptStartTime.Enabled = false;
                DptEndTime.Enabled = false;
                nudTimeSpan.Enabled = false;
                typeSend = 0;
                chbNoticeImmed.Text ="�����l��";
            } 


            else
            {
                DptStartTime.Enabled = true;
                DptEndTime.Enabled = true;
                typeSend = 1;
                chbNoticeImmed.Text = "���r�l��";
            }
        }
        #endregion



        #region ���͹���
        private void btnAddNotice_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.clbServer.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "NM_Hint_selectServer"));
                    return;
                }
                if (typeSend == 1)
                {                    
                    if (DptStartTime.Value > DptEndTime.Value)
                    {
                        MessageBox.Show(config.ReadConfigValue("GLOBAL", "NM_Hint_TimeSpan"));
                        return;
                    }
                    if (DptEndTime.Value <= DateTime.Now)
                    {
                        MessageBox.Show(config.ReadConfigValue("GLOBAL", "NM_Hint_EndTime"));
                        return;
                    }
                }
                if(cmbType.Text== "" )
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "NM_Hint_typeNull"));
                    return;
                }
                if (this.txtContent.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "NM_Hint_txtContent"));
                    return;
                }
                if (this.txtContent.Text.Trim().Length >= 512)
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "NM_Hint_contentLength"));
                    return;
                }

                serverIP = null;
                if (isCheck)
                {
                    serverIP = "255.255.255.255|";
                }
                else
                {
                    for (int i = 0; i < clbServer.CheckedItems.Count; i++)
                    {
                        serverIP += Operation_GD.GetItemAddr(mServerInfo, clbServer.CheckedItems[i].ToString());
                        serverIP += "|";
                    }
                }

                int noticeType = 0;
                if (cmbType.Text == config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeCheck"))
                {
                    noticeType = 0;
                }
                else if (cmbType.Text == config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeCommon"))
                {
                    noticeType = 1;
                }
                else if (cmbType.Text == config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeEvent"))
                {
                    noticeType = 2;
                }
                else if (cmbType.Text == config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeSystem"))
                {
                    noticeType = 3;
                }

                content = txtContent.Text.Trim().Replace("\n", "\\");

                this.GrpServer.Enabled = false;
                this.GrpNotice.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.UserByID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[2].eName = CEnum.TagName.SD_Limit;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(nudTimeSpan.Value.ToString());

                mContent[3].eName = CEnum.TagName.SD_Content;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = content;

                mContent[4].eName = CEnum.TagName.SD_EndTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DptEndTime.Value;

                mContent[5].eName = CEnum.TagName.SD_StartTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DptStartTime.Value;

                mContent[6].eName = CEnum.TagName.SD_Type;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = noticeType;

                mContent[7].eName = CEnum.TagName.SD_SendType;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = typeSend;

                backgroundWorkerSendNotice.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ���͹���backgroundWorker��Ϣ����
        private void backgroundWorkerSendNotice_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_SendNotes_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ���͹���backgroundWorker��Ϣ����
        private void backgroundWorkerSendNotice_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpServer.Enabled = true;
            this.GrpNotice.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);
            chbNoticeImmed.Checked = false;
            DptStartTime.Value = DateTime.Now;
            DptStartTime.Enabled = true;
            DptEndTime.Value = DateTime.Now;
            DptEndTime.Enabled = true;
            nudTimeSpan.Value = 1;
            nudTimeSpan.Enabled = true;
            cmbType.Items.Clear();
            cmbType.Items.Add(config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeCheck"));
            cmbType.Items.Add(config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeCommon"));
            cmbType.Items.Add(config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeEvent"));
            cmbType.Items.Add(config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeSystem"));
            txtContent.Text = "";
            NoticeList();            
        }
        #endregion



        #region �༭����
        private void btnEditNotice_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.clbServer.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "NM_Hint_selectServer"));
                    return;
                }
                if (DptStartTime.Value > DptEndTime.Value)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "NM_Hint_TimeSpan"));
                    return;
                }
                if (DptEndTime.Value <= DateTime.Now)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "NM_Hint_EndTime"));
                    return;
                }
                if (cmbType.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "NM_Hint_typeNull"));
                    return;
                }
                if (this.txtContent.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "NM_Hint_txtContent"));
                    return;
                }
                if (this.txtContent.Text.Trim().Length >= 512)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "NM_Hint_contentLength"));
                    return;
                }

                serverIP = null;
                if (isCheck)
                {
                    serverIP = "255.255.255.255,";
                }
                else
                {
                    for (int i = 0; i < clbServer.CheckedItems.Count; i++)
                    {
                        serverIP += Operation_GD.GetItemAddr(mServerInfo, clbServer.CheckedItems[i].ToString());
                        serverIP += ",";
                    }
                }

                int noticeType = 0;
                if (cmbType.Text == config.ReadConfigValue("MSD", "NM_UI_cmbNoticeCheck"))
                {
                    noticeType = 0;
                }
                else if (cmbType.Text == config.ReadConfigValue("MSD", "NM_UI_cmbNoticeCommon"))
                {
                    noticeType = 1;
                }
                else if (cmbType.Text == config.ReadConfigValue("MSD", "NM_UI_cmbNoticeEvent"))
                {
                    noticeType = 2;
                }
                else if (cmbType.Text == config.ReadConfigValue("MSD", "NM_UI_cmbNoticeSystem"))
                {
                    noticeType = 3;
                }

                content = txtContent.Text.Trim().Replace("\n", "\\");

                this.GrpServer.Enabled = false;
                this.GrpNotice.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[10];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.UserByID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[2].eName = CEnum.TagName.SD_Limit;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(nudTimeSpan.Value.ToString());

                mContent[3].eName = CEnum.TagName.SD_Content;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = content;

                mContent[4].eName = CEnum.TagName.SD_EndTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DptEndTime.Value;

                mContent[5].eName = CEnum.TagName.SD_StartTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DptStartTime.Value;

                mContent[6].eName = CEnum.TagName.SD_Type;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = noticeType;

                mContent[7].eName = CEnum.TagName.SD_SendType;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = typeSend;

                mContent[8].eName = CEnum.TagName.SD_ID;
                mContent[8].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[8].oContent = noticeID;

                mContent[9].eName = CEnum.TagName.SD_Status;
                mContent[9].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[9].oContent = 0;

                backgroundWorkerEditNotice.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region �༭����backgroundWorker��Ϣ����
        private void backgroundWorkerEditNotice_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_TaskList_Update, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region �༭����backgroundWorker��Ϣ����
        private void backgroundWorkerEditNotice_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpServer.Enabled = true;
            this.GrpNotice.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);

            btnAddNotice.Enabled = true;
            btnAddNotice.Visible = true;

            btnEditNotice.Enabled = false;
            btnEditNotice.Visible = false;

            chbNoticeImmed.Checked = false;
            DptStartTime.Value = DateTime.Now;
            DptStartTime.Enabled = true;
            DptEndTime.Value = DateTime.Now;
            DptEndTime.Enabled = true;
            nudTimeSpan.Value = 1;
            nudTimeSpan.Enabled = true;
            cmbType.Items.Clear();
            cmbType.Items.Add(config.ReadConfigValue("MSD", "NM_UI_cmbNoticeCheck"));
            cmbType.Items.Add(config.ReadConfigValue("MSD", "NM_UI_cmbNoticeCommon"));
            cmbType.Items.Add(config.ReadConfigValue("MSD", "NM_UI_cmbNoticeEvent"));
            cmbType.Items.Add(config.ReadConfigValue("MSD", "NM_UI_cmbNoticeSystem"));
            txtContent.Text = "";
            NoticeList();
        }
        #endregion



        #region ������Ϣ
        private void btnReset_Click(object sender, EventArgs e)
        {
            DptStartTime.Value = DateTime.Now;
            DptEndTime.Value = DateTime.Now;
            nudTimeSpan.Value = 1;
            txtContent.Text = "";
            chbNoticeImmed.Checked = false;
        }
        #endregion
        
        #region ����ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}