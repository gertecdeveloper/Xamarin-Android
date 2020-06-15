using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Gpos700_XamrinAndroid.Services
{
    [Activity(Label = "CodBarras")]
    public class CodBarras : Activity
    {
        private TextView txtLeitura;
        private Button btnEan8, btnEan13, btnEan14, btnQrcode;
        private Context context;

        public static string result;
        public static List<string> leituras;
        public Barcode barcode;

        public CodBarras()
        {
            this.context = Android.App.Application.Context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CodBarras);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            btnEan8 = FindViewById<Button>(Resource.Id.btnEan8);
            btnEan14 = FindViewById<Button>(Resource.Id.btnEan14);
            btnEan13 = FindViewById<Button>(Resource.Id.btnEan13);
            btnQrcode = FindViewById<Button>(Resource.Id.btnQrCode);
            txtLeitura = FindViewById<TextView>(Resource.Id.textLeitura);
            leituras = new List<string>();
            InitViews();
        }

        protected override void OnRestart()
        {
            StringBuilder stringBuilder = new StringBuilder(txtLeitura.Text.ToString());

            if (Barcode.resultado == null)
            {
                stringBuilder.Append(Barcode.name + ": Não foi possível ler o código.\n");
                txtLeitura.Text = (stringBuilder.ToString());
            }
            else
            {
                stringBuilder.Append(Barcode.resultado);
                txtLeitura.Text = (stringBuilder.ToString() + "\n");
            }
            base.OnRestart();
        }

        protected override void OnStart()
        {
            barcode = new Barcode();

            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        private void InitViews()
        {
            btnEan8.Click += delegate
            {
                Barcode.format.Add(ZXing.BarcodeFormat.EAN_8);
                Barcode.name = "EAN_8";
                StartActi();
            };

            btnEan14.Click += delegate
            {
                Barcode.format.Add(ZXing.BarcodeFormat.EAN_8);
                Barcode.format.Add(ZXing.BarcodeFormat.EAN_13);
                Barcode.format.Add(ZXing.BarcodeFormat.UPC_A);
                Barcode.format.Add(ZXing.BarcodeFormat.UPC_E);
                Barcode.format.Add(ZXing.BarcodeFormat.QR_CODE);
                Barcode.format.Add(ZXing.BarcodeFormat.CODE_93);
                Barcode.format.Add(ZXing.BarcodeFormat.CODE_39);
                Barcode.format.Add(ZXing.BarcodeFormat.CODE_128);
                Barcode.format.Add(ZXing.BarcodeFormat.ITF);
                Barcode.format.Add(ZXing.BarcodeFormat.RSS_14);
                Barcode.format.Add(ZXing.BarcodeFormat.RSS_EXPANDED);
                Barcode.name = "EAN_14";
                StartActi();

            };

            btnEan13.Click += delegate
            {
                Barcode.format.Add(ZXing.BarcodeFormat.EAN_13);
                Barcode.name = "EAN_13";
                StartActi();
            };

            btnQrcode.Click += delegate
            {
                Barcode.format.Add(ZXing.BarcodeFormat.QR_CODE);
                Barcode.name = "QR_CODE";
                StartActi();
            };
        }

        private void StartActi()
        {
            Barcode.resultado = null;
            Intent myIntent = new Intent(this.context, typeof(Barcode));
            myIntent.AddFlags(ActivityFlags.NewTask);
            this.context.StartActivity(myIntent);
        }
    }
}