using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Util;
using Android.Widget;
using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Single;
using EDMTDev.ZXingXamarinAndroid;
using System;
using System.Collections.Generic;
using ZXing;

namespace GertecXamarinAndroid.Services
{
    [Activity(Label = "Barcode")]

    public class Barcode : Activity, IPermissionListener
    {
        private ZXingScannerView scannerView;
        public Context context;
        public static List<ZXing.BarcodeFormat> format = new List<BarcodeFormat>();
        public static string name;
        public static string resultado;
        public Activity activity;
        public BarcodeFormat barcodeFormat;

        public Barcode()
        {
            this.context = Android.App.Application.Context;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Scanner);
            
            scannerView = FindViewById<ZXingScannerView>(Resource.Id.zxscan);
            activity = this;

            //Request Permission
            Dexter.WithActivity(this)
                .WithPermission(Manifest.Permission.Camera)
                .WithListener(this)
                .Check();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnDestroy()
        {
            if (scannerView != null)
            {
                scannerView.StopCamera();
            }
            base.OnDestroy();
        }
        protected override void OnRestart()
        {
            base.OnRestart();
        }
        protected override void OnStop()
        {
            format.Clear();
            base.OnStop();
        }
        protected override void OnPause()
        {
            base.OnPause();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        public void OnPermissionDenied(PermissionDeniedResponse p0)
        {
            Toast.MakeText(this, "Não foi possivel utilizar a camera!", ToastLength.Long).Show();
        }
        public void OnPermissionGranted(PermissionGrantedResponse p0)
        {
            scannerView.SetResultHandler(new MyResultHandler(this, this.scannerView));
            scannerView.SetFormats(format);
            scannerView.StartCamera();
        }
        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        {
            throw new NotImplementedException();
        }
        public class MyResultHandler : IResultHandler
        {
            public string formatName;
            private Barcode barcode;
            private ZXingScannerView scannerView;

            public MyResultHandler(Barcode barcode, ZXingScannerView scannerView)
            {
                this.barcode = barcode;
                this.scannerView = scannerView;
            }
            public void HandleResult(ZXing.Result rawResult)
            {
                if (Barcode.format.Count > 1)
                {
                    formatName = "EAN_14";

                }
                else if (Barcode.format.Contains(BarcodeFormat.EAN_8))
                {
                    formatName = "EAN_8";
                }
                else if (Barcode.format.Contains(BarcodeFormat.EAN_13))
                {
                    formatName = "EAN_13";
                }
                else if (Barcode.format.Contains(BarcodeFormat.QR_CODE))
                {
                    formatName = "QR_CODE";
                }

                Beeap();
                Barcode.resultado = (formatName + ": " + rawResult.Text);
                Result();
            }
            public void Beeap()
            {
                MediaPlayer mp;
                mp = MediaPlayer.Create(barcode.ApplicationContext, Resource.Raw.beep);
                mp.Start();
            }
            private void Result()
            {
                format.Clear();
                this.barcode.activity.Finish();
            }
        }
    }
}