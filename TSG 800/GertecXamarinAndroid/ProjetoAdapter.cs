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

namespace GertecXamarinAndroid.Services
{
    class ProjetoAdapter : BaseAdapter 
    {
        private Context c;
        private JavaList<Projeto> projetos;
        private LayoutInflater inflater;

        public ProjetoAdapter(Context c, JavaList<Projeto> projetos)
        {
            this.c = c;
            this.projetos = projetos;
        }

        public override int Count 
        {
            get { return projetos.Size(); }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return projetos.Get(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.Model, parent, false);
            }
            //BIND DATA
            MyHolder holder = new MyHolder(convertView);
            holder.NameText.Text = projetos[position].Name;
            holder.Img.SetImageResource(projetos[position].Image);

            return convertView;
        }
    }
}