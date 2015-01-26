using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace SoftwareKobo
{
    public sealed partial class FullWindowPopup : Control
    {
        /// <summary>
        /// 获取 Child 依赖项属性的标识符。
        /// </summary>
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(FullWindowPopup), new PropertyMetadata(default(UIElement)));

        private readonly Popup _popup;
        private Window _currentWindow;
        private Border _border;
        private AppBar _appBar;
        private Visibility? _appBarVisibility;

        public FullWindowPopup()
        {
            this.DefaultStyleKey = typeof(FullWindowPopup);

            _popup = new Popup() { Child = this };

            _currentWindow = Window.Current;
            _currentWindow.SizeChanged += Window_SizeChanged;
        }

        ~FullWindowPopup()
        {
            if (_currentWindow != null)
            {
                _currentWindow.SizeChanged -= Window_SizeChanged;
            }

#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ReSize();
        }

        /// <summary>
        /// 获取应用于 FullWindowPopup 子内容的 Transition 样式元素的集合。
        /// </summary>
        public TransitionCollection ChildTransitions
        {
            get
            {
                _popup.ChildTransitions = _popup.ChildTransitions ?? new TransitionCollection();
                return _popup.ChildTransitions;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // 获取 Generic.xaml 中定义的 Border;
            _border = (Border)GetTemplateChild("Border");
            // 初始化 Border 的大小，使其跟 Window 的大小一致。
            ReSize();
        }

        private void ReSize()
        {
            if (_border != null && _currentWindow != null)
            {
                Rect rect = _currentWindow.Bounds;
                _border.Width = rect.Width;
                _border.Height = rect.Height;
            }
        }

        /// <summary>
        /// 获取或设置要在弹出项中承载的内容。
        /// </summary>
        public UIElement Child
        {
            get
            {
                return (UIElement)GetValue(ChildProperty);
            }
            set
            {
                SetValue(ChildProperty, value);
            }
        }

        /// <summary>
        /// 获取或设置弹出项当前是否显示在屏幕上。
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return _popup.IsOpen;
            }
            set
            {
                if (value)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }

        /// <summary>
        /// 显示弹出项。
        /// </summary>
        public void Show()
        {
            if (_popup.IsOpen)
            {
                return;
            }

            // 清空上次显示时的缓存。
            _appBar = null;
            _appBarVisibility = null;

            if (IsAutoHideAppBar && _currentWindow != null)
            {
                Frame frame = _currentWindow.Content as Frame;
                if (frame != null)
                {
                    Page page = frame.Content as Page;
                    if (page != null)
                    {
                        _appBar = page.BottomAppBar;
                        if (_appBar != null)
                        {
                            // 缓存存在的 AppBar 的可视状态。
                            _appBarVisibility = _appBar.Visibility;
                            // 隐藏 AppBar。
                            _appBar.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }

#if WINDOWS_PHONE_APP

            if (IsAutoHideStatusBar)
            {
               StatusBar statusBar= StatusBar.GetForCurrentView();
                if (statusBar!=null)
                {
                    
                }
            }
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

            _popup.IsOpen = true;
        }

        /// <summary>
        /// 获取或设置显示弹出项时是否隐藏 AppBar。
        /// </summary>
        public bool IsAutoHideAppBar
        {
            get;
            set;
        }

        /// <summary>
        /// 隐藏弹出项。
        /// </summary>
        public void Hide()
        {
            if (_popup.IsOpen == false)
            {
                return;
            }

            // 缓存中存在 AppBar，恢复可视状态。
            if (_appBar != null && _appBarVisibility.HasValue)
            {
                _appBar.Visibility = _appBarVisibility.Value;
            }

#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif

            _popup.IsOpen = false;
        }

        /// <summary>
        /// 在 IsOpen 属性设置为 false 时引发。
        /// </summary>
        public event EventHandler<object> Closed
        {
            add
            {
                _popup.Closed += value;
            }
            remove
            {
                _popup.Closed -= value;
            }
        }

        /// <summary>
        /// 在 IsOpen 属性设置为 true 时引发。
        /// </summary>
        public event EventHandler<object> Opened
        {
            add
            {
                _popup.Opened += value;
            }
            remove
            {
                _popup.Opened -= value;
            }
        }
    }
}