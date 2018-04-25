using System;
using System.Reflection;
using System.Collections;

using C_Global;
using C_Socket;
using System.Windows.Forms;

namespace C_Event
{
	/// <summary>
	/// CSocketEvent Socket�¼���
	/// </summary>
	public class CSocketEvent
	{
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="strAddress">��������ַ</param>
		/// <param name="iServerPort">�������˿�</param>
		public CSocketEvent(string strAddress, int iServerPort)
		{
			mSocketClient = new CSocketClient();
			
			if (!mSocketClient.Init(strAddress, iServerPort))
			{
				throw new Exception("�޷��ҵ�������!");	
			}

			mSockData = new CSocketData();
		}

		/// <summary>
		/// ��������
		/// </summary>
		~CSocketEvent()
		{
			try
			{
				/*
				Message_Body[] mExitBody = new Message_Body[1];
				mExitBody[0].eName = C_Global.TagName.UserByID;
				mExitBody[0].eTag = C_Global.TagFormat.TLV_INTEGER;
				mExitBody[0].oContent = GetInfo("UserID");

				RequestResult(C_Global.ServiceKey.DISCONNECT, C_Global.Msg_Category.COMMON, mExitBody);
				*/

				mSocketClient.finallize();
			}
			catch (Exception)
			{
			
			}
		}

        // <summary>
        /// ��װ���ݼ�����Ϣ��
        /// </summary>
        /// <param name="queryList">���������б�</param>
        /// <param name="colCnt">��������</param>
        /// <returns>�������ݼ�����Ϣ��</returns>
        public static C_Socket.CSocketData COMMON_MES_RESP(Query_Structure[] queryList, C_Global.CEnum.Msg_Category category, C_Global.CEnum.ServiceKey service, int colCnt)
        {
            uint iPos = 0;
            TLV_Structure[] tlv = new TLV_Structure[queryList.Length * colCnt];
            int pos = 0;

            for (int i = 0; i < queryList.Length; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    //��ϢԪ�ظ�ʽ
                    C_Global.CEnum.TagFormat format_ = queryList[i].m_tagList[j].format;
                    //��ϢԪ������
                    C_Global.CEnum.TagName key_ = queryList[i].m_tagList[j].tag;
                    //��ϢԪ�ص�ֵ
                    byte[] bgMsg_Value = queryList[i].m_tagList[j].tag_buf;
                    //��Ϣ�Ľṹ��
                    TLV_Structure Msg_Value = new C_Socket.TLV_Structure(key_, format_, (uint)bgMsg_Value.Length, bgMsg_Value);
                    tlv[pos++] = Msg_Value;
                    iPos += Msg_Value.m_uiValueLen;
                }
                if (iPos + 20 >= 7192)
                {
                    pos = i;
                    break;
                }
            }
            //��װ��Ϣ��
            Packet_Body body = new C_Socket.Packet_Body(tlv, (uint)tlv.Length, (uint)colCnt);
            //��װ��Ϣͷ
            Packet_Head head = new C_Socket.Packet_Head(C_Socket.SeqId_Generator.Instance().GetNewSeqID(), category,service, body.m_uiBodyLen);
            return new C_Socket.CSocketData(new C_Socket.Packet(head, body));
        }

		/// <summary>
		/// ����ָ��������Ϣ
		/// </summary>
		/// <param name="Key">����</param>
		/// <param name="Data">����</param>
		/// <returns>T���ɹ���F��ʧ��</returns>
        public bool SaveInfo(object Key, object Data)
        {
            bool bSuccess = false;

            try
            {
                int iInfoCount = 0;
                bool bNewInfo = false;

                //�����Ƿ����
                if (this.mSaveInfo == null)
                {
                    this.mSaveInfo = new TSaveInfo[1];
                    this.mSaveInfo[0].oKey = Key;
                    this.mSaveInfo[0].oData = Data;
                }
                else
                {
                    iInfoCount = mSaveInfo.GetLength(0);

                    //������м���ֵ
                    for (int i = 0; i < iInfoCount; i++)
                    {
                        if (this.mSaveInfo[i].oKey.Equals(Key))
                        {
                            //�滻
                            bNewInfo = false;

                            this.mSaveInfo[i].oData = Data;
                        }
                        else
                        {
                            //���
                            bNewInfo = true;

                            this.mTempInfo = new TSaveInfo[iInfoCount + 1];


                            for (int j = 0; j < iInfoCount; j++)
                            {
                                this.mTempInfo[j].oKey = this.mSaveInfo[j].oKey;
                                this.mTempInfo[j].oData = this.mSaveInfo[j].oData;
                            }


                            this.mTempInfo[i].oKey = this.mSaveInfo[i].oKey;
                            this.mTempInfo[i].oData = this.mSaveInfo[i].oData;
                        }
                    }

                    //�������ս��
                    if (bNewInfo)
                    {
                        //����¼�ֵ
                        this.mTempInfo[iInfoCount].oKey = Key;
                        this.mTempInfo[iInfoCount].oData = Data;

                        //������������
                        this.mSaveInfo = this.mTempInfo;
                    }
                }

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;

                CEnum.TLogData tLogData = new CEnum.TLogData();

                tLogData.iSort = 5;
                tLogData.strDescribe = "����ؼ�������ʧ��!";
                tLogData.strException = e.Message;
            }

            return bSuccess;
        }

		/// <summary>
		/// ��ȡ��������
		/// </summary>
		/// <param name="Key">����</param>
		/// <returns>ָ����������</returns>
		public object GetInfo(object Key)
		{
			object oResult = null;

			for (int i=0; i<this.mSaveInfo.GetLength(0); i++)
			{
				if (this.mSaveInfo[i].oKey.Equals(Key))
				{
					oResult = this.mSaveInfo[i].oData;
				}
			}
			
			return oResult;
		}

		/// <summary>
		/// ȡ����Ϣ���
		/// </summary>
		/// <param name="mClient">Socket ����</param>
		/// <param name="eServerKey"></param>
		/// <param name="eMsgCategory"></param>
		/// <param name="tSendContent">���͵���Ϣ</param>
		/// <returns>��Ϣ���</returns>
        public CEnum.Message_Body[,] RequestResult(CEnum.ServiceKey eServerKey, CEnum.Msg_Category eMsgCategory, CEnum.Message_Body[] tSendContent)
		{
            CEnum.Message_Body[,] pReturnBody = null;

			try
			{
				CSocketData pSendata = mSockData.SocketSend(eServerKey, eMsgCategory, tSendContent);

				if (mSocketClient.Status())
				{			
					if (this.mSocketClient.SendDate(pSendata.bMsgBuffer))
					{
						pReturnBody = this.ReciveMessage(this.mSocketClient);
					}
					else
					{
                        pReturnBody = new CEnum.Message_Body[1, 1];
						pReturnBody[0,0].eName = CEnum.TagName.ERROR_Msg;
						pReturnBody[0,0].eTag = CEnum.TagFormat.TLV_STRING;
						pReturnBody[0,0].oContent = "ʧȥ����������";

                        CEnum.TLogData tLogData = new CEnum.TLogData();

						tLogData.iSort = 4;
						tLogData.strDescribe = "ʧȥ����������!";
						tLogData.strException = "N/A";			
					}					
				}
				else
				{
                    pReturnBody = new CEnum.Message_Body[1, 1];
                    pReturnBody[0, 0].eName = CEnum.TagName.ERROR_Msg;
                    pReturnBody[0, 0].eTag = CEnum.TagFormat.TLV_STRING;
					pReturnBody[0, 0].oContent = "ʧȥ����������";

                    CEnum.TLogData tLogData = new CEnum.TLogData();

					tLogData.iSort = 4;
					tLogData.strDescribe = "ʧȥ����������!";
					tLogData.strException = "N/A";
				}
			}
			catch (Exception e)
			{
                pReturnBody = new CEnum.Message_Body[1, 1];
                pReturnBody[0, 0].eName = CEnum.TagName.ERROR_Msg;
                pReturnBody[0, 0].eTag = CEnum.TagFormat.TLV_STRING;
				pReturnBody[0, 0].oContent = e.Message;//"ʧȥ����������";

                CEnum.TLogData tLogData = new CEnum.TLogData();

				tLogData.iSort = 5;
				tLogData.strDescribe = "���ͻ����Socket��Ϣʧ��!";
				tLogData.strException = e.Message;
			}

			return pReturnBody;
		}

        public byte[] RequestResult(byte[] val,int fileSize)
        {
            if (this.mSocketClient.SendDate(val))
            {
                return mSocketClient.ReceiveData(fileSize);
            }

            return null;
        }


		/// <summary>
		///  �ֶ�����
		/// </summary>
		/// <param name="eTagName">TagName</param>
		/// <returns>TagName ��Ӧ�ֶ�����</returns>
        public string DecodeFieldName(CEnum.TagName eTagName)
		{
			string strReturn = null;
			
			#region �����ֶ�����
            switch (eTagName)
            {
                case CEnum.TagName.UserName:// 0x0101 Format:STRING
                    strReturn = "�û�����";
                    break;
                case CEnum.TagName.PassWord:// 0x0102 Format:STRING
                    strReturn = "�û�����";
                    break;
                case CEnum.TagName.MAC:// 0x0103 Format:STRING
                    strReturn = "MAC��ַ";
                    break;
                case CEnum.TagName.Limit:// 0x0104Format:DateTime
                    strReturn = "ʹ��ʱЧ";
                    break;
                case CEnum.TagName.Status:// 0x0105 Format:STRING ״̬��Ϣ
                    strReturn = "״̬��Ϣ";
                    break;
                case CEnum.TagName.UserByID:// 0x0200 Format:NUMBER
                    strReturn = "����ԱID";
                    break;
                case CEnum.TagName.GameID:// 0x0200 Format:NUMBER
                    strReturn = "��ϷID";
                    break;
                case CEnum.TagName.ModuleName:// 0x0201 Format:STRING
                    strReturn = "ģ������";
                    break;
                case CEnum.TagName.ModuleClass:// 0x0202 Format:STRING
                    strReturn = "ģ�����";
                    break;
                case CEnum.TagName.ModuleContent:// 0x0203 Format:STRING
                    strReturn = "ģ������";
                    break;
                case CEnum.TagName.Module_ID:// 0x0301 Format:INTEGER
                    strReturn = "ģ��ID";
                    break;
                case CEnum.TagName.User_ID:// 0x0302 Format:INTEGER
                    strReturn = "�û�ID";
                    break;
                case CEnum.TagName.ModuleList:// 0x0302 Format:INTEGER
                    strReturn = "ģ���б�";
                    break;
                case CEnum.TagName.GameName:// 0x0302 Format:INTEGER
                    strReturn = "��Ϸ����";
                    break;
                case CEnum.TagName.GameContent:// 0x0302 Format:INTEGER
                    strReturn = "��Ϣ����";
                    break;
                case CEnum.TagName.Letter_ID://0x0602, //Format: String
                    strReturn = "�ż�ID";
                    break;
                case CEnum.TagName.Letter_Sender://0x0602, //Format: String
                    strReturn = "������";
                    break;
                case CEnum.TagName.Letter_Receiver://0x0603, //Format String
                    strReturn = "������";
                    break;
                case CEnum.TagName.Letter_Subject://0x0604, //Format: String
                    strReturn = "����";
                    break;
                case CEnum.TagName.Letter_Text://0x0605, //Format: String
                    strReturn = "����";
                    break;
                case CEnum.TagName.Send_Date://0x0606, //Format: Date
                    strReturn = "��������";
                    break;
                case CEnum.TagName.Process_Man://0x0607, //Format:String
                    strReturn = "������";
                    break;
                case CEnum.TagName.Process_Date://0x0608, //Format:Date
                    strReturn = "��������";
                    break;
                case CEnum.TagName.Transmit_Man://0x0609, //Format:String
                    strReturn = "��ת��";
                    break;
                case CEnum.TagName.Is_Process://0x060A, //Format:Integer
                    strReturn = "�Ƿ���";
                    break;
                case CEnum.TagName.Host_Addr:// 0x0401 Format:STRING
                    strReturn = "������ַ";
                    break;
                case CEnum.TagName.Host_Port:// 0x0402 Format:STRING
                    strReturn = "�����˿�";
                    break;
                case CEnum.TagName.Host_Pat:// 0x0403  Format:STRING
                    strReturn = "aaa";
                    break;
                case CEnum.TagName.Conn_Time:// 0x0404 Format:DateTime
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.Connect_Msg:// 0x0405 Format:STRING
                    strReturn = "����������Ϣ";
                    break;
                case CEnum.TagName.DisConnect_Msg:// 0x0406	Format:STRING
                    strReturn = "�Ͽ�������Ϣ";
                    break;
                case CEnum.TagName.Author_Msg: // 0x0407 Format: tring 
                    strReturn = "��֤��Ϣ";
                    break;
                case CEnum.TagName.Index:
                    strReturn = "��¼����";
                    break;
                case CEnum.TagName.PageSize:
                    strReturn = "ÿҳ��ʾ���ݸ���";
                    break;
                case CEnum.TagName.ERROR_Msg:
                    strReturn = "ϵͳ����";
                    break;
                case CEnum.TagName.RealName:
                    strReturn = "����";
                    break;
                case CEnum.TagName.DepartID:
                    strReturn = "����ID";
                    break;
                case CEnum.TagName.DepartName:
                    strReturn = "��������";
                    break;
                case CEnum.TagName.DepartRemark:
                    strReturn = "��������";
                    break;
                case CEnum.TagName.PageCount:
                    strReturn = "��ҳ��";
                    break;
                case CEnum.TagName.SDO_ChargeSum:
                    strReturn = "�ϼ�";
                    break;
                case CEnum.TagName.MJ_Level: // 0x0701, //Format:Integer
                case CEnum.TagName.MJ_Account: // 0x0702, //Format:String
                case CEnum.TagName.MJ_CharName: // 0x0703, //Format:String
                case CEnum.TagName.MJ_Exp: // 0x0704, //Format:Integer
                case CEnum.TagName.MJ_Exp_Next_Level: // 0x0705, //Format:Integer
                case CEnum.TagName.MJ_HP: // 0x0706, //Format:Integer
                case CEnum.TagName.MJ_HP_Max: // 0x0707, //Format:Integer 
                case CEnum.TagName.MJ_MP: // 0x0708, //Format:Integer
                case CEnum.TagName.MJ_MP_Max: // 0x0709, //Format:Integer 
                case CEnum.TagName.MJ_DP: // 0x0710, //Format:Integer
                case CEnum.TagName.MJ_DP_Increase_Ratio: // 0x0711, //Format:Integer 
                case CEnum.TagName.MJ_Exception_Dodge: // 0x0712, //Format:Integer 
                case CEnum.TagName.MJ_Exception_Recovery: // 0x0713, //Format:Integer
                case CEnum.TagName.MJ_Physical_Ability_Max: // 0x0714, //Format:Integer 
                case CEnum.TagName.MJ_Physical_Ability_Min: // 0x0715, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Ability_Max: // 0x0716, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Ability_Min: // 0x0717, //Format:Integer 
                case CEnum.TagName.MJ_Tao_Ability_Max: // 0x0718, //Format:Integer 
                case CEnum.TagName.MJ_Tao_Ability_Min: // 0x0719, //Format:Integer 
                case CEnum.TagName.MJ_Physical_Defend_Max: // 0x0720, //Format:Integer 
                case CEnum.TagName.MJ_Physical_Defend_Min: // 0x0721, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Defend_Max: // 0x0722, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Defend_Min: // 0x0723, //Format:Integer 
                case CEnum.TagName.MJ_Accuracy: // 0x0724, //Format:Integer 
                case CEnum.TagName.MJ_Phisical_Dodge: // 0x0725, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Dodge: // 0x0726, //Format:Integer 
                case CEnum.TagName.MJ_Move_Speed: // 0x0727, //Format:Integer 
                case CEnum.TagName.MJ_Attack_speed: // 0x0728, //Format:Integer 
                case CEnum.TagName.MJ_Max_Beibao: // 0x0729, //Format:Integer 
                case CEnum.TagName.MJ_Max_Wanli: // 0x0730, //Format:Integer 
                case CEnum.TagName.MJ_Max_Fuzhong: // 0x0731, //Format:Integer
                case CEnum.TagName.MJ_ActionType: // = 0x0740,//Format:Integer ���ID
                case CEnum.TagName.MJ_Time: // = 0x0741,//Format:TimeStamp  ����ʱ��
                case CEnum.TagName.MJ_CharIndex: // 0x0742,//���������
                case CEnum.TagName.MJ_CharName_Prefix: // 0x0743,//��Ұ������
                case CEnum.TagName.MJ_Exploit_Value: // 0x0744,//��ҹ�ѫֵ
                case CEnum.TagName.MJ_ServerIP:
                    strReturn = "�ͽ���Ϸ";
                    break;
                case CEnum.TagName.SDO_ServerIP:  //0x0801,//Format:String ����IP
                    strReturn = "������IP";
                    break;
                case CEnum.TagName.SDO_UserIndexID:  //0x0802,//Format:Integer ����û�ID
                    strReturn = "����û�ID";
                    break;
                case CEnum.TagName.SDO_Account:  //0x0803,//Format:String ��ҵ��ʺ�
                    strReturn = "��ҵ��ʺ�";
                    break;
                case CEnum.TagName.SDO_Level:  //0x0804,//Format:Integer ��ҵĵȼ�
                    strReturn = "��ҵĵȼ�";
                    break;
                case CEnum.TagName.SDO_Exp:  //0x0805,//Format:Integer ��ҵĵ�ǰ����ֵ
                    strReturn = "��ǰ����ֵ";
                    break;
                case CEnum.TagName.SDO_GameTotal:  //0x0806,//Format:Integer �ܾ���
                    strReturn = "��  ��  ��";
                    break;
                case CEnum.TagName.SDO_GameWin:  //0x0807,//Format:Integer ʤ����
                    strReturn = "ʤ  ��  ��";
                    break;
                case CEnum.TagName.SDO_DogFall:  //0x0808,//Format:Integer ƽ����
                    strReturn = "ƽ  ��  ��";
                    break;
                case CEnum.TagName.SDO_GameFall:  //0x0809,//Format:Integer ������
                    strReturn = "��  ��  ��";
                    break;
                case CEnum.TagName.SDO_Reputation:  //0x0810,//Format:Integer ����ֵ
                    strReturn = "��  ��  ֵ";
                    break;
                case CEnum.TagName.SDO_GCash:  //0x0811,//Format:Integer G��
                    strReturn = "���G������";
                    break;
                case CEnum.TagName.SDO_MCash:  //0x0812,//Format:Integer M��
                    strReturn = "���M������";
                    break;
                case CEnum.TagName.SDO_Address:  //0x0813,//Format:Integer ��ַ
                    strReturn = "��ҵĵ�ַ";
                    break;
                case CEnum.TagName.SDO_Age:  //0x0814,//Format:Integer ����
                    strReturn = "��ҵ�����";
                    break;
                case CEnum.TagName.SDO_ProductID:  //0x0815,//Format:Integer ��Ʒ���
                    strReturn = "��Ʒ���";
                    break;
                case CEnum.TagName.SDO_ProductName:  //0x0816,//Format:String ��Ʒ����
                    strReturn = "��Ʒ����";
                    break;
                case CEnum.TagName.SDO_ItemCode:  //0x0817,//Format:Integer ���߱��
                    strReturn = "���߱��";
                    break;
                case CEnum.TagName.SDO_ItemName:  //0x0818,//Format:String ��������
                    strReturn = "��������";
                    break;
                case CEnum.TagName.SDO_MoneyType:  //0x0819,//Format:Integer ��������
                    strReturn = "��������";
                    break;
                case CEnum.TagName.SDO_MoneyCost:  //0x0820,//Format:Integer ���ߵļ۸�
                    strReturn = "���߼۸�";
                    break;
                case CEnum.TagName.SDO_ShopTime:  //0x0821,//Format:DateTime ����ʱ��
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.SDO_MAINCH:  //0x0822,//Format:Integer ������
                    strReturn = "��  ��  ��";
                    break;
                case CEnum.TagName.SDO_SUBCH:  //0x0823,//Format:Integer ����
                    strReturn = "��      ��";
                    break;
                case CEnum.TagName.SDO_Online:  //0x0824,//Format:Integer �Ƿ�����
                    strReturn = "�Ƿ�����";
                    break;
                case CEnum.TagName.SDO_LoginTime:  //0x0825,//Format:DateTime ����ʱ��
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.SDO_LogoutTime:  //0x0826,//Format:DateTime ����ʱ��
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.SDO_AREANAME:  //0x0827,//Format tring ��������
                    strReturn = "����������";
                    break;
                case CEnum.TagName.SDO_NickName:  //0x0836,//�س�
                    strReturn = "�ء�������";
                    break;
                case CEnum.TagName.SDO_9YouAccount:  //0x0837,//9you����
                    strReturn = "9you�ʺ�";
                    break;
                case CEnum.TagName.SDO_SEX:  //0x0838,//�Ա�
                    strReturn = "�ԡ�����";
                    break;
                case CEnum.TagName.SDO_RegistDate:  //0x0839,//ע������
                    strReturn = "ע������";
                    break;
                case CEnum.TagName.SDO_FirstLogintime:  //0x0840,//��һ�ε�¼ʱ��
                    strReturn = "��һ�ε�¼ʱ��";
                    break;
                case CEnum.TagName.SDO_LastLogintime:  //0x0841,//���һ�ε�¼ʱ��
                    strReturn = "���һ�ε�¼ʱ��";
                    break;
                case CEnum.TagName.SDO_Ispad:  //0x0842,//�Ƿ���ע������̺
                    strReturn = "����̺״̬";
                    break;
                case CEnum.TagName.SDO_Desc:// = 0x0843,//��������
                    strReturn = "��������";
                    break;
                case CEnum.TagName.SDO_Postion:// = 0x0844,//����λ��
                    strReturn = "����λ��";
                    break;
                case CEnum.TagName.SDO_MinLevel:// = 0x0843,//��������
                    strReturn = "����ȼ�";
                    break;
                case CEnum.TagName.SDO_DateLimit:// = 0x0844,//����λ��
                    strReturn = "��������";
                    break;
                case CEnum.TagName.SDO_TimesLimit:// = 0x0844,//����λ��
                    strReturn = "ʹ�ô���";
                    break;
                case CEnum.TagName.SDO_City:
                    strReturn = "���ڳ���";
                    break;
                case CEnum.TagName.SDO_BeginTime: //0x0845,//Format:Date ���Ѽ�¼��ʼʱ��
                    strReturn = "���Ѽ�¼��ʼʱ��";
                    break;
                case CEnum.TagName.SDO_EndTime: //0x0846,//Format:Date ���Ѽ�¼����ʱ��
                    strReturn = "���Ѽ�¼����ʱ��";
                    break;
                case CEnum.TagName.SDO_SendTime: //0x0847,//Format:Date ������������
                    strReturn = "������������";
                    break;
                case CEnum.TagName.SDO_SendIndexID: //0x0848,//Format:Integer �����˵�ID
                    strReturn = "�����˵�ID";
                    break;
                case CEnum.TagName.SDO_SendUserID: //0x0849,//Format:String �������ʺ�
                    strReturn = "�������ʺ�";
                    break;
                case CEnum.TagName.SDO_ReceiveNick: //0x0850,//Format:String �������س�
                    strReturn = "�������س�";
                    break;
                case CEnum.TagName.SDO_BigType: //0x0851,//Format:Integer ���ߴ���
                    strReturn = "���ߴ���";
                    break;
                case CEnum.TagName.SDO_SmallType: // 0x0852,//Format:Integer ����С��
                    strReturn = "����С��";
                    break;
                case CEnum.TagName.SP_Name: //0x0412,//Format:String �洢������
                    strReturn = "�洢������";
                    break;
                case CEnum.TagName.Real_ACT: //0x0413,//Format:String ����������
                    strReturn = "����������";
                    break;
                case CEnum.TagName.ACT_Time: //0x0414,//Format:TimeStamp ����ʱ��
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.BeginTime:// = 0x0415,//Format:Date ��ʼ����
                    strReturn = "��ʼ����";
                    break;
                case CEnum.TagName.EndTime:// = 0x0416,//Format:Date ��������
                    strReturn = "��������";
                    break;
                case CEnum.TagName.SDO_Title:
                    strReturn = "��    ��";
                    break;
                case CEnum.TagName.SDO_Context:
                    strReturn = "�ڡ�����";
                    break;
                case CEnum.TagName.SDO_Email:
                    strReturn = "?��?��??";
                    break;
                case CEnum.TagName.SDO_StopTime:
                    strReturn = "ͣ��ʱ��";
                    break;
                case CEnum.TagName.AU_ACCOUNT:// = 0x1001,//����ʺ�
                    strReturn = "����ʺ�";
                    break;
                case CEnum.TagName.AU_UserNick:// = 0x1002,//����س�
                    strReturn = "����س�";
                    break;
                case CEnum.TagName.AU_Sex:// = 0x1003,//����Ա�
                    strReturn = "����Ա�";
                    break;
                case CEnum.TagName.AU_State:// = 0x1004,//���״̬
                    strReturn = "���״̬";
                    break;
                case CEnum.TagName.AU_STOPSTATUS:// = 0x1005,//�����߷�ͣ״̬
                    strReturn = "�����߷�ͣ״̬";
                    break;
                case CEnum.TagName.AU_Reason:// 0x1006,//��ͣ����
                    strReturn = "��ͣ����";
                    break;
                case CEnum.TagName.AU_BanDate:// = 0x1007,//��ͣ����
                    strReturn = "��ͣ����";
                    break;
                case CEnum.TagName.AU_ServerIP:// = 0x1008,//��������Ϸ������ Format:String
                    strReturn = "��������Ϸ������";
                    break;
                case CEnum.TagName.CR_ServerIP://0x1101,//������IP
                    strReturn = "������";
                    break;
                case CEnum.TagName.CR_ACCOUNT://0x1102,//����ʺ� Format:String
                    strReturn = "����ʺ�";
                    break;
                case CEnum.TagName.CR_Passord://0x1103,//������� Format:String
                    strReturn = "�������";
                    break;
                case CEnum.TagName.CR_NUMBER://0x1104,//������ Format:String
                    strReturn = "������";
                    break;
                case CEnum.TagName.CR_ISUSE://0x1105,//�Ƿ�ʹ��
                    strReturn = "�Ƿ�ʹ��";
                    break;
                case CEnum.TagName.CR_STATUS://0x1106,//���״̬ Format:Integer
                    strReturn = "���״̬";
                    break;
                case CEnum.TagName.CR_ActiveIP://0x1107,//���������IP Format:String
                    strReturn = "���������";
                    break;
                case CEnum.TagName.CR_ActiveDate://0x1108,//�������� Format:TimeStamp
                    strReturn = "��������";
                    break;
                case CEnum.TagName.CR_BoardID://0x1109,//����ID Format:Integer
                    strReturn = "����ID";
                    break;
                case CEnum.TagName.CR_BoardContext://0x1110,//�������� Format:String
                    strReturn = "��������";
                    break;
                case CEnum.TagName.CR_BoardColor://0x1111,//������ɫ Format:String
                    strReturn = "������ɫ";
                    break;
                case CEnum.TagName.CR_ValidTime://0x1112,//��Чʱ�� Format:TimeStamp
                    strReturn = "��Чʱ��";
                    break;
                case CEnum.TagName.CR_InValidTime://0x1113,//ʧЧʱ�� Format:TimeStamp
                    strReturn = "ʧЧʱ��";
                    break;
                case CEnum.TagName.CR_Valid://0x1114,//�Ƿ���Ч Format:Integer
                    strReturn = "�Ƿ���Ч";
                    break;
                case CEnum.TagName.CR_PublishID://0x1115,//������ID Format:Integer
                    strReturn = "������ID";
                    break;
                case CEnum.TagName.CR_DayLoop://0x1116,//ÿ�첥�� Format:Integer
                    strReturn = "ÿ�첥��";
                    break;
                case CEnum.TagName.CR_SPEED:
                    strReturn = "�����ٶ�";
                    break;
                case CEnum.TagName.CR_Mode:
                    strReturn = "���ŷ�ʽ";
                    break;
                case CEnum.TagName.CR_Channel:
                    strReturn = "Ƶ��ID";
                    break;
                case CEnum.TagName.CR_ACTION:
                    strReturn = "ʹ��״̬";
                    break;
                case CEnum.TagName.CR_Expire:
                    strReturn = "�Ƿ���Ч";
                    break;
                case CEnum.TagName.CR_BoardContext1:
                    strReturn = "�Ƿ���Ч2";
                    break;
                case CEnum.TagName.CR_BoardContext2:
                    strReturn = "�Ƿ���Ч3";
                    break;
                case CEnum.TagName.CARD_PDID:// 0x1202,
                    strReturn = "��ֵID";
                    break;
                case CEnum.TagName.CARD_PDkey:// 0x1203,
                    strReturn = "���׺�";
                    break;
                case CEnum.TagName.CARD_PDCardType:// 0x1204,
                    strReturn = "��ֵ�㿨����";
                    break;
                case CEnum.TagName.CARD_PDFrom:// 0x1205,
                    strReturn = "��ֵ��Դ";
                    break;
                case CEnum.TagName.CARD_PDCardNO:// 0x1206,
                    strReturn = "��ֵ����";
                    break;
                case CEnum.TagName.CARD_PDCardPASS:// 0x1207,
                    strReturn = "��ֵ������";
                    break;
                case CEnum.TagName.CARD_PDCardPrice:// 0x1208,
                    strReturn = "��ֵ����ֵ";
                    break;
                case CEnum.TagName.CARD_PDaction:// 0x1209,
                    strReturn = "��ֵ��ʽ";
                    break;
                case CEnum.TagName.CARD_PDuserid:// 0x1210,
                    strReturn = "��ֵ��ID";
                    break;
                case CEnum.TagName.CARD_PDusername:// 0x1211,
                    strReturn = "��ֵ��";
                    break;
                case CEnum.TagName.CARD_PDgetuserid:// 0x1212,
                    strReturn = "����ֵ��ID";
                    break;
                case CEnum.TagName.CARD_PDgetusername:// 0x1213,
                    strReturn = "��ֵ����";
                    break;
                case CEnum.TagName.CARD_PDdate:// 0x1214,
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.CARD_PDip:// 0x1215,
                    strReturn = "������IP";
                    break;
                case CEnum.TagName.CARD_PDstatus:// 0x1216,
                    strReturn = "���β���״̬";
                    break;
                case CEnum.TagName.CARD_UDID:// 0x1217,
                    strReturn = "��ֵID";
                    break;
                case CEnum.TagName.CARD_UDkey:// 0x1218,
                    strReturn = "���׺�";
                    break;
                case CEnum.TagName.CARD_UDusedo:// 0x1219,
                    strReturn = "����Ŀ��";
                    break;
                case CEnum.TagName.CARD_UDdirect:// 0x1220,
                    strReturn = "�Ƿ�ֱ����Ϸ";
                    break;
                case CEnum.TagName.CARD_UDuserid:// 0x1221,
                    strReturn = "ʹ����ID";
                    break;
                case CEnum.TagName.CARD_UDusername:// 0x1222,
                    strReturn = "������";
                    break;
                case CEnum.TagName.CARD_UDgetuserid:// 0x1223,
                    strReturn = "��ʹ����ID";
                    break;
                case CEnum.TagName.CARD_UDgetusername:// 0x1224,
                    strReturn = "���Ѷ���";
                    break;
                case CEnum.TagName.CARD_UDcoins:// 0x1225,
                    strReturn = "���";
                    break;
                case CEnum.TagName.CARD_UDtype:// 0x1226,
                    strReturn = "���ѷ�ʽ";
                    break;
                case CEnum.TagName.CARD_UDtargetvalue:// 0x1227,
                    strReturn = "ʹ��Ŀ�ı�����";
                    break;
                case CEnum.TagName.CARD_UDzone1:// 0x1228,
                    strReturn = "ʹ�÷���������1";
                    break;
                case CEnum.TagName.CARD_UDzone2:// 0x1229,
                    strReturn = "ʹ�÷���������2";
                    break;
                case CEnum.TagName.CARD_UDdate:// 0x1230,
                    strReturn = "����ʱ��";
                    break;
                case CEnum.TagName.CARD_UDip:// 0x1231,
                    strReturn = "������IP";
                    break;
                case CEnum.TagName.CARD_UDstatus:// 0x1232,
                    strReturn = "���β���״̬";
                    break;
                case CEnum.TagName.CARD_cardnum:// 0x1233,
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_cardpass:// 0x1234,
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_serial:// 0x1235,
                    strReturn = "�㿨���";
                    break;
                case CEnum.TagName.CARD_draft:// 0x1236,
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_type1:// 0x1237,
                    strReturn = "����1";
                    break;
                case CEnum.TagName.CARD_type2:// 0x1238,
                    strReturn = "����2";
                    break;
                case CEnum.TagName.CARD_type3:// 0x1239,
                    strReturn = "����3";
                    break;
                case CEnum.TagName.CARD_type4:// 0x1240,
                    strReturn = "����4";
                    break;
                case CEnum.TagName.CARD_price:// 0x1241,
                    strReturn = "���";
                    break;
                case CEnum.TagName.CARD_valid_date:// 0x1242,
                    strReturn = "��Ч��";
                    break;
                case CEnum.TagName.CARD_use_status:// 0x1243,
                    strReturn = "�Ƿ�ʹ��";
                    break;
                case CEnum.TagName.CARD_cardsent:// 0x1244,
                    strReturn = "????";
                    break;
                case CEnum.TagName.CARD_create_date:// 0x1245,
                    strReturn = "ʹ��ʱ��";
                    break;
                case CEnum.TagName.CARD_use_userid:// 0x1246,
                    strReturn = "ʹ����ID";
                    break;
                case CEnum.TagName.CARD_use_username:// 0x1247,
                    strReturn = "ʹ�����û���";
                    break;
                case CEnum.TagName.CARD_partner:// 0x1248,
                    strReturn = "��������ID";
                    break;
                case CEnum.TagName.CARD_skey:// 0x1249,
                    strReturn = "��ϸ��ID";
                    break;
                case CEnum.TagName.CARD_ActionType:
                    strReturn = "��������";
                    break;
                case CEnum.TagName.CARD_id:// 0x1251 ,//TLV_STRING ��֮��ע�Ῠ��
                    strReturn = "���֤����";
                    break;
                case CEnum.TagName.CARD_username:// 0x1252,//TLV_STRING ��֮��ע���û���
                    strReturn = "ע���û���";
                    break;
                case CEnum.TagName.CARD_nickname:// 0x1253,//TLV_STRING ��֮��ע���س�
                    strReturn = "ע���س�";
                    break;
                case CEnum.TagName.CARD_password:// 0x1254,//TLV_STRING ��֮��ע������
                    strReturn = "ע������";
                    break;
                case CEnum.TagName.CARD_sex:// 0x1255,//TLV_STRING ��֮��ע���Ա�
                    strReturn = "ע���Ա�";
                    break;
                case CEnum.TagName.CARD_rdate:// 0x1256,//TLV_Date ��֮��ע������
                    strReturn = "ע������";
                    break;
                case CEnum.TagName.CARD_rtime:// 0x1257,//TLV_Time ��֮��ע��ʱ��
                    strReturn = "ע��ʱ��";
                    break;
                case CEnum.TagName.CARD_securecode:// 0x1258,//TLV_STRING ��ȫ��
                    strReturn = "��ȫ��";
                    break;
                case CEnum.TagName.CARD_vis:// 0x1259,//TLV_INTEGER
                    strReturn = "~~~";
                    break;
                case CEnum.TagName.CARD_logdate:// 0x1260,//TLV_TimeStamp ����
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_realname:// 0x1263,//TLV_STRING ��ʵ����
                    strReturn = "��ʵ����";
                    break;
                case CEnum.TagName.CARD_birthday:// 0x1264,//TLV_Date ��������
                    strReturn = "��������";
                    break;
                case CEnum.TagName.CARD_cardtype:// 0x1265,//TLV_STRING
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_email:// 0x1267,//TLV_STRING �ʼ�
                    strReturn = "�ʼ�";
                    break;
                case CEnum.TagName.CARD_occupation:// 0x1268,//TLV_STRING ְҵ
                    strReturn = "ְҵ";
                    break;
                case CEnum.TagName.CARD_education:// 0x1269,//TLV_STRING �����̶�
                    strReturn = "�����̶�";
                    break;
                case CEnum.TagName.CARD_marriage:// 0x1270,//TLV_STRING ���
                    strReturn = "���";
                    break;
                case CEnum.TagName.CARD_constellation:// 0x1271,//TLV_STRING ����
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_shx:// 0x1272,//TLV_STRING ��Ф
                    strReturn = "��Ф";
                    break;
                case CEnum.TagName.CARD_city:// 0x1273,//TLV_STRING ����
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_address:// 0x1274,//TLV_STRING ��ϵ��ַ
                    strReturn = "��ϵ��ַ";
                    break;
                case CEnum.TagName.CARD_phone:// 0x1275,//TLV_STRING ��ϵ�绰
                    strReturn = "��ϵ�绰";
                    break;
                case CEnum.TagName.CARD_qq:// 0x1276,//TLV_STRING QQ
                    strReturn = "QQ��";
                    break;
                case CEnum.TagName.CARD_intro:// 0x1277,//TLV_STRING ����
                    strReturn = "����";
                    break;
                case CEnum.TagName.CARD_msn:// 0x1278,//TLV_STRING MSN
                    strReturn = "MSN��ַ";
                    break;
                case CEnum.TagName.CARD_mobilephone:// 0x1279,//TLV_STRING �ƶ��绰
                    strReturn = "�ƶ��绰";
                    break;
                case CEnum.TagName.CARD_SumTotal:
                    strReturn = "�ϼ�";
                    break;
                case CEnum.TagName.AuShop_orderid://=0x1301,//int(11)
                    strReturn = "���";
                    break;

                case CEnum.TagName.AuShop_udmark://=0x1302,//int(8)
                    strReturn = "udmark";
                    break;

                case CEnum.TagName.AuShop_bkey://=0x1303,//varchar(40)
                    strReturn = "bkey";
                    break;

                case CEnum.TagName.AuShop_pkey://=0x1304,//varchar(18)
                    strReturn = "pkey";
                    break;

                case CEnum.TagName.AuShop_userid://=0x1305,//int(11)
                    strReturn = "��ұ��";
                    break;

                case CEnum.TagName.AuShop_username://=0x1306,//varchar(20)
                    strReturn = "�û���";
                    break;

                case CEnum.TagName.AuShop_getuserid://=0x1307,//int(11)
                    strReturn = "������ұ��";
                    break;

                case CEnum.TagName.AuShop_getusername://=0x1308,//varchar(20)
                    strReturn = "���͸�";
                    break;

                case CEnum.TagName.AuShop_pcategory://=0x1309,//smallint(4)
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_pisgift://=0x1310,//enum('y','n')
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_islover://=0x1311,//enum('y','n')
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_ispresent://=0x1312,//enum('y','n')
                    strReturn = "����Ʒ";
                    break;

                case CEnum.TagName.AuShop_isbuysong://=0x1313,//enum('y','n')
                    strReturn = "�������";
                    break;

                case CEnum.TagName.AuShop_prule://=0x1314,//tinyint(1)
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_psex://=0x1315,//enum('all','m','f')
                    strReturn = "�Ա�";
                    break;

                case CEnum.TagName.AuShop_pbuytimes://=0x1316,//int(11)
                    strReturn = "����ʱ��";
                    break;

                case CEnum.TagName.AuShop_allprice://=0x1317,//int(11)
                    strReturn = "�Ƽ�";
                    break;

                case CEnum.TagName.AuShop_allaup://=0x1318,//int(11)
                    strReturn = "�̳ǻ���";
                    break;

                case CEnum.TagName.AuShop_buytime://=0x1319,//int(10)
                    strReturn = "����ʱ��";
                    break;

                case CEnum.TagName.AuShop_buytime2://=0x1320,//datetime
                    strReturn = "����ʱ��";
                    break;

                case CEnum.TagName.AuShop_buyip://=0x1321,//varchar(15)
                    strReturn = "������IP";
                    break;

                case CEnum.TagName.AuShop_zone://=0x1322,//tinyint(2)
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_status://=0x1323,//tinyint(1)
                    strReturn = "״̬";
                    break;

                case CEnum.TagName.AuShop_pid://=0x1324,//int(11)
                    strReturn = "ID";
                    break;

                case CEnum.TagName.AuShop_pname://=0x1326,//varchar(20)
                    strReturn = "��������";
                    break;

                case CEnum.TagName.AuShop_pgift://=0x1328,//enum('y','n')
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_pscash://=0x1330,//tinyint(2)
                    strReturn = "�ֽ�";
                    break;

                case CEnum.TagName.AuShop_pgamecode://=0x1331,//varchar(200)
                    strReturn = "��Ϸ����";
                    break;

                case CEnum.TagName.AuShop_pnew://=0x1332,//enum('y','n')
                    strReturn = "��";
                    break;

                case CEnum.TagName.AuShop_phot://=0x1333,//enum('y','n')
                    strReturn = "��";
                    break;

                case CEnum.TagName.AuShop_pcheap://=0x1334,//enum('y','n')
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_pchstarttime://=0x1335,//int(10)
                    strReturn = "��ʼʱ��";
                    break;

                case CEnum.TagName.AuShop_pchstoptime://=0x1336,//int(10)
                    strReturn = "ֹͣʱ��";
                    break;

                case CEnum.TagName.AuShop_pstorage://=0x1337,//smallint(5)
                    strReturn = "�ֿ�";
                    break;

                case CEnum.TagName.AuShop_pautoprice://=0x1339,//enum('y','n')
                    strReturn = "�Զ���۸�";
                    break;

                case CEnum.TagName.AuShop_price://=0x1340,//int(8)
                    strReturn = "�۸�";
                    break;

                case CEnum.TagName.AuShop_chprice://=0x1341,//int(8)
                    strReturn = "�۸�";
                    break;

                case CEnum.TagName.AuShop_aup://=0x1342,//int(8)
                    strReturn = "�̳ǻ���";
                    break;

                case CEnum.TagName.AuShop_chaup://=0x1343,//int(8)
                    strReturn = "chaup";
                    break;

                case CEnum.TagName.AuShop_ptimeitem://=0x1344,//varchar(200)
                    strReturn = "ptimeitem";
                    break;

                case CEnum.TagName.AuShop_pricedetail://=0x1345,//varchar(254)
                    strReturn = "�۸�˵��";
                    break;

                case CEnum.TagName.AuShop_pdesc://=0x1347,//text
                    strReturn = "pdesc";
                    break;

                case CEnum.TagName.AuShop_pbuys://=0x1348,//int(8)
                    strReturn = "����";
                    break;

                case CEnum.TagName.AuShop_pfocus://=0x1349,//tinyint(1)
                    strReturn = "λ��";
                    break;

                case CEnum.TagName.AuShop_pmark1://=0x1350,//enum('y','n')
                    strReturn = "��־";
                    break;

                case CEnum.TagName.AuShop_pmark2://=0x1351,//enum('y','n')
                    strReturn = "��־";
                    break;

                case CEnum.TagName.AuShop_pmark3://=0x1352,//enum('y','n')
                    strReturn = "��־";
                    break;

                case CEnum.TagName.AuShop_pinttime://=0x1353,//int(10)
                    strReturn = "�һ�ʱ��";
                    break;

                case CEnum.TagName.AuShop_pdate://=0x1354,//int(10)
                    strReturn = "pdate";
                    break;

                case CEnum.TagName.AuShop_pisuse://=0x1355,//enum('y','n')
                    strReturn = "�Ƿ�ʹ��";
                    break;

                case CEnum.TagName.AuShop_ppic://=0x1356,//varchar(36)
                    strReturn = "����ͼƬ";
                    break;

                case CEnum.TagName.AuShop_ppic1://=0x1357,//varchar(36)
                    strReturn = "����ͼƬ";
                    break;

                case CEnum.TagName.AuShop_usefeesum://=0x1358,//int
                    strReturn = "ʹ�û����ܶ�";
                    break;

                case CEnum.TagName.AuShop_useaupsum://=0x1359,//int
                    strReturn = "ʹ���̳ǻ����ܶ�";
                    break;

                case CEnum.TagName.AuShop_buyitemsum://=0x1360,//int
                    strReturn = "��������ܶ�";
                    break;

                case CEnum.TagName.AuShop_BeginDate://=0x1361,//date
                    strReturn = "��ʼʱ��";
                    break;

                case CEnum.TagName.AuShop_EndDate://=0x1362,//date
                    strReturn = "����ʱ��";
                    break;
                
                case CEnum.TagName.AuShop_GCashSum:// = 0x1363,
                    strReturn = "G���ܺ�";
                    break;

                case CEnum.TagName.AuShop_MCashSum:// = 0x1364,
                    strReturn = "M���ܺ�";
                    break;
                case CEnum.TagName.SDO_DaysLimit:
                    strReturn = "��������";
                    break;
                case CEnum.TagName.SDO_FirstPadTime:
                    strReturn = "��һ��ʹ������̺ʱ��";
                    break;
                default:
                    strReturn = "δ֪";
                    break;
			}
			#endregion

			return strReturn;
		}



