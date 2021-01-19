using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Nio.Charset;
using System;
using System.Diagnostics;
using System.Text;

namespace Gbot_XamarinAndroid.NFC
{
    [Activity(Label = "MenuNFC")]
    public class MenuNFC : Activity
    {
        private EditText editMensagemPadrao;
        private Button btn_ler;
        private Button btn_gravar;
        private Button btn_teste;
        private Button btn_formatarCartao;
        private NfcAdapter mNfcAdapter;
        private Tag tag;

        private LinearLayout painel_nfc;

        private TextView painel_nfc_resposta;

        private bool isWrite = false;
        private bool isRead = false;
        private bool isFormat = false;
        private bool isForceTeste = false;



        private static string MENSAGEM_PADRAO = "GERTEC1000";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menuNFC);
            // Create your application here

            editMensagemPadrao = FindViewById<EditText>(Resource.Id.editMensagemPadrao);
            
            btn_formatarCartao = FindViewById<Button>(Resource.Id.btn_formatarCartao);
            btn_gravar = FindViewById<Button>(Resource.Id.btn_gravar);
            btn_ler = FindViewById<Button>(Resource.Id.btn_leitura);
            btn_teste = FindViewById<Button>(Resource.Id.btn_teste);
            mNfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            painel_nfc = FindViewById<LinearLayout>(Resource.Id.painel_nfc);
            painel_nfc.Visibility = ViewStates.Invisible;

            painel_nfc_resposta = FindViewById<TextView>(Resource.Id.response);



