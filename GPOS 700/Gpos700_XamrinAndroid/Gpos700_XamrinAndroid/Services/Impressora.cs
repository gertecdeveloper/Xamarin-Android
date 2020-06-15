using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using BR.Com.Gertec.Gedi.Exceptions;
using Plugin.DeviceInfo;
using System;
using Gpos700_XamrinAndroid.Impressao;
using System.Linq;

namespace Gpos700_XamrinAndroid.Services
{
    [Activity(Label = "Impressora")]
    public class Impressora : Activity
    {
        string modelo;

        Context context;

        TextView txtModelo;

        EditText txtMensagem;

        RadioButton rbEsquerda, rbCentralizado, rbDireta;

        ToggleButton btnNegrito, btnItalico, btnSublinhado;

        Spinner spFonte, spTamanho;

        Spinner spCodeHeight, spCodeWidth, spTipoCode;

        Button btnStatusImpressora, btnImprimirMensagem, btnImprimirImagem, btnImprimirBarCode, btnImprimiTodasFunc;

        GertecPrinter printer;
        ConfigPrint configPrint;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Impressao);

            modelo = CrossDeviceInfo.Current.Model;

            // Ini Textview
            iniTextview();

            // Ini EditText
            iniEditText();

            // Ini Radio Button
            iniRadioButton();

            // Ini ToggleButton
            iniToggleButton();

            // Ini Spinner
            iniSpinner();

            // Ini Buttons
            iniButtons();

            // Ini funções bottuns
            iniFuncoesButtons();

            // Carrega todos os Spinner
            iniLoadSpinner();

            // Mostra modelo
            iniLoadModelo();

            // Ini context
            context = ApplicationContext;
            // #if __G700__
            printer = new GertecPrinter(context);
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

        private void iniTextview()
        {
            txtModelo = FindViewById<TextView>(Resource.Id.txtModelo);
        }

        private void iniEditText()
        {
            txtMensagem = FindViewById<EditText>(Resource.Id.txtMensagem);
        }

        private void iniRadioButton()
        {
            rbEsquerda = FindViewById<RadioButton>(Resource.Id.rbEsquerda);
            rbCentralizado = FindViewById<RadioButton>(Resource.Id.rbCentralizado);
            rbDireta = FindViewById<RadioButton>(Resource.Id.rbDireita);
        }

        private void iniToggleButton()
        {
            btnNegrito = FindViewById<ToggleButton>(Resource.Id.btnNegrito);
            btnItalico = FindViewById<ToggleButton>(Resource.Id.btnItalico);
            btnSublinhado = FindViewById<ToggleButton>(Resource.Id.btnSublinhado);
            
        }

        private void iniSpinner()
        {
            spFonte = FindViewById<Spinner>(Resource.Id.spFonte);
            spTamanho = FindViewById<Spinner>(Resource.Id.spTamanho);
            spCodeHeight = FindViewById<Spinner>(Resource.Id.spCodeHeight);
            spCodeWidth = FindViewById<Spinner>(Resource.Id.spCodeWidth);
            spTipoCode = FindViewById<Spinner>(Resource.Id.spTipoCode);
        }

        private void iniButtons()
        {
            btnStatusImpressora = FindViewById<Button>(Resource.Id.btnStatusImpressora);
            btnImprimirMensagem = FindViewById<Button>(Resource.Id.btnImprimirMensagem);
            btnImprimirImagem = FindViewById<Button>(Resource.Id.btnImprimirImagem);
            btnImprimirBarCode = FindViewById<Button>(Resource.Id.btnImprimirBarCode);
            btnImprimiTodasFunc = FindViewById<Button>(Resource.Id.btnImprimirTodasFunc);
        }

        private void iniFuncoesButtons()
        {
            btnStatusImpressora.Click += delegate
            {
                BtnStatusImpressora_Click();
            };


            btnImprimirMensagem.Click += delegate
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


                    }
                    catch (GediException e)
                    {
                        Toast.MakeText(this, e.Message, ToastLength.Long).Show();
                    }
                }
            };

            btnImprimirImagem.Click += delegate
            {
                BtnImprimirImagem_Click();
            };

            btnImprimirBarCode.Click += delegate
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
                }
            };

            btnImprimiTodasFunc.Click += delegate
            {
                BtnImprimirTodasFunc_Click();
            };
        }

        private void iniLoadModelo()
        {
            if (modelo.Equals("Smart G800"))
            {
                txtModelo.Text = "Xamarin Impressão TSG800";
            }
            else
            {
                txtModelo.Text = "Xamarin Impressão GPOS700";
            }
        }

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

        public void BtnStatusImpressora_Click()
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

        protected void BtnImprimirMensagem_Click(string mensagem, string alinhamento, bool btnNegrito, bool btnItalico, bool btnSublinhado, string fonte, int tamanho)
        {
            if (String.IsNullOrWhiteSpace(mensagem))
            {
                Toast.MakeText(context, "Escreva uma mensagem", ToastLength.Long).Show();
            }
            else if (String.IsNullOrWhiteSpace(alinhamento))
            {
                Toast.MakeText(context, "Selecione um alinhamento", ToastLength.Long).Show();
            }
            else
            {
                try
                {
                    configPrint.Alinhamento = alinhamento;
                    configPrint.Negrito = btnNegrito;
                    configPrint.Italico = btnItalico;
                    configPrint.SubLinhado = btnSublinhado;
                    configPrint.Fonte = fonte;
                    configPrint.Tamanho = tamanho;

                    printer.setConfigImpressao(configPrint);
                    printer.ImprimeTexto(mensagem);
                    printer.AvancaLinha(100);
                    printer.ImpressoraOutput();
                }
                catch (GediException e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Long).Show();
                }
            }
        }

        protected void BtnImprimirImagem_Click()
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
            printer.AvancaLinha(100);
            printer.ImpressoraOutput();
        }

        protected void BtnImprimirBarCode_Click(string mensagem, int height, int width, string tipoCode)
        {
            if (String.IsNullOrWhiteSpace(mensagem))
            {
                Toast.MakeText(context, "Escreva uma mensagem", ToastLength.Long).Show();
            }
            else if (tipoCode.Equals("EAN_8") || tipoCode.Equals("EAN_13"))
            {
                if (mensagem.All(char.IsDigit))
                {
                    if ((tipoCode.Equals("EAN_8") && mensagem.Length == 7) || (tipoCode.Equals("EAN_13") && mensagem.Length == 12))
                    {
                        printer.ImprimeBarCode(
                        mensagem,
                        height,
                        width,
                        tipoCode);
                        printer.AvancaLinha(100);
                        printer.ImpressoraOutput();
                    }
                    else
                    {
                        Toast.MakeText(context, "", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(context, "", ToastLength.Long).Show();
                }
            }
            else
            {
                printer.ImprimeBarCode(
                    mensagem,
                    height,
                    width,
                    tipoCode);
                printer.AvancaLinha(100);
                printer.ImpressoraOutput();
            }
        }

        protected void BtnImprimirTodasFunc_Click()
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