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
    [Activity(Label = "Rede")]
    public class Rede : Activity
    {
        private Button btnConfigurar;
        private EditText txtCodAtivacao, txtIp, txtMascara, txtGateway, txtDns, txtDns2, txtProxyIp, txtPorta, txtUser, txtPassword;
        private Spinner spinnerTipoRede, spinnerOptDns, spinnerOptProxy;
        private AlertDialog alerta;
        private static string[] tipoRede = new string[] { "Estático", "DHCP" };
        private static string[] optDns = new string[] { "Não", "Sim" };
        private static string[] optProxy = new string[] { "Não usa proxy", "Proxy com configuração", "Proxy transparente" };
        private static string appPackage = "com.companyname.gpos700_xamrinandroid";
        private static string caminhoXML = "data/data/" + appPackage + "/configRede.xml";
        Random gerador = new Random();
        SatFunctions satFunctions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.rede);
            satFunctions = new SatFunctions(this);

            btnConfigurar = FindViewById<Button>(Resource.Id.buttonEnviar);
            txtCodAtivacao = FindViewById<EditText>(Resource.Id.txtCodAtivacao);
            txtIp = FindViewById<EditText>(Resource.Id.txtIp);
            txtMascara = FindViewById<EditText>(Resource.Id.txtMascara);
            txtGateway = FindViewById<EditText>(Resource.Id.txtGateway);
            txtDns = FindViewById<EditText>(Resource.Id.txtDns);
            txtDns2 = FindViewById<EditText>(Resource.Id.txtDns2);
            txtProxyIp = FindViewById<EditText>(Resource.Id.txtProxyIp);
            txtPorta = FindViewById<EditText>(Resource.Id.txtPorta);
            txtUser = FindViewById<EditText>(Resource.Id.txtUser);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            spinnerTipoRede = FindViewById<Spinner>(Resource.Id.tipoRede);
            spinnerOptDns = FindViewById<Spinner>(Resource.Id.optDns);
            spinnerOptProxy = FindViewById<Spinner>(Resource.Id.optProxy);
            txtCodAtivacao.Text = GlobalValues.codigoAtivacao;


            ArrayAdapter adp = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, tipoRede);
            adp.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
            spinnerTipoRede.Adapter = adp;
            spinnerTipoRede.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerTipoRede_ItemSelected);

            ArrayAdapter adp2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, optDns);
            adp2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
            spinnerOptDns.Adapter = adp2;
            spinnerOptDns.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerOptDns_ItemSelected);

            ArrayAdapter adp3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, optProxy);
            adp3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
            spinnerOptProxy.Adapter = adp3;
            spinnerOptProxy.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerOptProxy_ItemSelected);

            btnConfigurar.Click += delegate
            {

                try
                {
                    var codigoAtivacao = txtCodAtivacao.Text;

                    if (!SatUtils.VerificaCodigoAtivacao(codigoAtivacao))
                    {
                        SatUtils.MostrarToast(this, "Código de Ativação deve ter entre 8 a 32 caracteres!");
                        return;
                    }

                    ConfiguracaoRede config = new ConfiguracaoRede();

                    // Monta as tags do XML sobre a parte de REDE
                    if (spinnerTipoRede.SelectedItem.Equals("Estático"))
                    {
                        config.tipoLan = "IPFIX";


                        if (!string.IsNullOrEmpty(txtIp.Text))
                            config.lanIP = txtIp.Text;

                        if (!string.IsNullOrEmpty(txtMascara.Text))
                            config.lanMask = txtMascara.Text;

                        if (!string.IsNullOrEmpty(txtGateway.Text))
                            config.lanGW = txtGateway.Text;

                        // Monta as tags do XML sobre a parte de DNS
                        if (spinnerOptDns.SelectedItem.Equals("Sim"))
                        {
                            if (!string.IsNullOrEmpty(txtDns.Text))
                                config.lanDNS1 = txtDns.Text;

                            if (!string.IsNullOrEmpty(txtDns2.Text))
                                config.lanDNS2 = txtDns2.Text;
                        }
                        else
                        {
                            config.lanDNS1 = "8.8.8.8";
                            config.lanDNS2 = "4.4.4.4";
                        }
                    }
                    else
                    {
                        config.tipoLan = "DHCP";
                    }


                    // Monta as tags do XML sobre a parte de PROXY
                    if (spinnerOptProxy.SelectedItem.Equals("Não usa proxy"))
                        config.proxy = "0";

                    else if (spinnerOptProxy.SelectedItem.Equals("Proxy com configuração"))
                    {
                        config.proxy = "1";

                        if (!string.IsNullOrEmpty(txtProxyIp.Text))
                            config.proxy_ip = txtProxyIp.Text;

                        if (!string.IsNullOrEmpty(txtPorta.Text))
                            config.proxy_porta = txtPorta.Text;

                        if (!string.IsNullOrEmpty(txtUser.Text))
                            config.proxy_user = txtUser.Text;

                        if (!string.IsNullOrEmpty(txtPassword.Text))
                            config.proxy_senha = txtPassword.Text;

                    }
                    else
                    {
                        config.proxy = "2";
                        if (!string.IsNullOrEmpty(txtProxyIp.Text))
                            config.proxy_ip = txtProxyIp.Text;

                        if (!string.IsNullOrEmpty(txtPorta.Text))
                            config.proxy_porta = txtPorta.Text;

                        if (!string.IsNullOrEmpty(txtUser.Text))
                            config.proxy_user = txtUser.Text;

                        if (!string.IsNullOrEmpty(txtPassword.Text))
                            config.proxy_senha = txtPassword.Text;

                    }

                    string resp = satFunctions.EnviarConfRede(SatUtils.GerarNumeroSessao, config.GerarXml(), codigoAtivacao);
                    RetornoSat retornoSat = OperacaoSat.invocarOperacaoSat("EnviarConfRede", resp);

                    //* Está função [OperacaoSat.formataRetornoSat] recebe como parâmetro a operação realizada e um objeto do tipo RetornoSat
                    //* Retorna uma String com os valores obtidos do retorno da Operação já formatados e prontos para serem exibidos na tela
                    // Recomenda-se acessar a função e entender como ela funciona
                    GlobalValues.codigoAtivacao = codigoAtivacao;
                    String retornoFormatado = OperacaoSat.formataRetornoSat(retornoSat);

                    SatUtils.DialogoRetorno(this, retornoFormatado);

                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message.ToString(), ToastLength.Long).Show();
                }
            };

        }

        private void spinnerOptProxy_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            if (spinnerOptProxy.SelectedItem.Equals("Não usa proxy"))
            {
                txtProxyIp.Enabled = false;
                txtPorta.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtProxyIp.Enabled = true;
                txtPorta.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void spinnerOptDns_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            if (spinnerOptDns.SelectedItem.Equals("Não"))
            {
                txtDns.Enabled = false;
                txtDns2.Enabled = false;
            }
            else
            {
                txtDns.Enabled = true;
                txtDns2.Enabled = true;
            }
        }

        private void spinnerTipoRede_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            if (spinnerTipoRede.SelectedItem.Equals("Estático"))
            {
                txtIp.Enabled = true;
                txtMascara.Enabled = true;
                txtGateway.Enabled = true;
            }
            else
            {
                txtIp.Enabled = false;
                txtMascara.Enabled = false;
                txtGateway.Enabled = false;
            }
        }
    }
}