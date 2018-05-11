using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TwainTest
{
    //class WIA_DPS_DOCUMENT_HANDLING_SELECT
    //{
    //    public const uint FEEDER = 0x00000001;
    //    public const uint FLATBED = 0x00000002;
    //}

    //class WIA_DPS_DOCUMENT_HANDLING_STATUS
    //{
    //    public const uint FEED_READY = 0x00000001;
    //}

    //class WIA_PROPERTIES
    //{
    //    public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024;
    //    public const uint WIA_DIP_FIRST = 2;
    //    public const uint WIA_DPA_FIRST = WIA_DIP_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
    //    public const uint WIA_DPC_FIRST = WIA_DPA_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
    //    //
    //    // Scanner only device properties (DPS)
    //    //
    //    public const uint WIA_DPS_FIRST = WIA_DPC_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
    //    public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = WIA_DPS_FIRST + 13;
    //    public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT = WIA_DPS_FIRST + 14;
    //}

    //class WIA_ERRORS
    //{
    //    public const uint BASE_VAL_WIA_ERROR = 0x80210000;
    //    public const uint WIA_ERROR_PAPER_EMPTY = BASE_VAL_WIA_ERROR + 3;
    //}


    public partial class Form1 : Form
    {
        string idEscaner;

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // parametros de scanner
                int resolucion = 150;
                int anchoImagen = 1200;
                int altoImagen = 1500;
                int modoColor = 1;

                // la mas importante
                bool HayMasPaginas = true; // tiene mas paginas :)

                // ruta principal donde se guardan los archivos
                string rutaBase = string.Format("{0}\\{1}", Application.StartupPath, DateTime.Now.ToString("yyyMMddhhmmss"));

                Directory.CreateDirectory(rutaBase);

                // elegir escanner
                WIA.CommonDialog dialogoSeleccionaEscanner = new WIA.CommonDialog();
                WIA.Device escaner = dialogoSeleccionaEscanner.ShowSelectDevice(WIA.WiaDeviceType.ScannerDeviceType, true, false);

                if (escaner != null)
                {
                    idEscaner = escaner.DeviceID;
                }
                else
                {
                    MessageBox.Show("No se eligió Escaner");
                    return;
                }

                int x = 0;
                int numeroPaginas = 0;

                while (HayMasPaginas)
                {
                    // mientras la bandera de mas paginas este activa
                    // conectar escaner
                    // mostrando avance de escaneo
                    WIA.CommonDialog dialogoEscaneo = new WIA.CommonDialogClass();
                    WIA.DeviceManager manager = new WIA.DeviceManagerClass();
                    WIA.Device dispoWIA = null;

                    // cargando el dispositivo
                    foreach (WIA.DeviceInfo scannerInfo in manager.DeviceInfos)
                    {
                        if (scannerInfo.DeviceID == idEscaner)
                        {
                            WIA.Properties infoprop = null;
                            infoprop = scannerInfo.Properties;

                            dispoWIA = scannerInfo.Connect();

                            break;
                        }
                    }

                    // inicia escaneo
                    WIA.ImageFile imgEscaneo = null;
                    WIA.Item escanerItem = dispoWIA.Items[1] as WIA.Item;

                    AdjustScannerSettings(escanerItem, resolucion, 0, 0, anchoImagen, altoImagen, 0, 0, modoColor);

                    // tomando la imagen
                    imgEscaneo = (WIA.ImageFile)dialogoEscaneo.ShowTransfer(escanerItem, WIA.FormatID.wiaFormatPNG, false);

                    string RutaNombreEscaneo = String.Format("{0}\\{1}.png", rutaBase, x.ToString());

                    if (File.Exists(RutaNombreEscaneo))
                    {
                        //file exists, delete it
                        File.Delete(RutaNombreEscaneo);
                    }

                    imgEscaneo.SaveFile(RutaNombreEscaneo);
                    numeroPaginas++;
                    x++;
                    imgEscaneo = null;

                    // preguntar si hay mas documentos pendientes de escaneo
                    if (MessageBox.Show("¿Deseas escanear otro documento?. Antes de seleccionar 'Yes' Por favor introduzca el documento a escanear.", "Escaner", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        HayMasPaginas = false;
                    }
                }

                // una vez escaneado los dosucmentos, se hace un pdf con ellos para despues hacerlo zip
                // rutaBase es el nombre de la variable de la carpeta
                MessageBox.Show("Escaneo finalizado. Armando archivo pdf");
                string nombreArchivoPdf = string.Format("{0}\\{1}.pdf", rutaBase, DateTime.Now.ToString("yyyMMddhhmmss"));

                Document pdfDoc = new Document(PageSize.LETTER, 2, 2, 2, 2);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(nombreArchivoPdf, FileMode.CreateNew));

                pdfDoc.Open();

                // tomar las imagenes del escaneo
                for (int i = 0; i < x; i++)
                {
                    // tomar la imagen
                    string rutaImagen = String.Format("{0}\\{1}.png", rutaBase, i.ToString());

                    PdfContentByte cb = writer.DirectContent;

                    using (FileStream fsSource = new FileStream(rutaImagen, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Image img = Image.GetInstance(fsSource);
                        img.ScaleToFit(pdfDoc.PageSize.Width, pdfDoc.PageSize.Height);
                        pdfDoc.Add(img);
                    }
                }

                pdfDoc.Close();

                MessageBox.Show("Archivo pdf generado. Generando archivo zip");

                // comprimir
                FileInfo pdfFileInfo = new FileInfo(nombreArchivoPdf);
                string nombreArchivoZip = string.Format("{0}\\{1}.zip", rutaBase, DateTime.Now.ToString("yyyMMddhhmmss"));
                ZipOutputStream zipStream = new ZipOutputStream(File.Create(nombreArchivoZip));
                zipStream.SetLevel(6);

                // abrir el archivo para escribirlo
                FileStream pdfFile = File.OpenRead(nombreArchivoPdf);
                byte[] buffer = new byte[pdfFile.Length];
                pdfFile.Read(buffer, 0, buffer.Length);

                // agregando el archivo a la carpeta comprimida
                ZipEntry entry = new ZipEntry(Path.GetFileName(nombreArchivoPdf))
                {
                    DateTime = pdfFileInfo.LastWriteTime,
                    Size = pdfFile.Length
                };

                pdfFile.Close();

                Crc32 objCrc32 = new Crc32();
                objCrc32.Reset();
                objCrc32.Update(buffer);

                entry.Crc = objCrc32.Value;
                zipStream.PutNextEntry(entry);
                zipStream.Write(buffer, 0, buffer.Length);

                zipStream.Finish();
                zipStream.Close();

                MessageBox.Show("Archivo zip generado. Enviando archivo por FTP");

                // enviar el documento en ftp

                MessageBox.Show("proceso completo.");
            }
            catch (Exception ex)
            {
                // cachando errores com
                if (ex is COMException)
                {
                    // Convert the error code to UINT
                    uint errorCode = (uint)((COMException)ex).ErrorCode;

                    // See the error codes
                    if (errorCode == 0x80210006)
                    {
                        MessageBox.Show("The scanner is busy or isn't ready");
                    }
                    else if (errorCode == 0x80210064)
                    {
                        MessageBox.Show("The scanning process has been cancelled.");
                    }
                    else if (errorCode == 0x8021000C)
                    {
                        MessageBox.Show("There is an incorrect setting on the WIA device.");
                    }
                    else if (errorCode == 0x80210005)
                    {
                        MessageBox.Show("The device is offline. Make sure the device is powered on and connected to the PC.");
                    }
                    else if (errorCode == 0x80210001)
                    {
                        MessageBox.Show("An unknown error has occurred with the WIA device.");
                    }
                }
                else
                {
                    MessageBox.Show("Error " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Adjusts the settings of the scanner with the providen parameters.
        /// </summary>
        /// <param name="scannnerItem">Scanner Item</param>
        /// <param name="scanResolutionDPI">Provide the DPI resolution that should be used e.g 150</param>
        /// <param name="scanStartLeftPixel"></param>
        /// <param name="scanStartTopPixel"></param>
        /// <param name="scanWidthPixels"></param>
        /// <param name="scanHeightPixels"></param>
        /// <param name="brightnessPercents"></param>
        /// <param name="contrastPercents">Modify the contrast percent</param>
        /// <param name="colorMode">Set the color mode</param>
        private static void AdjustScannerSettings(WIA.IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel, int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents, int colorMode)
        {
            const string WIA_SCAN_COLOR_MODE = "6146";
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_COLOR_MODE, colorMode);
        }

        private static void SetWIAProperty(WIA.IProperties properties, object propName, object propValue)
        {
            WIA.Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }
    }
}
