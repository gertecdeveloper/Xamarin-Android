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
using Gbot_XamarinAndroid.SAT.SatPages;

namespace Gbot_XamarinAndroid.SAT
{
    [Activity(Label = "MenuSat")]
    public class MenuSat : Activity
    {
        private Button btnAtivacao, btnAssinatura, btnTesteSat, btnConfigRede, btnAlterarCodAtivacao, btnFuncoes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menuSat);
            // Create your application here
            InitButtons();
        }

        private void InitButtons()
        {
            btnAtivacao = FindViewById<Button>(Resource.Id.buttonAtivar2);
            btnAssinatura = FindViewById<Button>(Resource.Id.buttonAssociar2);
            btnTesteSat = FindViewById<Button>(Resource.Id.buttonTeste);
            btnConfigRede = FindViewById<Button>(Resource.Id.buttonRede);
            btnAlterarCodAtivacao = FindViewById<Button>(Resource.Id.buttonAlterar2);
            btnFuncoes = FindViewById<Button>(Resource.Id.buttonOutros);

            btnAtivacao.Click += delegate
            {

                GoToActivity(typeof(Ativacao));
            };

            btnAlterarCodAtivacao.Click += delegate
            {
                GoToActivity(typeof(Alterar));
            };

            btnAssinatura.Click += delegate
            {
                GoToActivity(typeof(Associar));
            };

            btnConfigRede.Click += delegate
            {
                GoToActivity(typeof(Rede));
            };

            btnTesteSat.Click += delegate
            {
                GoToActivity(typeof(Teste));
            };

            btnFuncoes.Click += delegate
            {
                GoToActivity(typeof(Ferramentas));
            };

        }

        public void GoToActivity(Type myActivity)
        {
            StartActivity(myActivity);
        }
    }
}