using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiscordRPC;

namespace CustomRPC
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.Saved == true)
            {
                realTextbox.Text = Properties.Settings.Default.DevID;
                realTextbox1.Text = Properties.Settings.Default.Details;
                realTextbox2.Text = Properties.Settings.Default.State;
                realTextbox4.Text = Properties.Settings.Default.LargeImageKey;
                realTextbox5.Text = Properties.Settings.Default.SmallImageKey;
                realTextbox6.Text = Properties.Settings.Default.SmallImageText;
                realTextbox7.Text = Properties.Settings.Default.LargeImageText;
            }
        }

        public DiscordRpcClient client;

        private int Time = 1; /* 1 = Now
                               * 2 = Hour
                               * 3 = off
                               */
        public void UpdateDiscordRPC(string Details, string State, string LargeImageKey, string LargeImageText, string SmallImageKey, string SmallImageText)
        {
            RichPresence presence = new RichPresence();
            presence.Details = Details;
            presence.State = State;

            Timestamps TimeRPC = new Timestamps();

            switch (Time)
            {
                case 1:
                    TimeRPC.Start = DateTime.UtcNow;
                    presence.Timestamps = TimeRPC;
                    break;

                case 2:
                    TimeRPC.Start = DateTime.ParseExact(realTextbox3.Text, "HH:mm:ss", CultureInfo.InvariantCulture);
                    presence.Timestamps = TimeRPC;
                    break;

                default: 
                    break;
            }

            if (!String.IsNullOrEmpty(LargeImageKey) || !String.IsNullOrEmpty(SmallImageKey))
            {
                Assets asset = new Assets();
                asset.LargeImageKey = LargeImageKey;
                asset.SmallImageKey = SmallImageKey;

                asset.LargeImageText= LargeImageText;
                asset.SmallImageText = SmallImageText;
                presence.Assets = asset;
            }
            client.SetPresence(presence);
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void discordDeveloperPortal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.com/developers/applications");
        }

        private void discordDeveloperPortal_MouseEnter(object sender, MouseEventArgs e)
        {
            discordDeveloperPortal.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF83CBE4"));
        }

        private void discordDeveloperPortal_MouseLeave(object sender, MouseEventArgs e)
        {
            discordDeveloperPortal.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6173BB"));
        }

        private void StartRpcButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RectangleBK.Fill = new SolidColorBrush(Color.FromRgb(60, 69, 165));
        }

        private void StartRpcButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RectangleBK.Fill = new SolidColorBrush(Color.FromRgb(88, 101, 242));
        }

        public string DevID;
        public string Details;
        public string State;

        public string LargeImageKey;
        public string SmallImageKey;

        public string LargeImageText;
        public string SmallImageText;

        private int running = 0;
        private async void RectangleBK_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateDiscordRPC(Details, State, LargeImageKey, LargeImageText, SmallImageKey, SmallImageText);
            if (running == 0)
            {
                StartRPCLabel.Content = "Starting";
                client.Initialize();
                running++;
            }
            else
            {
                StartRPCLabel.Content = "Updating";
            }
            await Task.Delay(2500);
            StartRPCLabel.Content = "Update";
        }

        private void realTextbox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Details = realTextbox1.Text;
        }

        private void realTextbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            State = realTextbox2.Text;
        }

        private void realTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            client = new DiscordRpcClient(realTextbox.Text);
        }

        private void TimeElapsedLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (Time <= 2)
                Time++;
            else
                Time = 1;

            switch (Time)
            {
                case 1:
                    TimeElapsedLabel.Content = "Time Elapsed - Now";
                    Textbox_Copy2.Visibility= Visibility.Hidden;
                    Textbox_Copy2.IsHitTestVisible= false;
                    return;
                case 2:
                    TimeElapsedLabel.Content = "Time Elapsed - Hour";
                    Textbox_Copy2.Visibility = Visibility.Visible;
                    Textbox_Copy2.IsHitTestVisible = true;
                    return;
                default:
                    TimeElapsedLabel.Content = "Time Elapsed - Off";
                    Textbox_Copy2.Visibility = Visibility.Hidden;
                    Textbox_Copy2.IsHitTestVisible = false;
                    return; 
            }
        }

        private void TimeElapsedLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            TimeElapsedLabel.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF83CBE4"));
        }

        private void TimeElapsedLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            TimeElapsedLabel.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6173BB"));
        }

        private void RectangleBK1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.DevID = DevID;
            Properties.Settings.Default.Details = Details;
            Properties.Settings.Default.State = State;
            Properties.Settings.Default.LargeImageKey = LargeImageKey;
            Properties.Settings.Default.SmallImageKey = SmallImageKey;
            Properties.Settings.Default.LargeImageText = LargeImageText;
            Properties.Settings.Default.SmallImageText = SmallImageText;
            Properties.Settings.Default.Saved = true;
            Properties.Settings.Default.Save();
        }


        private void SaveRpcButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RectangleBK1.Fill = new SolidColorBrush(Color.FromRgb(220, 221, 222));
        }

        private void SaveRpcButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RectangleBK1.Fill = new SolidColorBrush(Color.FromRgb(185, 187, 190));
        }

        public void realTextbox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            LargeImageKey = realTextbox4.Text;
        }

        private void realTextbox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            SmallImageKey = realTextbox5.Text;
        }

        private void realTextbox6_TextChanged(object sender, TextChangedEventArgs e)
        {
            LargeImageText = realTextbox6.Text;
        }

        private void realTextbox7_TextChanged(object sender, TextChangedEventArgs e)
        {
            SmallImageText = realTextbox7.Text;
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Close.Background = new SolidColorBrush(Color.FromRgb(237, 66, 69));
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Close.Background = new SolidColorBrush(Color.FromRgb(32, 34, 37));
        }

        private void Minimize_MouseEnter(object sender, MouseEventArgs e)
        {
            Minimize.Background = new SolidColorBrush(Color.FromRgb(51, 54, 59));
        }

        private void Minimize_MouseLeave(object sender, MouseEventArgs e)
        {
            Minimize.Background = new SolidColorBrush(Color.FromRgb(32, 34, 37));
        }

        private void Minimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
