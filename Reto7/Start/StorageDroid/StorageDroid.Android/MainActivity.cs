using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using System.IO;

namespace StorageDroid.Droid
{
	[Activity (Label = "StorageDroid.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

            button.Click += Button_Click;

         
		}

        private void BtnRegister_Click(object sender, EventArgs e)
        {
        //    var intent = new Intent(this, typeof(RegistroActivity));
          //  intent.PutExtra("Reto", "Reto7+neomatrixisra25@hotmail.com");
            //StartActivity(intent);
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            string imageName = "IsraelPerezSaucedo20170523";
            var view = this.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Android.Graphics.Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 0, stream);
                bitmapData = stream.ToArray();

                // contenedor de azure blobs storage
               // var container = GetContainer();

                string identifierImg = imageName; //string.Format("{0}.jpg", Guid.NewGuid().ToString());

                StorageDroid.StorageService storageSvc = new StorageService();
                await storageSvc.performBlobOperation(imageName, bitmapData);

                Thread.Sleep(2000);

                Toast.MakeText(this, "Captura hecha correcta", ToastLength.Short).Show();
            }


            string email = "neomatrixisra25@hotmail.com"; //FindViewById<EditText>(Resource.Id.editTextEmail).Text;

            //Toast.MakeText(this, "Registro enviado correctamente.", ToastLength.Long).Show();

            ServiceHelper helper = new ServiceHelper();

            string reto = imageName + "jpg"; //"Reto7+" + email;//Intent.GetStringExtra("Reto");

            string androidId = Android.Provider.Settings.Secure.GetString(ContentResolver,
                Android.Provider.Settings.Secure.AndroidId);
            if (string.IsNullOrEmpty(email))
            {
                Toast.MakeText(this, "Por favor introduce un correo electrónico válido", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Enviando tu registro", ToastLength.Short).Show();
                await helper.InsertarEntidad(email, reto, androidId);
                Toast.MakeText(this, "Gracias por registrar la actividad.", ToastLength.Long).Show();
                SetResult(Result.Ok, Intent);
            }
        }
    }
}


