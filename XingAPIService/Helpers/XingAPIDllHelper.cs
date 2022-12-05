/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

// class Window �� ��ӹ޴� class ���� �Ʒ� �ڵ带 ����Ͽ� WndProc ����
//protected override void OnSourceInitialized(EventArgs e)
//{
//    base.OnSourceInitialized(e);
//    HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
//    source.AddHook(xingApiHelper.WndProc);
//}


namespace XingAPIService.Helpers
{
    public class XingAPIDllHelper : XingAPIHelper
    {
        IntPtr WndHandle = IntPtr.Zero; // ������ �ڵ�

        #region Constants
        private const int WM_USER = 0x400;
        public const int StartMsgID = WM_USER + 0x400;
        public const string RealServerIP = "hts.ebestsec.co.kr";
        public const string TestServerIP = "demo.ebestsec.co.kr";
        public const int ServerPort = 20001;

        public int SendPacketSize { get; set; } = -1;   // �ѹ��� ���۵Ǵ� �������� ũ�� (�⺻���� -1)
        public int ConnectTimeOut { get; set; } = -1;   // ������ ������ �õ��ϴ� �ð�����, 1/1000 �� ���� (�⺻���� -1, 10��)
                                                        // Connect �õ� �ÿ� �Էµ� �ð� ���� ������ ���� ���� ��� TimeOut �߻�
        #endregion

        #region Enums
        //------------------------------------------------------------------------------
        // �޽��� ����
        // �޽����� ID���� Connect�ÿ� ������ nStartMsgID�� ���Ͽ� ����ϸ� �ȴ�.
        public enum XING_MESSAGE
        {
            XM_DISCONNECT = StartMsgID + 1,                     // �������� ������ �������� ��� �߻�
            XM_RECEIVE_DATA = StartMsgID + 3,                   // RequestData�� ��û�� �����Ͱ� �����κ��� �޾��� �� �߻�
            XM_RECEIVE_REAL_DATA = StartMsgID + 4,              // AdviseData�� ��û�� �����Ͱ� �����κ��� �޾��� �� �߻�
            XM_LOGIN = StartMsgID + 5,                          // �����κ��� �α��� ��� �޾����� �߻�
            XM_LOGOUT = StartMsgID + 6,                         // �����κ��� �α׾ƿ� ��� �޾����� �߻�
            XM_TIMEOUT_DATA = StartMsgID + 7,                   // RequestData�� ��û�� �����Ͱ� Timeout �� �߻�������
            XM_RECEIVE_LINK_DATA = StartMsgID + 8,		        // HTS ���� ���� �����Ͱ� �߻����� ��	: by zzin 2013.11.11  
            XM_RECEIVE_REAL_DATA_CHART = StartMsgID + 10,		// �ǽð� �ڵ� ����� �� ��Ʈ ��ȸ ��, ��ǥ�ǽð� �����͸� �޾��� ��  : by zzin 2013.08.14  
            XM_RECEIVE_REAL_DATA_SEARCH = StartMsgID + 11,		// ����˻� �ǽð� �����͸� �޾��� �� 			: by 2017.11.24 LSW  
        }

        //------------------------------------------------------------------------------
        // Receive Flag
        public enum XING_RECEIVE_FLAG
        {
            REQUEST_DATA = 1,
            MESSAGE_DATA = 2,
            SYSTEM_ERROR_DATA = 3,
            RELEASE_DATA = 4,
            LINK_DATA = 10			// XM_RECEIVE_LINK_DATA �޽����� ���� �÷���
        }
        #endregion

        public XingAPIDllHelper(string windowName, Action<string> log) : base(windowName, log)
        {
        }

        // hwnd ���
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle message here
            switch (msg)
            {
                case (int)XING_MESSAGE.XM_DISCONNECT:
                    Log("XM_DISCONNECT �޼��� ���� �Ϸ�");
                    break;

                case (int)XING_MESSAGE.XM_RECEIVE_DATA:
                    Log("XM_RECEIVE_DATA �޼��� ���� �Ϸ�");
                    OnXMReceiveData(wParam, lParam);
                    break;

                case (int)XING_MESSAGE.XM_LOGIN:
                    Log("XM_LOGIN �޼��� ���� �Ϸ�");
                    break;

                case (int)XING_MESSAGE.XM_LOGOUT:
                    Log("XM_LOGOUT �޼��� ���� �Ϸ�");
                    break;

                case (int)XING_MESSAGE.XM_TIMEOUT_DATA:
                    Log("XM_TIMEOUT_DATA �޼��� ���� �Ϸ�");
                    break;

                default:
                    Log("XING_MESSAGE �� �˼� ���� ������ �޾���.");
                    break;
            }

