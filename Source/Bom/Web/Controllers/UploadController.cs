using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Bom.Web.Controllers.Upload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Ch.Knomes.IO;
using Bom.Web.Lib.Infrastructure;

namespace Bom.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BomBaseController
    {
        //  private static readonly ILogger _logger = LoggerFactory.GetLogger(typeof(UploadController));

        #region static part


        /// <summary>
        /// Char that separates files in the file string in case of multiple uploads
        /// </summary>
        /// <remarks>
        /// a) must be in sync with client part (js)
        /// b) current char was chosen as it is not support on window file systems
        /// </remarks>
        public const char SeparatorChar = ':';


        /// <summary>
        /// Codes for clientside textes (siehe InlineUplad.de.js etc.)
        /// </summary>
        /// <remarks>must always comply with codes in js/ts file</remarks>
        public static class LocalizationCodes
        {
            // ReSharper disable UnusedMember.Global
            public const string MessageInvalidFileType = "messageInvalidFileType";
            public const string MessageUnauthorized = "messageUnauthorized";
            public const string MessageUnknownError = "messageUnknownError";
            // .... other strings only used on client side
            // ReSharper restore UnusedMember.Global
        }

        #region configuration


        static UploadController()
        {
            // static constructor, ensures that upload folder exists
            var tempPath = Path.GetTempPath();
            var uploadFolder = "inlineUploadFolder";
            tempPath = Path.Combine(tempPath, uploadFolder);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            else
            {
                _cleanupOldTemporaryUploadFiles();
            }
            _tempUploadPath = tempPath;
        }


        /// <summary>
        /// Full physical path in filesystem where uploaded files are to be stored
        /// </summary>
        private static string _tempUploadPath;




        /// <summary>
        /// Default: Flag that tells application to clear uploaded files after user session end
        /// </summary>
        /// <remarks>trigger to clear files must still be called from Session method</remarks>
        private static bool DefaultCanClearUploadsUponSessionEnd = true;


        private static IUploadChecker DefaultUploadChecker = null;

        private static IPostSaveHandler DefaultUploadPostSaveTreatment = null;

        /// <summary>
        /// Only to be called upon application start (global.asax)
        /// </summary>
        /// <remarks>if not called default values apply</remarks>
        public static void Configure(string tmpUploadPath, IUploadChecker uploadChecker = null, IPostSaveHandler postSaveHandler = null, bool canClearUploadsUponSessionEnd = true)
        {
            // upload folder
            if (!string.IsNullOrWhiteSpace(tmpUploadPath))
            {
                _tempUploadPath = tmpUploadPath;
            }
            if (!Directory.Exists(_tempUploadPath))
            {
                Directory.CreateDirectory(_tempUploadPath);
            }

            // othter params
            DefaultUploadChecker = uploadChecker;
            DefaultUploadPostSaveTreatment = postSaveHandler;
            DefaultCanClearUploadsUponSessionEnd = canClearUploadsUponSessionEnd;

            // clean 
            _cleanupOldTemporaryUploadFiles();
        }


        #endregion


        #endregion


        #region upload

        /// <summary>
        ///  Flag that tells application to clear uploaded files after user session end
        /// </summary>
        /// <remarks>if not overridden will use Default</remarks>
        public virtual bool CanClearUploadsUponSessionEnd
        {
            get { return DefaultCanClearUploadsUponSessionEnd; }
        }

        /// <summary>
        ///  Check if upload is allowed or not
        /// </summary>
        /// <remarks>if not overridden will use Default</remarks>
        public virtual IUploadChecker UploadChecker
        {
            get { return DefaultUploadChecker; }
        }

        /// <summary>
        ///  post upload treatment (example: resize images etc.)
        /// </summary>
        /// <remarks>if not overridden will use Default</remarks>
        public virtual IPostSaveHandler UploadPostSaveTreatment
        {
            get { return DefaultUploadPostSaveTreatment; }
        }


        //[HttpPost]
        ////   [ValidateInput(false)]
        //public ActionResult Index()
        //{
        //    return Upload();
        //}

        [HttpPost]
        //     [ValidateInput(false)]
        public ActionResult Upload()
        {


            string passedFileName = "file"; // default if not passed
            try
            {
                var request = this.Request;

                const string keyFilename = "filename";
                StringValues queryFileName;
                if (request.Query.TryGetValue(keyFilename, out queryFileName) && queryFileName.Count > 0 && !string.IsNullOrWhiteSpace(queryFileName[0]))
                {
                    passedFileName = "" + Path.GetFileName(queryFileName[0].Trim());
                }

                string identifierString = _handleUpload(passedFileName, request.Body);
                return Content(identifierString); // Z.B. "[GUID]\file.txt"
            }
            catch (Exception ex)
            {

                //  _logger.LogErrorEx("Error upon Upload for file: " + passedFileName, ex);
                // return HandleError(this.HttpContext, ex.Message);
                Utils.Dev.Todo($"better errorhandling for upload! (caught error: {ex.Message}");
                throw;
            }
        }

        private string _handleUpload(string fileName, Stream filestream)
        {
            var request = this.Request;
            // var session = Session;

            // Parameter: is image (otherwise normal upload)
            StringValues isImageValue;
            var imageRequested = request.Query.TryGetValue("isImage", out isImageValue) && isImageValue.Count > 0 && isImageValue[0] == "true";

            // Upload allowed
            if (UploadChecker != null && !UploadChecker.CanPerformUpload(fileName, request, User, imageRequested))
            {
                throw new Exception(LocalizationCodes.MessageUnauthorized);
            }

            // Define Target path
            var inlineUploadPath = InlineUploadPathBase;
            var guidFolderName = CreateGuidFolder(inlineUploadPath).ToString();
            var guidFolderPhysicalPath = Path.Combine(inlineUploadPath, guidFolderName);


            // Datei speichern (passt ggf. Dateinamen an)
            fileName = StreamToFile(filestream, guidFolderPhysicalPath, fileName);
            filestream.Dispose(); // no longer needed

            // Post-Treatment (example:sicherstellen dass ein Web-Bild ist (oder ein sonstiges Bild wie TIFF und BMP, in diesem Fall konvertieren)
            if (UploadPostSaveTreatment != null)
            {
                try
                {
                    // Improve: this is all a bit of a hack:
                    //  -> PostTreatment should be able to give error messages (problem localization etc.)
                    //  -> too much file handling code
                    //      -> maybe just require PostTreatment not change file name .. that would be so much easier
                    //  -> deleting files to ensure there is only one file is all a bit hacky 
                    //     -> maybe just delete all files except the last one that complies with new file name

                    var fullFilePath = Path.Combine(guidFolderPhysicalPath, fileName);
                    string newFullfilePath = UploadPostSaveTreatment.TreatUploadAfterSaveHandler(fullFilePath, Request, imageRequested);
                    if (newFullfilePath.ToLowerInvariant() != fullFilePath.ToLowerInvariant())
                    {
                        // a) ensure File is still in same folder
                        if (Path.GetDirectoryName(fullFilePath) != Path.GetDirectoryName(newFullfilePath))
                        {
                            var destinationPath = Path.Combine(Path.GetDirectoryName(fullFilePath), Path.GetFileName(newFullfilePath));
                            System.IO.File.Copy(newFullfilePath, destinationPath);
                            System.IO.File.Delete(newFullfilePath);
                            newFullfilePath = destinationPath;
                        }
                        fileName = Path.GetFileName(newFullfilePath);
                        if (System.IO.File.Exists(fullFilePath) && System.IO.File.Exists(fullFilePath))
                        {
                            // only one file per folder, so we have to delete previous file as UploadPostSaveTreatment has created a new one
                            System.IO.File.Delete(fullFilePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // _logger.LogDebug("Error post handling file.  Ex: " + ex.Message);
                    throw new Exception(LocalizationCodes.MessageInvalidFileType, ex);
                }

            }

            // Rückgabe zum Client
            //    var relativeFileName = Path.Combine(guidFolderName, fileName);
            //    _addUploadToSessionForPossibleDeleteUponSessionEnd(session, relativeFileName); // to allow delete upon session end
            //    return relativeFileName; // Z.B. "[GUID]\file.txt"



            //_addUploadToSessionForPossibleDeleteUponSessionEnd(session, guidFolderName); // to allow delete upon session end
            return guidFolderName;
        }



        ///// <summary>
        ///// Returns error message that can be handled by client side ajax handler
        ///// </summary>
        //private ActionResult HandleError(HttpContextBase context, string errorMessage)
        //{
        //    context.Response.ClearHeaders();
        //    context.Response.ClearContent();
        //    context.Response.TrySkipIisCustomErrors = true;
        //    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        //    context.Response.StatusDescription = errorMessage; // clientside as parameter errorThrown in Ajax-Error-Handler
        //    context.Response.ContentType = "text/plain"; // (since response body is empty)
        //    context.Response.Flush();

        //    return Content(errorMessage);
        //}


        #endregion



        #region Upload-Path

        /// <summary>
        /// Absoluter Dateisystempfad einer Datei im Session-Temporärordner ("[Session-Temporärordner]\[relative\Filename]"
        /// </summary>
        /// <param name="context">HttpContextBase</param>
        /// <param name="relativeUniqueFolder">Relativen Pfad ab temporärem Folder</param>
        /// <returns>ganzen Pfad</returns>
        private static string GetUploadFullPath(string relativeUniqueFolder)
        {
            string path = Path.Combine(InlineUploadPathBase, relativeUniqueFolder);
            return path;
            //return Path.Combine(GetSessionUploadPath(context.Session), relativeFilename);
        }

        private static string InlineUploadPathBase
        {
            get
            {
                return _tempUploadPath;
            }
        }

        /// <summary>
        /// Erstellt einen Unterordner mit eindeutigem Namen (GUID), der zurückgegeben wird
        /// </summary>
        /// <param name="path">Pfad, in dem der neue Ordner angelegt wird</param>
        /// <returns>Guid</returns>
        private static Guid CreateGuidFolder(string path)
        {
            var guid = Guid.NewGuid();
            var subPathGuid = guid.ToString();
            var subFolderPath = Path.Combine(path, subPathGuid);
            Directory.CreateDirectory(subFolderPath);
            return guid;
        }


        public static IList<FileInfo> GetFileInfoListByString(string uploadString)
        {
            var files = new List<FileInfo>();

            if (!string.IsNullOrWhiteSpace(uploadString))
            {

                string[] folderStrings = uploadString.Split(SeparatorChar); //  :
                foreach (var folderName in folderStrings)
                {
                    var dirPath = GetUploadFullPath(folderName);
                    var dirInfo = new DirectoryInfo(dirPath);
                    if (dirInfo.Exists)
                    {
                        FileInfo[] fileInfos = dirInfo.GetFiles();
                        // possible improvment: log if there is 0 or more than one file
                        files.AddRange(fileInfos);
                    }
                    //var fileInfo = new FileInfo(fullFilePath);
                    //files.Add(fileInfo);
                }
            }
            return files;
        }



        #endregion


        #region temporary upload
        [HttpGet]
        // [ValidateInput(false)]
        public ActionResult TempUpload(string fileIdentifier)
        {
            //---------------

            if (string.IsNullOrWhiteSpace(fileIdentifier))
            {
                return Content("");
            }

            string uploadString = "";
            try
            {
                StringValues isImageValue;
                var isImage = Request.Query.TryGetValue("isImage", out isImageValue) && isImageValue.Count > 0 && isImageValue[0] == "true";
                var response = Response;
                if (UploadChecker != null && !UploadChecker.CanSeeTempUpload(fileIdentifier, Request, User, isImage))
                {
                    throw new Exception(LocalizationCodes.MessageUnauthorized);
                }

                uploadString = fileIdentifier;
                FileInfo uploadFile = GetFileInfoListByString(uploadString).FirstOrDefault();
                if (uploadFile == null || !uploadFile.Exists)
                {
                    return Content("");
                }

                //response.Clear(); --> Net .. not supported in .net core
                if (isImage)
                {
                    // Bild zurückgeben
                    if (!Ch.Knomes.Drawing.PictureUtility.IsImage(uploadFile.FullName))
                    {
                        throw new Exception("Passed Image path does not refer to an image");
                    }


                    var imageFormat = Ch.Knomes.Drawing.PictureUtility.GetImageFormatFromName(uploadFile.FullName, Ch.Knomes.Drawing.ImageFormat.Jpeg);
                    var mimeType = Ch.Knomes.Drawing.PictureUtility.GetMimeContentString(imageFormat);//MimeMapping.GetMimeMapping(picName); // "image/jpeg"
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = uploadFile.FullName,
                        Inline = true,
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    var fileBytes = System.IO.File.ReadAllBytes(uploadFile.FullName);
                    return new FileContentResult(fileBytes, mimeType);

                    // possible improvment: support resizing for temp pictures (can be done client side)
                    //response.ContentType = MimeMapping.GetMimeMapping(uploadFile.FullName);// Ch.Knomes.Drawing.ImageUtility.GetImageMimeType(uploadFile.FullName);
                    //response.WriteFile(uploadFile.FullName);
                }
                else
                {
                    // File als Dokument zurückgeben
                    var fileNameOnly = Path.GetFileName(uploadFile.FullName);
                    // response.ContentType = "application/octet-stream";
                    //response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileNameOnly + "\""); // Filename mit Spaces muss in "" eingeschlossen sein (wegen FF)

                    // response.WriteFile(uploadFile.FullName);
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = uploadFile.FullName,
                        Inline = true,
                    };
                    var fileBytes = System.IO.File.ReadAllBytes(uploadFile.FullName);
                    return new FileContentResult(fileBytes, "application/octet-stream");
                }
            }
            catch (Exception ex)
            {
                if (uploadString == null)
                {
                    uploadString = ""; //prevent null pointer
                }
                Utils.Dev.Todo($"Improve errorhandling: {nameof(TempUpload)} " + ex);
                //  _logger.LogErrorEx("Error upon Upload for file (legacy): " + uploadString, ex);
                throw;
            }
        }

        #endregion


        #region saving file

        #region Hilfsmethoden Dateispeicherung und -prüfung

        /// <summary>
        /// Legt eine Datei an und stellt dabei sicher, dass der Name gültig ist und die Endung kompatibel mit dem Typ
        /// </summary>
        /// <param name="fileStream">Quell-Stream, nicht null</param>
        /// <param name="targetDir">Physischer Dateipfad des Zielordners</param>
        /// <param name="requestedFileName">
        /// Gewünschter Dateiname (wird ggf. angepasst, dies kann aus dem Rückgabewert erkannt werden)
        /// - leer und null wird durch "datei" ersetzt
        /// - Verzeichnisinformation wird entfernt
        /// - ungültige Zeichen gemäss System.IO.Path werden entfernt
        /// </param>
        /// <returns>Der Dateiname unter welchem die Datei angelegt wurde.</returns>
        /// <remarks>
        /// Macht ungefährt das, was Ch.Deimos SavePostedFile macht, aber im Dateisystem (die Namensregeln sind ausserdem weniger scharf)
        /// </remarks>
        public static string StreamToFile(Stream fileStream, string targetDir, string requestedFileName)
        {
            if (fileStream == null) throw new ArgumentNullException("fileStream");

            // Dateinamen gültig machen:
            var finalFileName = requestedFileName;
            // leer und null wird durch "datei" ersetzt
            if (String.IsNullOrEmpty(finalFileName))
            {
                finalFileName = "datei";
            }
            // Verzeichnisinformation wird entfernt
            finalFileName = Path.GetFileName(finalFileName);
            // Zeichen behandeln, die in Dateisystem-Pfaden ungültig sind
            finalFileName = PathUtility.StripInvalidFilenameCharacters(finalFileName);
            // Zeichen behandeln, die in URL-Pfaden ungültig sind
            // finalFileName = UrlUtility.StripInvalidRequestPathCharacters(finalFileName);

            // Separator
            finalFileName = finalFileName.Replace(SeparatorChar, '_'); // Separator char is probably already stripped at this point (just be on the safe side as depending on the environment, this is not 100% sure)
            // Typ bestimmen und ggf. Endung anpassen
            var extension = Path.GetExtension(finalFileName) + "";

            // TODO -> 
            // 2. Filename: extension to lower (.JPG to .jpg ). should work but doesnt :-(  12.1.2015)


            //var mimeType = FileUtility.GetMimeType(fileStream, extension);
            //finalFileName = mimeType.MakePathOfThisType(finalFileName, true, MimeTypeFileExtensionChangeMode.ReplaceWhenNormalizing);

            // Speichern
            var targetFilePath = Path.Combine(targetDir, finalFileName);
            using (var streamToTempFile = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
            {
                _readWriteStream(fileStream, streamToTempFile);
            }

            return finalFileName;
        }

        /// <summary>
        /// Kopiert Daten vom Lese in den Schreibstream
        /// </summary>
        /// <param name="readStream">Lese-Stream</param>
        /// <param name="writeStream">Schreib-Stream</param>
        private static void _readWriteStream(Stream readStream, Stream writeStream)
        {
            const int length = 256;
            var buffer = new Byte[length];
            var bytesRead = readStream.Read(buffer, 0, length);
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }
            writeStream.Flush();
        }

        ///// <summary>
        ///// Sicherstellen, dass es sich um ein Webbild handelt
        ///// </summary>
        ///// <param name="folderPath">Dateisystempfad des Zielordners</param>
        ///// <param name="fileName">Dateiname relativ zum Zielordner</param>
        ///// <returns>Neuer Name</returns>
        ///// <exception cref="Exception">Es ist kein Bild oder es ist ein sonstiger Fehler aufgetreten (ungefilterte Exceptions aus Ch.Deimos bzw. System.Drawing)</exception>
        //public static string EnsureWebImage(string folderPath, string fileName)
        //{
        //    var filePath = Path.Combine(folderPath, fileName);
        //    fileName = Ch.Knomes.Drawing.ImageUtility.ResizeImage(filePath, folderPath, 0, 0, true); // converts to web-image (example: tiff -> jpeg, and slightly adapts ending in some cases)
        //    return fileName;
        //}

        #endregion

        #endregion

        #region cleanup

        #region session cleanup (outcommented)

        //private const string UploadClearSessionKey = "uplKnomesClearSesRelFilexeke";

        //private void _addUploadToSessionForPossibleDeleteUponSessionEnd(HttpSessionStateBase session, string uniqueFileKey)
        //{
        //    if (CanClearUploadsUponSessionEnd && !string.IsNullOrWhiteSpace(uniqueFileKey))
        //    {
        //        string toStore = uniqueFileKey;

        //        var existing = session[UploadClearSessionKey] as string;
        //        if (!string.IsNullOrEmpty(existing))
        //        {
        //            toStore = existing + SeparatorChar + uniqueFileKey;
        //        }
        //        session[UploadClearSessionKey] = toStore;
        //    }
        //}

        ///// <summary>
        ///// Clears all Files created during the session, (plus very old files, depending on param)
        ///// -> preferably called upon SessionEnd
        ///// </summary>
        ///// <remarks>
        ///// a) to free resources as quickly as possible (there are other measures too, but its nice to have files removed as quickly as possible)
        ///// b) depends on flag that must be set to true, otherwise it will have no effect
        ///// </remarks>
        //public static void CleanFilesOfUserSession(HttpSessionStateBase session, bool alsoDeleteOldFiles = true)
        //{
        //    try
        //    {
        //        // Lösche temporäre Files die während der Session angelegt wurden
        //        _cleanupTemporaryUploadFilesForCurrentSession(session, alsoDeleteOldFiles);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Exception abfangen und loggen
        //        _logger.LogWarning("Cleanup temporary session files failed. ErrorMessage: {0}", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Cleans old temporary files AND explicitly all that were created during the current user session
        ///// </summary>
        //private static void _cleanupTemporaryUploadFilesForCurrentSession(HttpSessionStateBase session, bool alsoDeleteOldFiles)
        //{
        //    var uniqueFileKeys = session[UploadClearSessionKey] as string;
        //    if (!string.IsNullOrEmpty(uniqueFileKeys))
        //    {
        //        IList<FileInfo> files = GetFileInfoListByString(uniqueFileKeys);
        //        foreach (var file in files)
        //        {
        //            if (file.Exists)
        //            {
        //                file.Delete();
        //            }
        //        }
        //    }
        //    if (alsoDeleteOldFiles)
        //    {
        //        _cleanupOldTemporaryUploadFiles();
        //    }
        //}

        #endregion

        /// <summary>
        /// Löscht die alte, temporäre Files 
        /// </summary>
        private static void _cleanupOldTemporaryUploadFiles()
        {
            try
            {
                var duration = new TimeSpan(1, 5, 0);
                var dir = new DirectoryInfo(_tempUploadPath);
                if (!dir.Exists)
                {
                    return;
                }
                // Subfolders
                foreach (var subDir in dir.GetDirectories())
                {
                    _clearFoldersFromOldFileAndDirectories(subDir, duration, true);
                }

                // Diesen Folder überprüfen (wegen Dateien in diesem Folder)
                _clearFoldersFromOldFileAndDirectories(dir, duration, false);
            }
            catch (Exception ex)
            {
                Utils.Dev.Todo("better errorhandling for upload cleanup !" + ex.Message);
                // _logger.LogWarning("Cleanup old temporary files failed", ex.Message);
            }
        }

        /// <summary>
        /// Leert ein Directory von alten Dateien und Subfoldern (rekursiv)
        /// </summary>
        /// <param name="dir">Directory</param>
        /// <param name="duration">Zeitspanne die vergangen sein muss um Dateien/Folders zu löschen</param>
        /// <param name="deleteDirectoriesIfEmpty">Wenn true wird das Directory mitgelöscht</param>
        private static void _clearFoldersFromOldFileAndDirectories(DirectoryInfo dir, TimeSpan duration, bool deleteDirectoriesIfEmpty)
        {
            if (!dir.Exists)
            {
                return;
            }
            foreach (var fi in dir.GetFiles())
            {
                var creationTime = fi.CreationTime;
                if (creationTime < (DateTime.Now - duration))
                {
                    fi.Delete();
                }
            }
            foreach (var subDir in dir.GetDirectories())
            {
                _clearFoldersFromOldFileAndDirectories(subDir, duration, deleteDirectoriesIfEmpty);
            }

            if (deleteDirectoriesIfEmpty && dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
            {
                dir.Delete(false);
            }
        }


        #endregion
    }
}