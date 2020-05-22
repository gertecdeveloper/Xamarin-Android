using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using BR.Com.Gertec.Gedi.Exceptions;
using GertecXamarinAndroid.Impressao;
using Plugin.DeviceInfo;

namespace GertecXamarinAndroid.Services
{
    [Activity(Label = "Impressora", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class Impressora : AppCompatActivity
    {
        string modelo;

        Context context;

        TextView txtModelo;

        EditText txtMensagem;

        RadioButton rbEsquerda, rbCentralizado, rbDireta;

        ToggleButton btnNegrito, btnItalico, btnSublinhado;

        Spinner spFonte, spTamanho;

        Spinner spCodeHeight, spCodeWidth, spTipoCode;

        Button btnStatusImpressora, btnImprimirMensagem, btnImprimirImagem, btnImpriTodasFunc, btnImprimirBarCode;

        GertecPrinter printer;
        ConfigPrint configPrint;

        private Activity mainActivity;



        public Impressora()
        {
            this.mainActivity = this;
            this.context = Android.App.Application.Context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Impressao);
            // Create your application here

            modelo = CrossDeviceInfo.Current.Model;

            // Ini Textview
            IniTextview();

            // Ini EditText
            IniEditText();

            // Ini Radio Button
            IniRadioButton();

            // Ini ToggleButton
            IniToggleButton();

            // Ini Spinner
            IniSpinner();

            // Ini Buttons
            IniButtons();

            // Ini funções bottuns
            iniFuncoesButtons();

            // Carrega todos os Spinner
            iniLoadSpinner();

            // Mostra modelo
            //iniLoadModelo();

            // Ini context
            context = ApplicationContext;
            // #if __G700__
            printer = new GertecPrinter(mainActivity);
            // #elif __G800__
            //     sdfjksdjfklsdjflks
            // #endif

            configPrint = new ConfigPrint();
            printer.setConfigImpressao(configPrint);


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void IniTextview()
        {
            txtModelo = FindViewById<TextView>(Resource.Id.txtModelo);
        }

        private void IniEditText()
        {
            txtMensagem = FindViewById<EditText>(Resource.Id.txtMensagem);
        }

        private void IniRadioButton()
        {
            rbEsquerda = FindViewById<RadioButton>(Resource.Id.rbEsquerda);
            rbCentralizado = FindViewById<RadioButton>(Resource.Id.rbCentralizado);
            rbDireta = FindViewById<RadioButton>(Resource.Id.rbDireita);
        }

        private void IniToggleButton()
        {
            btnNegrito = FindViewById<ToggleButton>(Resource.Id.btnNegrito);
            btnItalico = FindViewById<ToggleButton>(Resource.Id.btnItalico);
            btnSublinhado = FindViewById<ToggleButton>(Resource.Id.btnSublinhado);
        }

        private void IniSpinner()
        {
            spFonte = FindViewById<Spinner>(Resource.Id.spFonte);
            spTamanho = FindViewById<Spinner>(Resource.Id.spTamanho);
            spCodeHeight = FindViewById<Spinner>(Resource.Id.spCodeHeight);
            spCodeWidth = FindViewById<Spinner>(Resource.Id.spCodeWidth);
            spTipoCode = FindViewById<Spinner>(Resource.Id.spTipoCode);
        }

        private void IniButtons()
        {
            btnStatusImpressora = FindViewById<Button>(Resource.Id.btnStatusImpressora);
            btnImprimirMensagem = FindViewById<Button>(Resource.Id.btnImprimirMensagem);
            btnImprimirImagem = FindViewById<Button>(Resource.Id.btnImprimirImagem);
            btnImprimirBarCode = FindViewById<Button>(Resource.Id.btnImprimirBarCode);
            btnImpriTodasFunc = FindViewById<Button>(Resource.Id.btnImprimirTodasFunc);
        }

        /*private void iniLoadModelo()
        {
            if (modelo.Equals("Smart G800"))
            {
                txtModelo.Text = "Xamarin Impressão TSG800";
            }
            else
            {
                txtModelo.Text = "Xamarin Impressão TSG800";
            }
        }*/

        private void iniLoadSpinner()
        {
            ArrayAdapter adapter;

            // Irá mostrar todas as Fontes
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Fonts, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spFonte.Adapter = adapter;

            // Irá mostrar todos os possíveis tamanhos
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Tamanhos, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spTamanho.Adapter = adapter;

            // Irá mostrar todos os possíveis tamanhos
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Widths, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spCodeHeight.Adapter = adapter;

            // Irá mostrar todos os possíveis tamanhos
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Widths, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spCodeWidth.Adapter = adapter;

            // Irá mostrar todos os possíveis tamanhos
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.BarCodes, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spTipoCode.Adapter = adapter;
        }

        private void iniFuncoesButtons()
        {
            btnStatusImpressora.Click += BtnStatusImpressora_Click;
            btnImprimirMensagem.Click += BtnImprimirMensagem_Click;
            btnImprimirImagem.Click += BtnImprimirImagem_Click;
            btnImprimirBarCode.Click += BtnImprimirBarCode_Click;
            btnImpriTodasFunc.Click += BtnImprimirTodasFunc_Click;
        }

        protected void BtnStatusImpressora_Click(object sender, EventArgs args)
        {
            string statusImpressora;

            try
            {
                statusImpressora = printer.getStatusImpressora();
                Toast.MakeText(context, statusImpressora, ToastLength.Long).Show();
            }
            catch (Exception e)
            {
                Toast.MakeText(context, e.Message, ToastLength.Long).Show();
            }

        }

        protected void BtnImprimirMensagem_Click(object sender, EventArgs args)
        {
            string mensagem = txtMensagem.Text;
            if (String.IsNullOrWhiteSpace(mensagem))
            {
                Toast.MakeText(context, "Escreva uma mensagem", ToastLength.Long).Show();
            }
            else
            {
                try
                {

                    if (rbEsquerda.Checked)
                    {
                        configPrint.Alinhamento = "LEFT";
                    }
                    else if (rbCentralizado.Checked)
                    {
                        configPrint.Alinhamento = "CENTER";
                    }
                    else if (rbDireta.Checked)
                    {
                        configPrint.Alinhamento = "RIGHT";
                    }

                    configPrint.Negrito = btnNegrito.Checked;
                    configPrint.Italico = btnItalico.Checked;
                    configPrint.SubLinhado = btnSublinhado.Checked;

                    configPrint.Fonte = spFonte.SelectedItem.ToString();
                    configPrint.Tamanho = Int32.Parse(spTamanho.SelectedItem.ToString());

                    printer.setConfigImpressao(configPrint);

                    printer.ImprimeTexto(mensagem);

                    printer.ImpressoraOutput();
                    printer.AvancaLinha(150);


                }
                catch (GediException e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Long).Show();
                }
            }

        }

        protected void BtnImprimirImagem_Click(object sender, EventArgs args)
        {
            configPrint.IWidth = 430;
            configPrint.IHeight = 700;
            printer.setConfigImpressao(configPrint);
            printer.ImprimeImagem("invoice");
            configPrint.IWidth = 400;
            configPrint.IHeight = 150;
            printer.setConfigImpressao(configPrint);
            printer.ImprimeImagem("gertec");
            configPrint.IWidth = 300;
            configPrint.IHeight = 400;
            printer.setConfigImpressao(configPrint);
            printer.ImprimeImagem("gertecone");
            printer.ImpressoraOutput();
            printer.AvancaLinha(150);
        }

        protected void BtnImprimirBarCode_Click(object sender, EventArgs args)
        {
            if (txtMensagem.Text.Equals(""))
            {
                Toast.MakeText(ApplicationContext, "Preencha um texto", ToastLength.Long).Show();
            }
            else
            {
                printer.ImprimeBarCode(
                    txtMensagem.Text,
                    Int32.Parse(spCodeHeight.SelectedItem.ToString()),
                    Int32.Parse(spCodeWidth.SelectedItem.ToString()),
                    spTipoCode.SelectedItem.ToString());
                printer.ImpressoraOutput();
                printer.AvancaLinha(150);
            }

        }
        public void BtnImprimirTodasFunc_Click(object sender, EventArgs args)
        {
            configPrint.Italico = false;
            configPrint.Negrito = true;
            configPrint.Tamanho = 20;
            configPrint.Fonte = "MONOSPACE";
            printer.setConfigImpressao(configPrint);

            try
            {
                printer.getStatusImpressora();
                // Imprimindo imagem
                configPrint.IWidth = 300;
                configPrint.IHeight = 130;
                configPrint.Alinhamento = "CENTER";
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("==[Iniciando Impressao Imagem]==");
                printer.ImprimeImagem("gertec_2");
                printer.AvancaLinha(10);
                printer.ImprimeTexto("====[Fim Impressão Imagem]====");
                printer.AvancaLinha(10);
                // Fim Imagem

                // Impressão Centralizada
                configPrint.Alinhamento = "CENTER";
                configPrint.Tamanho = 30;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("CENTRALIZADO");
                printer.AvancaLinha(10);
                // Fim Impressão Centralizada

                // Impressão Esquerda
                configPrint.Alinhamento = "LEFT";
                configPrint.Tamanho = 40;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("ESQUERDA");
                printer.AvancaLinha(10);
                // Fim Impressão Esquerda

                // Impressão Direita
                configPrint.Alinhamento = "RIGHT";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("DIREITA");
                printer.AvancaLinha(10);
                // Fim Impressão Direita

                // Impressão Negrito
                configPrint.Negrito = true;
                configPrint.Alinhamento = "LEFT";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("=======[Escrita Netrigo]=======");
                printer.AvancaLinha(10);
                // Fim Impressão Negrito

                // Impressão Italico
                configPrint.Negrito = false;
                configPrint.Italico = true;
                configPrint.Alinhamento = "LEFT";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("=======[Escrita Italico]=======");
                printer.AvancaLinha(10);
                // Fim Impressão Italico

                // Impressão Italico
                configPrint.Negrito = false;
                configPrint.Italico = false;
                configPrint.SubLinhado = true;
                configPrint.Alinhamento = "LEFT";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("======[Escrita Sublinhado]=====");
                printer.AvancaLinha(10);
                // Fim Impressão Italico

                // Impressão BarCode 128
                configPrint.Negrito = false;
                configPrint.Italico = false;
                configPrint.SubLinhado = false;
                configPrint.Alinhamento = "CENTER";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("====[Codigo Barras CODE 128]====");
                printer.ImprimeBarCode("12345678901234567890", 120, 120, "CODE_128");
                printer.AvancaLinha(10);
                // Fim Impressão BarCode 128

                // Impressão Normal
                configPrint.Negrito = false;
                configPrint.Italico = false;
                configPrint.SubLinhado = true;
                configPrint.Alinhamento = "LEFT";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("=======[Escrita Normal]=======");
                printer.AvancaLinha(10);
                // Fim Impressão Normal

                // Impressão Normal
                configPrint.Negrito = false;
                configPrint.Italico = false;
                configPrint.SubLinhado = true;
                configPrint.Alinhamento = "LEFT";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("=========[BlankLine 50]=========");
                printer.AvancaLinha(50);
                printer.ImprimeTexto("=======[Fim BlankLine 50]=======");
                printer.AvancaLinha(10);
                // Fim Impressão Normal

                // Impressão BarCode 13
                configPrint.Negrito = false;
                configPrint.Italico = false;
                configPrint.SubLinhado = false;
                configPrint.Alinhamento = "CENTER";
                configPrint.Tamanho = 20;
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("=====[Codigo Barras EAN13]=====");
                printer.ImprimeBarCode("7891234567895", 120, 120, "EAN_13");
                printer.AvancaLinha(10);
                // Fim Impressão BarCode 128

                // Impressão BarCode 13
                printer.setConfigImpressao(configPrint);
                printer.ImprimeTexto("===[Codigo QrCode Gertec LIB]==");
                printer.AvancaLinha(10);
                printer.ImprimeBarCode("Gertec Developer Partner LIB", 240, 240, "QR_CODE");

                configPrint.Negrito = false;
                configPrint.Italico = false;
                configPrint.SubLinhado = false;
                configPrint.Alinhamento = "CENTER";
                configPrint.Tamanho = 20;
                printer.ImprimeTexto("===[Codigo QrCode Gertec IMG]==");
                printer.ImprimeBarCodeIMG("Gertec Developer Partner IMG", 240, 240, "QR_CODE");

                printer.AvancaLinha(100);
                // Fim Imagem
            }
            catch (GediException e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
            }
            finally
            {
                try
                {
                    printer.ImpressoraOutput();
                }
                catch (GediException e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Long).Show();
                }
            }
        }
    }
}