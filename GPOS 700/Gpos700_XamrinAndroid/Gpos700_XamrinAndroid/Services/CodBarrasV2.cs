using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing.QrCode;
using EDMTDev.ZXingXamarinAndroid;
using Com.Karumi.Dexter;
using Android;
using Com.Karumi.Dexter.Listener.Single;
using Com.Karumi.Dexter.Listener;
using Android.Media;

namespace Gpos700_XamrinAndroid.Services
{
    [Activity(Label = "CodBarrasV2")]
    public class CodBarrasV2 : Activity, IPermissionListener
    {
        private ZXingScannerView scannerView;
        private TextView textResult;
        private bool PopUp = false;
        private Button btnFlash;
        private bool flash = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CodBarrasV2);

            //Init View
            textResult = FindViewById<TextView>(Resource.Id.txt_result);
            scannerView = FindViewById<ZXingScannerView>(Resource.Id.zxscan);
            btnFlash = FindViewById<Button>(Resource.Id.btnFlash);
            btnFlash.Text = "Flash - Desligado";
            InitViews();

            //Request Permission
            Dexter.WithActivity(this)
                .WithPermission(Manifest.Permission.Camera)
                .WithListener(this)
                .Check();
        }

        private void InitViews()
        {
            btnFlash.Click += delegate
            {
                if (flash)
                {
                    scannerView.Flash = false;
                    flash = false;
                    btnFlash.Text = "Flash - Desligado";
                }
                else
                {
                    scannerView.Flash = true;
                    flash = true;
                    btnFlash.Text = "Flash - Ligado";
                }
            };
        }

        protected override void OnDestroy()
        {
            if (scannerView != null)
                scannerView.StopCamera();
            base.OnDestroy();
        }

        public void OnResume()
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
            scannerView.StartCamera();
        }

        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        {
            throw new NotImplementedException();
        }

        public void StartCamera()
        {
            scannerView.SetResultHandler(new MyResultHandler(this, this.scannerView));
            scannerView.StopCamera();
            scannerView.StartCamera();
        }

        public class MyResultHandler : IResultHandler
        {
            public CodBarrasV2 codBarrasV2;
            private TextView textResult;
            private ZXingScannerView scannerView;

            public MyResultHandler(CodBarrasV2 codBarrasV2, ZXingScannerView scannerView)
            {
                this.scannerView = scannerView;
                this.codBarrasV2 = codBarrasV2;
            }

            public void HandleResult(ZXing.Result rawResult)
            {
                if (codBarrasV2.PopUp == false)
                {
                    codBarrasV2.PopUp = true;

                    //Aciona o beep
                    MediaPlayer mp;
                    mp = MediaPlayer.Create(codBarrasV2.ApplicationContext, Resource.Raw.beep);// the song is a filename which i have pasted inside a folder **raw** created under the **res** folder.//
                    mp.Start();

                    AlertDialog alertDialog = new AlertDialog.Builder(this.codBarrasV2).Create();
                    alertDialog.SetTitle("Código" + rawResult.BarcodeFormat);
                    alertDialog.SetMessage(rawResult.BarcodeFormat + ": " + rawResult.Text);
                    alertDialog.SetButton("Ok", delegate
                    {
                        alertDialog.Dismiss();
                        this.codBarrasV2.StartCamera();
                    });

                    alertDialog.Show();
                    codBarrasV2.textResult.Text = (rawResult.BarcodeFormat + ": " + rawResult.Text);
                    codBarrasV2.PopUp = false;
                }
            }

        }
    }
}