            initViews();
            InitNFC();
        }

     

        private void InitNFC()
        {
            base.OnStart();
            mNfcAdapter = NfcAdapter.GetDefaultAdapter(this);
        }

    
        private void initViews()
        {
            btn_formatarCartao.Click += delegate
            {
                ShowFormat();
            };

            btn_teste.Click += delegate
            {
                ShowReadWrite();
            };

            btn_ler.Click += delegate
            {
                ShowRead();
            };

            btn_gravar.Click += delegate
            {
                ShowWrite();
            };
        }

     

       

        public string idCartao(Tag tag)
        {

            byte[] idCartao = null;

            idCartao = tag.GetId();
            
            if (idCartao == null) return "";

            return BitConverter.ToInt32(idCartao, 0).ToString();
        }


        private void ShowPainel(Boolean show,string title, string response)
        {            
            painel_nfc.Visibility = show ? ViewStates.Visible : ViewStates.Invisible;
            var painel_nfc_title = FindViewById<TextView>(Resource.Id.title);
            painel_nfc_title.Text = title;
            painel_nfc_resposta.Text = response;
        }

        private void ShowWrite()
        {
            isWrite = true;
            isRead = false;
            isForceTeste = false;
            isFormat = false;
            ShowPainel(true, "Gravar cartão", "Aproxime o cartão");
        }

        private void ShowRead()
        {
            isRead = true;
            isWrite = false;
            isForceTeste = false;
            isFormat = false;

            ShowPainel(true,"Leitura do Cartão NFC","Aproxime o cartão");
          
        }

     
        private void ShowFormat()
        {
            isFormat = true;
            isRead = false;
            isWrite = false;
            isForceTeste = false;

            ShowPainel(true, "Formatar cartão", "Aproxime o cartão");

        }

        private void ShowReadWrite()
        {
            isForceTeste = true;
            isFormat = false;
            isRead = false;
            isWrite = false;

            ShowPainel(true, "Teste Leitura/Gravação", "Aproxime o cartão");

        }

        private Boolean FormatNFC(Ndef ndef)
        {
            bool retorno = false;

            NdefFormatable ndefFormatable = NdefFormatable.Get(ndef.Tag);
            Java.Lang.String msg = new Java.Lang.String(MENSAGEM_PADRAO);
            try
            {

                if (ndefFormatable == null)
                {
                    return retorno;
                }

                if (!ndefFormatable.IsConnected)
                {
                    ndefFormatable.Connect();
                }
                ndefFormatable.Format(new NdefMessage(NdefRecord.CreateMime
                    ("UTF-8", msg.GetBytes(Charset.ForName("UTF-8")))));
                ndefFormatable.Close();
                retorno = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new System.Exception("Não foi possível ler o cartão.");
            }

            return retorno;
        }

        private bool WriteNFC(Ndef ndef, string mensagem)
        {
            bool retorno = false;
            try
            {
                if (ndef != null)
                {

                    ndef.Connect();
                    NdefRecord mimeRecord = null;

                    Java.Lang.String str = new Java.Lang.String(mensagem);

                    mimeRecord = NdefRecord.CreateMime
                        ("UTF-8", str.GetBytes(Charset.ForName("UTF-8")));

                    ndef.WriteNdefMessage(new NdefMessage(mimeRecord));
                    ndef.Close();
                    retorno = true;

                }
                else
                {
                    retorno = FormatNFC(ndef);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new System.Exception("Não foi possível ler o cartão.");
            }

            return retorno;
        }

        private string ReadNFC(Ndef ndef)
        {
            string message;

            try
            {
                if (ndef == null)
                {
                    throw new System.Exception("Não foi possível ler o cartão.");
                }

                if (!ndef.IsConnected)
                {
                    ndef.Connect();
                }

                NdefMessage ndefMessage = ndef.NdefMessage;
                if (ndefMessage == null)
                {
                    throw new System.Exception("Não foi possível ler o cartão.");
                }
                else
                {
                    message = Encoding.UTF8.GetString(ndefMessage.GetRecords()[0].GetPayload());
                    return message;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new System.Exception("Não foi possível ler o cartão.");
            }

        }

            

        protected override void OnResume()
        {
            base.OnResume();
            IntentFilter tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
            IntentFilter ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            IntentFilter techDetected = new IntentFilter(NfcAdapter.ActionTechDiscovered);
            IntentFilter idDetected = new IntentFilter((NfcAdapter.ExtraAid));
            IntentFilter[] nfcIntentFilter = new IntentFilter[] { techDetected, tagDetected, ndefDetected, idDetected };

            //Enable the foreground dispatch.
            mNfcAdapter.EnableForegroundDispatch
            (
                this,
                PendingIntent.GetActivity(this, 0, new Intent(this, GetType()).AddFlags(ActivityFlags.SingleTop), 0),
                nfcIntentFilter,
                new string[][] { new string[] { "android.nfc.tech.Ndef", "android.nfc.action.NDEF_DISCOVERED" } }
            );
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (mNfcAdapter != null)
            {
                mNfcAdapter.DisableForegroundDispatch(this);
            }
        }

       
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;

            Log.Debug("TAG", "onNewIntent: " + intent.Action);

            if (tag != null)
            {

                var id = idCartao(tag);
                 var ndef = Ndef.Get(tag);
              
                    if (ndef == null)
                    {
                        painel_nfc_resposta.Text = "ID Cartão: " + id + "\nMensagem: Tipo de cartão não suportado.";
                    }
                    else if (isWrite)
                    {
                        string messageToWrite = editMensagemPadrao.Text;
                        if (messageToWrite.Equals(""))
                        {
                            Toast.MakeText(Android.App.Application.Context, "Preencha uma mensagem", ToastLength.Short).Show();
                        }
                        else
                        {
                           if ( WriteNFC(ndef, messageToWrite))
                            painel_nfc_resposta.Text = "Cartão gravado com sucesso!";
                    }
                    }
                    else if (isRead)
                    {
                        var resposta = ReadNFC(ndef);
                        painel_nfc_resposta.Text = "ID Cartão: " + id + "\nMensagem: " + resposta;
                }
                    else if (isFormat)
                    {
                       if (FormatNFC(ndef))
                       {
                            painel_nfc_resposta.Text = "Cartão formatado com sucesso!";
                        }else
                        {
                        painel_nfc_resposta.Text = "Não precisa formatar";
                    }
                        
                    }
                    else if (isForceTeste)
                    {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    if (WriteNFC(ndef, "GERTEC1000"))
                    {
                        var resposta = ReadNFC(ndef);
                        stopwatch.Stop();

                        painel_nfc_resposta.Text = "ID Cartão: " + id + "\nMensagem: " + resposta + "\nTempo execução: " + stopwatch.Elapsed.TotalSeconds + " segundos";
                    }

                }

            }
        }

    }
}