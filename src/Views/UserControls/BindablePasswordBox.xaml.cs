using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Pkcs12Converter.Views.UserControls {
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl {
        Boolean isPasswordChanging;
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(String),
            typeof(BindablePasswordBox), new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, passwordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public BindablePasswordBox() {
            InitializeComponent();
        }

        public String Password {
            get => (String)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        void passwordBox_PasswordChanged(Object sender, RoutedEventArgs args) {
            isPasswordChanging = true;
            Password = PasswordBox.Password;
            isPasswordChanging = false;
        }
        void UpdatePassword() {
            if (!isPasswordChanging) {
                PasswordBox.Password = Password;
            }
        }

        static void passwordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            if (d is BindablePasswordBox passwordBox) {
                passwordBox.UpdatePassword();
            }
        }
    }
}

