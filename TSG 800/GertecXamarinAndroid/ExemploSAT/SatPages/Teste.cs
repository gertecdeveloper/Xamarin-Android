using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Widget;
using GertecXamarinAndroid.ExemploSAT.ServiceSAT;
using Java.Lang;

namespace GertecXamarinAndroid.ExemploSAT.SatPages
{
    [Activity(Label = "Teste")]
    public class Teste : Activity
    {
        public static string ultimaChaveVenda;
        private Button btnConsultar;
        private Button btnStatus;
        private Button btnTeste;
        private Button btnVendas;
        private Button btnCancelamento;
        private Button btnSessao;
        private EditText txtCodAtivacao;
        private AlertDialog alerta;
        SatFunctions satFunctions;
        private string codigoSessao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.teste);
            satFunctions = new SatFunctions(this);

            btnConsultar = FindViewById<Button>(Resource.Id.btnConsultarSat);
            btnStatus = FindViewById<Button>(Resource.Id.buttonStatus);
            btnTeste = FindViewById<Button>(Resource.Id.buttonFimaFim);
            btnVendas = FindViewById<Button>(Resource.Id.buttonTesteVenda);
            btnCancelamento = FindViewById<Button>(Resource.Id.buttonCancelamento);
            btnSessao = FindViewById<Button>(Resource.Id.buttonConsultarSessao);
            txtCodAtivacao = FindViewById<EditText>(Resource.Id.txtCodAtivacao);
            txtCodAtivacao.Text = GlobalValues.codigoAtivacao;

            InitFuncoes();
        }

        public void InitFuncoes()
        {
            btnConsultar.Click += delegate
            {
                repostaSat("ConsultarSat");
            };

            btnStatus.Click += delegate
            {
                var codigoAtivacao = txtCodAtivacao.Text;
                if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                repostaSat("ConsultarStatusOperacional");
            };

            btnTeste.Click += delegate
            {

                var codigoAtivacao = txtCodAtivacao.Text;

                if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                repostaSat("EnviarTesteFim");
            };

            btnVendas.Click += delegate
            {
                var codigoAtivacao = txtCodAtivacao.Text;
                if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                {
                    SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                    return;
                }

                repostaSat("EnviarTesteVendas");
               

            };

            btnCancelamento.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Atenção");
                builder.SetMessage("Digite a chave de cancelamento");

                EditText inputChaveCancelamento = new EditText(this);
                inputChaveCancelamento.Text = GlobalValues.ultimaChaveVenda;
                builder.SetView(inputChaveCancelamento);
                builder.SetNeutralButton("OK", OkAction);

                void OkAction(object sender, DialogClickEventArgs e)
                {
                    ultimaChaveVenda = inputChaveCancelamento.Text;
                    var codigoAtivacao = txtCodAtivacao.Text;

                    if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                    {
                        SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                        return;
                    }

                    if (string.IsNullOrEmpty(ultimaChaveVenda))
                    {
                        SatUtils.MostrarToast(this, "Digite uma chave de cancelamento!");
                        return;
                    }
                    
                    repostaSat("CancelarUltimaVenda");
                }

                alerta = builder.Create();
                alerta.Show();
            };

            btnSessao.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Atenção");
                builder.SetMessage("Digite o número da sessão");
                EditText inputCodigoSessao = new EditText(this);
                inputCodigoSessao.SetRawInputType(InputTypes.ClassNumber);
                builder.SetView(inputCodigoSessao);
                builder.SetNeutralButton("OK", OkAction);
                inputCodigoSessao.Text = GlobalValues.ultimaSessao;

                void OkAction(object sender, DialogClickEventArgs e)
                {
                    codigoSessao = inputCodigoSessao.Text.Trim();
                    var codigoAtivacao = txtCodAtivacao.Text;

                    if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                    {
                        SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                        return;
                    }

                    if (string.IsNullOrEmpty(codigoSessao))
                    {
                        SatUtils.MostrarToast(this, "Digite um número de sessão!");
                        return;
                    }

                    repostaSat("ConsultarNumeroSessao");
                }
                alerta = builder.Create();
                alerta.Show();
            };
        }

        public void repostaSat(string operacao)
        {
            string retornoOperaca = "";
            switch (operacao)
            {
                case "ConsultarSat":
                    retornoOperaca = satFunctions.ConsultarSat(SatUtils.GerarNumeroSessao);
                    break;
                case "ConsultarStatusOperacional":
                    retornoOperaca = satFunctions.ConsultarStatusOperacional(SatUtils.GerarNumeroSessao, txtCodAtivacao.Text);
                    break;
                case "EnviarTesteFim":
                    retornoOperaca = satFunctions.EnviarTesteFim(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "EnviarTesteVendas":
                    retornoOperaca = satFunctions.EnviarTesteVendas(txtCodAtivacao.Text, SatUtils.GerarNumeroSessao);
                    break;
                case "CancelarUltimaVenda":
                    retornoOperaca = satFunctions.CancelarUltimaVenda(txtCodAtivacao.Text, ultimaChaveVenda, SatUtils.GerarNumeroSessao);
                    break;
                case "ConsultarNumeroSessao":
                    retornoOperaca = satFunctions.ConsultarNumeroSessao(txtCodAtivacao.Text, Integer.ParseInt(codigoSessao), SatUtils.GerarNumeroSessao);
                    break;
                default:
                    retornoOperaca = "";
                    break;
            }

           
            RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat(operacao, retornoOperaca);
            /*
             * Está verificação(abaixo) tem como objetivo capturar a "Chave de Consulta" retornado na operação EnviarTesteVendas
             * O valor é armazenado em uma variavel global e quando o usuario abre a tela para cancelar venda, o campo (Chave de Cancelamento) já fica preenchido
             */
            if (operacao == "EnviarTesteVendas")
            {
                //chave ultima venda
                GlobalValues.ultimaChaveVenda = retornoSat.ChaveConsulta;
            }

            GlobalValues.codigoAtivacao = txtCodAtivacao.Text;
            //* Está função [OperacaoSat.formataRetornoSat] recebe como parâmetro a operação realizada e um objeto do tipo RetornoSat
            //* Retorna uma String com os valores obtidos do retorno da Operação já formatados e prontos para serem exibidos na tela
            // Recomenda-se acessar a função e entender como ela funciona
            string retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);
            SatUtils.DialogoRetorno(this, retornoFormatado);
        }


        }
}
