/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

// class Window 를 상속받는 class 에서 아래 코드를 사용하여 WndProc 지정
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
        IntPtr WndHandle = IntPtr.Zero; // 윈도우 핸들

        #region Constants
        private const int WM_USER = 0x400;
        public const int StartMsgID = WM_USER + 0x400;
        public const string RealServerIP = "hts.ebestsec.co.kr";
        public const string TestServerIP = "demo.ebestsec.co.kr";
        public const int ServerPort = 20001;

        public int SendPacketSize { get; set; } = -1;   // 한번에 전송되는 데이터의 크기 (기본값은 -1)
        public int ConnectTimeOut { get; set; } = -1;   // 서버에 연결을 시도하는 시간으로, 1/1000 초 단위 (기본값은 -1, 10초)
                                                        // Connect 시도 시에 입력된 시간 동안 연결이 되지 않을 경우 TimeOut 발생
        #endregion

        #region Enums
        //------------------------------------------------------------------------------
        // 메시지 정의
        // 메시지의 ID값은 Connect시에 설정한 nStartMsgID와 더하여 사용하면 된다.
        public enum XING_MESSAGE
        {
            XM_DISCONNECT = StartMsgID + 1,                     // 서버와의 연결이 끊어졌을 경우 발생
            XM_RECEIVE_DATA = StartMsgID + 3,                   // RequestData로 요청한 데이터가 서버로부터 받았을 때 발생
            XM_RECEIVE_REAL_DATA = StartMsgID + 4,              // AdviseData로 요청한 데이터가 서버로부터 받았을 때 발생
            XM_LOGIN = StartMsgID + 5,                          // 서버로부터 로그인 결과 받았을때 발생
            XM_LOGOUT = StartMsgID + 6,                         // 서버로부터 로그아웃 결과 받았을때 발생
            XM_TIMEOUT_DATA = StartMsgID + 7,                   // RequestData로 요청한 데이터가 Timeout 이 발생했을때
            XM_RECEIVE_LINK_DATA = StartMsgID + 8,		        // HTS 에서 연동 데이터가 발생했을 때	: by zzin 2013.11.11  
            XM_RECEIVE_REAL_DATA_CHART = StartMsgID + 10,		// 실시간 자동 등록한 후 차트 조회 시, 지표실시간 데이터를 받았을 때  : by zzin 2013.08.14  
            XM_RECEIVE_REAL_DATA_SEARCH = StartMsgID + 11,		// 종목검색 실시간 데이터를 받았을 때 			: by 2017.11.24 LSW  
        }

        //------------------------------------------------------------------------------
        // Receive Flag
        public enum XING_RECEIVE_FLAG
        {
            REQUEST_DATA = 1,
            MESSAGE_DATA = 2,
            SYSTEM_ERROR_DATA = 3,
            RELEASE_DATA = 4,
            LINK_DATA = 10			// XM_RECEIVE_LINK_DATA 메시지의 구분 플래그
        }
        #endregion

        public XingAPIDllHelper(string windowName, Action<string> log) : base(windowName, log)
        {
        }

        // hwnd 얻기
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle message here
            switch (msg)
            {
                case (int)XING_MESSAGE.XM_DISCONNECT:
                    Log("XM_DISCONNECT 메세지 수신 완료");
                    break;

                case (int)XING_MESSAGE.XM_RECEIVE_DATA:
                    Log("XM_RECEIVE_DATA 메세지 수신 완료");
                    OnXMReceiveData(wParam, lParam);
                    break;

                case (int)XING_MESSAGE.XM_LOGIN:
                    Log("XM_LOGIN 메세지 수신 완료");
                    break;

                case (int)XING_MESSAGE.XM_LOGOUT:
                    Log("XM_LOGOUT 메세지 수신 완료");
                    break;

                case (int)XING_MESSAGE.XM_TIMEOUT_DATA:
                    Log("XM_TIMEOUT_DATA 메세지 수신 완료");
                    break;

                default:
                    Log("XING_MESSAGE 로 알수 없는 뭔가를 받았음.");
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
                    Log("REQUEST_DATA 를 받았음.");
                    break;

                case (int)XING_RECEIVE_FLAG.MESSAGE_DATA:
                    Log("MESSAGE_DATA 를 받았음.");
                    break;

                case (int)XING_RECEIVE_FLAG.SYSTEM_ERROR_DATA:
                    Log("SYSTEM_ERROR_DATA 를 받았음.");
                    int errorCode = GetLastError();
                    string errorMsg = GetErrorMessage(errorCode);
                    Log(errorMsg);
                    break;

                case (int)XING_RECEIVE_FLAG.RELEASE_DATA:
                    Log("RELEASE_DATA 를 받았음.");
                    break;

                case (int)XING_RECEIVE_FLAG.LINK_DATA:
                    Log("LINK_DATA 를 받았음.");
                    break;

                default:
                    Log("XM_RECEIVE_DATA로 알수 없는 뭔가를 받았음.");
                    break;
            }
        }



        #region 서버 연결 - 테스트 완료
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

        #region 로그인 - 테스트 완료
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

        #region 오류 - 테스트 완료
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

        #region 조회성TR 관련 - 테스트 완료
        public override int Request(string pszCode, IntPtr lpData, int nDataSize, bool bNext = false, string pszNextKey = "", int nTimeOut = 30)
        {
            if (IntPtr.Zero == WndHandle)
                return -1;

            return ETK_Request(WndHandle, pszCode, lpData, nDataSize, bNext, pszNextKey, nTimeOut);
        }

        //public static int 데이터요청(string 코드, IntPtr 데이터, int 데이터크기, byte 헤더타입, bool 압축여부, bool 암호와여부, bool 인증여부, bool 계속여부, int 시간초과)
        //{
        //    if (IntPtr.Zero == WndHandle)
        //        return -1;

        //    return ETK_RequestData(WndHandle, 코드, 데이터, 데이터크기, 헤더타입, 압축여부, 암호와여부, 인증여부, 계속여부, 시간초과);
        //}

        //public static int 데이터요청2(string 코드, IntPtr 데이터, int 데이터크기,
        //    int nAccPos, byte 헤더타입, string ServiceCode, bool 압축여부, bool 인증여부,
        //    bool bCert, bool 계속여부, string 다음키, int 시간초과)
        //{
        //    if (IntPtr.Zero == WndHandle)
        //        return -1;

        //    return ETK_RequestDataEx(WndHandle, 코드, 데이터, 데이터크기,
        //    nAccPos, 헤더타입, ServiceCode, 압축여부, 인증여부,
        //    bCert, 계속여부, 다음키, 시간초과);
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

        #region 실시간TR 관련
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

        #region 계좌 관련 - 테스트 완료
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

        #region 정보얻기 - 테스트 완료
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

        #region 부가 서비스 TR 조회
        
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
        // 서버 연결
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Connect(IntPtr hWnd, string strServerIP, int iPort, int iStartMsgID, int iTimeout, int iSendMaxPacketSize);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_IsConnected();

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Disconnect();

        // 로그인
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Login(IntPtr hWnd, string strID, string strPwd, string CertPwd, int iType, bool bShowCertErrDlg);

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_Logout(IntPtr hWnd);

        // 오류 메시지
        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_GetLastError();

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_GetErrorMessage(int iErrorCode, IntPtr pBuffer, int iSize);

        // 조회성TR 관련
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

        // 실시간TR 관련
        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_AdviseRealData(IntPtr hWnd, string TrNo, string strData, int nDataUnitLen);

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_UnadviseRealData(IntPtr hWnd, string TrNo, string strData, int nDataUnitLen);

        [DllImport("XingAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_UnadviseWindow(IntPtr hWnd);

        // 계좌 관련
        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ETK_GetAccountListCount();

        [DllImport("XingAPI.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ETK_GetAccountList(int iIndex, IntPtr strAcc, int iAccSize);

        // 정보얻기
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

    #region 자료구조
    // 조회TR 수신 Packet
    [StructLayout(LayoutKind.Sequential, Pack = 1), ComVisible(false)]
    public class RECV_PACKET
    {
        public int nRqID;                       // Request ID
        public int nDataLength;                 // 받은 데이터 크기
        public int nTotalDataBufferSize;        // lpData에 할당된 크기
        public int nElapsedTime;                // 전송에서 수신까지 걸린시간(1/1000초)
        public int nDataMode;                   // 1:BLOCK MODE, 2:NON-BLOCK MODE
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10 + 1)]
        public byte[] szTrCode;                 // AP Code
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] cCont;                    // '0' : 다음조회 없음, '1' : 다음조회 있음
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18 + 1)]
        public byte[] szContKey;                // 연속키, Data Header가 B 인 경우에만 사용
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30 + 1)]
        public byte[] szUserData;               // 사용자 데이터
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] szBlockName;              // Block 명, Block Mode 일때 사용
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;          // Message Data
    }

    // 메시지 수신 Packet
    [StructLayout(LayoutKind.Sequential, Pack = 1), ComVisible(false)]
    public class MSG_PACKET
    {
        public int RequestID;                   // Request ID
        public int nIsSystemError;              // 0:일반메시지, 1:System Error 메시지
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5 + 1)]
        public byte[] szMsgCode;                // 메시지 코드
        public int nMsgLength;                  // Message 길이
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpszMessageData;          // Message Data
    }

    // 실시간TR 수신 Packet
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