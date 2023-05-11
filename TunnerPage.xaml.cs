using System;
using System.Timers;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Plugin.AudioRecorder;
using Xamarin.Essentials;
using FftSharp;
using System.IO.Pipes;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Core;
using System.Runtime.InteropServices.ComTypes;
using ScottPlot.Drawing.Colormaps;
using Xamarin.Forms.Shapes;
using ScottPlot.Styles;

namespace Mobile_app
{

    public partial class MainPage : ContentPage
    {
        public AudioRecorderService audioRecorderService = new AudioRecorderService()
        {
            StopRecordingOnSilence = false,
            TotalAudioTimeout = TimeSpan.FromSeconds(15),
            SilenceThreshold = 0.00f,
            StopRecordingAfterTimeout = true,
        };
        public bool isRunning1 = false;
        private bool isRunning2 = false;
        private bool isRunning3 = false;
        private bool isRunning4 = false;
        private bool isRunning5 = false;
        private bool isRunning6 = false;

        public MainPage()
        {
            InitializeComponent();
        }



        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            isRunning2 = false;
            isRunning3 = false;
            isRunning4 = false;
            isRunning5 = false;
            isRunning1 = false;
            isRunning6 = false;
            IconTintColorEffect.SetTintColor(string_1, Color.FromRgb(255, 255, 255));
            IconTintColorEffect.SetTintColor(string_2, Color.FromRgb(255, 255, 255));
            IconTintColorEffect.SetTintColor(string_3, Color.FromRgb(255, 255, 255));
            IconTintColorEffect.SetTintColor(string_4, Color.FromRgb(255, 255, 255));
            IconTintColorEffect.SetTintColor(string_5, Color.FromRgb(255, 255, 255));
            IconTintColorEffect.SetTintColor(string_6, Color.FromRgb(255, 255, 255));

        }

