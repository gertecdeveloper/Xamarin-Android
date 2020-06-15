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

namespace Gpos700_XamrinAndroid.TEF
{
    public class RetornoGer7
    {
        public string type { get; set; }
        public string version { get; set; }
        public string status { get; set; }
        public string config { get; set; }
        public string license { get; set; }
        public string terminal { get; set; }
        public string merchant { get; set; }
        public string id { get; set; }
        public string product { get; set; }
        public string response { get; set; }
        public string authorization { get; set; }
        public string amount { get; set; }
        public string installments { get; set; }
        public string instmode { get; set; }
        public string stan { get; set; }
        public string rrn { get; set; }
        public string time { get; set; }
        public string print { get; set; }
        public string track2 { get; set; }
        public string aid { get; set; }
        public string cardholder { get; set; }
        public string prefname { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string label { get; set; }
    }
    public class RetornoMsiTef
    {
        private string CODRESP;
        private string COMP_DADOS_CONF;
        private string CODTRANS;
        private string VLTROCO;
        private string REDE_AUT;
        private string BANDEIRA;
        private string NSU_SITEF;
        private string NSU_HOST;
        private string COD_AUTORIZACAO;
        private string TIPO_PARC;
        private string NUM_PARC;
        private string VIA_ESTABELECIMENTO;
        private string VIA_CLIENTE;

        public string getNSUHOST()
        {
            return this.NSU_HOST;
        }
        public string getSitefTipoParcela()
        {
            return this.TIPO_PARC;
        }
        public string getNSUSitef()
        {
            return this.NSU_SITEF;
        }
        public string getCodTrans()
        {
            return this.CODTRANS;
        }
        public void setCodTrans(string _cODTRANS)
        {
            this.CODTRANS = _cODTRANS;
        }

        public string getNameTransCod()
        {
            string retorno = "Valor invalido";
            switch (this.TIPO_PARC)
            {
                case "00":
                    retorno = "A vista";
                    break;
                case "01":
                    retorno = "Pré-Datado";
                    break;
                case "02":
                    retorno = "Parcelado Loja";
                    break;
                case "03":
                    retorno = "Parcelado Adm";
                    break;
            }
            return retorno;
        }

        public string getvlTroco()
        {
            return this.VLTROCO;
        }
        public string getParcelas()
        {
            if (this.NUM_PARC != null)
            {
                return this.NUM_PARC;
            }
            return "";
        }

        public string getCodAutorizacao()
        {
            return this.COD_AUTORIZACAO;
        }
        public string textoImpressoEstabelecimento()
        {
            return this.VIA_ESTABELECIMENTO;
        }

        public string textoImpressoCliente() { return this.VIA_CLIENTE; }
        public string getCompDadosConf() { return this.COMP_DADOS_CONF; }
        public string getCodResp() { return this.CODRESP; }
        public string getRedeAut() { return this.REDE_AUT; }
        public string getBandeira() { return this.BANDEIRA; }
    }
}