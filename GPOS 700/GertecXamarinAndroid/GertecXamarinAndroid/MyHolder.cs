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

namespace GertecXamarinAndroid
{
    class MyHolder
    {
        public TextView NameText;
        public ImageView Img;

        public MyHolder(View itemView)
        {
            NameText = itemView.FindViewById<TextView>(Resource.Id.txtProjeto);
            Img = itemView.FindViewById<ImageView>(Resource.Id.imgIcon);
        }
    }
}