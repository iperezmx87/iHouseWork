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

namespace StorageDroid.Droid
{
    [Activity(Label = "RegistroActivity")]
    public class RegistroActivity : Activity
    {
        int numItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registro);
            FindViewById<Button>(Resource.Id.btnRegistraActividad).Click += RegistroActivity_Click;
        }

        async void RegistroActivity_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceHelper helper = new ServiceHelper();

                string email = "neomatrxisra25@hotmail.com"; //FindViewById<EditText>(Resource.Id.editTextEmail).Text;
                string reto = Intent.GetStringExtra("Reto");
                
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
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

    }
}