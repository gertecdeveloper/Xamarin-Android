using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using System;
using Gpos700_XamrinAndroid.Services;
using Gpos700_XamrinAndroid.TEF;

namespace Gpos700_XamrinAndroid
{
    [Activity(Label = "GertecOne XamarinAndroid", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Context context;
        private ListView lv;
        private ProjetoAdapter adapter;
        private JavaList<Projeto> projetos;

        public MainActivity()
        {
            this.context = Android.App.Application.Context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            lv = FindViewById<ListView>(Resource.Id.lvProjetos);
            adapter = new ProjetoAdapter(this, GetProjetos());
            lv.Adapter = adapter;
            lv.ItemClick += Lv_ItemClick;

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = projetos[e.Position].Name;
            Console.WriteLine(select);
            switch (select)
            {
                case "Código de Barras":
                    //Console.WriteLine("Código de Barras");
                    GoToActivity(typeof(CodBarras));
                    break;
                case "Código de Barras V2":
                    //Console.WriteLine("Código de Barras V2");
                    GoToActivity(typeof(CodBarrasV2));
                    break;
                case "Impressão":
                    //Console.WriteLine("Impressão");
                    GoToActivity(typeof(Impressora));
                    break;
                case "NFC Gedi":
                    //Console.WriteLine("NFC Gedi");
                    GoToActivity(typeof(NfcGedi));
                    break;
                case "NFC Id":
                    //Console.WriteLine("NFC Id");
                    GoToActivity(typeof(NfcID));
                    break;
                case "TEF":
                    //Console.WriteLine("TEF");
                    GoToActivity(typeof(Tef));
                    break;
            }
        }

        public void GoToActivity(Type myActivity)
        {
            StartActivity(myActivity);
        }

        private JavaList<Projeto> GetProjetos()
        {
            projetos = new JavaList<Projeto>();
            Projeto proj;

            proj = new Projeto("Código de Barras", Resource.Drawable.barcode);
            projetos.Add(proj);

            proj = new Projeto("Código de Barras V2", Resource.Drawable.qr_code);
            projetos.Add(proj);

            proj = new Projeto("Impressão", Resource.Drawable.print);
            projetos.Add(proj);

            proj = new Projeto("NFC Gedi", Resource.Drawable.nfc);
            projetos.Add(proj);

            proj = new Projeto("NFC Id", Resource.Drawable.nfc1);
            projetos.Add(proj);

            proj = new Projeto("TEF", Resource.Drawable.pos);
            projetos.Add(proj);

            return projetos;
        }
    }
}