        /// <summary>
        /// ��ȡCSocketEvent���Ѵ��ServersCount��IpForServer + i��(CSocketEvent)(Server + i)
        /// 
        /// </summary>
        /// <param name="m_ClientEvent"></param>
        /// <returns></returns>
        public CSocketEvent GetSocket(CSocketEvent m_ClientEvent, string sCurrServerIp)
        {
            CSocketEvent returnValue = null;

            if (sCurrServerIp == null)
            {
                return m_ClientEvent;
            }

            int ServersCount = int.Parse(m_ClientEvent.GetInfo("ServersCount").ToString());

            for (int i = 1; i <= ServersCount; i++)
            {
                if ((m_ClientEvent.GetInfo("IpForServer" + i).ToString()).IndexOf(sCurrServerIp) != -1)
                {
                    returnValue = (CSocketEvent)m_ClientEvent.GetInfo("Server" + i);
                    break;
                }
            }

            if (returnValue == null)
                returnValue = m_ClientEvent;


            return returnValue;

        }

		#region ˽�б���
		private struct TSaveInfo
		{
			public object oKey;
			public object oData;
		}
		private TSaveInfo[] mSaveInfo = null;
		private TSaveInfo[] mTempInfo = null;
		private CSocketClient mSocketClient = null;
		private CSocketData mSockData = null;

