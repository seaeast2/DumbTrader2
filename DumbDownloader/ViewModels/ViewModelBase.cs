using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.ViewModels
{
    /// <summary>
    /// 모든 ViewModel class 의 Base class.
    /// 프로퍼티 변경 통지와 DisplayName 프로퍼티를 지원한다.
    /// 이 클래스는 추상 클래스 이다.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region 생성자
        protected ViewModelBase()
        {
        }
        #endregion // Constructor


        #region DisplayName
        /// <summary>
        /// 이 ViewModel 객체의 이름
        /// 상속 객체는 원하는 대로 이름을 변경 할 수 있다.
        /// </summary>
        public virtual string? DisplayName { get; protected set; }
        #endregion // DisplayName


        #region 디버깅 지원
        /// <summary>
        /// 이 ViewModel 객체가 public 인 property 를 
        /// 가지고 있지 않을 때 경고 메세지를 띄움. 
        /// Release 빌드일 때는 포함되지 않는다.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            //속성 이름이 이 개체의 실제 공용 인스턴스 속성과 일치하는지 확인합니다.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// 예외가 발생하는지 또는 잘못된 속성 이름이 VerifyPropertyName 메서드에
        /// 전달될 때 Debug.Fail()이 사용되는지 여부를 반환합니다. 
        /// 기본값은 false이지만 단위 테스트에서 사용하는 하위 클래스는 
        /// 이 속성의 getter를 재정의하여 true를 반환할 수 있습니다.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        #endregion // Debugging Aides


        #region INotifyPropertyChanged Members
        /// <summary>
        /// 이 객체의 프로퍼티가 새로운 값을 가지면 이벤트 발생 시킴
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 이 객체의 PropertyChanged 이벤트를 발생 시킴
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion // INotifyPropertyChanged Members


        #region IDisposable Members
        /// <summary>
        /// 이 개체가 응용 프로그램에서 제거되고 가비지 수집 대상이 될 때 호출됩니다.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// 자식 클래스는 이 메서드를 재정의하여 이벤트 처리기 제거와 
        /// 같은 정리 논리를 수행할 수 있습니다.
        /// </summary>
        protected virtual void OnDispose()
        {

        }

#if DEBUG
        /// <summary>
        /// ViewModel 개체가 적절하게 가비지 수집되도록 하는 데 유용합니다.
        /// </summary>
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif
        #endregion // IDisposable Members
    }
}
