using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Gpos700_XamrinAndroid.ExemploSAT.ServiceSAT;
using Java.IO;

namespace Gpos700_XamrinAndroid.ExemploSAT.SatPages
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

            btnBloquear = FindViewById<Button>(Resource.Id.btnBloquear);
            btnDesbloquear = FindViewById<Button>(Resource.Id.btnDesbloquear);
            btnLog = FindViewById<Button>(Resource.Id.btnLog);
            btnAtualizar = FindViewById<Button>(Resource.Id.btnAtualizar);
            btnVersao = FindViewById<Button>(Resource.Id.btnVersao);
            txtCodAtivacao = FindViewById<EditText>(Resource.Id.txtCodAtivacao);

            txtCodAtivacao.Text = GlobalValues.codigoAtivacao;

            btnBloquear.Click += delegate
            {
                
                if (!SatUtils.VerificaCodigoAtivacao(txtCodAtivacao.Text))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                RespostaSat("BloquearSat");
                
            };

            btnDesbloquear.Click += delegate
            {
                
                if (!SatUtils.VerificaCodigoAtivacao(txtCodAtivacao.Text))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                RespostaSat("DesbloquearSat");

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
                    RespostaSat("ExtrairLog");
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

                RespostaSat("AtualizarSoftware");

            };

            btnVersao.Click += delegate
            {

                RespostaSat("Versao");

            };
        }

        public void RespostaSat(String operacao)
        {
            String retornoOperacao = "";
            switch (operacao)
            {
                case "BloquearSat":
                    retornoOperacao = satFunctions.BloquearSat(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "DesbloquearSat":
                    retornoOperacao = satFunctions.DesbloquearSat(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "ExtrairLog":
                    retornoOperacao = satFunctions.ExtrairLog(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "AtualizarSoftware":
                    retornoOperacao = satFunctions.AtualizarSoftware(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "Versao":
                    retornoOperacao = satFunctions.Versao();
                    break;
                default:
                    retornoOperacao = "";
                    break;
            }


            GlobalValues.codigoAtivacao = txtCodAtivacao.Text;
            RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat(operacao, retornoOperacao);

            //* Está função [OperacaoSat.formataRetornoSat] recebe como parâmetro a operação realizada e um objeto do tipo RetornoSat
            //* Retorna uma String com os valores obtidos do retorno da Operação já formatados e prontos para serem exibidos na tela
            // Recomenda-se acessar a função e entender como ela funciona
            string retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);
            SatUtils.DialogoRetorno(this, retornoFormatado);
        }




    }
}