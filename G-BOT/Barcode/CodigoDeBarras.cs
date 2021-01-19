using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Wangpos.Sdk4.Libbasebinder;
using Xamarin.Essentials;

namespace Gbot_XamarinAndroid.Pages
{
    [Activity(Label = "CodigoDeBarras")]
    public class CodigoDeBarras : Activity
    {
        Scaner scaner;
        Boolean run_scaner;
        ListView list_resposta;
        List<string> items;
        ArrayAdapter<string> adapter;
        MediaPlayer player;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.codBarras);
            // Create your application here

            run_scaner = false;
            list_resposta = FindViewById<ListView>(Resource.Id.lvConsulta);

            items = new List<string>();
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
            list_resposta.Adapter = adapter;

            var btnStar = FindViewById<Button>(Resource.Id.btnStar);

            btnStar.Click += delegate
            {
                run_scaner = true;
                Byte[] leitura = new Byte[2048];
                int[] tamanho = new int[1];

                Task.Run(async () =>
                {
                    //inicia o scaner
                    scaner = new Scaner(this);

                    while (run_scaner)
                    {
                        // inicia o scan por um codigo de barras
                        int result = scaner.ScanSingle(leitura, tamanho);

                        if (result == 0 && run_scaner)
                        {
                            //converte o array de bytes para string
                            string leitura_barcode = System.Text.Encoding.UTF8.GetString(leitura, 2, leitura.Length - 2);

                            // necessario para atualizar a thread principal
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                BeepSound();
                                adapter.Insert(leitura_barcode, 0);
                                adapter.NotifyDataSetChanged();
                            });
                        }
                        // reduz o processamento
                        Thread.Sleep(1000);
                    }
                });

            };

            var btnStop = FindViewById<Button>(Resource.Id.btnStop);

            btnStop.Click += delegate
            {
                run_scaner = false;
            };

            var btnLigarLed = FindViewById<Button>(Resource.Id.btnLigarLed);

            btnLigarLed.Click += delegate
            {
                Barcode.LedUtil.SetRedLed();
            };

            var btnDesligarLed = FindViewById<Button>(Resource.Id.btnDesligarLed);

            btnDesligarLed.Click += delegate
            {
                Barcode.LedUtil.SetOffLed();
            };
        }

        protected override void OnStop()
        {
            base.OnStop();
            Barcode.LedUtil.SetOffLed();
            run_scaner = false;
        }

        public void BeepSound()
        {
            player = MediaPlayer.Create(this, Resource.Raw.Bleep);
            player.Start();
        }
    }
}