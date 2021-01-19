using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Gbot_XamarinAndroid.SAT;
using System;
using Gbot_XamarinAndroid.NFC;
using Gbot_XamarinAndroid.TEF;
using Gbot_XamarinAndroid.Pages;
using Gbot_XamarinAndroid.Sensor;
using Gbot_XamarinAndroid.GbotFala;
using Android.Hardware.Usb;

namespace Gbot_XamarinAndroid
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/Imagem1", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true),
        MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter"),
        IntentFilter(new[] { "android.hardware.usb.action.USB_DEVICE_ATTACHED", "android.intent.action.MAIN" },
    Categories = new[] { "android.intent.category.LAUNCHER" })]
    public class MainActivity : AppCompatActivity
    {
        public static string Model = Build.Model;
        public static string PLATAFORMA = "Xamarin Android";
        public const string VERSION = "1.0.0";

        public const string GLASS_MODEL = "A112";

        public TextView txtProject;

        public static string getVersion()
        {
            return PLATAFORMA + " - " + VERSION + " - " + Model;
        }

        
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
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            lv = FindViewById<ListView>(Resource.Id.lvProjetos);
            adapter = new ProjetoAdapter(this, GetProjetos());
            lv.Adapter = adapter;
            lv.ItemClick += Lv_ItemClick;

            txtProject = FindViewById<TextView>(Resource.Id.txtNameProject);
            txtProject.Text = getVersion();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = projetos[e.Position].Name;
            switch (select)
            {
                case "Código de Barras":
                    GoToActivity(typeof(CodigoDeBarras));
                    break;
                case "NFC - NDEF":
                    GoToActivity(typeof(MenuNFC));
                    break;
                case "FALA G-Bot":
                    GoToActivity(typeof(FalaGbot));
                    break;
                case "Modo Quiosque":
                    GoToActivity(typeof(KioskMode.KioskMode));
                    break;
                case "Sensor de Presença":
                    GoToActivity(typeof(SensorPresenca));
                    break;
                case "Tef":
                    GoToActivity(typeof(Tef));
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

        private JavaList<Projeto> GetProjetos()
        {
            projetos = new JavaList<Projeto>();
            Projeto proj;

            proj = new Projeto("Código de Barras", Resource.Drawable.barcode);
            projetos.Add(proj);

            proj = new Projeto("NFC - NDEF", Resource.Drawable.nfc2);
            projetos.Add(proj);

            proj = new Projeto("Sensor de Presença", Resource.Drawable.sensor);
            projetos.Add(proj);

            proj = new Projeto("FALA G-Bot", Resource.Drawable.speaker);
            projetos.Add(proj);

            proj = new Projeto("Modo Quiosque", Resource.Drawable.kiosk);
            projetos.Add(proj);

            proj = new Projeto("Tef", Resource.Drawable.pos);
            projetos.Add(proj);

            proj = new Projeto("SAT", Resource.Drawable.icon_sat);
            projetos.Add(proj);

            

            return projetos;
        }
    }
}