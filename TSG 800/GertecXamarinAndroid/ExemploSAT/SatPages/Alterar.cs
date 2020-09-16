using System;

using Android.App;
using Android.OS;
using Android.Widget;
using GertecXamarinAndroid.ExemploSAT.ServiceSAT;

namespace GertecXamarinAndroid.ExemploSAT.SatPages
{
    [Activity(Label = "Alterar")]
    public class Alterar : Activity
    {
        private Button btnAlterar;
        private EditText txtCodAtual, txtCodNovo, txtCodConfirmar;
        private Spinner spinner;
        ArrayAdapter<String> adapter = null;
        private AlertDialog alerta;
        private int opcao = 1;
        String[] optCod = new String[] { "Código de ativação", "Código de Emergência" };
        SatFunctions satFunctions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.alterar);
            satFunctions = new SatFunctions(this);

            txtCodAtual = FindViewById<EditText>(Resource.Id.txtCodAtual);
            txtCodConfirmar = FindViewById<EditText>(Resource.Id.txtCodConfirmar);
            txtCodNovo = FindViewById<EditText>(Resource.Id.txtCodNovo);

            spinner = FindViewById<Spinner>(Resource.Id.spinner1);

            txtCodAtual.Text = GlobalValues.codigoAtivacao;

            btnAlterar = FindViewById<Button>(Resource.Id.btnAlterar);

            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, optCod);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = (adapter);

            btnAlterar.Click += delegate
            {
                try
                {
                    var codigoAtivacaoAtual = txtCodAtual.Text;
                    var codigoAtivacaoNovo = txtCodNovo.Text;
                    var codigoAtivacaoNovoConfirmacao = txtCodConfirmar.Text;

                    if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacaoAtual) || !SatUtils.VerificaCodigoAtivacao(codigoAtivacaoNovo) || !SatUtils.VerificaCodigoAtivacao(codigoAtivacaoNovoConfirmacao))
                    {
                        SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                        return;
                    }

                    if(codigoAtivacaoNovo != txtCodConfirmar.Text)
                    {
                        SatUtils.MostrarToast(this, "O Novo Código de Ativação e a Confirmação do Novo Código de Ativação não correspondem!");
                        return;
                    }                  

                    opcao = spinner.SelectedItem.Equals("Código de ativação") ? 1 : 2;

                    GlobalValues.codigoAtivacao = codigoAtivacaoNovo;
                    string retorno = satFunctions.TrocarCodAtivacao(codigoAtivacaoAtual, opcao, codigoAtivacaoNovo, SatUtils.GerarNumeroSessao);
                    RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat("TrocarCodAtivacao", retorno);

                    string retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);
                    SatUtils.DialogoRetorno(this, retornoFormatado);

                } catch(Exception e)
                {
                    SatUtils.MostrarToast(this, e.Message);
                }
            };
        }

    }    
}

