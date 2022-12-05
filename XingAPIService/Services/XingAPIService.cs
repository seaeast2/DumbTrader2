using DumbStockAPIService.Helpers;
using DumbStockAPIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XA_DATASETLib;
using XA_SESSIONLib;

namespace DumbStockAPIService.Services
{
    public class XingAPIService
    {
        #region Constant
        const string REAL_SERVER_IP = "hts.ebestsec.co.kr";
        const string TEST_SERVER_IP = "demo.ebestsec.co.kr";
        const int SERVER_PORT = 20001;
        #endregion

        #region Public Section
        // 서버 접속 여부 확인
        public bool IsConnected
        { 
            get 
            {
                return myXASession.IsConnected();
            } 
        }

        public bool IsLoggedIn
        {
            get;
            private set;
        } = false;

        // 내부 로그를 받아볼 생각이면 추가한다.
        public Action<string>? Logger = null;

        // 서버 종류
        public enum ServerType { Real, Test };
        #endregion

        #region Private Section
        // xingAPI Session 클래스 선언
        XASessionClass myXASession = new XASessionClass();

        // 조회용 쿼리 컨테이너
        List<XATimerQuery> mTQuerys;
        #endregion

        public XingAPIService()
        {
            myXASession._IXASessionEvents_Event_Login += LoginEventHandler;
        }

        // 서버 연결
        public bool Connect(ServerType serverType)
        {
            bool isConn = false;
            if (serverType == ServerType.Real)
            {
                isConn = myXASession.ConnectServer(REAL_SERVER_IP, SERVER_PORT);
            }
            else
            {
                isConn = myXASession.ConnectServer(TEST_SERVER_IP, SERVER_PORT);
            }

            // 접속 실패일 경우 오류 출력 
            if (!isConn)
            {
                int ErrCode = myXASession.GetLastError();
                var ErrMsg = myXASession.GetErrorMessage(ErrCode);
                string err_log = String.Format("[서버연결] ERR - {0}({1})", ErrMsg, ErrCode);
                Logger?.Invoke(err_log); // Logger 가 있으면 출력
            }

            return isConn;
        }

        // 서버 연결 해제
        public void Disconnect()
        {
            myXASession.DisconnectServer();
        }

        // 로그인
        public void Login(string id, string pass, bool certErrDlg)
        {
            if (!IsConnected)
            {
                Logger?.Invoke("[로그인] 서버와의 접속이 끊어져 있습니다. 먼저 서버에 접속을 하세요.");
                return;
            }
            
            // 세번째 인자는 공인인증비번을 입력하는 칸이지만 보안을 위해 입력 받지 않고 보안 프로그램에서 입력하는 것이 좋다.
            // reqLogin의 결과 값은 로그인 완료 여부가 아닌, 서버에 로그인 요청을 전송 완료 여부이다. 때문에 Login Handler 함수를
            // 별도 추가하여 요청한 로그인 정보가 정상적으로 성공했는지 확인해야한다.
            bool loginResult = myXASession.Login(id, pass, "", 0, certErrDlg);
            if (!loginResult) 
            {
                Logger?.Invoke("[로그인] HTS 로그인 요청 실패");
                int ErrCode = myXASession.GetLastError();
                var ErrMsg = myXASession.GetErrorMessage(ErrCode);
                string err_log = String.Format("[로그인] ERR - {0}({1})", ErrMsg, ErrCode);
                Logger?.Invoke(err_log);
            }

            Logger?.Invoke("[로그인] HTS 로그인 요청 완료");
        }

        // 로그아웃
        public void Logout()
        {
            myXASession.Logout();
            Logger?.Invoke("[로그아웃] HTS 로그아웃 요청 완료");
        }

        // 로그인 확인
        private void LoginEventHandler(string code, string msg)
        {
            if (code == "0000") // code 0000은 성공을 의미
            {
                IsLoggedIn = true;
                Logger?.Invoke("[로그인] HTS 로그인 완료");
            }
        }

        // 서버 연결과 로그인 상태 확인
        public bool CheckConnAndLoginState()
        {
            if (!IsConnected)
            {
                Logger?.Invoke("[서버연결] 서버 연결이 끊어져 있습니다.");
                return false;
            }

            if (!IsLoggedIn)
            {
                Logger?.Invoke("[로그인] 로그인이 안되어 있습니다. 먼저 로그인 하세요.");
                return false;
            }

            return true;
        }

        // 현재 가진 모든 계좌 정보를 리턴
        public (bool Result, List<AccountInfo>? Accounts) GetAccountInfos()
        {
            if (!CheckConnAndLoginState()) 
            {
                return (false, null);
            }

            // 보유 계좌 정보 Loading
            List<AccountInfo> accountInfos = new List<AccountInfo>();
            int AccountCount = myXASession.GetAccountListCount();
            for (int i = 0; i < AccountCount; i++)
            {
                AccountInfo accountInfo = new AccountInfo();
                accountInfo.Number = myXASession.GetAccountList(i);
                accountInfo.Name = myXASession.GetAccountName(accountInfo.Number);
                accountInfo.NickName = myXASession.GetAcctNickname(accountInfo.Number);
                accountInfo.DetailName = myXASession.GetAcctDetailName(accountInfo.Number);

                
                Logger?.Invoke(String.Format("[계좌정보] Number : {0}, Name : {1}, NickName : {2}, Detail : {3}", 
                    accountInfo.Number, accountInfo.Name, accountInfo.NickName, accountInfo.DetailName));
                
                accountInfos.Add(accountInfo);
            }

            return (true, accountInfos);
        }

        // 조회 쿼리 등록
        public void RegisterTRQuery(string tCode)
        {
            // 기존에 같은 tcode가 없으면 추가
            if (mTQuerys.Find(t => t.TCode.Equals(tCode)) == null)
                mTQuerys.Add(new XATimerQuery(tCode));
        }
        
        // 조회 쿼리 얻기
        public XATimerQuery? GetTQuery(string tCode)
        {
            return mTQuerys.Find(t => t.TCode.Equals(tCode));
        }
    }
}
