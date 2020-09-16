using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using GertecXamarinAndroid.ExemploSAT.SatPages;
using GertecXamarinAndroid.Services;

namespace GertecXamarinAndroid
{
    [Activity(Label = "GertecXamarinAndroid", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
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
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            lv = FindViewById<ListView>(Resource.Id.lvProjetos);
            adapter = new ProjetoAdapter(this,GetProjetos());
            lv.Adapter = adapter;
            lv.ItemClick += Lv_ItemClick;
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = projetos[e.Position].Name;
            switch (select){
                case "Código de Barras":
                    GoToActivity(typeof(CodBarras));
                    break;
                case "Código de Barras V2":
                    GoToActivity(typeof(CodBarrasV2));
                    break;
                case "Impressão":
                    GoToActivity(typeof(Impressora));
                    break;
                case "NFC Leitura/Gravação":
                    GoToActivity(typeof(Nfc));
                    break;
                case "SAT":
                    GoToActivity(typeof(MenuSat));
                    break;
            }
        }

        public void GoToActivity(Type myActivity)
        {
            StartActivity(myActivity);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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

            proj = new Projeto("NFC Leitura/Gravação", Resource.Drawable.nfc2);
            projetos.Add(proj);

            proj = new Projeto("SAT", Resource.Drawable.icon_sat);
            projetos.Add(proj);

            return projetos;
        }

    }
}