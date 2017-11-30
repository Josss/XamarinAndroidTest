using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database.Sqlite;
using Android.Database;

namespace XamarinAndroidTest.Services
{
    class DataStore : SQLiteOpenHelper
    {
        private const string _DatabaseName = "mydatabase.db";

        public DataStore(Context context) : base (context, _DatabaseName, null, 1)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(PersonHelper.CreateQuery);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL(PersonHelper.DeleteQuery);
            OnCreate(db);
        }

        public class PersonHelper
        {
            private const string TableName = "persontable";
            private const string ColumnID = "id";
            private const string ColumnName = "name";
            private const string ColumnDob = "dob";

            public const string CreateQuery = "CREATE TABLE " + TableName + " ( "
                + ColumnID + " INTEGER PRIMARY KEY,"
                + ColumnName + " TEXT,"
                + ColumnDob + " TEXT)";


            public const string DeleteQuery = "DROP TABLE IF EXISTS " + TableName;
            
            public PersonHelper()
            {
            }

            public static void InsertPerson(Context context, Person person)
            {
                SQLiteDatabase db = new DataStore(context).WritableDatabase;
                ContentValues contentValues = new ContentValues();
                contentValues.Put(ColumnName, person.Name);
                contentValues.Put(ColumnDob, person.Dob.ToString());

                db.Insert(TableName, null, contentValues);
                db.Close();
            }

            public static List<Person> GetPeople(Context context)
            {
                List<Person> people = new List<Person>();
                SQLiteDatabase db = new DataStore(context).ReadableDatabase;
                string[] columns = new string[] { ColumnID, ColumnName, ColumnDob };

                using (ICursor cursor = db.Query(TableName, columns, null, null, null, null, null))
                {
                    while (cursor.MoveToNext())
                    {
                        people.Add(new Person
                        {
                            Id = cursor.GetInt(cursor.GetColumnIndexOrThrow(ColumnID)),
                            Name = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnName)),
                            Dob = DateTime.Parse(cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnDob)))
                        });
                    }
                }
                db.Close();
                return people;
            }

            public static void UpdatePerson(Context context, Person person)
            {
                SQLiteDatabase db = new DataStore(context).WritableDatabase;
                ContentValues contentValues = new ContentValues();
                contentValues.Put(ColumnName, person.Name);
                contentValues.Put(ColumnDob, person.Dob.ToString());

                db.Update(TableName, contentValues, ColumnID + "=?", new string[] { person.Id.ToString() });
                db.Close();
            }

            public static void DeletePerson(Context context, int id)
            {
                SQLiteDatabase db = new DataStore(context).WritableDatabase;
                db.Delete(TableName, ColumnID + "=?", new string[] { id.ToString() });
                db.Close();
            }
        }
    }
}