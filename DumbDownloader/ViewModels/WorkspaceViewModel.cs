using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DumbDownloader.ViewModels
{
    /// <summary>
    /// 이 ViewModelBase 하위 클래스는 해당 CloseCommand가 
    /// 실행될 때 UI에서 제거되도록 요청합니다. 
    /// 이 클래스는 추상 클래스입니다.
    /// </summary>
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Fields
        RelayCommand? _closeCommand;
        #endregion // Fields

        #region Constructor
        protected WorkspaceViewModel()
        {
        }
        #endregion // Constructor

        #region CloseCommand
        /// <summary>
        /// 호출될 때 사용자 인터페이스에서 이 작업공간을 
        /// 제거하려고 시도하는 명령을 반환합니다.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(() => this.OnRequestClose());

                return _closeCommand;
            }
        }
        #endregion // CloseCommand

        #region RequestClose [event]
        /// <summary>
        /// 이 작업 공간이 UI에서 제거되어야 할 때 발생합니다.
        /// </summary>
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion // RequestClose [event]
    }
}
