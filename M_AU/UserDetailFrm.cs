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

namespace M_Audition
{
    [C_Global.CModuleAttribute("9YOU�û���Ϣ", "Frm_YOU_UserDetail", "ע���û������� -- 9YOU�û���Ϣ", "AU Group")] 
    public partial class Frm_YOU_UserDetail : Form
    {
        private CSocketEvent m_ClientEvent = null;

        public Frm_YOU_UserDetail()
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
            Frm_YOU_UserDetail mModuleFrm = new Frm_YOU_UserDetail();
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

        #region ����

        C_Global.CEnum.Message_Body[,] modiInfoResult = null;

        CEnum.Message_Body[,] mResult = null;

        #endregion


        #region ����
        /// <summary>
        /// �޸������Ϣ
        /// </summary>
        private void ResetIDCard()
        {
            try
            {
                C_Global.CEnum.Message_Body[] messageBody = new C_Global.CEnum.Message_Body[2];

                messageBody[0].eTag = C_Global.CEnum.TagFormat.TLV_STRING;
                messageBody[0].eName = C_Global.CEnum.TagName.CARD_username;
                messageBody[0].oContent = mResult[0, 1].oContent.ToString();

                messageBody[1].eTag = C_Global.CEnum.TagFormat.TLV_INTEGER;
                messageBody[1].eName = C_Global.CEnum.TagName.UserByID;
                messageBody[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                modiInfoResult = m_ClientEvent.RequestResult(CEnum.ServiceKey.CARD_USERINFO_CLEAR, C_Global.CEnum.Msg_Category.CARD_ADMIN, messageBody);

                if (modiInfoResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(modiInfoResult[0, 0].oContent.ToString());
                    return;
                }

                if (modiInfoResult[0, 0].oContent.ToString().Equals("FAILURE"))
                {
                    MessageBox.Show("������ҵ����֤��������ʧ��");
                    return;
                }

                if (modiInfoResult[0, 0].oContent.ToString().Equals("SUCCESS"))
                {
                    MessageBox.Show("������ҵ����֤�������ͳɹ�");
                    ReadInfo();
                    return;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void ResetVerify()
        {
            try
            {
                C_Global.CEnum.Message_Body[] messageBody = new C_Global.CEnum.Message_Body[2];

                messageBody[0].eTag = C_Global.CEnum.TagFormat.TLV_STRING;
                messageBody[0].eName = C_Global.CEnum.TagName.CARD_username;
                messageBody[0].oContent = mResult[0, 1].oContent.ToString();

                messageBody[1].eTag = C_Global.CEnum.TagFormat.TLV_INTEGER;
                messageBody[1].eName = C_Global.CEnum.TagName.UserByID;
                messageBody[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                modiInfoResult = m_ClientEvent.RequestResult(CEnum.ServiceKey.CARD_USERSECURE_CLEAR, C_Global.CEnum.Msg_Category.CARD_ADMIN, messageBody);


                if (modiInfoResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(modiInfoResult[0, 0].oContent.ToString());
                    return;
                }

                if (modiInfoResult[0, 0].oContent.ToString().Equals("FAILURE"))
                {
                    MessageBox.Show("������ҵ���֤��ʧ��");
                    return;
                }

                if (modiInfoResult[0, 0].oContent.ToString().Equals("SUCCESS"))
                {
                    
                    MessageBox.Show("������ҵ���֤��ɹ�");
                    ReadInfo();

                    return;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }


        private void ReadInfo()
        {
            mResult = null;

            CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];

            mContent[0].eName = CEnum.TagName.CARD_username;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = TxtID.Text;

            mContent[1].eName = CEnum.TagName.CARD_nickname;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = TxtUser.Text;

            mContent[2].eName = CEnum.TagName.CARD_ActionType;
            mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[2].oContent = 1;

            mResult = Operation_Card.GetResult(this.m_ClientEvent, CEnum.ServiceKey.CARD_USERINFO_QUERY, mContent);

            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            TxtID.Enabled = false;
            TxtUser.Enabled = false;

            btnRestIDCode.Enabled = true;
            btnResetV.Enabled = true;

            //LabelTextBox[] lblTextBoxArray = new LabelTextBox[mResult.GetLength(1)];

            for (int i = 0; i < mResult.GetLength(1); i++)
            {
                LabelTextBox mDisplay = new LabelTextBox(false);
                //lblTextBoxArray[0] = mDisplay;

                mDisplay.Parent = PnlResult;
                mDisplay.Position = C_Controls.LabelTextBox.LabelTextBox.ELABELPOSITION.LEFT;
                mDisplay.Width = 222;

                //mDisplay.Visible = false;
                
                if (i % 2 == 0)
                {
                    mDisplay.Top = 20 * i + 30;
                    mDisplay.Left = 44;
                }
                else
                {
                    mDisplay.Top = 20 * (i - 1) + 30;
                    mDisplay.Left = mDisplay.Width + 111;
                }
                
                mDisplay.LabelText = this.m_ClientEvent.DecodeFieldName(mResult[0, i].eName) + "��";
                mDisplay.TextBoxText = mResult[0, i].oContent.ToString();
               
                
            }

            /*
            for (int i = 0; i < lblTextBoxArray.Length; i++)
            {
                lblTextBoxArray[i].IsVisable = true;
            }
            */
            //foreach (Control m in PnlResult.Controls.Find("LabelTextBox", true))
            //{
                //m.Visible = true;
            //}


            PnlResult.Visible = true;
            
            for (int i = 0; i < PnlResult.Controls.Count; i++)
            {
                if (PnlResult.Controls[i].GetType() == typeof(LabelTextBox))
                {
                    LabelTextBox mControls = (LabelTextBox)PnlResult.Controls[i];
                    mControls.ReadOnly = true;
                    mControls.IsVisable = true;
                }
            }
            
        }
        #endregion

        

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            

            if (TxtID.Text.Trim().Length <= 0 && TxtUser.Text.Trim().Length <= 0)
            {
                MessageBox.Show("������һ��������");
                return;
            }

            ReadInfo();
        }

        private void Frm_YOU_UserDetail_Load(object sender, EventArgs e)
        {
            foreach (Control m in PnlResult.Controls.Find("LabelTextBox", true))
            {
                m.Dispose();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            foreach (Control m in PnlResult.Controls.Find("LabelTextBox", true))
            {
                m.Dispose();
            }

            TxtID.Enabled = true;
            TxtUser.Enabled = true;

            btnRestIDCode.Enabled = false;
            btnResetV.Enabled = false;
        }

        private void btnResetV_Click(object sender, EventArgs e)
        {
            ResetVerify();
        }

        private void btnRestIDCode_Click(object sender, EventArgs e)
        {
            ResetIDCard();
        }
    }
}