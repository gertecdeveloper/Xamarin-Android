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
    [Activity(Label = "Associar")]
    public class Associar : Activity
    {
        private Button btnAssociar;
        private EditText txtCodAtivacao, txtAssinatura, txtCnpjContribuinte, txtCnpjSH;
        SatFunctions satLib;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.assinatura);
            satLib = new SatFunctions(this);

            btnAssociar = FindViewById<Button>(Resource.Id.btnAssociar);

            txtCodAtivacao = FindViewById<EditText>(Resource.Id.txtCodAtivacao);
            txtAssinatura = FindViewById<EditText>(Resource.Id.txtAssinatura);
            txtCnpjContribuinte = FindViewById<EditText>(Resource.Id.txtCNPJ);
            txtCnpjContribuinte.AddTextChangedListener(new Mask(txtCnpjContribuinte, "##.###.###/####-##"));
            txtCnpjSH = FindViewById<EditText>(Resource.Id.txtSW);
            txtCnpjSH.AddTextChangedListener(new Mask(txtCnpjSH, "##.###.###/####-##"));
            txtCodAtivacao.Text = GlobalValues.codigoAtivacao;

            txtCnpjContribuinte.Text = "03.654.119/0001-76";
            txtCnpjSH.Text = "16.716.114/0001-72";
            txtAssinatura.Text = "SGR-SAT SISTEMA DE GESTAO E RETAGUARDA DO SAT";

            btnAssociar.Click += delegate
            {
                var codigoAtivacao = txtCodAtivacao.Text;
                var assinatura = txtAssinatura.Text;

                if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                if (string.IsNullOrEmpty(assinatura))
                {
                    SatUtils.MostrarToast(this, "Assinatura AC Inválida!");
                    return;
                }

                string resp = satLib.AssociarSat(
                    SatUtils.SomenteNumeros(txtCnpjContribuinte.Text),
                    SatUtils.SomenteNumeros(txtCnpjSH.Text),
                    codigoAtivacao,
                    assinatura,
                    SatUtils.GerarNumeroSessao);

                RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat("AssociarSAT", resp);

                //* Está função [OperacaoSat.formataRetornoSat] recebe como parâmetro a operação realizada e um objeto do tipo RetornoSat
                //* Retorna uma String com os valores obtidos do retorno da Operação já formatados e prontos para serem exibidos na tela
                // Recomenda-se acessar a função e entender como ela funciona
                string retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);


                SatUtils.DialogoRetorno(this, retornoFormatado);
                GlobalValues.codigoAtivacao = codigoAtivacao;


            };
            
        }
    }
}