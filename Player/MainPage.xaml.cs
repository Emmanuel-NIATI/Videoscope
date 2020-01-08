using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UltrasonicDistanceSensor;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Player
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // Variables liées au GPIO
        private GpioController _gpc;

        private GpioPin _pin27;
        private GpioPin _pin05;
        private GpioPin _pin13;
        private GpioPin _pin19;
        private GpioPin _pin26;

        // Variables liées aux boutons
        private bool btnGray;
        private bool btnWhite;
        private bool btnGreen;
        private bool btnBlue;
        private bool btnYellow;

        // Variables liées aux couleurs
        private Brush SCB_Gray;
        private Brush SCB_White;
        private Brush SCB_Green;
        private Brush SCB_Blue;
        private Brush SCB_Yellow;

        // Variable lmiée à la mesure de la distance
        HC_SR04 _HC_SR04;
        double distance;

        public MainPage()
        {

            this.InitializeComponent();

            // Initialisation des variables
            this.InitVariables();

            // Initialisation du GPIO
            this.InitGpio();

            // Initialisation du Timer
            this.TravauxTimer();

        }

        private void InitVariables()
        {

            // Variables liées aux boutons
            btnGray = false;
            btnWhite = false;
            btnGreen = false;
            btnBlue = false;
            btnYellow = false;

            // Variables liées aux couleurs
            SCB_Gray = new SolidColorBrush(Windows.UI.Colors.Gray);
            SCB_White = new SolidColorBrush(Windows.UI.Colors.White);
            SCB_Green = new SolidColorBrush(Windows.UI.Colors.Green);
            SCB_Blue = new SolidColorBrush(Windows.UI.Colors.Blue);
            SCB_Yellow = new SolidColorBrush(Windows.UI.Colors.Yellow);

            // Variables liées à la lecture des vidéos
            media.Visibility = Visibility.Visible;
            media.Source = new Uri("ms-appx:///Video/divers/2018-03-22 - France 2.mp4");

            // Variable associée à la mesure de la distance
            _HC_SR04 = new HC_SR04( HC_SR04.AvailableGpioPin.GpioPin_22, HC_SR04.AvailableGpioPin.GpioPin_17);
            distance = 0;

        }

        private void InitGpio()
        {

            // Configuration du contrôleur du GPIO par défaut
            _gpc = GpioController.GetDefault();

            // Bouton Gris sur GPIO27 en entrée
            _pin27 = _gpc.OpenPin(27);
            _pin27.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin27.DebounceTimeout = new TimeSpan(10000);

            // Bouton Blanc sur GPIO05 en entrée
            _pin05 = _gpc.OpenPin(5);
            _pin05.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin05.DebounceTimeout = new TimeSpan(10000);

            // Bouton Vert sur GPIO13 en entrée
            _pin13 = _gpc.OpenPin(13);
            _pin13.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin13.DebounceTimeout = new TimeSpan(10000);

            // Bouton Bleu sur GPIO19 en entrée
            _pin19 = _gpc.OpenPin(19);
            _pin19.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin19.DebounceTimeout = new TimeSpan(10000);

            // Bouton Jaune sur GPIO26 en entrée
            _pin26 = _gpc.OpenPin(26);
            _pin26.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _pin26.DebounceTimeout = new TimeSpan(10000);

        }

        private void TravauxTimer()
        {

            // Configuration du Timer
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();

        }

        private void playVideo(String fileName)
        {


        }

        private void DispatcherTimer_Tick(object sender, object e)
        {

            if(btnGray)
            {

                btn.Background = SCB_Gray;
                btnGray = !btnGray;

                // playVideo("ms-appx:///Video/fede/fediasa.mp4");
                playVideo("ms-appx:///Video/divers/Liste 01 - Piste 01 - Arielle Dombasle - Extraterrestre.mp4");

            }
            else if(btnWhite)
            {

                btn.Background = SCB_White;
                btnWhite = !btnWhite;

                // playVideo("ms-appx:///Video/snpad/snpad.mp4");
                playVideo("ms-appx:///Video/divers/Liste 01 - Piste 04 - Alice et Moi - C'est toi qu'elle préfère.mp4");

            }
            else if (btnGreen)
            {

                btn.Background = SCB_Green;
                btnGreen = !btnGreen;

                // playVideo("ms-appx:///Video/snptp/snptp.mp4");
                playVideo("ms-appx:///Video/divers/Liste 01 - Piste 05 - Angèle - Je veux tes yeux.mp4");

            }
            else if (btnBlue)
            {

                btn.Background = SCB_Blue;
                btnBlue = !btnBlue;

                // playVideo("ms-appx:///Video/ufso/ufso.mp4");
                playVideo("ms-appx:///Video/divers/Liste 01 - Piste 06 - Indochine - Des fleurs pour Salinger.mp4");

            }
            else if (btnYellow)
            {

                btn.Background = SCB_Yellow;
                btnYellow = !btnYellow;
                playVideo("ms-appx:///Video/divers/Liste 02 - Piste 01 - Kraftwerk - Radioactivity.mp4");

            }

            distance = _HC_SR04.GetDistance();

            btn.Content = distance + " cm";

            if( distance < 50 )
            {

                media.Play();

            }

        }


        // Action sur les boutons
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            // Bouton Gris sur GPIO27
            _pin27.ValueChanged += _pin27_ValueChanged;
            // Bouton Blanc sur GPIO05
            _pin05.ValueChanged += _pin05_ValueChanged;
            // Bouton Vert sur GPIO13
            _pin13.ValueChanged += _pin13_ValueChanged;
            // Bouton Bleu sur GPIO19
            _pin19.ValueChanged += _pin19_ValueChanged;
            // Bouton Jaune sur GPIO26
            _pin26.ValueChanged += _pin26_ValueChanged;

        }

        // Bouton Gris
        private void _pin27_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                btnGray = true;
                                
            }



        }

        // Bouton Blanc
        private void _pin05_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                btnWhite = true;

            }

        }

        // Bouton Vert
        private void _pin13_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                btnGreen = true;

            }

        }

        // Bouton Bleu
        private void _pin19_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                btnBlue = true;

            }

        }

        // Bouton Jaune
        private void _pin26_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            // Détection du front montant
            if (args.Edge == GpioPinEdge.RisingEdge)
            {

                btnYellow = true;

            }

        }

    }

}
