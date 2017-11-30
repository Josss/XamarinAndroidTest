using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XamarinAndroidTest.Services;
using Java.Lang;

namespace XamarinAndroidTest
{
    [Activity(Label = "XamarinAndroidTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.button1);

            button.Click += Button_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            EditText nameField = FindViewById<EditText>(Resource.Id.name);
            DatePicker picker = (DatePicker)FindViewById(Resource.Id.datePicker1);

            string nameStr = nameField.Text;
            DateTime dob = picker.DateTime;

            if (string.IsNullOrEmpty(nameStr))
            {
                Toast.MakeText(this, "Name should not be empty.", ToastLength.Short).Show();
                return;
            }

            // Just save it.
            Person person = new Person
            {
                Name = nameStr,
                Dob = dob
            };
            DataStore.PersonHelper.InsertPerson(this, person);
            Toast.MakeText(this, "Person created, fetching the data back.", ToastLength.Short).Show();

            var people = DataStore.PersonHelper.GetPeople(this);
            person = people[people.Count - 1];
            Toast.MakeText(this, $"{person.Name} was born on {person.Dob.ToString("MMMM dd yyyy")}. \n {people.Count} people found.", ToastLength.Short).Show();
        }
    }
}
