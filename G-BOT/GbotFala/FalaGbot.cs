using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace Gbot_XamarinAndroid.GbotFala
{
    [Activity(Label = "FalaGbot")]
    public class FalaGbot : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.falaGbot);
            // Create your application here

            var btnFrase1 = FindViewById<Button>(Resource.Id.btnFrase1);

            btnFrase1.Click += async delegate
            {
                await TextToSpeech.SpeakAsync("Sejam todos bem-vindos à aprensentação  do G-Bot");
            };

            var btnFrase2 = FindViewById<Button>(Resource.Id.btnFrase2);

            btnFrase2.Click += async delegate
            {
                await TextToSpeech.SpeakAsync("Possui reconhecimento facial");
            };

            var btnFrase3 = FindViewById<Button>(Resource.Id.btnFrase3);
            btnFrase3.Click += async delegate
            {
                await TextToSpeech.SpeakAsync("Microfone e leitor NFC");
            };

            var btnFrase4 = FindViewById<Button>(Resource.Id.btnFrase4);
            var txtFrase = FindViewById<EditText>(Resource.Id.txtFrase);
            btnFrase4.Click += async delegate
            {
                if (String.IsNullOrWhiteSpace(txtFrase.Text))
                {
                    Toast.MakeText(this, "Por favor digite uma palavra ou frase!", ToastLength.Long).Show();
                    return;
                }                  

                await TextToSpeech.SpeakAsync(txtFrase.Text);
            };

        }
    }
}