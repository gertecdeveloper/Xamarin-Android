using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gbot_XamarinAndroid.Sensor
{
    [Activity(Label = "Sensor")]
    public class SensorPresenca : Activity
    {
        Boolean SensorStart = false;

        EditText text;

        ListView list_resposta;
        List<string> items;
        ArrayAdapter<string> adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sensor);
            // Create your application here

            list_resposta = FindViewById<ListView>(Resource.Id.lvSensor);
            items = new List<string>();
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
            list_resposta.Adapter = adapter;

            //text = FindViewById<EditText>(Resource.Id.editText1);
            //text.Text = "Teste aqui";

            var btnStart = FindViewById<Button>(Resource.Id.btnStart);
            var btnStopSensor = FindViewById<Button>(Resource.Id.btnStopSensor);

            btnStart.Click += delegate
            {
                SensorStart = true;
            };

            btnStopSensor.Click += delegate
            {
                SensorStart = false;
            };
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {           

            if (e.KeyCode == Keycode.F4 && SensorStart)
            {
                string result = DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss") + " - Pessoa identificada à frente do equipamento.\n";
                adapter.Insert(result, 0);
                adapter.NotifyDataSetChanged();
                
            }

            return base.OnKeyDown(keyCode, e);
        }
      

    }
}