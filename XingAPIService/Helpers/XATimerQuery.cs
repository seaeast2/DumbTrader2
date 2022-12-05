using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using XA_DATASETLib;

namespace DumbStockAPIService.Helpers
{
    // 
    public class XATimerQuery
    {
        #region Public Section
        public string TCode { get; }
        public string ResPath { get; }

        // 내부 로그를 받아볼 생각이면 추가한다.
        public Action<string>? Logger = null;
        #endregion

        #region Private Section
        // Xing 쿼리 객체
        XAQueryClass mXAQuery = new XAQueryClass();

        // 마지막으로 요청을 보낸 시간
        DateTime mLastRequestTime = DateTime.MinValue;
        int mTRRequestInterval = 1000; // 기본 1초(1000ms)당 1번으로 설정

        const int mRequestIntervalSafeMargin = 100; // 시간제한 안전마진 100ms
        #endregion

        public XATimerQuery(string tCode)
        {
            TCode = tCode;
            ResPath = String.Format(@"C:\eBEST\xingAPI\Res\{0}.res", TCode);
            mXAQuery.ResFileName = ResPath;

            // TR 의 요청 제한 시간을 ms 단위로 변경하여 저장
            mTRRequestInterval = 1000 / GetTRCountPerSec() + mRequestIntervalSafeMargin;
        }

        // Request용 InBlock 설정
        public void SetInBlock(Action<XATimerQuery> inBlock)
        {
            inBlock(this);
        }

        // 동기 방식
        public int Request(bool bNext, Action<XATimerQuery> inBlock, Action<string> receiveDataEvent)
        {
            SetInBlock(inBlock);
            return Request(bNext, receiveDataEvent);
        }

        public int Request(bool bNext, Action<string> receiveDataEvent)
        {
            // 요청을 보내기 전에 먼저 응답 callback 설정
            mXAQuery.ReceiveData += receiveDataEvent.Invoke;

            return Request(bNext);
        }

        public int Request(bool bNext)
        {
            // if (현재시간 - 마지막 시간 + 안전마진 < 시간제한)
            //      제한이 될 때까지 기다림
            //
            // Request() 실행
            DateTime now = DateTime.Now;
            int elapsedTime = now.Millisecond - mLastRequestTime.Millisecond + mRequestIntervalSafeMargin;
            if (elapsedTime < mTRRequestInterval)
            {
                Thread.Sleep(mTRRequestInterval - elapsedTime);
            }

            mLastRequestTime = now;
            int Res = mXAQuery.Request(bNext);
            if (Res < 0) // 실패 처리
            {
                Logger?.Invoke(String.Format("[요청실패 (동기)] TCODE : {0} 요청에 실패했습니다. Ret : {1}", TCode, Res));
                return Res;
            }

            Logger?.Invoke(String.Format("[요청완료(동기)] TCODE : {0}", TCode));
            return Res;
        }

        // 비동기 형식
        public async Task<int> RequestAsync(bool bNext, Action<XATimerQuery> inBlock, Action<string> receiveDataEvent)
        {
            SetInBlock(inBlock);
            return await RequestAsync(bNext, receiveDataEvent);
        }

        public async Task<int> RequestAsync(bool bNext, Action<string> receiveDataEvent)
        {
            // 요청을 보내기 전에 먼저 응답 callback 설정
            mXAQuery.ReceiveData += receiveDataEvent.Invoke;
            return await RequestAsync(bNext);
        }

        public async Task<int> RequestAsync(bool bNext)
        {
            DateTime now = DateTime.Now;
            int elapsedTime = now.Millisecond - mLastRequestTime.Millisecond + mRequestIntervalSafeMargin;
            if (elapsedTime < mTRRequestInterval)
            {
                await Task.Delay(mTRRequestInterval - elapsedTime);
            }

            mLastRequestTime = now;
            int Res = mXAQuery.Request(bNext);
            if (Res < 0) // 실패 처리
            {
                Logger?.Invoke(String.Format("[요청실패 (동기)] TCODE : {0} 요청에 실패했습니다. Ret : {1}", TCode, Res));
                return Res;
            }

            Logger?.Invoke(String.Format("[요청완료(비동기)] TCODE : {0}", TCode));
            return Res;
        }