            return IntPtr.Zero;
        }

        public void OnXMReceiveData(IntPtr wParam, IntPtr lParam)
        {
            int wparam = wParam.ToInt32();
            switch (wparam)
            {
                case (int)XING_RECEIVE_FLAG.REQUEST_DATA:
                    Log("REQUEST_DATA �� �޾���.");
                    break;

                case (int)XING_RECEIVE_FLAG.MESSAGE_DATA:
                    Log("MESSAGE_DATA �� �޾���.");
                    break;

                case (int)XING_RECEIVE_FLAG.SYSTEM_ERROR_DATA:
                    Log("SYSTEM_ERROR_DATA �� �޾���.");
                    int errorCode = GetLastError();
                    string errorMsg = GetErrorMessage(errorCode);
                    Log(errorMsg);
                    break;

                case (int)XING_RECEIVE_FLAG.RELEASE_DATA:
                    Log("RELEASE_DATA �� �޾���.");
                    break;

                case (int)XING_RECEIVE_FLAG.LINK_DATA:
                    Log("LINK_DATA �� �޾���.");
                    break;

                default:
                    Log("XM_RECEIVE_DATA�� �˼� ���� ������ �޾���.");
                    break;
            }
        }



        #region ���� ���� - �׽�Ʈ �Ϸ�
        public override bool Connect(string pszSvrIP, int nPort)
        {
            WndHandle = FindWindow(null, WindowName);
            if (WndHandle == IntPtr.Zero)
                return false;

            return ETK_Connect(WndHandle, pszSvrIP, nPort, StartMsgID, ConnectTimeOut, SendPacketSize);
        }

        public override bool IsConnected()
        {
            return ETK_IsConnected();
        }

        public override bool Disconnect()
        {
            return ETK_Disconnect();
        }
        #endregion

        #region �α��� - �׽�Ʈ �Ϸ�
        public override bool Login(string pszID, string pszPwd, string pszCertPwd, int nType, bool bShowCertErrDlg = false)
        {
            if (IntPtr.Zero == WndHandle)
                return false;

            return ETK_Login(WndHandle, pszID, pszPwd, pszCertPwd, nType, bShowCertErrDlg);
        }

        public override bool Logout()
        {
            if (IntPtr.Zero == WndHandle)
                return false;

            return ETK_Logout(WndHandle);
        }
        #endregion

        #region ���� - �׽�Ʈ �Ϸ�
        public override int GetLastError()
        {
            return ETK_GetLastError();
        }

        public override string GetErrorMessage(int nErrorCode)
        {
            IntPtr pErrorStr = Marshal.AllocHGlobal(512);
            ETK_GetErrorMessage(nErrorCode, pErrorStr, 512);

            string ErrorMsg = Marshal.PtrToStringAnsi(pErrorStr);
            Marshal.FreeHGlobal(pErrorStr);
            return ErrorMsg;
        }
        #endregion

        #region ��ȸ��TR ���� - �׽�Ʈ �Ϸ�
        public override int Request(string pszCode, IntPtr lpData, int nDataSize, bool bNext = false, string pszNextKey = "", int nTimeOut = 30)
        {
            if (IntPtr.Zero == WndHandle)
                return -1;

            return ETK_Request(WndHandle, pszCode, lpData, nDataSize, bNext, pszNextKey, nTimeOut);
        }

        //public static int �����Ϳ�û(string �ڵ�, IntPtr ������, int ������ũ��, byte ���Ÿ��, bool ���࿩��, bool ��ȣ�Ϳ���, bool ��������, bool ��ӿ���, int �ð��ʰ�)
        //{
        //    if (IntPtr.Zero == WndHandle)
        //        return -1;

        //    return ETK_RequestData(WndHandle, �ڵ�, ������, ������ũ��, ���Ÿ��, ���࿩��, ��ȣ�Ϳ���, ��������, ��ӿ���, �ð��ʰ�);
        //}

