using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo
{
    public sealed partial class FullWindowPopup : Control
    {
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (IsCloseOnBackPress)
            {
                Hide();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 获取或设置显示弹出项时是否可以按下返回键隐藏。
        /// </summary>
        public bool IsCloseOnBackPress
        {
            get;
            set;
        }
    }
}
