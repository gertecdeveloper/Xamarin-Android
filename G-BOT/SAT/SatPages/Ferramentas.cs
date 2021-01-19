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
using Gbot_XamarinAndroid.SAT.ServiceSAT;

namespace Gbot_XamarinAndroid.SAT.SatPages
{
    [Activity(Label = "Ferramentas")]
    public class Ferramentas : Activity
    {
        private Button btnDesbloquear, btnBloquear, btnLog, btnAtualizar, btnVersao;
        private EditText txtCodAtivacao;
        SatFunctions satFunctions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ferramentas);
            satFunctions = new SatFunctions(this);

            btnBloquear = FindViewById<Button>(Resource.Id.buttonBloquear);
            btnDesbloquear = FindViewById<Button>(Resource.Id.buttonDesbloquear);
            btnLog = FindViewById<Button>(Resource.Id.buttonLog);
            btnAtualizar = FindViewById<Button>(Resource.Id.buttonAtualizar);
            btnVersao = FindViewById<Button>(Resource.Id.buttonVersao);
            txtCodAtivacao = FindViewById<EditText>(Resource.Id.txtCodAtivacao);

            txtCodAtivacao.Text = GlobalValues.codigoAtivacao;

            btnBloquear.Click += delegate
            {

                if (!SatUtils.VerificaCodigoAtivacao(txtCodAtivacao.Text))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                RepostaSat("BloquearSat");

            };

            btnDesbloquear.Click += delegate
            {

                if (!SatUtils.VerificaCodigoAtivacao(txtCodAtivacao.Text))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                RepostaSat("DesbloquearSat");

            };

            btnLog.Click += delegate
            {

                if (!SatUtils.VerificaCodigoAtivacao(txtCodAtivacao.Text))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }


                try
                {
                    RepostaSat("ExtrairLog");
                }
                catch (Exception e)
                {
                    SatUtils.MostrarToast(this, e.Message);
                }


            };

            btnAtualizar.Click += delegate
            {

                if (!SatUtils.VerificaCodigoAtivacao(txtCodAtivacao.Text))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                RepostaSat("AtualizarSoftware");

            };

            btnVersao.Click += delegate
            {

                RepostaSat("Versao");

            };
        }

        public void RepostaSat(String operacao)
        {
            String retornoOperaca = "";
            switch (operacao)
            {
                case "BloquearSat":
                    retornoOperaca = satFunctions.BloquearSat(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "DesbloquearSat":
                    retornoOperaca = satFunctions.DesbloquearSat(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "ExtrairLog":
                    retornoOperaca = satFunctions.ExtrairLog(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "AtualizarSoftware":
                    retornoOperaca = satFunctions.AtualizarSoftware(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "Versao":
                    retornoOperaca = satFunctions.Versao();
                    break;
                default:
                    retornoOperaca = "";
                    break;
            }


            GlobalValues.codigoAtivacao = txtCodAtivacao.Text;
            RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat(operacao, retornoOperaca);

            //* Está função [OperacaoSat.formataRetornoSat] recebe como parâmetro a operação realizada e um objeto do tipo RetornoSat
            //* Retorna uma String com os valores obtidos do retorno da Operação já formatados e prontos para serem exibidos na tela
            // Recomenda-se acessar a função e entender como ela funciona
            string retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);
            SatUtils.DialogoRetorno(this, retornoFormatado);
        }
    }
}