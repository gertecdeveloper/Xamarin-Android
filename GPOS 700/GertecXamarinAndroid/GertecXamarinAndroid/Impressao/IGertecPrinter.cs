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

namespace GertecXamarinAndroid.Impressao
{
    interface IGertecPrinter
    {
        void setConfigImpressao(ConfigPrint config);
        String getStatusImpressora();
        void ImpressoraOutput();
        void ImprimeTexto(String texto);
        void ImprimeTexto(String texto, int tamanho);
        void ImprimeTexto(String texto, bool negrito);
        void ImprimeTexto(String texto, bool negrito, bool italico);
        void ImprimeTexto(String texto, bool negrito, bool italico, bool sublinhado);
        bool sPrintLine(String texto);
        bool ImprimeImagem(String imagem);
        bool ImprimeBarCode(String texto, int height, int width, string barcodeFormat);
        void AvancaLinha(int linhas);
        bool IsImpressoraOK();
    }
}