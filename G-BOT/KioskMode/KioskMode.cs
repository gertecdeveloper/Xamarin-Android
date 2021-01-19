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

namespace Gbot_XamarinAndroid.KioskMode
{
    [Activity(Label = "KisokMode")]
    public class KioskMode : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.kioskMode);


            var btnStartModoKiosk = FindViewById<Button>(Resource.Id.btStartLockTask);
            btnStartModoKiosk.Click += delegate
            {
                // trava o aplicativo e coloca em fullscreen
                this.StartLockTask();
                SetVisibleMode(true);
            };

            var btnStopModoKiosk = FindViewById<Button>(Resource.Id.btStopLockTask);
            btnStopModoKiosk.Click += delegate
            {
                //libera o aplicativo
                this.StopLockTask();
                SetVisibleMode(false);
            };
        }

        private void SetVisibleMode(bool isFullscreen)
        {
            RunOnUiThread(() =>
            {
                if (isFullscreen)
                {
                    //altera a interface para fullscreen
                    this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                        SystemUiFlags.Fullscreen
                        | SystemUiFlags.HideNavigation
                        | SystemUiFlags.Immersive
                        | SystemUiFlags.ImmersiveSticky
                        | SystemUiFlags.LowProfile
                        | SystemUiFlags.LayoutStable
                        | SystemUiFlags.LayoutHideNavigation
                        | SystemUiFlags.LayoutFullscreen
                    );
                }
                else
                {
                    this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                        SystemUiFlags.LayoutStable
                    );
                }
            });
        }




    }
}