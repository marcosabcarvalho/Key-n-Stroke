﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using WpfColorFontDialog;

namespace PxKeystrokesWPF
{
    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //MessageBox.Show("Convert " + value.ToString() + " tt: " + targetType.ToString() + " par " + parameter.ToString());
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //MessageBox.Show("Convert Back " + value.ToString() + " tt: " + targetType.ToString() + " par " + parameter.ToString());
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }

    public class FloatPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Log.e("CNV", "Convert " + value.ToString() + " tt: " + targetType.ToString());
            return (int) ((float) value * 100f);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Log.e("CNV", "Convert Back " + value.ToString() + " tt: " + targetType.ToString());
            return (int)value / 100f;
        }
    }

    public class MediaColorDrawingColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Log.e("CNV", "Convert " + value.ToString() + " tt: " + targetType.ToString());
            return UIHelper.ToMediaColor((System.Drawing.Color) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Log.e("CNV", "Convert Back " + value.ToString() + " tt: " + targetType.ToString());
            return UIHelper.ToDrawingColor((System.Windows.Media.Color) value);
        }
    }

    /// <summary>
    /// Interaktionslogik für Settings1.xaml
    /// </summary>
    public partial class Settings1 : Window
    {
        public Settings1(SettingsStore s)
        {
            InitializeComponent();
            settings = s;
            Log.e("BIN", "Set Data context in settings window");
            layout_root.DataContext = settings;
            AvailableShortcutKeys.Text = String.Join(", ", KeystrokeDisplay.AvailableKeysForShortcut);
            SettingsModeShortcutDefault.Text = settings.KeystrokeHistorySettingsModeShortcutDefault;
        }

        SettingsStore settings;

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            UrlOpener.OpenGithub();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            settings.SaveAll();
        }

        private void Bn_reset_position_Click(object sender, RoutedEventArgs e)
        {
            settings.PanelLocation = settings.PanelLocationDefault;
            settings.PanelSize = settings.PanelSizeDefault;
            settings.WindowLocation = settings.WindowLocationDefault;
            settings.WindowSize = settings.WindowSizeDefault;
        }

        private void bn_reset_all_Click(object sender, RoutedEventArgs e)
        {
            settings.ResetAll();
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnButtonTextFontClick(object sender, RoutedEventArgs e)
        {
            ColorFontDialog dialog = new ColorFontDialog(true, true, false);
            dialog.Font = settings.LabelFont;
            dialog.Loaded += FontDialogLoaded;
            if (dialog.ShowDialog() == true)
            {
                FontInfo font = dialog.Font;
                if (font != null)
                {
                    settings.LabelFont = font;
                }
            }
        }

        private void FontDialogLoaded(object sender, RoutedEventArgs e)
        {
            // run after the init funtions of the colordialog itself
            ColorFontDialog dialog = (ColorFontDialog)sender;
            TextBox sampleText = UIHelper.FindChild<TextBox>((DependencyObject)dialog.Content, "txtSampleText");
            sampleText.Background = new SolidColorBrush(UIHelper.ToMediaColor(settings.BackgroundColor));
            sampleText.Foreground = new SolidColorBrush(UIHelper.ToMediaColor(settings.LabelColor));
        }


        private void TextBoxKeystrokeHistorySettingsModeShortcut_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = TextBoxKeystrokeHistorySettingsModeShortcut.Text;
            if (KeystrokeDisplay.ValidateShortcutSetting(text))
            {
                settings.KeystrokeHistorySettingsModeShortcut = text;
                TextBoxKeystrokeHistorySettingsModeShortcut.Background = SystemColors.ControlLightLightBrush;
            } else
            {
                TextBoxKeystrokeHistorySettingsModeShortcut.Background = new SolidColorBrush(Color.FromRgb(250, 189, 185));
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            TextBoxKeystrokeHistorySettingsModeShortcut.Text = settings.KeystrokeHistorySettingsModeShortcutDefault;
        }
    }
}