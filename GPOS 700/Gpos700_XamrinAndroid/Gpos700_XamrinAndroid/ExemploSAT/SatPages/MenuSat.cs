using System;

using Android.App;
using Android.OS;
using Android.Widget;

namespace Gpos700_XamrinAndroid.ExemploSAT.SatPages
{
    [Activity(Label = "MenuSat")]
    public class MenuSat : Activity
    {
        private Button btnAtivacao, btnAssinatura, btnTesteSat, btnConfigRede, btnAlterarCodAtivacao, btnFuncoes;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu_sat);

            InitButtons();

        }

        private void InitButtons()
        {
            btnAtivacao = FindViewById<Button>(Resource.Id.btnAtivacaoSat);
            btnAssinatura = FindViewById<Button>(Resource.Id.btnAssociarAssinaturaSat);
            btnTesteSat = FindViewById<Button>(Resource.Id.btnTesteSat);
            btnConfigRede = FindViewById<Button>(Resource.Id.btnConfigRedeSat);
            btnAlterarCodAtivacao = FindViewById<Button>(Resource.Id.btnAlterarCodSat);
            btnFuncoes = FindViewById<Button>(Resource.Id.btnOutrasFuncoesSat);

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