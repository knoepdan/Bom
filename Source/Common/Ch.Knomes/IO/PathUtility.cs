using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;


namespace Ch.Knomes.IO
{
    public static class PathUtility
    {

        // alternative HierarchyData 
        // https://docs.microsoft.com/en-us/sql/relational-databases/hierarchical-data-sql-server?view=sql-server-2017
        // https://github.com/aspnet/EntityFrameworkCore/issues/365  (there is some support in ef core)




        /// <summary>
        /// Entfernt oder ersetzt ungültige Zeichen in Pfäden gemäss System.IO.Path.GetInvalidPathChars().
        /// </summary>
        /// <param name="path">Der zu überprüfende Pfad</param>
        /// <param name="replaceWith">Ungültige Zeichen werden hiermit ersetzt</param>
        /// <remarks>
        /// Wichtig: Diese Methode ist nicht geeignet, um einen gültigen Dateinamen zu erzeugen, da
        /// sie z.B. den Pfadtrenner "/" oder das Wildcard-Token "*" zulässt (und weitere)
        /// <see cref="StripInvalidFilenameCharacters"/>.
        /// </remarks>
        public static string StripInvalidPathCharacters(string? path, string replaceWith = "")
        {
            if (path == null)
            {
                return "";
            }
            var invalid = System.IO.Path.GetInvalidPathChars(); //new ArrayList(System.IO.Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(CultureInfo.InvariantCulture), replaceWith, StringComparison.InvariantCulture);
            }
            return path;
        }

        /// <summary>
        /// Entfernt oder ersetzt ungültige Zeichen in Dateinamen gemäss System.IO.Path.GetInvalidPathChars().
        /// </summary>
        /// <param name="path">Der zu überprüfende Pfad</param>
        /// <param name="replaceWith">Ungültige Zeichen werden hiermit ersetzt</param>
        public static string StripInvalidFilenameCharacters(string? path, string replaceWith = "")
        {
            if(path == null)
            {
                return "";
            }
            var invalid = System.IO.Path.GetInvalidFileNameChars();//new ArrayList(System.IO.Path.GetInvalidFileNameChars());
            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(CultureInfo.InvariantCulture), replaceWith, StringComparison.InvariantCulture);
            }
            return path;
        }
    }
}