		/// <summary>
		/// �����ֶ�����
		/// </summary>
		/// <param name="iRow">�б�ǩ</param>
		/// <param name="iField">�ֶα�ǩ</param>
		/// <param name="tTlv">��Ϣ��</param>
		/// <param name="tBody">��Ϣ����</param>
		/// <returns>��Ϣ����</returns>
        private CEnum.Message_Body[,] DecodeRecive(int iRow, int iField, TLV_Structure tTlv, CEnum.Message_Body[,] tBody)
		{
            #region �����ֶ�����
            switch (tTlv.m_Tag)
            {
                case CEnum.TagName.UserName:// 0x0101 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.PassWord:// 0x0102 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.MAC:// 0x0103 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Limit:// 0x0104Format:DateTime
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_DATE;
                    tBody[iRow, iField].oContent = tTlv.toDate();
                    break;
                case CEnum.TagName.User_Status:// 0x0103 Format:INT
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.UserByID:// 0x0104Format:DateTime
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.GameID:// 0x0200 Format:INT
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.ModuleName:// 0x0201 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.ModuleClass:// 0x0202 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.ModuleContent:// 0x0203 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Module_ID:// 0x0301 Format:INTEGER
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.User_ID:// 0x0302 Format:INTEGER
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.ModuleList:// 0x0302 Format:INTEGER
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.GameName:// 0x0302 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.GameContent:// 0x0302 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Letter_ID://0x0602, //Format: String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.Letter_Sender://0x0602, //Format: String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Letter_Receiver://0x0603, //Format String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Letter_Subject://0x0604, //Format: String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Letter_Text://0x0605, //Format: String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Send_Date://0x0606, //Format: Date
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_DATE;
                    tBody[iRow, iField].oContent = tTlv.toDate();
                    break;
                case CEnum.TagName.Process_Man://0x0607, //Format:String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Process_Date://0x0608, //Format:Date
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_DATE;
                    tBody[iRow, iField].oContent = tTlv.toDate();
                    break;
                case CEnum.TagName.Transmit_Man://0x0609, //Format:String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Is_Process://0x060A, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.Host_Addr:// 0x0401 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Host_Port:// 0x0402 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Host_Pat:// 0x0403  Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Conn_Time:// 0x0404 Format:DateTime
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_DATE;
                    tBody[iRow, iField].oContent = tTlv.toDate();
                    break;
                case CEnum.TagName.Connect_Msg:// 0x0405 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.DisConnect_Msg:// 0x0406	Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Status://0x0408 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Index:
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.PageSize:
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.RealName://0x0408 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.DepartID://0x0408 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.DepartName://0x0408 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.DepartRemark://0x0408 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.Author_Msg:// 0x0407	Format:STRING
                case CEnum.TagName.ERROR_Msg:	//0xFFFF Format:String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.PageCount://0x0408 Format:STRING
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Level: // 0x0701, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Account: // 0x0702, //Format:String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.MJ_CharName: // 0x0703, //Format:String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                    break;
                case CEnum.TagName.MJ_Exp: // 0x0704, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Exp_Next_Level: // 0x0705, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_HP: // 0x0706, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_HP_Max: // 0x0707, //Format:Integer 
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_MP: // 0x0708, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_MP_Max: // 0x0709, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_DP: // 0x0710, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_DP_Increase_Ratio: // 0x0711, //Format:Integer 
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Exception_Dodge: // 0x0712, //Format:Integer 
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Exception_Recovery: // 0x0713, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Physical_Ability_Max: // 0x0714, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Physical_Ability_Min: // 0x0715, //Format:Integer 
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Magic_Ability_Max: // 0x0716, //Format:Integer
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_Magic_Ability_Min: // 0x0717, //Format:Integer 
                case CEnum.TagName.MJ_Tao_Ability_Max: // 0x0718, //Format:Integer 
                case CEnum.TagName.MJ_Tao_Ability_Min: // 0x0719, //Format:Integer 
                case CEnum.TagName.MJ_Physical_Defend_Max: // 0x0720, //Format:Integer 
                case CEnum.TagName.MJ_Physical_Defend_Min: // 0x0721, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Defend_Max: // 0x0722, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Defend_Min: // 0x0723, //Format:Integer 
                case CEnum.TagName.MJ_Accuracy: // 0x0724, //Format:Integer 
                case CEnum.TagName.MJ_Phisical_Dodge: // 0x0725, //Format:Integer 
                case CEnum.TagName.MJ_Magic_Dodge: // 0x0726, //Format:Integer 
                case CEnum.TagName.MJ_Move_Speed: // 0x0727, //Format:Integer 
                case CEnum.TagName.MJ_Attack_speed: // 0x0728, //Format:Integer 
                case CEnum.TagName.MJ_Max_Beibao: // 0x0729, //Format:Integer 
                case CEnum.TagName.MJ_Max_Wanli: // 0x0730, //Format:Integer 
                case CEnum.TagName.MJ_Max_Fuzhong: // 0x0731, //Format:Integer
                case CEnum.TagName.MJ_TypeID:
                case CEnum.TagName.MJ_Money:
                case CEnum.TagName.MJ_ActionType: // = 0x0740,//Format:Integer ���ID
                case CEnum.TagName.SDO_9YouAccount://0x0837,//9you���ʺ�
                case CEnum.TagName.MJ_CharIndex: // 0x0742,//���������
                case CEnum.TagName.MJ_Exploit_Value: // 0x0744,//��ҹ�ѫֵ
                case CEnum.TagName.CR_SPEED:// = 0x1129,//�����ٶ�Format:Integer
                
                case CEnum.TagName.CR_PSTID:// = 0x1117,//ע��� Format:Integer
                case CEnum.TagName.CR_SEX:// = 0x1118,//�Ա� Format:Integer
                case CEnum.TagName.CR_LEVEL:// = 0x1119,//�ȼ� Format:Integer
                case CEnum.TagName.CR_EXP:// = 0x1120,//���� Format:Integer
                case CEnum.TagName.CR_License:// = 0x1121,//����Format:Integer
                case CEnum.TagName.CR_Money:// = 0x1122,//��ǮFormat:Integer
                case CEnum.TagName.CR_RMB:// = 0x1123,//�����Format:Integer
                case CEnum.TagName.CR_RaceTotal:// = 0x1124,//��������Format:Integer
                case CEnum.TagName.CR_RaceWon:// = 0x1125,//ʤ������Format:Integer
                case CEnum.TagName.CR_ExpOrder:// = 0x1126,//��������Format:Integer
                case CEnum.TagName.CR_WinRateOrder:// = 0x1127,//ʤ������Format:Integer
                case CEnum.TagName.CR_WinNumOrder:// = 0x1128,//ʤ����������Format:Integer
                case CEnum.TagName.CR_Channel:// = 0x1133,//Ƶ��ID
                case CEnum.TagName.CR_Expire:// = 0x1137,//��Ч��ʽ
                case CEnum.TagName.CR_ChannelID:
                ////////////////////////////
                case CEnum.TagName.UpdateFileSize:// = 0x0115//Format:Integer �ļ���С]
                case CEnum.TagName.AuShop_GCashSum:// = 0x1363,
                case CEnum.TagName.AuShop_MCashSum:// = 0x1364,
                case CEnum.TagName.CARD_use_userid:// 0x1246
                ///
                
                case CEnum.TagName.o2jam_SenderIndexID://=0x1410,//int
                case CEnum.TagName.o2jam_ReceiverIndexID://=0x1413,//int




                case CEnum.TagName.o2jam_GEM://=0x1422,//int
                case CEnum.TagName.o2jam_MCASH://=0x1423,//int
                case CEnum.TagName.o2jam_O2CASH://=0x1424,//int
                case CEnum.TagName.o2jam_MUSICCASH://=0x1425,//int
                case CEnum.TagName.o2jam_ITEMCASH://=0x1426,//int
                case CEnum.TagName.o2jam_USER_INDEX_ID://=0x1427,//int
                case CEnum.TagName.o2jam_ITEM_INDEX_ID://=0x1428,//int
                case CEnum.TagName.o2jam_USED_COUNT://=0x1429,//int

                case CEnum.TagName.o2jam_OLD_USED_COUNT://=0x1431,//int
                case CEnum.TagName.o2jam_CURRENT_CASH://=0x1433,//int
                case CEnum.TagName.o2jam_CHARGED_CASH://=0x1434,//int

                case CEnum.TagName.o2jam_KIND://=0x1438,//int
                case CEnum.TagName.o2jam_PLANET://=0x1439,//int
                case CEnum.TagName.o2jam_VAL://=0x1440,//int
                case CEnum.TagName.o2jam_EFFECT://=0x1441,//int
                case CEnum.TagName.o2jam_JUSTICE://=0x1442,//int
                case CEnum.TagName.o2jam_LIFE://=0x1443,//int
                case CEnum.TagName.o2jam_PRICE_KIND://=0x1444,//int
                case CEnum.TagName.o2jam_Exp://=0x1445,//Int
                case CEnum.TagName.o2jam_Battle://=0x1446,//Int
                case CEnum.TagName.o2jam_POSITION://=0x1448,//int
                case CEnum.TagName.o2jam_COMPANY_ID://=0x1449,//int

                case CEnum.TagName.o2jam_ITEM_USE_COUNT://=0x1454,//int
                case CEnum.TagName.o2jam_ITEM_ATTR_KIND://=0x1455,//int
                case CEnum.TagName.O2JAM_BuyType:// = 0x1509,//TLV_INTEGER

                case CEnum.TagName.O2JAM_EQUIP1://=0x1463,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP2://=0x1464,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP3://=0x1465,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP4://=0x1466,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP5://=0x1467,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP6://=0x1468,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP7://=0x1469,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP8://=0x1470,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP9://=0x1471,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP10://=0x1472,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP11://=0x1473,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP12://=0x1474,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP13://=0x1475,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP14://=0x1476,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP15://=0x1477,//TLV_STRING,
                case CEnum.TagName.O2JAM_EQUIP16://=0x1478,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG1://=0x1479,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG2://=0x1480,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG3://=0x1481,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG4://=0x1482,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG5://=0x1483,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG6://=0x1484,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG7://=0x1485,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG8://=0x1486,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG9://=0x1487,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG10://=0x1488,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG11://=0x1489,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG12://=0x1490,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG13://=0x1491,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG14://=0x1492,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG15://=0x1493,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG16://=0x1494,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG17://=0x1495,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG18://=0x1496,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG19://=0x1497,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG20://=0x1498,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG21://=0x1499,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG22://=0x1500,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG23://=0x1501,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG24://=0x1502,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG25://=0x1503,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG26://=0x1504,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG27://=0x1505,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG28://=0x1506,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG29://=0x1507,//TLV_STRING,
                case CEnum.TagName.O2JAM_BAG30://=0x1508,//TLV_STRING

                
		        case CEnum.TagName.O2JAM2_Rdate:// = 0x1606,//TLV_Date
                case CEnum.TagName.O2JAM2_IsUse:// = 0x1607,//TLV_Integer
                case CEnum.TagName.O2JAM2_Status:// = 0x1608,//TLV_Integer
                case CEnum.TagName.SysAdmin:

                case CEnum.TagName.SDO_SenceID://= 0x0858,
                case CEnum.TagName.SDO_WeekDay://= 0x0859,
                case CEnum.TagName.SDO_MatPtHR://= 0x0860,
                case CEnum.TagName.SDO_MatPtMin://= 0x0861,
                case CEnum.TagName.SDO_StPtHR://= 0x0862,
                case CEnum.TagName.SDO_StPtMin://= 0x0863,
                case CEnum.TagName.SDO_EdPtHR://= 0x0864,
                case CEnum.TagName.SDO_EdPtMin://= 0x0865,
                case CEnum.TagName.SDO_GCash://= 0x0866,
                case CEnum.TagName.SDO_MCash://= 0x0867,
                
                case CEnum.TagName.SDO_MusicID1://= 0x0869,

                case CEnum.TagName.SDO_LV1://= 0x0871,
                case CEnum.TagName.SDO_MusicID2://= 0x0872,

                case CEnum.TagName.SDO_LV2://= 0x0874,
                case CEnum.TagName.SDO_MusicID3://= 0x0875,

                case CEnum.TagName.SDO_LV3://= 0x0877,
                case CEnum.TagName.SDO_MusicID4://= 0x0878,

                case CEnum.TagName.SDO_LV4://= 0x0880,
                case CEnum.TagName.SDO_MusicID5://= 0x0881,

                case CEnum.TagName.SDO_LV5://= 0x0883,

                case CEnum.TagName.SDO_Precent:// = 0x0884,

                case CEnum.TagName.O2JAM2_UserIndexID://= 0x1609,//TLV_Integer
                case CEnum.TagName.O2JAM2_Level://= 0x1612,//Format:TLV_INTEGER �ȼ�
                case CEnum.TagName.O2JAM2_Win://= 0x1613,//Format:TLV_INTEGER ʤ
                case CEnum.TagName.O2JAM2_Draw://= 0x1614,//Format:TLV_INTEGER ƽ
                case CEnum.TagName.O2JAM2_Lose://= 0x1615,//Format:TLV_INTEGER ��
                case CEnum.TagName.O2JAM2_Exp://= 0x1616,//Format:TLV_INTEGER ����
                case CEnum.TagName.O2JAM2_TOTAL://= 0x1617,//Format:TLV_INTEGER �ܾ���
                case CEnum.TagName.O2JAM2_GCash://= 0x1618,//Format:TLV_INTEGER G��
                case CEnum.TagName.O2JAM2_MCash://= 0x1619,//Format:TLV_INTEGER M��
                case CEnum.TagName.O2JAM2_ItemCode://= 0x1620,//Format:TLV_INTEGER
                case CEnum.TagName.O2JAM2_Position://= 0x1625,//int
                case CEnum.TagName.O2JAM2_MoneyType://= 0x1628,//int
                case CEnum.TagName.O2JAM2_ItemSource://= 0x1624,//int
                case CEnum.TagName.O2JAM2_DayLimit:
                case CEnum.TagName.O2JAM2_StopStatus:// = 0x1635,//int

                case CEnum.TagName.SDO_wPlanetID:// = 0x0893, 
		        case CEnum.TagName.SDO_wChannelID:// = 0x0894,
	            case CEnum.TagName.SDO_iLimitUser:// = 0x0895, 
	            case CEnum.TagName.SDO_iCurrentUser:// = 0x0896,
                case CEnum.TagName.Soccer_charidx://��ɫ���к� (ExSoccer.dbo.t_character[idx])
		        case CEnum.TagName.Soccer_charexp: //����ֵ
                case CEnum.TagName.Soccer_charlevel://�ȼ�
                case CEnum.TagName.Soccer_charpoint://G���� 
                case CEnum.TagName.Soccer_match ://������
                case CEnum.TagName.Soccer_win://
                case CEnum.TagName.Soccer_lose://ʧ��
                case CEnum.TagName.Soccer_draw://ƽ��
                case CEnum.TagName.Soccer_drop://ƽ��		
                case CEnum.TagName.Soccer_charpos://λ��
                case CEnum.TagName.SDO_IsBattle:

                case CEnum.TagName.SDO_ItemCode1:
                case CEnum.TagName.SDO_ItemCode2:
                case CEnum.TagName.SDO_ItemCode3:
                case CEnum.TagName.SDO_ItemCode4:
                case CEnum.TagName.SDO_ItemCode5:



                case CEnum.TagName.RayCity_ConnectionLogIDX:
                case CEnum.TagName.RayCity_ConnectionLogKey:
                case CEnum.TagName.RayCity_CharacterMileage:
                case CEnum.TagName.RayCity_CarMileage:
                case CEnum.TagName.RayCity_CarIDX:
                case CEnum.TagName.RayCity_ItemBuySellLogIDX:
                case CEnum.TagName.RayCity_MissionLogIDX:
                case CEnum.TagName.RayCity_MoneyLogIDX:
                case CEnum.TagName.RayCity_RaceLogIDX:
                case CEnum.TagName.RayCity_CurMailCount:
                case CEnum.TagName.RayCity_TargetTradeSessionIDX:
                case CEnum.TagName.RayCity_CashItemLogIDX:
                case CEnum.TagName.FJ_Sex: //TLV_INTEGER
                case CEnum.TagName.FJ_BoardFlag://Format:INT ״̬



                #region ���߷ɳ�
                /// <summary>
                /// ���߷ɳ�
                /// </summary>
                //case CEnum.TagName.RC_ICurrentVehicleID:

                case CEnum.TagName.RC_Rank:
                case CEnum.TagName.RC_ID:
                case CEnum.TagName.RC_isAdult:
                case CEnum.TagName.RC_State:
                case CEnum.TagName.RC_Sex:
                case CEnum.TagName.RC_Vehicle:
                case CEnum.TagName.RC_Level:
                case CEnum.TagName.RC_MatchNum:
                case CEnum.TagName.RC_9YOUPointer:
                case CEnum.TagName.RC_Money:
                case CEnum.TagName.RC_PlayCounter:
                case CEnum.TagName.RC_UserIndexID:
                case CEnum.TagName.RayCity_StartNum:
                case CEnum.TagName.RayCity_EndNum:
                case CEnum.TagName.RayCity_TradeWaitItemIDX:
                case CEnum.TagName.RayCity_CarInventoryItemIDX:
                case CEnum.TagName.RayCity_TradeWaitCellNo:
                case CEnum.TagName.RayCity_TargetCarInventoryIDX:
                case CEnum.TagName.RayCity_TargetInventoryCellNo:
                case CEnum.TagName.RayCity_TradeWaitItemState:
                case CEnum.TagName.RayCity_TradeSessionIDX:
                case CEnum.TagName.RayCity_TargetCharacterID:
                case CEnum.TagName.RayCity_TradeMoney:
                case CEnum.TagName.RayCity_TradeSessionState:
                case CEnum.TagName.RayCity_SoloRaceWin:
                case CEnum.TagName.RayCity_SoloRaceLose:
                case CEnum.TagName.RayCity_SoloRaceRetire:
                case CEnum.TagName.RayCity_TeamRaceWin:
                case CEnum.TagName.RayCity_TeamRaceLose:
                case CEnum.TagName.RayCity_FieldSoloRaceWin:
                case CEnum.TagName.RayCity_FieldSoloRaceLose:
                case CEnum.TagName.RayCity_FieldSoloRaceRetire:
                case CEnum.TagName.RayCity_FieldTeamRaceWin:
                case CEnum.TagName.RayCity_FieldTeamRaceLose:
                case CEnum.TagName.RayCity_ItemIDX:
                case CEnum.TagName.RayCity_Bonding:
                case CEnum.TagName.RayCity_MaxEndurance:
                case CEnum.TagName.RayCity_CurEndurance:
                case CEnum.TagName.RayCity_ItemOption1:
                case CEnum.TagName.RayCity_ItemOption2:
                case CEnum.TagName.RayCity_ItemOption3:
                case CEnum.TagName.RayCity_ItemState:
                case CEnum.TagName.RayCity_ItemPrice:
                case CEnum.TagName.RayCity_BeforeCharacterMoney:
                case CEnum.TagName.RayCity_AfterCharacterMoney:
                case CEnum.TagName.RayCity_MoneyType:
                case CEnum.TagName.RayCity_ServerID:
                case CEnum.TagName.RayCity_ActionType:
                case CEnum.TagName.RayCity_NyGender:
                case CEnum.TagName.RayCity_CharacterID:
                case CEnum.TagName.RayCity_CarID:
                case CEnum.TagName.RayCity_CarType:
                case CEnum.TagName.RayCity_LastEquipInventoryIDX:
                case CEnum.TagName.RayCity_CarState:
                case CEnum.TagName.RayCity_AccountID:
                case CEnum.TagName.RayCity_RecentMailIDX:
                case CEnum.TagName.RayCity_RecentGiftIDX:
                case CEnum.TagName.RayCity_LastUseCarIDX:
                case CEnum.TagName.RayCity_GarageIDX:
                case CEnum.TagName.RayCity_LastTutorialID:
                case CEnum.TagName.RayCity_CharacterState:
                case CEnum.TagName.RayCity_FriendIDX:
                case CEnum.TagName.RayCity_FriendCharacterID:
                case CEnum.TagName.RayCity_FriendGroupIDX:
                case CEnum.TagName.RayCity_FriendState:
                case CEnum.TagName.RayCity_FriendGroupType:
                case CEnum.TagName.RayCity_FriendGroupState:
                case CEnum.TagName.RayCity_CarInventoryIDX:
                case CEnum.TagName.RayCity_InventoryType:
                case CEnum.TagName.RayCity_MaxInventorySize:
                case CEnum.TagName.RayCity_CurrentInventorySize:
                case CEnum.TagName.RayCity_QuestLogIDX:
                case CEnum.TagName.RayCity_QuestID:
                case CEnum.TagName.RayCity_QuestState:
                case CEnum.TagName.RayCity_DealLogIDX:
                case CEnum.TagName.RayCity_ItemID:
                case CEnum.TagName.RayCity_BuyDealClientID:
                case CEnum.TagName.RayCity_SellDealClientID:
                case CEnum.TagName.RayCity_BuyPrice:
                case CEnum.TagName.RayCity_SellPrice:
                case CEnum.TagName.RayCity_DealLogState:
                case CEnum.TagName.RayCity_QuestOJTLogIDX:
                case CEnum.TagName.RayCity_QuestOJTIDX:
                case CEnum.TagName.RayCity_TaskValue:
                case CEnum.TagName.RayCity_ExecuteCount:
                case CEnum.TagName.RayCity_QuestOJTState:
                case CEnum.TagName.RayCity_CharacterLevel:
                case CEnum.TagName.RayCity_CharacterExp:
                case CEnum.TagName.RayCity_CharacterMoney:
                case CEnum.TagName.RayCity_MaxCombo:
                case CEnum.TagName.RayCity_MaxSP:
                case CEnum.TagName.RayCity_MaxMailCount:
                case CEnum.TagName.RayCity_CurrentRP:
                case CEnum.TagName.RayCity_AccumulatedRP:
                case CEnum.TagName.RayCity_RelaxGauge:
                case CEnum.TagName.RayCity_StartPos_TownCode:
                case CEnum.TagName.RayCity_StartPos_FieldCode:
                case CEnum.TagName.RayCity_DealInventoryItemIDX:
                case CEnum.TagName.RayCity_InventoryCellNo:
                case CEnum.TagName.RayCity_DealInventoryItemState:
                case CEnum.TagName.RayCity_CarLevel:
                case CEnum.TagName.RayCity_CarExp:
                case CEnum.TagName.RayCity_FuelQuantity:
                case CEnum.TagName.RayCity_MissionID:
                case CEnum.TagName.RayCity_TotMissionCnt:
                case CEnum.TagName.RayCity_TotMissionSuccessCnt:
                case CEnum.TagName.RayCity_TotMissionFailCnt:
                case CEnum.TagName.RayCity_TotEXP:
                case CEnum.TagName.RayCity_TotCarEXP:
                case CEnum.TagName.RayCity_TotIncoming:
                case CEnum.TagName.RayCity_BingoCardIDX:
                case CEnum.TagName.RayCity_BingoCardID:
                case CEnum.TagName.RayCity_BingoRewardType:
                case CEnum.TagName.RayCity_BingoRewardValue:
                case CEnum.TagName.RayCity_BingoRewardAmount:
                case CEnum.TagName.RayCity_BingoCardState:
                case CEnum.TagName.RayCity_AccountState:
                case CEnum.TagName.RayCity_CharacterType:
                case CEnum.TagName.RayCity_TotPlayTime:
                case CEnum.TagName.RayCity_LoginCount:
                case CEnum.TagName.RayCity_IsLogin:
                case CEnum.TagName.RayCity_GuildMemberIDX:
                case CEnum.TagName.RayCity_GuildGroupIDX:
                case CEnum.TagName.RayCity_GuildMemberState:
                case CEnum.TagName.RayCity_GuildGroupRole:
                case CEnum.TagName.RayCity_GuildGroupState:
                case CEnum.TagName.RayCity_GuildIDX:
                case CEnum.TagName.RayCity_MaxGuildMember:
                case CEnum.TagName.RayCity_CurGuildMember:
                case CEnum.TagName.RayCity_IncreaseItemCount:
                case CEnum.TagName.RayCity_TrackRaceWin:
                case CEnum.TagName.RayCity_TrackRaceLose:
                case CEnum.TagName.RayCity_FieldRaceWin:
                case CEnum.TagName.RayCity_FieldRaceLose:
                case CEnum.TagName.RayCity_GuildState:
                case CEnum.TagName.RayCity_NyBirthYear:
                case CEnum.TagName.RayCity_RewardExp:
                case CEnum.TagName.RayCity_RewardCarExp:
                case CEnum.TagName.RayCity_RewardMoney:
                case CEnum.TagName.RayCity_MissionGrade:
                case CEnum.TagName.RayCity_MissionResult:
                case CEnum.TagName.RayCity_Duration:
                case CEnum.TagName.RayCity_AdjustType:
                case CEnum.TagName.RayCity_UpdateSource:
                case CEnum.TagName.RayCity_MoneyAmount:
                case CEnum.TagName.RayCity_RaceID:
                case CEnum.TagName.RayCity_RaceType:
                case CEnum.TagName.RayCity_TrackID:
                case CEnum.TagName.RayCity_RewardRP_RankBase:
                case CEnum.TagName.RayCity_RewardRP_TimeBase:
                case CEnum.TagName.RayCity_RaceResult:
                case CEnum.TagName.RayCity_Rank:
                case CEnum.TagName.RayCity_RaceTime:
                case CEnum.TagName.RayCity_ItemTypeID:
                case CEnum.TagName.RayCity_SkillID:
                case CEnum.TagName.RayCity_SkillLevel:
                case CEnum.TagName.RayCity_SkillIDX:
                case CEnum.TagName.RayCity_NyCashChargeLogIDX:
                case CEnum.TagName.RayCity_NyPayID:
                case CEnum.TagName.RayCity_ChargeAmount:
                case CEnum.TagName.RayCity_ChargeState:

                case CEnum.TagName.RayCity_CouponIDX:   //0x2200,//Format:Integer ������к�
                case CEnum.TagName.RayCity_IssueCount:
                case CEnum.TagName.RayCity_CouponState:
                case CEnum.TagName.RayCity_IssueCouponIDX:
                case CEnum.TagName.RC_SenderID:
                case CEnum.TagName.RC_ReceiverID:
                case CEnum.TagName.RC_SenderOperation:
                case CEnum.TagName.RC_ReceiverOperation:
                case CEnum.TagName.RayCity_StockID:
                case CEnum.TagName.RayCity_GiftState:
                case CEnum.TagName.RayCity_FromCharacterID:
                case CEnum.TagName.RayCity_PaymentType:
                //case CEnum.TagName.�����ȼ�:
                //case CEnum.TagName.�����������:
                //case CEnum.TagName.Һ�������ȼ�:
                case CEnum.TagName.RC_PlayerID:
                case CEnum.TagName.RC_PlayerOperation:


                case CEnum.TagName.RC_state_number:
                case CEnum.TagName.RC_ItemCode:
                case CEnum.TagName.RC_TimeLoop:
                case CEnum.TagName.RC_ChannelServerID:
                case CEnum.TagName.RC_MultipleType:
                case CEnum.TagName.RC_MultipleValue:
                case CEnum.TagName.RC_IGroup:

                case CEnum.TagName.RayCity_Interval:
                case CEnum.TagName.RayCity_NoticeID:
                case CEnum.TagName.RayCity_Repeat:

                case CEnum.TagName.RC_Split_IForce://TLV_INTEGER ��Ӫ
                case CEnum.TagName.RC_IRunDistance://TLV_INTEGER ����ʻ����
                case CEnum.TagName.RC_ILostConnection://TLV_INTEGER ���ߴ���
                case CEnum.TagName.RC_IEscapeCounter://TLV_INTEGER ���ܴ���
                case CEnum.TagName.RC_IWinCounter://TLV_INTEGER ��ʤ�Ĵ���
                case CEnum.TagName.RC_IGameMoney://TLV_INTEGER ��Ϸ��
                case CEnum.TagName.RC_AllOnlineTime://TLV_INTEGER ����ʱ��
                case CEnum.TagName.RayCity_NyCashBalance://��� INT
                #endregion

                #region SD�Ҵ�online
                case CEnum.TagName.f_idx://���ID int
                case CEnum.TagName.f_gender://�Ա� int 0Ů1��
                case CEnum.TagName.f_adult://�Ƿ���� 0����1δ����
                case CEnum.TagName.f_level://���� int
                case CEnum.TagName.f_Exp://���� int
                case CEnum.TagName.f_shootCnt://������ int
                case CEnum.TagName.f_collectCnt://�ռ��� int
                case CEnum.TagName.f_fightCnt://ս���� int
                case CEnum.TagName.f_winCnt://ʤ�� int
                case CEnum.TagName.f_loseCnt://ʧ�� int
                case CEnum.TagName.f_drawCnt://ƽ�� int
                case CEnum.TagName.SD_ban://��ͣ int
                case CEnum.TagName.SD_NeedExp://������Ҫ�ľ��� int
                case CEnum.TagName.SD_GC://G�� int
                case CEnum.TagName.SD_GP://M�� int
                case CEnum.TagName.SD_KillNum://��ɱ���� int

                case CEnum.TagName.SD_SlotNumber://���� int                 
                case CEnum.TagName.SD_ItemID://����ID int                  
                case CEnum.TagName.SD_RemainCount://ʣ������ int 
                case CEnum.TagName.SD_BuyType://�������� int



                case CEnum.TagName.SD_CustomLvMax://�������ϳɴ���
                case CEnum.TagName.SD_CustomPoint://����ϳɵ���

                case CEnum.TagName.SD_Limit://��� int
                case CEnum.TagName.SD_Type://���� int                
                case CEnum.TagName.SD_Number://���� int

                case CEnum.TagName.SD_FromIdx://�����û�id int
                case CEnum.TagName.SD_ToIdx://�����û�id int

                case CEnum.TagName.SD_ItemID1://����ID1 int                
                case CEnum.TagName.SD_ItemID2://����ID2 int                
                case CEnum.TagName.SD_ItemID3://����ID3 int
                case CEnum.TagName.SD_Number1://����1 int
                case CEnum.TagName.SD_Number2://����2 int
                case CEnum.TagName.SD_Number3://����3 int
                case CEnum.TagName.SD_N10://���﷢�ͺ�ʣ���point int                
                case CEnum.TagName.SD_N12://���㷽ʽ(0.point 1.cash) int
                case CEnum.TagName.SD_SendType://���͹������� int
                case CEnum.TagName.SD_Status://���͹���״̬ int
                case CEnum.TagName.SD_N11://ʹ�������ﹺ���ϵ�point int
                case CEnum.TagName.SD_QusetID://����ID int
                case CEnum.TagName.SD_UColor_1://��ɫ1 string
                case CEnum.TagName.SD_UColor_2://��ɫ2 string
                case CEnum.TagName.SD_UColor_3://��ɫ3 string
                case CEnum.TagName.SD_UColor_4://��ɫ4 string
                case CEnum.TagName.SD_UColor_5://��ɫ5 string
                case CEnum.TagName.SD_UColor_6://��ɫ6 string
                case CEnum.TagName.SD_RewardCount://������ int
                case CEnum.TagName.SD_Star://�����Ǽ�
                case CEnum.TagName.SD_Money_Old://�޸�ǰ��Ǯ int
               

                #endregion

                #region ������2
                /// <summary>
                /// ������2����
                /// </summary>


                case CEnum.TagName.JW2_State://���״̬ Format:Integer
                case CEnum.TagName.JW2_UserSN://Format:Integer �û����к�
                case CEnum.TagName.JW2_Sex://����Ա� Format:Integer
                case CEnum.TagName.JW2_AvatarItem: //Format:Integer ����ID
                case CEnum.TagName.JW2_Exp://Format:Integer ��Ҿ���
                case CEnum.TagName.JW2_Money: //Format:Integer ��Ǯ
                case CEnum.TagName.JW2_Cash: //Format:Integer �ֽ�
                case CEnum.TagName.JW2_Level: //Format:Integer �ȼ�
                case CEnum.TagName.JW2_UseItem://Format:Integer�Ƿ���ʹ���У�1��0����
                case CEnum.TagName.JW2_RemainCount://Format:Integerʣ�������0Ϊ���޴�
                case CEnum.TagName.JW2_TaskID://�����Format:Integer
                case CEnum.TagName.JW2_Status://�Ƿ����״̬Format:Integer
                case CEnum.TagName.JW2_Interval://���ʱ��Format:Integer
                case CEnum.TagName.JW2_POWER://Ȩ�ޣ���ͨ�û���0������ԱΪ1 Format:Integer
                case CEnum.TagName.JW2_GoldMedal://���� Format:Integer
                case CEnum.TagName.JW2_SilverMedal://���� Format:Integer
                case CEnum.TagName.JW2_CopperMedal://ͭ�� Format:Integer
                case CEnum.TagName.JW2_PARA://��������õ�һ������ Format:Integer
                case CEnum.TagName.JW2_ATONCE://�Ƿ���������Format:Integer
                case CEnum.TagName.JW2_BOARDSN://��С���ȣ������¼Ψһ��ʾFormat:Integer
                case CEnum.TagName.JW2_BUGLETYPE://����0mb��С����,1���ַ�С����,1mb��������,11���ַ�������,20��20�����Ǻ��Format:Integer
                //���///////////
                case CEnum.TagName.JW2_Chapter://�½� int
                case CEnum.TagName.JW2_CurStage://Ŀǰ�ȼ� int
                case CEnum.TagName.JW2_MaxStage://���ȼ� int
                case CEnum.TagName.JW2_Stage0://�ؿ�1 int
                case CEnum.TagName.JW2_Stage1:// int
                case CEnum.TagName.JW2_Stage2:// int
                case CEnum.TagName.JW2_Stage3://int
                case CEnum.TagName.JW2_Stage4:// int
                case CEnum.TagName.JW2_Stage5:// int
                case CEnum.TagName.JW2_Stage6:// int
                case CEnum.TagName.JW2_Stage7:// int
                case CEnum.TagName.JW2_Stage8: //int
                case CEnum.TagName.JW2_Stage9:// int
                case CEnum.TagName.JW2_Stage10:// int
                case CEnum.TagName.JW2_Stage11:// int
                case CEnum.TagName.JW2_Stage12: //int
                case CEnum.TagName.JW2_Stage13:// int
                case CEnum.TagName.JW2_Stage14: //int
                case CEnum.TagName.JW2_Stage15://int
                case CEnum.TagName.JW2_Stage16: //int
                case CEnum.TagName.JW2_Stage17:// int
                case CEnum.TagName.JW2_Stage18://int
                case CEnum.TagName.JW2_Stage19://�ؿ�20 int
                //���end///////////
                case CEnum.TagName.JW2_BUYSN://����SN int
                case CEnum.TagName.JW2_GOODSTYPE://�������� int
                case CEnum.TagName.JW2_RECESN://�����˵�SN�������ͬ�Ǳ��ˣ� int 
                case CEnum.TagName.JW2_GOODSINDEX://��Ʒ��� int
                case CEnum.TagName.JW2_RECEID://�����˵�ID�������ͬ�Ǳ��ˣ� int
                case CEnum.TagName.JW2_MALESN://����SN int
                case CEnum.TagName.JW2_FEMAILESN://Ů��SN int
                case CEnum.TagName.JW2_RINGLEVEL://��ָ�ȼ� int
                case CEnum.TagName.JW2_REDHEARTNUM://�������� int
                case CEnum.TagName.JW2_WEDDINGNO://������� int
                case CEnum.TagName.JW2_KISSNUM://kiss���� int
                case CEnum.TagName.JW2_BREAKSN://���SN int

                case CEnum.TagName.JW2_FAMILYID://����ID int
                case CEnum.TagName.JW2_DUTYID://ְҵ�� int
                case CEnum.TagName.JW2_COUPLESN://������� int 

                case CEnum.TagName.JW2_POINT://���� int
                case CEnum.TagName.JW2_LOGINTYPE://����0���룬1�ǳ� int
                case CEnum.TagName.JW2_AvatarItemType://��Ʒ���ͣ�ͷ���ȣ�int
                case CEnum.TagName.JW2_ItemPos://��Ʒλ��(0,����,1,��Ʒ��,2,������) int
                case CEnum.TagName.JW2_ItemNo://��Ʒ��� int

                case CEnum.TagName.JW2_FamilyLevel://�ȼ� int

                case CEnum.TagName.JW2_ItemCode://����ID int
                case CEnum.TagName.JW2_Productid://��ƷID int
                case CEnum.TagName.JW2_FamilyPoint://������� int
                case CEnum.TagName.JW2_PetSn://����ID int
                case CEnum.TagName.JW2_PetLevel://����ȼ� int
                case CEnum.TagName.JW2_PetExp://���ﾭ�� int 
                case CEnum.TagName.JW2_PetEvol://���׽׶� int
                case CEnum.TagName.JW2_MailSn://���к� int

                case CEnum.TagName.JW2_Num://���� int


                case CEnum.TagName.JW2_BeforeCash://����ǰ��� int 
                case CEnum.TagName.JW2_AfterCash://���Ѻ��� int


                case CEnum.TagName.JW2_Center_Avatarid://�м������ID,int

                case CEnum.TagName.JW2_Center_State://״̬ int(0װ����2δװ��)
                case CEnum.TagName.JW2_ZoneID://������ID int
                case CEnum.TagName.JW2_VailedDay://ʱ�ޣ�7�죬30�죬65535=���ޣ� int
                case CEnum.TagName.JW2_IntRo://״̬��1�Լ�����/0�������ͣ� int
                case CEnum.TagName.JW2_SubGameID://����ϷID int

                case CEnum.TagName.JW2_Forever://(1���ã�0������) int

                case CEnum.TagName.JW2_ReportSn://�ٱ�ID int
                case CEnum.TagName.JW2_ReporterSn://�ٱ���ID int 
                case CEnum.TagName.JW2_ReportType://�ٱ����� int 
                case CEnum.TagName.JW2_MissionID://����ID int



                case CEnum.TagName.jw2_goodsprice://����۸� int
                case CEnum.TagName.jw2_beforemoney://����ǰ��Ǯ�� int
                case CEnum.TagName.jw2_aftermoney://������Ǯ�� int

                case CEnum.TagName.jw2_serverno://GS��� int
                case CEnum.TagName.jw2_port://GS�˿�int

                case CEnum.TagName.jw2_MultiDays://������Ծ���� int
                case CEnum.TagName.jw2_TodayActivePoint://�����õĻ�Ծ�ȵ��� int

                case CEnum.TagName.jw2_Wedding_Home:


                case CEnum.TagName.jw2_frmLove://���ܶ� int
                case CEnum.TagName.jw2_petaggID://���ﵰID	int

                case CEnum.TagName.jw2_FirstFamilySN://���뷽����id int
                case CEnum.TagName.jw2_SecondFamilySN://�����뷽����id int 
                case CEnum.TagName.jw2_PetID:// ����ID	int 

                case CEnum.TagName.jw2_EggNum://���ﵰ���� int
                case CEnum.TagName.jw2_PetAggID_Small:// С���ﵰID	int 
                case CEnum.TagName.AU_UseNum://�������� int
                case CEnum.TagName.AU_BadgeID://����ID int 


                case CEnum.TagName.AU_famid://����ID int 

                case CEnum.TagName.Magic_Sex:
                case CEnum.TagName.Magic_GuildID:

                case CEnum.TagName.AU_UTP://��ѯ�û����� (char 1) [1: Send; 2: Recv]
                #endregion

                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                case CEnum.TagName.MJ_CharName_Prefix: // 0x0743,//��Ұ������
                case CEnum.TagName.SDO_ServerIP:  //0x0801,//Format:String ����IP
                case CEnum.TagName.SDO_Account:  //0x0803,//Format:String ��ҵ��ʺ�
                case CEnum.TagName.SDO_ProductName:  //0x0816,//Format:String ��Ʒ����
                case CEnum.TagName.SDO_ItemName:  //0x0818,//Format:String ��������
                case CEnum.TagName.SDO_AREANAME:  //0x0827,//Format:String ��������
                case CEnum.TagName.SDO_NickName:  //0x0836,//�س�
                case CEnum.TagName.SDO_Address:  //0x0813,//Format:Integer ��ַ
                case CEnum.TagName.SDO_Desc:    // = 0x0843,//��������
                case CEnum.TagName.SDO_City:
                case CEnum.TagName.SDO_SendUserID: //0x0849,//Format:String �������ʺ�
                case CEnum.TagName.SDO_ReceiveNick: //0x0850,//Format:String �������س�
                case CEnum.TagName.SDO_Title:
                case CEnum.TagName.SDO_Context:
                case CEnum.TagName.SDO_REASON:// = 0x0853,//Format:String ͣ������
                case CEnum.TagName.SDO_Email:
                ////////
                case CEnum.TagName.ServerInfo_IP:// = 0x0901,//Format tring ������IP
                case CEnum.TagName.ServerInfo_City:// = 0x0902,//Format tring ����
                case CEnum.TagName.ServerInfo_GameName:// = 0x0904,//Format tring ��Ϸ��
                ///////
                case CEnum.TagName.SP_Name: //0x0412,//Format:String �洢������
                case CEnum.TagName.Real_ACT: //0x0413,//Format:String ����������
                case CEnum.TagName.MJ_Reason://0x0745,//ͣ������
                case CEnum.TagName.MJ_ServerIP:
                ///
                case CEnum.TagName.UpdateFileName:// = 0x0112,//Format:String �ļ���
                case CEnum.TagName.UpdateFileVersion:// = 0x0113,//Format:String �ļ��汾
                case CEnum.TagName.UpdateFilePath:// = 0x0114,//Format:String �ļ�·��
                //////////
                case CEnum.TagName.CR_ServerIP:// = 0x1101,//������IP
                case CEnum.TagName.CR_ACCOUNT:// = 0x1102,//����ʺ� Format tring
                case CEnum.TagName.CR_Passord:// = 0x1103,//������� Format tring
                case CEnum.TagName.CR_NUMBER:// = 0x1104,//������ Format tring
                case CEnum.TagName.CR_ISUSE:// = 0x1105,//�Ƿ�ʹ��
                case CEnum.TagName.CR_ActiveIP:// = 0x1107,//���������IP Format:String
                case CEnum.TagName.CR_BoardContext:// = 0x1110,//�������� Format:String
                case CEnum.TagName.CR_BoardColor:// = 0x1111,//������ɫ Format:String
                case CEnum.TagName.CR_NickName:// = 0x1132://,//�س� Format:String
                case CEnum.TagName.CR_BoardContext1:// = 0x1135,//����1
                case CEnum.TagName.CR_BoardContext2:// = 0x1136,//����2
                case CEnum.TagName.CR_ChannelName:
                case CEnum.TagName.CR_UserName:// = 0x1636,//string  
                ///////
                case CEnum.TagName.AU_ACCOUNT://=0x1001://,//����ʺ�Format:String
                case CEnum.TagName.AU_UserNick://=0x1002://,//����س�Format:String
                case CEnum.TagName.AU_Reason://=0x1006://,//��ͣ����Format:String
                case CEnum.TagName.AU_ServerIP://=0x1008://,//��������Ϸ������Format:String
                case CEnum.TagName.AU_EquipState://=0x1011://,//Format:String
                case CEnum.TagName.AU_BuyNick://=0x1013://,//Format:String�����س�
                case CEnum.TagName.AU_SendNick://=0x1019://,//Format:String�����س�
                
                case CEnum.TagName.AU_RecvNick://=0x1021://,//Format:String�������س�
                case CEnum.TagName.AU_Memo://=0x1029://,//Format:String��ע
                case CEnum.TagName.AU_UserID://=0x1030://,//Format:String���ID
                case CEnum.TagName.AU_Password://=0x1040://,//Format:String����
                case CEnum.TagName.AU_UserName://=0x1041://,//Format:String�û���
                case CEnum.TagName.AU_UserGender://=0x1042://,//Format:String
                case CEnum.TagName.AU_UserRegion://=0x1044://,//Format:String
                case CEnum.TagName.AU_UserEMail://=0x1045://,//Format:String�û������ʼ�
                case CEnum.TagName.AU_ItemName:// = 0x1047,//������
                case CEnum.TagName.AU_ItemStyle:// = 0x1048,//��������
                case CEnum.TagName.AU_Demo:// = 0x1049,//���� 
                case CEnum.TagName.AU_BeginTime:// = 0x1050,//��ʼʱ��
                case CEnum.TagName.AU_EndTime:// = 0x1051,//����ʱ��
                case CEnum.TagName.AU_SendUserID:// = 0x1052,//�������ʺ�
                case CEnum.TagName.AU_RecvUserID:// = 0x1053,//�������ʺ� 
                case CEnum.TagName.AU_Sex://=0x1003://,//����Ա�Format:Integer
                case CEnum.TagName.AU_GSName:
                ///////
                ///////
                case CEnum.TagName.CARD_PDkey:// 0x1203
                case CEnum.TagName.CARD_PDCardType:// 0x1204��
                case CEnum.TagName.CARD_PDFrom:// 0x1205
                case CEnum.TagName.CARD_PDCardNO:// 0x1206
                case CEnum.TagName.CARD_PDCardPASS:// 0x1207
                case CEnum.TagName.CARD_PDaction:// 0x1209
                case CEnum.TagName.CARD_PDuserid:// 0x1210
                case CEnum.TagName.CARD_PDusername:// 0x1211
                case CEnum.TagName.CARD_PDgetusername:// 0x1213
                
                case CEnum.TagName.CARD_PDip:// 0x1215
                case CEnum.TagName.CARD_PDstatus:// 0x1216
                case CEnum.TagName.CARD_UDkey:// 0x1218 
                case CEnum.TagName.CARD_UDusedo:// 0x1219
                case CEnum.TagName.CARD_UDdirect:// 0x1220
                case CEnum.TagName.CARD_UDusername:// 0x1222 
                case CEnum.TagName.CARD_UDgetuserid:// 0x1223 //TLV_STRING
                case CEnum.TagName.CARD_UDgetusername:// 0x1224 //TLV_STRING
                case CEnum.TagName.CARD_UDtype:// 0x1226 //TLV_STRING
                case CEnum.TagName.CARD_UDtargetvalue:// 0x1227
                case CEnum.TagName.CARD_UDzone1:// 0x1228
                case CEnum.TagName.CARD_UDzone2:// 0x1229
                case CEnum.TagName.CARD_UDip:// 0x1231
                case CEnum.TagName.CARD_UDstatus:// 0x1232
                case CEnum.TagName.CARD_cardnum:// 0x1233
                case CEnum.TagName.CARD_cardpass:// 0x1234
                case CEnum.TagName.CARD_serial:// 0x1235
                case CEnum.TagName.CARD_draft:// 0x1236
                case CEnum.TagName.CARD_type1:// 0x1237
                case CEnum.TagName.CARD_type2:// 0x1238
                case CEnum.TagName.CARD_type3:// 0x1239
                case CEnum.TagName.CARD_type4:// 0x1240
                case CEnum.TagName.CARD_valid_date:// 0x1242
                case CEnum.TagName.CARD_use_status:// 0x1243
                case CEnum.TagName.CARD_cardsent:// 0x1244
                case CEnum.TagName.CARD_create_date:// 0x1245
                
                case CEnum.TagName.CARD_use_username:// 0x1247
                case CEnum.TagName.CARD_partner:// 0x1248
                case CEnum.TagName.CARD_skey:// 0x1249
                ////////
                case CEnum.TagName.CARD_id:// 0x1251 ,//TLV_STRING ��֮��ע�Ῠ��
                case CEnum.TagName.CARD_username:// 0x1252,//TLV_STRING ��֮��ע���û���
                case CEnum.TagName.CARD_nickname:// 0x1253,//TLV_STRING ��֮��ע���س�
                case CEnum.TagName.CARD_password:// 0x1254,//TLV_STRING ��֮��ע������
                case CEnum.TagName.CARD_sex:// 0x1255,//TLV_STRING ��֮��ע���Ա�
                case CEnum.TagName.CARD_securecode:// 0x1258,//TLV_STRING ��ȫ��
                case CEnum.TagName.CARD_vis:// 0x1259,//TLV_INTEGER
                case CEnum.TagName.CARD_realname:// 0x1263,//TLV_STRING ��ʵ����
                case CEnum.TagName.CARD_cardtype:// 0x1265,//TLV_STRING
                case CEnum.TagName.CARD_email:// 0x1267,//TLV_STRING �ʼ�
                case CEnum.TagName.CARD_occupation:// 0x1268,//TLV_STRING ְҵ
                case CEnum.TagName.CARD_education:// 0x1269,//TLV_STRING �����̶�
                case CEnum.TagName.CARD_marriage:// 0x1270,//TLV_STRING ���
                case CEnum.TagName.CARD_constellation:// 0x1271,//TLV_STRING ����
                case CEnum.TagName.CARD_shx:// 0x1272,//TLV_STRING ��Ф
                case CEnum.TagName.CARD_city:// 0x1273,//TLV_STRING ����
                case CEnum.TagName.CARD_address:// 0x1274,//TLV_STRING ��ϵ��ַ
                case CEnum.TagName.CARD_phone:// 0x1275,//TLV_STRING ��ϵ�绰
                case CEnum.TagName.CARD_qq:// 0x1276,//TLV_STRING QQ
                case CEnum.TagName.CARD_intro:// 0x1277,//TLV_STRING ����
                case CEnum.TagName.CARD_msn:// 0x1278,//TLV_STRING MSN
                case CEnum.TagName.CARD_mobilephone:// 0x1279,//TLV_STRING �ƶ��绰
                ///
                case CEnum.TagName.AuShop_bkey://=0x1303,//varchar(40)
                case CEnum.TagName.AuShop_pkey://=0x1304,//varchar(18)

                case CEnum.TagName.AuShop_username://=0x1306,//varchar(20)

                case CEnum.TagName.AuShop_getusername://=0x1308,//varchar(20)

                case CEnum.TagName.AuShop_pisgift://=0x1310,//enum('y','n')
                case CEnum.TagName.AuShop_islover://=0x1311,//enum('y','n')
                case CEnum.TagName.AuShop_ispresent://=0x1312,//enum('y','n')
                case CEnum.TagName.AuShop_isbuysong://=0x1313,//enum('y','n')
                case CEnum.TagName.AuShop_psex://=0x1315,//enum('all','m','f')

                case CEnum.TagName.AuShop_zone://=0x1322,//tinyint(2)
                case CEnum.TagName.AuShop_buyip://=0x1321,//varchar(15)
                case CEnum.TagName.AuShop_pname://=0x1326,//varchar(20)
                case CEnum.TagName.AuShop_pgift://=0x1328,//enum('y','n')
                case CEnum.TagName.AuShop_pgamecode://=0x1331,//varchar(200)
                case CEnum.TagName.AuShop_pnew://=0x1332,//enum('y','n')
                case CEnum.TagName.AuShop_phot://=0x1333,//enum('y','n')
                case CEnum.TagName.AuShop_pcheap://=0x1334,//enum('y','n')
                case CEnum.TagName.AuShop_pautoprice://=0x1339,//enum('y','n')
                case CEnum.TagName.AuShop_ptimeitem://=0x1344,//varchar(200)
                case CEnum.TagName.AuShop_pricedetail://=0x1345,//varchar(254)
                case CEnum.TagName.AuShop_pdesc://=0x1347,//text
                case CEnum.TagName.AuShop_pmark1://=0x1350,//enum('y','n')
                case CEnum.TagName.AuShop_pmark2://=0x1351,//enum('y','n')
                case CEnum.TagName.AuShop_pmark3://=0x1352,//enum('y','n')
                case CEnum.TagName.AuShop_pisuse://=0x1355,//enum('y','n')
                case CEnum.TagName.AuShop_ppic://=0x1356,//varchar(36)
                case CEnum.TagName.AuShop_ppic1://=0x1357,//varchar(36)
                case CEnum.TagName.CARD_price:// 0x1241
                //////
                case CEnum.TagName.o2jam_ServerIP://=0x1401,//Format:TLV_STRINGIP
                case CEnum.TagName.o2jam_UserID://=0x1402,//Format:TLV_STRING�û��ʺ�
                case CEnum.TagName.o2jam_UserNick://=0x1403,//Format:TLV_STRING�û��س�



                
                


                case CEnum.TagName.o2jam_USER_ID://=0x1457,//varchar
                case CEnum.TagName.o2jam_USER_NICKNAME://=0x1458,//varchar
                
                case CEnum.TagName.o2jam_SenderID://=0x1409,//varchar

                case CEnum.TagName.o2jam_SenderNickName://=0x1411,//varchar
                case CEnum.TagName.o2jam_ReceiverID://=0x1412,//varchar

                case CEnum.TagName.o2jam_ReceiverNickName://=0x1414,//varchar
                case CEnum.TagName.o2jam_Title://=0x1415,//varchar
                case CEnum.TagName.o2jam_Content://=0x1416,//varchar

                case CEnum.TagName.o2jam_ReadFlag://=0x1419,//char
                case CEnum.TagName.o2jam_TypeFlag://=0x1420,//char
                case CEnum.TagName.o2jam_KIND_CASH://=0x1435,//char
                case CEnum.TagName.o2jam_NAME://=0x1437,//varchar
                case CEnum.TagName.o2jam_DESCRIBE://=0x1450,//varchar

                case CEnum.TagName.o2jam_ITEM_NAME://=0x1453,//varchar

                case CEnum.TagName.AuShop_buytime://=0x1319,//int(10)

                case CEnum.TagName.Process_Reason:// = 0x060B,//Format tring

                case CEnum.TagName.O2JAM2_ServerIP:// = 0x1601,//TLV_STRING
		        case CEnum.TagName.O2JAM2_UserID:// = 0x1602,//TLV_STRING
                case CEnum.TagName.O2JAM2_UserName:// = 0x1603,//TLV_STRING
                case CEnum.TagName.O2JAM2_Id1:// = 0x1604,//TLV_Integer
                case CEnum.TagName.O2JAM2_Id2:// = 0x1605,//TLV_Integer
                case CEnum.TagName.o2jam2_consumetype:// = 0x1631
                
                ///////
                case CEnum.TagName.SDO_MusicName1:// = 0x0870,
                case CEnum.TagName.SDO_MusicName2:// = 0x0873,
                case CEnum.TagName.SDO_MusicName3:// = 0x0876,
                case CEnum.TagName.SDO_MusicName4:// = 0x0879,
                case CEnum.TagName.SDO_MusicName5:// = 0x0882,

                case CEnum.TagName.SDO_Sence://= 0x0868,
                case CEnum.TagName.SDO_KeyID:// = 0x0885,
		        case CEnum.TagName.SDO_KeyWord:// = 0x0886,
		        case CEnum.TagName.SDO_MasterID:// = 0x0887,
		        case CEnum.TagName.SDO_Master:// = 0x0888,
		        case CEnum.TagName.SDO_SlaverID:// = 0x0889,
                case CEnum.TagName.SDO_Slaver:// = 0x0890,


                case CEnum.TagName.O2JAM2_UserNick://= 0x1610,//Format:TLV_STRING �û��س�
                case CEnum.TagName.O2JAM2_ItemName://= 0x1621,//Format:TLV_String
                case CEnum.TagName.O2JAM2_Title://= 0x1629,
                case CEnum.TagName.O2JAM2_Context://= 0x1630,
                case CEnum.TagName.O2JAM2_REASON:// = 0x1636,//string
                case CEnum.TagName.SDO_ChannelList:
                case CEnum.TagName.SDO_BoardMessage:
                case CEnum.TagName.SDO_ipaddr:// = 0x0897,
                case CEnum.TagName.SDO_MAIN_CH:
                case CEnum.TagName.SDO_SUB_CH:// = 0x0897,

                case CEnum.TagName.SDO_FirstPadTime:
                case CEnum.TagName.Soccer_charname: //��ɫ��
               
                case CEnum.TagName.Soccer_Type: //��ѯ����
                case CEnum.TagName.Soccer_String: //��ѯֵ 
                case CEnum.TagName.Soccer_deleted_date ://ɾ������
		        case CEnum.TagName.Soccer_status ://״̬
                case CEnum.TagName.Soccer_ServerIP:
                case CEnum.TagName.Soccer_loginId: //Login ID
                case CEnum.TagName.Soccer_charsex:// 
                case CEnum.TagName.Soccer_admid:
                case CEnum.TagName.Soccer_regDate://���ע������ string 
		        case CEnum.TagName.Soccer_c_date://��ɫ�������� string 
                case CEnum.TagName.Soccer_kind:
                case CEnum.TagName.SDO_Lock_Status:
                case CEnum.TagName.SDO_State://TLV_STRING  ״̬
                case CEnum.TagName.SDO_ItemName1://TLV_STRING  ������1
		        case CEnum.TagName.SDO_ItemName2://TLV_STRING  ����ID2
		        case CEnum.TagName.SDO_ItemName3://TLV_STRING  ����ID3
                case CEnum.TagName.SDO_ItemName4://TLV_STRING  ����ID4
		        case CEnum.TagName.SDO_ItemName5://TLV_STRING  ����ID5
                case CEnum.TagName.SD_UserName_Old://�޸�ǰ�û��� string
                #region ���߷ɳ�
                case CEnum.TagName.RayCity_ServerIP:
                case CEnum.TagName.RayCity_GuildName:
                case CEnum.TagName.RayCity_GuildMessage:
                case CEnum.TagName.RayCity_GuildGroupName:
                case CEnum.TagName.RayCity_LastLoginIPPrv:
                case CEnum.TagName.RayCity_LastLoginIPPub:
                case CEnum.TagName.RayCity_NyPassword:
                case CEnum.TagName.RayCity_NyNickName:
                case CEnum.TagName.RayCity_FriendGroupName:
                case CEnum.TagName.RayCity_CarName:
                case CEnum.TagName.RayCity_CharacterName:
                case CEnum.TagName.RayCity_FriendCharacterName:
                case CEnum.TagName.RayCity_NyUserID:
                case CEnum.TagName.RayCity_StartPos_X:
                case CEnum.TagName.RayCity_StartPos_Y:
                case CEnum.TagName.RayCity_StartPos_Z:
                case CEnum.TagName.RayCity_IPAddress:
                case CEnum.TagName.RayCity_ItemName:
                case CEnum.TagName.RayCity_ItemTypeName:
                case CEnum.TagName.RayCity_Title:
                case CEnum.TagName.RayCity_Message:
                case CEnum.TagName.RayCity_ActionTypeName:
                case CEnum.TagName.RayCity_SkillName:
                case CEnum.TagName.RayCity_TargetName:
                case CEnum.TagName.RayCity_GiftMessage:
                case CEnum.TagName.RayCity_FromCharacterName:
                case CEnum.TagName.RayCity_QuestName:
                case CEnum.TagName.RayCity_CountryCode:
                case CEnum.TagName.RayCity_CouponNumber:
                case CEnum.TagName.RayCity_rccdkey:
                case CEnum.TagName.RayCity_getuser:
                case CEnum.TagName.RayCity_gettime:
                case CEnum.TagName.RayCity_use_state:
                case CEnum.TagName.RayCity_activename:

                case CEnum.TagName.FJ_descr:
                #endregion

                #region SD�Ҵ�online
                case CEnum.TagName.SD_GoldAccount:
                case CEnum.TagName.SD_IsUsed:
                case CEnum.TagName.SD_UserName:
                case CEnum.TagName.SD_UseDate:
                case CEnum.TagName.SD_ActiceCode:

                case CEnum.TagName.f_passWd://���� string
                case CEnum.TagName.f_pilot://�º� string
                case CEnum.TagName.f_levelname://�������� string

                case CEnum.TagName.SD_ItemName://������ string 

                case CEnum.TagName.SD_UnitName://������ string
                case CEnum.TagName.SD_UnitNickName://�����س� string
                case CEnum.TagName.SD_StateSaleIntention://�Ƿ�������� string
                case CEnum.TagName.SD_BatItemName://ս���������� string
                case CEnum.TagName.SD_OperatorNickname://�����س� string

                case CEnum.TagName.SD_ServerIP://������IP string
                case CEnum.TagName.SD_ServerName://���������� string                
                case CEnum.TagName.SD_Content://��ͣ���� string
                case CEnum.TagName.SD_Title://���� string

                case CEnum.TagName.SD_TmpPWD://��ʱ���� string
                case CEnum.TagName.SD_passWd://��ʵ���� string
                case CEnum.TagName.SD_TempPassWord://���� string

                case CEnum.TagName.SD_ItemName1://������1 string
                case CEnum.TagName.SD_ItemName2://������2 string
                case CEnum.TagName.SD_ItemName3://������3 string
                case CEnum.TagName.SD_FromUser://�����û� string
                case CEnum.TagName.SD_ToUser://�����û� string
                case CEnum.TagName.SD_Make://���� string
                case CEnum.TagName.SD_LastMoney://ʣ���Ǯ string
                case CEnum.TagName.SD_UseMoney://ɾ��ʱ�õ���Ǯ string
                case CEnum.TagName.SD_DeleteResult://ɾ��ԭ�� string
                case CEnum.TagName.SD_UnitType://�������� string
                case CEnum.TagName.SD_ShopType://�̵깺�������� string
                case CEnum.TagName.SD_LimitType://�����ڼ䵽����� string
                case CEnum.TagName.SD_ChangeBody://��װ��λ string
                case CEnum.TagName.SD_ChangeItem://��װ���� string
                case CEnum.TagName.SD_CombPic://�ϳ�ͼֽ string
                case CEnum.TagName.SD_CombItem://�ϳ��ز� string
                case CEnum.TagName.SD_Time://ʱ�� string
                case CEnum.TagName.SD_QuestionName://�������� string
                case CEnum.TagName.SD_Questionlevel://�����Ѷ� int

                case CEnum.TagName.SD_QuestionGetItem://������������ string
                case CEnum.TagName.SD_Msg://���� string 
                case CEnum.TagName.SD_FirendName://���� string
                case CEnum.TagName.SD_GetMoney://�õ���Ǯ string
                case CEnum.TagName.SD_State://״̬ int
                case CEnum.TagName.SD_QusetName://�������� string
                case CEnum.TagName.SD_ClearedDate://���ʱ�� string 
                case CEnum.TagName.SD_UnitLevelNumber://����ȼ� int
                case CEnum.TagName.SD_UnitExp://���徭�� int  
                case CEnum.TagName.SD_HP://string
                case CEnum.TagName.SD_DashLevel://string
                case CEnum.TagName.SD_StrikingPower://string
                case CEnum.TagName.SD_FatalAttack://string
                case CEnum.TagName.SD_DefensivePower://string
                case CEnum.TagName.SD_MotivePower://string


                case CEnum.TagName.SD_SkillName://���� string

                case CEnum.TagName.SD_UDecal_1://��ǩ1 string
                case CEnum.TagName.SD_UDecal_2://��ǩ2 string
                case CEnum.TagName.SD_UDecal_3://��ǩ3 string
                case CEnum.TagName.SD_UnitLevelName://����ȼ�
                case CEnum.TagName.SD_TypeName://����
            
                case CEnum.TagName.SD_LostCodeMoney://ʣ����� string
                case CEnum.TagName.SD_UseCodeMoney://ʹ�ô��� string
                case CEnum.TagName.SD_LostMoney://���ĵ�G��
                case CEnum.TagName.SD_GuildName://�������� guildName
                case CEnum.TagName.SD_GuildBass://������� string
                case CEnum.TagName.SD_Oldvalue://�۳�ǰ��� string
                case CEnum.TagName.SD_Newvalue://�۳����� string 
                #endregion

                #region ������2
                case CEnum.TagName.JW2_ACCOUNT://����ʺ� Format:String

                case CEnum.TagName.JW2_ServerName://����������
                case CEnum.TagName.JW2_ServerIP://��Ϸ������ Format:String
                case CEnum.TagName.JW2_Reason://��ͣ���� Format:String
                case CEnum.TagName.JW2_GSServerIP://���� Format:String
                case CEnum.TagName.JW2_UserID: //Format:String ���ID
                case CEnum.TagName.JW2_BoardMessage://��������,���ȷ�������Format:String
                case CEnum.TagName.JW2_Region://���� Format:String
                case CEnum.TagName.JW2_QQ://QQ�� Format:String
                case CEnum.TagName.JW2_AvatarItemName://�������� Format:String
                case CEnum.TagName.JW2_MALEUSERNAME://�����û��� string
                case CEnum.TagName.JW2_MALEUSERNICK://�����ǳ� string 
                case CEnum.TagName.JW2_FEMALEUSERNAME://Ů���û��� string
                case CEnum.TagName.JW2_FEMAILEUSERNICK://Ů���ǳ� string
                case CEnum.TagName.JW2_WEDDINGNAME://�������� string
                case CEnum.TagName.JW2_WEDDINGVOW://��������string
                case CEnum.TagName.JW2_CONFIRMSN://��֤��SN string
                case CEnum.TagName.JW2_CONFIRMUSERNAME://��֤���û��� string
                case CEnum.TagName.JW2_CONFIRMUSERNICK://��֤���ǳ� string
                case CEnum.TagName.JW2_BREAKUSERNAME://����û��� string
                case CEnum.TagName.JW2_BREAKUSERNICK://����ǳ� string
                case CEnum.TagName.JW2_FAMILYNAME://���� string
                case CEnum.TagName.JW2_DUTYNAME://ְҵ�� string
                case CEnum.TagName.JW2_IP://IP��ַ string
                case CEnum.TagName.JW2_PWD://���� string 
                case CEnum.TagName.JW2_MailTitle://�ʼ����� string 
                case CEnum.TagName.JW2_MailContent://�ʼ����� string 
                case CEnum.TagName.JW2_Repute://���� string
                case CEnum.TagName.JW2_ItemName://�������� string
                case CEnum.TagName.JW2_ProductName://��Ʒ���� string
                case CEnum.TagName.JW2_PetName://�������� string
                case CEnum.TagName.JW2_PetNick://�����Զ������� string
                case CEnum.TagName.JW2_SendNick://�������ǳ� string 
                case CEnum.TagName.JW2_ShaikhNick://�峤���� string
                case CEnum.TagName.JW2_SubFamilyName://������������ string

                case CEnum.TagName.JW2_LASTLOGINDATE://����½ʱ�� date
                case CEnum.TagName.JW2_REGISTDATE://����ʱ�� date
                case CEnum.TagName.JW2_COUPLEDATE://�������� date
                case CEnum.TagName.JW2_WEDDINGDATE://���ʱ�� date
                case CEnum.TagName.JW2_BREAKDATE://����ʱ�� date
                case CEnum.TagName.JW2_CMBREAKDATE://ȷ�Ϸ���ʱ�� date
                case CEnum.TagName.JW2_LASTKISSDATE://���һ��Kissʱ�� date
                case CEnum.TagName.JW2_TIME://ʱ�� date
                case CEnum.TagName.JW2_ATTENDTIME://����ʱ�� date
                case CEnum.TagName.JW2_SendDate://�������� date
                case CEnum.TagName.JW2_ExpireDate://Format:TimesStamp  ��������

                case CEnum.TagName.JW2_FCMSTATUS://������״̬ int
                case CEnum.TagName.JW2_NowTitle://�ƺ� int


                case CEnum.TagName.JW2_Rate://��Ů���� int
                case CEnum.TagName.JW2_CNT://���� int

                case CEnum.TagName.JW2_sNum://����STRING


                case CEnum.TagName.JW2_AttendRank://�������� int
                case CEnum.TagName.JW2_MoneyRank://�Ƹ����� int
                case CEnum.TagName.JW2_PowerRank://ʵ������ int
                case CEnum.TagName.JW2_MessengerName://Messenger�ƺ�
                case CEnum.TagName.JW2_TmpPWD://��ʱ���� string 
                case CEnum.TagName.JW2_Type://���� string
                case CEnum.TagName.JW2_SendUser://�����û� string 
                case CEnum.TagName.JW2_RecUser://�����û� string

                case CEnum.TagName.JW2_Center_AvatarName://�м�������� string
                case CEnum.TagName.JW2_LOGDATE://ʱ�� date
                case CEnum.TagName.JW2_Last_Op_Time://���ʹ��ʱ�� date

                case CEnum.TagName.JW2_Center_EndTime://�^�ڕr�g date

                case CEnum.TagName.JW2_Family_Money://���׽�Ǯ string 
                case CEnum.TagName.JW2_ReporterNick://�ٱ����ǳ� string
                case CEnum.TagName.JW2_ReportedNick://���ٱ����ǳ� string
                case CEnum.TagName.JW2_Memo://�ٱ�˵�� string 

                case CEnum.TagName.JW2_OLD_FAMILYNAME://�ϼ����� string
                case CEnum.TagName.JW2_OLD_PETNAME://�ϳ����Զ�����
                case CEnum.TagName.JW2_BuyLimitDay://�������� string
                case CEnum.TagName.JW2_BuyMoneyCost://��ʵ�۸� string
                case CEnum.TagName.JW2_BuyOrgCost://ԭʼ�۸� string 

                case CEnum.TagName.JW2_MissionName://�������� string
                case CEnum.TagName.jw2_MaterialCode://�ϳɲ��� string
                case CEnum.TagName.jw2_activepoint://��Ծ�� int

                case CEnum.TagName.TIMEBegin:
                case CEnum.TagName.TIMEEend:

                case CEnum.TagName.jw2_pic_Name://ͼƬ���� string 


                case CEnum.TagName.jw2_petaggName://���ﵰ�� string

                case CEnum.TagName.jw2_petGetTime://����ʱ��  string

                case CEnum.TagName.jw2_getTime://�һ�ʱ��	 string

                case CEnum.TagName.jw2_FirstFamilyName://���뷽������ string
                case CEnum.TagName.jw2_SecondFamilyName://�����뷽������ string
                case CEnum.TagName.jw2_FirstFamilyMaster://���뷽�����峤�� string
                case CEnum.TagName.jw2_SecondFamilyMaster://�����뷽�峤�� string
                case CEnum.TagName.jw2_FirstFamilyUseMoneyr://���뷽���Ļ��� string
                case CEnum.TagName.jw2_SecondFamilyUseMoneyr://�����뷽���Ļ��� string
                case CEnum.TagName.jw2_ApplyDate:// ����ʱ��  string
                case CEnum.TagName.jw2_grade:
                case CEnum.TagName.jw2_ApplyState:// ����״̬  string


                case CEnum.TagName.jw2_PetAggName_Small:// С���ﵰ����	string 
                case CEnum.TagName.jw2_OverTimes://�һ�ʱ�� string 

                case CEnum.TagName.jw2_useLove://�������ܶ� string


                case CEnum.TagName.AU_GSServerIP:
                case CEnum.TagName.AuShop_ItemTable:
                case CEnum.TagName.AU_BroadMsg://�������� string

                case CEnum.TagName.Magic_category://��־����
                case CEnum.TagName.Magic_action://��־С��
                case CEnum.TagName.Magic_Dates://����
             
                #endregion

                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = tTlv.toString();
                
                    break;

                case CEnum.TagName.JW2_UserNick://����س� Format:String
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_UNICODE;
                    tBody[iRow, iField].oContent = tTlv.toUnicode();
                    break;


                case CEnum.TagName.SDO_UserIndexID:  //0x0802,//Format:Integer ����û�ID
                case CEnum.TagName.SDO_Level:  //0x0804,//Format:Integer ��ҵĵȼ�
                case CEnum.TagName.SDO_Exp:  //0x0805,//Format:Integer ��ҵĵ�ǰ����ֵ
                case CEnum.TagName.SDO_GameTotal:  //0x0806,//Format:Integer �ܾ���
                case CEnum.TagName.SDO_GameWin:  //0x0807,//Format:Integer ʤ����
                case CEnum.TagName.SDO_DogFall:  //0x0808,//Format:Integer ƽ����
                case CEnum.TagName.SDO_GameFall:  //0x0809,//Format:Integer ������
                case CEnum.TagName.SDO_Reputation:  //0x0810,//Format:Integer ����ֵ

                case CEnum.TagName.SDO_Age:  //0x0814,//Format:Integer ����
                case CEnum.TagName.SDO_ProductID:  //0x0815,//Format:Integer ��Ʒ���
                case CEnum.TagName.SDO_ItemCode:  //0x0817,//Format:Integer ���߱��
                case CEnum.TagName.SDO_MoneyType:  //0x0819,//Format:Integer ��������
                case CEnum.TagName.SDO_MoneyCost:  //0x0820,//Format:Integer ���ߵļ۸�
                case CEnum.TagName.SDO_MAINCH:  //0x0822,//Format:Integer ������
                case CEnum.TagName.SDO_SUBCH:  //0x0823,//Format:Integer ����
                case CEnum.TagName.SDO_Online:  //0x0824,//Format:Integer �Ƿ�����
                case CEnum.TagName.SDO_ActiveStatus:
                case CEnum.TagName.SDO_SEX:  //0x0838,//�Ա�
                case CEnum.TagName.SDO_Ispad:  //0x0842,//�Ƿ���ע������̺
                case CEnum.TagName.SDO_Postion:// = 0x0844,//����λ��
                case CEnum.TagName.SDO_MinLevel:
                case CEnum.TagName.SDO_TimesLimit:
                case CEnum.TagName.SDO_SendIndexID: //0x0848,//Format:Integer �����˵�ID
                case CEnum.TagName.SDO_BigType: //0x0851,//Format:Integer ���ߴ���
                case CEnum.TagName.SDO_SmallType: // 0x0852,//Format:Integer ����С��
                case CEnum.TagName.SDO_StopStatus:
                case CEnum.TagName.SDO_DaysLimit:// = 0x0855,//Format:Integer ʹ������
                case CEnum.TagName.SDO_ChargeSum:
                case CEnum.TagName.SDO_Interval:
                case CEnum.TagName.SDO_TaskID:
                case CEnum.TagName.SDO_Status:
                case CEnum.TagName.SDO_BanDate:
                ///
                case CEnum.TagName.ServerInfo_GameID:// = 0x0903,//Format:Integer ��ϷID
                case CEnum.TagName.ServerInfo_GameDBID:// = 0x0905,//Format:Integer ��Ϸ���ݿ�����
                case CEnum.TagName.ServerInfo_GameFlag:// = 0x0906,//Format:Integer ��Ϸ������״̬
                case CEnum.TagName.ServerInfo_Idx:// = 0x0907,//format:Integer
                ///
                case CEnum.TagName.OnlineActive:
                /////
                
                case CEnum.TagName.CR_STATUS:// = 0x1106,//���״̬ Format:Integer
                case CEnum.TagName.CR_BoardID:// = 0x1109,//����ID Format:Integer
                case CEnum.TagName.CR_Valid:// = 0x1114,//�Ƿ���Ч Format:Integer
                case CEnum.TagName.CR_PublishID:// = 0x1115,//������ID Format:Integer
                case CEnum.TagName.CR_DayLoop:// = 0x1116,//ÿ�첥�� Format:Integer
                case CEnum.TagName.CR_Mode://= 0x1130,//���ŷ�ʽ Format:Integer
                case CEnum.TagName.CR_ACTION:// = 0x1131,//��ѯ������Format:Integer
                case CEnum.TagName.CR_UserID:// = 0x1134,//�û�ID
                case CEnum.TagName.CR_Last_Playing_Time:
                case CEnum.TagName.CR_Total_Time:
                ///
                
                case CEnum.TagName.AU_State://=0x1004://,//���״̬Format:Integer
                case CEnum.TagName.AU_STOPSTATUS://=0x1005://,//�����߷�ͣ״̬Format:Integer
                case CEnum.TagName.AU_Id9you://=0x1009://,//Format:Integer9youID
                case CEnum.TagName.AU_UserSN://=0x1010://,//Format:Integer�û����к�
                case CEnum.TagName.AU_AvatarItem://=0x1012://,//Format:Integer
                case CEnum.TagName.AU_BuyType://=0x1016://,//Format:Integer��������
                case CEnum.TagName.AU_PresentID://=0x1017://,//Format:Integer����ID
                case CEnum.TagName.AU_SendSN://=0x1018://,//Format:Integer����SN
                case CEnum.TagName.AU_RecvSN://=0x1020://,//Format:String������SN
                case CEnum.TagName.AU_Kind://=0x1022://,//Format:Integer����
                case CEnum.TagName.AU_ItemID://=0x1023://,//Format:Integer����ID
                case CEnum.TagName.AU_Period://=0x1024://,//Format:Integer�ڼ�
                case CEnum.TagName.AU_BeforeCash://=0x1025://,//Format:Integer����֮ǰ���
                case CEnum.TagName.AU_AfterCash://=0x1026://,//Format:Integer����֮����
                case CEnum.TagName.AU_Exp://=0x1031://,//Format:Integer��Ҿ���
                case CEnum.TagName.AU_Point://=0x1032://,//Format:Integer���λ��
                case CEnum.TagName.AU_Money://=0x1033://,//Format:Integer��Ǯ
                case CEnum.TagName.AU_Cash://=0x1034://,//Format:Integer�ֽ�
                case CEnum.TagName.AU_Level://=0x1035://,//Format:Integer�ȼ�
                case CEnum.TagName.AU_Ranking://=0x1036://,//Format:Integer����
                case CEnum.TagName.AU_IsAllowMsg://=0x1037://,//Format:Integer������Ϣ
                case CEnum.TagName.AU_IsAllowInvite://=0x1038://,//Format:Integer��������
                case CEnum.TagName.AU_UserPower://=0x1043://,//Format:Integer
                case CEnum.TagName.AU_SexIndex:// = 0x1054,//�Ա�
                ///
                case CEnum.TagName.CARD_PDID:// 0x1202
                case CEnum.TagName.CARD_PDgetuserid:// 0x1212
                case CEnum.TagName.CARD_UDID:// 0x1217 //TLV_INTEGER
                case CEnum.TagName.CARD_UDuserid:// 0x1221 //TLV_INTEGER
                case CEnum.TagName.CARD_UDcoins:// 0x1225 //TLV_INTEGER
                ///
                case CEnum.TagName.AuShop_orderid://=0x1301,//int(11)
                case CEnum.TagName.AuShop_udmark://=0x1302,//int(8)
                case CEnum.TagName.AuShop_userid://=0x1305,//int(11)
                case CEnum.TagName.AuShop_getuserid://=0x1307,//int(11)
                case CEnum.TagName.AuShop_pcategory://=0x1309,//smallint(4)
                case CEnum.TagName.AuShop_prule://=0x1314,//tinyint(1)
                case CEnum.TagName.AuShop_pbuytimes://=0x1316,//int(11)
                case CEnum.TagName.AuShop_allprice://=0x1317,//int(11)
                case CEnum.TagName.AuShop_allaup://=0x1318,//int(11)
                
                case CEnum.TagName.AuShop_status://=0x1323,//tinyint(1)
                case CEnum.TagName.AuShop_pid://=0x1324,//int(11)
                case CEnum.TagName.AuShop_pscash://=0x1330,//tinyint(2)
                case CEnum.TagName.AuShop_pchstarttime://=0x1335,//int(10)
                case CEnum.TagName.AuShop_pchstoptime://=0x1336,//int(10)
                case CEnum.TagName.AuShop_pstorage://=0x1337,//smallint(5)
                case CEnum.TagName.AuShop_price://=0x1340,//int(8)
                case CEnum.TagName.AuShop_chprice://=0x1341,//int(8)
                case CEnum.TagName.AuShop_aup://=0x1342,//int(8)
                case CEnum.TagName.AuShop_chaup://=0x1343,//int(8)
                case CEnum.TagName.AuShop_pbuys://=0x1348,//int(8)
                case CEnum.TagName.AuShop_pfocus://=0x1349,//tinyint(1)
                case CEnum.TagName.AuShop_pdate://=0x1354,//int(10)
                
                case CEnum.TagName.AuShop_usefeesum://=0x1358,//int
                case CEnum.TagName.AuShop_useaupsum://=0x1359,//int
                case CEnum.TagName.AuShop_buyitemsum://=0x1360,//int

                case CEnum.TagName.o2jam_Level://=0x1405,//Format:TLV_STRING�ȼ�

                case CEnum.TagName.o2jam_Sex://=0x1404,//Format:TLV_INTEGER�Ա�

                case CEnum.TagName.o2jam_Win://=0x1406,//Format:TLV_STRINGʤ
                case CEnum.TagName.o2jam_Draw://=0x1407,//Format:TLV_STRINGƽ
                case CEnum.TagName.o2jam_Lose://=0x1408,//Format:TLV_STRING��
                case CEnum.TagName.o2jam_SEX://=0x1459,//char
                case CEnum.TagName.o2jam_WriteDate://=0x1417,//datetime
                case CEnum.TagName.O2JAM2_ComsumeCode:// = 0x1632,
                case CEnum.TagName.O2JAM2_Timeslimt://= 0x1622,
                case CEnum.TagName.Soccer_m_id://������к� int
		        case CEnum.TagName.Soccer_m_auth://����Ƿ�ͣ�� int
                case CEnum.TagName.Soccer_char_max:
                case CEnum.TagName.Soccer_char_cnt:
                case CEnum.TagName.Soccer_ret:
                case CEnum.TagName.SDO_LevPercent:
                case CEnum.TagName.SDO_ItemCodeBy:
                case CEnum.TagName.SDO_mood://TLV_INTEGER ����ֵ
                case CEnum.TagName.SDO_Food://TLV_INTEGER ��ʳ��
                case CEnum.TagName.SDO_DateLimit1://TLV_INTEGER ��������
		        case CEnum.TagName.SDO_TimeLimit1://TLV_INTEGER ��������
                case CEnum.TagName.SDO_DateLimit2://TLV_INTEGER ��������
		        case CEnum.TagName.SDO_TimeLimit2://TLV_INTEGER ��������
                case CEnum.TagName.SDO_DateLimit3://TLV_INTEGER ��������
		        case CEnum.TagName.SDO_TimeLimit3://TLV_INTEGER ��������
                case CEnum.TagName.SDO_DateLimit4://TLV_INTEGER ��������
		        case CEnum.TagName.SDO_TimeLimit4://TLV_INTEGER ��������
	            case CEnum.TagName.SDO_DateLimit5://TLV_INTEGER ��������
		        case CEnum.TagName.SDO_TimeLimit5://TLV_INTEGER ��������
                
                case CEnum.TagName.SDO_PreValue://��Сֵ
                case CEnum.TagName.SDO_EndValue://���ֵ
                case CEnum.TagName.SDO_NorProFirst://��һ�δ򿪵ĸ���
                case CEnum.TagName.SDO_NorPro://��ͨ����ĸ���
                case CEnum.TagName.SDO_SpePro://���ⱦ��ĸ���
                case CEnum.TagName.SDO_baoxiangid://id int
                case CEnum.TagName.SDO_Mark://��ʶ int
                case CEnum.TagName.SD_ID://id INT

                case CEnum.TagName.LORD_Gold://���
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_INTEGER;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;



                case CEnum.TagName.MJ_Time: // = 0x0741,//Format:TimeStamp  ����ʱ��
                case CEnum.TagName.SD_QuestionTime://�������ʱ�� string

                case CEnum.TagName.jw2_LastGetPointDate:

                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_DATE;
                    tBody[iRow, iField].oContent = tTlv.toDate();
                    break;
                
                case CEnum.TagName.SDO_DateLimit:
                case CEnum.TagName.SDO_LoginTime:  //0x0825,//Format:DateTime ����ʱ��
                case CEnum.TagName.SDO_LogoutTime:  //0x0826,//Format:DateTime ����ʱ��
                case CEnum.TagName.SDO_ShopTime:  //0x0821,//Format:DateTime ����ʱ��
                case CEnum.TagName.SDO_StopTime:// = 0x0854,//Format:TimeStamp ͣ��ʱ��
                ///////
                case CEnum.TagName.ACT_Time: //0x0414,//Format:TimeStamp ����ʱ��
                ///
                case CEnum.TagName.CR_ActiveDate:// = 0x1108,//�������� Format:TimeStamp        
                case CEnum.TagName.CR_ValidTime:// = 0x1112,//��Чʱ�� Format:TimeStamp
                case CEnum.TagName.CR_InValidTime:// = 0x1113,//ʧЧʱ�� Format:TimeStamp
                ///
                case CEnum.TagName.AU_BanDate://=0x1007://,//��ͣ����Format:TimeStamp
                case CEnum.TagName.AU_BuyDate://=0x1014://,//Format:Timestamp��������
                case CEnum.TagName.AU_ExpireDate://=0x1015://,//Format:TimesStamp��������
                case CEnum.TagName.AU_SendDate://=0x1027://,//Format:TimeStamp��������
                case CEnum.TagName.AU_RecvDate://=0x1028://,//Format:TimeStamp��������
                case CEnum.TagName.AU_LastLoginTime://=0x1039://,//Format:TimeStamp����¼ʱ��
                case CEnum.TagName.AU_RegistedTime://=0x1046://,//Format:TimeStampע��ʱ��
                case CEnum.TagName.AuShop_buytime2://=0x1320,//datetime
                ///
                case CEnum.TagName.CARD_UDdate:// 0x1230 timestamp
                case CEnum.TagName.AuShop_pinttime://=0x1353,//int(10)
                case CEnum.TagName.o2jam_ReadDate://=0x1418,//datetime
                case CEnum.TagName.o2jam_Ban_Date://=0x1421,//datetime
                case CEnum.TagName.o2jam_REG_DATE://=0x1430,//datetime
                case CEnum.TagName.o2jam_CREATE_TIME://=0x1460,//datetime
                case CEnum.TagName.o2jam_UPDATE_TIME://=0x1451,//datetime
                case CEnum.TagName.o2jam_BeginDate:// = 0x1461,
                case CEnum.TagName.o2jam_EndDate:// = 0x1462,
                case CEnum.TagName.CARD_PDdate:// 0x1214

                case CEnum.TagName.BeginTime:// = 0x0415,//Format:Date ��ʼ����
                case CEnum.TagName.EndTime:// = 0x0416,//Format:Date ��������
                case CEnum.TagName.SDO_RegistDate:  //0x0839,//ע������
                case CEnum.TagName.SDO_FirstLogintime:  //0x0840,//��һ�ε�¼ʱ��
                case CEnum.TagName.SDO_LastLogintime:  //0x0841,//���һ�ε�¼ʱ��
                case CEnum.TagName.SDO_BeginTime: //0x0845,//Format:Date ���Ѽ�¼��ʼʱ��
                case CEnum.TagName.SDO_EndTime: //0x0846,//Format:Date ���Ѽ�¼����ʱ��
                case CEnum.TagName.SDO_SendTime: //0x0847,//Format:Date ������������
                ///
                case CEnum.TagName.CARD_rdate:// 0x1256,//TLV_Date ��֮��ע������
                case CEnum.TagName.CARD_birthday:// 0x1264,//TLV_Date ��������
                ///
                case CEnum.TagName.AuShop_BeginDate://=0x1361,//date
                case CEnum.TagName.AuShop_EndDate://=0x1362,//
                ///
                ///
                
                case CEnum.TagName.O2JAM2_DateLimit://= 0x1623,
                case CEnum.TagName.O2JAM2_BeginDate://= 0x1626,
                case CEnum.TagName.O2JAM2_ENDDate://= 0x1627,
                case CEnum.TagName.O2JAM2_StopTime:// = 0x1634,//date
                case CEnum.TagName.CR_Last_Login:// = 0x1634,//date
                case CEnum.TagName.CR_Last_Logout:// = 0x1634,//date

                #region ���߷ɳ�
                /// <summary>
                /// ���߷ɳ�
                /// </summary>
                case CEnum.TagName.RC_OnlineTime:
                case CEnum.TagName.RC_OfflineTime:
                case CEnum.TagName.RC_BeginDate:
                case CEnum.TagName.RC_EndDate:
                case CEnum.TagName.RC_BanDate:
                case CEnum.TagName.RC_Sys_timeLastLogout://TLV_TimeStamp ���һ���˳�ʱ��
                case CEnum.TagName.RC_CreatDate:
                case CEnum.TagName.RayCity_TodayPlayTime:
                case CEnum.TagName.RayCity_TodayOfflineTime:
                case CEnum.TagName.RayCity_LastLoginTime:
                case CEnum.TagName.RayCity_LastLogoutTime:
                case CEnum.TagName.RayCity_BeginDate:
                case CEnum.TagName.RayCity_EndDate:
                case CEnum.TagName.RayCity_ExpireDate:
                case CEnum.TagName.RayCity_FixedTime:
                case CEnum.TagName.RayCity_CreateDate:
                case CEnum.TagName.RayCity_LastUpdateDate:
                case CEnum.TagName.RayCity_StartDate:
                case CEnum.TagName.RayCity_SendDate://��������
                case CEnum.TagName.RC_BuyTime:
              
                case CEnum.TagName.RC_ReceiveTime:
                case CEnum.TagName.RC_OperationTime:
                case CEnum.TagName.RC_ActiveTime:
                #endregion
                ///
                #region �Ҵ�online
                case CEnum.TagName.f_regDate://ע������ timestamp
                case CEnum.TagName.f_loginDate://����½���� timestamp
                case CEnum.TagName.f_logoutDate://���ǳ����� timestamp
                case CEnum.TagName.f_bandate://��ͣ���� timestamp
                case CEnum.TagName.SD_GetTime://���ʱ�� timestamp
                case CEnum.TagName.SD_DateEnd://����ʱ�� timestamp
                case CEnum.TagName.SD_StartTime://��ʼʱ��  timestamp
                case CEnum.TagName.SD_EndTime://����ʱ�� timestamp
                case CEnum.TagName.SD_SendTime://����ʱ�� dateTime
                case CEnum.TagName.SD_DelTime://ɾ��ʱ�� timestamp
                case CEnum.TagName.SD_LevelUpTime://����ʱ�� timestamp

                #endregion


                case CEnum.TagName.AU_Log_start://��ѯ��ʼʱ�� (char 14) [YYYYMMDDHHIISS]
                case CEnum.TagName.AU_Log_end://��ѯ����ʱ�� (char 14) [YYYYMMDDHHIISS]
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    tBody[iRow, iField].oContent = tTlv.toTimeStamp();
                    break;
                case CEnum.TagName.CARD_PDCardPrice:// 0x1208
                
                case CEnum.TagName.CARD_SumTotal:
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_MONEY;
                    tBody[iRow, iField].oContent = tTlv.toMoney();
                    break;

                case CEnum.TagName.O2JAM2_Sex://= 0x1611,//Format:TLV_BOOLEAN �Ա�
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_BOOLEAN;
                    tBody[iRow, iField].oContent = tTlv.toInteger();
                    break;
                default:
                    tBody[iRow, iField].eTag = CEnum.TagFormat.TLV_STRING;
                    tBody[iRow, iField].oContent = "�޷���ñ�ǩ";
                    break;
			}
			#endregion

			return tBody;
		}
		/// <summary>
		/// ������Ϣ
		/// </summary>
        private CEnum.Message_Body[,] ReciveMessage(CSocketClient pSocketClient)
		{
			int iField = 0;
            CEnum.Message_Body[,] p_ReturnBody = null;

			Packet mPacket = null;
			Packet_Head mPacketHead = null;
			Packet_Body mPacketbody = null;
			
			try
			{
				CSocketData mReciveData = new CSocketData();

				mReciveData = mSockData.SocketRecive(pSocketClient.ReceiveData());

				if (System.Text.Encoding.Unicode.GetString(mReciveData.bMsgBuffer).Equals("FAILURE"))
				{
                    p_ReturnBody = new CEnum.Message_Body[1, 1];
					p_ReturnBody[0,0].eName = CEnum.TagName.ERROR_Msg;
					p_ReturnBody[0,0].eTag = CEnum.TagFormat.TLV_STRING;
					p_ReturnBody[0,0].oContent = "��������ʱ";

					return p_ReturnBody;
				}

				if (!mReciveData.bValidMsag)
				{
                    p_ReturnBody = new CEnum.Message_Body[1, 1];
                    p_ReturnBody[0, 0].eName = CEnum.TagName.ERROR_Msg;
                    p_ReturnBody[0, 0].eTag = CEnum.TagFormat.TLV_STRING;
					p_ReturnBody[0,0].oContent = "���ݽ����쳣";

					return p_ReturnBody;
				}

				mPacket = mReciveData.m_Packet;
				mPacketHead = mReciveData.m_Packet.m_Head;
				mPacketbody = mReciveData.m_Packet.m_Body;	

				#region ���� MessageID
				switch (mReciveData.GetMessageID())
                {
                    #region ---
                    /*
                    case CEnum.Message_Tag_ID.CONNECT: //0x800001,
                    case CEnum.Message_Tag_ID.CONNECT_RESP: //0x808001,
                    case CEnum.Message_Tag_ID.DISCONNECT: //0x800002,
                    case CEnum.Message_Tag_ID.DISCONNECT_RESP: //0x808002,
                    case CEnum.Message_Tag_ID.ACCOUNT_AUTHOR: //0x800003,
                    case CEnum.Message_Tag_ID.ACCOUNT_AUTHOR_RESP: //0x808003,
                    case CEnum.Message_Tag_ID.USER_CREATE: //0x810001,
                    case CEnum.Message_Tag_ID.USER_CREATE_RESP: //0x818001,
                    case CEnum.Message_Tag_ID.USER_UPDATE: //0x810002,
                    case CEnum.Message_Tag_ID.USER_UPDATE_RESP: //0x818002,
                    case CEnum.Message_Tag_ID.USER_DELETE: //0x810003,
                    case CEnum.Message_Tag_ID.USER_DELETE_RESP: //0x818003,
                    case CEnum.Message_Tag_ID.USER_QUERY: //0x810004,
                    case CEnum.Message_Tag_ID.USER_PASSWD_MODIF: //0x810005
                    case CEnum.Message_Tag_ID.USER_PASSWD_MODIF_RESP: //0x818005
                    case CEnum.Message_Tag_ID.MODULE_CREATE: //0x820001,
                    case CEnum.Message_Tag_ID.MDDULE_CREATE_RESP: //0x828001,
                    case CEnum.Message_Tag_ID.MODULE_UPDATE: //0x820002,
                    case CEnum.Message_Tag_ID.MODULE_UPDATE_RESP: //0x828002,
                    case CEnum.Message_Tag_ID.MODULE_DELETE: //0x820003,
                    case CEnum.Message_Tag_ID.MODULE_DELETE_RESP: //0x828003,
                    case CEnum.Message_Tag_ID.MODULE_QUERY: //0x820004,
                    case CEnum.Message_Tag_ID.USER_MODULE_CREATE: //0x830001,
                    case CEnum.Message_Tag_ID.USER_MODULE_CREATE_RESP: //0x838001,
                    case CEnum.Message_Tag_ID.USER_MODULE_UPDATE: //0x830002,
                    case CEnum.Message_Tag_ID.USER_MODULE_UPDATE_RESP: //0x838002,
                    case CEnum.Message_Tag_ID.USER_MODULE_DELETE: //0x830003,
                    case CEnum.Message_Tag_ID.USER_MODULE_DELETE_RESP: //0x838003,
                    case CEnum.Message_Tag_ID.GAME_CREATE: //0x840001,//����GM�ʺ�����
                    case CEnum.Message_Tag_ID.GAME_CREATE_RESP: //0x848001,//����GM�ʺ���Ӧ
                    case CEnum.Message_Tag_ID.GAME_UPDATE: //0x840002,//����GM�ʺ���Ϣ����
                    case CEnum.Message_Tag_ID.GAME_UPDATE_RESP: //0x848002,//����GM�ʺ���Ϣ��Ӧ
                    case CEnum.Message_Tag_ID.GAME_DELETE: //0x840003,//ɾ��GM�ʺ���Ϣ����
                    case CEnum.Message_Tag_ID.GAME_DELETE_RESP: //0x848003,//ɾ��GM�ʺ���Ϣ��Ӧ
                    case CEnum.Message_Tag_ID.GAME_QUERY:
                    case CEnum.Message_Tag_ID.GAME_MODULE_QUERY:
                    case CEnum.Message_Tag_ID.NOTDEFINED: //0x0,
                    case CEnum.Message_Tag_ID.NOTES_LETTER_PROCESS://  0x850002, //�ʼ�����
                    case CEnum.Message_Tag_ID.NOTES_LETTER_PROCESS_RESP://  0x858002,//�ʼ�����
                    case CEnum.Message_Tag_ID.ERROR: //0xFFFFFFFF
                    case CEnum.Message_Tag_ID.UPDATE_ACTIVEUSER://���������û�״̬
                    case CEnum.Message_Tag_ID.UPDATE_ACTIVEUSER_RESP://���������û�״̬��Ӧ
                    case CEnum.Message_Tag_ID.LINK_SERVERIP_CREATE:// = 0x0007,
                    case CEnum.Message_Tag_ID.LINK_SERVERIP_CREATE_RESP:// = 0x8007,
                    
                    ////////////////////////��Ϸ������Ϣ -- �ͽ�////////////////////////
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_UPDATE: //0x860002,//�޸����״̬
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_UPDATE_RESP: //0x868002,//�޸����״̬��Ӧ
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_EXPLOIT_UPDATE: //0x860004,//�޸Ĺ�ѫֵ
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_EXPLOIT_UPDATE_RESP: //0x868004,//�޸Ĺ�ѫֵ��Ӧ
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_CREATE: //0x860006,//��Ӻ���
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_CREATE_RESP: //0x868006,//��Ӻ�����Ӧ
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_DELETE: //0x860007,//ɾ������
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_DELETE_RESP: //0x868007,//ɾ��������Ӧ
                    case CEnum.Message_Tag_ID.MJ_GUILDTABLE_UPDATE: //0x860008,//�޸ķ����������Ѵ��ڰ��
                    case CEnum.Message_Tag_ID.MJ_GUILDTABLE_UPDATE_RESP: //0x868008,//�޸ķ����������Ѵ��ڰ����Ӧ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_CREATE: //0x860009,//���������ϵ�account����������Ϣ���浽���ط�������
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_CREATE_RESP: //0x868009,//���������ϵ�account����������Ϣ���浽���ط���������Ӧ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_DELETE: //0x860009,//���÷�ͣ�ʺ�
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_DELETE_RESP: //0x868009,//���÷�ͣ�ʺŵ���Ӧ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_RESTORE: //0x860010,//����ʺ�
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_RESTORE_RESP: //0x868010,//����ʺ���Ӧ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_LIMIT_RESTORE: //0x860011,//��ʱ�޵ķ�ͣ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_LIMIT_RESTORE_RESP: //0x868011,//��ʱ�޵ķ�ͣ��Ӧ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_LOCAL_CREATE: //0x860012,//����������뵽���� 
                    case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_LOCAL_CREATE_RESP: //0x868012,//����������뵽����
                    case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_UPDATE: //0x860013,//�޸�������� 
                    case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_UPDATE_RESP: //0x868013,//�޸��������
                    case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_RESTORE: //0x860014,//�ָ��������
                    case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_RESTORE_RESP: //0x868014,//�ָ��������
                    
                    ////////////////////////��Ϸ������Ϣ -- ��������////////////////////////
                    case CEnum.Message_Tag_ID.SDO_ACCOUNT_CLOSE:  //0x870028,//��ͣ�ʻ���Ȩ����Ϣ
                    case CEnum.Message_Tag_ID.SDO_ACCOUNT_CLOSE_RESP:  //0x878028,//��ͣ�ʻ���Ȩ����Ϣ��Ӧ
                    case CEnum.Message_Tag_ID.SDO_ACCOUNT_OPEN:  //0x870029,//����ʻ���Ȩ����Ϣ
                    case CEnum.Message_Tag_ID.SDO_ACCOUNT_OPEN_RESP:  //0x878029,//����ʻ���Ȩ����Ϣ��Ӧ
                    case CEnum.Message_Tag_ID.SDO_PASSWORD_RECOVERY:  //0x870030,//����һ�����
                    case CEnum.Message_Tag_ID.SDO_PASSWORD_RECOVERY_RESP:  //0x878030,//����һ�������Ӧ
                    case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_UPDATE:  //0x870034,//�޸���ҵ��˺���Ϣ
                    case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_UPDATE_RESP:  //0x878034,//�޸���ҵ��˺���Ϣ��Ӧ

						iField = 1;
                        p_ReturnBody = new CEnum.Message_Body[mPacketbody.TLVCount, iField];
						
						for (int i=0; i<mPacketbody.TLVCount; i++)
						{
							TLV_Structure mStruct = new TLV_Structure(mPacketbody.getTLVByIndex(i).m_Tag, mPacketbody.getTLVByIndex(i).m_uiValueLen, mPacketbody.getTLVByIndex(i).m_bValueBuffer);
							
							p_ReturnBody[i,0].eName = mPacketbody.getTLVByIndex(i).m_Tag;
							p_ReturnBody = DecodeRecive(i, iField - 1, mStruct, p_ReturnBody);
						}					
						break;
                    case CEnum.Message_Tag_ID.MODULE_QUERY_RESP: //0x828004,
                    case CEnum.Message_Tag_ID.USER_MODULE_QUERY:// 0x830004:��ѯ�û�����Ӧģ������
                    case CEnum.Message_Tag_ID.USER_MODULE_QUERY_RESP://0x838004:��ѯ�û�����Ӧģ����Ӧ
                    case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSFER: //0x850001, //ȡ���ʼ��б�
                    case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSFER_RESP: //0x858001,//ȡ���ʼ��б����Ӧ
                    case CEnum.Message_Tag_ID.USER_QUERY_RESP: //0x818004,
                    case CEnum.Message_Tag_ID.GAME_QUERY_RESP: //0x818004,
                    case CEnum.Message_Tag_ID.GAME_MODULE_QUERY_RESP: //0x848005,
                    case CEnum.Message_Tag_ID.USER_QUERY_ALL: //0x818004,
                    case CEnum.Message_Tag_ID.USER_QUERY_ALL_RESP: //0x848005,
                    case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSMIT: //0x818004,
                    case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSMIT_RESP: //0x848005,
                    case CEnum.Message_Tag_ID.DEPART_QUERY: //0x818004,
                    case CEnum.Message_Tag_ID.DEPART_QUERY_RESP: //0x848005,
                    case CEnum.Message_Tag_ID.SERVERINFO_IP_QUERY:
                    case CEnum.Message_Tag_ID.SERVERINFO_IP_QUERY_RESP:
                    case CEnum.Message_Tag_ID.SERVERINFO_IP_ALL_QUERY:// = 0x0006,//��ѯ������Ϸ����IP
                    case CEnum.Message_Tag_ID.SERVERINFO_IP_ALL_QUERY_RESP:// = 0x8006,//��ѯ������Ϸ����IP��Ӧ
                    ////////////////////////��Ϸ������Ϣ -- �ͽ�////////////////////////
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_QUERY: //0x860001,//������״̬
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_QUERY_RESP: //0x868001,//������״̬��Ӧ
                    case CEnum.Message_Tag_ID.MJ_LOGINTABLE_QUERY: //0x860003,//�������Ƿ�����
                    case CEnum.Message_Tag_ID.MJ_LOGINTABLE_QUERY_RESP: //0x868003,//�������Ƿ�������Ӧ
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_QUERY: //0x860005,//�г���������
                    case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_QUERY_RESP: //0x868005,//�г�����������Ӧ
                    case CEnum.Message_Tag_ID.MJ_ITEMLOG_QUERY: //0x860015,//�����û����׼�¼
                    case CEnum.Message_Tag_ID.MJ_ITEMLOG_QUERY_RESP: //0x868015,//�����û����׼�¼
                    case CEnum.Message_Tag_ID.MJ_GMTOOLS_LOG_QUERY: //0x860016,//���ʹ���߲�����¼
                    case CEnum.Message_Tag_ID.MJ_GMTOOLS_LOG_QUERY_RESP: //0x868016,//���ʹ���߲�����¼
                    case CEnum.Message_Tag_ID.MJ_MONEYFIGHTERSORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_MONEYFIGHTERSORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_MONEYRABBISORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_MONEYRABBISORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_MONEYSORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_MONEYSORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_MONEYTAOISTSORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_MONEYTAOISTSORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_LEVELFIGHTERSORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_LEVELFIGHTERSORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_LEVELRABBISORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_LEVELRABBISORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_LEVELSORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_LEVELSORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_LEVELTAOISTSORT_QUERY:
                    case CEnum.Message_Tag_ID.MJ_LEVELTAOISTSORT_QUERY_RESP:
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_QUERY:// = 0x860027,//��ѯ�ͽ������ʺ�
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_QUERY_RESP:// = 0x868027,//��ѯ�ͽ������ʺ���Ӧ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_QUERY:// = 0x0026,//�ͽ��ʺŲ�ѯ
                    case CEnum.Message_Tag_ID.MJ_ACCOUNT_QUERY_RESP:// = 0x8026,//�ͽ��ʺŲ�ѯ��Ӧ
                    ////////////////////////��Ϸ������Ϣ -- ��������////////////////////////
                    case CEnum.Message_Tag_ID.SDO_ACCOUNT_QUERY:  //0x870026,//�鿴��ҵ��ʺ���Ϣ
                    case CEnum.Message_Tag_ID.SDO_ACCOUNT_QUERY_RESP:  //0x878026,//�鿴��ҵ��ʺ���Ϣ��Ӧ
                    case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_QUERY:  //0x870027,//�鿴�������ϵ���Ϣ
                    case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_QUERY_RESP:  //0x878027,//�鿴�������ϵ���Ϣ��Ӧ
                    case CEnum.Message_Tag_ID.SDO_CONSUME_QUERY:  //0x870031,//�鿴��ҵ����Ѽ�¼
                    case CEnum.Message_Tag_ID.SDO_CONSUME_QUERY_RESP:  //0x878031,//�鿴��ҵ����Ѽ�¼��Ӧ
                    case CEnum.Message_Tag_ID.SDO_USERONLINE_QUERY:  //0x870032,//�鿴���������״̬
                    case CEnum.Message_Tag_ID.SDO_USERONLINE_QUERY_RESP:  //0x878032,//�鿴���������״̬��Ӧ
                    case CEnum.Message_Tag_ID.SDO_USERTRADE_QUERY:  //0x870033,//�鿴��ҽ���״̬
                    case CEnum.Message_Tag_ID.SDO_USERTRADE_QUERY_RESP:  //0x878033,//�鿴��ҽ���״̬��Ӧ
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_QUERY:  //0x870033,//�鿴��ҽ���״̬
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_QUERY_RESP:  //0x878033,//�鿴��ҽ���״̬��Ӧ
                    case CEnum.Message_Tag_ID.SDO_GIFTBOX_CREATE:
                    case CEnum.Message_Tag_ID.SDO_GIFTBOX_CREATE_RESP:
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_DELETE:
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_DELETE_RESP:
                    case CEnum.Message_Tag_ID.SDO_USERLOGIN_STATUS_QUERY:
                    case CEnum.Message_Tag_ID.SDO_USERLOGIN_STATUS_QUERY_RESP:
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_BYOWNER_QUERY:
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_BYOWNER_QUERY_RESP:
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_TRADE_QUERY:// 0x870040,//�鿴��ҽ��׼�¼��Ϣ
                    case CEnum.Message_Tag_ID.SDO_ITEMSHOP_TRADE_QUERY_RESP:// 0x878040,//�鿴��ҽ��׼�¼��Ϣ����Ӧ
                    case CEnum.Message_Tag_ID.SDO_MEMBERSTOPSTATUS_QUERY:// = 0x870041,//�鿴���ʺ�״̬
                    case CEnum.Message_Tag_ID.SDO_MEMBERSTOPSTATUS_QUERY_RESP:// = 0x878041,///�鿴���ʺ�״̬����Ӧ
                    case CEnum.Message_Tag_ID.SDO_GIFTBOX_QUERY:// = 0x870042,//�鿴�������еĵ���
                    case CEnum.Message_Tag_ID.SDO_GIFTBOX_QUERY_RESP:// = 0x878042,//�鿴�������еĵ�����Ӧ
                    case CEnum.Message_Tag_ID.SDO_MEMBERBANISHMENT_QUERY: //0x870044,//�鿴����ͣ����ʺ�
                    case CEnum.Message_Tag_ID.SDO_MEMBERBANISHMENT_QUERY_RESP: //0x878044,//�鿴����ͣ����ʺ���Ӧ
                    case CEnum.Message_Tag_ID.SDO_USERMCASH_QUERY: //0x870045,//��ҳ�ֵ��¼��ѯ
                    case CEnum.Message_Tag_ID.SDO_USERMCASH_QUERY_RESP: //0x878045,//��ҳ�ֵ��¼��ѯ��Ӧ
                    case CEnum.Message_Tag_ID.SDO_USERGCASH_UPDATE: //0x870046,//�������G��
                    case CEnum.Message_Tag_ID.SDO_USERGCASH_UPDATE_RESP: //0x878046,//�������G�ҵ���Ӧ
                    case CEnum.Message_Tag_ID.SDO_MEMBERLOCAL_BANISHMENT://= 0x870047,//���ر���ͣ����Ϣ
                    case CEnum.Message_Tag_ID.SDO_MEMBERLOCAL_BANISHMENT_RESP: //0x878047,//���ر���ͣ����Ϣ��Ӧ
                    //////////////////////////GM ������־///////////////////////////////////
                    case CEnum.Message_Tag_ID.GMTOOLS_OperateLog_Query:// 0x800005,//�鿴���߲�����¼
                    case CEnum.Message_Tag_ID.GMTOOLS_OperateLog_Query_RESP:// 0x808005,//�鿴���߲�����¼��Ӧ
                    //////////////////////////����//////////////////////////////////////////
	                case CEnum.Message_Tag_ID.CLIENT_PATCH_COMPARE:// = 0x0008,//�ͻ��˰汾����
                    case CEnum.Message_Tag_ID.CLIENT_PATCH_COMPARE_RESP:// = 0x8008,//�ͻ��˰汾����
                    */
                    #endregion
                        /// <summary>
                        /// ����ģ��(0x80)
                        /// </summary>
                        case CEnum.Message_Tag_ID.CONNECT:// 0x800001://��������
                        case CEnum.Message_Tag_ID.CONNECT_RESP:// 0x808001://������Ӧ
                        case CEnum.Message_Tag_ID.DISCONNECT:// 0x800002://�Ͽ�����
                        case CEnum.Message_Tag_ID.DISCONNECT_RESP:// 0x808002://�Ͽ���Ӧ
                        case CEnum.Message_Tag_ID.ACCOUNT_AUTHOR:// 0x800003://�û������֤����
                        case CEnum.Message_Tag_ID.ACCOUNT_AUTHOR_RESP:// 0x808003://�û������֤��Ӧ
                        case CEnum.Message_Tag_ID.SERVERINFO_IP_QUERY:// 0x800004:
                        case CEnum.Message_Tag_ID.SERVERINFO_IP_QUERY_RESP:// 0x808004:
                        case CEnum.Message_Tag_ID.GMTOOLS_OperateLog_Query:// 0x800005://�鿴���߲�����¼
                        case CEnum.Message_Tag_ID.GMTOOLS_OperateLog_Query_RESP:// 0x808005://�鿴���߲�����¼��Ӧ
                        case CEnum.Message_Tag_ID.SERVERINFO_IP_ALL_QUERY:// 0x800006:
                        case CEnum.Message_Tag_ID.SERVERINFO_IP_ALL_QUERY_RESP:// 0x808006:
                        case CEnum.Message_Tag_ID.LINK_SERVERIP_CREATE:// 0x800007:
                        case CEnum.Message_Tag_ID.LINK_SERVERIP_CREATE_RESP:// 0x808007:
                        case CEnum.Message_Tag_ID.CLIENT_PATCH_COMPARE:// 0x800008:
                        case CEnum.Message_Tag_ID.CLIENT_PATCH_COMPARE_RESP:// 0x808008:
                        case CEnum.Message_Tag_ID.CLIENT_PATCH_UPDATE:// 0x800009:
                        case CEnum.Message_Tag_ID.CLIENT_PATCH_UPDATE_RESP:// 0x808009:

                        /// <summary>
                        /// �û�����ģ��(0x81)
                        /// </summary>
                        case CEnum.Message_Tag_ID.USER_CREATE:// 0x810001://����GM�ʺ�����
                        case CEnum.Message_Tag_ID.USER_CREATE_RESP:// 0x818001://����GM�ʺ���Ӧ
                        case CEnum.Message_Tag_ID.USER_UPDATE:// 0x810002://����GM�ʺ���Ϣ����
                        case CEnum.Message_Tag_ID.USER_UPDATE_RESP:// 0x818002://����GM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.USER_DELETE:// 0x810003://ɾ��GM�ʺ���Ϣ����
                        case CEnum.Message_Tag_ID.USER_DELETE_RESP:// 0x818003://ɾ��GM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.USER_QUERY:// 0x810004://��ѯGM�ʺ���Ϣ����
                        case CEnum.Message_Tag_ID.USER_QUERY_RESP:// 0x818004://��ѯGM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.USER_PASSWD_MODIF:// 0x810005://�����޸�����
                        case CEnum.Message_Tag_ID.USER_PASSWD_MODIF_RESP:// 0x818005: //�����޸���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.USER_QUERY_ALL:// 0x810006://��ѯ����GM�ʺ���Ϣ
                        case CEnum.Message_Tag_ID.USER_QUERY_ALL_RESP:// 0x818006://��ѯ����GM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.DEPART_QUERY:// 0x810007: //��ѯ�����б�
                        case CEnum.Message_Tag_ID.DEPART_QUERY_RESP:// 0x818007://��ѯ�����б���Ӧ
                        case CEnum.Message_Tag_ID.UPDATE_ACTIVEUSER:// 0x810008://���������û�״̬
                        case CEnum.Message_Tag_ID.UPDATE_ACTIVEUSER_RESP:// 0x818008://���������û�״̬��Ӧ

                        /// <summary>
                        /// ģ�����(0x82)
                        /// </summary>
                        case CEnum.Message_Tag_ID.MODULE_CREATE:// 0x820001://����ģ����Ϣ����
                        case CEnum.Message_Tag_ID.MDDULE_CREATE_RESP:// 0x828001://����ģ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.MODULE_UPDATE://0x820002://����ģ����Ϣ����
                        case CEnum.Message_Tag_ID.MODULE_UPDATE_RESP:// 0x828002://����ģ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.MODULE_DELETE:// 0x820003://ɾ��ģ������
                        case CEnum.Message_Tag_ID.MODULE_DELETE_RESP:// 0x828003://ɾ��ģ����Ӧ
                        case CEnum.Message_Tag_ID.MODULE_QUERY:// 0x820004://��ѯģ����Ϣ������
                        case CEnum.Message_Tag_ID.MODULE_QUERY_RESP:// 0x828004://��ѯģ����Ϣ����Ӧ

                        /// <summary>
                        /// �û���ģ�����(0x83)
                        /// </summary>
                        case CEnum.Message_Tag_ID.USER_MODULE_CREATE:// 0x830001://�����û���ģ������
                        case CEnum.Message_Tag_ID.USER_MODULE_CREATE_RESP:// 0x838001://�����û���ģ����Ӧ
                        case CEnum.Message_Tag_ID.USER_MODULE_UPDATE:// 0x830002://�����û���ģ�������
                        case CEnum.Message_Tag_ID.USER_MODULE_UPDATE_RESP:// 0x838002://�����û���ģ�����Ӧ
                        case CEnum.Message_Tag_ID.USER_MODULE_DELETE:// 0x830003://ɾ���û���ģ������
                        case CEnum.Message_Tag_ID.USER_MODULE_DELETE_RESP:// 0x838003://ɾ���û���ģ����Ӧ
                        case CEnum.Message_Tag_ID.USER_MODULE_QUERY:// 0x830004://��ѯ�û�����Ӧģ������
                        case CEnum.Message_Tag_ID.USER_MODULE_QUERY_RESP:// 0x838004://��ѯ�û�����Ӧģ����Ӧ

                        /// <summary>
                        /// ��Ϸ����(0x84)
                        /// </summary>
                        case CEnum.Message_Tag_ID.GAME_CREATE:// 0x840001://����GM�ʺ�����
                        case CEnum.Message_Tag_ID.GAME_CREATE_RESP:// 0x848001://����GM�ʺ���Ӧ
                        case CEnum.Message_Tag_ID.GAME_UPDATE:// 0x840002://����GM�ʺ���Ϣ����
                        case CEnum.Message_Tag_ID.GAME_UPDATE_RESP:// 0x848002://����GM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.GAME_DELETE:// 0x840003://ɾ��GM�ʺ���Ϣ����
                        case CEnum.Message_Tag_ID.GAME_DELETE_RESP:// 0x848003://ɾ��GM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.GAME_QUERY:// 0x840004://��ѯGM�ʺ���Ϣ����
                        case CEnum.Message_Tag_ID.GAME_QUERY_RESP:// 0x848004://��ѯGM�ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.GAME_MODULE_QUERY:// 0x840005://��ѯ��Ϸ��ģ���б�
                        case CEnum.Message_Tag_ID.GAME_MODULE_QUERY_RESP:// 0x848005://��ѯ��Ϸ��ģ���б���Ӧ


                        /// <summary>
                        /// NOTES����(0x85)
                        /// </summary>
                        case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSFER:// 0x850001: //ȡ���ʼ��б�
                        case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSFER_RESP:// 0x858001://ȡ���ʼ��б����Ӧ
                        case CEnum.Message_Tag_ID.NOTES_LETTER_PROCESS:// 0x850002: //�ʼ�����
                        case CEnum.Message_Tag_ID.NOTES_LETTER_PROCESS_RESP:// 0x858002://�ʼ�����
                        case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSMIT:// 0x850003: //�ʼ�ת���б�
                        case CEnum.Message_Tag_ID.NOTES_LETTER_TRANSMIT_RESP:// 0x858003://�ʼ�ת���б�

                        /// <summary>
                        /// �ͽ�GM����(0x86)
                        /// </summary>
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_QUERY:// 0x860001://������״̬
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_QUERY_RESP:// 0x868001://������״̬��Ӧ
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_UPDATE:// 0x860002://�޸����״̬
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_UPDATE_RESP:// 0x868002://�޸����״̬��Ӧ
                        case CEnum.Message_Tag_ID.MJ_LOGINTABLE_QUERY:// 0x860003://�������Ƿ�����
                        case CEnum.Message_Tag_ID.MJ_LOGINTABLE_QUERY_RESP:// 0x868003://�������Ƿ�������Ӧ
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_EXPLOIT_UPDATE:// 0x860004://�޸Ĺ�ѫֵ
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_EXPLOIT_UPDATE_RESP:// 0x868004://�޸Ĺ�ѫֵ��Ӧ
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_QUERY:// 0x860005://�г���������
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_QUERY_RESP:// 0x868005://�г�����������Ӧ
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_CREATE:// 0x860006://��Ӻ���
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_CREATE_RESP:// 0x868006://��Ӻ�����Ӧ
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_DELETE:// 0x860007://ɾ������
                        case CEnum.Message_Tag_ID.MJ_CHARACTERINFO_FRIEND_DELETE_RESP:// 0x868007://ɾ��������Ӧ
                        case CEnum.Message_Tag_ID.MJ_GUILDTABLE_UPDATE:// 0x860008://�޸ķ����������Ѵ��ڰ��
                        case CEnum.Message_Tag_ID.MJ_GUILDTABLE_UPDATE_RESP:// 0x868008://�޸ķ����������Ѵ��ڰ����Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_CREATE:// 0x860009://���������ϵ�account����������Ϣ���浽���ط�������
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_CREATE_RESP:// 0x868009://���������ϵ�account����������Ϣ���浽���ط���������Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_DELETE:// 0x860010://���÷�ͣ�ʺ�
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_DELETE_RESP:// 0x868010://���÷�ͣ�ʺŵ���Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_RESTORE:// 0x860011://����ʺ�
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_REMOTE_RESTORE_RESP:// 0x868011://����ʺ���Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_LIMIT_RESTORE:// 0x860012://��ʱ�޵ķ�ͣ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_LIMIT_RESTORE_RESP:// 0x868012://��ʱ�޵ķ�ͣ��Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_LOCAL_CREATE:// 0x860013://����������뵽���� 
                        case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_LOCAL_CREATE_RESP:// 0x868013://����������뵽����
                        case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_UPDATE:// 0x860014://�޸�������� 
                        case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_UPDATE_RESP:// 0x868014://�޸��������
                        case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_RESTORE:// 0x860015://�ָ��������
                        case CEnum.Message_Tag_ID.MJ_ACCOUNTPASSWD_REMOTE_RESTORE_RESP:// 0x868015://�ָ��������
                        case CEnum.Message_Tag_ID.MJ_ITEMLOG_QUERY:// 0x860016://�����û����׼�¼
                        case CEnum.Message_Tag_ID.MJ_ITEMLOG_QUERY_RESP:// 0x868016://�����û����׼�¼
                        case CEnum.Message_Tag_ID.MJ_GMTOOLS_LOG_QUERY:// 0x860017://���ʹ���߲�����¼
                        case CEnum.Message_Tag_ID.MJ_GMTOOLS_LOG_QUERY_RESP:// 0x868017://���ʹ���߲�����¼
                        case CEnum.Message_Tag_ID.MJ_MONEYSORT_QUERY:// 0x860018://���ݽ�Ǯ����
                        case CEnum.Message_Tag_ID.MJ_MONEYSORT_QUERY_RESP:// 0x868018://���ݽ�Ǯ�������Ӧ
                        case CEnum.Message_Tag_ID.MJ_LEVELSORT_QUERY:// 0x860019://���ݵȼ�����
                        case CEnum.Message_Tag_ID.MJ_LEVELSORT_QUERY_RESP:// 0x868019://���ݵȼ��������Ӧ
                        case CEnum.Message_Tag_ID.MJ_MONEYFIGHTERSORT_QUERY:// 0x860020://���ݲ�ְͬҵ��Ǯ����
                        case CEnum.Message_Tag_ID.MJ_MONEYFIGHTERSORT_QUERY_RESP:// 0x868020://���ݲ�ְͬҵ��Ǯ�������Ӧ
                        case CEnum.Message_Tag_ID.MJ_LEVELFIGHTERSORT_QUERY:// 0x860021://���ݲ�ְͬҵ�ȼ�����
                        case CEnum.Message_Tag_ID.MJ_LEVELFIGHTERSORT_QUERY_RESP:// 0x868021://���ݲ�ְͬҵ�ȼ��������Ӧ
                        case CEnum.Message_Tag_ID.MJ_MONEYTAOISTSORT_QUERY:// 0x860022://���ݵ�ʿ��Ǯ����
                        case CEnum.Message_Tag_ID.MJ_MONEYTAOISTSORT_QUERY_RESP:// 0x868022://���ݵ�ʿ��Ǯ�������Ӧ
                        case CEnum.Message_Tag_ID.MJ_LEVELTAOISTSORT_QUERY:// 0x860023://���ݵ�ʿ�ȼ�����
                        case CEnum.Message_Tag_ID.MJ_LEVELTAOISTSORT_QUERY_RESP:// 0x868023://���ݵ�ʿ�ȼ��������Ӧ
                        case CEnum.Message_Tag_ID.MJ_MONEYRABBISORT_QUERY:// 0x860024://���ݷ�ʦ��Ǯ����
                        case CEnum.Message_Tag_ID.MJ_MONEYRABBISORT_QUERY_RESP:// 0x868024://���ݷ�ʦ��Ǯ�������Ӧ
                        case CEnum.Message_Tag_ID.MJ_LEVELRABBISORT_QUERY:// 0x860025://���ݷ�ʦ�ȼ�����
                        case CEnum.Message_Tag_ID.MJ_LEVELRABBISORT_QUERY_RESP:// 0x868025://���ݷ�ʦ�ȼ��������Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_QUERY://  0x860026://�ͽ��ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_QUERY_RESP:// 0x868026://�ͽ��ʺŲ�ѯ��Ӧ
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_QUERY:// 0x860027://��ѯ�ͽ������ʺ�
                        case CEnum.Message_Tag_ID.MJ_ACCOUNT_LOCAL_QUERY_RESP:// 0x868027://��ѯ�ͽ������ʺ���Ӧ
                        case CEnum.Message_Tag_ID.SDO_ACCOUNT_QUERY:// 0x870026://�鿴��ҵ��ʺ���Ϣ
                        case CEnum.Message_Tag_ID.SDO_ACCOUNT_QUERY_RESP:// 0x878026://�鿴��ҵ��ʺ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_QUERY:// 0x870027://�鿴�������ϵ���Ϣ
                        case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_QUERY_RESP:// 0x878027://�鿴�������ϵ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_ACCOUNT_CLOSE:// 0x870028://��ͣ�ʻ���Ȩ����Ϣ
                        case CEnum.Message_Tag_ID.SDO_ACCOUNT_CLOSE_RESP:// 0x878028://��ͣ�ʻ���Ȩ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_ACCOUNT_OPEN:// 0x870029://����ʻ���Ȩ����Ϣ
                        case CEnum.Message_Tag_ID.SDO_ACCOUNT_OPEN_RESP:// 0x878029://����ʻ���Ȩ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_PASSWORD_RECOVERY:// 0x870030://����һ�����
                        case CEnum.Message_Tag_ID.SDO_PASSWORD_RECOVERY_RESP:// 0x878030://����һ�������Ӧ
                        case CEnum.Message_Tag_ID.SDO_CONSUME_QUERY:// 0x870031://�鿴��ҵ����Ѽ�¼
                        case CEnum.Message_Tag_ID.SDO_CONSUME_QUERY_RESP:// 0x878031://�鿴��ҵ����Ѽ�¼��Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERONLINE_QUERY:// 0x870032://�鿴���������״̬
                        case CEnum.Message_Tag_ID.SDO_USERONLINE_QUERY_RESP:// 0x878032://�鿴���������״̬��Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERTRADE_QUERY:// 0x870033://�鿴��ҽ���״̬
                        case CEnum.Message_Tag_ID.SDO_USERTRADE_QUERY_RESP:// 0x878033://�鿴��ҽ���״̬��Ӧ
                        case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_UPDATE:// 0x870034://�޸���ҵ��˺���Ϣ
                        case CEnum.Message_Tag_ID.SDO_CHARACTERINFO_UPDATE_RESP:// 0x878034://�޸���ҵ��˺���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_QUERY:// 0x870035://�鿴��Ϸ�������е�����Ϣ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_QUERY_RESP:// 0x878035://�鿴��Ϸ�������е�����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_DELETE:// 0x870036://ɾ����ҵ�����Ϣ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_DELETE_RESP:// 0x878036://ɾ����ҵ�����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_GIFTBOX_CREATE:// 0x870037://����������е�����Ϣ
                        case CEnum.Message_Tag_ID.SDO_GIFTBOX_CREATE_RESP:// 0x878037://����������е�����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_GIFTBOX_QUERY:// 0x870038://�鿴�������еĵ���
                        case CEnum.Message_Tag_ID.SDO_GIFTBOX_QUERY_RESP:// 0x878038://�鿴�������еĵ�����Ӧ
                        case CEnum.Message_Tag_ID.SDO_GIFTBOX_DELETE:// 0x870039://ɾ���������еĵ���
                        case CEnum.Message_Tag_ID.SDO_GIFTBOX_DELETE_RESP:// 0x878039://ɾ���������еĵ�����Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERLOGIN_STATUS_QUERY:// 0x870040://�鿴��ҵ�¼״̬
                        case CEnum.Message_Tag_ID.SDO_USERLOGIN_STATUS_QUERY_RESP:// 0x878040://�鿴��ҵ�¼״̬��Ӧ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_BYOWNER_QUERY:// 0x870041:////�鿴������ϵ�����Ϣ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_BYOWNER_QUERY_RESP:// 0x878041:////�鿴������ϵ�����Ϣ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_TRADE_QUERY:// 0x870042://�鿴��ҽ��׼�¼��Ϣ
                        case CEnum.Message_Tag_ID.SDO_ITEMSHOP_TRADE_QUERY_RESP:// 0x878042://�鿴��ҽ��׼�¼��Ϣ����Ӧ
                        case CEnum.Message_Tag_ID.SDO_MEMBERSTOPSTATUS_QUERY:// 0x870043://�鿴���ʺ�״̬
                        case CEnum.Message_Tag_ID.SDO_MEMBERSTOPSTATUS_QUERY_RESP:// 0x878043:///�鿴���ʺ�״̬����Ӧ
                        case CEnum.Message_Tag_ID.SDO_MEMBERBANISHMENT_QUERY:// 0x870044://�鿴����ͣ����ʺ�
                        case CEnum.Message_Tag_ID.SDO_MEMBERBANISHMENT_QUERY_RESP:// 0x878044://�鿴����ͣ����ʺ���Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERMCASH_QUERY:// 0x870045://��ҳ�ֵ��¼��ѯ
                        case CEnum.Message_Tag_ID.SDO_USERMCASH_QUERY_RESP:// 0x878045://��ҳ�ֵ��¼��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERGCASH_UPDATE:// 0x870046://�������G��
                        case CEnum.Message_Tag_ID.SDO_USERGCASH_UPDATE_RESP:// 0x878046://�������G�ҵ���Ӧ
                        case CEnum.Message_Tag_ID.SDO_MEMBERLOCAL_BANISHMENT:// = 0x870047://���ر���ͣ����Ϣ
                        case CEnum.Message_Tag_ID.SDO_MEMBERLOCAL_BANISHMENT_RESP:// 0x878047://���ر���ͣ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_EMAIL_QUERY:// 0x870048://�õ���ҵ�EMAIL
                        case CEnum.Message_Tag_ID.SDO_EMAIL_QUERY_RESP:// 0x878048://�õ���ҵ�EMAIL��Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERCHARAGESUM_QUERY:// 0x870049://�õ���ֵ��¼�ܺ�
                        case CEnum.Message_Tag_ID.SDO_USERCHARAGESUM_QUERY_RESP:// 0x878049://�õ���ֵ��¼�ܺ���Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERCONSUMESUM_QUERY:// 0x870050://�õ����Ѽ�¼�ܺ�
                        case CEnum.Message_Tag_ID.SDO_USERCONSUMESUM_QUERY_RESP:// 0x878050://�õ����Ѽ�¼�ܺ���Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERGCASH_QUERY:// = 0x870051,//���?�Ҽ�¼��ѯ
                        case CEnum.Message_Tag_ID.SDO_USERGCASH_QUERY_RESP:// = 0x878051,//���?�Ҽ�¼��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.SDO_USERNICK_UPDATE:// =0x870069, 
                        case CEnum.Message_Tag_ID.SDO_USERNICK_UPDATE_RESP:// =0x878069, 
                        case CEnum.Message_Tag_ID.SDO_BOARDMESSAGE_REQ:// = 0x870071,
                        case CEnum.Message_Tag_ID.SDO_BOARDMESSAGE_REQ_RESP:// = 0x878071,
                        case CEnum.Message_Tag_ID.SDO_CHANNELLIST_QUERY:// =  0x870072,
		                case CEnum.Message_Tag_ID.SDO_CHANNELLIST_QUERY_RESP:// = 0x878072,
		                case CEnum.Message_Tag_ID.SDO_ALIVE_REQ:// = 0x870073,
                        case CEnum.Message_Tag_ID.SDO_ALIVE_REQ_RESP:// = 0x878073,
                        case CEnum.Message_Tag_ID.SDO_BOARDTASK_INSERT:
                        case CEnum.Message_Tag_ID.SDO_BOARDTASK_INSERT_RESP:
                        case CEnum.Message_Tag_ID.SDO_DAYSLIMIT_QUERY:
                        case CEnum.Message_Tag_ID.SDO_DAYSLIMIT_QUERY_RESP:

                        case CEnum.Message_Tag_ID.SDO_FRIENDS_QUERY:
                        case CEnum.Message_Tag_ID.SDO_FRIENDS_QUERY_RESP: 
                        case CEnum.Message_Tag_ID.SDO_USERLOGIN_DEL:
                        case CEnum.Message_Tag_ID.SDO_USERLOGIN_DEL_RESP :
                        case CEnum.Message_Tag_ID.SDO_USERLOGIN_CLEAR :
                        case CEnum.Message_Tag_ID.SDO_USERLOGIN_CLEAR_RESP:
                        case CEnum.Message_Tag_ID.SDO_GATEWAY_QUERY:
                        case CEnum.Message_Tag_ID.SDO_GATEWAY_QUERY_RESP:
                        case CEnum.Message_Tag_ID.CR_ACCOUNT_QUERY:
                        case CEnum.Message_Tag_ID.CR_ACCOUNT_QUERY_RESP:
                        case CEnum.Message_Tag_ID.CR_ACCOUNTACTIVE_QUERY:
                        case CEnum.Message_Tag_ID.CR_ACCOUNTACTIVE_QUERY_RESP:
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_QUERY:// = 0x890003,//������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_QUERY_RESP:// = 0x898003,//������Ϣ��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_CREATE:// = 0x890003,//��������
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_CREATE_RESP:// = 0x898003,//����������Ӧ
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_UPDATE:// = 0x890004,//���¹�����Ϣ
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_UPDATE_RESP:// = 0x898004,//���¹�����Ϣ����Ӧ
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_DELETE:// = 0x890005,//ɾ��������Ϣ
                        case CEnum.Message_Tag_ID.CR_CALLBOARD_DELETE_RESP:// = 0x898005,//ɾ��������Ϣ����Ӧ
                        case CEnum.Message_Tag_ID.CR_CHARACTERINFO_QUERY:// = 0x890007,//��ҽ�ɫ��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.CR_CHARACTERINFO_QUERY_RESP:// = 0x898007,//��ҽ�ɫ��Ϣ��ѯ����Ӧ
                        case CEnum.Message_Tag_ID.CR_CHARACTERINFO_UPDATE:// = 0x890008,//��ҽ�ɫ��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.CR_CHARACTERINFO_UPDATE_RESP:// = 0x898008,//��ҽ�ɫ��Ϣ��ѯ����Ӧ
                        case CEnum.Message_Tag_ID.CR_CHANNEL_QUERY:// = 0x890009,//����Ƶ����ѯ
                        case CEnum.Message_Tag_ID.CR_CHANNEL_QUERY_RESP:// = 0x898009,//����Ƶ����ѯ����Ӧ
                        case CEnum.Message_Tag_ID.CR_NICKNAME_QUERY:// = 0x890009,//����Ƶ����ѯ
                        case CEnum.Message_Tag_ID.CR_NICKNAME_QUERY_RESP:// = 0x898009,//����Ƶ����ѯ����Ӧ
                        case CEnum.Message_Tag_ID.CR_LOGIN_LOGOUT_QUERY:// = 0x890009,//����Ƶ����ѯ
                        case CEnum.Message_Tag_ID.CR_LOGIN_LOGOUT_QUERY_RESP:// = 0x898009,//����Ƶ����ѯ����Ӧ
                        case CEnum.Message_Tag_ID.CR_ERRORCHANNEL_QUERY ://������󹫸�Ƶ����ѯ
                        case CEnum.Message_Tag_ID.CR_ERRORCHANNEL_QUERY_RESP ://������󹫸�Ƶ����ѯ����Ӧ

                        case CEnum.Message_Tag_ID.AU_ACCOUNT_QUERY://=0x880001,//����ʺ���Ϣ��ѯ
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_QUERY_RESP://=0x888001,//����ʺ���Ϣ��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.AU_ACCOUNTREMOTE_QUERY://=0x880002,//��Ϸ��������ͣ������ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.AU_ACCOUNTREMOTE_QUERY_RESP://=0x888002,//��Ϸ��������ͣ������ʺŲ�ѯ��Ӧ
                        case CEnum.Message_Tag_ID.AU_ACCOUNTLOCAL_QUERY://=0x880003,//���ط�ͣ������ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.AU_ACCOUNTLOCAL_QUERY_RESP://=0x888003,//���ط�ͣ������ʺŲ�ѯ��Ӧ
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_CLOSE://=0x880004,//��ͣ������ʺ�
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_CLOSE_RESP://=0x888004,//��ͣ������ʺ���Ӧ
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_OPEN://=0x880005,//��������ʺ�
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_OPEN_RESP://=0x888005,//��������ʺ���Ӧ
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_BANISHMENT_QUERY://=0x880006,//��ҷ�ͣ�ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.AU_ACCOUNT_BANISHMENT_QUERY_RESP://=0x888006,//��ҷ�ͣ�ʺŲ�ѯ��Ӧ
                        case CEnum.Message_Tag_ID.AU_CHARACTERINFO_QUERY://=0x880007,//��ѯ��ҵ��˺���Ϣ
                        case CEnum.Message_Tag_ID.AU_CHARACTERINFO_QUERY_RESP://=0x888007,//��ѯ��ҵ��˺���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.AU_CHARACTERINFO_UPDATE://=0x880008,//�޸���ҵ��˺���Ϣ
                        case CEnum.Message_Tag_ID.AU_CHARACTERINFO_UPDATE_RESP://=0x888008,//�޸���ҵ��˺���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_QUERY://=0x880009,//�鿴��Ϸ�������е�����Ϣ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_QUERY_RESP://=0x888009,//�鿴��Ϸ�������е�����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_DELETE://=0x880010,//ɾ����ҵ�����Ϣ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_DELETE_RESP://=0x888010,//ɾ����ҵ�����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_BYOWNER_QUERY://=0x880011,////�鿴������ϵ�����Ϣ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_BYOWNER_QUERY_RESP://=0x888011,////�鿴������ϵ�����Ϣ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_TRADE_QUERY://=0x880012,//�鿴��ҽ��׼�¼��Ϣ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_TRADE_QUERY_RESP://=0x888012,//�鿴��ҽ��׼�¼��Ϣ����Ӧ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_CREATE://=0x880013,//����������е�����Ϣ
                        case CEnum.Message_Tag_ID.AU_ITEMSHOP_CREATE_RESP://=0x888013,//����������е�����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.AU_LEVELEXP_QUERY://=0x880014,//�鿴��ҵȼ�����
                        case CEnum.Message_Tag_ID.AU_LEVELEXP_QUERY_RESP://=0x888014,////�鿴��ҵȼ�������Ӧ
                        case CEnum.Message_Tag_ID.AU_USERLOGIN_STATUS_QUERY://=0x880015,//�鿴��ҵ�¼״̬
                        case CEnum.Message_Tag_ID.AU_USERLOGIN_STATUS_QUERY_RESP://=0x888015,//�鿴��ҵ�¼״̬��Ӧ
                        case CEnum.Message_Tag_ID.AU_USERCHARAGESUM_QUERY://=0x880016,//�õ���ֵ��¼�ܺ�
                        case CEnum.Message_Tag_ID.AU_USERCHARAGESUM_QUERY_RESP://=0x888016,//�õ���ֵ��¼�ܺ���Ӧ
                        case CEnum.Message_Tag_ID.AU_CONSUME_QUERY://=0x880017,//�鿴��ҵ����Ѽ�¼
                        case CEnum.Message_Tag_ID.AU_CONSUME_QUERY_RESP://=0x888017,//�鿴��ҵ����Ѽ�¼��Ӧ
                        case CEnum.Message_Tag_ID.AU_USERCONSUMESUM_QUERY://=0x880018,//�õ����Ѽ�¼�ܺ�
                        case CEnum.Message_Tag_ID.AU_USERCONSUMESUM_QUERY_RESP://=0x888018,//�õ����Ѽ�¼�ܺ���Ӧ
                        case CEnum.Message_Tag_ID.AU_USERMCASH_QUERY://=0x880019,//��ҳ�ֵ��¼��ѯ
                        case CEnum.Message_Tag_ID.AU_USERMCASH_QUERY_RESP://=0x888019,//��ҳ�ֵ��¼��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.AU_USERGCASH_QUERY://=0x880020,//���?�Ҽ�¼��ѯ
                        case CEnum.Message_Tag_ID.AU_USERGCASH_QUERY_RESP://=0x888020,//���?�Ҽ�¼��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.AU_USERGCASH_UPDATE://=0x880021,//�������G��
                        case CEnum.Message_Tag_ID.AU_USERGCASH_UPDATE_RESP://=0x888021,//�������G�ҵ���Ӧ

                    case CEnum.Message_Tag_ID.Au_User_Msg_Query: //��ѯ�������Ϸ�е������¼ 
                    case CEnum.Message_Tag_ID.Au_User_Msg_Query_Resp://��ѯ�������Ϸ�е������¼    

                    case CEnum.Message_Tag_ID.Au_BroaditeminfoNow_Query ://��ǰʱ���û�����������־
                    case CEnum.Message_Tag_ID.Au_BroaditeminfoNow_Query_Resp://��ǰʱ���û�����������־

                    case CEnum.Message_Tag_ID.Au_BroaditeminfoBymsg_Query://����ģ����ѯ�û�����������־
                    case CEnum.Message_Tag_ID.Au_BroaditeminfoBymsg_Query_Resp://����ģ����ѯ�û�����������־

                    case CEnum.Message_Tag_ID.AU_MsgServerinfo_Query :
                    case CEnum.Message_Tag_ID.AU_MsgServerinfo_Query_RESP :
                    case CEnum.Message_Tag_ID.MAGIC_Account_Query: //��ɫ��������
                    case CEnum.Message_Tag_ID.MAGIC_Account_Query_Resp:
                        case CEnum.Message_Tag_ID.CARD_USERCHARGEDETAIL_QUERY:// 0x900001,// ��ͨ��ѯ
                        case CEnum.Message_Tag_ID.CARD_USERCHARGEDETAIL_QUERY_RESP:// 0x908001,// ��ͨ��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.CARD_USERCONSUME_QUERY:// = 0x900003,//���б����Ѳ�ѯ
                        case CEnum.Message_Tag_ID.CARD_USERCONSUME_QUERY_RESP:// = 0x908003,//���б����Ѳ�ѯ��Ӧ
                        case CEnum.Message_Tag_ID.CARD_VNETCHARGE_QUERY:// 0x900005,//�����ǿճ�ֵ��ѯ
                        case CEnum.Message_Tag_ID.CARD_VNETCHARGE_QUERY_RESP:// 0x908005,//�����ǿճ�ֵ��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.CARD_USERDETAIL_SUM_QUERY:// = 0x900005,//��ֵ�ϼƲ�ѯ
                        case CEnum.Message_Tag_ID.CARD_USERDETAIL_SUM_QUERY_RESP:// = 0x908005,//��ֵ�ϼƲ�ѯ��Ӧ
                        case CEnum.Message_Tag_ID.CARD_USERCONSUME_SUM_QUERY:// = 0x900006,//���ѺϼƲ�ѯ
                        case CEnum.Message_Tag_ID.CARD_USERCONSUME_SUM_QUERY_RESP:// = 0x908006,//���Ѻϼ���Ӧ
                        case CEnum.Message_Tag_ID.CARD_USERINFO_QUERY:// = 0x900007,//���ע����Ϣ��ѯ
                        case CEnum.Message_Tag_ID.CARD_USERINFO_QUERY_RESP:// = 0x908007,//���ע����Ϣ��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.CARD_USERINFO_CLEAR:// = 0x900008,
                        case CEnum.Message_Tag_ID.CARD_USERINFO_CLEAR_RESP:// = 0x908008,
                        case CEnum.Message_Tag_ID.CARD_USERNICK_QUERY:// = 0x0010,
                        case CEnum.Message_Tag_ID.CARD_USERNICK_QUERY_RESP:// = 0x8010,
                    case CEnum.Message_Tag_ID.CARD_USERINITACTIVE_QUERY://������Ϸ
                    case CEnum.Message_Tag_ID.CARD_USERINITACTIVE_QUERY_RESP:
                        case CEnum.Message_Tag_ID.AU_USERNICK_UPDATE:// = 0x900011,
                        case CEnum.Message_Tag_ID.AU_USERNICK_UPDATE_RESP:// = 0x908011,


                        case CEnum.Message_Tag_ID.AUSHOP_USERGPURCHASE_QUERY:// = 0x910001,//�û�G�ҹ����¼
                        case CEnum.Message_Tag_ID.AUSHOP_USERGPURCHASE_QUERY_RESP:// = 0x918001,//�û�G�ҹ����¼
                        case CEnum.Message_Tag_ID.AUSHOP_USERMPURCHASE_QUERY:// = 0x910002,//�û�M�ҹ����¼
                        case CEnum.Message_Tag_ID.AUSHOP_USERMPURCHASE_QUERY_RESP:// = 0x918002,//�û�M�ҹ����¼
                        case CEnum.Message_Tag_ID.AUSHOP_AVATARECOVER_QUERY:// = 0x910003,//���߻��նһ���
                        case CEnum.Message_Tag_ID.AUSHOP_AVATARECOVER_QUERY_RESP:// = 0x918003,//���߻��նһ���
                        case CEnum.Message_Tag_ID.AUSHOP_USERINTERGRAL_QUERY:// = 0x910004,//�û����ּ�¼
                        case CEnum.Message_Tag_ID.AUSHOP_USERINTERGRAL_QUERY_RESP:// = 0x918004,//�û����ּ�¼

                        case CEnum.Message_Tag_ID.AUSHOP_USERGPURCHASE_SUM_QUERY:// = 0x0005,//�û�G�ҹ����¼�ϼ�
                        case CEnum.Message_Tag_ID.AUSHOP_USERGPURCHASE_SUM_QUERY_RESP:// = 0x8005,//�û�G�ҹ����¼�ϼ���Ӧ
                        case CEnum.Message_Tag_ID.AUSHOP_USERMPURCHASE_SUM_QUERY:// = 0x0006,//�û�M�ҹ����¼�ϼ�
                        case CEnum.Message_Tag_ID.AUSHOP_USERMPURCHASE_SUM_QUERY_RESP:// = 0x8006,//�û�M�ҹ����¼�ϼ���Ӧ
                        case CEnum.Message_Tag_ID.AUSHOP_AVATARECOVER_DETAIL_QUERY:// = 0x910007,// �߻��նһ���ϸ��¼
                        case CEnum.Message_Tag_ID.AUSHOP_AVATARECOVER_DETAIL_QUERY_RESP:// = 0x918007,// �߻��նһ���ϸ��¼

                        case CEnum.Message_Tag_ID.DEPARTMENT_CREATE:// = 0x810009,//���Ŵ���
                        case CEnum.Message_Tag_ID.DEPARTMENT_CREATE_RESP:// = 0x818009,//���Ŵ�����Ӧ
                        case CEnum.Message_Tag_ID.DEPARTMENT_UPDATE:// = 0x810010,//�����޸�
                        case CEnum.Message_Tag_ID.DEPARTMENT_UPDATE_RESP:// = 0x818010,//�����޸���Ӧ
                        case CEnum.Message_Tag_ID.DEPARTMENT_DELETE:// = 0x810011,//����ɾ��
                        case CEnum.Message_Tag_ID.DEPARTMENT_DELETE_RESP:// = 0x818011,//����ɾ����Ӧ
                        case CEnum.Message_Tag_ID.DEPARTMENT_ADMIN:// = 0x810012,//���Ź���
                        case CEnum.Message_Tag_ID.DEPARTMENT_ADMIN_RESP:// = 0x818012,//���Ź�����Ӧ
                        case CEnum.Message_Tag_ID.DEPARTMENT_RELATE_QUERY:// = 0x810013,//���Ź�����ѯ
                        case CEnum.Message_Tag_ID.DEPARTMENT_RELATE_QUERY_RESP:// = 0x818013,//���Ź�����ѯ

                        case CEnum.Message_Tag_ID.O2JAM_CHARACTERINFO_QUERY://= 0x920001,//��ҽ�ɫ��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_CHARACTERINFO_QUERY_RESP://= 0x928001,//��ҽ�ɫ��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_CHARACTERINFO_UPDATE://= 0x920002,//��ҽ�ɫ��Ϣ����
                        case CEnum.Message_Tag_ID.O2JAM_CHARACTERINFO_UPDATE_RESP://= 0x928002,//��ҽ�ɫ��Ϣ����
                        case CEnum.Message_Tag_ID.O2JAM_ITEM_QUERY://= 0x920001,//��ҵ�����Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_ITEM_QUERY_RESP://= 0x928001,//��ҽ�ɫ��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_ITEM_UPDATE://= 0x920002,//��ҵ�����Ϣ����
                        case CEnum.Message_Tag_ID.O2JAM_ITEM_UPDATE_RESP://= 0x928002,//��ҵ�����Ϣ����
                        case CEnum.Message_Tag_ID.O2JAM_CONSUME_QUERY://= 0x920001,//���������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_CONSUME_QUERY_RESP://= 0x928001,//���������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_ITEMDATA_QUERY://= 0x920001,//��ҽ�����Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_ITEMDATA_QUERY_RESP://= 0x928001,//��ҽ�����Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_GIFTBOX_QUERY:// = 0x920007://,//�������в�ѯ
		                case CEnum.Message_Tag_ID.O2JAM_GIFTBOX_QUERY_RESP:// = 0x928007,//�������в�ѯ
		                case CEnum.Message_Tag_ID.O2JAM_USERGCASH_UPDATE:// = 0x920008,//�������G��
		                case CEnum.Message_Tag_ID.O2JAM_USERGCASH_UPDATE_RESP:// = 0x928008,//�������G�ҵ���Ӧ
		                case CEnum.Message_Tag_ID.O2JAM_CONSUME_SUM_QUERY://= 0x920009,//���������Ϣ��ѯ
		                case CEnum.Message_Tag_ID.O2JAM_CONSUME_SUM_QUERY_RESP://= 0x928009,//���������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_GIFTBOX_CREATE_QUERY://= 0x920010,//��ҽ�����Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_GIFTBOX_CREATE_QUERY_RESP://= 0x928010,//��ҽ�����Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM_ITEMNAME_QUERY:// = 0x920011,
                        case CEnum.Message_Tag_ID.O2JAM_ITEMNAME_QUERY_RESP:// = 0x928011,
                        case CEnum.Message_Tag_ID.O2JAM_GIFTBOX_DELETE:// = 0x920012,
                        case CEnum.Message_Tag_ID.O2JAM_GIFTBOX_DELETE_RESP://  =0x928012,



                        case CEnum.Message_Tag_ID.DEPART_RELATE_GAME_QUERY:// = 0x810014,
                        case CEnum.Message_Tag_ID.DEPART_RELATE_GAME_QUERY_RESP:// = 0x818014,
                        case CEnum.Message_Tag_ID.USER_SYSADMIN_QUERY:// = 0x810015,
                        case CEnum.Message_Tag_ID.USER_SYSADMIN_QUERY_RESP:// = 0x818015,

                        case CEnum.Message_Tag_ID.CARD_USERSECURE_CLEAR:// = 0x900009,//������Ұ�ȫ����Ϣ
                        case CEnum.Message_Tag_ID.CARD_USERSECURE_CLEAR_RESP:// = 0x908009,//������Ұ�ȫ����Ϣ��Ӧ


                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNT_QUERY:// = 0x930001,//����ʺ���Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNT_QUERY_RESP:// = 0x938001,//����ʺ���Ϣ��ѯ��Ӧ
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNTACTIVE_QUERY:// = 0x930002,//����ʺż�����Ϣ
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNTACTIVE_QUERY_RESP:// = 0x938002,//����ʺż�����Ӧ


                        case CEnum.Message_Tag_ID.O2JAM2_CHARACTERINFO_QUERY://= 0x930003,//�û���Ϣ��ѯ
                        case CEnum.Message_Tag_ID.O2JAM2_CHARACTERINFO_QUERY_RESP://= 0x938003,
                        case CEnum.Message_Tag_ID.O2JAM2_CHARACTERINFO_UPDATE://= 0x930004,//�û���Ϣ�޸�
                        case CEnum.Message_Tag_ID.O2JAM2_CHARACTERINFO_UPDATE_RESP://= 0x938004,
                        case CEnum.Message_Tag_ID.O2JAM2_ITEMSHOP_QUERY://= 0x930005,
                        case CEnum.Message_Tag_ID.O2JAM2_ITEMSHOP_QUERY_RESP://= 0x938005,
                        case CEnum.Message_Tag_ID.O2JAM2_ITEMSHOP_DELETE://= 0x930006,
                        case CEnum.Message_Tag_ID.O2JAM2_ITEMSHOP_DELETE_RESP://= 0x938006,
                        case CEnum.Message_Tag_ID.O2JAM2_MESSAGE_QUERY://= 0x930007,
                        case CEnum.Message_Tag_ID.O2JAM2_MESSAGE_QUERY_RESP://= 0x938007,
                        case CEnum.Message_Tag_ID.O2JAM2_MESSAGE_CREATE://= 0x930008,
                        case CEnum.Message_Tag_ID.O2JAM2_MESSAGE_CREATE_RESP://= 0x938008,
                        case CEnum.Message_Tag_ID.O2JAM2_MESSAGE_DELETE://= 0x930009,
                        case CEnum.Message_Tag_ID.O2JAM2_MESSAGE_DELETE_RESP://= 0x938009,
                        case CEnum.Message_Tag_ID.O2JAM2_CONSUME_QUERY://= 0x930010,
                        case CEnum.Message_Tag_ID.O2JAM2_CONUMSE_QUERY_RESP://= 0x938010,
                        case CEnum.Message_Tag_ID.O2JAM2_CONSUME_SUM_QUERY://= 0x930011,
                        case CEnum.Message_Tag_ID.O2JAM2_CONUMSE_QUERY_SUM_RESP://= 0x938011,
                        case CEnum.Message_Tag_ID.O2JAM2_TRADE_QUERY://= 0x930012,
                        case CEnum.Message_Tag_ID.O2JAM2_TRADE_QUERY_RESP://= 0x938012,
                        case CEnum.Message_Tag_ID.O2JAM2_TRADE_SUM_QUERY://= 0x930013,
                        case CEnum.Message_Tag_ID.O2JAM2_TRADE_QUERY_SUM_RESP://= 0x938013,
                        case CEnum.Message_Tag_ID.O2JAM2_AVATORLIST_QUERY:// = 0x930014,
                        case CEnum.Message_Tag_ID.O2JAM2_AVATORLIST_QUERY_RESP:// = 0x938014,


                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_QUERY:// = 0x870052,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_QUERY_RESP:// = 0x878052,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_INSERT:// = 0x870053,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_INSERT_RESP:// = 0x878053,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_UPDATE:// = 0x870054,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_UPDATE_RESP:// = 0x878054,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_DELETE:// = 0x870055,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_DELETE_RESP:// = 0x878055,
                        case CEnum.Message_Tag_ID.SDO_MUSICDATA_QUERY:// = 0x870056,
                        case CEnum.Message_Tag_ID.SDO_MUSICDATA_QUERY_RESP:// = 0x878056,

                        case CEnum.Message_Tag_ID.SDO_MUSICDATA_OWN_QUERY:// = 0x870057,
                        case CEnum.Message_Tag_ID.SDO_MUSICDATA_OWN_QUERY_RESP:// = 0x878057,

                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_QUERY:// = 0x870058,
		                case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_QUERY_RESP://  = 0x878058,
		                case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_CREATE://  = 0x870059,
		                case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_CREATE_RESP://  = 0x878059,
		                case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_UPDATE://  = 0x870060,
		                case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_UPDATE_RESP://  = 0x878060,
		                case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_DELETE://  = 0x870061,
                        case CEnum.Message_Tag_ID.SDO_CHALLENGE_SCENE_DELETE_RESP://  = 0x878061,
                        case CEnum.Message_Tag_ID.SDO_StageAward_Query:
                        case CEnum.Message_Tag_ID.SDO_StageAward_Query_RESP:
                        case CEnum.Message_Tag_ID.SDO_StageAward_Create:	
                        case CEnum.Message_Tag_ID.SDO_StageAward_Create_RESP:
                        case CEnum.Message_Tag_ID.SDO_StageAward_Delete:
                        case CEnum.Message_Tag_ID.SDO_StageAward_Delete_RESP:
                        case CEnum.Message_Tag_ID.SDO_StageAward_Update:
                        case CEnum.Message_Tag_ID.SDO_StageAward_Update_RESP:

                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_CREATE:// = 0x870062://,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_CREATE_RESP:// = 0x878062://,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_UPDATE:// = 0x870063://,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_UPDATE_RESP:// = 0x878063://,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_DELETE:// = 0x870064://,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_DELETE_RESP:// = 0x878064://,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_QUERY:// = 0x870065,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_QUERY_RESP:// = 0x878065,

                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_OWNER_QUERY:// = 0x870066,
                        case CEnum.Message_Tag_ID.SDO_MEDALITEM_OWNER_QUERY_RESP:// = 0x878066,

                        case CEnum.Message_Tag_ID.SDO_MEMBERDANCE_OPEN:// = 0x870067,
                        case CEnum.Message_Tag_ID.SDO_MEMBERDANCE_OPEN_RESP:// = 0x878067,
                        case CEnum.Message_Tag_ID.SDO_MEMBERDANCE_CLOSE:// = 0x870068,
                        case CEnum.Message_Tag_ID.SDO_MEMBERDANCE_CLOSE_RESP:// = 0x878068,

                        case CEnum.Message_Tag_ID.SDO_PADKEYWORD_QUERY:// = 0x870070,
                        case CEnum.Message_Tag_ID.SDO_PADKEYWORD_QUERY_RESP:// = 0x878070,
                        case CEnum.Message_Tag_ID.SDO_BOARDTASK_QUERY:
                        case CEnum.Message_Tag_ID.SDO_BOARDTASK_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SDO_BOARDTASK_UPDATE:
                        case CEnum.Message_Tag_ID.SDO_BOARDTASK_UPDATE_RESP:
                        case CEnum.Message_Tag_ID.SDO_USERINTEGRAL_QUERY:
                        case CEnum.Message_Tag_ID.SDO_USERINTEGRAL_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_QUERY:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_CREATE :
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_CREATE_RESP:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_UPDATE:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_UPDATE_RESP:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_DELETE:
                        case CEnum.Message_Tag_ID.SDO_YYHAPPYITEM_DELETE_RESP:
            		    case CEnum.Message_Tag_ID.SDO_PetInfo_Query:
                        case CEnum.Message_Tag_ID.SDO_PetInfo_Query_RESP:
                        case CEnum.Message_Tag_ID.SDO_BAOXIANGRate_Query:
                        case CEnum.Message_Tag_ID.SDO_BAOXIANGRate_Query_RESP:
                        case CEnum.Message_Tag_ID.SDO_BAOXIANGRate_Update:
                        case CEnum.Message_Tag_ID.SDO_BAOXIANGRate_Update_RESP:
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNT_CLOSE:// = 0x930015,//��ͣ�ʻ���Ȩ����Ϣ
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNT_CLOSE_RESP:// = 0x938015,//��ͣ�ʻ���Ȩ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNT_OPEN:// = 0x930016,//����ʻ���Ȩ����Ϣ
                        case CEnum.Message_Tag_ID.O2JAM2_ACCOUNT_OPEN_RESP:// = 0x938016,//����ʻ���Ȩ����Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.O2JAM2_MEMBERBANISHMENT_QUERY:
                        case CEnum.Message_Tag_ID.O2JAM2_MEMBERBANISHMENT_QUERY_RESP:
                        case CEnum.Message_Tag_ID.O2JAM2_MEMBERSTOPSTATUS_QUERY:
                        case CEnum.Message_Tag_ID.O2JAM2_MEMBERSTOPSTATUS_QUERY_RESP:
                        case CEnum.Message_Tag_ID.O2JAM2_MEMBERLOCAL_BANISHMENT:
                        case CEnum.Message_Tag_ID.O2JAM2_MEMBERLOCAL_BANISHMENT_RESP:
                        case CEnum.Message_Tag_ID.O2JAM2_USERLOGIN_DELETE:
                        case CEnum.Message_Tag_ID.O2JAM2_USERLOGIN_DELETE_RESP:

                        case CEnum.Message_Tag_ID.SOCCER_CHARACTERINFO_QUERY:
                        case CEnum.Message_Tag_ID.SOCCER_CHARACTERINFO_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SOCCER_CHARCHECK_QUERY:
                        case CEnum.Message_Tag_ID.SOCCER_CHARCHECK_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SOCCER_CHARITEMS_RECOVERY_QUERY:
                        case CEnum.Message_Tag_ID.SOCCER_CCHARITEMS_RECOVERY_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SOCCER_CHARPOINT_QUERY:
                        case CEnum.Message_Tag_ID.SOCCER_CHARPOINT_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SOCCER_DELETEDCHARACTERINFO_QUERY:
                        case CEnum.Message_Tag_ID.SOCCER_DELETEDCHARACTERINFO_QUERY_RESP:
                        case CEnum.Message_Tag_ID.SOCCER_CHARACTERSTATE_MODIFY://ͣ���ɫ
                        case CEnum.Message_Tag_ID.SOCCER_CHARACTERSTATE_MODIFY_RESP:
                        case CEnum.Message_Tag_ID.SOCCER_ACCOUNTSTATE_MODIFY ://ͣ�����
                        case CEnum.Message_Tag_ID.SOCCER_ACCOUNTSTATE_MODIFY_RESP :
                        case CEnum.Message_Tag_ID.SOCCER_CHARACTERSTATE_QUERY ://ͣ���ɫ��ѯ
                        case CEnum.Message_Tag_ID.SOCCER_CHARACTERSTATE_QUERY_RESP :
                        case CEnum.Message_Tag_ID.SOCCER_ACCOUNTSTATE_QUERY ://ͣ����Ҳ�ѯ
                        case CEnum.Message_Tag_ID.SOCCER_ACCOUNTSTATE_QUERY_RESP: 
                        case CEnum.Message_Tag_ID.NOTDEFINED:// 0x0:

                        #region �Ҵ�
                        /// <summary>
                        ///�Ҵ�
                        /// </summary>
                        case CEnum.Message_Tag_ID.SD_ActiveCode_Query:
                        case CEnum.Message_Tag_ID.SD_ActiveCode_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Account_Query://�ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.SD_Account_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_UserIteminfo_Query://�û�������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_UserIteminfo_Query_Resp:

                        case CEnum.Message_Tag_ID.SD_UserLoginfo_Query://�û���½��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_UserLoginfo_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_UserMailinfo_Query://�û��ʼ���Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_UserMailinfo_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_UserGuildinfo_Query://�û�������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_UserGuildinfo_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_UserStorageinfo_Query://�û��ֿ���Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_UserStorageinfo_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_UserAdditem_Add://��ӵ���
                        case CEnum.Message_Tag_ID.SD_UserAdditem_Add_Resp:
                        case CEnum.Message_Tag_ID.SD_UserAdditem_Del://ɾ������
                        case CEnum.Message_Tag_ID.SD_UserAdditem_Del_Resp:
                        case CEnum.Message_Tag_ID.SD_BanUser_Query://��ѯ��ͣ�û�
                        case CEnum.Message_Tag_ID.SD_BanUser_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_BanUser_Ban://��ͣ�û�
                        case CEnum.Message_Tag_ID.SD_BanUser_Ban_Resp:
                        case CEnum.Message_Tag_ID.SD_BanUser_UnBan://����û�
                        case CEnum.Message_Tag_ID.SD_BanUser_UnBan_Resp:
                        case CEnum.Message_Tag_ID.SD_AccountDetail_Query://�ʺ���ϸ��Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_AccountDetail_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Item_MixTree_Query: //��һ������
                        case CEnum.Message_Tag_ID.SD_Item_MixTree_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Item_UserUnits_Query:	//��һ�����Ϣ
                        case CEnum.Message_Tag_ID.SD_Item_UserUnits_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Item_UserCombatitems_Query:	//���ս������
                        case CEnum.Message_Tag_ID.SD_Item_UserCombatitems_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Item_Operator_Query://��Ҹ��ٵ���
                        case CEnum.Message_Tag_ID.SD_Item_Operator_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Item_Paint_Query://���Ϳ�ϵ���
                        case CEnum.Message_Tag_ID.SD_Item_Paint_Query_Resp:
                        case CEnum.Message_Tag_ID.SD_Item_Skill_Query://��Ҽ��ܵ���
                        case CEnum.Message_Tag_ID.SD_Item_Skill_Query_Resp://��Ҽ��ܵ���
                        case CEnum.Message_Tag_ID.SD_Item_Sticker_Query://��ұ�ǩ����
                        case CEnum.Message_Tag_ID.SD_Item_Sticker_Query_Resp://��ұ�ǩ����
                        case CEnum.Message_Tag_ID.SD_UserGrift_Query://������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_UserGrift_Query_Resp://������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_KickUser_Query://���û�����
                        case CEnum.Message_Tag_ID.SD_KickUser_Query_Resp://���û�����
                        case CEnum.Message_Tag_ID.SD_SendNotes_Query://���͹���
                        case CEnum.Message_Tag_ID.SD_SendNotes_Query_Resp://���͹���
                        case CEnum.Message_Tag_ID.SD_SeacrhNotes_Query://������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_SeacrhNotes_Query_Resp://������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_ItemType_Query://��ȡ��������
                        case CEnum.Message_Tag_ID.SD_ItemType_Query_Resp://��ȡ��������
                        case CEnum.Message_Tag_ID.SD_ItemList_Query://��ȡ�����б�
                        case CEnum.Message_Tag_ID.SD_ItemList_Query_Resp://��ȡ�����б�
                        case CEnum.Message_Tag_ID.SD_TmpPassWord_Query://��ʱ����
                        case CEnum.Message_Tag_ID.SD_TmpPassWord_Query_Resp://��ʱ����
                        case CEnum.Message_Tag_ID.SD_ReTmpPassWord_Query://�ָ���ʱ����
                        case CEnum.Message_Tag_ID.SD_ReTmpPassWord_Query_Resp://�ָ���ʱ����
                        case CEnum.Message_Tag_ID.SD_SearchPassWord_Query://��ѯ��ʱ����
                        case CEnum.Message_Tag_ID.SD_SearchPassWord_Query_Resp://��ѯ��ʱ����
                        case CEnum.Message_Tag_ID.SD_UpdateExp_Query://�޸ĵȼ�
                        case CEnum.Message_Tag_ID.SD_UpdateExp_Query_Resp://�޸ĵȼ�
                        case CEnum.Message_Tag_ID.SD_Grift_FromUser_Query://������������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_Grift_FromUser_Query_Resp://������������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_Grift_ToUser_Query://������������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_Grift_ToUser_Query_Resp://������������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.SD_TaskList_Update://�޸Ĺ���
                        case CEnum.Message_Tag_ID.SD_TaskList_Update_Resp://�޸Ĺ���
                        case CEnum.Message_Tag_ID.SD_Account_Active_Query://ͨ���ʺŲ�ѯ������Ϣ
                        case CEnum.Message_Tag_ID.SD_Account_Active_Query_Resp://ͨ���ʺŲ�ѯ������Ϣ

                        case CEnum.Message_Tag_ID.SD_BuyLog_Query://��ҹ������
                        case CEnum.Message_Tag_ID.SD_BuyLog_Query_Resp://��ҹ������
                        case CEnum.Message_Tag_ID.SD_Delete_ItemLog_Query://���ɾ�����߼�¼
                        case CEnum.Message_Tag_ID.SD_Delete_ItemLog_Query_Resp://���ɾ�����߼�¼
                        case CEnum.Message_Tag_ID.SD_LogInfo_Query://�����־��¼
                        case CEnum.Message_Tag_ID.SD_LogInfo_Query_Resp://�����־��¼
                        case CEnum.Message_Tag_ID.SD_Firend_Query://��Һ��Ѳ�ѯ
                        case CEnum.Message_Tag_ID.SD_Firend_Query_Resp://��Һ��Ѳ�ѯ
                        case CEnum.Message_Tag_ID.SD_UserRank_query://���������ѯ
                        case CEnum.Message_Tag_ID.SD_UserRank_query_Resp://���������ѯ
                        case CEnum.Message_Tag_ID.SD_UpdateUnitsExp_Query://�޸���һ���ȼ�
                        case CEnum.Message_Tag_ID.SD_UpdateUnitsExp_Query_Resp://�޸���һ���ȼ�
                        case CEnum.Message_Tag_ID.SD_UnitsItem_Query://��ѯ�������
                        case CEnum.Message_Tag_ID.SD_UnitsItem_Query_Resp://��ѯ�������
                        case CEnum.Message_Tag_ID.SD_GetGmAccount_Query://��ѯGM�˺�
                        case CEnum.Message_Tag_ID.SD_GetGmAccount_Query_Resp://��ѯGM�˺�
                        case CEnum.Message_Tag_ID.SD_UpdateGmAccount_Query://�޸�GM�˺�
                        case CEnum.Message_Tag_ID.SD_UpdateGmAccount_Query_Resp://�޸�GM�˺�
                        case CEnum.Message_Tag_ID.SD_UpdateMoney_Query://�޸�G��
                        case CEnum.Message_Tag_ID.SD_UpdateMoney_Query_Resp://�޸�G��
                        case CEnum.Message_Tag_ID.SD_Card_Info_Query://����/��ʯ����ѯ
                        case CEnum.Message_Tag_ID.SD_Card_Info_Query_Resp://����/��ʯ����ѯ
                        case CEnum.Message_Tag_ID.SD_UserAdditem_Add_All://������ӵ���
                        case CEnum.Message_Tag_ID.SD_UserAdditem_Add_All_Resp://������ӵ���
                        //case CEnum.Message_Tag_ID.SDO_Family_CONSUME_QUERY://�������Ѳ�ѯ
                        //case CEnum.Message_Tag_ID.SDO_Family_CONSUME_QUERY_RESP://�������Ѳ�ѯ
                        case CEnum.Message_Tag_ID.SD_ReGetUnits_Query://�ָ�����
                        case CEnum.Message_Tag_ID.SD_ReGetUnits_Query_Resp://�ָ�����
                        #endregion


                        #region ���쭳�
                        /// <summary>
                        ///���쭳�
                        /// </summary>
                        case CEnum.Message_Tag_ID.RC_Character_Query:
                        case CEnum.Message_Tag_ID.RC_Character_Query_Resp:
                        case CEnum.Message_Tag_ID.RC_UserLoginOut_Query:
                        case CEnum.Message_Tag_ID.RC_UserLoginOut_Query_Resp:
                        case CEnum.Message_Tag_ID.RC_UserLogin_Del:
                        case CEnum.Message_Tag_ID.RC_UserLogin_Del_Resp:
                        case CEnum.Message_Tag_ID.RC_RcCode_Query:
                        case CEnum.Message_Tag_ID.RC_RcCode_Query_Resp:
                        case CEnum.Message_Tag_ID.RC_RcUser_Query:
                        case CEnum.Message_Tag_ID.RC_RcUser_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_Character_Query:
                        case CEnum.Message_Tag_ID.RayCity_Character_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_CharacterState_Query:
                        case CEnum.Message_Tag_ID.RayCity_CharacterState_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_RaceState_Query:
                        case CEnum.Message_Tag_ID.RayCity_RaceState_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_InventoryList_Query:
                        case CEnum.Message_Tag_ID.RayCity_InventoryList_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_InventoryDetail_Query:
                        case CEnum.Message_Tag_ID.RayCity_InventoryDetail_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_CarList_Query:
                        case CEnum.Message_Tag_ID.RayCity_CarList_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_Guild_Query:
                        case CEnum.Message_Tag_ID.RayCity_Guild_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_QuestLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_QuestLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_MissionLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_MissionLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_DealLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_DealLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_FriendList_Query:
                        case CEnum.Message_Tag_ID.RayCity_FriendList_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BasicAccount_Query:
                        case CEnum.Message_Tag_ID.RayCity_BasicAccount_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_GuildMember_Query:
                        case CEnum.Message_Tag_ID.RayCity_GuildMember_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BasicCharacter_Query:
                        case CEnum.Message_Tag_ID.RayCity_BasicCharacter_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BuyCar_Query:
                        case CEnum.Message_Tag_ID.RayCity_BuyCar_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ConnectionLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_ConnectionLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_CarInventory_Query:
                        case CEnum.Message_Tag_ID.RayCity_CarInventory_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ConnectionState_Query:
                        case CEnum.Message_Tag_ID.RayCity_ConnectionState_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ItemShop_Insert:
                        case CEnum.Message_Tag_ID.RayCity_ItemShop_Insert_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ItemShop_Query:
                        case CEnum.Message_Tag_ID.RayCity_ItemShop_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_MoneyLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_MoneyLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_RaceLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_RaceLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_AddMoney:
                        case CEnum.Message_Tag_ID.RayCity_AddMoney_Resp:
                        case CEnum.Message_Tag_ID.RayCity_Skill_Query:
                        case CEnum.Message_Tag_ID.RayCity_Skill_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_PlayerSkill_Query:
                        case CEnum.Message_Tag_ID.RayCity_PlayerSkill_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_PlayerSkill_Delete:
                        case CEnum.Message_Tag_ID.RayCity_PlayerSkill_Delete_Resp:
                        case CEnum.Message_Tag_ID.RayCity_PlayerSkill_Insert:
                        case CEnum.Message_Tag_ID.RayCity_PlayerSkill_Insert_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ItemType_Query:
                        case CEnum.Message_Tag_ID.RayCity_ItemType_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_TradeInfo_Query:
                        case CEnum.Message_Tag_ID.RayCity_TradeInfo_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_TradeDetail_Query:
                        case CEnum.Message_Tag_ID.RayCity_TradeDetail_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_SetPos_Update:
                        case CEnum.Message_Tag_ID.RayCity_SetPos_Update_Resp:
                        case CEnum.Message_Tag_ID.RayCity_GMUser_Query:
                        case CEnum.Message_Tag_ID.RayCity_GMUser_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_GMUser_Update:
                        case CEnum.Message_Tag_ID.RayCity_GMUser_Update_Resp:
                        case CEnum.Message_Tag_ID.RayCity_PlayerAccount_Create:
                        case CEnum.Message_Tag_ID.RayCity_PlayerAccount_Create_Resp:
                        case CEnum.Message_Tag_ID.RayCity_WareHousePwd_Update:
                        case CEnum.Message_Tag_ID.RayCity_WareHousePwd_Update_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BingoCard_Query:
                        case CEnum.Message_Tag_ID.RayCity_BingoCard_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_UserCharge_Query:
                        case CEnum.Message_Tag_ID.RayCity_UserCharge_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ItemConsume_Query:
                        case CEnum.Message_Tag_ID.RayCity_ItemConsume_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_UserMails_Query:
                        case CEnum.Message_Tag_ID.RayCity_UserMails_Query_Resp:
                       case CEnum.Message_Tag_ID.RayCity_CashItemDetailLog_Query:
                        case CEnum.Message_Tag_ID.RayCity_CashItemDetailLog_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_Coupon_Query:
                        case CEnum.Message_Tag_ID.RayCity_Coupon_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_ActiveCard_Query:
                        case CEnum.Message_Tag_ID.RayCity_ActiveCard_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BoardList_Query:
                        case CEnum.Message_Tag_ID.RayCity_BoardList_Query_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BoardList_Insert:
                        case CEnum.Message_Tag_ID.RayCity_BoardList_Insert_Resp:
                        case CEnum.Message_Tag_ID.RayCity_BoardList_Delete:
                        case CEnum.Message_Tag_ID.RayCity_BoardList_Delete_Resp:
                      
                        #endregion

                        #region ���߷ɳ�
                        /// <summary>
                        ///���߷ɳ�
                        /// </summary>
                        
                        case CEnum.Message_Tag_ID.RC_Character_Update:
                        case CEnum.Message_Tag_ID.RC_Character_Update_Resp:
                        


                        #endregion


                        #region ������2

                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_QUERY://����ʺ���Ϣ��ѯ��1.2.3.4.5.8��
                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_QUERY_RESP://����ʺ���Ϣ��ѯ��Ӧ��1.2.3.4.5.8��
                        /////////////��ͣ��/////////////////////////////////////////
                        case CEnum.Message_Tag_ID.JW2_ACCOUNTREMOTE_QUERY://��Ϸ��������ͣ������ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.JW2_ACCOUNTREMOTE_QUERY_RESP://��Ϸ��������ͣ������ʺŲ�ѯ��Ӧ

                        case CEnum.Message_Tag_ID.JW2_ACCOUNTLOCAL_QUERY://���ط�ͣ������ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.JW2_ACCOUNTLOCAL_QUERY_RESP://���ط�ͣ������ʺŲ�ѯ��Ӧ

                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_CLOSE://��ͣ������ʺ�
                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_CLOSE_RESP://��ͣ������ʺ���Ӧ
                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_OPEN://��������ʺ�
                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_OPEN_RESP://��������ʺ���Ӧ
                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_BANISHMENT_QUERY://��ҷ�ͣ�ʺŲ�ѯ
                        case CEnum.Message_Tag_ID.JW2_ACCOUNT_BANISHMENT_QUERY_RESP://��ҷ�ͣ�ʺŲ�ѯ��Ӧ
                        ////////////////////////////
                        case CEnum.Message_Tag_ID.JW2_BANISHPLAYER://����
                        case CEnum.Message_Tag_ID.JW2_BANISHPLAYER_RESP://������Ӧ
                        case CEnum.Message_Tag_ID.JW2_BOARDMESSAGE://����
                        case CEnum.Message_Tag_ID.JW2_BOARDMESSAGE_RESP://������Ӧ

                        case CEnum.Message_Tag_ID.JW2_LOGINOUT_QUERY://����������/�ǳ���Ϣ
                        case CEnum.Message_Tag_ID.JW2_LOGINOUT_QUERY_RESP://����������/�ǳ���Ϣ��Ӧ
                        case CEnum.Message_Tag_ID.JW2_RPG_QUERY://��ѯ������ڣ�6��
                        case CEnum.Message_Tag_ID.JW2_RPG_QUERY_RESP://��ѯ���������Ӧ��6��

                        case CEnum.Message_Tag_ID.JW2_ITEMSHOP_BYOWNER_QUERY:////�鿴������ϵ�����Ϣ��7��7��
                        case CEnum.Message_Tag_ID.JW2_ITEMSHOP_BYOWNER_QUERY_RESP:////�鿴������ϵ�����Ϣ��Ӧ��7��7��


                        case CEnum.Message_Tag_ID.JW2_DUMMONEY_QUERY:////��ѯ����������ң�7��8��
                        case CEnum.Message_Tag_ID.JW2_DUMMONEY_QUERY_RESP:////��ѯ�������������Ӧ��7��8��
                        case CEnum.Message_Tag_ID.JW2_HOME_ITEM_QUERY:////�鿴������Ʒ�嵥�����ޣ�7��9��
                        case CEnum.Message_Tag_ID.JW2_HOME_ITEM_QUERY_RESP:////�鿴������Ʒ�嵥��������Ӧ��7��9��
                        case CEnum.Message_Tag_ID.JW2_SMALL_PRESENT_QUERY:////�鿴�������7��10��
                        case CEnum.Message_Tag_ID.JW2_SMALL_PRESENT_QUERY_RESP:////�鿴���������Ӧ��7��10��
                        case CEnum.Message_Tag_ID.JW2_WASTE_ITEM_QUERY:////�鿴�����Ե��ߣ�7��11��
                        case CEnum.Message_Tag_ID.JW2_WASTE_ITEM_QUERY_RESP:////�鿴�����Ե�����Ӧ��7��11��
                        case CEnum.Message_Tag_ID.JW2_SMALL_BUGLE_QUERY:////�鿴С���ȣ�7��12��
                        case CEnum.Message_Tag_ID.JW2_SMALL_BUGLE_QUERY_RESP:////�鿴С������Ӧ��7��12��

                        case CEnum.Message_Tag_ID.JW2_WEDDINGINFO_QUERY://������Ϣ
                        case CEnum.Message_Tag_ID.JW2_WEDDINGINFO_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_WEDDINGLOG_QUERY://������ʷ
                        case CEnum.Message_Tag_ID.JW2_WEDDINGLOG_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_WEDDINGGROUND_QUERY://�����Ϣ
                        case CEnum.Message_Tag_ID.JW2_WEDDINGGROUND_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_COUPLEINFO_QUERY://������Ϣ
                        case CEnum.Message_Tag_ID.JW2_COUPLEINFO_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_COUPLELOG_QUERY://������ʷ
                        case CEnum.Message_Tag_ID.JW2_COUPLELOG_QUERY_RESP:


                        case CEnum.Message_Tag_ID.JW2_FAMILYINFO_QUERY://������Ϣ
                        case CEnum.Message_Tag_ID.JW2_FAMILYINFO_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_FAMILYMEMBER_QUERY://�����Ա��Ϣ
                        case CEnum.Message_Tag_ID.JW2_FAMILYMEMBER_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_SHOP_QUERY://�̳���Ϣ
                        case CEnum.Message_Tag_ID.JW2_SHOP_QUERY_RESP:
                        case CEnum.Message_Tag_ID.JW2_User_Family_Query://�û�������Ϣ
                        case CEnum.Message_Tag_ID.JW2_User_Family_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_FamilyItemInfo_Query://���������Ϣ
                        case CEnum.Message_Tag_ID.JW2_FamilyItemInfo_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_FamilyBuyLog_Query://���幺����־
                        case CEnum.Message_Tag_ID.JW2_FamilyBuyLog_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_FamilyTransfer_Query://����ת����Ϣ
                        case CEnum.Message_Tag_ID.JW2_FamilyTransfer_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_FriendList_Query://�����б�
                        case CEnum.Message_Tag_ID.JW2_FriendList_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_BasicInfo_Query://������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.JW2_BasicInfo_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_FamilyMember_Applying://�����г�Ա
                        case CEnum.Message_Tag_ID.JW2_FamilyMember_Applying_Resp:

                        case CEnum.Message_Tag_ID.JW2_BasicRank_Query://���صȼ�
                        case CEnum.Message_Tag_ID.JW2_BasicBank_Query_Resp:


                        ///////////����////////////////////////////
                        case CEnum.Message_Tag_ID.JW2_BOARDTASK_INSERT://�������
                        case CEnum.Message_Tag_ID.JW2_BOARDTASK_INSERT_RESP://���������Ӧ
                        case CEnum.Message_Tag_ID.JW2_BOARDTASK_QUERY://�����ѯ
                        case CEnum.Message_Tag_ID.JW2_BOARDTASK_QUERY_RESP://�����ѯ��Ӧ
                        case CEnum.Message_Tag_ID.JW2_BOARDTASK_UPDATE://�������
                        case CEnum.Message_Tag_ID.JW2_BOARDTASK_UPDATE_RESP://���������Ӧ

                        case CEnum.Message_Tag_ID.JW2_ITEM_DEL://����ɾ��������0����Ʒ��1�����2��
                        case CEnum.Message_Tag_ID.JW2_ITEM_DEL_RESP://����ɾ��������0����Ʒ��1�����2��

                        case CEnum.Message_Tag_ID.JW2_MODIFYSEX_QUERY://�޸Ľ�ɫ�Ա�
                        case CEnum.Message_Tag_ID.JW2_MODIFYSEX_QUERY_RESP:

                        case CEnum.Message_Tag_ID.JW2_MODIFYLEVEL_QUERY://�޸ĵȼ�
                        case CEnum.Message_Tag_ID.JW2_MODIFYLEVEL_QUERY_RESP:

                        case CEnum.Message_Tag_ID.JW2_MODIFYEXP_QUERY://�޸ľ���
                        case CEnum.Message_Tag_ID.JW2_MODIFYEXP_QUERY_RESP:

                        case CEnum.Message_Tag_ID.JW2_BAN_BIGBUGLE://���ô�����
                        case CEnum.Message_Tag_ID.JW2_BAN_BIGBUGLE_RESP:

                        case CEnum.Message_Tag_ID.JW2_BAN_SMALLBUGLE://����С����
                        case CEnum.Message_Tag_ID.JW2_BAN_SMALLBUGLE_RESP:

                        case CEnum.Message_Tag_ID.JW2_DEL_CHARACTER://ɾ����ɫ
                        case CEnum.Message_Tag_ID.JW2_DEL_CHARACTER_RESP:

                        case CEnum.Message_Tag_ID.JW2_RECALL_CHARACTER://�ָ���ɫ
                        case CEnum.Message_Tag_ID.JW2_RECALL_CHARACTER_RESP:

                        case CEnum.Message_Tag_ID.JW2_MODIFY_MONEY://�޸Ľ�Ǯ
                        case CEnum.Message_Tag_ID.JW2_MODIFY_MONEY_RESP:

                        case CEnum.Message_Tag_ID.JW2_ADD_ITEM://���ӵ���
                        case CEnum.Message_Tag_ID.JW2_ADD_ITEM_RESP:

                        case CEnum.Message_Tag_ID.JW2_CREATE_GM://ÿ����������GM
                        case CEnum.Message_Tag_ID.JW2_CREATE_GM_RESP:

                        case CEnum.Message_Tag_ID.JW2_MODIFY_PWD://�޸�����
                        case CEnum.Message_Tag_ID.JW2_MODIFY_PWD_RESP:

                        case CEnum.Message_Tag_ID.JW2_RECALL_PWD://�ָ�����
                        case CEnum.Message_Tag_ID.JW2_RECALL_PWD_RESP:


                        case CEnum.Message_Tag_ID.JW2_ItemInfo_Query:
                        case CEnum.Message_Tag_ID.JW2_ItemInfo_Query_Resp://


                        case CEnum.Message_Tag_ID.JW2_ITEM_SELECT://����ģ����ѯ
                        case CEnum.Message_Tag_ID.JW2_ITEM_SELECT_RESP://

                        case CEnum.Message_Tag_ID.JW2_PetInfo_Query://������Ϣ
                        case CEnum.Message_Tag_ID.JW2_PetInfo_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_Messenger_Query://�ƺŲ�ѯ
                        case CEnum.Message_Tag_ID.JW2_Messenger_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_Wedding_Paper://���֤��
                        case CEnum.Message_Tag_ID.JW2_Wedding_Paper_Resp:

                        case CEnum.Message_Tag_ID.JW2_CoupleParty_Card://�����ɶԿ�
                        case CEnum.Message_Tag_ID.JW2_CoupleParty_Card_Resp:


                        case CEnum.Message_Tag_ID.JW2_MailInfo_Query://ֽ����Ϣ
                        case CEnum.Message_Tag_ID.JW2_MailInfo_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_MoneyLog_Query://��Ǯ��־��ѯ
                        case CEnum.Message_Tag_ID.JW2_MoneyLog_Query_Resp:

                        case CEnum.Message_Tag_ID.JW2_FamilyFund_Log://������־
                        case CEnum.Message_Tag_ID.JW2_FamilyFund_Log_Resp:

                        case CEnum.Message_Tag_ID.JW2_FamilyItem_Log://���������־
                        case CEnum.Message_Tag_ID.JW2_FamilyItem_Log_Resp:

                        case CEnum.Message_Tag_ID.JW2_Item_Log://������־
                        case CEnum.Message_Tag_ID.JW2_Item_Log_Resp:


                        case CEnum.Message_Tag_ID.JW2_ADD_ITEM_ALL://���ӵ���(����)
                        case CEnum.Message_Tag_ID.JW2_ADD_ITEM_ALL_RESP:

                        case CEnum.Message_Tag_ID.JW2_CashMoney_Log://������־
                        case CEnum.Message_Tag_ID.JW2_CashMoney_Log_Resp:


                        case CEnum.Message_Tag_ID.JW2_SearchPassWord_Query://��ѯ��ʱ����
                        case CEnum.Message_Tag_ID.JW2_SearchPassWord_Query_Resp://��ѯ��ʱ����

                        case CEnum.Message_Tag_ID.JW2_CenterAvAtarItem_Bag_Query://�м䱳������
                        case CEnum.Message_Tag_ID.JW2_CenterAvAtarItem_Bag_Query_Resp://�м䱳������

                        case CEnum.Message_Tag_ID.JW2_CenterAvAtarItem_Equip_Query://�м����ϵ���
                        case CEnum.Message_Tag_ID.JW2_CenterAvAtarItem_Equip_Query_Resp://�м����ϵ���

                        case CEnum.Message_Tag_ID.JW2_House_Query://С����
                        case CEnum.Message_Tag_ID.JW2_House_Query_Resp://С����

                        case CEnum.Message_Tag_ID.JW2_GM_Update://GM��B�޸�
                        case CEnum.Message_Tag_ID.JW2_GM_Update_Resp://GM��B�޸�
                        case CEnum.Message_Tag_ID.JW2_JB_Query://�e����Ϣ��?
                        case CEnum.Message_Tag_ID.JW2_JB_Query_Resp://�e����Ϣ��?

                        case CEnum.Message_Tag_ID.JW2_UpDateFamilyName_Query://�޸ļ�����
                        case CEnum.Message_Tag_ID.JW2_UpDateFamilyName_Query_Resp://�޸ļ�����

                        case CEnum.Message_Tag_ID.JW2_UpdatePetName_Query://�޸�?����
                        case CEnum.Message_Tag_ID.JW2_UpdatePetName_Query_Resp://�޸�?����

                        case CEnum.Message_Tag_ID.JW2_Act_Card_Query://�����ѯ
                        case CEnum.Message_Tag_ID.JW2_Act_Card_Query_Resp://�����ѯ

                        case CEnum.Message_Tag_ID.JW2_Center_Kick_Query://���g������
                        case CEnum.Message_Tag_ID.JW2_Center_Kick_Query_Resp://���g������


                        case CEnum.Message_Tag_ID.JW2_ChangeServerExp_Query://�޸ķ�����??����
                        case CEnum.Message_Tag_ID.JW2_ChangeServerExp_Query_Resp://�޸ķ�����??����

                        case CEnum.Message_Tag_ID.JW2_ChangeServerMoney_Query://�޸ķ�������Ǯ����
                        case CEnum.Message_Tag_ID.JW2_ChangeServerMoney_Query_Resp://�޸ķ�������Ǯ����

                        case CEnum.Message_Tag_ID.JW2_MissionInfoLog_Query://����LOG��ѯ
                        case CEnum.Message_Tag_ID.JW2_MissionInfoLog_Query_Resp://����LOG��ѯ

                        case CEnum.Message_Tag_ID.JW2_AgainBuy_Query://�ظ������˿�
                        case CEnum.Message_Tag_ID.JW2_AgainBuy_Query_Resp://�ظ������˿�

                        case CEnum.Message_Tag_ID.JW2_GSSvererList_Query://�������б�GS
                        case CEnum.Message_Tag_ID.JW2_GSSvererList_Query_Resp://�������б�GS

                        case CEnum.Message_Tag_ID.JW2_Materiallist_Query://�Ñ��ϳɲ��ϲ�ѯ
                        case CEnum.Message_Tag_ID.JW2_Materiallist_Query_Resp://�Ñ��ϳɲ��ϲ�ѯ

                        case CEnum.Message_Tag_ID.JW2_MaterialHistory_Query://�Ñ��ϳɼ�¼
                        case CEnum.Message_Tag_ID.JW2_MaterialHistory_Query_Resp://�Ñ��ϳɼ�¼

                        case CEnum.Message_Tag_ID.JW2_ACTIVEPOINT_QUERY://��Ծ�Ȳ�ѯ	
                        case CEnum.Message_Tag_ID.JW2_ACTIVEPOINT_QUERY_Resp://��Ծ�Ȳ�ѯ

                        case CEnum.Message_Tag_ID.JW2_GETPIC_Query://�����Ҫ��˵�ͼƬ�б�
                        case CEnum.Message_Tag_ID.JW2_GETPIC_Query_Resp://�����Ҫ��˵�ͼƬ�б�

                        case CEnum.Message_Tag_ID.JW2_CHKPIC_Query://���ͼƬ 2���ͨ����3��˲�ͨ�� 
                        case CEnum.Message_Tag_ID.JW2_CHKPIC_Query_Resp://���ͼƬ 2���ͨ����3��˲�ͨ��

                        case CEnum.Message_Tag_ID.JW2_PicCard_Query://�DƬ�ς���ʹ��
                        case CEnum.Message_Tag_ID.JW2_PicCard_Query_Resp://�DƬ�ς���ʹ��

                        case CEnum.Message_Tag_ID.JW2_FamilyPet_Query://��������ѯ
                        case CEnum.Message_Tag_ID.JW2_FamilyPet_Query_Resp://��������ѯ


                        case CEnum.Message_Tag_ID.JW2_BuyPetAgg_Query://�峤������ﵰ��ѯ
                        case CEnum.Message_Tag_ID.JW2_BuyPetAgg_Query_Resp://�峤������ﵰ��ѯ

                        case CEnum.Message_Tag_ID.JW2_PetFirend_Query://������ｻ�Ѳ�ѯ
                        case CEnum.Message_Tag_ID.JW2_PetFirend_Query_Resp://������ｻ�Ѳ�ѯ

                        case CEnum.Message_Tag_ID.JW2_SmallPetAgg_Query://�����Ա��ȡС������Ϣ��ѯ
                        case CEnum.Message_Tag_ID.JW2_SmallPetAgg_Query_Resp://�����Ա��ȡС������Ϣ��ѯ
                        #endregion


                        case CEnum.Message_Tag_ID.ERROR:// 0xFFFFFF:

						#region ���������к��ֶ�
						int iCount = 0;		//�ظ�����

						System.Collections.ArrayList t_StructCount = mPacketbody.m_TLVList;
						System.Collections.ArrayList t_StructUsed = (System.Collections.ArrayList)t_StructCount.Clone();

						for (int i=0; i<t_StructCount.Count; i++)
						{
							for (int j=i+1; j<t_StructCount.Count; j++)
							{
								if (((TLV_Structure)t_StructCount[i]).m_Tag != ((TLV_Structure)t_StructCount[j]).m_Tag)
								{
									iCount++;
									t_StructCount.RemoveAt(j);
									j--;
								}
							}
						}

						//�޼�¼�ж�
						if (t_StructCount.Count == 0)
						{
                            p_ReturnBody = new CEnum.Message_Body[1, 1];
                            p_ReturnBody[0, 0].eName = CEnum.TagName.ERROR_Msg;
                            p_ReturnBody[0, 0].eTag = CEnum.TagFormat.TLV_STRING;
							p_ReturnBody[0,0].oContent = "������!";

							return p_ReturnBody;
						}
						iField = t_StructUsed.Count / t_StructCount.Count;
						#endregion

                        p_ReturnBody = new CEnum.Message_Body[t_StructUsed.Count - iCount, iField];

						#region ��Ϣ���� -- ������Ϣ
						int iTemp = 0;
						for (int i=0; i<t_StructCount.Count; i++)
						{
							for (int j=i*iField; j<t_StructUsed.Count; j++)
							{								
								//�����ֶα�ǩ
								if (iTemp == iField)
								{
									iTemp = 0;
									break;
								}
								
								TLV_Structure mStruct = (TLV_Structure)t_StructUsed[j];

								p_ReturnBody[i,iTemp].eName = mStruct.m_Tag;
								p_ReturnBody = DecodeRecive(i, iTemp, mStruct, p_ReturnBody);

								iTemp++;
							}
						}
						#endregion						
						
						break;
				}
				#endregion

				return p_ReturnBody;
			}
			catch (Exception e)
			{
                p_ReturnBody = new CEnum.Message_Body[1, 1];
                p_ReturnBody[0, 0].eName = CEnum.TagName.ERROR_Msg;
                p_ReturnBody[0, 0].eTag = CEnum.TagFormat.TLV_STRING;
				p_ReturnBody[0,0].oContent = "���ݽ����쳣";

                CEnum.TLogData tLogData = new CEnum.TLogData();

				tLogData.iSort = 5;
				tLogData.strDescribe = "����Socket��Ϣʧ��!";
				tLogData.strException = e.Message;

				return p_ReturnBody;
			}
		}
		#endregion
	}
}
