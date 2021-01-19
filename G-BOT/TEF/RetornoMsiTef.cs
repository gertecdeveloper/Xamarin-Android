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

namespace Gbot_XamarinAndroid.TEF
{
    public class RetornoMsiTef
    {
        public string NUM_PARC { get; set; }

        public string NSUHOST { get; }
        public string SitefTipoParcela { get; }
        public string NSUSitef { get; }
        public string CodTrans { get; set; }

        public string NameTransCod
        {
            get
            {
                switch (SitefTipoParcela)
                {
                    case "00":
                        return "A vista";
                    case "01":
                        return "Pré-Datado";
                    case "02":
                        return "Parcelado Loja";
                    case "03":
                        return "Parcelado Adm";
                    default:
                        return "Valor invalido";
                };
            }
        }

        public string VlTroco { get; }
        public string Parcelas => NUM_PARC ?? "";

        public string CodAutorizacao { get; }
        public string TextoImpressoEstabelecimento { get; }

        public string TextoImpressoCliente { get; }
        public string CompDadosConf { get; }
        public string CodResp { get; }
        public string RedeAut { get; }
        public string Bandeira { get; }
    }
}