        //public static int �����Ϳ�û2(string �ڵ�, IntPtr ������, int ������ũ��,
        //    int nAccPos, byte ���Ÿ��, string ServiceCode, bool ���࿩��, bool ��������,
        //    bool bCert, bool ��ӿ���, string ����Ű, int �ð��ʰ�)
        //{
        //    if (IntPtr.Zero == WndHandle)
        //        return -1;

        //    return ETK_RequestDataEx(WndHandle, �ڵ�, ������, ������ũ��,
        //    nAccPos, ���Ÿ��, ServiceCode, ���࿩��, ��������,
        //    bCert, ��ӿ���, ����Ű, �ð��ʰ�);
        //}

        public override bool ReleaseRequestData(int nRequestID)
        {
            return ETK_ReleaseRequestData(nRequestID);
        }

        public override bool ReleaseMessageData(IntPtr lParam)
        {
            return ETK_ReleaseMessageData(lParam);
        }
        #endregion

        #region �ǽð�TR ����
        public override bool AdviseRealData(string TrNo, string strData, int nDataUnitLen)
        {
            if (IntPtr.Zero == WndHandle)
                return false;

            return ETK_AdviseRealData(WndHandle, TrNo, strData, nDataUnitLen);
        }

        public override bool UnadviseRealData(string TrNo, string strData, int nDataUnitLen)
        {
            if (IntPtr.Zero == WndHandle)
                return false;

            return ETK_UnadviseRealData(WndHandle, TrNo, strData, nDataUnitLen);
        }

        public override bool UnadviseWindow()
        {
            if (IntPtr.Zero == WndHandle)
                return false;

            return ETK_UnadviseWindow(WndHandle);
        }
        #endregion

        #region ���� ���� - �׽�Ʈ �Ϸ�
        public override int GetAccountListCount()
        {
            return ETK_GetAccountListCount();
        }

        public override bool GetAccountList(int nIndex, ref string AccountNumber)
        {
            IntPtr pReceivedAccountData = Marshal.AllocHGlobal(512);
            bool bResult = ETK_GetAccountList(nIndex, pReceivedAccountData, 512);
            if (true == bResult)
                AccountNumber = Marshal.PtrToStringAnsi(pReceivedAccountData);
            else
                AccountNumber = "";
            Marshal.FreeHGlobal(pReceivedAccountData);

            return bResult;
        }
        #endregion

        #region ������� - �׽�Ʈ �Ϸ�
        public override string GetCommMedia()
        {
            IntPtr pReceivedData = Marshal.AllocHGlobal(512);
            ETK_GetCommMedia(pReceivedData);

            string MediaInfo = Marshal.PtrToStringAnsi(pReceivedData);
            Marshal.FreeHGlobal(pReceivedData);
            return MediaInfo;
        }

        public override string GetETKMedia()
        {
            IntPtr pReceivedData = Marshal.AllocHGlobal(512);
            ETK_GetETKMedia(pReceivedData);

            string MediaInfo = Marshal.PtrToStringAnsi(pReceivedData);
            Marshal.FreeHGlobal(pReceivedData);
            return MediaInfo;
        }

        public override string GetClientIP()
        {
            IntPtr pRcvData = Marshal.AllocHGlobal(512);
            ETK_GetClientIP(pRcvData);

            string ClientIP = Marshal.PtrToStringAnsi(pRcvData);
            Marshal.FreeHGlobal(pRcvData);
            return ClientIP;
        }

        public override string GetServerName()
        {
            IntPtr pRcvData = Marshal.AllocHGlobal(512);
            ETK_GetServerName(pRcvData);

            string SeverName = Marshal.PtrToStringAnsi(pRcvData);
            Marshal.FreeHGlobal(pRcvData);
            return SeverName;
        }
        #endregion

        #region �ΰ� ���� TR ��ȸ
        
        public override int RequestService(string strCode, string strData)
        {
            if (IntPtr.Zero == WndHandle)
                return -1;

            return ETK_RequestService(WndHandle, strCode, strData);
        }
        
        public override int RemoveService(string strCode, string strData)
        {
            if (IntPtr.Zero == WndHandle)
                return -1;

            return ETK_RemoveService(WndHandle, strCode, strData);
        }
        #endregion

        #region XingApi
        // ���� ����
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Connect(IntPtr hWnd, string strServerIP, int iPort, int iStartMsgID, int iTimeout, int iSendMaxPacketSize);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_IsConnected();

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Disconnect();