        // OutBlock 접근 지원 함수
        public string GetFieldData(string szBlockName, string szFieldName, int nRecordIndex)
        {
            return mXAQuery.GetFieldData(szBlockName, szFieldName, nRecordIndex);
        }

        // InBlock Setting 지원 함수
        public void SetFieldData(string szBlockName, string szFieldName, int nOccursIndex, string szData)
        {
            mXAQuery.SetFieldData(szBlockName, szFieldName, nOccursIndex, szData);
        }

        // Occur 데이터 블록 개수 얻기
        public int GetBlockCount(string szBlockName)
        {
            return mXAQuery.GetBlockCount(szBlockName);
        }

        // 블록의 개수를 설정합니다. InBlock의 경우에만 사용합니다.
        // szBlockName : TR의 블록명
        // nCount : 블록의 개수
        public void SetBlockCount(string szBlockName, int nCount)
        {
            mXAQuery.SetBlockCount(szBlockName, nCount);
        }

        // szFileName Res 파일의 Full path 명
        // 반환값 : 지정한 위치의 Res 파일 정보를 읽는데 성공하면 TRUE, 실패하면 FALSE
        public bool LoadFromResFile(string szFileName)
        {
            return mXAQuery.LoadFromResFile(szFileName);
        }

        // 지정한 블록의 내용을 삭제합니다.
        public void ClearBlockdata(string szFieldName)
        {
            mXAQuery.ClearBlockdata(szFieldName);
        }

        // 블록의 전체 데이터(값)를 취득합니다.

        public string GetBlockData(string szBlockName)
        {
            return mXAQuery.GetBlockData(szBlockName);
        }

        // TR의 초당 전송 가능 횟수를 취득합니다.
        public int GetTRCountPerSec()
        {
            return mXAQuery.GetTRCountPerSec(TCode);
        }

        // 부가 서비스 TR을 요청합니다.
        // 자세한 사용법은 메뉴얼 참조. tcode 마다 szData 의 입력 여부가 차이가 난다.
        public int RequestService(string szCode, string szData)
        {
            return mXAQuery.RequestService(szCode, szData);
        }

        // 부가 서비스 TR을 해제합니다.
        public int RemoveService(string szCode, string szData)
        {
            return mXAQuery.RemoveService(szCode, szData);
        }

        // API에서 HTS로의 연동을 원할 때 요청합니다.
        // 자세한 내용은 메뉴얼 참조
        public bool RequestLinkToHTS(string szLinkName, string szData, string szFiller)
        {
            return mXAQuery.RequestLinkToHTS(szLinkName, szData, szFiller);
        }

        // t8411 TR 처럼 압축 데이터 수신이 가능한 TR에 압축 해제용으로 사용합니다
        public int Decompress(string szBlockName)
        {
            return mXAQuery.Decompress(szBlockName);
        }

        // 차트 지표 실시간 데이터를 수신 했을 때, 필드 데이터(값)을 취득합니다.
        // (차트 지표데이터 조회 시, 실시간 자동 등록을 ‘1”로 했을 경우)
        public string GetFieldChartRealData(string szBlockName, string szFieldName)
        {
            return mXAQuery.GetFieldChartRealData(szBlockName, szFieldName);
        }

        // COM 버전에서 attribute(속성) 정보를 취득합니다.
        public string GetAttribute(string szBlockName, string szFieldName, string szAttribute, int nRecordIndex)
        {
            return mXAQuery.GetAttribute(szBlockName, szFieldName, szAttribute, nRecordIndex);
        }

        // TR의 초당 전송 가능 횟수(Base)를 취득합니다
        // 1초당 1건 전송 가능시 1, 5초당 1건 전송 가능시 5를 취득합니다
        public int GetTRCountBaseSec()
        {
            return mXAQuery.GetTRCountBaseSec(TCode);
        }

        // 10분내 요청한 TR의 횟수를 취득합니다.
        public int GetTRCountRequest()
        {
            return mXAQuery.GetTRCountRequest(TCode);
        }

        // TR의 10분당 제한 건수를 취득합니다.
        // TR의 10분당 제한 건수를 취득합니다. 제한이 없는 경우 0 을 반환합니다.
        public int GetTRCountLimit()
        {
            return mXAQuery.GetTRCountLimit(TCode);
        }
    }
}
