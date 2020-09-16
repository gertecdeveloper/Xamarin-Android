namespace Gpos700_XamrinAndroid.ExemploTEF
{
    public class RetornoGer7
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public string Config { get; set; }
        public string License { get; set; }
        public string Terminal { get; set; }
        public string Merchant { get; set; }
        public string Id { get; set; }
        public string Product { get; set; }
        public string Response { get; set; }
        public string Authorization { get; set; }
        public string Amount { get; set; }
        public string Installments { get; set; }
        public string Instmode { get; set; }
        public string Stan { get; set; }
        public string Rrn { get; set; }
        public string Time { get; set; }
        public string Print { get; set; }
        public string Track2 { get; set; }
        public string Aid { get; set; }
        public string Cardholder { get; set; }
        public string Prefname { get; set; }
        public string Errcode { get; set; }
        public string Errmsg { get; set; }
        public string Label { get; set; }
    }
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
                switch(SitefTipoParcela)
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