        private async void String1_Click(object sender, EventArgs e)
        {
            isRunning2 = false;
            isRunning3 = false;
            isRunning4 = false;
            isRunning5 = false;
            isRunning6 = false;
            if (!isRunning1)
            {
                isRunning1 = true;
                while (isRunning1)
                {
                    await audioRecorderService.StartRecording();
                    await Task.Delay(500); // запись звука продолжительностью 1 секунда
                    await audioRecorderService.StopRecording();
                    string path = audioRecorderService.FilePath;
                    double main = 0;

                    Transformation.Processing(path, out double[] frequency, out double[] power, 280, 360);
                    if (frequency != null && power != null)
                    {
                        double p = -10000;
                        for (int i = 0; i < frequency.Length; i++)
                        {
                            if (power[i] > p)
                            {
                                p = power[i];
                                main = frequency[i];
                            }
                        }
                    }

                    
                    double standard = 329.63; // значение эталона
                    int maxDistance = 2; // максимальное расстояние до эталона

                    double distance = Math.Abs(main - standard);
                    
                    int green = (int)(255 * (1 - distance / (double)(maxDistance)));
                    int red = (int)(255 * (distance / (double)(maxDistance)));
                    int blue = 0;

                    if (distance < maxDistance)
                    {
                        button1.Text = "Струна настроена";
                    }
                    else
                    {
                        if (main > standard)
                        {
                            button1.Text = $"Ослабь струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                        else
                        {
                            button1.Text = $"Затяни струну,эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                    }
                    IconTintColorEffect.SetTintColor(string_1, Color.FromRgb(red, green, blue));
                }
            }
        }
        private async void String2_Click(object sender, EventArgs e)
        {
            isRunning1 = false;
            isRunning3 = false;
            isRunning4 = false;
            isRunning5 = false;
            isRunning6 = false;

            if (!isRunning2)
            {

                isRunning2 = true;
                while (isRunning2)
                {
                    await audioRecorderService.StartRecording();
                    await Task.Delay(500); // запись звука продолжительностью 1 секунда
                    await audioRecorderService.StopRecording();
                    string path = audioRecorderService.FilePath;
                    double main = 0;



                    Transformation.Processing(path, out double[] frequency, out double[] power, 200, 290);
                    if (frequency != null && power != null)
                    {
                        double p = -10000;
                        for (int i = 0; i < frequency.Length; i++)
                        {
                            if (power[i] > p)
                            {
                                p = power[i];
                                main = frequency[i];
                            }
                        }
                    }

                    
                    double standard = 246.94; // значение эталона
                    int maxDistance = 2; // максимальное расстояние до эталона

                    double distance = Math.Abs(main - standard);
                    if (distance < maxDistance)
                    {
                        button1.Text = "Струна настроена";
                    }
                    else
                    {
                        if (main > standard)
                        {
                            button1.Text = $"Ослабь струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                        else
                        {
                            button1.Text = $"Затяни струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                    }
                    int green = (int)(255 * (1 - distance / (double)(maxDistance)));
                    int red = (int)(255 * (distance / (double)(maxDistance)));
                    int blue = 0;
                    IconTintColorEffect.SetTintColor(string_2, Color.FromRgb(red, green, blue));
                }
            }

        }
        private async void String3_Click(object sender, EventArgs e)
        {

            isRunning2 = false;
            isRunning1 = false;
            isRunning4 = false;
            isRunning5 = false;
            isRunning6 = false;
            if (!isRunning3)
            {

                isRunning3 = true;
                while (isRunning3)
                {
                    await audioRecorderService.StartRecording();
                    await Task.Delay(500); // запись звука продолжительностью 1 секунда
                    await audioRecorderService.StopRecording();
                    string path = audioRecorderService.FilePath;
                    double main = 0;

                    Transformation.Processing(path, out double[] frequency, out double[] power, 160, 230);
                    if (frequency != null && power != null)
                    {
                        double p = -10000;
                        for (int i = 0; i < frequency.Length; i++)
                        {
                            if (power[i] > p)
                            {
                                p = power[i];
                                main = frequency[i];
                            }
                        }
                    }
                   
                    double standard = 196; // значение эталона
                    int maxDistance = 2; // максимальное расстояние до эталона

                    double distance = Math.Abs(main - standard);
                    if (distance < maxDistance)
                    {
                        button1.Text = "Струна настроена";
                    }
                    else
                    {
                        if(main > standard)
                        {
                            button1.Text = $"Ослабь струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main,2)} Гц";
                        }
                        else
                        {
                            button1.Text = $"Затяни струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                    }
                    int green = (int)(255 * (1 - distance / (double)(maxDistance)));
                    int red = (int)(255 * (distance / (double)(maxDistance)));
                    int blue = 0;
                    IconTintColorEffect.SetTintColor(string_3, Color.FromRgb(red, green, blue));
                }
            }

        }
        private async void String4_Click(object sender, EventArgs e)
        {
            isRunning2 = false;
            isRunning3 = false;
            isRunning1 = false;
            isRunning5 = false;
            isRunning6 = false;
            if (!isRunning4)
            {
                isRunning4 = true;
                while (isRunning4)
                {
                    await audioRecorderService.StartRecording();
                    await Task.Delay(500); // запись звука продолжительностью 1 секунда
                    await audioRecorderService.StopRecording();
                    string path = audioRecorderService.FilePath;
                    double main = 0;

                    Transformation.Processing(path, out double[] frequency, out double[] power, 110, 180);
                    if (frequency != null && power != null)
                    {
                        double p = -10000;
                        for (int i = 0; i < frequency.Length; i++)
                        {
                            if (power[i] > p)
                            {
                                p = power[i];
                                main = frequency[i];
                            }
                        }
                    }

                    
                    double standard = 146.83; // значение эталона
                    int maxDistance = 2; // максимальное расстояние до эталона

                    double distance = Math.Abs(main - standard);
                    if (distance < maxDistance)
                    {
                        button1.Text = "Струна настроена";
                    }
                    else
                    {
                        if (main > standard)
                        {
                            button1.Text = $"Ослабь струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                        else
                        {
                            button1.Text = $"Затяни струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                    }
                    int green = (int)(255 * (1 - distance / (double)(maxDistance)));
                    int red = (int)(255 * (distance / (double)(maxDistance)));
                    int blue = 0;
                    IconTintColorEffect.SetTintColor(string_4, Color.FromRgb(red, green, blue));

                }
            }

        }
        private async void String5_Click(object sender, EventArgs e)
        {
            isRunning2 = false;
            isRunning3 = false;
            isRunning4 = false;
            isRunning1 = false;
            isRunning6 = false;
            if (!isRunning5)
            {

                isRunning5 = true;
                while (isRunning5)
                {
                    await audioRecorderService.StartRecording();
                    await Task.Delay(500); // запись звука продолжительностью 1 секунда


                    await audioRecorderService.StopRecording();
                    string path = audioRecorderService.FilePath;
                    double main = 0;

                    Transformation.Processing(path, out double[] frequency, out double[] power, 80, 140);
                    if (frequency != null && power != null)
                    {
                        double p = -10000;
                        for (int i = 0; i < frequency.Length; i++)
                        {
                            if (power[i] > p)
                            {
                                p = power[i];
                                main = frequency[i];
                            }
                        }
                    }

                    double standard = 110; // значение эталона
                    int maxDistance = 2; // максимальное расстояние до эталона

                    double distance = Math.Abs(main - standard);
                    if (distance < maxDistance)
                    {
                        button1.Text = "Струна настроена" ;
                    }
                    else
                    {
                        if (main > standard)
                        {
                            button1.Text = $"Ослабь струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                        else
                        {
                            button1.Text = $"Затяни струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                    }
                    int green = (int)(255 * (1 - distance / (double)(maxDistance)));
                    int red = (int)(255 * (distance / (double)(maxDistance)));
                    int blue = 0;
                    IconTintColorEffect.SetTintColor(string_5, Color.FromRgb(red, green, blue));

                }
            }

        }
        private async void String6_Click(object sender, EventArgs e)
        {
            
            isRunning2 = false;
            isRunning3 = false;
            isRunning4 = false;
            isRunning5 = false;
            isRunning1 = false;
            if (!isRunning6)
            {

                isRunning6 = true;
                while (isRunning6)
                {
                    await audioRecorderService.StartRecording();
                    await Task.Delay(500); // запись звука продолжительностью 1 секунда
                    await audioRecorderService.StopRecording();
                    string path = audioRecorderService.FilePath;
                    double main = 0;

                    Transformation.Processing(path, out double[] frequency, out double[] power, 50, 110);
                    if (frequency != null && power != null)
                    {
                        double p = -10000;
                        for (int i = 0; i < frequency.Length; i++)
                        {
                            if (power[i] > p)
                            {
                                p = power[i];
                                main = frequency[i];
                            }
                        }
                    }

                   
                    double standard = 82.41; // значение эталона
                    int maxDistance = 2; // максимальное расстояние до эталона

                    double distance = Math.Abs(main - standard);
                    if (distance < maxDistance)
                    {
                        button1.Text = "Струна настроена";
                    }
                    else
                    {
                        if (main > standard)
                        {
                            button1.Text = $"Ослабь струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                        else
                        {
                            button1.Text = $"Затяни струну, эталонная частота: {standard} Гц, реальная: {Math.Round(main, 2)} Гц";
                        }
                    }
                    int green = (int)(255 * (1 - distance / (double)(maxDistance)));
                    int red = (int)(255 * (distance / (double)(maxDistance)));
                    int blue = 0;
                    IconTintColorEffect.SetTintColor(string_6, Color.FromRgb(red, green, blue));
                }
            }
        }
    }
}