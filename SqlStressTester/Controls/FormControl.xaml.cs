using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SqlStressTester.Controls
{
    /// <summary>
    /// Interaktionslogik für FormControl.xaml
    /// </summary>
    public partial class FormControl : UserControl
    {

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(FormControl), new(null));
        public string HeaderText { get => (string)GetValue(HeaderTextProperty); set => SetValue(HeaderTextProperty, value); }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FormControl), new(null));
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        public FormControl()
        {
            InitializeComponent();
        }
    }
}
