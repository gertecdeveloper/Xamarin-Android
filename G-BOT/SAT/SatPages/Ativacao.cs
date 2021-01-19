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
    [Activity(Label = "Ativacao")]
    public class Ativacao : Activity
    {
        private EditText txtCodAtivacao, txtconfCodAtivacao, txtCNPJContribuinte;
        private Button btnAtivarSat;

        SatFunctions satLib;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ativacao);

            satLib = new SatFunctions(this);

            txtCNPJContribuinte = FindViewById<EditText>(Resource.Id.txtCNPJ);
            txtCNPJContribuinte.Text = "03.654.119/0001-76";
            txtCNPJContribuinte.AddTextChangedListener(new Mask(txtCNPJContribuinte, "##.###.###/####-##"));
            txtCodAtivacao = FindViewById<EditText>(Resource.Id.txtCodAtivacao);
            txtconfCodAtivacao = FindViewById<EditText>(Resource.Id.txtConfCodAtivacao);
            btnAtivarSat = FindViewById<Button>(Resource.Id.buttonAtivar);
            txtCodAtivacao.Text = GlobalValues.codigoAtivacao;

            btnAtivarSat.Click += delegate
            {

                var codigoAtivacao = txtCodAtivacao.Text;
                var codigoAtivacaoConfirmacao = txtconfCodAtivacao.Text;

                if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                if (codigoAtivacao != codigoAtivacaoConfirmacao)
                {
                    SatUtils.MostrarToast(this, "O Código de Ativação e a Confirmação do Código de Ativação não correspondem!");
                    return;
                }

                GlobalValues.codigoAtivacao = codigoAtivacao;
                string resp = satLib.AtivarSat(txtCodAtivacao.Text.ToString(),
                                                SatUtils.SomenteNumeros(txtCNPJContribuinte.Text),
                                                SatUtils.GerarNumeroSessao);

                RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat("AtivarSAT", resp);

                //* Está função [OperacaoSat.formataRetornoSat] recebe como parâmetro a operação realizada e um objeto do tipo RetornoSat
                //* Retorna uma String com os valores obtidos do retorno da Operação já formatados e prontos para serem exibidos na tela
                // Recomenda-se acessar a função e entender como ela funciona
                string retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);



                SatUtils.DialogoRetorno(this, retornoFormatado);

            
            };
            
        }
    }
}