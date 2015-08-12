using System;
using Finisar.SQLite;

namespace Akuma
{
    public class DBConnection
    {
        private static DBConnection _instance;

        private DBConnection() { }

        public static DBConnection GetInstance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DBConnection();
                }

                return _instance;
            }
        }

        public SQLiteConnection OpenConnection(String strCon)
        {
            SQLiteConnection sqlcon = new SQLiteConnection(strCon);

            return sqlcon;
        }
    }
}
