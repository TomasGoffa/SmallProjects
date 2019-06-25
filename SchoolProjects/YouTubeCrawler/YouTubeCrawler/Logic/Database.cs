/// <summary>
/// This file is part of school project for
/// Semantic and Social Web
/// 
/// Authors     : Matej Kvetko
///             : Tomas Goffa
/// </summary>

namespace YouTubeCrawler
{
    using System.Collections.Generic;
    using LiteDB;

    public static class Database
    {

        public static void AddToDBS (string route, DatabaseRow row)
        {
            using (var db = new LiteDatabase (route))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<DatabaseRow> ("customers");
                col.Insert (row);
            }
        }

        public static IEnumerable<DatabaseRow> SelectAllDatabase (string route)
        {
            using (var db = new LiteDatabase (route))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<DatabaseRow> ("customers");
                return col.FindAll ();
            }
        }

        public static void ClearDBS (string route)
        {
            using (var db = new LiteDatabase (route))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<DatabaseRow> ("customers");
                col.Delete (Query.All ());
            }
        }
    }
}