        // �α���
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Login(IntPtr hWnd, string strID, string strPwd, string CertPwd, int iType, bool bShowCertErrDlg);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Logout(IntPtr hWnd);

        // ���� �޽���
        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_GetLastError();

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_GetErrorMessage(int iErrorCode, IntPtr pBuffer, int iSize);

        // ��ȸ��TR ����
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_Request(IntPtr hWnd, string strTrCode, IntPtr pData, int iDataSize,
            [In, MarshalAs(UnmanagedType.Bool)] bool bNext, string strNextKey, int nTimeOut);

        
        //[DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //private static extern int ETK_RequestData(IntPtr hWnd, string strCode, IntPtr pData, int iDataSize,
        //    byte chHeaderType,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bCompress,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bEncrypt,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bCert,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bNext,
        //    int nTimeOut);

        //[DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //private static extern int ETK_RequestDataEx(IntPtr hWnd, string strCode, IntPtr pData, int iDataSize,
        //    int nAccPos, byte chHeaderType, string ServiceCode,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bCompress,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bEncrypt,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bCert,
        //    [In, MarshalAs(UnmanagedType.Bool)] bool bNext,
        //    string NextKey, int nTimeOut);
        

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_ReleaseRequestData(int nRequestID);

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_ReleaseMessageData(IntPtr lParam);

        // �ǽð�TR ����
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_AdviseRealData(IntPtr hWnd, string TrNo, string strData, int nDataUnitLen);

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_UnadviseRealData(IntPtr hWnd, string TrNo, string strData, int nDataUnitLen);

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_UnadviseWindow(IntPtr hWnd);

        // ���� ����
        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_GetAccountListCount();

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_GetAccountList(int iIndex, IntPtr strAcc, int iAccSize);

        // �������
        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void ETK_GetCommMedia(IntPtr pMedia);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void ETK_GetETKMedia(IntPtr pMedia);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void ETK_GetClientIP(IntPtr pClient);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void ETK_GetServerName(IntPtr pServerName);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_RequestService(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string strCode, [MarshalAs(UnmanagedType.LPTStr)] string strData);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_RemoveService(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string strCode, [MarshalAs(UnmanagedType.LPTStr)] string strData);

        #endregion
    }

    #region �ڷᱸ��
    // ��ȸTR ���� Packet
    [StructLayout(LayoutKind.Sequential, Pack = 1), ComVisible(false)]
    public class RECV_PACKET
    {
        public int nRqID;                       // Request ID
        public int nDataLength;                 // ���� ������ ũ��
        public int nTotalDataBufferSize;        // lpData�� �Ҵ�� ũ��
        public int nElapsedTime;                // ���ۿ��� ���ű��� �ɸ��ð�(1/1000��)
        public int nDataMode;                   // 1:BLOCK MODE, 2:NON-BLOCK MODE
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10 + 1)]
        public byte[] szTrCode;                 // AP Code
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] cCont;                    // '0' : ������ȸ ����, '1' : ������ȸ ����
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18 + 1)]
        public byte[] szContKey;                // ����Ű, Data Header�� B �� ��쿡�� ���
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30 + 1)]
        public byte[] szUserData;               // ����� ������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] szBlockName;              // Block ��, Block Mode �϶� ���
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;          // Message Data
    }

    // �޽��� ���� Packet
    [StructLayout(LayoutKind.Sequential, Pack = 1), ComVisible(false)]
    public class MSG_PACKET
    {
        public int RequestID;                   // Request ID
        public int nIsSystemError;              // 0:�Ϲݸ޽���, 1:System Error �޽���
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5 + 1)]
        public byte[] szMsgCode;                // �޽��� �ڵ�
        public int nMsgLength;                  // Message ����
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpszMessageData;          // Message Data
    }

    // �ǽð�TR ���� Packet
    [StructLayout(LayoutKind.Sequential, Pack = 1), ComVisible(false)]
    public struct RECV_REAL_PACKET
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 + 1)]
        public byte[] szTrCode;

        public int nKeyLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 + 1)]
        public byte[] szKeyData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 + 1)]
        public byte[] szRegKey;

        public int nDataLength;

        [MarshalAs(UnmanagedType.LPStr)]
        public string pszData;
    }
    #endregion
}
*/