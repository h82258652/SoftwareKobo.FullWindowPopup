using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI;
using WindowsPhoneDemo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍
using SoftwareKobo;

namespace WindowsPhoneDemo
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListViewDemo : Page, INotifyPropertyChanged
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private ObservableCollection<string> _list;

        public ObservableCollection<string> List
        {
            get
            {
                _list = _list ?? new ObservableCollection<string>();
                return _list;
            }
            set
            {
                _list = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("List"));
                }
            }
        }

        public ListViewDemo()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.DataContext = this;

            this.InitToTop();
        }

        private FullWindowPopup popup;

        public void InitToTop()
        {
            popup = new FullWindowPopup();

            var canvas = new Canvas();
            popup.Child = canvas;

            var border = new Border();
            border.BorderThickness = new Thickness(1);
            border.BorderBrush=new SolidColorBrush(Colors.Gray);
            border.CornerRadius = new CornerRadius(5);
            border.Padding = new Thickness(5);
            border.Background=new SolidColorBrush(Colors.DeepSkyBlue);
            border.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            border.Child = new TextBlock(){Text = "Top",FontSize = 20};
            border.PointerPressed += border_PointerPressed;
            border.PointerReleased += border_PointerReleased;
            border.PointerMoved += border_PointerMoved;

            canvas.Children.Add(border);

           Canvas.SetLeft(border,100);
            Canvas.SetTop(border,400);

            popup.Show();
        }

        void border_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var border = sender as Border;
           var p= e.GetCurrentPoint(null).Position;
            var pX = p.X - start.Value.X;
            var pY = p.Y - start.Value.Y;
            var bX = Canvas.GetLeft(border);
            var bY = Canvas.GetTop(border);
            Canvas.SetLeft(border,bX+pX);
            Canvas.SetTop(border,bY+pY);
        }

        private Point? start;

        void border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            start = null;
        }

        void border_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            start = e.GetCurrentPoint(null).Position;
        }

        /// <summary>
        /// 获取与此 <see cref="Page"/> 关联的 <see cref="NavigationHelper"/>。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get
            {
                return this.navigationHelper;
            }
        }

        /// <summary>
        /// 获取此 <see cref="Page"/> 的视图模型。
        /// 可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get
            {
                return this.defaultViewModel;
            }
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源; 通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。 首次访问页面时，该状态将为 null。</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="sender">事件的来源；通常为 <see cref="NavigationHelper"/></param>
        ///<param name="e">提供要使用可序列化状态填充的空字典
        ///的事件数据。</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (popup != null)
            {
                popup.Hide();
            }
        }

        #region NavigationHelper 注册

        /// <summary>
        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// <para>
        /// 应将页面特有的逻辑放入用于
        /// <see cref="NavigationHelper.LoadState"/>
        /// 和 <see cref="NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。
        /// </para>
        /// </summary>
        /// <param name="e">提供导航方法数据和
        /// 无法取消导航请求的事件处理程序。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            List.Add(DateTime.Now.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
