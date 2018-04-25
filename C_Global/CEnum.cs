  using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace C_Global
{
    public class CEnum
    {
        #region Socket ��Ϣ����
        /// <summary>
        /// �������ݱ�ǩ
        /// </summary>
        public enum TagName : ushort
        {
            ///////////////////////////////////////////////////////////////////////
            Magic_Dates = 0x5736,//����
            UserName = 0x0101, //Format:STRING �û���
            PassWord = 0x0102, //Format:STRING ����
            MAC = 0x0103, //Format:STRING  MAC��
            Limit = 0x0104,//Format:DateTime GM�ʺ�ʹ��ʱЧ
            User_Status = 0x0105,//Format:INT ״̬��Ϣ
            UserByID = 0x0106,//Format:INT ����ԱID
            RealName = 0x0107,//Format:STRING ������
            DepartID = 0x0108,//Format:INT ����ID
            DepartName = 0x0109,//Format:STRING ��������
            DepartRemark = 0x0110,//Format:STRING ��������
            OnlineActive = 0x0111,//Format:Integer ����״̬
            UpdateFileName = 0x0112,//Format:String �ļ���
            UpdateFileVersion = 0x0113,//Format:String �ļ��汾
            UpdateFilePath = 0x0114,//Format:String �ļ�·��
            UpdateFileSize = 0x0115,//Format:Integer �ļ���С

            Process_Reason = 0x060B,//Format tring
            ///////////////////////////////////////////////////////////////////////
            GameID = 0x0200, //Format:INTEGER ��ϢID
            ModuleName = 0x0201, //Format:STRING ģ������
            ModuleClass = 0x0202, //Format:STRING ģ�����
            ModuleContent = 0x0203, //Format:STRING ģ������
            ///////////////////////////////////////////////////////////////////////
            Module_ID = 0x0301, //Format:INTEGER ģ��ID
            User_ID = 0x0302, //Format:INTEGER �û�ID
            ModuleList = 0x0303, //Format:String ģ���б�
            SysAdmin = 0x0116,//Format:Integer �Ƿ���ϵͳ����Ա
            ///////////////////////////////////////////////////////////////////////
            Host_Addr = 0x0401, //Format:STRING
            Host_Port = 0x0402, //Format:STRING
            Host_Pat = 0x0403,  //Format:STRING
            Conn_Time = 0x0404, //Format:DateTime �������Ӧʱ��
            Connect_Msg = 0x0405,//Format:STRING ����������Ϣ
            DisConnect_Msg = 0x0406,//Format:STRING	 ����˿���Ϣ
            Author_Msg = 0x0407, //Format:STRING ��֤�û�����Ϣ
            Status = 0x0408,//Format:STRING �������
            Index = 0x0409, //Format:Integer ��¼�����
            PageSize = 0x0410,//Format:Integer ��¼ҳ��ʾ����
            PageCount = 0x0411,//Format:Integer ��ʾ��ҳ��
            SP_Name = 0x0412,//Format:Integer �洢������
            Real_ACT = 0x0413,//Format:String ����������
            ACT_Time = 0x0414,//Format:TimeStamp ����ʱ��
            BeginTime = 0x0415,//Format:Date ��ʼ����
            EndTime = 0x0416,//Format:Date ��������
            ///////////////////////////////////////////////////////////////////////
            GameName = 0x0501, //Format:STRING ��Ϸ����
            GameContent = 0x0502, //Format:STRING ��Ϣ����
            ///////////////////////////////////////////////////////////////////////
            Letter_ID = 0x0601, //Format:Integer 
            Letter_Sender = 0x0602, //Format:String
            Letter_Receiver = 0x0603, //Format:String
            Letter_Subject = 0x0604, //Format:String
            Letter_Text = 0x0605, //Format:String
            Send_Date = 0x0606, //Format:Date
            Process_Man = 0x0607, //Format:Integer
            Process_Date = 0x0608, //Format:Date
            Transmit_Man = 0x0609, //Format:Integer
            Is_Process = 0x060A, //Format:Integer
            ///////////////////////////////////////////////////////////////////////
            MJ_Level = 0x0701, //Format:Integer ��ҵȼ�
            MJ_Account = 0x0702, //Format:String ����ʺ�
            MJ_CharName = 0x0703, //Format:String ����س�
            MJ_Exp = 0x0704, //Format:Integer ��ҵ�ǰ����
            MJ_Exp_Next_Level = 0x0705, //Format:Integer ����´������ľ��� 
            MJ_HP = 0x0706, //Format:Integer ���HPֵ
            MJ_HP_Max = 0x0707, //Format:Integer �������HPֵ
            MJ_MP = 0x0708, //Format:Integer ���MPֵ
            MJ_MP_Max = 0x0709, //Format:Integer �������MPֵ
            MJ_DP = 0x0710, //Format:Integer ���DPֵ
            MJ_DP_Increase_Ratio = 0x0711, //Format:Integer �������DPֵ
            MJ_Exception_Dodge = 0x0712, //Format:Integer �쳣״̬�ر�
            MJ_Exception_Recovery = 0x0713, //Format:Integer �쳣״̬�ظ�
            MJ_Physical_Ability_Max = 0x0714, //Format:Integer �����������ֵ
            MJ_Physical_Ability_Min = 0x0715, //Format:Integer ����������Сֵ
            MJ_Magic_Ability_Max = 0x0716, //Format:Integer ħ���������ֵ
            MJ_Magic_Ability_Min = 0x0717, //Format:Integer ħ��������Сֵ
            MJ_Tao_Ability_Max = 0x0718, //Format:Integer �����������ֵ
            MJ_Tao_Ability_Min = 0x0719, //Format:Integer ����������Сֵ
            MJ_Physical_Defend_Max = 0x0720, //Format:Integer ������ֵ
            MJ_Physical_Defend_Min = 0x0721, //Format:Integer �����Сֵ
            MJ_Magic_Defend_Max = 0x0722, //Format:Integer ħ�����ֵ
            MJ_Magic_Defend_Min = 0x0723, //Format:Integer ħ����Сֵ
            MJ_Accuracy = 0x0724, //Format:Integer ������
            MJ_Phisical_Dodge = 0x0725, //Format:Integer ����ر���
            MJ_Magic_Dodge = 0x0726, //Format:Integer ħ���ر���
            MJ_Move_Speed = 0x0727, //Format:Integer �ƶ��ٶ�
            MJ_Attack_speed = 0x0728, //Format:Integer �����ٶ�
            MJ_Max_Beibao = 0x0729, //Format:Integer ��������
            MJ_Max_Wanli = 0x0730, //Format:Integer ��������
            MJ_Max_Fuzhong = 0x0731, //Format:Integer ��������
            MJ_PASSWD = 0x0732,//Format:String �������
            MJ_ServerIP = 0x0733,//Format:String ������ڷ�����
            MJ_TongID = 0x0734,//Format:Integer ���ID
            MJ_TongName = 0x0735,//Format:String �������
            MJ_TongLevel = 0x0736,//Format:Integer ���ȼ�
            MJ_TongMemberCount = 0x0737,//Format:Integer �������
            MJ_Money = 0x0738,//Format:Money ��ҽ�Ǯ
            MJ_TypeID = 0x0739,//Format:Integer ��ҽ�ɫ����ID
            MJ_ActionType = 0x0740,//Format:Integer ���ID
            MJ_Time = 0x0741,//Format:TimeStamp  ����ʱ��
            MJ_CharIndex = 0x0742,//���������
            MJ_CharName_Prefix = 0x0743,//��Ұ������
            MJ_Exploit_Value = 0x0744,//��ҹ�ѫֵ
            MJ_Reason = 0x0745,//ͣ������

            ///////////////////////////////////////////////////////////////////////
            SDO_ServerIP = 0x0801,//Format:String ����IP
            SDO_UserIndexID = 0x0802,//Format:Integer ����û�ID
            SDO_Account = 0x0803,//Format:String ��ҵ��ʺ�
            SDO_Level = 0x0804,//Format:Integer ��ҵĵȼ�
            SDO_Exp = 0x0805,//Format:Integer ��ҵĵ�ǰ����ֵ
            SDO_GameTotal = 0x0806,//Format:Integer �ܾ���
            SDO_GameWin = 0x0807,//Format:Integer ʤ����
            SDO_DogFall = 0x0808,//Format:Integer ƽ����
            SDO_GameFall = 0x0809,//Format:Integer ������
            SDO_Reputation = 0x0810,//Format:Integer ����ֵ
            SDO_GCash = 0x0811,//Format:Integer G��
            SDO_MCash = 0x0812,//Format:Integer M��
            SDO_Address = 0x0813,//Format:Integer ��ַ
            SDO_Age = 0x0814,//Format:Integer ����
            SDO_ProductID = 0x0815,//Format:Integer ��Ʒ���
            SDO_ProductName = 0x0816,//Format:String ��Ʒ����
            SDO_ItemCode = 0x0817,//Format:Integer ���߱��
            SDO_ItemName = 0x0818,//Format:String ��������
            SDO_TimesLimit = 0x0819,//Format:Integer ʹ�ô���
            SDO_DateLimit = 0x0820,//Format:Integer ʹ��ʱЧ
            SDO_MoneyType = 0x0821,//Format:Integer ��������
            SDO_MoneyCost = 0x0822,//Format:Integer ���ߵļ۸�
            SDO_ShopTime = 0x0823,//Format:DateTime ����ʱ��
            SDO_MAINCH = 0x0824,//Format:Integer ������
            SDO_SUBCH = 0x0825,//Format:Integer ����
            SDO_Online = 0x0826,//Format:Integer �Ƿ�����
            SDO_LoginTime = 0x0827,//Format:DateTime ����ʱ��
            SDO_LogoutTime = 0x0828,//Format:DateTime ����ʱ��
            SDO_AREANAME = 0x0829,//Format:String ��������
            SDO_City = 0x0830,//Format:String �����ס����
            SDO_Title = 0x0831,//Format:String ��������
            SDO_Context = 0x0832,//Format:String ��������
            SDO_MinLevel = 0x0833,//Format:Integer �������ߵ���С�ȼ�
            SDO_ActiveStatus = 0x0834,//Format:Integer ����״̬
            SDO_StopStatus = 0x0835,//Format:Integer ��ͣ״̬
            SDO_NickName = 0x0836,//Format:String �س�
            SDO_9YouAccount = 0x0837,//Format:Integer 9you���ʺ�
            SDO_SEX = 0x0838,//Format:Integer �Ա�
            SDO_RegistDate = 0x0839,//Format:Date ע������
            SDO_FirstLogintime = 0x0840,//Format:Date ��һ�ε�¼ʱ��
            SDO_LastLogintime = 0x0841,//Format:Date ���һ�ε�¼ʱ��
            SDO_Ispad = 0x0842,//Format:Integer �Ƿ���ע������̺
            SDO_Desc = 0x0843,//Format:String ��������
            SDO_Postion = 0x0844,//Format:Integer ����λ��
            SDO_BeginTime = 0x0845,//Format:Date ���Ѽ�¼��ʼʱ��
            SDO_EndTime = 0x0846,//Format:Date ���Ѽ�¼����ʱ��
            SDO_SendTime = 0x0847,//Format:Date ������������
            SDO_SendIndexID = 0x0848,//Format:Integer �����˵�ID
            SDO_SendUserID = 0x0849,//Format:String �������ʺ�
            SDO_ReceiveNick = 0x0850,//Format:String �������س�
            SDO_BigType = 0x0851,//Format:Integer ���ߴ���
            SDO_SmallType = 0x0852,//Format:Integer ����С��
            SDO_REASON = 0x0853,//Format:String ͣ������
            SDO_StopTime = 0x0854,//Format:TimeStamp ͣ��ʱ��
            SDO_DaysLimit = 0x0855,//Format:Integer ʹ������
            SDO_Email = 0x0856,//Format:String �ʼ�
            SDO_ChargeSum = 0x0857,//Format:String ��ֵ�ϼ�
            SDO_KeyID = 0x0885,
            SDO_KeyWord = 0x0886,
            SDO_MasterID = 0x0887,
            SDO_Master = 0x0888,
            SDO_SlaverID = 0x0889,
            SDO_Slaver = 0x0890,
            SDO_ChannelList = 0x0891,
            SDO_BoardMessage = 0x0892,

            SDO_wPlanetID = 0x0893,
            SDO_wChannelID = 0x0894,
            SDO_iLimitUser = 0x0895,
            SDO_iCurrentUser = 0x0896,
            SDO_ipaddr = 0x0897,
            SDO_Interval = 0x0898,
            SDO_TaskID = 0x0899,
            SDO_Status = 0x08100,
            SDO_Score = 0x08101,//����
            SDO_FirstPadTime = 0x08102,
            SDO_MAIN_CH = 0x08104,// ������
            SDO_SUB_CH = 0x08105,// ����
            SDO_BanDate = 0x08103,//ͣ�������
            SDO_Lock_Status = 0x08196,

            SDO_LevPercent = 0x08106,
            SDO_ItemCodeBy = 0x08107,
            SDO_IsBattle = 0x8131,

            SDO_ItemCode1 = 0x8137,//TLV_STRING  ����ID
            SDO_DateLimit1 = 0x8138,//TLV_INTEGER ��������
            SDO_TimeLimit1 = 0x8139,//TLV_INTEGER ��������
            SDO_ItemCode2 = 0x8140,//TLV_STRING  ����ID
            SDO_DateLimit2 = 0x8141,//TLV_INTEGER ��������
            SDO_TimeLimit2 = 0x8142,//TLV_INTEGER ��������
            SDO_ItemCode3 = 0x8143,//TLV_STRING  ����ID
            SDO_DateLimit3 = 0x8144,//TLV_INTEGER ��������
            SDO_TimeLimit3 = 0x8145,//TLV_INTEGER ��������
            SDO_ItemCode4 = 0x8146,//TLV_STRING  ����ID
            SDO_DateLimit4 = 0x8147,//TLV_INTEGER ��������
            SDO_TimeLimit4 = 0x8148,//TLV_INTEGER ��������
            SDO_ItemCode5 = 0x8149,//TLV_STRING  ����ID
            SDO_DateLimit5 = 0x8150,//TLV_INTEGER ��������
            SDO_TimeLimit5 = 0x8151,//TLV_INTEGER ��������
            SDO_ItemName1 = 0x8152,//TLV_STRING  ������1
            SDO_ItemName2 = 0x8153,//TLV_STRING  ����ID2
            SDO_ItemName3 = 0x8154,//TLV_STRING  ����ID3
            SDO_ItemName4 = 0x8155,//TLV_STRING  ����ID4
            SDO_ItemName5 = 0x8156,//TLV_STRING  ����ID5


            ServerInfo_IP = 0x0901,//Format:String ������IP
            ServerInfo_City = 0x0902,//Format:String ����
            ServerInfo_GameID = 0x0903,//Format:Integer ��ϷID
            ServerInfo_GameName = 0x0904,//Format:String ��Ϸ��
            ServerInfo_GameDBID = 0x0905,//Format:Integer ��Ϸ���ݿ�����
            ServerInfo_GameFlag = 0x0906,//Format:Integer ��Ϸ������״̬
            ServerInfo_Idx = 0x0907,
            ServerInfo_DBName = 0x0908,
            ///////////////////////////////////////////////////////////////////////
            /// <summary>
            /// �����Ŷ���
            /// </summary>
            AU_ACCOUNT = 0x1001,//����ʺ� Format:String
            AU_UserNick = 0x1002,//����س� Format:String
            AU_Sex = 0x1003,//����Ա� Format:Integer
            AU_State = 0x1004,//���״̬ Format:Integer
            AU_STOPSTATUS = 0x1005,//�����߷�ͣ״̬ Format:Integer
            AU_Reason = 0x1006,//��ͣ���� Format:String
            AU_BanDate = 0x1007,//��ͣ���� Format:TimeStamp
            AU_ServerIP = 0x1008,//��������Ϸ������ Format:String
            AU_Id9you = 0x1009, //Format:Integer 9youID
            AU_UserSN = 0x1010, //Format:Integer �û����к�
            AU_EquipState = 0x1011, //Format:String 
            AU_AvatarItem = 0x1012, //Format:Integer
            AU_BuyNick = 0x1013, //Format:String �����س�
            AU_BuyDate = 0x1014,//Format:Timestamp ��������
            AU_ExpireDate = 0x1015,//Format:TimesStamp  ��������
            AU_BuyType = 0x1016, // Format:Integer ��������

            AU_PresentID = 0x1017, //Format:Integer ����ID
            AU_SendSN = 0x1018, //Format:Integer  ����SN
            AU_SendNick = 0x1019, //Format:String �����س�
            AU_RecvSN = 0x1020, //Format:String ������SN
            AU_RecvNick = 0x1021, //Format:String �������س�
            AU_Kind = 0x1022, //Format:Integer ����
            AU_ItemID = 0x1023, //Format:Integer ����ID
            AU_Period = 0x1024, //Format:Integer �ڼ�
            AU_BeforeCash = 0x1025, //Format:Integer ����֮ǰ���
            AU_AfterCash = 0x1026, //Format:Integer ����֮����
            AU_SendDate = 0x1027, //Format:TimeStamp ��������
            AU_RecvDate = 0x1028,//Format:TimeStamp ��������
            AU_Memo = 0x1029,//Format:String ��ע
            AU_UserID = 0x1030, //Format:String ���ID
            AU_Exp = 0x1031, //Format:Integer ��Ҿ���
            AU_Point = 0x1032, //Format:Integer ���λ��
            AU_Money = 0x1033, //Format:Integer ��Ǯ
            AU_Cash = 0x1034, //Format:Integer �ֽ�
            AU_Level = 0x1035, //Format:Integer �ȼ�
            AU_Ranking = 0x1036, //Format:Integer ����
            AU_IsAllowMsg = 0x1037, //Format:Integer ������Ϣ
            AU_IsAllowInvite = 0x1038, //Format:Integer ��������
            AU_LastLoginTime = 0x1039, //Format:TimeStamp ����¼ʱ��
            AU_Password = 0x1040, //Format:String ����
            AU_UserName = 0x1041, //Format:String �û���
            AU_UserGender = 0x1042, //Format:String 
            AU_UserPower = 0x1043, //Format:Integer
            AU_UserRegion = 0x1044, //Format:String 
            AU_UserEMail = 0x1045, //Format:String �û������ʼ�
            AU_RegistedTime = 0x1046, //Format:TimeStamp ע��ʱ��

            AU_ItemName = 0x1047,//������
            AU_ItemStyle = 0x1048,//��������
            AU_Demo = 0x1049,//���� 
            AU_BeginTime = 0x1050,//��ʼʱ��
            AU_EndTime = 0x1051,//����ʱ��
            AU_SendUserID = 0x1052,//�������ʺ�
            AU_RecvUserID = 0x1053,//�������ʺ� 
            AU_SexIndex = 0x1054,//�Ա�
            AU_GSName = 0x1084,

            AU_UseNum = 0x2296,//�������� int
            AU_BadgeID = 0x2292,//����ID int 



            AU_famid = 0x2219,//����ID int 
            AU_GSServerIP = 0x1085,
            AuShop_ItemTable = 0x1376,
            AU_BroadMsg = 0x2242,//�������� string
            Magic_Sex = 0x5005,// �Ա�
            Magic_GuildID = 0x5068, //����ID
            AU_Log_start = 0x2276,//��ѯ��ʼʱ�� (char 14) [YYYYMMDDHHIISS]
            AU_Log_end = 0x2277,//��ѯ����ʱ�� (char 14) [YYYYMMDDHHIISS]
            AU_UTP = 0x2274,//��ѯ�û����� (char 1) [1: Send; 2: Recv]
  
            Magic_category = 0x5071,//��־����
            Magic_action = 0x5072,//��־С��


            /// <summary>
            /// ��񿨶�������
            /// </summary>
            CR_ServerIP = 0x1101,//������IP
            CR_ACCOUNT = 0x1102,//����ʺ� Format tring
            CR_Passord = 0x1103,//������� Format tring
            CR_NUMBER = 0x1104,//������ Format tring
            CR_ISUSE = 0x1105,//�Ƿ�ʹ��
            CR_STATUS = 0x1106,//���״̬ Format:Integer
            CR_ActiveIP = 0x1107,//���������IP Format:String
            CR_ActiveDate = 0x1108,//�������� Format:TimeStamp
            CR_BoardID = 0x1109,//����ID Format:Integer
            CR_BoardContext = 0x1110,//�������� Format:String
            CR_BoardColor = 0x1111,//������ɫ Format:String
            CR_ValidTime = 0x1112,//��Чʱ�� Format:TimeStamp
            CR_InValidTime = 0x1113,//ʧЧʱ�� Format:TimeStamp
            CR_Valid = 0x1114,//�Ƿ���Ч Format:Integer
            CR_PublishID = 0x1115,//������ID Format:Integer
            CR_DayLoop = 0x1116,//ÿ�첥�� Format:Integer
            CR_PSTID = 0x1117,//ע��� Format:Integer
            CR_SEX = 0x1118,//�Ա� Format:Integer
            CR_LEVEL = 0x1119,//�ȼ� Format:Integer
            CR_EXP = 0x1120,//���� Format:Integer
            CR_License = 0x1121,//����Format:Integer
            CR_Money = 0x1122,//��ǮFormat:Integer
            CR_RMB = 0x1123,//�����Format:Integer
            CR_RaceTotal = 0x1124,//��������Format:Integer
            CR_RaceWon = 0x1125,//ʤ������Format:Integer
            CR_ExpOrder = 0x1126,//��������Format:Integer
            CR_WinRateOrder = 0x1127,//ʤ������Format:Integer
            CR_WinNumOrder = 0x1128,//ʤ����������Format:Integer
            CR_SPEED = 0x1129,//�����ٶ�Format:Integer
            CR_Mode = 0x1130,//���ŷ�ʽ Format:Integer
            CR_ACTION = 0x1131,//��ѯ������Format:Integer
            CR_NickName = 0x1132,//�س� Format:String
            CR_Channel = 0x1133,//Ƶ��ID
            CR_UserID = 0x1134,//�û�ID
            CR_BoardContext1 = 0x1135,//����1
            CR_BoardContext2 = 0x1136,//����2
            CR_Expire = 0x1137,//��Ч��ʽ
            CR_ChannelID = 0x1138,//Ƶ��ID
            CR_ChannelName = 0x1139,//Ƶ������
            CR_Last_Login = 0x1140,//�ϴε���ʱ��		
            CR_Last_Logout = 0x1141,//�ϴεǳ�ʱ��		
            CR_Last_Playing_Time = 0x1142,//�ϴ���Ϸʱ��		
            CR_Total_Time = 0x1143,//�ܵ���Ϸʱ��    
            CR_UserName = 0x1144,//�������
            /// <summary>
            /// 
            /// </summary>
            CARD_PDID = 0x1202,
            CARD_PDkey = 0x1203,
            CARD_PDCardType = 0x1204,
            CARD_PDFrom = 0x1205,
            CARD_PDCardNO = 0x1206,
            CARD_PDCardPASS = 0x1207,
            CARD_PDCardPrice = 0x1208,
            CARD_PDaction = 0x1209,
            CARD_PDuserid = 0x1210,
            CARD_PDusername = 0x1211,
            CARD_PDgetuserid = 0x1212,
            CARD_PDgetusername = 0x1213,
            CARD_PDdate = 0x1214,
            CARD_PDip = 0x1215,
            CARD_PDstatus = 0x1216,
            CARD_UDID = 0x1217,
            CARD_UDkey = 0x1218,
            CARD_UDusedo = 0x1219,
            CARD_UDdirect = 0x1220,
            CARD_UDuserid = 0x1221,
            CARD_UDusername = 0x1222,
            CARD_UDgetuserid = 0x1223,
            CARD_UDgetusername = 0x1224,
            CARD_UDcoins = 0x1225,
            CARD_UDtype = 0x1226,
            CARD_UDtargetvalue = 0x1227,
            CARD_UDzone1 = 0x1228,
            CARD_UDzone2 = 0x1229,
            CARD_UDdate = 0x1230,
            CARD_UDip = 0x1231,
            CARD_UDstatus = 0x1232,
            CARD_cardnum = 0x1233,
            CARD_cardpass = 0x1234,
            CARD_serial = 0x1235,
            CARD_draft = 0x1236,
            CARD_type1 = 0x1237,
            CARD_type2 = 0x1238,
            CARD_type3 = 0x1239,
            CARD_type4 = 0x1240,
            CARD_price = 0x1241,
            CARD_valid_date = 0x1242,
            CARD_use_status = 0x1243,
            CARD_cardsent = 0x1244,
            CARD_create_date = 0x1245,
            CARD_use_userid = 0x1246,
            CARD_use_username = 0x1247,
            CARD_partner = 0x1248,
            CARD_skey = 0x1249,
            CARD_ActionType = 0x1250,
            CARD_id = 0x1251,//TLV_STRING ��֮��ע�Ῠ��
            CARD_username = 0x1252,//TLV_STRING ��֮��ע���û���
            CARD_nickname = 0x1253,//TLV_STRING ��֮��ע���س�
            CARD_password = 0x1254,//TLV_STRING ��֮��ע������
            CARD_sex = 0x1255,//TLV_STRING ��֮��ע���Ա�
            CARD_rdate = 0x1256,//TLV_Date ��֮��ע������
            CARD_rtime = 0x1257,//TLV_Time ��֮��ע��ʱ��
            CARD_securecode = 0x1258,//TLV_STRING ��ȫ��
            CARD_vis = 0x1259,//TLV_INTEGER
            CARD_logdate = 0x1260,//TLV_TimeStamp ����
            CARD_realname = 0x1263,//TLV_STRING ��ʵ����
            CARD_birthday = 0x1264,//TLV_Date ��������
            CARD_cardtype = 0x1265,//TLV_STRING
            CARD_email = 0x1267,//TLV_STRING �ʼ�
            CARD_occupation = 0x1268,//TLV_STRING ְҵ
            CARD_education = 0x1269,//TLV_STRING �����̶�
            CARD_marriage = 0x1270,//TLV_STRING ���
            CARD_constellation = 0x1271,//TLV_STRING ����
            CARD_shx = 0x1272,//TLV_STRING ��Ф
            CARD_city = 0x1273,//TLV_STRING ����
            CARD_address = 0x1274,//TLV_STRING ��ϵ��ַ
            CARD_phone = 0x1275,//TLV_STRING ��ϵ�绰
            CARD_qq = 0x1276,//TLV_STRING QQ
            CARD_intro = 0x1277,//TLV_STRING ����
            CARD_msn = 0x1278,//TLV_STRING MSN
            CARD_mobilephone = 0x1279,//TLV_STRING �ƶ��绰
            CARD_SumTotal = 0x1280,//TLV_INTEGER �ϼ�

            /// <summary>
            /// 
            /// </summary>
            AuShop_orderid = 0x1301,//int(11) 
            AuShop_udmark = 0x1302,//int(8) 
            AuShop_bkey = 0x1303,//varchar(40) 
            AuShop_pkey = 0x1304,//varchar(18) 
            AuShop_userid = 0x1305,//int(11) 
            AuShop_username = 0x1306,//varchar(20) 
            AuShop_getuserid = 0x1307,//int(11) 
            AuShop_getusername = 0x1308,//varchar(20) 
            AuShop_pcategory = 0x1309,//smallint(4) 
            AuShop_pisgift = 0x1310,//enum('y','n') 
            AuShop_islover = 0x1311,//enum('y','n') 
            AuShop_ispresent = 0x1312,//enum('y','n') 
            AuShop_isbuysong = 0x1313,//enum('y','n') 
            AuShop_prule = 0x1314,//tinyint(1) 
            AuShop_psex = 0x1315,//enum('all','m','f') 
            AuShop_pbuytimes = 0x1316,//int(11) 
            AuShop_allprice = 0x1317,//int(11) 
            AuShop_allaup = 0x1318,//int(11)
            AuShop_buytime = 0x1319,//int(10) 
            AuShop_buytime2 = 0x1320,//datetime 
            AuShop_buyip = 0x1321,//varchar(15) 
            AuShop_zone = 0x1322,//tinyint(2) 
            AuShop_status = 0x1323,//tinyint(1) 
            AuShop_pid = 0x1324,//int(11) 
            AuShop_pname = 0x1326,//varchar(20) 
            AuShop_pgift = 0x1328,//enum('y','n') 
            AuShop_pscash = 0x1330,//tinyint(2) 
            AuShop_pgamecode = 0x1331,//varchar(200) 
            AuShop_pnew = 0x1332,//enum('y','n') 
            AuShop_phot = 0x1333,//enum('y','n') 
            AuShop_pcheap = 0x1334,//enum('y','n') 
            AuShop_pchstarttime = 0x1335,//int(10) 
            AuShop_pchstoptime = 0x1336,//int(10) 
            AuShop_pstorage = 0x1337,//smallint(5) 
            AuShop_pautoprice = 0x1339,//enum('y','n') 
            AuShop_price = 0x1340,//int(8) 
            AuShop_chprice = 0x1341,//int(8) 
            AuShop_aup = 0x1342,//int(8) 
            AuShop_chaup = 0x1343,//int(8) 
            AuShop_ptimeitem = 0x1344,//varchar(200) 
            AuShop_pricedetail = 0x1345,//varchar(254) 
            AuShop_pdesc = 0x1347,//text
            AuShop_pbuys = 0x1348,//int(8) 
            AuShop_pfocus = 0x1349,//tinyint(1) 
            AuShop_pmark1 = 0x1350,//enum('y','n') 
            AuShop_pmark2 = 0x1351,//enum('y','n') 
            AuShop_pmark3 = 0x1352,//enum('y','n') 
            AuShop_pinttime = 0x1353,//int(10) 
            AuShop_pdate = 0x1354,//int(10) 
            AuShop_pisuse = 0x1355,//enum('y','n') 
            AuShop_ppic = 0x1356,//varchar(36) 
            AuShop_ppic1 = 0x1357,//varchar(36) 
            AuShop_usefeesum = 0x1358,//int
            AuShop_useaupsum = 0x1359,//int
            AuShop_buyitemsum = 0x1360,//int
            AuShop_BeginDate = 0x1361,//date
            AuShop_EndDate = 0x1362,//

            AuShop_GCashSum = 0x1363,//int
            AuShop_MCashSum = 0x1364,//int


            /// <summary>
            /// 
            /// </summary>
            /// <summary>
            /// ������
            /// </summary>
            o2jam_ServerIP = 0x1401,//Format:TLV_STRING IP
            o2jam_UserID = 0x1402,//Format:TLV_STRING �û��ʺ�
            o2jam_UserNick = 0x1403,//Format:TLV_STRING �û��س�
            o2jam_Sex = 0x1404,//Format:TLV_INTEGER �Ա�
            o2jam_Level = 0x1405,//Format:TLV_STRING �ȼ�
            o2jam_Win = 0x1406,//Format:TLV_STRING ʤ
            o2jam_Draw = 0x1407,//Format:TLV_STRING ƽ
            o2jam_Lose = 0x1408,//Format:TLV_STRING ��
            o2jam_SenderID = 0x1409,				//varchar
            o2jam_SenderIndexID = 0x1410,				//int
            o2jam_SenderNickName = 0x1411,				//varchar
            o2jam_ReceiverID = 0x1412,				//varchar
            o2jam_ReceiverIndexID = 0x1413,				//int
            o2jam_ReceiverNickName = 0x1414,				//varchar
            o2jam_Title = 0x1415,				//varchar
            o2jam_Content = 0x1416,				//varchar
            o2jam_WriteDate = 0x1417,				//datetime
            o2jam_ReadDate = 0x1418,				//datetime
            o2jam_ReadFlag = 0x1419,				//char
            o2jam_TypeFlag = 0x1420,				//char
            o2jam_Ban_Date = 0x1421,				//datetime
            o2jam_GEM = 0x1422,				//int
            o2jam_MCASH = 0x1423,				//int
            o2jam_O2CASH = 0x1424,				//int
            o2jam_MUSICCASH = 0x1425,				//int
            o2jam_ITEMCASH = 0x1426,				//int
            o2jam_USER_INDEX_ID = 0x1427,				//int
            o2jam_ITEM_INDEX_ID = 0x1428,				//int
            o2jam_USED_COUNT = 0x1429,				//int
            o2jam_REG_DATE = 0x1430,				//datetime
            o2jam_OLD_USED_COUNT = 0x1431,				//int
            o2jam_CURRENT_CASH = 0x1433,				//int
            o2jam_CHARGED_CASH = 0x1434,				//int
            o2jam_KIND_CASH = 0x1435,				//char
            o2jam_NAME = 0x1437,				//varchar
            o2jam_KIND = 0x1438,				//int
            o2jam_PLANET = 0x1439,				//int
            o2jam_VAL = 0x1440,				//int
            o2jam_EFFECT = 0x1441,				//int
            o2jam_JUSTICE = 0x1442,				//int
            o2jam_LIFE = 0x1443,				//int
            o2jam_PRICE_KIND = 0x1444,				//int
            o2jam_Exp = 0x1445, //Int
            o2jam_Battle = 0x1446,//Int
            o2jam_POSITION = 0x1448,				//int
            o2jam_COMPANY_ID = 0x1449,				//int
            o2jam_DESCRIBE = 0x1450,				//varchar
            o2jam_UPDATE_TIME = 0x1451,				//datetime
            o2jam_ITEM_NAME = 0x1453,				//varchar
            o2jam_ITEM_USE_COUNT = 0x1454,				//int
            o2jam_ITEM_ATTR_KIND = 0x1455,				//int
            o2jam_USER_ID = 0x1457,				//varchar
            o2jam_USER_NICKNAME = 0x1458,				//varchar
            o2jam_SEX = 0x1459,				//char
            o2jam_CREATE_TIME = 0x1460,				//datetime
            o2jam_BeginDate = 0x1461,
            o2jam_EndDate = 0x1462,
            O2JAM_BuyType = 0x1509,//TLV_INTEGER


            O2JAM_EQUIP1 = 0x1463,		//TLV_STRING,
            O2JAM_EQUIP2 = 0x1464,	//TLV_STRING,
            O2JAM_EQUIP3 = 0x1465,	//TLV_STRING,
            O2JAM_EQUIP4 = 0x1466,	//TLV_STRING,
            O2JAM_EQUIP5 = 0x1467,	//TLV_STRING,
            O2JAM_EQUIP6 = 0x1468,	//TLV_STRING,
            O2JAM_EQUIP7 = 0x1469,	//TLV_STRING,
            O2JAM_EQUIP8 = 0x1470,	//TLV_STRING,
            O2JAM_EQUIP9 = 0x1471,	//TLV_STRING,
            O2JAM_EQUIP10 = 0x1472,		//TLV_STRING,
            O2JAM_EQUIP11 = 0x1473,	//TLV_STRING,
            O2JAM_EQUIP12 = 0x1474,	//TLV_STRING,
            O2JAM_EQUIP13 = 0x1475,	//TLV_STRING,
            O2JAM_EQUIP14 = 0x1476,	//TLV_STRING,
            O2JAM_EQUIP15 = 0x1477,	//TLV_STRING,
            O2JAM_EQUIP16 = 0x1478,		//TLV_STRING,
            O2JAM_BAG1 = 0x1479,		//TLV_STRING,
            O2JAM_BAG2 = 0x1480,	//TLV_STRING,
            O2JAM_BAG3 = 0x1481,	//TLV_STRING,
            O2JAM_BAG4 = 0x1482,	//TLV_STRING,
            O2JAM_BAG5 = 0x1483,		//TLV_STRING,
            O2JAM_BAG6 = 0x1484,	//TLV_STRING,
            O2JAM_BAG7 = 0x1485,	//TLV_STRING,
            O2JAM_BAG8 = 0x1486,	//TLV_STRING,
            O2JAM_BAG9 = 0x1487,	//TLV_STRING,
            O2JAM_BAG10 = 0x1488,	//TLV_STRING,
            O2JAM_BAG11 = 0x1489,	//TLV_STRING,
            O2JAM_BAG12 = 0x1490,	//TLV_STRING,
            O2JAM_BAG13 = 0x1491,	//TLV_STRING,
            O2JAM_BAG14 = 0x1492,	//TLV_STRING,
            O2JAM_BAG15 = 0x1493,	//TLV_STRING,
            O2JAM_BAG16 = 0x1494,	//TLV_STRING,
            O2JAM_BAG17 = 0x1495,	//TLV_STRING,
            O2JAM_BAG18 = 0x1496,	//TLV_STRING,
            O2JAM_BAG19 = 0x1497,	//TLV_STRING,
            O2JAM_BAG20 = 0x1498,	//TLV_STRING,
            O2JAM_BAG21 = 0x1499,	//TLV_STRING,
            O2JAM_BAG22 = 0x1500,	//TLV_STRING,
            O2JAM_BAG23 = 0x1501,	//TLV_STRING,
            O2JAM_BAG24 = 0x1502,	//TLV_STRING,
            O2JAM_BAG25 = 0x1503,	//TLV_STRING,
            O2JAM_BAG26 = 0x1504,	//TLV_STRING,
            O2JAM_BAG27 = 0x1505,	//TLV_STRING,
            O2JAM_BAG28 = 0x1506,	//TLV_STRING,
            O2JAM_BAG29 = 0x1507,	//TLV_STRING,
            O2JAM_BAG30 = 0x1508,	//TLV_STRING



            /// <summary>
            /// ������II
            /// </summary>
            O2JAM2_ServerIP = 0x1601,//TLV_STRING
            O2JAM2_UserID = 0x1602,//TLV_STRING
            O2JAM2_UserName = 0x1603,//TLV_STRING
            O2JAM2_Id1 = 0x1604,//TLV_Integer
            O2JAM2_Id2 = 0x1605,//TLV_Integer
            O2JAM2_Rdate = 0x1606,//TLV_Date
            O2JAM2_IsUse = 0x1607,//TLV_Integer
            O2JAM2_Status = 0x1608,//TLV_Integer


            O2JAM2_UserIndexID = 0x1609,//TLV_Integer
            O2JAM2_UserNick = 0x1610,//Format:TLV_STRING �û��س�
            O2JAM2_Sex = 0x1611,//Format:TLV_BOOLEAN �Ա�
            O2JAM2_Level = 0x1612,//Format:TLV_INTEGER �ȼ�
            O2JAM2_Win = 0x1613,//Format:TLV_INTEGER ʤ
            O2JAM2_Draw = 0x1614,//Format:TLV_INTEGER ƽ
            O2JAM2_Lose = 0x1615,//Format:TLV_INTEGER ��
            O2JAM2_Exp = 0x1616,//Format:TLV_INTEGER ����
            O2JAM2_TOTAL = 0x1617,//Format:TLV_INTEGER �ܾ���
            O2JAM2_GCash = 0x1618,//Format:TLV_INTEGER G��
            O2JAM2_MCash = 0x1619,//Format:TLV_INTEGER M��
            O2JAM2_ItemCode = 0x1620,//Format:TLV_INTEGER
            O2JAM2_ItemName = 0x1621,//Format:TLV_String
            O2JAM2_Timeslimt = 0x1622,
            O2JAM2_DateLimit = 0x1623,
            O2JAM2_ItemSource = 0x1624,
            O2JAM2_Position = 0x1625,//int
            O2JAM2_BeginDate = 0x1626,
            O2JAM2_ENDDate = 0x1627,
            O2JAM2_MoneyType = 0x1628,//int
            O2JAM2_Title = 0x1629,
            O2JAM2_Context = 0x1630,
            o2jam2_consumetype = 0x1631,//int
            O2JAM2_ComsumeCode = 0x1632,//int
            O2JAM2_DayLimit = 0x1633,//int

            O2JAM2_StopTime = 0x1634,//date
            O2JAM2_StopStatus = 0x1635,//int
            O2JAM2_REASON = 0x1636,//string




            SDO_SenceID = 0x0858,
            SDO_WeekDay = 0x0859,
            SDO_MatPtHR = 0x0860,
            SDO_MatPtMin = 0x0861,
            SDO_StPtHR = 0x0862,
            SDO_StPtMin = 0x0863,
            SDO_EdPtHR = 0x0864,
            SDO_EdPtMin = 0x0865,

            SDO_Sence = 0x0868,
            SDO_MusicID1 = 0x0869,
            SDO_MusicName1 = 0x0870,
            SDO_LV1 = 0x0871,
            SDO_MusicID2 = 0x0872,
            SDO_MusicName2 = 0x0873,
            SDO_LV2 = 0x0874,
            SDO_MusicID3 = 0x0875,
            SDO_MusicName3 = 0x0876,
            SDO_LV3 = 0x0877,
            SDO_MusicID4 = 0x0878,
            SDO_MusicName4 = 0x0879,
            SDO_LV4 = 0x0880,
            SDO_MusicID5 = 0x0881,
            SDO_MusicName5 = 0x0882,
            SDO_LV5 = 0x0883,
            SDO_Precent = 0x0884,
            SDO_State = 0x8134,//TLV_STRING  ״̬
            SDO_mood = 0x8135,//TLV_INTEGER ����ֵ
            SDO_Food = 0x8136,//TLV_INTEGER ��ʳ��


            SDO_PreValue = 0x8160,//��Сֵ
            SDO_EndValue = 0x8161,//���ֵ
            SDO_NorProFirst = 0x8162,//��һ�δ򿪵ĸ���
            SDO_NorPro = 0x8163,//��ͨ����ĸ���
            SDO_SpePro = 0x8164,//���ⱦ��ĸ���
            SDO_baoxiangid = 0x8165,//id int
            SDO_Mark = 0x8166,//��ʶ int
            //SDO_FirstPadTime = 0x0885,
            Soccer_ServerIP = 0x1701,
            Soccer_loginId = 0x1702,//Login ID
            Soccer_charsex = 0x1703,// 
            Soccer_charidx = 0x1704,//��ɫ���к� (ExSoccer.dbo.t_character[idx])
            Soccer_charexp = 0x1705,//����ֵ
            Soccer_charlevel = 0x1706,//�ȼ�
            Soccer_charpoint = 0x1707,//G���� 
            Soccer_match = 0x1708,//������
            Soccer_win = 0x1709,//
            Soccer_lose = 0x1710,//ʧ��
            Soccer_draw = 0x1711,//ƽ��
            Soccer_drop = 0x1712,//ƽ��		
            Soccer_charname = 0x1713,//��ɫ��
            Soccer_charpos = 0x1714,//λ��
            Soccer_Type = 0x1715,//��ѯ����
            Soccer_String = 0x1716,//��ѯֵ 
            Soccer_admid = 0x1717,//������ID
            Soccer_deleted_date = 0x1718,//ɾ������
            Soccer_status = 0x1719,//״̬
            Soccer_m_id = 0x1720,//������к� int
            Soccer_m_auth = 0x1721,//����Ƿ�ͣ�� int
            Soccer_regDate = 0x1722,//���ע������ string 
            Soccer_c_date = 0x1723,//��ɫ�������� string 
            Soccer_char_max = 0x1724,//tinyint
            Soccer_char_cnt = 0x1725,//int
            Soccer_ret = 0x1726,//int
            Soccer_kind = 0x1727,//socket,name string

            #region �Ҵ�Online
            SD_GoldAccount = 0x4401,//TLV_STRING�Ƿ�߼��ʺ�
            SD_IsUsed = 0x4402,//�Ƿ�ʹ��
            SD_UserName = 0x4403,//����ʺ�
            SD_UseDate = 0x4404,//ʹ������
            SD_ActiceCode = 0x4405,//������


            f_passWd = 0x4406,//���� string
            f_gender = 0x4407,//�Ա� int 0Ů1��
            f_adult = 0x4408,//�Ƿ���� 0����1δ����
            f_regDate = 0x4409,//ע������ timestamp
            f_loginDate = 0x4410,//����½���� timestamp
            f_logoutDate = 0x4411,//���ǳ����� timestamp
            f_bandate = 0x4412,//��ͣ���� timestamp
            f_pilot = 0x4413,//�º� string
            f_level = 0x4414,//���� int
            f_levelname = 0x4415,//�������� string
            f_Exp = 0x4416,//���� int
            f_shootCnt = 0x4417,//������ int
            f_collectCnt = 0x4418,//�ռ��� int
            f_fightCnt = 0x4419,//ս���� int
            f_winCnt = 0x4420,//ʤ�� int
            f_loseCnt = 0x4421,//ʧ�� int
            f_drawCnt = 0x4422,//ƽ�� int
            f_idx = 0x4423, //���ID int

            SD_ban = 0x4424,//��ͣ int
            SD_NeedExp = 0x4425,//������Ҫ�ľ��� int
            SD_GC = 0x4426,//G�� int
            SD_GP = 0x4427,//M�� int
            SD_KillNum = 0x4428,//��ɱ���� int

            SD_SlotNumber = 0x4429,//���� int 
            SD_ItemName = 0x4430,//������ string 
            SD_ItemID = 0x4431,//����ID int 
            SD_GetTime = 0x4432,//���ʱ�� timestamp 
            SD_RemainCount = 0x4433,//ʣ������ int 
            SD_BuyType = 0x4434,//�������� int

            SD_DateEnd = 0x4435,//����ʱ�� timestamp
            SD_UnitName = 0x4436,//������ string
            SD_UnitNickName = 0x4437,//�����س� string
            SD_UnitLevelNumber = 0x4438,//����ȼ� int
            SD_UnitExp = 0x4439,//���徭�� int
            SD_StateSaleIntention = 0x4440,//�Ƿ�������� string
            SD_CustomLvMax = 0x4441,//�������ϳɴ���
            SD_CustomPoint = 0x4442,//����ϳɵ���
            SD_BatItemName = 0x4443,//ս���������� string
            SD_OperatorNickname = 0x4444,//�����س� string

            SD_ServerIP = 0x4445,//������IP string

            SD_StartTime = 0x4446,//��ʼʱ��  DateTime
            SD_EndTime = 0x4447,//����ʱ�� DateTime
            SD_ServerName = 0x4448,//���������� string
            SD_ID = 0x4449,//id INT
            SD_Content = 0x4450,//��ͣ���� string
            SD_Limit = 0x4451,//��� int
            SD_Type = 0x4452,//���� int
            SD_Title = 0x4453,//���� string
            SD_Number = 0x4454,//���� int
            SD_TmpPWD = 0x4455,//��ʱ���� string
            SD_passWd = 0x4456,//��ʵ���� string
            SD_TempPassWord = 0x4457,//���� string

            SD_FromIdx = 0x4458,//�����û�id int
            SD_ToIdx = 0x4459,//�����û�id int

            SD_ItemName1 = 0x4460,//������1 string
            SD_ItemID1 = 0x4461,//����ID1 int
            SD_ItemName2 = 0x4462,//������2 string
            SD_ItemID2 = 0x4463,//����ID2 int
            SD_ItemName3 = 0x4464,//������3 string
            SD_ItemID3 = 0x4465,//����ID3 int
            SD_Number1 = 0x4466,//����1 int
            SD_Number2 = 0x4467,//����2 int
            SD_Number3 = 0x4468,//����3 int

            SD_N10 = 0x4469,//���﷢�ͺ�ʣ���point int
            SD_N11 = 0x4470,//ʹ�������ﹺ���ϵ�point int
            SD_N12 = 0x4471,//���㷽ʽ(0.point 1.cash) int
            SD_SendTime = 0x4472,//����ʱ�� dateTime
            SD_FromUser = 0x4473,//�����û� string
            SD_ToUser = 0x4474,//�����û� string
            SD_Make = 0x4475,//���� string
            SD_SendType = 0x4476,//���͹������� int 0:��ʱ���� 1:��ͨ����
            SD_Status = 0x4477,//���͹���״̬ int

            SD_LastMoney = 0x4478,//ʣ���Ǯ string
            SD_UseMoney = 0x4479,//ɾ��ʱ�õ���Ǯ string
            SD_DeleteResult = 0x4480,//ɾ��ԭ�� string
            SD_UnitType = 0x4481,//�������� string
            SD_ShopType = 0x4482,//�̵깺�������� string
            SD_LimitType = 0x4483,//�����ڼ䵽����� string
            SD_ChangeBody = 0x4484,//��װ��λ string
            SD_ChangeItem = 0x4485,//��װ���� string
            SD_CombPic = 0x4486,//�ϳ�ͼֽ string
            SD_CombItem = 0x4487,//�ϳ��ز� string
            SD_Time = 0x4488,//ʱ�� string
            SD_QuestionName = 0x4489,//�������� string
            SD_Questionlevel = 0x4490,//�����Ѷ� int
            SD_QuestionTime = 0x4491,//�������ʱ�� string
            SD_QuestionGetItem = 0x4492,//������������ string
            SD_Msg = 0x4493,//���� string 
            SD_FirendName = 0x4494,//���� string
            SD_GetMoney = 0x4495,//�õ���Ǯ string
            SD_State = 0x4496,//״̬ int
            SD_QusetID = 0x4497,//����ID int
            SD_QusetName = 0x4498,//�������� string
            SD_ClearedDate = 0x4499,//���ʱ�� string 
            SD_HP = 0x4500,//string
            SD_DashLevel = 0x4501,//string
            SD_StrikingPower = 0x4502,//string
            SD_FatalAttack = 0x4503,//string
            SD_DefensivePower = 0x4504,//string
            SD_MotivePower = 0x4505,//string
            SD_DelTime = 0x4506,//ɾ��ʱ�� timestamp

            SD_SkillName = 0x4507,//���� string
            SD_UColor_1 = 0x4508,//��ɫ1 string
            SD_UColor_2 = 0x4509,//��ɫ2 string
            SD_UColor_3 = 0x4510,//��ɫ3 string
            SD_UColor_4 = 0x4511,//��ɫ4 string
            SD_UColor_5 = 0x4512,//��ɫ5 string
            SD_UColor_6 = 0x4513,//��ɫ6 string
            SD_UDecal_1 = 0X4514,//��ǩ1 string
            SD_UDecal_2 = 0X4515,//��ǩ2 string
            SD_UDecal_3 = 0X4516,//��ǩ3 string
            SD_RewardCount = 0x4518,
            SD_UnitLevelName = 0x4519,//����ȼ�         
            SD_TypeName = 0x4520,//����
            SD_Star = 0x4521,//�����Ǽ�
            SD_LevelUpTime = 0x4523,//����ʱ�� timestamp
            SD_Money_Old = 0x4524,//�޸�ǰ��Ǯ int
            SD_UserName_Old = 0x4522,//�޸�ǰ�û��� string
            SD_LostCodeMoney = 0x4525,//ʣ����� string
            SD_UseCodeMoney = 0x4526,//ʹ�ô��� string
            SD_LostMoney = 0x4527,//���ĵ�G��

            SD_GuildName = 0x4539,//�������� guildName
            SD_GuildBass = 0x4547,//������� string 
            SD_Oldvalue = 0x4549, //�۳�ǰ��� string
            SD_Newvalue = 0x4550,//�۳����� string 
            #endregion

            #region ������2
            /// <summary>
            /// ������2����
            /// </summary>
            LORD_Gold = 0x6206,//���
            JW2_ACCOUNT = 0x7001,//����ʺ� Format:String
            JW2_UserNick = 0x7002,//����س� Format:String
            JW2_ServerName = 0x7003,//����������
            JW2_ServerIP = 0x7004,//��Ϸ������ Format:String
            JW2_State = 0x7005,//���״̬ Format:Integer
            JW2_Reason = 0x7006,//��ͣ���� Format:String
            JW2_BanDate = 0x7007,//��ͣ���� Format:dataTime
            JW2_UserSN = 0x7008, //Format:Integer �û����к�
            JW2_GSServerIP = 0x7009,//���� Format:String
            JW2_UserID = 0x7010, //Format:String ���ID
            JW2_Sex = 0x7011,//����Ա� Format:Integer
            JW2_AvatarItem = 0x7012, //Format:Integer ����ID
            JW2_ExpireDate = 0x7013,//Format:TimesStamp  ��������
            JW2_Exp = 0x7014, //Format:Integer ��Ҿ���
            JW2_Money = 0x7015, //Format:Integer ��Ǯ
            JW2_Cash = 0x7016, //Format:Integer �ֽ�
            JW2_Level = 0x7017, //Format:Integer �ȼ�
            JW2_UseItem = 0x7018,//Format:Integer�Ƿ���ʹ���У�1��0����
            JW2_RemainCount = 0x7019,//Format:Integerʣ�������0Ϊ���޴�
            JW2_BeginTime = 0x7020,//��ʼʱ��Format:DATE
            JW2_EndTime = 0x7021,//����ʱ��Format:DATE
            JW2_BoardMessage = 0x7022,//��������,���ȷ�������Format:String
            JW2_TaskID = 0x7023,//�����Format:Integer
            JW2_Status = 0x7024,//�Ƿ����״̬Format:Integer
            JW2_Interval = 0x7025,//���ʱ��Format:Integer
            JW2_UseTime = 0x7026,//ʹ��ʱ�� TLV_DATE
            JW2_POWER = 0x7027,//Ȩ�ޣ���ͨ�û���0������ԱΪ1 Format:Integer
            JW2_GoldMedal = 0x7028,//���� Format:Integer
            JW2_SilverMedal = 0x7029,//���� Format:Integer
            JW2_CopperMedal = 0x7030,//ͭ�� Format:Integer
            JW2_Region = 0x7031,//���� Format:String
            JW2_QQ = 0x7032,//QQ�� Format:String
            JW2_PARA = 0x7033,//��������õ�һ������ Format:Integer
            JW2_ATONCE = 0x7034,//�Ƿ���������Format:Integer
            JW2_BOARDSN = 0x7035,//��С���ȣ������¼Ψһ��ʾFormat:Integer
            JW2_BUGLETYPE = 0x7036,//����0mb��С����,1���ַ�С����,1mb��������,11���ַ�������,20��20�����Ǻ��Format:Integer
            //���///////////
            JW2_Chapter = 0x7037,//�½�
            JW2_CurStage = 0x7038,//Ŀǰ�ȼ�
            JW2_MaxStage = 0x7039,//���ȼ�
            JW2_Stage0 = 0x7040,//�ؿ�1
            JW2_Stage1 = 0x7041,
            JW2_Stage2 = 0x7042,
            JW2_Stage3 = 0x7043,
            JW2_Stage4 = 0x7044,
            JW2_Stage5 = 0x7045,
            JW2_Stage6 = 0x7046,
            JW2_Stage7 = 0x7047,
            JW2_Stage8 = 0x7048,
            JW2_Stage9 = 0x7049,
            JW2_Stage10 = 0x7050,
            JW2_Stage11 = 0x7051,
            JW2_Stage12 = 0x7052,
            JW2_Stage13 = 0x7053,
            JW2_Stage14 = 0x7054,
            JW2_Stage15 = 0x7055,
            JW2_Stage16 = 0x7056,
            JW2_Stage17 = 0x7057,
            JW2_Stage18 = 0x7058,
            JW2_Stage19 = 0x7059,//�ؿ�20
            //���end///////////
            JW2_BUYSN = 0x7060,//����SN
            JW2_GOODSTYPE = 0x7061,//��������
            JW2_RECESN = 0x7062,//�����˵�SN�������ͬ�Ǳ��ˣ�
            JW2_GOODSINDEX = 0x7063,//��Ʒ���
            JW2_BUYDATE = 0x7064,//��������
            JW2_RECEID = 0x7065,//�����˵�ID�������ͬ�Ǳ��ˣ�
            JW2_AvatarItemName = 0x7066, //�������� Format:String
            JW2_MALESN = 0x7067,//����SN
            JW2_MALEUSERNAME = 0x7068,//�����û���
            JW2_MALEUSERNICK = 0x7069,//�����ǳ�
            JW2_FEMAILESN = 0x7070,//Ů��SN
            JW2_FEMALEUSERNAME = 0x7071,//Ů���û���
            JW2_FEMAILEUSERNICK = 0x7072,//Ů���ǳ�
            JW2_WEDDINGDATE = 0x7073,//���ʱ��
            JW2_UNWEDDINGDATE = 0x7074,//���ʱ��
            JW2_WEDDINGNAME = 0x7075,//��������
            JW2_WEDDINGVOW = 0x7076,//��������
            JW2_RINGLEVEL = 0x7077,//��ָ�ȼ�
            JW2_REDHEARTNUM = 0x7078,//��������
            JW2_WEDDINGNO = 0x7079,//�������
            JW2_CONFIRMSN = 0x7080,//��֤��SN
            JW2_CONFIRMUSERNAME = 0x7081,//��֤���û���
            JW2_CONFIRMUSERNICK = 0x7082,//��֤���ǳ�
            JW2_COUPLEDATE = 0x7083,//��������
            JW2_KISSNUM = 0x7084,//kiss����
            JW2_LASTKISSDATE = 0x7085,//���һ��Kissʱ��
            JW2_BREAKDATE = 0x7088,//����ʱ��
            JW2_CMBREAKDATE = 0x7089,//ȷ�Ϸ���ʱ��
            JW2_BREAKSN = 0x7090,//���SN
            JW2_BREAKUSERNAME = 0x7091,//����û���
            JW2_BREAKUSERNICK = 0x7092,//����ǳ�
            JW2_LASTLOGINDATE = 0x7093,//����½ʱ��
            JW2_REGISTDATE = 0x7094,//����ʱ��
            JW2_FCMSTATUS = 0x7095,//������״̬
            JW2_FAMILYID = 0x7096,//����ID
            JW2_FAMILYNAME = 0x7097,//����
            JW2_DUTYID = 0x7098,//ְҵ��
            JW2_DUTYNAME = 0x7099,//ְҵ��
            JW2_ATTENDTIME = 0x7100,//����ʱ��
            JW2_COUPLESN = 0x7101,//�������
            JW2_CREATETIME = 0x7102,//����ʱ��
            JW2_CNT = 0x7103,//����
            JW2_POINT = 0x7104,//����
            JW2_LOGINTYPE = 0x7105,//����0���룬1�ǳ�
            JW2_TIME = 0x7106,//ʱ��
            JW2_IP = 0x7107,//IP��ַ
            JW2_PWD = 0x7108,//����
            JW2_AvatarItemType = 0x7109,//��Ʒ���ͣ�ͷ���ȣ�
            JW2_ItemPos = 0x7110,//��Ʒλ��(0,����,1,��Ʒ��,2,������)
            JW2_MailTitle = 0x7111,//�ʼ�����
            JW2_MailContent = 0x7112,//�ʼ�����
            JW2_ItemNo = 0x7113,//��Ʒ���
            JW2_Repute = 0x7115,//����
            JW2_NowTitle = 0x7116,//�ƺ�
            JW2_FamilyLevel = 0x7117,//�ȼ�
            JW2_AttendRank = 0x7118,//��������
            JW2_MoneyRank = 0x7119,//�Ƹ�����
            JW2_PowerRank = 0x7120,//ʵ������
            JW2_ItemCode = 0x7121,//����ID
            JW2_ItemName = 0x7122,//��������
            JW2_Productid = 0x7123,//��ƷID
            JW2_ProductName = 0x7124,//��Ʒ����
            JW2_FamilyPoint = 0x7125,//�������
            JW2_PetSn = 0x7126,//����ID
            JW2_PetName = 0x7127,//��������
            JW2_PetNick = 0x7128,//�����Զ�������
            JW2_PetLevel = 0x7129,//����ȼ�
            JW2_PetExp = 0x7130,//���ﾭ��
            JW2_PetEvol = 0x7131,//���׽׶�
            JW2_MailSn = 0x7132,//���к�
            JW2_SendNick = 0x7133,//�������ǳ�
            JW2_SendDate = 0x7134,//��������
            JW2_Num = 0x7135,//����
            JW2_Rate = 0x7136,//��Ů����
            JW2_ShaikhNick = 0x7137,//�峤����
            JW2_SubFamilyName = 0x7138,//������������
            JW2_sNum = 0x7139,//����STRING
            JW2_MessengerName = 0x7140,//Messenger�ƺ�
            JW2_TmpPWD = 0x7141,//��ʱ���� string 
            JW2_Type = 0x7142,//���� string
            JW2_SendUser = 0x7143,//�����û� string 
            JW2_RecUser = 0x7144,//�����û� string
            JW2_BeforeCash = 0x7145,//����ǰ��� int 
            JW2_AfterCash = 0x7146,//���Ѻ��� int
            JW2_Center_Avatarid = 0x7147,//�м������ID,int
            JW2_Center_AvatarName = 0x7148,//�м�������� string
            JW2_Center_State = 0x7149,//״̬ int(0װ����2δװ��)
            JW2_Last_Op_Time = 0x7150,//���ʹ��ʱ�� date
            JW2_LOGDATE = 0x7151,//ʱ�� date
            JW2_ZoneID = 0x7152,//������ID int
            JW2_VailedDay = 0x7153,//ʱ�ޣ�7�죬30�죬65535=���ޣ� int
            JW2_IntRo = 0x7154,//״̬��1�Լ�����0�Ǳ������ͣ� int
            JW2_SubGameID = 0x7155,//����ϷID int
            JW2_Center_EndTime = 0x7156,//�^�ڕr�g date
            JW2_Forever = 0x7157,//(1���ã�0������) int
            JW2_Family_Money = 0x7159,//���׽�Ǯ string 
            JW2_ReportSn = 0x7160,//�ٱ�ID int
            JW2_ReporterSn = 0x7161,//�ٱ���ID int 
            JW2_ReporterNick = 0x7162,//�ٱ����ǳ� string
            JW2_ReportedNick = 0x7163,//���ٱ����ǳ� string
            JW2_Memo = 0x7164,//�ٱ�˵�� string
            JW2_ReportType = 0x7165,//�ٱ����� int 
            JW2_OLD_FAMILYNAME = 0x7166,//�ϼ����� string
            JW2_OLD_PETNAME = 0x7167,//�ϳ����Զ�����
            JW2_BuyLimitDay = 0x7168,//�������� string
            JW2_BuyMoneyCost = 0x7169,//��ʵ�۸� string
            JW2_BuyOrgCost = 0x7170,//ԭʼ�۸� string 

            JW2_MissionID = 0x7171,//����ID int
            JW2_MissionName = 0x7172,//�������� string


            jw2_goodsprice = 0x7173,//����۸� int
            jw2_beforemoney = 0x7174,//����ǰ��Ǯ�� int
            jw2_aftermoney = 0x7175,//������Ǯ�� int



            jw2_serverno = 0x7176,//GS��� int
            jw2_port = 0x7177,//GS�˿�int

            jw2_MaterialCode = 0x7178,//�ϳɲ��� string

            jw2_LastGetPointDate = 0x7179,//���һ�λ�û�Ծ�ȵ����� data
            jw2_MultiDays = 0x7180,//������Ծ���� int
            jw2_TodayActivePoint = 0x7181,//�����õĻ�Ծ�ȵ��� int
            jw2_activepoint = 0x7182,//��Ծ�� int

            TIMEBegin = 0x0633,
            TIMEEend = 0x0634,

            jw2_pic_Name = 0x7183,//ͼƬ���� string 

            jw2_Wedding_Home = 0x7184,//�������� int (500,����С��;700,��ܰС��;1000,��������)

            jw2_frmLove = 0x7185,//���ܶ� int
            jw2_petaggName = 0x7186,//���ﵰ�� string
            jw2_petaggID = 0x7187,//���ﵰID	int
            jw2_petGetTime = 0x7188,//����ʱ��  string
            jw2_useLove = 0x7189,//�������ܶ� int
            jw2_getTime = 0x7190,//�һ�ʱ��	 string
            jw2_FirstFamilySN = 0x7191,//���뷽����id int
            jw2_SecondFamilySN = 0x7192,//�����뷽����id int 
            jw2_FirstFamilyName = 0x7193,//���뷽������ string
            jw2_SecondFamilyName = 0x7194,//�����뷽������ string
            jw2_FirstFamilyMaster = 0x7195,//���뷽�����峤�� string
            jw2_SecondFamilyMaster = 0x7196,//�����뷽�峤�� string
            jw2_FirstFamilyUseMoneyr = 0x7197,//���뷽���Ļ��� string
            jw2_SecondFamilyUseMoneyr = 0x7198,//�����뷽���Ļ��� string
            jw2_ApplyDate = 0x7199,// ����ʱ��  string 
            jw2_ApplyState = 0x7200,// ����״̬  string

            jw2_PetID = 0x7201,// ����ID	int 
            jw2_grade = 0x7202,//������׶� string 
            jw2_EggNum = 0x7203,//���ﵰ���� int

            jw2_PetAggID_Small = 0x7204,// С���ﵰID	int 
            jw2_PetAggName_Small = 0x7205,// С���ﵰ����	string 
            jw2_OverTimes = 0x7206,//�һ�ʱ�� string 
            
            #endregion

            #region ���߷ɳ�
            /// <summary>
            /// ���߷ɳ�
            /// </summary>
            RC_ServerIP = 0x0001,//TLV_String
            RC_Account = 0x0002,//TLV_String
            RC_CharName = 0x0003,//TLV_String
            RC_isPlatinum = 0x0020,//TLV_String
            RC_isUse = 0x0021,//TLV_String
            RC_Rank = 0x0005,//TLV_String
            RC_LoginIP = 0x0017,//TLV_String
            RC_Vehicle = 0x0006,//TLV_INTEGER
            RC_Level = 0x0007,//TLV_INTEGER
            RC_Sex = 0x0004,//TLV_INTEGER
            RC_Exp = 0x0008,//TLV_Money
            RC_MatchNum = 0x0009,//TLV_INTEGER
            RC_9YOUPointer = 0x0010,//TLV_INTEGER
            RC_IGroup = 0x0011,//TLV_INTEGER
            RC_Money = 0x0012,//TLV_INTEGER
            RC_PlayCounter = 0x0015,//TLV_INTEGER
            RC_UserIndexID = 0x0016,//TLV_INTEGER
            RC_ActiveCode = 0x0022,
            RC_BeginDate = 0x0018,//TLV_TimeStamp
            RC_EndDate = 0x0019,//TLV_TimeStamp
            RC_OnlineTime = 0x0013,//TLV_TimeStamp
            RC_OfflineTime = 0x0014,//TLV_TimeStamp
            RC_UseDate = 0x0023,
            RC_Split_IForce = 0x0024,//TLV_INTEGER ��Ӫ
            RC_IRunDistance = 0x0025,//TLV_INTEGER ����ʻ����
            RC_ILostConnection = 0x0026,//TLV_INTEGER ���ߴ���
            RC_IEscapeCounter = 0x0027,//TLV_INTEGER ���ܴ���
            RC_IWinCounter = 0x0028,//TLV_INTEGER ��ʤ�Ĵ���
            RC_IGameMoney = 0x0029,//TLV_INTEGER ��Ϸ��
            RC_AllOnlineTime = 0x0030,//TLV_INTEGER ����ʱ��
            RC_Sys_timeLastLogout = 0x0031,//TLV_TimeStamp ���һ���˳�ʱ��
            RC_Sys_ILastLoginIp = 0x0032,//TLV_String ���һ�ε�½IP
            RC_BanDate = 0x0033,//TLV_DATE ��ͣʱ��
            RC_BanReason = 0x0034,//TLV_string ��ͣ����
            RC_State = 0x0035,
            RC_ID = 0x0036,
            RC_chName = 0x0063,
            RC_chAccount = 0x0064,
            RC_Car_Level = 0x0037,
            RC_Pref_DM = 0x0038,
            RC_Add_Nos = 0x0039,
            RC_Pref_TS = 0x0040,
            RC_Pref_OS = 0x0041,
            RC_Pref_TR = 0x0042,
            RC_Pref_AF = 0x0043,
            RC_Pref_AB = 0x0044,
            RC_BodyKit = 0x0045,
            RC_BodyKit_Front = 0x0046,
            RC_BodyKit_Middle = 0x0047,
            RC_BodyKit_Rear = 0x0048,
            RC_BodyKit_Hood = 0x0049,
            RC_BodyKit_Wing = 0x0050,
            RC_BodyKit_Wheel = 0x0051,
            RC_Tex_TailLogo = 0x0052,
            RC_Tex_Diffuse = 0x0053,
            RC_Color_Body = 0x0054,
            RC_Color_Wheel = 0x0055,
            RC_Color_Glass = 0x0056,
            RC_Color_Tyre = 0x0057,
            RC_isAdult = 0x0058,
            RC_CreatDate = 0x0059,
            RC_UserNick = 0x0060,
            RC_playerSectionInfo = 0x0071,
            RC_changedValue = 0x0072,
            RC_ItemType = 0x0073,
            RC_ItemID = 0x0074,
            RC_BuyTime = 0x0075,
            RC_ItemName = 0x0062,
            RC_ItemCode = 0x0061,
            RayCity_GuildPoint = 0x2198,
            RayCity_CarIDX = 0x2001,
            RayCity_CharacterID = 0x2002,
            RayCity_CarID = 0x2003,
            RayCity_CarName = 0x2004,
            RayCity_CarType = 0x2005,
            RayCity_LastEquipInventoryIDX = 0x2006,
            RayCity_CarState = 0x2007,
            RayCity_CreateDate = 0x2008,
            RayCity_LastUpdateDate = 0x2009,
            RayCity_AccountID = 0x2011,
            RayCity_CharacterName = 0x2012,
            RayCity_RecentMailIDX = 0x2013,
            RayCity_RecentGiftIDX = 0x2014,
            RayCity_LastUseCarIDX = 0x2015,
            RayCity_GarageIDX = 0x2016,
            RayCity_LastTutorialID = 0x2017,
            RayCity_CharacterState = 0x2018,
            RayCity_FriendIDX = 0x2019,
            RayCity_FriendCharacterID = 0x2020,
            RayCity_FriendCharacterName = 0x2021,
            RayCity_FriendGroupIDX = 0x2022,
            RayCity_FriendState = 0x2023,
            RayCity_FriendGroupName = 0x2024,
            RayCity_FriendGroupType = 0x2025,
            RayCity_FriendGroupState = 0x2026,
            RayCity_CarInventoryIDX = 0x2028,
            RayCity_InventoryType = 0x2029,
            RayCity_MaxInventorySize = 0x2030,
            RayCity_CurrentInventorySize = 0x2031,
            RayCity_QuestLogIDX = 0x2032,
            RayCity_QuestID = 0x2033,
            RayCity_QuestState = 0x2034,
            RayCity_DealLogIDX = 0x2010,
            RayCity_ItemID = 0x2035,
            RayCity_BuyDealClientID = 0x2036,
            RayCity_SellDealClientID = 0x2037,
            RayCity_BuyPrice = 0x2038,
            RayCity_SellPrice = 0x2039,
            RayCity_DealLogState = 0x2040,
            RayCity_QuestOJTLogIDX = 0x2041,
            RayCity_QuestOJTIDX = 0x2042,
            RayCity_TaskValue = 0x2043,
            RayCity_ExecuteCount = 0x2044,
            RayCity_QuestOJTState = 0x2045,
            RayCity_CharacterLevel = 0x2046,
            RayCity_CharacterExp = 0x2047,
            RayCity_CharacterMoney = 0x2048,
            RayCity_CharacterMileage = 0x2049,
            RayCity_MaxCombo = 0x2050,
            RayCity_MaxSP = 0x2051,
            RayCity_MaxMailCount = 0x2052,
            RayCity_CurMailCount = 0x2053,
            RayCity_CurrentRP = 0x2054,
            RayCity_AccumulatedRP = 0x2055,
            RayCity_RelaxGauge = 0x2056,
            RayCity_StartPos_TownCode = 0x2057,
            RayCity_StartPos_FieldCode = 0x2058,
            RayCity_StartPos_X = 0x2059,
            RayCity_StartPos_Y = 0x2060,
            RayCity_StartPos_Z = 0x2061,
            RayCity_DealInventoryItemIDX = 0x2062,
            RayCity_InventoryCellNo = 0x2063,
            RayCity_DealInventoryItemState = 0x2065,
            RayCity_CarLevel = 0x2066,
            RayCity_CarExp = 0x2067,
            RayCity_CarMileage = 0x2068,
            RayCity_FuelQuantity = 0x2069,
            RayCity_MissionLogIDX = 0x2070,
            RayCity_MissionID = 0x2071,
            RayCity_TotMissionCnt = 0x2072,
            RayCity_TotMissionSuccessCnt = 0x2073,
            RayCity_TotMissionFailCnt = 0x2074,
            RayCity_TotEXP = 0x2075,
            RayCity_TotCarEXP = 0x2076,
            RayCity_TotIncoming = 0x2077,
            RayCity_BingoCardIDX = 0x2078,
            RayCity_BingoCardID = 0x2079,
            RayCity_BingoRewardType = 0x2080,
            RayCity_BingoRewardValue = 0x2081,
            RayCity_BingoRewardAmount = 0x2082,
            RayCity_BingoCardState = 0x2083,
            RayCity_NyUserID = 0x2084,
            RayCity_NyPassword = 0x2085,
            RayCity_NyNickName = 0x2086,
            RayCity_NyGender = 0x2087,
            RayCity_NyBirthYear = 0x2088,
            RayCity_AccountState = 0x2089,
            RayCity_CharacterType = 0x2090,
            RayCity_TotPlayTime = 0x2092,
            RayCity_LoginCount = 0x2093,
            RayCity_LastLoginTime = 0x2094,
            RayCity_LastLogoutTime = 0x2095,
            RayCity_LastLoginIPPrv = 0x2096,
            RayCity_LastLoginIPPub = 0x2097,
            RayCity_TodayPlayTime = 0x2098,
            RayCity_TodayOfflineTime = 0x2099,
            RayCity_IsLogin = 0x2100,
            RayCity_GuildMemberIDX = 0x2101,
            RayCity_GuildGroupIDX = 0x2102,
            RayCity_GuildMemberState = 0x2103,
            RayCity_GuildGroupName = 0x2104,
            RayCity_GuildGroupRole = 0x2105,
            RayCity_GuildGroupState = 0x2106,
            RayCity_GuildIDX = 0x2107,
            RayCity_GuildName = 0x2108,
            RayCity_GuildMessage = 0x2109,
            RayCity_MaxGuildMember = 0x2110,
            RayCity_CurGuildMember = 0x2111,
            RayCity_IncreaseItemCount = 0x2112,
            RayCity_TrackRaceWin = 0x2113,
            RayCity_TrackRaceLose = 0x2114,
            RayCity_FieldRaceWin = 0x2115,
            RayCity_FieldRaceLose = 0x2116,
            RayCity_GuildState = 0x2117,
            RayCity_ServerIP = 0x2118,
            RayCity_SoloRaceWin = 0x2119,
            RayCity_SoloRaceLose = 0x2120,
            RayCity_SoloRaceRetire = 0x2121,
            RayCity_TeamRaceWin = 0x2122,
            RayCity_TeamRaceLose = 0x2123,
            RayCity_FieldSoloRaceWin = 0x2124,
            RayCity_FieldSoloRaceLose = 0x2125,
            RayCity_FieldSoloRaceRetire = 0x2126,
            RayCity_FieldTeamRaceWin = 0x2127,
            RayCity_FieldTeamRaceLose = 0x2128,
            RayCity_ConnectionLogIDX = 0x2129,
            RayCity_ConnectionLogKey = 0x2130,
            RayCity_ServerID = 0x2131,
            RayCity_IPAddress = 0x2132,
            RayCity_ActionType = 0x2133,
            RayCity_ItemName = 0x2134,
            RayCity_BeginDate = 0x2135,
            RayCity_EndDate = 0x2136,
            RayCity_Title = 0x2137,
            RayCity_Message = 0x2138,
            RayCity_ItemIDX = 0x2139,
            RayCity_Bonding = 0x2140,
            RayCity_MaxEndurance = 0x2141,
            RayCity_CurEndurance = 0x2142,
            RayCity_ExpireDate = 0x2143,
            RayCity_ItemOption1 = 0x2144,
            RayCity_ItemOption2 = 0x2145,
            RayCity_ItemOption3 = 0x2146,
            RayCity_ItemState = 0x2147,
            RayCity_ItemPrice = 0x2148,
            RayCity_BeforeCharacterMoney = 0x2149,
            RayCity_AfterCharacterMoney = 0x2150,
            RayCity_MoneyType = 0x2151,
            RayCity_ItemBuySellLogIDX = 0x2152,
            RayCity_RewardExp = 0x2153,
            RayCity_RewardCarExp = 0x2154,
            RayCity_RewardMoney = 0x2155,
            RayCity_MissionGrade = 0x2156,
            RayCity_MissionResult = 0x2157,
            RayCity_Duration = 0x2158,
            RayCity_MoneyLogIDX = 0x2159,
            RayCity_AdjustType = 0x2160,
            RayCity_UpdateSource = 0x2161,
            RayCity_MoneyAmount = 0x2162,
            RayCity_RaceLogIDX = 0x2163,
            RayCity_RaceID = 0x2164,
            RayCity_RaceType = 0x2165,
            RayCity_TrackID = 0x2166,
            RayCity_RewardRP_RankBase = 0x2167,
            RayCity_RewardRP_TimeBase = 0x2168,
            RayCity_Rank = 0x2170,
            RayCity_RaceTime = 0x2171,
            RayCity_RaceResult = 0x2172,
            RayCity_FixedTime = 0x2173,
            RayCity_SkillID = 0x2174,
            RayCity_SkillName = 0x2175,
            RayCity_SkillLevel = 0x2176,
            RayCity_SkillIDX = 0x2177,
            RayCity_ItemTypeID = 0x2178,
            RayCity_ItemTypeName = 0x2179,
            RayCity_TradeWaitItemIDX = 0x2180,
            RayCity_CarInventoryItemIDX = 0x2181,
            RayCity_TradeWaitCellNo = 0x2182,
            RayCity_TargetCarInventoryIDX = 0x2183,
            RayCity_TargetInventoryCellNo = 0x2184,
            RayCity_TradeWaitItemState = 0x2185,
            RayCity_TradeSessionIDX = 0x2186,
            RayCity_TargetTradeSessionIDX = 0x2187,
            RayCity_TargetCharacterID = 0x2188,
            RayCity_TradeMoney = 0x2189,
            RayCity_TradeSessionState = 0x2190,
            RayCity_ActionTypeName = 0x2191,
            RayCity_StartNum = 0x2192,
            RayCity_EndNum = 0x2193,
            RayCity_NyCashChargeLogIDX = 0x2194,
            RayCity_NyPayID = 0x2195,
            RayCity_ChargeAmount = 0x2196,
            RayCity_ChargeState = 0x2197,
            RayCity_SendDate = 0x3150,//��������
            RayCity_FromCharacterID = 0x3143,
            RayCity_FromCharacterName = 0x3144,
            RayCity_PaymentType = 0x3145,
            RayCity_CashItemLogIDX = 0x3146,
            RayCity_StockID = 0x3147,
            RayCity_GiftMessage = 0x3148,
            RayCity_GiftState = 0x3149,
            RayCity_QuestName = 0x3142,
            RayCity_NyCashBalance = 0x2091, //��� INT
            RayCity_TargetName = 0x2199,

            RayCity_CouponIDX = 0x3151, //���к�
            RayCity_CountryCode = 0x3152,
            RayCity_CouponName = 0x3153,
            RayCity_IssueCount = 0x3154,
            RayCity_StartDate = 0x3155,
            RayCity_CouponState = 0x3156,
            RayCity_CouponNumber = 0x3157,
            RayCity_IssueCouponIDX = 0x3158,

            RayCity_rccdkey = 0x3159,
            RayCity_getuser = 0x3160,
            RayCity_gettime = 0x3161,
            RayCity_use_state = 0x3162,
            RayCity_activename = 0x3163,
            RayCity_Interval = 0x3164,//TLV_INTEGER
            RayCity_NoticeID = 0x3165,//TLV_INTEGER
            RayCity_Repeat = 0x3166,//TLV_INTEGER  �ظ�����
            RC_ItemInstanceID = 0x9015,
            RC_SenderID = 0x9016,
            RC_SenderOperation = 0x9017,
            RC_ReceiverID = 0x9018,
            RC_ReceiveTime = 0x9019,
            RC_ReceiverOperation = 0x9020,

            RC_PlayerID = 0x9021,
            RC_PlayerOperation = 0x9022,
            RC_OperationTime = 0x9023,
            RC_MoneyCount = 0x9024,
            RC_login_account = 0x9025,
            RC_state_number = 0x9026,
            RC_usernick = 0x9027,
            RC_DBname = 0x9028,
            RC_TimeLoop = 0x9029,
            RC_Text = 0x9030,
            RC_ChannelServerID = 0x9031,
            RC_MultipleType = 0x9032,
            RC_MultipleValue = 0x9033,
            RC_UseAccount = 0x9034,
            RC_GlobalAccount = 0x9035,
            RC_UseTime = 0x9036,
            RC_ActiveTime = 0x9037,
            RC_chNotes = 0x9039,

            RC_CurrentPartnerID = 0x9041, //int
            RC_CurrentTitleID = 0x9042,
            RC_I9youMoney = 0x9043,//int M�� 

            RC_OperationType = 0x9056,//int
            RC_ItemBigType = 0x9057,//int
            RC_TeamID = 0x9058,
            RC_Job = 0x9060,
            RC_JoinTime = 0x9061,
            RC_Name = 0x9062,
            RC_TeamLevel = 0x9063,
            RC_FoundTime = 0x9064,
            RC_Founder = 0x9065,
            RC_Picture = 0x9066,
            RC_OnlinePercent = 0x9067,
            RC_Distance = 0x9068,
            RC_Pronouncement = 0x9069,
            RC_Bulletin = 0x9070,
            #endregion
            FJ_BoardFlag = 0x1858,//Format:INT ״̬
            FJ_Sex = 0x1803,
            FJ_descr = 0x1958,

            ERROR_Msg = 0xFFFF //Format:STRING  ��֤�û�����Ϣ
        }
        /// <summary>
        /// �������ͱ�ǩ
        /// </summary>
        public enum TagFormat : byte
        {
            TLV_STRING = 0,
            TLV_MONEY = 1,
            TLV_DATE = 2,
            TLV_INTEGER = 3,
            TLV_EXTEND = 4,
            TLV_NUMBER = 5,
            TLV_TIME = 6,
            TLV_TIMESTAMP = 7,
            TLV_BOOLEAN = 8,
            TLV_UNICODE = 9,
        }
        /// <summary>
        /// ���������ǩ
        /// </summary>
        public enum ServiceKey : ushort
        {
            /// <summary>
            /// ����ģ��(0x80)
            /// </summary>
            CONNECT = 0x0001,
            CONNECT_RESP = 0x8001,
            DISCONNECT = 0x0002,
            DISCONNECT_RESP = 0x8002,
            ACCOUNT_AUTHOR = 0x0003,
            ACCOUNT_AUTHOR_RESP = 0x8003,

            SERVERINFO_IP_QUERY = 0x0004,
            SERVERINFO_IP_QUERY_RESP = 0x8004,
            GMTOOLS_OperateLog_Query = 0x0005,//�鿴���߲�����¼
            GMTOOLS_OperateLog_Query_RESP = 0x8005,//�鿴���߲�����¼��Ӧ
            SERVERINFO_IP_ALL_QUERY = 0x0006,//�鿴������Ϸ��������ַ
            SERVERINFO_IP_ALL_QUERY_RESP = 0x8006,//�鿴������Ϸ��������ַ��Ӧ
            LINK_SERVERIP_CREATE = 0x0007,//�����Ϸ�������ݿ�
            LINK_SERVERIP_CREATE_RESP = 0x8007,//�����Ϸ�������ݿ���Ӧ
            CLIENT_PATCH_COMPARE = 0x0008,
            CLIENT_PATCH_COMPARE_RESP = 0x8008,
            CLIENT_PATCH_UPDATE = 0x0009,
            CLIENT_PATCH_UPDATE_RESP = 0x8009,

            /// <summary>
            ///�û�����ģ��(0x81) 
            /// </summary>
            USER_CREATE = 0x0001,
            USER_CREATE_RESP = 0x8001,
            USER_UPDATE = 0x0002,
            USER_UPDATE_RESP = 0x8002,
            USER_DELETE = 0x0003,
            USER_DELETE_RESP = 0x8003,
            USER_QUERY = 0x0004,
            USER_QUERY_RESP = 0x8004,
            USER_PASSWD_MODIF = 0x0005,
            USER_PASSWD_MODIF_RESP = 0x8005,
            USER_QUERY_ALL = 0x0006,
            USER_QUERY_ALL_RESP = 0x8006,
            DEPART_QUERY = 0x0007,
            DEPART_QUERY_RESP = 0x8007,
            UPDATE_ACTIVEUSER = 0x0008,//���������û�״̬
            UPDATE_ACTIVEUSER_RESP = 0x8008,//���������û�״̬��Ӧ

            /// <summary>
            /// ģ�����(0x82)
            /// </summary>
            MODULE_CREATE = 0x0001,
            MODULE_CREATE_RESP = 0x8001,
            MODULE_UPDATE = 0x0002,
            MODULE_UPDATE_RESP = 0x8002,
            MODULE_DELETE = 0x0003,
            MODULE_DELETE_RESP = 0x8003,
            MODULE_QUERY = 0x0004,
            MODULE_QUERY_RESP = 0x8004,

            /// <summary>
            /// �û���ģ��(0x83) 
            /// </summary>
            USER_MODULE_CREATE = 0x0001,
            USER_MODULE_CREATE_RESP = 0x8001,
            USER_MODULE_UPDATE = 0x0002,
            USER_MODULE_UPDATE_RESP = 0x8002,
            USER_MODULE_DELETE = 0x0003,
            USER_MODULE_DELETE_RESP = 0x8003,
            USER_MODULE_QUERY = 0x0004,
            USER_MODULE_QUERY_RESP = 0x8004,

            /// <summary>
            /// ��Ϸ����(0x84) 
            /// </summary>
            GAME_CREATE = 0x0001,
            GAME_CREATE_RESP = 0x8001,
            GAME_UPDATE = 0x0002,
            GAME_UPDATE_RESP = 0x8002,
            GAME_DELETE = 0x0003,
            GAME_DELETE_RESP = 0x8003,
            GAME_QUERY = 0x0004,
            GAME_QUERY_RESP = 0x8004,
            GAME_MODULE_QUERY = 0x0005,
            GAME_MODULE_QUERY_RESP = 0x8005,

            /// <summary>
            /// NOTES����(0x85) 
            /// </summary>
            NOTES_LETTER_TRANSFER = 0x0001,
            NOTES_LETTER_TRANSFER_RESP = 0x8001,
            NOTES_LETTER_PROCESS = 0x0002, //�ʼ�����
            NOTES_LETTER_PROCESS_RESP = 0x8002,//�ʼ�����
            NOTES_LETTER_TRANSMIT = 0x0003,//�ʼ�ת��
            NOTES_LETTER_TRANSMIT_RESP = 0x8003,//�ʼ�ת����Ӧ

            /// <summary>
            /// �ͽ�GM���߹���(0x86)
            /// </summary>
            MJ_CHARACTERINFO_QUERY = 0x0001,//������״̬
            MJ_CHARACTERINFO_QUERY_RESP = 0x8001,//������״̬��Ӧ
            MJ_CHARACTERINFO_UPDATE = 0x0002,//�޸����״̬
            MJ_CHARACTERINFO_UPDATE_RESP = 0x8002,//�޸����״̬��Ӧ
            MJ_LOGINTABLE_QUERY = 0x0003,//�������Ƿ�����
            MJ_LOGINTABLE_QUERY_RESP = 0x8003,//�������Ƿ�������Ӧ
            MJ_CHARACTERINFO_EXPLOIT_UPDATE = 0x0004,//�޸Ĺ�ѫֵ
            MJ_CHARACTERINFO_EXPLOIT_UPDATE_RESP = 0x8004,//�޸Ĺ�ѫֵ��Ӧ
            MJ_CHARACTERINFO_FRIEND_QUERY = 0x0005,//�г���������
            MJ_CHARACTERINFO_FRIEND_QUERY_RESP = 0x8005,//�г�����������Ӧ
            MJ_CHARACTERINFO_FRIEND_CREATE = 0x0006,//��Ӻ���
            MJ_CHARACTERINFO_FRIEND_CREATE_RESP = 0x8006,//��Ӻ�����Ӧ
            MJ_CHARACTERINFO_FRIEND_DELETE = 0x0007,//ɾ������
            MJ_CHARACTERINFO_FRIEND_DELETE_RESP = 0x8007,//ɾ��������Ӧ
            MJ_GUILDTABLE_UPDATE = 0x0008,//�޸ķ����������Ѵ��ڰ��
            MJ_GUILDTABLE_UPDATE_RESP = 0x8008,//�޸ķ����������Ѵ��ڰ����Ӧ
            MJ_ACCOUNT_LOCAL_CREATE = 0x0009,//���������ϵ�account����������Ϣ���浽���ط�������
            MJ_ACCOUNT_LOCAL_CREATE_RESP = 0x8009,//���������ϵ�account����������Ϣ���浽���ط���������Ӧ
            MJ_ACCOUNT_REMOTE_DELETE = 0x0010,//���÷�ͣ�ʺ�
            MJ_ACCOUNT_REMOTE_DELETE_RESP = 0x8010,//���÷�ͣ�ʺŵ���Ӧ
            MJ_ACCOUNT_REMOTE_RESTORE = 0x0011,//����ʺ�
            MJ_ACCOUNT_REMOTE_RESTORE_RESP = 0x8011,//����ʺ���Ӧ
            MJ_ACCOUNT_LIMIT_RESTORE = 0x0012,//��ʱ�޵ķ�ͣ
            MJ_ACCOUNT_LIMIT_RESTORE_RESP = 0x8012,//��ʱ�޵ķ�ͣ��Ӧ
            MJ_ACCOUNTPASSWD_LOCAL_CREATE = 0x0013,//����������뵽���� 
            MJ_ACCOUNTPASSWD_LOCAL_CREATE_RESP = 0x8013,//����������뵽����
            MJ_ACCOUNTPASSWD_REMOTE_UPDATE = 0x0014,//�޸�������� 
            MJ_ACCOUNTPASSWD_REMOTE_UPDATE_RESP = 0x8014,//�޸��������
            MJ_ACCOUNTPASSWD_REMOTE_RESTORE = 0x0015,//�ָ��������
            MJ_ACCOUNTPASSWD_REMOTE_RESTORE_RESP = 0x8015,//�ָ��������
            MJ_ITEMLOG_QUERY = 0x0016,//�����û����׼�¼
            MJ_ITEMLOG_QUERY_RESP = 0x8016,//�����û����׼�¼
            MJ_GMTOOLS_LOG_QUERY = 0x0017,//���ʹ���߲�����¼
            MJ_GMTOOLS_LOG_QUERY_RESP = 0x8017,//���ʹ���߲�����¼
            MJ_MONEYSORT_QUERY = 0x0018,//���ݽ�Ǯ����
            MJ_MONEYSORT_QUERY_RESP = 0x8018,//���ݽ�Ǯ�������Ӧ
            MJ_LEVELSORT_QUERY = 0x0019,//���ݵȼ�����
            MJ_LEVELSORT_QUERY_RESP = 0x8019,//���ݵȼ��������Ӧ
            MJ_MONEYFIGHTERSORT_QUERY = 0x0020,//���ݲ�ְͬҵ��Ǯ����
            MJ_MONEYFIGHTERSORT_QUERY_RESP = 0x8020,//���ݲ�ְͬҵ��Ǯ�������Ӧ
            MJ_LEVELFIGHTERSORT_QUERY = 0x0021,//���ݲ�ְͬҵ�ȼ�����
            MJ_LEVELFIGHTERSORT_QUERY_RESP = 0x8021,//���ݲ�ְͬҵ�ȼ��������Ӧ
            MJ_MONEYTAOISTSORT_QUERY = 0x0022,//���ݵ�ʿ��Ǯ����
            MJ_MONEYTAOISTSORT_QUERY_RESP = 0x8022,//���ݵ�ʿ��Ǯ�������Ӧ
            MJ_LEVELTAOISTSORT_QUERY = 0x0023,//���ݵ�ʿ�ȼ�����
            MJ_LEVELTAOISTSORT_QUERY_RESP = 0x8023,//���ݵ�ʿ�ȼ��������Ӧ
            MJ_MONEYRABBISORT_QUERY = 0x0024,//���ݷ�ʦ��Ǯ����
            MJ_MONEYRABBISORT_QUERY_RESP = 0x8024,//���ݷ�ʦ��Ǯ�������Ӧ
            MJ_LEVELRABBISORT_QUERY = 0x0025,//���ݷ�ʦ�ȼ�����
            MJ_LEVELRABBISORT_QUERY_RESP = 0x8025,//���ݷ�ʦ�ȼ��������Ӧ
            MJ_ACCOUNT_QUERY = 0x0026,//�ͽ��ʺŲ�ѯ
            MJ_ACCOUNT_QUERY_RESP = 0x8026,//�ͽ��ʺŲ�ѯ��Ӧ
            MJ_ACCOUNT_LOCAL_QUERY = 0x0027,//��ѯ�ͽ������ʺ�
            MJ_ACCOUNT_LOCAL_QUERY_RESP = 0x8027,//��ѯ�ͽ������ʺ���Ӧ

            /// <summary>
            /// SDO_ADMIN �������߹��߲�����Ϣ��
            /// </summary>
            SDO_ACCOUNT_QUERY = 0x0026,//�鿴��ҵ��ʺ���Ϣ
            SDO_ACCOUNT_QUERY_RESP = 0x8026,//�鿴��ҵ��ʺ���Ϣ��Ӧ
            SDO_CHARACTERINFO_QUERY = 0x0027,//�鿴�������ϵ���Ϣ
            SDO_CHARACTERINFO_QUERY_RESP = 0x8027,//�鿴�������ϵ���Ϣ��Ӧ
            SDO_ACCOUNT_CLOSE = 0x0028,//��ͣ�ʻ���Ȩ����Ϣ
            SDO_ACCOUNT_CLOSE_RESP = 0x8028,//��ͣ�ʻ���Ȩ����Ϣ��Ӧ
            SDO_ACCOUNT_OPEN = 0x0029,//����ʻ���Ȩ����Ϣ
            SDO_ACCOUNT_OPEN_RESP = 0x8029,//����ʻ���Ȩ����Ϣ��Ӧ
            SDO_PASSWORD_RECOVERY = 0x0030,//����һ�����
            SDO_PASSWORD_RECOVERY_RESP = 0x8030,//����һ�������Ӧ
            SDO_CONSUME_QUERY = 0x0031,//�鿴��ҵ����Ѽ�¼
            SDO_CONSUME_QUERY_RESP = 0x8031,//�鿴��ҵ����Ѽ�¼��Ӧ
            SDO_USERONLINE_QUERY = 0x0032,//�鿴���������״̬
            SDO_USERONLINE_QUERY_RESP = 0x8032,//�鿴���������״̬��Ӧ
            SDO_USERTRADE_QUERY = 0x0033,//�鿴��ҽ���״̬
            SDO_USERTRADE_QUERY_RESP = 0x8033,//�鿴��ҽ���״̬��Ӧ
            SDO_CHARACTERINFO_UPDATE = 0x0034,//�޸���ҵ��˺���Ϣ
            SDO_CHARACTERINFO_UPDATE_RESP = 0x8034,//�޸���ҵ��˺���Ϣ��Ӧ
            SDO_ITEMSHOP_QUERY = 0x0035,//�鿴��Ϸ�������е�����Ϣ
            SDO_ITEMSHOP_QUERY_RESP = 0x8035,//�鿴��Ϸ�������е�����Ϣ��Ӧ
            SDO_ITEMSHOP_DELETE = 0x0036,//ɾ����ҵ�����Ϣ
            SDO_ITEMSHOP_DELETE_RESP = 0x8036,//ɾ����ҵ�����Ϣ��Ӧ
            SDO_GIFTBOX_CREATE = 0x0037,//����������е�����Ϣ
            SDO_GIFTBOX_CREATE_RESP = 0x8037,//����������е�����Ϣ��Ӧ
            SDO_GIFTBOX_QUERY = 0x0038,//�鿴�������еĵ���
            SDO_GIFTBOX_QUERY_RESP = 0x8038,//�鿴�������еĵ�����Ӧ
            SDO_GIFTBOX_DELETE = 0x0039,//ɾ���������еĵ���
            SDO_GIFTBOX_DELETE_RESP = 0x8039,//ɾ���������еĵ�����Ӧ
            SDO_USERLOGIN_STATUS_QUERY = 0x0040,//�鿴��ҵ�¼״̬
            SDO_USERLOGIN_STATUS_QUERY_RESP = 0x8040,//�鿴��ҵ�¼״̬��Ӧ
            SDO_ITEMSHOP_BYOWNER_QUERY = 0x0041,////�鿴������ϵ�����Ϣ
            SDO_ITEMSHOP_BYOWNER_QUERY_RESP = 0x8041,////�鿴������ϵ�����Ϣ
            SDO_ITEMSHOP_TRADE_QUERY = 0x0042,//�鿴��ҽ��׼�¼��Ϣ
            SDO_ITEMSHOP_TRADE_QUERY_RESP = 0x8042,//�鿴��ҽ��׼�¼��Ϣ����Ӧ
            SDO_MEMBERSTOPSTATUS_QUERY = 0x0043,//�鿴���ʺ�״̬
            SDO_MEMBERSTOPSTATUS_QUERY_RESP = 0x8043,///�鿴���ʺ�״̬����Ӧ
            SDO_MEMBERBANISHMENT_QUERY = 0x0044,//�鿴����ͣ����ʺ�
            SDO_MEMBERBANISHMENT_QUERY_RESP = 0x8044,//�鿴����ͣ����ʺ���Ӧ
            SDO_USERMCASH_QUERY = 0x0045,//��ҳ�ֵ��¼��ѯ
            SDO_USERMCASH_QUERY_RESP = 0x8045,//��ҳ�ֵ��¼��ѯ��Ӧ
            SDO_USERGCASH_UPDATE = 0x0046,//�������G��
            SDO_USERGCASH_UPDATE_RESP = 0x8046,//�������G�ҵ���Ӧ
            SDO_MEMBERLOCAL_BANISHMENT = 0x0047,//���ر���ͣ����Ϣ
            SDO_MEMBERLOCAL_BANISHMENT_RESP = 0x8047,//���ر���ͣ����Ϣ��Ӧ
            SDO_EMAIL_QUERY = 0x0048,//�õ���ҵ�EMAIL
            SDO_EMAIL_QUERY_RESP = 0x8048,//�õ���ҵ�EMAIL��Ӧ
            SDO_USERCHARAGESUM_QUERY = 0x0049,//�õ���ֵ��¼�ܺ�
            SDO_USERCHARAGESUM_QUERY_RESP = 0x8049,//�õ���ֵ��¼�ܺ���Ӧ
            SDO_USERCONSUMESUM_QUERY = 0x0050,//�õ����Ѽ�¼�ܺ�
            SDO_USERCONSUMESUM_QUERY_RESP = 0x8050,//�õ����Ѽ�¼�ܺ���Ӧ
            SDO_USERGCASH_QUERY = 0x0051,//���?�Ҽ�¼��ѯ
            SDO_USERGCASH_QUERY_RESP = 0x8051,//���?�Ҽ�¼��ѯ��Ӧ

            SDO_USERNICK_UPDATE = 0x0069,
            SDO_USERNICK_UPDATE_RESP = 0x8069,

            SDO_PADKEYWORD_QUERY = 0x0070,
            SDO_PADKEYWORD_QUERY_RESP = 0x8070,

            SDO_BOARDMESSAGE_REQ = 0x0071,
            SDO_BOARDMESSAGE_REQ_RESP = 0x8071,

            SDO_CHANNELLIST_QUERY = 0x0072,
            SDO_CHANNELLIST_QUERY_RESP = 0x8072,
            SDO_ALIVE_REQ = 0x0073,
            SDO_ALIVE_REQ_RESP = 0x8073,

            SDO_BOARDTASK_QUERY = 0x0074,
            SDO_BOARDTASK_QUERY_RESP = 0x8074,

            SDO_BOARDTASK_UPDATE = 0x0075,
            SDO_BOARDTASK_UPDATE_RESP = 0x8075,

            SDO_BOARDTASK_INSERT = 0x0076,
            SDO_BOARDTASK_INSERT_RESP = 0x8076,
            SDO_DAYSLIMIT_QUERY = 0x0077,//��Ч�ڲ�ѯ
            SDO_DAYSLIMIT_QUERY_RESP = 0x8077,
            SDO_FRIENDS_QUERY = 0x0078,//
            SDO_FRIENDS_QUERY_RESP = 0x8078,

            SDO_USERLOGIN_DEL = 0x0079,
            SDO_USERLOGIN_DEL_RESP = 0x8079,
            SDO_USERLOGIN_CLEAR = 0x0080,
            SDO_USERLOGIN_CLEAR_RESP = 0x8080,
            SDO_GATEWAY_QUERY = 0x0081,
            SDO_GATEWAY_QUERY_RESP = 0x8081,

            SDO_USERINTEGRAL_QUERY = 0x0082,
            SDO_USERINTEGRAL_QUERY_RESP = 0x8082,

            SDO_StageAward_Query = 0x0111,
            SDO_StageAward_Query_RESP = 0x8111,
            SDO_StageAward_Create = 0x0112,
            SDO_StageAward_Create_RESP = 0x8112,
            SDO_StageAward_Delete = 0x0113,
            SDO_StageAward_Delete_RESP = 0x8113,
            SDO_StageAward_Update = 0x0114,
            SDO_StageAward_Update_RESP = 0x8114,
            SDO_BAOXIANGRate_Query = 0x0120,
            SDO_BAOXIANGRate_Query_RESP = 0x8120,
            SDO_BAOXIANGRate_Update = 0x0121,
            SDO_BAOXIANGRate_Update_RESP = 0x8121,
            /// <summary>
            /// AU_ADMIN �����Ź��߲�����Ϣ��
            /// </summary>
            AU_ACCOUNT_QUERY = 0x0001,//����ʺ���Ϣ��ѯ
            AU_ACCOUNT_QUERY_RESP = 0x8001,//����ʺ���Ϣ��ѯ��Ӧ
            AU_ACCOUNTREMOTE_QUERY = 0x0002,//��Ϸ��������ͣ������ʺŲ�ѯ
            AU_ACCOUNTREMOTE_QUERY_RESP = 0x8002,//��Ϸ��������ͣ������ʺŲ�ѯ��Ӧ
            AU_ACCOUNTLOCAL_QUERY = 0x0003,//���ط�ͣ������ʺŲ�ѯ
            AU_ACCOUNTLOCAL_QUERY_RESP = 0x8003,//���ط�ͣ������ʺŲ�ѯ��Ӧ
            AU_ACCOUNT_CLOSE = 0x0004,//��ͣ������ʺ�
            AU_ACCOUNT_CLOSE_RESP = 0x8004,//��ͣ������ʺ���Ӧ
            AU_ACCOUNT_OPEN = 0x0005,//��������ʺ�
            AU_ACCOUNT_OPEN_RESP = 0x8005,//��������ʺ���Ӧ
            AU_ACCOUNT_BANISHMENT_QUERY = 0x0006,//��ҷ�ͣ�ʺŲ�ѯ
            AU_ACCOUNT_BANISHMENT_QUERY_RESP = 0x8006,//��ҷ�ͣ�ʺŲ�ѯ��Ӧ
            AU_CHARACTERINFO_QUERY = 0x0007,//��ѯ��ҵ��˺���Ϣ
            AU_CHARACTERINFO_QUERY_RESP = 0x8007,//��ѯ��ҵ��˺���Ϣ��Ӧ
            AU_CHARACTERINFO_UPDATE = 0x0008,//�޸���ҵ��˺���Ϣ
            AU_CHARACTERINFO_UPDATE_RESP = 0x8008,//�޸���ҵ��˺���Ϣ��Ӧ
            AU_ITEMSHOP_QUERY = 0x0009,//�鿴��Ϸ�������е�����Ϣ
            AU_ITEMSHOP_QUERY_RESP = 0x8009,//�鿴��Ϸ�������е�����Ϣ��Ӧ
            AU_ITEMSHOP_DELETE = 0x0010,//ɾ����ҵ�����Ϣ
            AU_ITEMSHOP_DELETE_RESP = 0x8010,//ɾ����ҵ�����Ϣ��Ӧ
            AU_ITEMSHOP_BYOWNER_QUERY = 0x0011,////�鿴������ϵ�����Ϣ
            AU_ITEMSHOP_BYOWNER_QUERY_RESP = 0x8011,////�鿴������ϵ�����Ϣ
            AU_ITEMSHOP_TRADE_QUERY = 0x0012,//�鿴��ҽ��׼�¼��Ϣ
            AU_ITEMSHOP_TRADE_QUERY_RESP = 0x8012,//�鿴��ҽ��׼�¼��Ϣ����Ӧ
            AU_ITEMSHOP_CREATE = 0x0013,//����������е�����Ϣ
            AU_ITEMSHOP_CREATE_RESP = 0x8013,//����������е�����Ϣ��Ӧ
            AU_LEVELEXP_QUERY = 0x0014,//�鿴��ҵȼ�����
            AU_LEVELEXP_QUERY_RESP = 0x8014,////�鿴��ҵȼ�������Ӧ
            AU_USERLOGIN_STATUS_QUERY = 0x0015,//�鿴��ҵ�¼״̬
            AU_USERLOGIN_STATUS_QUERY_RESP = 0x8015,//�鿴��ҵ�¼״̬��Ӧ
            AU_USERCHARAGESUM_QUERY = 0x0016,//�õ���ֵ��¼�ܺ�
            AU_USERCHARAGESUM_QUERY_RESP = 0x8016,//�õ���ֵ��¼�ܺ���Ӧ
            AU_CONSUME_QUERY = 0x0017,//�鿴��ҵ����Ѽ�¼
            AU_CONSUME_QUERY_RESP = 0x8017,//�鿴��ҵ����Ѽ�¼��Ӧ
            AU_USERCONSUMESUM_QUERY = 0x0018,//�õ����Ѽ�¼�ܺ�
            AU_USERCONSUMESUM_QUERY_RESP = 0x8018,//�õ����Ѽ�¼�ܺ���Ӧ
            AU_USERMCASH_QUERY = 0x0019,//��ҳ�ֵ��¼��ѯ
            AU_USERMCASH_QUERY_RESP = 0x8019,//��ҳ�ֵ��¼��ѯ��Ӧ
            AU_USERGCASH_QUERY = 0x0020,//���?�Ҽ�¼��ѯ
            AU_USERGCASH_QUERY_RESP = 0x8020,//���?�Ҽ�¼��ѯ��Ӧ
            AU_USERGCASH_UPDATE = 0x0021,//�������G��
            AU_USERGCASH_UPDATE_RESP = 0x8021,//�������G�ҵ���Ӧ


            Au_User_Msg_Query = 0x0062, //��ѯ�������Ϸ�е������¼ 
            Au_User_Msg_Query_Resp = 0x8062,//��ѯ�������Ϸ�е������¼    

            Au_BroaditeminfoNow_Query = 0x0059,//��ǰʱ���û�����������־
            Au_BroaditeminfoNow_Query_Resp = 0x8059,//��ǰʱ���û�����������־

            Au_BroaditeminfoBymsg_Query = 0x0060,//����ģ����ѯ�û�����������־
            Au_BroaditeminfoBymsg_Query_Resp = 0x8060,//����ģ����ѯ�û�����������־

            AU_MsgServerinfo_Query = 0x0018,
            AU_MsgServerinfo_Query_RESP = 0x8018,
            /// <summary>
            /// CR_ADMIN ��񿨶������߲�����Ϣ��
            /// </summary>
            CR_ACCOUNT_QUERY = 0x0001,//����ʺ���Ϣ��ѯ
            CR_ACCOUNT_QUERY_RESP = 0x8001,//����ʺ���Ϣ��ѯ��Ӧ
            CR_ACCOUNTACTIVE_QUERY = 0x0002,//����ʺż�����Ϣ
            CR_ACCOUNTACTIVE_QUERY_RESP = 0x8002,//����ʺż�����Ӧ
            CR_CALLBOARD_QUERY = 0x0003,//������Ϣ��ѯ
            CR_CALLBOARD_QUERY_RESP = 0x8003,//������Ϣ��ѯ��Ӧ
            CR_CALLBOARD_CREATE = 0x0004,//��������
            CR_CALLBOARD_CREATE_RESP = 0x8004,//����������Ӧ
            CR_CALLBOARD_UPDATE = 0x0005,//���¹�����Ϣ
            CR_CALLBOARD_UPDATE_RESP = 0x8005,//���¹�����Ϣ����Ӧ
            CR_CALLBOARD_DELETE = 0x0006,//ɾ��������Ϣ
            CR_CALLBOARD_DELETE_RESP = 0x8006,//ɾ��������Ϣ����Ӧ

            CR_CHARACTERINFO_QUERY = 0x0007,//��ҽ�ɫ��Ϣ��ѯ
            CR_CHARACTERINFO_QUERY_RESP = 0x8007,//��ҽ�ɫ��Ϣ��ѯ����Ӧ
            CR_CHARACTERINFO_UPDATE = 0x0008,//��ҽ�ɫ��Ϣ��ѯ
            CR_CHARACTERINFO_UPDATE_RESP = 0x8008,//��ҽ�ɫ��Ϣ��ѯ����Ӧ
            CR_CHANNEL_QUERY = 0x0009,//����Ƶ����ѯ
            CR_CHANNEL_QUERY_RESP = 0x8009,//����Ƶ����ѯ����Ӧ
            CR_NICKNAME_QUERY = 0x0010,//����ǳƲ�ѯ
            CR_NICKNAME_QUERY_RESP = 0x8010,//����ǳƵ���Ӧ
            CR_LOGIN_LOGOUT_QUERY = 0x0011,//������ߡ�����ʱ���ѯ
            CR_LOGIN_LOGOUT_QUERY_RESP = 0x8011,//������ߡ�����ʱ���ѯ����Ӧ
            CR_ERRORCHANNEL_QUERY = 0x0012,//������󹫸�Ƶ����ѯ
            CR_ERRORCHANNEL_QUERY_RESP = 0x8012,//������󹫸�Ƶ����ѯ����Ӧ


            /// <summary>
            /// ��ֵ����GM����(0x90)
            /// </summary>
            CARD_USERCHARGEDETAIL_QUERY = 0x0001,//һ��ͨ��ѯ
            CARD_USERCHARGEDETAIL_QUERY_RESP = 0x8001,//һ��ͨ��ѯ��Ӧ
            CARD_USERDETAIL_QUERY = 0x0002,//��֮���û�ע����Ϣ��ѯ
            CARD_USERDETAIL_QUERY_RESP = 0x8002,//��֮���û�ע����Ϣ��ѯ��Ӧ
            CARD_USERCONSUME_QUERY = 0x0003,//���б����Ѳ�ѯ
            CARD_USERCONSUME_QUERY_RESP = 0x8003,//���б����Ѳ�ѯ��Ӧ
            CARD_VNETCHARGE_QUERY = 0x0004,//�����ǿճ�ֵ��ѯ
            CARD_VNETCHARGE_QUERY_RESP = 0x8004,//�����ǿճ�ֵ��ѯ��Ӧ
            CARD_USERDETAIL_SUM_QUERY = 0x0005,//��ֵ�ϼƲ�ѯ
            CARD_USERDETAIL_SUM_QUERY_RESP = 0x8005,//��ֵ�ϼƲ�ѯ��Ӧ
            CARD_USERCONSUME_SUM_QUERY = 0x0006,//���ѺϼƲ�ѯ
            CARD_USERCONSUME_SUM_QUERY_RESP = 0x8006,//���Ѻϼ���Ӧ
            CARD_USERINFO_QUERY = 0x0007,//���ע����Ϣ��ѯ
            CARD_USERINFO_QUERY_RESP = 0x8007,//���ע����Ϣ��ѯ��Ӧ
            CARD_USERINFO_CLEAR = 0x0008,
            CARD_USERINFO_CLEAR_RESP = 0x8008,
            CARD_USERINITACTIVE_QUERY = 0x0015,//������Ϸ
            CARD_USERINITACTIVE_QUERY_RESP = 0x8015,

            /// <summary>
            /// �������̳�(0x91)
            /// </summary>
            AUSHOP_USERGPURCHASE_QUERY = 0x0001,//�û�G�ҹ����¼
            AUSHOP_USERGPURCHASE_QUERY_RESP = 0x8001,//�û�G�ҹ����¼
            AUSHOP_USERMPURCHASE_QUERY = 0x0002,//�û�M�ҹ����¼
            AUSHOP_USERMPURCHASE_QUERY_RESP = 0x8002,//�û�M�ҹ����¼
            AUSHOP_AVATARECOVER_QUERY = 0x0003,//���߻��նһ���
            AUSHOP_AVATARECOVER_QUERY_RESP = 0x8003,//���߻��նһ���
            AUSHOP_USERINTERGRAL_QUERY = 0x0004,//�û����ּ�¼
            AUSHOP_USERINTERGRAL_QUERY_RESP = 0x8004,//�û����ּ�¼

            AUSHOP_USERGPURCHASE_SUM_QUERY = 0x0005,//�û�G�ҹ����¼�ϼ�
            AUSHOP_USERGPURCHASE_SUM_QUERY_RESP = 0x8005,//�û�G�ҹ����¼�ϼ���Ӧ
            AUSHOP_USERMPURCHASE_SUM_QUERY = 0x0006,//�û�M�ҹ����¼�ϼ�
            AUSHOP_USERMPURCHASE_SUM_QUERY_RESP = 0x8006,//�û�M�ҹ����¼�ϼ���Ӧ

            AUSHOP_AVATARECOVER_DETAIL_QUERY = 0x0007,// �߻��նһ���ϸ��¼
            AUSHOP_AVATARECOVER_DETAIL_QUERY_RESP = 0x8007,// �߻��նһ���ϸ��¼


            DEPARTMENT_CREATE = 0x0009,//���Ŵ���
            DEPARTMENT_CREATE_RESP = 0x8009,//���Ŵ�����Ӧ
            DEPARTMENT_UPDATE = 0x0010,//�����޸�
            DEPARTMENT_UPDATE_RESP = 0x8010,//�����޸���Ӧ
            DEPARTMENT_DELETE = 0x0011,//����ɾ��
            DEPARTMENT_DELETE_RESP = 0x8011,//����ɾ����Ӧ
            DEPARTMENT_ADMIN = 0x0012,//���Ź���
            DEPARTMENT_ADMIN_RESP = 0x8012,//���Ź�����Ӧ

            /// <summary>
            /// �����Ź���(0x92)
            /// </summary>
            O2JAM_CHARACTERINFO_QUERY = 0x0001,//��ҽ�ɫ��Ϣ��ѯ
            O2JAM_CHARACTERINFO_QUERY_RESP = 0x8001,//��ҽ�ɫ��Ϣ��ѯ
            O2JAM_CHARACTERINFO_UPDATE = 0x0002,//��ҽ�ɫ��Ϣ����
            O2JAM_CHARACTERINFO_UPDATE_RESP = 0x8002,//��ҽ�ɫ��Ϣ����
            O2JAM_ITEM_QUERY = 0x0003,//��ҵ�����Ϣ��ѯ
            O2JAM_ITEM_QUERY_RESP = 0x8003,//��ҽ�ɫ��Ϣ��ѯ
            O2JAM_ITEM_UPDATE = 0x0004,//��ҵ�����Ϣ����
            O2JAM_ITEM_UPDATE_RESP = 0x8004,//��ҵ�����Ϣ����
            O2JAM_CONSUME_QUERY = 0x0005,//���������Ϣ��ѯ
            O2JAM_CONSUME_QUERY_RESP = 0x8005,//���������Ϣ��ѯ
            O2JAM_ITEMDATA_QUERY = 0x0006,// ���б��ѯ
            O2JAM_ITEMDATA_QUERY_RESP = 0x8006,// ���б���Ϣ��ѯ
            O2JAM_GIFTBOX_QUERY = 0x0007,//�������в�ѯ
            O2JAM_GIFTBOX_QUERY_RESP = 0x8007,//�������в�ѯ
            O2JAM_USERGCASH_UPDATE = 0x0008,//�������G��
            O2JAM_USERGCASH_UPDATE_RESP = 0x8008,//�������G�ҵ���Ӧ
            O2JAM_CONSUME_SUM_QUERY = 0x0009,//���������Ϣ��ѯ
            O2JAM_CONSUME_SUM_QUERY_RESP = 0x8009,//���������Ϣ��ѯ
            O2JAM_GIFTBOX_CREATE_QUERY = 0x0010,//����������� ��
            O2JAM_GIFTBOX_CREATE_QUERY_RESP = 0x8010,//����������� ��
            O2JAM_ITEMNAME_QUERY = 0x0011,
            O2JAM_ITEMNAME_QUERY_RESP = 0x8011,

            O2JAM_GIFTBOX_DELETE = 0x0012,
            O2JAM_GIFTBOX_DELETE_RESP = 0x8012,

            DEPARTMENT_RELATE_QUERY = 0x0013,//���Ź�����ѯ
            DEPARTMENT_RELATE_QUERY_RESP = 0x8013,//���Ź�����ѯ


            DEPART_RELATE_GAME_QUERY = 0x0014,
            DEPART_RELATE_GAME_QUERY_RESP = 0x8014,
            USER_SYSADMIN_QUERY = 0x0015,
            USER_SYSADMIN_QUERY_RESP = 0x8015,
            CARD_USERSECURE_CLEAR = 0x0009,//������Ұ�ȫ����Ϣ
            CARD_USERSECURE_CLEAR_RESP = 0x8009,//������Ұ�ȫ����Ϣ��Ӧ


            /// <summary>
            /// ������IIGM����(0x93)
            /// </summary>
            O2JAM2_ACCOUNT_QUERY = 0x0001,//����ʺ���Ϣ��ѯ
            O2JAM2_ACCOUNT_QUERY_RESP = 0x8001,//����ʺ���Ϣ��ѯ��Ӧ
            O2JAM2_ACCOUNTACTIVE_QUERY = 0x0002,//����ʺż�����Ϣ
            O2JAM2_ACCOUNTACTIVE_QUERY_RESP = 0x8002,//����ʺż�����Ӧ

            O2JAM2_CHARACTERINFO_QUERY = 0x0003,//�û���Ϣ��ѯ
            O2JAM2_CHARACTERINFO_QUERY_RESP = 0x8003,
            O2JAM2_CHARACTERINFO_UPDATE = 0x0004,//�û���Ϣ�޸�
            O2JAM2_CHARACTERINFO_UPDATE_RESP = 0x8004,
            O2JAM2_ITEMSHOP_QUERY = 0x0005,
            O2JAM2_ITEMSHOP_QUERY_RESP = 0x8005,
            O2JAM2_ITEMSHOP_DELETE = 0x0006,
            O2JAM2_ITEMSHOP_DELETE_RESP = 0x8006,
            O2JAM2_MESSAGE_QUERY = 0x0007,
            O2JAM2_MESSAGE_QUERY_RESP = 0x8007,
            O2JAM2_MESSAGE_CREATE = 0x0008,
            O2JAM2_MESSAGE_CREATE_RESP = 0x8008,
            O2JAM2_MESSAGE_DELETE = 0x0009,
            O2JAM2_MESSAGE_DELETE_RESP = 0x8009,
            O2JAM2_CONSUME_QUERY = 0x0010,
            O2JAM2_CONUMSE_QUERY_RESP = 0x8010,
            O2JAM2_CONSUME_SUM_QUERY = 0x0011,
            O2JAM2_CONUMSE_QUERY_SUM_RESP = 0x8011,
            O2JAM2_TRADE_QUERY = 0x0012,
            O2JAM2_TRADE_QUERY_RESP = 0x8012,
            O2JAM2_TRADE_SUM_QUERY = 0x0013,
            O2JAM2_TRADE_QUERY_SUM_RESP = 0x8013,
            O2JAM2_AVATORLIST_QUERY = 0x0014,
            O2JAM2_AVATORLIST_QUERY_RESP = 0x8014,

            O2JAM2_ACCOUNT_CLOSE = 0x0015,//��ͣ�ʻ���Ȩ����Ϣ
            O2JAM2_ACCOUNT_CLOSE_RESP = 0x8015,//��ͣ�ʻ���Ȩ����Ϣ��Ӧ

            O2JAM2_ACCOUNT_OPEN = 0x0016,//����ʻ���Ȩ����Ϣ
            O2JAM2_ACCOUNT_OPEN_RESP = 0x8016,//����ʻ���Ȩ����Ϣ��Ӧ
            O2JAM2_MEMBERBANISHMENT_QUERY = 0x0017,
            O2JAM2_MEMBERBANISHMENT_QUERY_RESP = 0x8017,
            O2JAM2_MEMBERSTOPSTATUS_QUERY = 0x0018,
            O2JAM2_MEMBERSTOPSTATUS_QUERY_RESP = 0x8018,
            O2JAM2_MEMBERLOCAL_BANISHMENT = 0x0019,
            O2JAM2_MEMBERLOCAL_BANISHMENT_RESP = 0x8019,
            O2JAM2_USERLOGIN_DELETE = 0x0020,
            O2JAM2_USERLOGIN_DELETE_RESP = 0x8020,




            SDO_CHALLENGE_QUERY = 0x0052,
            SDO_CHALLENGE_QUERY_RESP = 0x8052,
            SDO_CHALLENGE_INSERT = 0x0053,
            SDO_CHALLENGE_INSERT_RESP = 0x8053,
            SDO_CHALLENGE_UPDATE = 0x0054,
            SDO_CHALLENGE_UPDATE_RESP = 0x8054,
            SDO_CHALLENGE_DELETE = 0x0055,
            SDO_CHALLENGE_DELETE_RESP = 0x8055,
            SDO_MUSICDATA_QUERY = 0x0056,
            SDO_MUSICDATA_QUERY_RESP = 0x8056,

            SDO_MUSICDATA_OWN_QUERY = 0x0057,
            SDO_MUSICDATA_OWN_QUERY_RESP = 0x8057,


            SDO_CHALLENGE_SCENE_QUERY = 0x0058,
            SDO_CHALLENGE_SCENE_QUERY_RESP = 0x8058,
            SDO_CHALLENGE_SCENE_CREATE = 0x0059,
            SDO_CHALLENGE_SCENE_CREATE_RESP = 0x8059,
            SDO_CHALLENGE_SCENE_UPDATE = 0x0060,
            SDO_CHALLENGE_SCENE_UPDATE_RESP = 0x8060,
            SDO_CHALLENGE_SCENE_DELETE = 0x0061,
            SDO_CHALLENGE_SCENE_DELETE_RESP = 0x8061,


            SDO_MEDALITEM_CREATE = 0x0062,
            SDO_MEDALITEM_CREATE_RESP = 0x8062,
            SDO_MEDALITEM_UPDATE = 0x0063,
            SDO_MEDALITEM_UPDATE_RESP = 0x8063,
            SDO_MEDALITEM_DELETE = 0x0064,
            SDO_MEDALITEM_DELETE_RESP = 0x8064,
            SDO_MEDALITEM_QUERY = 0x0065,
            SDO_MEDALITEM_QUERY_RESP = 0x8065,

            SDO_MEDALITEM_OWNER_QUERY = 0x0066,
            SDO_MEDALITEM_OWNER_QUERY_RESP = 0x8066,


            SDO_MEMBERDANCE_OPEN = 0x0067,
            SDO_MEMBERDANCE_OPEN_RESP = 0x8067,
            SDO_MEMBERDANCE_CLOSE = 0x0068,
            SDO_MEMBERDANCE_CLOSE_RESP = 0x8068,

            SDO_YYHAPPYITEM_QUERY = 0x0084,
            SDO_YYHAPPYITEM_QUERY_RESP = 0x8084,
            SDO_YYHAPPYITEM_CREATE = 0x0085,
            SDO_YYHAPPYITEM_CREATE_RESP = 0x8085,
            SDO_YYHAPPYITEM_UPDATE = 0x0086,
            SDO_YYHAPPYITEM_UPDATE_RESP = 0x8086,
            SDO_YYHAPPYITEM_DELETE = 0x0087,
            SDO_YYHAPPYITEM_DELETE_RESP = 0x8087,
            SDO_PetInfo_Query = 0x0088,
            SDO_PetInfo_Query_RESP = 0x8088,
            /// <summary>
            /// ��������
            /// </summary>
            SOCCER_CHARACTERINFO_QUERY = 0x0001,//�û���Ϣ��ѯ
            SOCCER_CHARACTERINFO_QUERY_RESP = 0x8001,
            SOCCER_CHARCHECK_QUERY = 0x0002,//�û�NameCheck,SocketCheck
            SOCCER_CHARCHECK_QUERY_RESP = 0x8002,
            SOCCER_CHARITEMS_RECOVERY_QUERY = 0x0003,//�û�����
            SOCCER_CCHARITEMS_RECOVERY_QUERY_RESP = 0x8003,
            SOCCER_CHARPOINT_QUERY = 0x0004,//�û�G���޸�
            SOCCER_CHARPOINT_QUERY_RESP = 0x8004,
            SOCCER_DELETEDCHARACTERINFO_QUERY = 0x0005,//ɾ���û���ѯ
            SOCCER_DELETEDCHARACTERINFO_QUERY_RESP = 0x8005,

            SOCCER_CHARACTERSTATE_MODIFY = 0x0006,//ͣ���ɫ
            SOCCER_CHARACTERSTATE_MODIFY_RESP = 0x8006,
            SOCCER_ACCOUNTSTATE_MODIFY = 0x0007,//ͣ�����
            SOCCER_ACCOUNTSTATE_MODIFY_RESP = 0x8007,
            SOCCER_CHARACTERSTATE_QUERY = 0x0008,//ͣ���ɫ��ѯ
            SOCCER_CHARACTERSTATE_QUERY_RESP = 0x8008,
            SOCCER_ACCOUNTSTATE_QUERY = 0x0009,//ͣ����Ҳ�ѯ
            SOCCER_ACCOUNTSTATE_QUERY_RESP = 0x8009,

            CARD_USERNICK_QUERY = 0x0010,
            CARD_USERNICK_QUERY_RESP = 0x8010,

            AU_USERNICK_UPDATE = 0x0022,
            AU_USERNICK_UPDATE_RESP = 0x8022,

            LINK_SERVERIP_DELETE = 0x0010,
            LINK_SERVERIP_DELETE_RESP = 0x8010,

            #region �Ҵ�
            /// <summary>
            /// �Ҵ�(Ox70)
            /// </summary>
            SD_ActiveCode_Query = 0x0001,
            SD_ActiveCode_Query_Resp = 0x8001,
            SD_Account_Query = 0x0002,//�ʺŲ�ѯ
            SD_Account_Query_Resp = 0x8002,

            SD_UserLoginfo_Query = 0x0013,//�û���½��Ϣ��ѯ
            SD_UserLoginfo_Query_Resp = 0x8013,
            SD_UserMailinfo_Query = 0x0004,//�û��ʼ���Ϣ��ѯ
            SD_UserMailinfo_Query_Resp = 0x8004,
            SD_UserGuildinfo_Query = 0x0005,//�û�������Ϣ��ѯ
            SD_UserGuildinfo_Query_Resp = 0x8005,
            SD_UserStorageinfo_Query = 0x0006,//�û��ֿ���Ϣ��ѯ
            SD_UserStorageinfo_Query_Resp = 0x8006,
            SD_UserAdditem_Add = 0x0007,//��ӵ���
            SD_UserAdditem_Add_Resp = 0x8007,
            SD_UserAdditem_Del = 0x0011,//ɾ������
            SD_UserAdditem_Del_Resp = 0x8011,
            SD_BanUser_Query = 0x0008,//��ѯ��ͣ�û�
            SD_BanUser_Query_Resp = 0x8008,
            SD_BanUser_Ban = 0x0009,//��ͣ�û�
            SD_BanUser_Ban_Resp = 0x8009,
            SD_BanUser_UnBan = 0x0010,//����û�
            SD_BanUser_UnBan_Resp = 0x8010,
            SD_AccountDetail_Query = 0x0012,//�ʺ���ϸ��Ϣ��ѯ
            SD_AccountDetail_Query_Resp = 0x8012,

            SD_UserIteminfo_Query = 0x0003,//�û�������Ϣ��ѯ
            SD_UserIteminfo_Query_Resp = 0x8003,

            SD_Item_UserUnits_Query = 0x0014,	//��һ�����Ϣ
            SD_Item_UserUnits_Query_Resp = 0x8014,
            SD_Item_UserCombatitems_Query = 0x0015,	//���ս������
            SD_Item_UserCombatitems_Query_Resp = 0x8015,
            SD_Item_Operator_Query = 0x0016,	//��Ҹ��ٵ���
            SD_Item_Operator_Query_Resp = 0x8016,
            SD_Item_Paint_Query = 0x0017,	//���Ϳ�ϵ���
            SD_Item_Paint_Query_Resp = 0x8017,
            SD_Item_Skill_Query = 0x0018,//��Ҽ��ܵ���
            SD_Item_Skill_Query_Resp = 0x8018,//��Ҽ��ܵ���
            SD_Item_Sticker_Query = 0x0019,//��ұ�ǩ����
            SD_Item_Sticker_Query_Resp = 0x8019,//��ұ�ǩ����

            SD_Item_MixTree_Query = 0x0020,	//��һ������
            SD_Item_MixTree_Query_Resp = 0x8020,

            SD_UserGrift_Query = 0x0022,//������Ϣ��ѯ
            SD_UserGrift_Query_Resp = 0x8022,//������Ϣ��ѯ
            SD_KickUser_Query = 0x0021,//���û�����
            SD_KickUser_Query_Resp = 0x8021,//���û�����
            SD_SendNotes_Query = 0x0023,//���͹���
            SD_SendNotes_Query_Resp = 0x8023,//���͹���
            SD_SeacrhNotes_Query = 0x0024,//������Ϣ��ѯ
            SD_SeacrhNotes_Query_Resp = 0x8024,//������Ϣ��ѯ
            SD_ItemType_Query = 0x0025,//��ȡ��������
            SD_ItemType_Query_Resp = 0x8025,//��ȡ��������
            SD_ItemList_Query = 0x0026,//��ȡ�����б�
            SD_ItemList_Query_Resp = 0x8026,//��ȡ�����б�

            SD_TmpPassWord_Query = 0x0027,//��ʱ����
            SD_TmpPassWord_Query_Resp = 0x8027,//��ʱ����
            SD_ReTmpPassWord_Query = 0x0028,//�ָ���ʱ����
            SD_ReTmpPassWord_Query_Resp = 0x8028,//�ָ���ʱ����
            SD_SearchPassWord_Query = 0x0029,//��ѯ��ʱ����
            SD_SearchPassWord_Query_Resp = 0x8029,//��ѯ��ʱ����
            SD_UpdateExp_Query = 0x0030,//�޸ĵȼ�
            SD_UpdateExp_Query_Resp = 0x8030,//�޸ĵȼ�

            SD_Grift_FromUser_Query = 0x0031,//������������Ϣ��ѯ
            SD_Grift_FromUser_Query_Resp = 0x8031,//������������Ϣ��ѯ
            SD_Grift_ToUser_Query = 0x0032,//������������Ϣ��ѯ
            SD_Grift_ToUser_Query_Resp = 0x8032,//������������Ϣ��ѯ

            SD_TaskList_Update = 0x0033,//�޸Ĺ���
            SD_TaskList_Update_Resp = 0x8033,//�޸Ĺ���

            SD_Account_Active_Query = 0x0034,//ͨ���ʺŲ�ѯ������Ϣ
            SD_Account_Active_Query_Resp = 0x8034,//ͨ���ʺŲ�ѯ������Ϣ

            SD_BuyLog_Query = 0x0035,//��ҹ������
            SD_BuyLog_Query_Resp = 0x8035,//��ҹ������
            SD_Delete_ItemLog_Query = 0x0036,//���ɾ�����߼�¼
            SD_Delete_ItemLog_Query_Resp = 0x8036,//���ɾ�����߼�¼
            SD_LogInfo_Query = 0x0037,//�����־��¼
            SD_LogInfo_Query_Resp = 0x8037,//�����־��¼
            SD_Firend_Query = 0x0038,//��Һ��Ѳ�ѯ
            SD_Firend_Query_Resp = 0x8038,//��Һ��Ѳ�ѯ
            SD_UserRank_query = 0x0039,//���������ѯ
            SD_UserRank_query_Resp = 0x8039,//���������ѯ
            SD_UpdateUnitsExp_Query = 0x0040,//�޸���һ���ȼ�
            SD_UpdateUnitsExp_Query_Resp = 0x8040,//�޸���һ���ȼ�

            SD_UnitsItem_Query = 0x0041,//��ѯ�������
            SD_UnitsItem_Query_Resp = 0x8041,//��ѯ�������

            SD_GetGmAccount_Query = 0x0042,//��ѯGM�˺�
            SD_GetGmAccount_Query_Resp = 0x8042,//��ѯGM�˺�
            SD_UpdateGmAccount_Query = 0x0043,//�޸�GM�˺�
            SD_UpdateGmAccount_Query_Resp = 0x8043,//�޸�GM�˺�


            SD_UpdateMoney_Query = 0x0044,//�޸�G��
            SD_UpdateMoney_Query_Resp = 0x8044,//�޸�G��

            SD_Card_Info_Query = 0x0045,//����/��ʯ����ѯ
            SD_Card_Info_Query_Resp = 0x8045,//����/��ʯ����ѯ

            SD_UserAdditem_Add_All = 0x0046,//������ӵ���
            SD_UserAdditem_Add_All_Resp = 0x8046,//������ӵ���

            SD_ReGetUnits_Query = 0x0047,//�ָ�����
            SD_ReGetUnits_Query_Resp = 0x8047,//�ָ�����
            #endregion


            #region ������2
            JW2_ACCOUNT_QUERY = 0x0001,//����ʺ���Ϣ��ѯ��1.2.3.4.5.8��
            JW2_ACCOUNT_QUERY_RESP = 0x8001,//����ʺ���Ϣ��ѯ��Ӧ��1.2.3.4.5.8��
            /////////////��ͣ��/////////////////////////////////////////
            JW2_ACCOUNTREMOTE_QUERY = 0x0002,//��Ϸ��������ͣ������ʺŲ�ѯ
            JW2_ACCOUNTREMOTE_QUERY_RESP = 0x8002,//��Ϸ��������ͣ������ʺŲ�ѯ��Ӧ

            JW2_ACCOUNTLOCAL_QUERY = 0x0003,//���ط�ͣ������ʺŲ�ѯ
            JW2_ACCOUNTLOCAL_QUERY_RESP = 0x8003,//���ط�ͣ������ʺŲ�ѯ��Ӧ

            JW2_ACCOUNT_CLOSE = 0x0004,//��ͣ������ʺ�
            JW2_ACCOUNT_CLOSE_RESP = 0x8004,//��ͣ������ʺ���Ӧ
            JW2_ACCOUNT_OPEN = 0x0005,//��������ʺ�
            JW2_ACCOUNT_OPEN_RESP = 0x8005,//��������ʺ���Ӧ
            JW2_ACCOUNT_BANISHMENT_QUERY = 0x0006,//��ҷ�ͣ�ʺŲ�ѯ
            JW2_ACCOUNT_BANISHMENT_QUERY_RESP = 0x8006,//��ҷ�ͣ�ʺŲ�ѯ��Ӧ
            ////////////////////////////
            JW2_BANISHPLAYER = 0x0007,//����
            JW2_BANISHPLAYER_RESP = 0x8007,//������Ӧ
            JW2_BOARDMESSAGE = 0x0008,//����
            JW2_BOARDMESSAGE_RESP = 0x8008,//������Ӧ

            JW2_LOGINOUT_QUERY = 0x0009,//����������/�ǳ���Ϣ
            JW2_LOGINOUT_QUERY_RESP = 0x8009,//����������/�ǳ���Ϣ��Ӧ

            JW2_RPG_QUERY = 0x0010,//��ѯ������ڣ�6��
            JW2_RPG_QUERY_RESP = 0x8010,//��ѯ���������Ӧ��6��

            JW2_ITEMSHOP_BYOWNER_QUERY = 0x0011,////�鿴������ϵ�����Ϣ��7��7��
            JW2_ITEMSHOP_BYOWNER_QUERY_RESP = 0x8011,////�鿴������ϵ�����Ϣ��Ӧ��7��7��


            JW2_DUMMONEY_QUERY = 0x0012,////��ѯ����������ң�7��8��
            JW2_DUMMONEY_QUERY_RESP = 0x8012,////��ѯ�������������Ӧ��7��8��
            JW2_HOME_ITEM_QUERY = 0x0013,////�鿴������Ʒ�嵥�����ޣ�7��9��
            JW2_HOME_ITEM_QUERY_RESP = 0x8013,////�鿴������Ʒ�嵥��������Ӧ��7��9��
            JW2_SMALL_PRESENT_QUERY = 0x0014,////�鿴�������7��10��
            JW2_SMALL_PRESENT_QUERY_RESP = 0x8014,////�鿴���������Ӧ��7��10��
            JW2_WASTE_ITEM_QUERY = 0x0015,////�鿴�����Ե��ߣ�7��11��
            JW2_WASTE_ITEM_QUERY_RESP = 0x8015,////�鿴�����Ե�����Ӧ��7��11��
            JW2_SMALL_BUGLE_QUERY = 0x0016,////�鿴С���ȣ�7��12��
            JW2_SMALL_BUGLE_QUERY_RESP = 0x8016,////�鿴С������Ӧ��7��12��


            JW2_WEDDINGINFO_QUERY = 0x0017,//������Ϣ
            JW2_WEDDINGINFO_QUERY_RESP = 0x8017,
            JW2_WEDDINGLOG_QUERY = 0x0018,//������ʷ
            JW2_WEDDINGLOG_QUERY_RESP = 0x8018,
            JW2_WEDDINGGROUND_QUERY = 0x0019,//�����Ϣ
            JW2_WEDDINGGROUND_QUERY_RESP = 0x8019,
            JW2_COUPLEINFO_QUERY = 0x0020,//������Ϣ
            JW2_COUPLEINFO_QUERY_RESP = 0x8020,
            JW2_COUPLELOG_QUERY = 0x0021,//������ʷ
            JW2_COUPLELOG_QUERY_RESP = 0x8021,


            JW2_FAMILYINFO_QUERY = 0x0022,//������Ϣ
            JW2_FAMILYINFO_QUERY_RESP = 0x8022,
            JW2_FAMILYMEMBER_QUERY = 0x0023,//�����Ա��Ϣ
            JW2_FAMILYMEMBER_QUERY_RESP = 0x8023,
            JW2_SHOP_QUERY = 0x0024,//�̳���Ϣ
            JW2_SHOP_QUERY_RESP = 0x8024,
            JW2_User_Family_Query = 0x0025,//�û�������Ϣ
            JW2_User_Family_Query_Resp = 0x8025,

            JW2_FamilyItemInfo_Query = 0x0026,//���������Ϣ
            JW2_FamilyItemInfo_Query_Resp = 0x8026,

            JW2_FamilyBuyLog_Query = 0x0027,//���幺����־
            JW2_FamilyBuyLog_Query_Resp = 0x8027,

            JW2_FamilyTransfer_Query = 0x0028,//����ת����Ϣ
            JW2_FamilyTransfer_Query_Resp = 0x8028,

            JW2_FriendList_Query = 0x0029,//�����б�
            JW2_FriendList_Query_Resp = 0x8029,

            JW2_BasicInfo_Query = 0x0030,//������Ϣ��ѯ
            JW2_BasicInfo_Query_Resp = 0x8030,

            JW2_FamilyMember_Applying = 0x0031,//�����г�Ա
            JW2_FamilyMember_Applying_Resp = 0x8031,

            JW2_BasicRank_Query = 0x0032,//���صȼ�
            JW2_BasicBank_Query_Resp = 0x8032,


            ///////////����////////////////////////////
            JW2_BOARDTASK_INSERT = 0x0033,//�������
            JW2_BOARDTASK_INSERT_RESP = 0x8033,//���������Ӧ
            JW2_BOARDTASK_QUERY = 0x0034,//�����ѯ
            JW2_BOARDTASK_QUERY_RESP = 0x8034,//�����ѯ��Ӧ
            JW2_BOARDTASK_UPDATE = 0x0035,//�������
            JW2_BOARDTASK_UPDATE_RESP = 0x8035,//���������Ӧ

            JW2_ITEM_DEL = 0x0036,//����ɾ��������0����Ʒ��1�����2��
            JW2_ITEM_DEL_RESP = 0x8036,//����ɾ��������0����Ʒ��1�����2��

            JW2_MODIFYSEX_QUERY = 0x0037,//�޸Ľ�ɫ�Ա�
            JW2_MODIFYSEX_QUERY_RESP = 0x8037,

            JW2_MODIFYLEVEL_QUERY = 0x0038,//�޸ĵȼ�
            JW2_MODIFYLEVEL_QUERY_RESP = 0x8038,

            JW2_MODIFYEXP_QUERY = 0x0039,//�޸ľ���
            JW2_MODIFYEXP_QUERY_RESP = 0x8039,

            JW2_BAN_BIGBUGLE = 0x0040,//���ô�����
            JW2_BAN_BIGBUGLE_RESP = 0x8040,

            JW2_BAN_SMALLBUGLE = 0x0041,//����С����
            JW2_BAN_SMALLBUGLE_RESP = 0x8041,

            JW2_DEL_CHARACTER = 0x0042,//ɾ����ɫ
            JW2_DEL_CHARACTER_RESP = 0x8042,

            JW2_RECALL_CHARACTER = 0x0043,//�ָ���ɫ
            JW2_RECALL_CHARACTER_RESP = 0x8043,

            JW2_MODIFY_MONEY = 0x0044,//�޸Ľ�Ǯ
            JW2_MODIFY_MONEY_RESP = 0x8044,

            JW2_ADD_ITEM = 0x0045,//���ӵ���
            JW2_ADD_ITEM_RESP = 0x8045,

            JW2_CREATE_GM = 0x0046,//ÿ����������GM
            JW2_CREATE_GM_RESP = 0x8046,

            JW2_MODIFY_PWD = 0x0047,//�޸�����
            JW2_MODIFY_PWD_RESP = 0x8047,

            JW2_RECALL_PWD = 0x0048,//�ָ�����
            JW2_RECALL_PWD_RESP = 0x8048,


            JW2_ItemInfo_Query = 0x0049,//���߲�ѯ
            JW2_ItemInfo_Query_Resp = 0x8049,//


            JW2_ITEM_SELECT = 0x0050,//����ģ����ѯ
            JW2_ITEM_SELECT_RESP = 0x8050,//

            JW2_PetInfo_Query = 0x0051,//������Ϣ
            JW2_PetInfo_Query_Resp = 0x8051,

            JW2_Messenger_Query = 0x0052,//�ƺŲ�ѯ
            JW2_Messenger_Query_Resp = 0x8052,

            JW2_Wedding_Paper = 0x0053,//���֤��
            JW2_Wedding_Paper_Resp = 0x8053,

            JW2_CoupleParty_Card = 0x0054,//�����ɶԿ�
            JW2_CoupleParty_Card_Resp = 0x8054,


            JW2_MailInfo_Query = 0x0055,//ֽ����Ϣ
            JW2_MailInfo_Query_Resp = 0x8055,

            JW2_MoneyLog_Query = 0x0056,//��Ǯ��־��ѯ
            JW2_MoneyLog_Query_Resp = 0x8056,

            JW2_FamilyFund_Log = 0x0057,//������־
            JW2_FamilyFund_Log_Resp = 0x8057,

            JW2_FamilyItem_Log = 0x0058,//���������־
            JW2_FamilyItem_Log_Resp = 0x8058,

            JW2_Item_Log = 0x0059,//������־
            JW2_Item_Log_Resp = 0x8059,


            JW2_ADD_ITEM_ALL = 0x0060,//���ӵ���(����)
            JW2_ADD_ITEM_ALL_RESP = 0x8060,

            JW2_CashMoney_Log = 0x0061,//������־
            JW2_CashMoney_Log_Resp = 0x8061,

            JW2_SearchPassWord_Query = 0x0062,//��ѯ��ʱ����
            JW2_SearchPassWord_Query_Resp = 0x8062,//��ѯ��ʱ����

            JW2_CenterAvAtarItem_Bag_Query = 0x0063,//�м䱳������
            JW2_CenterAvAtarItem_Bag_Query_Resp = 0x8063,//�м䱳������

            JW2_CenterAvAtarItem_Equip_Query = 0x0064,//�м����ϵ���
            JW2_CenterAvAtarItem_Equip_Query_Resp = 0x8064,//�м����ϵ���

            JW2_House_Query = 0x0065,//С����
            JW2_House_Query_Resp = 0x8065,//С����

            JW2_GM_Update = 0x0066,//GM��B�޸�
            JW2_GM_Update_Resp = 0x8066,//GM��B�޸�
            JW2_JB_Query = 0x0067,//�e����Ϣ��?
            JW2_JB_Query_Resp = 0x8067,//�e����Ϣ��?


            JW2_UpDateFamilyName_Query = 0x0068,//�޸ļ�����
            JW2_UpDateFamilyName_Query_Resp = 0x8068,//�޸ļ�����

            JW2_UpdatePetName_Query = 0x0069,//�޸�?����
            JW2_UpdatePetName_Query_Resp = 0x8069,//�޸�?����


            JW2_Act_Card_Query = 0x0070,//�����ѯ
            JW2_Act_Card_Query_Resp = 0x8070,//�����ѯ

            JW2_Center_Kick_Query = 0x0071,//���g������
            JW2_Center_Kick_Query_Resp = 0x8071,//���g������

            JW2_ChangeServerExp_Query = 0x0072,//�޸ķ�����??����
            JW2_ChangeServerExp_Query_Resp = 0x8072,//�޸ķ�����??����

            JW2_ChangeServerMoney_Query = 0x0073,//�޸ķ�������Ǯ����
            JW2_ChangeServerMoney_Query_Resp = 0x8073,//�޸ķ�������Ǯ����

            JW2_MissionInfoLog_Query = 0x0074,//����LOG��ѯ
            JW2_MissionInfoLog_Query_Resp = 0x8074,//����LOG��ѯ

            JW2_AgainBuy_Query = 0x0075,//�ظ������˿�
            JW2_AgainBuy_Query_Resp = 0x8075,//�ظ������˿�

            JW2_GSSvererList_Query = 0x0076,//�������б�GS
            JW2_GSSvererList_Query_Resp = 0x8076,//�������б�GS


            JW2_Materiallist_Query = 0x0079,//�Ñ��ϳɲ��ϲ�ѯ
            JW2_Materiallist_Query_Resp = 0x8079,//�Ñ��ϳɲ��ϲ�ѯ

            JW2_MaterialHistory_Query = 0x0080,//�Ñ��ϳɼ�¼
            JW2_MaterialHistory_Query_Resp = 0x8080,//�Ñ��ϳɼ�¼

            JW2_ACTIVEPOINT_QUERY = 0x0081,//��Ծ�Ȳ�ѯ	
            JW2_ACTIVEPOINT_QUERY_Resp = 0x8081,//��Ծ�Ȳ�ѯ

            JW2_GETPIC_Query = 0x0082,//�����Ҫ��˵�ͼƬ�б�
            JW2_GETPIC_Query_Resp = 0x8082,//�����Ҫ��˵�ͼƬ�б�

            JW2_CHKPIC_Query = 0x0083,//���ͼƬ 2���ͨ����3��˲�ͨ�� 
            JW2_CHKPIC_Query_Resp = 0x8083,//���ͼƬ 2���ͨ����3��˲�ͨ��

            JW2_PicCard_Query = 0x0084,//�DƬ�ς���ʹ��
            JW2_PicCard_Query_Resp = 0x8084,//�DƬ�ς���ʹ��

            JW2_FamilyPet_Query = 0x0085,//��������ѯ
            JW2_FamilyPet_Query_Resp = 0x8085,//��������ѯ

            JW2_BuyPetAgg_Query = 0x0086,//�峤������ﵰ��ѯ
            JW2_BuyPetAgg_Query_Resp = 0x8086,//�峤������ﵰ��ѯ

            JW2_PetFirend_Query = 0x0087,//������ｻ�Ѳ�ѯ
            JW2_PetFirend_Query_Resp = 0x8087,//������ｻ�Ѳ�ѯ

            JW2_SmallPetAgg_Query = 0x0088,//�����Ա��ȡС������Ϣ��ѯ
            JW2_SmallPetAgg_Query_Resp = 0x8088,//�����Ա��ȡС������Ϣ��ѯ
            #endregion
            MAGIC_Account_Query = 0x0001,//��ɫ��������
            MAGIC_Account_Query_Resp = 0x8001,//��ɫ��������
            ERROR = 0xFFFF,

              #region ���߷ɳ�
            /// <summary>
            ///���߷ɳ�
            /// </summary>
            RayCity_Character_Query = 0x0001,
            RayCity_Character_Query_Resp = 0x8001,
            RayCity_CharacterState_Query = 0x0002,
            RayCity_CharacterState_Query_Resp = 0x8002,
            RayCity_RaceState_Query = 0x0003,
            RayCity_RaceState_Query_Resp = 0x8003,
            RayCity_InventoryList_Query = 0x0004,
            RayCity_InventoryList_Query_Resp = 0x8004,
            RayCity_InventoryDetail_Query = 0x0005,
            RayCity_InventoryDetail_Query_Resp = 0x8005,
            RayCity_CarList_Query = 0x0006,
            RayCity_CarList_Query_Resp = 0x8006,
            RayCity_Guild_Query = 0x0007,
            RayCity_Guild_Query_Resp = 0x8007,
            RayCity_QuestLog_Query = 0x0008,
            RayCity_QuestLog_Query_Resp = 0x8008,
            RayCity_MissionLog_Query = 0x0009,
            RayCity_MissionLog_Query_Resp = 0x8009,
            RayCity_DealLog_Query = 0x0010,
            RayCity_DealLog_Query_Resp = 0x8010,
            RayCity_FriendList_Query = 0x0011,
            RayCity_FriendList_Query_Resp = 0x8011,
            RayCity_BasicAccount_Query = 0x0012,
            RayCity_BasicAccount_Query_Resp = 0x8012,
            RayCity_GuildMember_Query = 0x0013,
            RayCity_GuildMember_Query_Resp = 0x8013,
            RayCity_BasicCharacter_Query = 0x0014,
            RayCity_BasicCharacter_Query_Resp = 0x8014,
            RayCity_BuyCar_Query = 0x0015,
            RayCity_BuyCar_Query_Resp = 0x8015,
            RayCity_ConnectionLog_Query = 0x0016,
            RayCity_ConnectionLog_Query_Resp = 0x8016,
            RayCity_CarInventory_Query = 0x0017,
            RayCity_CarInventory_Query_Resp = 0x8017,
            RayCity_ConnectionState_Query = 0x0018,
            RayCity_ConnectionState_Resp = 0x8018,
            RayCity_ItemShop_Insert = 0x0019,
            RayCity_ItemShop_Insert_Resp = 0x8019,
            RayCity_ItemShop_Query = 0x0020,
            RayCity_ItemShop_Query_Resp = 0x8020,
            RayCity_MoneyLog_Query = 0x0021,
            RayCity_MoneyLog_Query_Resp = 0x8021,
            RayCity_RaceLog_Query = 0x0022,
            RayCity_RaceLog_Query_Resp = 0x8022,
            RayCity_AddMoney = 0x0023,
            RayCity_AddMoney_Resp = 0x8023,
            RayCity_Skill_Query = 0x0024,
            RayCity_Skill_Query_Resp = 0x8024,
            RayCity_PlayerSkill_Query = 0x0025,
            RayCity_PlayerSkill_Query_Resp = 0x8025,
            RayCity_PlayerSkill_Delete = 0x0026,
            RayCity_PlayerSkill_Delete_Resp = 0x8026,
            RayCity_PlayerSkill_Insert = 0x0027,
            RayCity_PlayerSkill_Insert_Resp = 0x8027,
            RayCity_ItemType_Query = 0x0028,
            RayCity_ItemType_Query_Resp = 0x8028,
            RayCity_GMUser_Query = 0x0029,
            RayCity_GMUser_Query_Resp = 0x8029,
            RayCity_GMUser_Update = 0x0030,
            RayCity_GMUser_Update_Resp = 0x8030,
            RayCity_TradeInfo_Query = 0x0031,
            RayCity_TradeInfo_Query_Resp = 0x8031,
            RayCity_TradeDetail_Query = 0x0032,
            RayCity_TradeDetail_Query_Resp = 0x8032,
            RayCity_SetPos_Update = 0x0033,
            RayCity_SetPos_Update_Resp = 0x8033,
            RayCity_PlayerAccount_Create = 0x0034,
            RayCity_PlayerAccount_Create_Resp = 0x8034,
            RayCity_WareHousePwd_Update = 0x0035,
            RayCity_WareHousePwd_Update_Resp = 0x8035,
            RayCity_BingoCard_Query = 0x0036,
            RayCity_BingoCard_Query_Resp = 0x8036,
            RayCity_UserCharge_Query = 0x0037,
            RayCity_UserCharge_Query_Resp = 0x8037,
            RayCity_ItemConsume_Query = 0x0038,
            RayCity_ItemConsume_Query_Resp = 0x8038,
            RayCity_UserMails_Query = 0x0039,
            RayCity_UserMails_Query_Resp = 0x8039,
            RayCity_CashItemDetailLog_Query = 0x0040,
            RayCity_CashItemDetailLog_Query_Resp = 0x8040,
            RC_Character_Update = 0x0015,
            RC_Character_Update_Resp = 0x8015,
            #region ���쭳�
            RC_Character_Query = 0x0001,
            RC_Character_Query_Resp = 0x8001,
            RC_UserLoginOut_Query = 0x0002,
            RC_UserLoginOut_Query_Resp = 0x8002,
            RC_UserLogin_Del = 0x0003,
            RC_UserLogin_Del_Resp = 0x8003,
            RC_RcCode_Query = 0x0004,
            RC_RcCode_Query_Resp = 0x8004,
            RC_RcUser_Query = 0x0005,
            RC_RcUser_Query_Resp = 0x8005,
            #endregion
            RayCity_Coupon_Query = 0x0041,
            RayCity_Coupon_Query_Resp = 0x8041,
            RayCity_ActiveCard_Query = 0x0042,
            RayCity_ActiveCard_Query_Resp = 0x8042,

            RayCity_BoardList_Query = 0x0043,
            RayCity_BoardList_Query_Resp = 0x8043,
            RayCity_BoardList_Insert = 0x0044,
            RayCity_BoardList_Insert_Resp = 0x8044,
            RayCity_BoardList_Delete = 0x0045,
            RayCity_BoardList_Delete_Resp = 0x8045,
            #endregion

               CS_Accountbyid_Query = 0x0033,
            CS_Accountbyid_Query_Resp = 0x8033,
        }

        /// <summary>
        /// ��Ϣ���ͱ�ǩ
        /// </summary>
        public enum Msg_Category : byte
        {
            COMMON = 0x80,//������Ϣ��
            USER_ADMIN = 0x81,//GM�ʺŲ�����Ϣ��
            MODULE_ADMIN = 0x82,//ģ�������Ϣ��
            USER_MODULE_ADMIN = 0x83,//�û���ģ�������Ϣ��
            GAME_ADMIN = 0x84, //��Ϸģ�������Ϣ��
            NOTES_ADMIN = 0x85,//NOTESģ�������Ϣ��
            MJ_ADMIN = 0x86,//�ͽ���ϷGM���߲�����Ϣ��
            SDO_ADMIN = 0x87,//�������߲�����Ϣ��
            AU_ADMIN = 0x88,//������
            CR_ADMIN = 0x89,//��񿨶���������Ϣ��
            CARD_ADMIN = 0x90,
            AUSHOP_ADMIN = 0x91,
            O2JAM_ADMIN = 0x92,
            O2JAM2_ADMIN = 0x93,
            SOCCER_ADMIN = 0x94,//���������¼��Ϣ��

            JW2_ADMIN = 0x61,//������2


            RC_ADMIN = 0x96,
            RAYCITY_ADMIN = 0x97,//���߷ɳ���¼��Ϣ��
            SD_ADMIN = 0x70,//SD�ߴ��¼��Ϣ��
            ERROR = 0xFF
        }

        /// <summary>
        /// ��Ϣ״̬��ǩ
        /// </summary>
        public enum Body_Status : ushort
        {
            MSG_STRUCT_OK = 0,
            MSG_STRUCT_ERROR = 1,
            ILLEGAL_SOURCE_ADDR = 2,
            AUTHEN_ERROR = 3,
            OTHER_ERROR = 4
        }
        /// <summary>
        /// ��Ϣͷ��ǩ
        /// </summary>
        public enum Message_Tag_ID : uint
        {
            /// <summary>
            /// ����ģ��(0x80)
            /// </summary>
            CONNECT = 0x800001,//��������
            CONNECT_RESP = 0x808001,//������Ӧ
            DISCONNECT = 0x800002,//�Ͽ�����
            DISCONNECT_RESP = 0x808002,//�Ͽ���Ӧ
            ACCOUNT_AUTHOR = 0x800003,//�û������֤����
            ACCOUNT_AUTHOR_RESP = 0x808003,//�û������֤��Ӧ
            SERVERINFO_IP_QUERY = 0x800004,
            SERVERINFO_IP_QUERY_RESP = 0x808004,
            GMTOOLS_OperateLog_Query = 0x800005,//�鿴���߲�����¼
            GMTOOLS_OperateLog_Query_RESP = 0x808005,//�鿴���߲�����¼��Ӧ
            SERVERINFO_IP_ALL_QUERY = 0x800006,
            SERVERINFO_IP_ALL_QUERY_RESP = 0x808006,
            LINK_SERVERIP_CREATE = 0x800007,
            LINK_SERVERIP_CREATE_RESP = 0x808007,
            CLIENT_PATCH_COMPARE = 0x800008,
            CLIENT_PATCH_COMPARE_RESP = 0x808008,
            CLIENT_PATCH_UPDATE = 0x800009,
            CLIENT_PATCH_UPDATE_RESP = 0x808009,

            /// <summary>
            /// �û�����ģ��(0x81)
            /// </summary>
            USER_CREATE = 0x810001,//����GM�ʺ�����
            USER_CREATE_RESP = 0x818001,//����GM�ʺ���Ӧ
            USER_UPDATE = 0x810002,//����GM�ʺ���Ϣ����
            USER_UPDATE_RESP = 0x818002,//����GM�ʺ���Ϣ��Ӧ
            USER_DELETE = 0x810003,//ɾ��GM�ʺ���Ϣ����
            USER_DELETE_RESP = 0x818003,//ɾ��GM�ʺ���Ϣ��Ӧ
            USER_QUERY = 0x810004,//��ѯGM�ʺ���Ϣ����
            USER_QUERY_RESP = 0x818004,//��ѯGM�ʺ���Ϣ��Ӧ
            USER_PASSWD_MODIF = 0x810005,//�����޸�����
            USER_PASSWD_MODIF_RESP = 0x818005, //�����޸���Ϣ��Ӧ
            USER_QUERY_ALL = 0x810006,//��ѯ����GM�ʺ���Ϣ
            USER_QUERY_ALL_RESP = 0x818006,//��ѯ����GM�ʺ���Ϣ��Ӧ
            DEPART_QUERY = 0x810007, //��ѯ�����б�
            DEPART_QUERY_RESP = 0x818007,//��ѯ�����б���Ӧ
            UPDATE_ACTIVEUSER = 0x810008,//���������û�״̬
            UPDATE_ACTIVEUSER_RESP = 0x818008,//���������û�״̬��Ӧ
            /// <summary>
            /// ģ�����(0x82)
            /// </summary>
            MODULE_CREATE = 0x820001,//����ģ����Ϣ����
            MDDULE_CREATE_RESP = 0x828001,//����ģ����Ϣ��Ӧ
            MODULE_UPDATE = 0x820002,//����ģ����Ϣ����
            MODULE_UPDATE_RESP = 0x828002,//����ģ����Ϣ��Ӧ
            MODULE_DELETE = 0x820003,//ɾ��ģ������
            MODULE_DELETE_RESP = 0x828003,//ɾ��ģ����Ӧ
            MODULE_QUERY = 0x820004,//��ѯģ����Ϣ������
            MODULE_QUERY_RESP = 0x828004,//��ѯģ����Ϣ����Ӧ

            /// <summary>
            /// �û���ģ�����(0x83)
            /// </summary>
            USER_MODULE_CREATE = 0x830001,//�����û���ģ������
            USER_MODULE_CREATE_RESP = 0x838001,//�����û���ģ����Ӧ
            USER_MODULE_UPDATE = 0x830002,//�����û���ģ�������
            USER_MODULE_UPDATE_RESP = 0x838002,//�����û���ģ�����Ӧ
            USER_MODULE_DELETE = 0x830003,//ɾ���û���ģ������
            USER_MODULE_DELETE_RESP = 0x838003,//ɾ���û���ģ����Ӧ
            USER_MODULE_QUERY = 0x830004,//��ѯ�û�����Ӧģ������
            USER_MODULE_QUERY_RESP = 0x838004,//��ѯ�û�����Ӧģ����Ӧ

            /// <summary>
            /// ��Ϸ����(0x84)
            /// </summary>
            GAME_CREATE = 0x840001,//����GM�ʺ�����
            GAME_CREATE_RESP = 0x848001,//����GM�ʺ���Ӧ
            GAME_UPDATE = 0x840002,//����GM�ʺ���Ϣ����
            GAME_UPDATE_RESP = 0x848002,//����GM�ʺ���Ϣ��Ӧ
            GAME_DELETE = 0x840003,//ɾ��GM�ʺ���Ϣ����
            GAME_DELETE_RESP = 0x848003,//ɾ��GM�ʺ���Ϣ��Ӧ
            GAME_QUERY = 0x840004,//��ѯGM�ʺ���Ϣ����
            GAME_QUERY_RESP = 0x848004,//��ѯGM�ʺ���Ϣ��Ӧ
            GAME_MODULE_QUERY = 0x840005,//��ѯ��Ϸ��ģ���б�
            GAME_MODULE_QUERY_RESP = 0x848005,//��ѯ��Ϸ��ģ���б���Ӧ


            /// <summary>
            /// NOTES����(0x85)
            /// </summary>
            NOTES_LETTER_TRANSFER = 0x850001, //ȡ���ʼ��б�
            NOTES_LETTER_TRANSFER_RESP = 0x858001,//ȡ���ʼ��б����Ӧ
            NOTES_LETTER_PROCESS = 0x850002, //�ʼ�����
            NOTES_LETTER_PROCESS_RESP = 0x858002,//�ʼ�����
            NOTES_LETTER_TRANSMIT = 0x850003, //�ʼ�ת���б�
            NOTES_LETTER_TRANSMIT_RESP = 0x858003,//�ʼ�ת���б�

            /// <summary>
            /// �ͽ�GM����(0x86)
            /// </summary>
            MJ_CHARACTERINFO_QUERY = 0x860001,//������״̬
            MJ_CHARACTERINFO_QUERY_RESP = 0x868001,//������״̬��Ӧ
            MJ_CHARACTERINFO_UPDATE = 0x860002,//�޸����״̬
            MJ_CHARACTERINFO_UPDATE_RESP = 0x868002,//�޸����״̬��Ӧ
            MJ_LOGINTABLE_QUERY = 0x860003,//�������Ƿ�����
            MJ_LOGINTABLE_QUERY_RESP = 0x868003,//�������Ƿ�������Ӧ
            MJ_CHARACTERINFO_EXPLOIT_UPDATE = 0x860004,//�޸Ĺ�ѫֵ
            MJ_CHARACTERINFO_EXPLOIT_UPDATE_RESP = 0x868004,//�޸Ĺ�ѫֵ��Ӧ
            MJ_CHARACTERINFO_FRIEND_QUERY = 0x860005,//�г���������
            MJ_CHARACTERINFO_FRIEND_QUERY_RESP = 0x868005,//�г�����������Ӧ
            MJ_CHARACTERINFO_FRIEND_CREATE = 0x860006,//��Ӻ���
            MJ_CHARACTERINFO_FRIEND_CREATE_RESP = 0x868006,//��Ӻ�����Ӧ
            MJ_CHARACTERINFO_FRIEND_DELETE = 0x860007,//ɾ������
            MJ_CHARACTERINFO_FRIEND_DELETE_RESP = 0x868007,//ɾ��������Ӧ
            MJ_GUILDTABLE_UPDATE = 0x860008,//�޸ķ����������Ѵ��ڰ��
            MJ_GUILDTABLE_UPDATE_RESP = 0x868008,//�޸ķ����������Ѵ��ڰ����Ӧ
            MJ_ACCOUNT_LOCAL_CREATE = 0x860009,//���������ϵ�account����������Ϣ���浽���ط�������
            MJ_ACCOUNT_LOCAL_CREATE_RESP = 0x868009,//���������ϵ�account����������Ϣ���浽���ط���������Ӧ
            MJ_ACCOUNT_REMOTE_DELETE = 0x860010,//���÷�ͣ�ʺ�
            MJ_ACCOUNT_REMOTE_DELETE_RESP = 0x868010,//���÷�ͣ�ʺŵ���Ӧ
            MJ_ACCOUNT_REMOTE_RESTORE = 0x860011,//����ʺ�
            MJ_ACCOUNT_REMOTE_RESTORE_RESP = 0x868011,//����ʺ���Ӧ
            MJ_ACCOUNT_LIMIT_RESTORE = 0x860012,//��ʱ�޵ķ�ͣ
            MJ_ACCOUNT_LIMIT_RESTORE_RESP = 0x868012,//��ʱ�޵ķ�ͣ��Ӧ
            MJ_ACCOUNTPASSWD_LOCAL_CREATE = 0x860013,//����������뵽���� 
            MJ_ACCOUNTPASSWD_LOCAL_CREATE_RESP = 0x868013,//����������뵽����
            MJ_ACCOUNTPASSWD_REMOTE_UPDATE = 0x860014,//�޸�������� 
            MJ_ACCOUNTPASSWD_REMOTE_UPDATE_RESP = 0x868014,//�޸��������
            MJ_ACCOUNTPASSWD_REMOTE_RESTORE = 0x860015,//�ָ��������
            MJ_ACCOUNTPASSWD_REMOTE_RESTORE_RESP = 0x868015,//�ָ��������
            MJ_ITEMLOG_QUERY = 0x860016,//�����û����׼�¼
            MJ_ITEMLOG_QUERY_RESP = 0x868016,//�����û����׼�¼
            MJ_GMTOOLS_LOG_QUERY = 0x860017,//���ʹ���߲�����¼
            MJ_GMTOOLS_LOG_QUERY_RESP = 0x868017,//���ʹ���߲�����¼
            MJ_MONEYSORT_QUERY = 0x860018,//���ݽ�Ǯ����
            MJ_MONEYSORT_QUERY_RESP = 0x868018,//���ݽ�Ǯ�������Ӧ
            MJ_LEVELSORT_QUERY = 0x860019,//���ݵȼ�����
            MJ_LEVELSORT_QUERY_RESP = 0x868019,//���ݵȼ��������Ӧ
            MJ_MONEYFIGHTERSORT_QUERY = 0x860020,//���ݲ�ְͬҵ��Ǯ����
            MJ_MONEYFIGHTERSORT_QUERY_RESP = 0x868020,//���ݲ�ְͬҵ��Ǯ�������Ӧ
            MJ_LEVELFIGHTERSORT_QUERY = 0x860021,//���ݲ�ְͬҵ�ȼ�����
            MJ_LEVELFIGHTERSORT_QUERY_RESP = 0x868021,//���ݲ�ְͬҵ�ȼ��������Ӧ
            MJ_MONEYTAOISTSORT_QUERY = 0x860022,//���ݵ�ʿ��Ǯ����
            MJ_MONEYTAOISTSORT_QUERY_RESP = 0x868022,//���ݵ�ʿ��Ǯ�������Ӧ
            MJ_LEVELTAOISTSORT_QUERY = 0x860023,//���ݵ�ʿ�ȼ�����
            MJ_LEVELTAOISTSORT_QUERY_RESP = 0x868023,//���ݵ�ʿ�ȼ��������Ӧ
            MJ_MONEYRABBISORT_QUERY = 0x860024,//���ݷ�ʦ��Ǯ����
            MJ_MONEYRABBISORT_QUERY_RESP = 0x868024,//���ݷ�ʦ��Ǯ�������Ӧ
            MJ_LEVELRABBISORT_QUERY = 0x860025,//���ݷ�ʦ�ȼ�����
            MJ_LEVELRABBISORT_QUERY_RESP = 0x868025,//���ݷ�ʦ�ȼ��������Ӧ
            MJ_ACCOUNT_QUERY = 0x860026,//�ͽ��ʺŲ�ѯ
            MJ_ACCOUNT_QUERY_RESP = 0x868026,//�ͽ��ʺŲ�ѯ��Ӧ
            MJ_ACCOUNT_LOCAL_QUERY = 0x860027,//��ѯ�ͽ������ʺ�
            MJ_ACCOUNT_LOCAL_QUERY_RESP = 0x868027,//��ѯ�ͽ������ʺ���Ӧ
            SDO_ACCOUNT_QUERY = 0x870026,//�鿴��ҵ��ʺ���Ϣ
            SDO_ACCOUNT_QUERY_RESP = 0x878026,//�鿴��ҵ��ʺ���Ϣ��Ӧ
            SDO_CHARACTERINFO_QUERY = 0x870027,//�鿴�������ϵ���Ϣ
            SDO_CHARACTERINFO_QUERY_RESP = 0x878027,//�鿴�������ϵ���Ϣ��Ӧ
            SDO_ACCOUNT_CLOSE = 0x870028,//��ͣ�ʻ���Ȩ����Ϣ
            SDO_ACCOUNT_CLOSE_RESP = 0x878028,//��ͣ�ʻ���Ȩ����Ϣ��Ӧ
            SDO_ACCOUNT_OPEN = 0x870029,//����ʻ���Ȩ����Ϣ
            SDO_ACCOUNT_OPEN_RESP = 0x878029,//����ʻ���Ȩ����Ϣ��Ӧ
            SDO_PASSWORD_RECOVERY = 0x870030,//����һ�����
            SDO_PASSWORD_RECOVERY_RESP = 0x878030,//����һ�������Ӧ
            SDO_CONSUME_QUERY = 0x870031,//�鿴��ҵ����Ѽ�¼
            SDO_CONSUME_QUERY_RESP = 0x878031,//�鿴��ҵ����Ѽ�¼��Ӧ
            SDO_USERONLINE_QUERY = 0x870032,//�鿴���������״̬
            SDO_USERONLINE_QUERY_RESP = 0x878032,//�鿴���������״̬��Ӧ
            SDO_USERTRADE_QUERY = 0x870033,//�鿴��ҽ���״̬
            SDO_USERTRADE_QUERY_RESP = 0x878033,//�鿴��ҽ���״̬��Ӧ
            SDO_CHARACTERINFO_UPDATE = 0x870034,//�޸���ҵ��˺���Ϣ
            SDO_CHARACTERINFO_UPDATE_RESP = 0x878034,//�޸���ҵ��˺���Ϣ��Ӧ
            SDO_ITEMSHOP_QUERY = 0x870035,//�鿴��Ϸ�������е�����Ϣ
            SDO_ITEMSHOP_QUERY_RESP = 0x878035,//�鿴��Ϸ�������е�����Ϣ��Ӧ
            SDO_ITEMSHOP_DELETE = 0x870036,//ɾ����ҵ�����Ϣ
            SDO_ITEMSHOP_DELETE_RESP = 0x878036,//ɾ����ҵ�����Ϣ��Ӧ
            SDO_GIFTBOX_CREATE = 0x870037,//����������е�����Ϣ
            SDO_GIFTBOX_CREATE_RESP = 0x878037,//����������е�����Ϣ��Ӧ
            SDO_GIFTBOX_QUERY = 0x870038,//�鿴�������еĵ���
            SDO_GIFTBOX_QUERY_RESP = 0x878038,//�鿴�������еĵ�����Ӧ
            SDO_GIFTBOX_DELETE = 0x870039,//ɾ���������еĵ���
            SDO_GIFTBOX_DELETE_RESP = 0x878039,//ɾ���������еĵ�����Ӧ
            SDO_USERLOGIN_STATUS_QUERY = 0x870040,//�鿴��ҵ�¼״̬
            SDO_USERLOGIN_STATUS_QUERY_RESP = 0x878040,//�鿴��ҵ�¼״̬��Ӧ
            SDO_ITEMSHOP_BYOWNER_QUERY = 0x870041,////�鿴������ϵ�����Ϣ
            SDO_ITEMSHOP_BYOWNER_QUERY_RESP = 0x878041,////�鿴������ϵ�����Ϣ
            SDO_ITEMSHOP_TRADE_QUERY = 0x870042,//�鿴��ҽ��׼�¼��Ϣ
            SDO_ITEMSHOP_TRADE_QUERY_RESP = 0x878042,//�鿴��ҽ��׼�¼��Ϣ����Ӧ
            SDO_MEMBERSTOPSTATUS_QUERY = 0x870043,//�鿴���ʺ�״̬
            SDO_MEMBERSTOPSTATUS_QUERY_RESP = 0x878043,///�鿴���ʺ�״̬����Ӧ
            SDO_MEMBERBANISHMENT_QUERY = 0x870044,//�鿴����ͣ����ʺ�
            SDO_MEMBERBANISHMENT_QUERY_RESP = 0x878044,//�鿴����ͣ����ʺ���Ӧ
            SDO_USERMCASH_QUERY = 0x870045,//��ҳ�ֵ��¼��ѯ
            SDO_USERMCASH_QUERY_RESP = 0x878045,//��ҳ�ֵ��¼��ѯ��Ӧ
            SDO_USERGCASH_UPDATE = 0x870046,//�������G��
            SDO_USERGCASH_UPDATE_RESP = 0x878046,//�������G�ҵ���Ӧ
            SDO_MEMBERLOCAL_BANISHMENT = 0x870047,//���ر���ͣ����Ϣ
            SDO_MEMBERLOCAL_BANISHMENT_RESP = 0x878047,//���ر���ͣ����Ϣ��Ӧ
            SDO_EMAIL_QUERY = 0x870048,//�õ���ҵ�EMAIL
            SDO_EMAIL_QUERY_RESP = 0x878048,//�õ���ҵ�EMAIL��Ӧ
            SDO_USERCHARAGESUM_QUERY = 0x870049,//�õ���ֵ��¼�ܺ�
            SDO_USERCHARAGESUM_QUERY_RESP = 0x878049,//�õ���ֵ��¼�ܺ���Ӧ
            SDO_USERCONSUMESUM_QUERY = 0x870050,//�õ����Ѽ�¼�ܺ�
            SDO_USERCONSUMESUM_QUERY_RESP = 0x878050,//�õ����Ѽ�¼�ܺ���Ӧ
            SDO_USERGCASH_QUERY = 0x870051,//���?�Ҽ�¼��ѯ
            SDO_USERGCASH_QUERY_RESP = 0x878051,//���?�Ҽ�¼��ѯ��Ӧ

            SDO_USERNICK_UPDATE = 0x870069,
            SDO_USERNICK_UPDATE_RESP = 0x878069,

            SDO_PADKEYWORD_QUERY = 0x870070,
            SDO_PADKEYWORD_QUERY_RESP = 0x878070,

            SDO_BOARDMESSAGE_REQ = 0x870071,
            SDO_BOARDMESSAGE_REQ_RESP = 0x878071,

            SDO_CHANNELLIST_QUERY = 0x870072,
            SDO_CHANNELLIST_QUERY_RESP = 0x878072,
            SDO_ALIVE_REQ = 0x870073,
            SDO_ALIVE_REQ_RESP = 0x878073,

            SDO_BOARDTASK_QUERY = 0x870074,
            SDO_BOARDTASK_QUERY_RESP = 0x878074,

            SDO_BOARDTASK_UPDATE = 0x870075,
            SDO_BOARDTASK_UPDATE_RESP = 0x878075,

            SDO_BOARDTASK_INSERT = 0x870076,
            SDO_BOARDTASK_INSERT_RESP = 0x878076,

            SDO_DAYSLIMIT_QUERY = 0x870077,
            SDO_DAYSLIMIT_QUERY_RESP = 0x878077,
            SDO_FRIENDS_QUERY = 0x870078,//
            SDO_FRIENDS_QUERY_RESP = 0x878078,
            SDO_USERLOGIN_DEL = 0x870079,
            SDO_USERLOGIN_DEL_RESP = 0x878079,
            SDO_USERLOGIN_CLEAR = 0x870080,
            SDO_USERLOGIN_CLEAR_RESP = 0x878080,
            SDO_GATEWAY_QUERY = 0x870081,
            SDO_GATEWAY_QUERY_RESP = 0x878081,

            SDO_USERINTEGRAL_QUERY = 0x870082,
            SDO_USERINTEGRAL_QUERY_RESP = 0x878082,

            SDO_YYHAPPYITEM_QUERY = 0x870084,
            SDO_YYHAPPYITEM_QUERY_RESP = 0x878084,
            SDO_YYHAPPYITEM_CREATE = 0x870085,
            SDO_YYHAPPYITEM_CREATE_RESP = 0x878085,
            SDO_YYHAPPYITEM_UPDATE = 0x870086,
            SDO_YYHAPPYITEM_UPDATE_RESP = 0x878086,
            SDO_YYHAPPYITEM_DELETE = 0x870087,
            SDO_YYHAPPYITEM_DELETE_RESP = 0x878087,
            SDO_StageAward_Query = 0x870111,
            SDO_StageAward_Query_RESP = 0x878111,
            SDO_StageAward_Create = 0x870112,
            SDO_StageAward_Create_RESP = 0x878112,
            SDO_StageAward_Delete = 0x870113,
            SDO_StageAward_Delete_RESP = 0x878113,
            SDO_StageAward_Update = 0x870114,
            SDO_StageAward_Update_RESP = 0x878114,
            SDO_BAOXIANGRate_Query = 0x870120,
            SDO_BAOXIANGRate_Query_RESP = 0x878120,
            SDO_BAOXIANGRate_Update = 0x870121,
            SDO_BAOXIANGRate_Update_RESP = 0x878121,
            MAGIC_Account_Query = 0x710001,//��ɫ��������
            MAGIC_Account_Query_Resp = 0x718001,//��ɫ��������
            /// <summary>
            /// ������GM����(0x88)
            /// </summary>
            AU_ACCOUNT_QUERY = 0x880001,//����ʺ���Ϣ��ѯ
            AU_ACCOUNT_QUERY_RESP = 0x888001,//����ʺ���Ϣ��ѯ��Ӧ
            AU_ACCOUNTREMOTE_QUERY = 0x880002,//��Ϸ��������ͣ������ʺŲ�ѯ
            AU_ACCOUNTREMOTE_QUERY_RESP = 0x888002,//��Ϸ��������ͣ������ʺŲ�ѯ��Ӧ
            AU_ACCOUNTLOCAL_QUERY = 0x880003,//���ط�ͣ������ʺŲ�ѯ
            AU_ACCOUNTLOCAL_QUERY_RESP = 0x888003,//���ط�ͣ������ʺŲ�ѯ��Ӧ
            AU_ACCOUNT_CLOSE = 0x880004,//��ͣ������ʺ�
            AU_ACCOUNT_CLOSE_RESP = 0x888004,//��ͣ������ʺ���Ӧ
            AU_ACCOUNT_OPEN = 0x880005,//��������ʺ�
            AU_ACCOUNT_OPEN_RESP = 0x888005,//��������ʺ���Ӧ
            AU_ACCOUNT_BANISHMENT_QUERY = 0x880006,//��ҷ�ͣ�ʺŲ�ѯ
            AU_ACCOUNT_BANISHMENT_QUERY_RESP = 0x888006,//��ҷ�ͣ�ʺŲ�ѯ��Ӧ
            AU_CHARACTERINFO_QUERY = 0x880007,//��ѯ��ҵ��˺���Ϣ
            AU_CHARACTERINFO_QUERY_RESP = 0x888007,//��ѯ��ҵ��˺���Ϣ��Ӧ
            AU_CHARACTERINFO_UPDATE = 0x880008,//�޸���ҵ��˺���Ϣ
            AU_CHARACTERINFO_UPDATE_RESP = 0x888008,//�޸���ҵ��˺���Ϣ��Ӧ
            AU_ITEMSHOP_QUERY = 0x880009,//�鿴��Ϸ�������е�����Ϣ
            AU_ITEMSHOP_QUERY_RESP = 0x888009,//�鿴��Ϸ�������е�����Ϣ��Ӧ
            AU_ITEMSHOP_DELETE = 0x880010,//ɾ����ҵ�����Ϣ
            AU_ITEMSHOP_DELETE_RESP = 0x888010,//ɾ����ҵ�����Ϣ��Ӧ
            AU_ITEMSHOP_BYOWNER_QUERY = 0x880011,////�鿴������ϵ�����Ϣ
            AU_ITEMSHOP_BYOWNER_QUERY_RESP = 0x888011,////�鿴������ϵ�����Ϣ
            AU_ITEMSHOP_TRADE_QUERY = 0x880012,//�鿴��ҽ��׼�¼��Ϣ
            AU_ITEMSHOP_TRADE_QUERY_RESP = 0x888012,//�鿴��ҽ��׼�¼��Ϣ����Ӧ
            AU_ITEMSHOP_CREATE = 0x880013,//����������е�����Ϣ
            AU_ITEMSHOP_CREATE_RESP = 0x888013,//����������е�����Ϣ��Ӧ
            AU_LEVELEXP_QUERY = 0x880014,//�鿴��ҵȼ�����
            AU_LEVELEXP_QUERY_RESP = 0x888014,////�鿴��ҵȼ�������Ӧ
            AU_USERLOGIN_STATUS_QUERY = 0x880015,//�鿴��ҵ�¼״̬
            AU_USERLOGIN_STATUS_QUERY_RESP = 0x888015,//�鿴��ҵ�¼״̬��Ӧ
            AU_USERCHARAGESUM_QUERY = 0x880016,//�õ���ֵ��¼�ܺ�
            AU_USERCHARAGESUM_QUERY_RESP = 0x888016,//�õ���ֵ��¼�ܺ���Ӧ
            AU_CONSUME_QUERY = 0x880017,//�鿴��ҵ����Ѽ�¼
            AU_CONSUME_QUERY_RESP = 0x888017,//�鿴��ҵ����Ѽ�¼��Ӧ
            AU_USERCONSUMESUM_QUERY = 0x880018,//�õ����Ѽ�¼�ܺ�
            AU_USERCONSUMESUM_QUERY_RESP = 0x888018,//�õ����Ѽ�¼�ܺ���Ӧ
            AU_USERMCASH_QUERY = 0x880019,//��ҳ�ֵ��¼��ѯ
            AU_USERMCASH_QUERY_RESP = 0x888019,//��ҳ�ֵ��¼��ѯ��Ӧ
            AU_USERGCASH_QUERY = 0x880020,//���?�Ҽ�¼��ѯ
            AU_USERGCASH_QUERY_RESP = 0x888020,//���?�Ҽ�¼��ѯ��Ӧ
            AU_USERGCASH_UPDATE = 0x880021,//�������G��
            AU_USERGCASH_UPDATE_RESP = 0x888021,//�������G�ҵ���Ӧ


            Au_User_Msg_Query = 0x880062, //��ѯ�������Ϸ�е������¼ 
            Au_User_Msg_Query_Resp = 0x888062,//��ѯ�������Ϸ�е������¼    

            Au_BroaditeminfoNow_Query = 0x880059,//��ǰʱ���û�����������־
            Au_BroaditeminfoNow_Query_Resp = 0x888059,//��ǰʱ���û�����������־

            Au_BroaditeminfoBymsg_Query = 0x880060,//����ģ����ѯ�û�����������־
            Au_BroaditeminfoBymsg_Query_Resp = 0x888060,//����ģ����ѯ�û�����������־

            AU_MsgServerinfo_Query = 0x800018,
            AU_MsgServerinfo_Query_RESP = 0x808018,

            /// <summary>
            /// ��񿨶���GM����(0x89)
            /// </summary>
            CR_ACCOUNT_QUERY = 0x890001,//����ʺ���Ϣ��ѯ
            CR_ACCOUNT_QUERY_RESP = 0x898001,//����ʺ���Ϣ��ѯ��Ӧ
            CR_ACCOUNTACTIVE_QUERY = 0x890002,//����ʺż�����Ϣ
            CR_ACCOUNTACTIVE_QUERY_RESP = 0x898002,//����ʺż�����Ӧ
            CR_CALLBOARD_QUERY = 0x890003,//������Ϣ��ѯ
            CR_CALLBOARD_QUERY_RESP = 0x898003,//������Ϣ��ѯ��Ӧ
            CR_CALLBOARD_CREATE = 0x890004,//��������
            CR_CALLBOARD_CREATE_RESP = 0x898004,//����������Ӧ
            CR_CALLBOARD_UPDATE = 0x890005,//���¹�����Ϣ
            CR_CALLBOARD_UPDATE_RESP = 0x898005,//���¹�����Ϣ����Ӧ
            CR_CALLBOARD_DELETE = 0x890006,//ɾ��������Ϣ
            CR_CALLBOARD_DELETE_RESP = 0x898006,//ɾ��������Ϣ����Ӧ

            CR_CHARACTERINFO_QUERY = 0x890007,//��ҽ�ɫ��Ϣ��ѯ
            CR_CHARACTERINFO_QUERY_RESP = 0x898007,//��ҽ�ɫ��Ϣ��ѯ����Ӧ
            CR_CHARACTERINFO_UPDATE = 0x890008,//��ҽ�ɫ��Ϣ��ѯ
            CR_CHARACTERINFO_UPDATE_RESP = 0x898008,//��ҽ�ɫ��Ϣ��ѯ����Ӧ
            CR_CHANNEL_QUERY = 0x890009,//����Ƶ����ѯ
            CR_CHANNEL_QUERY_RESP = 0x898009,//����Ƶ����ѯ����Ӧ
            CR_NICKNAME_QUERY = 0x890010,//����ǳƲ�ѯ
            CR_NICKNAME_QUERY_RESP = 0x898010,//����ǳƵ���Ӧ
            CR_LOGIN_LOGOUT_QUERY = 0x890011,//������ߡ�����ʱ���ѯ
            CR_LOGIN_LOGOUT_QUERY_RESP = 0x898011,//������ߡ�����ʱ���ѯ����Ӧ
            CR_ERRORCHANNEL_QUERY = 0x890012,//������󹫸�Ƶ����ѯ
            CR_ERRORCHANNEL_QUERY_RESP = 0x898012,//������󹫸�Ƶ����ѯ����Ӧ


            /// <summary>
            /// ��ֵ����GM����(0x90)
            /// </summary>
            CARD_USERCHARGEDETAIL_QUERY = 0x900001,//һ��ͨ��ѯ
            CARD_USERCHARGEDETAIL_QUERY_RESP = 0x908001,//һ��ͨ��ѯ��Ӧ
            CARD_USERDETAIL_QUERY = 0x900002,//��֮���û�ע����Ϣ��ѯ
            CARD_USERDETAIL_QUERY_RESP = 0x908002,////��֮���û�ע����Ϣ��ѯ��Ӧ
            CARD_USERCONSUME_QUERY = 0x900003,//���б����Ѳ�ѯ
            CARD_USERCONSUME_QUERY_RESP = 0x908003,//���б����Ѳ�ѯ��Ӧ
            CARD_VNETCHARGE_QUERY = 0x900004,//�����ǿճ�ֵ��ѯ
            CARD_VNETCHARGE_QUERY_RESP = 0x908004,//�����ǿճ�ֵ��ѯ��Ӧ
            CARD_USERDETAIL_SUM_QUERY = 0x900005,//��ֵ�ϼƲ�ѯ
            CARD_USERDETAIL_SUM_QUERY_RESP = 0x908005,//��ֵ�ϼƲ�ѯ��Ӧ
            CARD_USERCONSUME_SUM_QUERY = 0x900006,//���ѺϼƲ�ѯ
            CARD_USERCONSUME_SUM_QUERY_RESP = 0x908006,//���Ѻϼ���Ӧ
            CARD_USERINFO_QUERY = 0x900007,//���ע����Ϣ��ѯ
            CARD_USERINFO_QUERY_RESP = 0x908007,//���ע����Ϣ��ѯ��Ӧ��˹�� 26��21:00 �С��� 3
            CARD_USERINFO_CLEAR = 0x900008,
            CARD_USERINFO_CLEAR_RESP = 0x908008,
            CARD_USERINITACTIVE_QUERY = 0x900015,//������Ϸ
            CARD_USERINITACTIVE_QUERY_RESP = 0x908015,
            /// <summary>
            /// �������̳�(0x91)
            /// </summary>
            AUSHOP_USERGPURCHASE_QUERY = 0x910001,//�û�G�ҹ����¼
            AUSHOP_USERGPURCHASE_QUERY_RESP = 0x918001,//�û�G�ҹ����¼
            AUSHOP_USERMPURCHASE_QUERY = 0x910002,//�û�M�ҹ����¼
            AUSHOP_USERMPURCHASE_QUERY_RESP = 0x918002,//�û�M�ҹ����¼
            AUSHOP_AVATARECOVER_QUERY = 0x910003,//���߻��նһ���
            AUSHOP_AVATARECOVER_QUERY_RESP = 0x918003,//���߻��նһ���
            AUSHOP_USERINTERGRAL_QUERY = 0x910004,//�û����ּ�¼
            AUSHOP_USERINTERGRAL_QUERY_RESP = 0x918004,//�û����ּ�¼

            AUSHOP_USERGPURCHASE_SUM_QUERY = 0x910005,//�û�G�ҹ����¼�ϼ�
            AUSHOP_USERGPURCHASE_SUM_QUERY_RESP = 0x918005,//�û�G�ҹ����¼�ϼ���Ӧ
            AUSHOP_USERMPURCHASE_SUM_QUERY = 0x910006,//�û�M�ҹ����¼�ϼ�
            AUSHOP_USERMPURCHASE_SUM_QUERY_RESP = 0x918006,//�û�M�ҹ����¼�ϼ���Ӧ

            AUSHOP_AVATARECOVER_DETAIL_QUERY = 0x910007,// �߻��նһ���ϸ��¼
            AUSHOP_AVATARECOVER_DETAIL_QUERY_RESP = 0x918007,// �߻��նһ���ϸ��¼


            DEPARTMENT_CREATE = 0x810009,//���Ŵ���
            DEPARTMENT_CREATE_RESP = 0x818009,//���Ŵ�����Ӧ
            DEPARTMENT_UPDATE = 0x810010,//�����޸�
            DEPARTMENT_UPDATE_RESP = 0x818010,//�����޸���Ӧ
            DEPARTMENT_DELETE = 0x810011,//����ɾ��
            DEPARTMENT_DELETE_RESP = 0x818011,//����ɾ����Ӧ
            DEPARTMENT_ADMIN = 0x810012,//���Ź���
            DEPARTMENT_ADMIN_RESP = 0x818012,//���Ź�����Ӧ

            DEPARTMENT_RELATE_QUERY = 0x810013,//���Ź�����ѯ
            DEPARTMENT_RELATE_QUERY_RESP = 0x818013,//���Ź�����ѯ

            /// <summary>
            /// �����Ź���(0x92)
            /// </summary>
            O2JAM_CHARACTERINFO_QUERY = 0x920001,//��ҽ�ɫ��Ϣ��ѯ
            O2JAM_CHARACTERINFO_QUERY_RESP = 0x928001,//��ҽ�ɫ��Ϣ��ѯ
            O2JAM_CHARACTERINFO_UPDATE = 0x920002,//��ҽ�ɫ��Ϣ����
            O2JAM_CHARACTERINFO_UPDATE_RESP = 0x928002,//��ҽ�ɫ��Ϣ����
            O2JAM_ITEM_QUERY = 0x920003,//��ҵ�����Ϣ��ѯ
            O2JAM_ITEM_QUERY_RESP = 0x928003,//��ҽ�ɫ��Ϣ��ѯ
            O2JAM_ITEM_UPDATE = 0x920004,//��ҵ�����Ϣ����
            O2JAM_ITEM_UPDATE_RESP = 0x928004,//��ҵ�����Ϣ����
            O2JAM_CONSUME_QUERY = 0x920005,//���������Ϣ��ѯ
            O2JAM_CONSUME_QUERY_RESP = 0x928005,//���������Ϣ��ѯ
            O2JAM_ITEMDATA_QUERY = 0x920006,// ���б��ѯ
            O2JAM_ITEMDATA_QUERY_RESP = 0x928006,// ���б���Ϣ��ѯ
            O2JAM_GIFTBOX_QUERY = 0x920007,//�������в�ѯ
            O2JAM_GIFTBOX_QUERY_RESP = 0x928007,//�������в�ѯ
            O2JAM_USERGCASH_UPDATE = 0x920008,//�������G��
            O2JAM_USERGCASH_UPDATE_RESP = 0x928008,//�������G�ҵ���Ӧ
            O2JAM_CONSUME_SUM_QUERY = 0x920009,//���������Ϣ��ѯ
            O2JAM_CONSUME_SUM_QUERY_RESP = 0x928009,//���������Ϣ��ѯ
            O2JAM_GIFTBOX_CREATE_QUERY = 0x920010,//�����������d��
            O2JAM_GIFTBOX_CREATE_QUERY_RESP = 0x928010,//�����������d��
            O2JAM_ITEMNAME_QUERY = 0x920011,
            O2JAM_ITEMNAME_QUERY_RESP = 0x928011,

            O2JAM_GIFTBOX_DELETE = 0x920012,
            O2JAM_GIFTBOX_DELETE_RESP = 0x928012,


            DEPART_RELATE_GAME_QUERY = 0x810014,
            DEPART_RELATE_GAME_QUERY_RESP = 0x818014,
            USER_SYSADMIN_QUERY = 0x810015,
            USER_SYSADMIN_QUERY_RESP = 0x818015,
            CARD_USERSECURE_CLEAR = 0x900009,//������Ұ�ȫ����Ϣ
            CARD_USERSECURE_CLEAR_RESP = 0x908009,//������Ұ�ȫ����Ϣ��Ӧ


            /// <summary>
            /// ������IIGM����(0x93)
            /// </summary>
            O2JAM2_ACCOUNT_QUERY = 0x930001,//����ʺ���Ϣ��ѯ
            O2JAM2_ACCOUNT_QUERY_RESP = 0x938001,//����ʺ���Ϣ��ѯ��Ӧ
            O2JAM2_ACCOUNTACTIVE_QUERY = 0x930002,//����ʺż�����Ϣ
            O2JAM2_ACCOUNTACTIVE_QUERY_RESP = 0x938002,//����ʺż�����Ӧ

            O2JAM2_CHARACTERINFO_QUERY = 0x930003,//�û���Ϣ��ѯ
            O2JAM2_CHARACTERINFO_QUERY_RESP = 0x938003,
            O2JAM2_CHARACTERINFO_UPDATE = 0x930004,//�û���Ϣ�޸�
            O2JAM2_CHARACTERINFO_UPDATE_RESP = 0x938004,
            O2JAM2_ITEMSHOP_QUERY = 0x930005,
            O2JAM2_ITEMSHOP_QUERY_RESP = 0x938005,
            O2JAM2_ITEMSHOP_DELETE = 0x930006,
            O2JAM2_ITEMSHOP_DELETE_RESP = 0x938006,
            O2JAM2_MESSAGE_QUERY = 0x930007,
            O2JAM2_MESSAGE_QUERY_RESP = 0x938007,
            O2JAM2_MESSAGE_CREATE = 0x930008,
            O2JAM2_MESSAGE_CREATE_RESP = 0x938008,
            O2JAM2_MESSAGE_DELETE = 0x930009,
            O2JAM2_MESSAGE_DELETE_RESP = 0x938009,
            O2JAM2_CONSUME_QUERY = 0x930010,
            O2JAM2_CONUMSE_QUERY_RESP = 0x938010,
            O2JAM2_CONSUME_SUM_QUERY = 0x930011,
            O2JAM2_CONUMSE_QUERY_SUM_RESP = 0x938011,
            O2JAM2_TRADE_QUERY = 0x930012,
            O2JAM2_TRADE_QUERY_RESP = 0x938012,
            O2JAM2_TRADE_SUM_QUERY = 0x930013,
            O2JAM2_TRADE_QUERY_SUM_RESP = 0x938013,
            O2JAM2_AVATORLIST_QUERY = 0x930014,
            O2JAM2_AVATORLIST_QUERY_RESP = 0x938014,

            O2JAM2_ACCOUNT_CLOSE = 0x930015,//��ͣ�ʻ���Ȩ����Ϣ
            O2JAM2_ACCOUNT_CLOSE_RESP = 0x938015,//��ͣ�ʻ���Ȩ����Ϣ��Ӧ
            O2JAM2_ACCOUNT_OPEN = 0x930016,//����ʻ���Ȩ����Ϣ
            O2JAM2_ACCOUNT_OPEN_RESP = 0x938016,//����ʻ���Ȩ����Ϣ��Ӧ
            O2JAM2_MEMBERBANISHMENT_QUERY = 0x930017,
            O2JAM2_MEMBERBANISHMENT_QUERY_RESP = 0x938017,
            O2JAM2_MEMBERSTOPSTATUS_QUERY = 0x930018,
            O2JAM2_MEMBERSTOPSTATUS_QUERY_RESP = 0x938018,
            O2JAM2_MEMBERLOCAL_BANISHMENT = 0x930019,
            O2JAM2_MEMBERLOCAL_BANISHMENT_RESP = 0x938019,

            O2JAM2_USERLOGIN_DELETE = 0x930020,
            O2JAM2_USERLOGIN_DELETE_RESP = 0x938020,



            SDO_CHALLENGE_QUERY = 0x870052,
            SDO_CHALLENGE_QUERY_RESP = 0x878052,
            SDO_CHALLENGE_INSERT = 0x870053,
            SDO_CHALLENGE_INSERT_RESP = 0x878053,
            SDO_CHALLENGE_UPDATE = 0x870054,
            SDO_CHALLENGE_UPDATE_RESP = 0x878054,
            SDO_CHALLENGE_DELETE = 0x870055,
            SDO_CHALLENGE_DELETE_RESP = 0x878055,
            SDO_MUSICDATA_QUERY = 0x870056,
            SDO_MUSICDATA_QUERY_RESP = 0x878056,


            SDO_MUSICDATA_OWN_QUERY = 0x870057,
            SDO_MUSICDATA_OWN_QUERY_RESP = 0x878057,


            SDO_CHALLENGE_SCENE_QUERY = 0x870058,
            SDO_CHALLENGE_SCENE_QUERY_RESP = 0x878058,
            SDO_CHALLENGE_SCENE_CREATE = 0x870059,
            SDO_CHALLENGE_SCENE_CREATE_RESP = 0x878059,
            SDO_CHALLENGE_SCENE_UPDATE = 0x870060,
            SDO_CHALLENGE_SCENE_UPDATE_RESP = 0x878060,
            SDO_CHALLENGE_SCENE_DELETE = 0x870061,
            SDO_CHALLENGE_SCENE_DELETE_RESP = 0x878061,


            SDO_MEDALITEM_CREATE = 0x870062,
            SDO_MEDALITEM_CREATE_RESP = 0x878062,
            SDO_MEDALITEM_UPDATE = 0x870063,
            SDO_MEDALITEM_UPDATE_RESP = 0x878063,
            SDO_MEDALITEM_DELETE = 0x870064,
            SDO_MEDALITEM_DELETE_RESP = 0x878064,
            SDO_MEDALITEM_QUERY = 0x870065,
            SDO_MEDALITEM_QUERY_RESP = 0x878065,


            SDO_MEDALITEM_OWNER_QUERY = 0x870066,
            SDO_MEDALITEM_OWNER_QUERY_RESP = 0x878066,

            SDO_MEMBERDANCE_OPEN = 0x870067,
            SDO_MEMBERDANCE_OPEN_RESP = 0x878067,
            SDO_MEMBERDANCE_CLOSE = 0x870068,
            SDO_MEMBERDANCE_CLOSE_RESP = 0x878068,
            SDO_PetInfo_Query = 0x870088,
            SDO_PetInfo_Query_RESP = 0x878088,


            #region �Ҵ�
            /// <summary>
            /// �Ҵ�(Ox70)
            /// </summary>
            SD_ActiveCode_Query = 0x700001,
            SD_ActiveCode_Query_Resp = 0x708001,
            SD_Account_Query = 0x700002,//�ʺŲ�ѯ
            SD_Account_Query_Resp = 0x708002,
            SD_UserIteminfo_Query = 0x700003,//�û�������Ϣ��ѯ
            SD_UserIteminfo_Query_Resp = 0x708003,

            SD_UserMailinfo_Query = 0x700004,//�û��ʼ���Ϣ��ѯ
            SD_UserMailinfo_Query_Resp = 0x708004,
            SD_UserGuildinfo_Query = 0x700005,//�û�������Ϣ��ѯ
            SD_UserGuildinfo_Query_Resp = 0x708005,
            SD_UserStorageinfo_Query = 0x700006,//�û��ֿ���Ϣ��ѯ
            SD_UserStorageinfo_Query_Resp = 0x708006,
            SD_UserAdditem_Add = 0x700007,//��ӵ���
            SD_UserAdditem_Add_Resp = 0x708007,
            SD_UserAdditem_Del = 0x700011,//ɾ������
            SD_UserAdditem_Del_Resp = 0x708011,
            SD_BanUser_Query = 0x700008,//��ѯ��ͣ�û�
            SD_BanUser_Query_Resp = 0x708008,
            SD_BanUser_Ban = 0x700009,//��ͣ�û�
            SD_BanUser_Ban_Resp = 0x708009,
            SD_BanUser_UnBan = 0x700010,//����û�
            SD_BanUser_UnBan_Resp = 0x708010,
            SD_AccountDetail_Query = 0x700012,//�ʺ���ϸ��Ϣ��ѯ
            SD_AccountDetail_Query_Resp = 0x708012,

            SD_UserLoginfo_Query = 0x700013,//�û���½��Ϣ��ѯ
            SD_UserLoginfo_Query_Resp = 0x708013,

            SD_Item_UserUnits_Query = 0x700014,	//��һ�����Ϣ
            SD_Item_UserUnits_Query_Resp = 0x708014,
            SD_Item_UserCombatitems_Query = 0x700015,	//���ս������
            SD_Item_UserCombatitems_Query_Resp = 0x708015,
            SD_Item_Operator_Query = 0x700016,	//��Ҹ��ٵ���
            SD_Item_Operator_Query_Resp = 0x708016,
            SD_Item_Paint_Query = 0x700017,	//���Ϳ�ϵ���
            SD_Item_Paint_Query_Resp = 0x708017,
            SD_Item_Skill_Query = 0x700018,//��Ҽ��ܵ���
            SD_Item_Skill_Query_Resp = 0x708018,//��Ҽ��ܵ���
            SD_Item_Sticker_Query = 0x700019,//��ұ�ǩ����
            SD_Item_Sticker_Query_Resp = 0x708019,//��ұ�ǩ����

            SD_Item_MixTree_Query = 0x700020,	//��һ������
            SD_Item_MixTree_Query_Resp = 0x708020,

            SD_KickUser_Query = 0x700021,//���û�����
            SD_KickUser_Query_Resp = 0x708021,//���û�����
            SD_UserGrift_Query = 0x700022,//������Ϣ��ѯ
            SD_UserGrift_Query_Resp = 0x708022,//������Ϣ��ѯ
            SD_SendNotes_Query = 0x700023,//���͹���
            SD_SendNotes_Query_Resp = 0x708023,//���͹���
            SD_SeacrhNotes_Query = 0x700024,//������Ϣ��ѯ
            SD_SeacrhNotes_Query_Resp = 0x708024,//������Ϣ��ѯ
            SD_ItemType_Query = 0x700025,//��ȡ��������
            SD_ItemType_Query_Resp = 0x708025,//��ȡ��������
            SD_ItemList_Query = 0x700026,//��ȡ�����б�
            SD_ItemList_Query_Resp = 0x708026,//��ȡ�����б�

            SD_TmpPassWord_Query = 0x700027,//��ʱ����
            SD_TmpPassWord_Query_Resp = 0x708027,//��ʱ����
            SD_ReTmpPassWord_Query = 0x700028,//�ָ���ʱ����
            SD_ReTmpPassWord_Query_Resp = 0x708028,//�ָ���ʱ����
            SD_SearchPassWord_Query = 0x700029,//��ѯ��ʱ����
            SD_SearchPassWord_Query_Resp = 0x708029,//��ѯ��ʱ����
            SD_UpdateExp_Query = 0x700030,//�޸ĵȼ�
            SD_UpdateExp_Query_Resp = 0x708030,//�޸ĵȼ�

            SD_Grift_FromUser_Query = 0x700031,//������������Ϣ��ѯ
            SD_Grift_FromUser_Query_Resp = 0x708031,//������������Ϣ��ѯ
            SD_Grift_ToUser_Query = 0x700032,//������������Ϣ��ѯ
            SD_Grift_ToUser_Query_Resp = 0x708032,//������������Ϣ��ѯ

            SD_TaskList_Update = 0x700033,//�޸Ĺ���
            SD_TaskList_Update_Resp = 0x708033,//�޸Ĺ���

            SD_Account_Active_Query = 0x700034,//ͨ���ʺŲ�ѯ������Ϣ
            SD_Account_Active_Query_Resp = 0x708034,//ͨ���ʺŲ�ѯ������Ϣ

            SD_BuyLog_Query = 0x700035,//��ҹ������
            SD_BuyLog_Query_Resp = 0x708035,//��ҹ������
            SD_Delete_ItemLog_Query = 0x700036,//���ɾ�����߼�¼
            SD_Delete_ItemLog_Query_Resp = 0x708036,//���ɾ�����߼�¼
            SD_LogInfo_Query = 0x700037,//�����־��¼
            SD_LogInfo_Query_Resp = 0x708037,//�����־��¼

            SD_Firend_Query = 0x700038,//��Һ��Ѳ�ѯ
            SD_Firend_Query_Resp = 0x708038,//��Һ��Ѳ�ѯ

            SD_UserRank_query = 0x700039,//���������ѯ
            SD_UserRank_query_Resp = 0x708039,//���������ѯ

            SD_UpdateUnitsExp_Query = 0x700040,//�޸���һ���ȼ�
            SD_UpdateUnitsExp_Query_Resp = 0x708040,//�޸���һ���ȼ�

            SD_UnitsItem_Query = 0x700041,//��ѯ�������
            SD_UnitsItem_Query_Resp = 0x708041,//��ѯ�������

            SD_GetGmAccount_Query = 0x700042,//��ѯGM�˺�
            SD_GetGmAccount_Query_Resp = 0x708042,//��ѯGM�˺�
            SD_UpdateGmAccount_Query = 0x700043,//�޸�GM�˺�
            SD_UpdateGmAccount_Query_Resp = 0x708043,//�޸�GM�˺�

            SD_UpdateMoney_Query = 0x700044,//�޸�G��
            SD_UpdateMoney_Query_Resp = 0x708044,//�޸�G��

            SD_Card_Info_Query = 0x700045,//����/��ʯ����ѯ
            SD_Card_Info_Query_Resp = 0x708045,//����/��ʯ����ѯ

            SD_UserAdditem_Add_All = 0x700046,//������ӵ���
            SD_UserAdditem_Add_All_Resp = 0x708046,//������ӵ���

            SD_ReGetUnits_Query = 0x700047,//�ָ�����
            SD_ReGetUnits_Query_Resp = 0x708047,//�ָ�����
            #endregion

            #region ������2

            JW2_ACCOUNT_QUERY = 0x610001,//����ʺ���Ϣ��ѯ��1.2.3.4.5.8��
            JW2_ACCOUNT_QUERY_RESP = 0x618001,//����ʺ���Ϣ��ѯ��Ӧ��1.2.3.4.5.8��
            /////////////��ͣ��/////////////////////////////////////////
            JW2_ACCOUNTREMOTE_QUERY = 0x610002,//��Ϸ��������ͣ������ʺŲ�ѯ
            JW2_ACCOUNTREMOTE_QUERY_RESP = 0x618002,//��Ϸ��������ͣ������ʺŲ�ѯ��Ӧ

            JW2_ACCOUNTLOCAL_QUERY = 0x610003,//���ط�ͣ������ʺŲ�ѯ
            JW2_ACCOUNTLOCAL_QUERY_RESP = 0x618003,//���ط�ͣ������ʺŲ�ѯ��Ӧ

            JW2_ACCOUNT_CLOSE = 0x610004,//��ͣ������ʺ�
            JW2_ACCOUNT_CLOSE_RESP = 0x618004,//��ͣ������ʺ���Ӧ
            JW2_ACCOUNT_OPEN = 0x610005,//��������ʺ�
            JW2_ACCOUNT_OPEN_RESP = 0x618005,//��������ʺ���Ӧ
            JW2_ACCOUNT_BANISHMENT_QUERY = 0x610006,//��ҷ�ͣ�ʺŲ�ѯ
            JW2_ACCOUNT_BANISHMENT_QUERY_RESP = 0x618006,//��ҷ�ͣ�ʺŲ�ѯ��Ӧ
            ////////////////////////////
            JW2_BANISHPLAYER = 0x610007,//����
            JW2_BANISHPLAYER_RESP = 0x618007,//������Ӧ

            JW2_BOARDMESSAGE = 0x610008,//����
            JW2_BOARDMESSAGE_RESP = 0x618008,//������Ӧ

            JW2_LOGINOUT_QUERY = 0x610009,//����������/�ǳ���Ϣ
            JW2_LOGINOUT_QUERY_RESP = 0x618009,//����������/�ǳ���Ϣ��Ӧ
            JW2_RPG_QUERY = 0x610010,//��ѯ������ڣ�6��
            JW2_RPG_QUERY_RESP = 0x618010,//��ѯ���������Ӧ��6��

            JW2_ITEMSHOP_BYOWNER_QUERY = 0x610011,////�鿴������ϵ�����Ϣ��7��7��
            JW2_ITEMSHOP_BYOWNER_QUERY_RESP = 0x618011,////�鿴������ϵ�����Ϣ��Ӧ��7��7��


            JW2_DUMMONEY_QUERY = 0x610012,////��ѯ����������ң�7��8��
            JW2_DUMMONEY_QUERY_RESP = 0x618012,////��ѯ�������������Ӧ��7��8��
            JW2_HOME_ITEM_QUERY = 0x610013,////�鿴������Ʒ�嵥�����ޣ�7��9��
            JW2_HOME_ITEM_QUERY_RESP = 0x618013,////�鿴������Ʒ�嵥��������Ӧ��7��9��
            JW2_SMALL_PRESENT_QUERY = 0x610014,////�鿴�������7��10��
            JW2_SMALL_PRESENT_QUERY_RESP = 0x618014,////�鿴���������Ӧ��7��10��
            JW2_WASTE_ITEM_QUERY = 0x610015,////�鿴�����Ե��ߣ�7��11��
            JW2_WASTE_ITEM_QUERY_RESP = 0x618015,////�鿴�����Ե�����Ӧ��7��11��
            JW2_SMALL_BUGLE_QUERY = 0x610016,////�鿴С���ȣ�7��12��
            JW2_SMALL_BUGLE_QUERY_RESP = 0x618016,////�鿴С������Ӧ��7��12��


            JW2_WEDDINGINFO_QUERY = 0x610017,//������Ϣ
            JW2_WEDDINGINFO_QUERY_RESP = 0x618017,
            JW2_WEDDINGLOG_QUERY = 0x610018,//������ʷ
            JW2_WEDDINGLOG_QUERY_RESP = 0x618018,
            JW2_WEDDINGGROUND_QUERY = 0x610019,//�����Ϣ
            JW2_WEDDINGGROUND_QUERY_RESP = 0x618019,
            JW2_COUPLEINFO_QUERY = 0x610020,//������Ϣ
            JW2_COUPLEINFO_QUERY_RESP = 0x618020,
            JW2_COUPLELOG_QUERY = 0x610021,//������ʷ
            JW2_COUPLELOG_QUERY_RESP = 0x618021,


            JW2_FAMILYINFO_QUERY = 0x610022,//������Ϣ
            JW2_FAMILYINFO_QUERY_RESP = 0x618022,
            JW2_FAMILYMEMBER_QUERY = 0x610023,//�����Ա��Ϣ
            JW2_FAMILYMEMBER_QUERY_RESP = 0x618023,
            JW2_SHOP_QUERY = 0x610024,//�̳���Ϣ
            JW2_SHOP_QUERY_RESP = 0x618024,
            JW2_User_Family_Query = 0x610025,//�û�������Ϣ
            JW2_User_Family_Query_Resp = 0x618025,

            JW2_FamilyItemInfo_Query = 0x610026,//���������Ϣ
            JW2_FamilyItemInfo_Query_Resp = 0x618026,

            JW2_FamilyBuyLog_Query = 0x610027,//���幺����־
            JW2_FamilyBuyLog_Query_Resp = 0x618027,

            JW2_FamilyTransfer_Query = 0x610028,//����ת����Ϣ
            JW2_FamilyTransfer_Query_Resp = 0x618028,

            JW2_FriendList_Query = 0x610029,//�����б�
            JW2_FriendList_Query_Resp = 0x618029,

            JW2_BasicInfo_Query = 0x610030,//������Ϣ��ѯ
            JW2_BasicInfo_Query_Resp = 0x618030,

            JW2_FamilyMember_Applying = 0x610031,//�����г�Ա
            JW2_FamilyMember_Applying_Resp = 0x618031,

            JW2_BasicRank_Query = 0x610032,//���صȼ�
            JW2_BasicBank_Query_Resp = 0x618032,


            ///////////����////////////////////////////
            JW2_BOARDTASK_INSERT = 0x610033,//�������
            JW2_BOARDTASK_INSERT_RESP = 0x618033,//���������Ӧ
            JW2_BOARDTASK_QUERY = 0x610034,//�����ѯ
            JW2_BOARDTASK_QUERY_RESP = 0x618034,//�����ѯ��Ӧ
            JW2_BOARDTASK_UPDATE = 0x610035,//�������
            JW2_BOARDTASK_UPDATE_RESP = 0x618035,//���������Ӧ

            JW2_ITEM_DEL = 0x610036,//����ɾ��������0����Ʒ��1�����2��
            JW2_ITEM_DEL_RESP = 0x618036,//����ɾ��������0����Ʒ��1�����2��

            JW2_MODIFYSEX_QUERY = 0x610037,//�޸Ľ�ɫ�Ա�
            JW2_MODIFYSEX_QUERY_RESP = 0x618037,

            JW2_MODIFYLEVEL_QUERY = 0x610038,//�޸ĵȼ�
            JW2_MODIFYLEVEL_QUERY_RESP = 0x618038,

            JW2_MODIFYEXP_QUERY = 0x610039,//�޸ľ���
            JW2_MODIFYEXP_QUERY_RESP = 0x618039,

            JW2_BAN_BIGBUGLE = 0x610040,//���ô�����
            JW2_BAN_BIGBUGLE_RESP = 0x618040,

            JW2_BAN_SMALLBUGLE = 0x610041,//����С����
            JW2_BAN_SMALLBUGLE_RESP = 0x618041,

            JW2_DEL_CHARACTER = 0x610042,//ɾ����ɫ
            JW2_DEL_CHARACTER_RESP = 0x618042,

            JW2_RECALL_CHARACTER = 0x610043,//�ָ���ɫ
            JW2_RECALL_CHARACTER_RESP = 0x618043,

            JW2_MODIFY_MONEY = 0x610044,//�޸Ľ�Ǯ
            JW2_MODIFY_MONEY_RESP = 0x618044,

            JW2_ADD_ITEM = 0x610045,//���ӵ���
            JW2_ADD_ITEM_RESP = 0x618045,

            JW2_CREATE_GM = 0x610046,//ÿ����������GM
            JW2_CREATE_GM_RESP = 0x618046,

            JW2_MODIFY_PWD = 0x610047,//�޸�����
            JW2_MODIFY_PWD_RESP = 0x618047,

            JW2_RECALL_PWD = 0x610048,//�ָ�����
            JW2_RECALL_PWD_RESP = 0x618048,


            JW2_ItemInfo_Query = 0x610049,//���߲�ѯ
            JW2_ItemInfo_Query_Resp = 0x618049,//


            JW2_ITEM_SELECT = 0x610050,//����ģ����ѯ
            JW2_ITEM_SELECT_RESP = 0x618050,//

            JW2_PetInfo_Query = 0x610051,//������Ϣ
            JW2_PetInfo_Query_Resp = 0x618051,

            JW2_Messenger_Query = 0x610052,//�ƺŲ�ѯ
            JW2_Messenger_Query_Resp = 0x618052,

            JW2_Wedding_Paper = 0x610053,//���֤��
            JW2_Wedding_Paper_Resp = 0x618053,

            JW2_CoupleParty_Card = 0x610054,//�����ɶԿ�
            JW2_CoupleParty_Card_Resp = 0x618054,


            JW2_MailInfo_Query = 0x610055,//ֽ����Ϣ
            JW2_MailInfo_Query_Resp = 0x618055,

            JW2_MoneyLog_Query = 0x610056,//��Ǯ��־��ѯ
            JW2_MoneyLog_Query_Resp = 0x618056,

            JW2_FamilyFund_Log = 0x610057,//������־
            JW2_FamilyFund_Log_Resp = 0x618057,

            JW2_FamilyItem_Log = 0x610058,//���������־
            JW2_FamilyItem_Log_Resp = 0x618058,

            JW2_Item_Log = 0x610059,//������־
            JW2_Item_Log_Resp = 0x618059,

            JW2_ADD_ITEM_ALL = 0x610060,//���ӵ���(����)
            JW2_ADD_ITEM_ALL_RESP = 0x618060,

            JW2_CashMoney_Log = 0x610061,//������־
            JW2_CashMoney_Log_Resp = 0x618061,

            JW2_SearchPassWord_Query = 0x610062,//��ѯ��ʱ����
            JW2_SearchPassWord_Query_Resp = 0x618062,//��ѯ��ʱ����

            JW2_CenterAvAtarItem_Bag_Query = 0x610063,//�м䱳������
            JW2_CenterAvAtarItem_Bag_Query_Resp = 0x618063,//�м䱳������

            JW2_CenterAvAtarItem_Equip_Query = 0x610064,//�м����ϵ���
            JW2_CenterAvAtarItem_Equip_Query_Resp = 0x618064,//�м����ϵ���


            JW2_House_Query = 0x610065,//С����
            JW2_House_Query_Resp = 0x618065,//С����

            JW2_GM_Update = 0x610066,//GM��B�޸�
            JW2_GM_Update_Resp = 0x618066,//GM��B�޸�
            JW2_JB_Query = 0x610067,//�e����Ϣ��?
            JW2_JB_Query_Resp = 0x618067,//�e����Ϣ��?

            JW2_UpDateFamilyName_Query = 0x610068,//�޸ļ�����
            JW2_UpDateFamilyName_Query_Resp = 0x618068,//�޸ļ�����

            JW2_UpdatePetName_Query = 0x610069,//�޸�?����
            JW2_UpdatePetName_Query_Resp = 0x618069,//�޸�?����



            JW2_Act_Card_Query = 0x610070,//�����ѯ
            JW2_Act_Card_Query_Resp = 0x618070,//�����ѯ


            JW2_Center_Kick_Query = 0x610071,//���g������
            JW2_Center_Kick_Query_Resp = 0x618071,//���g������

            JW2_ChangeServerExp_Query = 0x610072,//�޸ķ�����??����
            JW2_ChangeServerExp_Query_Resp = 0x618072,//�޸ķ�����??����

            JW2_ChangeServerMoney_Query = 0x610073,//�޸ķ�������Ǯ����
            JW2_ChangeServerMoney_Query_Resp = 0x618073,//�޸ķ�������Ǯ����

            JW2_MissionInfoLog_Query = 0x610074,//����LOG��ѯ
            JW2_MissionInfoLog_Query_Resp = 0x618074,//����LOG��ѯ

            JW2_AgainBuy_Query = 0x610075,//�ظ������˿�
            JW2_AgainBuy_Query_Resp = 0x618075,//�ظ������˿�

            JW2_GSSvererList_Query = 0x610076,//�������б�GS
            JW2_GSSvererList_Query_Resp = 0x618076,//�������б�GS


            JW2_Materiallist_Query = 0x610079,//�Ñ��ϳɲ��ϲ�ѯ
            JW2_Materiallist_Query_Resp = 0x618079,//�Ñ��ϳɲ��ϲ�ѯ

            JW2_MaterialHistory_Query = 0x610080,//�Ñ��ϳɼ�¼
            JW2_MaterialHistory_Query_Resp = 0x618080,//�Ñ��ϳɼ�¼

            JW2_ACTIVEPOINT_QUERY = 0x610081,//��Ծ�Ȳ�ѯ	
            JW2_ACTIVEPOINT_QUERY_Resp = 0x618081,//��Ծ�Ȳ�ѯ

            JW2_GETPIC_Query = 0x610082,//�����Ҫ��˵�ͼƬ�б�
            JW2_GETPIC_Query_Resp = 0x618082,//�����Ҫ��˵�ͼƬ�б�

            JW2_CHKPIC_Query = 0x610083,//���ͼƬ 2���ͨ����3��˲�ͨ�� 
            JW2_CHKPIC_Query_Resp = 0x618083,//���ͼƬ 2���ͨ����3��˲�ͨ��

            JW2_PicCard_Query = 0x610084,//�DƬ�ς���ʹ��
            JW2_PicCard_Query_Resp = 0x618084,//�DƬ�ς���ʹ��

            JW2_FamilyPet_Query = 0x610085,//��������ѯ
            JW2_FamilyPet_Query_Resp = 0x618085,//��������ѯ

            JW2_BuyPetAgg_Query = 0x610086,//�峤������ﵰ��ѯ
            JW2_BuyPetAgg_Query_Resp = 0x618086,//�峤������ﵰ��ѯ

            JW2_PetFirend_Query = 0x610087,//������ｻ�Ѳ�ѯ
            JW2_PetFirend_Query_Resp = 0x618087,//������ｻ�Ѳ�ѯ

            JW2_SmallPetAgg_Query = 0x610088,//�����Ա��ȡС������Ϣ��ѯ
            JW2_SmallPetAgg_Query_Resp = 0x618088,//�����Ա��ȡС������Ϣ��ѯ
            #endregion

            /// <summary>
            /// �������� (0x94)Add by KeHuaQing 2006-09-14
            /// </summary>
            SOCCER_CHARACTERINFO_QUERY = 0x940001,//�û���Ϣ��ѯ
            SOCCER_CHARACTERINFO_QUERY_RESP = 0x948001,
            SOCCER_CHARCHECK_QUERY = 0x940002,//�û�NameCheck,SocketCheck
            SOCCER_CHARCHECK_QUERY_RESP = 0x948002,
            SOCCER_CHARITEMS_RECOVERY_QUERY = 0x940003,//�û�����
            SOCCER_CCHARITEMS_RECOVERY_QUERY_RESP = 0x948003,
            SOCCER_CHARPOINT_QUERY = 0x940004,//�û�G���޸�
            SOCCER_CHARPOINT_QUERY_RESP = 0x948004,
            SOCCER_DELETEDCHARACTERINFO_QUERY = 0x940005,//ɾ���û���ѯ
            SOCCER_DELETEDCHARACTERINFO_QUERY_RESP = 0x948005,

            SOCCER_CHARACTERSTATE_MODIFY = 0x940006,//ͣ���ɫ
            SOCCER_CHARACTERSTATE_MODIFY_RESP = 0x948006,
            SOCCER_ACCOUNTSTATE_MODIFY = 0x940007,//ͣ�����
            SOCCER_ACCOUNTSTATE_MODIFY_RESP = 0x948007,
            SOCCER_CHARACTERSTATE_QUERY = 0x940008,//ͣ���ɫ��ѯ
            SOCCER_CHARACTERSTATE_QUERY_RESP = 0x948008,
            SOCCER_ACCOUNTSTATE_QUERY = 0x940009,//ͣ����Ҳ�ѯ
            SOCCER_ACCOUNTSTATE_QUERY_RESP = 0x948009,

            CARD_USERNICK_QUERY = 0x900010,
            CARD_USERNICK_QUERY_RESP = 0x908010,

            AU_USERNICK_UPDATE = 0x880022,
            AU_USERNICK_UPDATE_RESP = 0x888022,


            LINK_SERVERIP_DELETE = 0x800010,
            LINK_SERVERIP_DELETE_RESP = 0x808010,


            NOTDEFINED = 0x0,
            ERROR = 0xFFFFFF,

            #region ���߷ɳ�
            /// <summary>
            /// ���߷ɳ�
            /// </summary>
            RC_Character_Query = 0x960001,
            RC_Character_Query_Resp = 0x968001,
            RC_UserLoginOut_Query = 0x960002,
            RC_UserLoginOut_Query_Resp = 0x968002,
            RC_UserLogin_Del = 0x960003,
            RC_UserLogin_Del_Resp = 0x968003,
            RC_RcCode_Query = 0x960004,
            RC_RcCode_Query_Resp = 0x968004,
            RC_RcUser_Query = 0x960005,
            RC_RcUser_Query_Resp = 0x968005,
            RC_Character_Update = 0x960015,
            RC_Character_Update_Resp = 0x968015,

            RayCity_Character_Query = 0x970001,
            RayCity_Character_Query_Resp = 0x978001,
            RayCity_CharacterState_Query = 0x970002,
            RayCity_CharacterState_Query_Resp = 0x978002,
            RayCity_RaceState_Query = 0x970003,
            RayCity_RaceState_Query_Resp = 0x978003,
            RayCity_InventoryList_Query = 0x970004,
            RayCity_InventoryList_Query_Resp = 0x978004,
            RayCity_InventoryDetail_Query = 0x970005,
            RayCity_InventoryDetail_Query_Resp = 0x978005,
            RayCity_CarList_Query = 0x970006,
            RayCity_CarList_Query_Resp = 0x978006,
            RayCity_Guild_Query = 0x970007,
            RayCity_Guild_Query_Resp = 0x978007,
            RayCity_QuestLog_Query = 0x970008,
            RayCity_QuestLog_Query_Resp = 0x978008,
            RayCity_MissionLog_Query = 0x970009,
            RayCity_MissionLog_Query_Resp = 0x978009,
            RayCity_DealLog_Query = 0x970010,
            RayCity_DealLog_Query_Resp = 0x978010,
            RayCity_FriendList_Query = 0x970011,
            RayCity_FriendList_Query_Resp = 0x978011,
            RayCity_BasicAccount_Query = 0x970012,
            RayCity_BasicAccount_Query_Resp = 0x978012,
            RayCity_GuildMember_Query = 0x970013,
            RayCity_GuildMember_Query_Resp = 0x978013,
            RayCity_BasicCharacter_Query = 0x970014,
            RayCity_BasicCharacter_Query_Resp = 0x978014,
            RayCity_BuyCar_Query = 0x970015,
            RayCity_BuyCar_Query_Resp = 0x978015,
            RayCity_ConnectionLog_Query = 0x970016,
            RayCity_ConnectionLog_Query_Resp = 0x978016,
            RayCity_CarInventory_Query = 0x970017,
            RayCity_CarInventory_Query_Resp = 0x978017,
            RayCity_ConnectionState_Query = 0x970018,
            RayCity_ConnectionState_Resp = 0x978018,
            RayCity_ItemShop_Insert = 0x970019,
            RayCity_ItemShop_Insert_Resp = 0x978019,
            RayCity_ItemShop_Query = 0x970020,
            RayCity_ItemShop_Query_Resp = 0x978020,
            RayCity_MoneyLog_Query = 0x970021,
            RayCity_MoneyLog_Query_Resp = 0x978021,
            RayCity_RaceLog_Query = 0x970022,
            RayCity_RaceLog_Query_Resp = 0x978022,
            RayCity_AddMoney = 0x970023,
            RayCity_AddMoney_Resp = 0x978023,
            RayCity_Skill_Query = 0x970024,
            RayCity_Skill_Query_Resp = 0x978024,
            RayCity_PlayerSkill_Query = 0x970025,
            RayCity_PlayerSkill_Query_Resp = 0x978025,
            RayCity_PlayerSkill_Delete = 0x970026,
            RayCity_PlayerSkill_Delete_Resp = 0x978026,
            RayCity_PlayerSkill_Insert = 0x970027,
            RayCity_PlayerSkill_Insert_Resp = 0x978027,
            RayCity_ItemType_Query = 0x970028,
            RayCity_ItemType_Query_Resp = 0x978028,
            RayCity_TradeInfo_Query = 0x970031,
            RayCity_TradeInfo_Query_Resp = 0x978031,
            RayCity_TradeDetail_Query = 0x970032,
            RayCity_TradeDetail_Query_Resp = 0x978032,
            RayCity_SetPos_Update = 0x970033,
            RayCity_SetPos_Update_Resp = 0x978033,
            RayCity_BingoCard_Query = 0x970036,
            RayCity_BingoCard_Query_Resp = 0x978036,
            RayCity_GMUser_Query = 0x970029,
            RayCity_GMUser_Query_Resp = 0x978029,
            RayCity_GMUser_Update = 0x970030,
            RayCity_GMUser_Update_Resp = 0x978030,
            RayCity_PlayerAccount_Create = 0x970034,
            RayCity_PlayerAccount_Create_Resp = 0x978034,
            RayCity_WareHousePwd_Update = 0x970035,
            RayCity_WareHousePwd_Update_Resp = 0x978035,
            RayCity_UserCharge_Query = 0x970037,
            RayCity_UserCharge_Query_Resp = 0x978037,
            RayCity_ItemConsume_Query = 0x970038,
            RayCity_ItemConsume_Query_Resp = 0x978038,
            RayCity_UserMails_Query = 0x970039,
            RayCity_UserMails_Query_Resp = 0x978039,
            RayCity_CashItemDetailLog_Query = 0x970040,
            RayCity_CashItemDetailLog_Query_Resp = 0x978040,
            RayCity_Coupon_Query = 0x970041,
            RayCity_Coupon_Query_Resp = 0x978041,
            RayCity_ActiveCard_Query = 0x970042,
            RayCity_ActiveCard_Query_Resp = 0x978042,
            RayCity_BoardList_Query = 0x970043,
            RayCity_BoardList_Query_Resp = 0x978043,
            RayCity_BoardList_Insert = 0x970044,
            RayCity_BoardList_Insert_Resp = 0x978044,
            RayCity_BoardList_Delete = 0x970045,
            RayCity_BoardList_Delete_Resp = 0x978045,
            CS_Carinfo_Update = 0x960024,
            CS_Carinfo_Update_Resp = 0x968024,
            #endregion
        }
        #endregion

        /// <summary>
        /// Socket ��Ϣ��
        /// </summary>
        [Serializable()]
        [StructLayout(LayoutKind.Sequential)]
        public struct Message_Body
        {
            public TagFormat eTag;
            public TagName eName;
            public object oContent;
        }

        /// <summary>
        /// ��־����
        /// </summary>
        [Serializable()]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct TLogData
        {
            public int iSort;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 255)]
            public string strDescribe;
            //[MarshalAs(UnmanagedType.LPStr)]
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 255)]
            public string strException;
        }
    }
}
