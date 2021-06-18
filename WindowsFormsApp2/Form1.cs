using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        AForge.Video.DirectShow.VideoCaptureDevice videoSource;

        List<Image> lstImagem      = new List<Image>();
        List<byte[]> lstImagemByte = new List<byte[]>();

        Stopwatch stopwatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();

            AForge.Video.DirectShow.FilterInfoCollection videosources = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);

            if (videosources != null)
            {
                videoSource = new AForge.Video.DirectShow.VideoCaptureDevice(videosources[0].MonikerString);

                stopwatch.Start();

                Image oImage = null;

                videoSource.NewFrame += (s, e) =>
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }                   

                    //oImage = (Bitmap)e.Frame.Clone();

                    pictureBox1.Image = (Bitmap)e.Frame.Clone();

                    //if (chkIniciar.Checked)
                    //{
                    //    if (!ComparaImagem(oImage, (Bitmap)e.Frame.Clone()))
                    //    {
                    //        EnviaImagemServidor(imageToByteArrayJPG(oImage));
                    //    }
                    //}

                    //lstImagemByte.Add(imageToByteArrayJPG(oImage));

                    //lstImagem.Add((Bitmap)e.Frame.Clone());



                    if (!ComparaImagem(oImage, (Bitmap)e.Frame.Clone()))
                    {
                        //Bitmap image1 = AForge.Imaging.Image.(oImage);
                        //Bitmap image2 = AForge.Imaging.Image.FromFile(@"c:\temp\test2.jpg");

                        //// create filter
                        //ThresholdedDifference filter = new ThresholdedDifference(60);
                        //// apply the filter
                        //filter.OverlayImage = image1;

                        //Bitmap resultImage = filter.Apply(image2);


                        oImage = (Bitmap)e.Frame.Clone();

                        gravaImagem(oImage);
                    }

                    Application.DoEvents();
                };

                videoSource.Start();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void gravaImagem(Image Imagem)
        {
            Guid oGuid = Guid.NewGuid();

            //pictureBox1.Image.Save(@"c:\temp\" + oGuid + ".jpg", ImageFormat.Jpeg);

            Imagem.Save(@"c:\temp\" + oGuid + ".jpg", ImageFormat.Jpeg);

            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            //byte[] ar = new byte[ms.Length];
            //ms.Write(ar, 0, ar.Length);

        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image.Save(@"c:\temp\" + oGuid + ".jpg", ImageFormat.Jpeg);

            //string[] fileArray = Directory.GetFiles(@"c:\temp\", "*.jpg");

            //foreach (string file in fileArray)
            //{
            //    //FileStream stream = new FileStream(file, FileMode.Open);

            //    //pictureBox2.Image = (Bitmap)Bitmap.FromStream(stream);
            //    //pictureBox2.Image = (Bitmap)Image.FromFile(file);


            //    //FileStream fs = new System.IO.FileStream(file, FileMode.Open, FileAccess.Read);
            //    //pictureBox2.Image = (Bitmap)Image.FromStream(fs);
            //    //fs.Close();


            //    //ok
            //    //Thread.Sleep(20);
            //    //pictureBox2.Image = Image.FromFile(file);

            //    //ok
            //    //var image = Image.FromFile(file);
            //    //pictureBox2.Image = image;
            //    //System.Threading.Thread.Sleep(200); // delay by 0.2 seconds to see the transition 
            //    //Application.DoEvents();


            //    var image = Image.FromFile(file);
            //    pictureBox2.Image = image;
            //    System.Threading.Thread.Sleep(200); // delay by 0.2 seconds to see the transition 
            //    Application.DoEvents();
            //}


            //string[] fileArray = Directory.GetFiles(@"c:\temp\", "*.jpg");

            foreach (Image file in lstImagem)
            {
                //FileStream stream = new FileStream(file, FileMode.Open);

                //pictureBox2.Image = (Bitmap)Bitmap.FromStream(stream);
                //pictureBox2.Image = (Bitmap)Image.FromFile(file);


                //FileStream fs = new System.IO.FileStream(file, FileMode.Open, FileAccess.Read);
                //pictureBox2.Image = (Bitmap)Image.FromStream(fs);
                //fs.Close();


                //ok
                //Thread.Sleep(20);
                //pictureBox2.Image = Image.FromFile(file);

                //ok
                //var image = Image.FromFile(file);
                //pictureBox2.Image = image;
                //System.Threading.Thread.Sleep(200); // delay by 0.2 seconds to see the transition 
                //Application.DoEvents();


                //var image = Image.FromStream(file);
                pictureBox2.Image = file;
                System.Threading.Thread.Sleep(200); // delay by 0.2 seconds to see the transition 
                Application.DoEvents();

                //string base64String = Convert.ToBase64String(converterDemo(file));

                //Guid oGuid = Guid.NewGuid();

                //System.IO.File.WriteAllText(@"c:\temp\" + oGuid + ".txt", base64String);
            }
        }

        private bool ComparaImagem(Image imagem1, Image imagem2)
        {
            if (imagem1 == null)
                return  false;

            if (imagem2 == null)
                return  false;

            return Enumerable.SequenceEqual(GetImageByteArray(imagem1), GetImageByteArray(imagem2));
        }
        private byte[] GetImageByteArray(Image image)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                var byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, Convert.ToInt32(stream.Length));
                return byteArray;
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
                using (var ms = new MemoryStream())
                {
                    imageIn.Save(ms, imageIn.RawFormat);
                    return ms.ToArray();
                }
        }


        public byte[] imageToByteArrayJPG(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            base.OnClosed(e);

            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
            }

            stopwatch.Stop();

            var tempoToal = stopwatch.Elapsed;

            //foreach (byte[] x in lstImagemByte)
            //{
            //    Console.WriteLine(x.LongLength);
            //}

        }

        //public static Task<HttpResponseMessage> PostFormDataAsync<T>(this HttpClient httpClient, string url, string token, T data)
        //{
        //    var content = new MultipartFormDataContent();

        //    foreach (var prop in data.GetType().GetProperties())
        //    {
        //        var value = prop.GetValue(data);
        //        if (value is FormFile)
        //        {
        //            var file = value as FormFile;
        //            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
        //            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = prop.Name, FileName = file.FileName };
        //        }
        //        else
        //        {
        //            content.Add(new StringContent(JsonConvert.SerializeObject(value)), prop.Name);
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(token))
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    return httpClient.PostAsync(url, content);
        //}

        private async Task<Stream> ReceberImagemdoServidor()
        {
            Guid oGuid = Guid.NewGuid();

            //instance of HTTPClient
            HttpClient client = new HttpClient();

            //send  request asynchronously
            //HttpResponseMessage response = await client.GetAsync("https://localhost:44323/weatherforecast/ENVIAIMAGEM");
            HttpResponseMessage response = await client.GetAsync("http://prorsoft.com.br.thor.hostazul.com.br/weatherforecast/ENVIAIMAGEM");

            // Check that response was successful or throw exception
            //response.EnsureSuccessStatusCode();

            MemoryStream oMemory = new MemoryStream();

            //copy the content from response to filestream
            await response.Content.CopyToAsync(oMemory);

            pictureBox2.Image = Image.FromStream(oMemory);
            //System.Threading.Thread.Sleep(200); // delay by 0.2 seconds to see the transition 
            Application.DoEvents();

            return null;
        }

        //private async Task<Stream> ReceberImagemdoServidor()
        //{
        //    Guid oGuid = Guid.NewGuid();

        //    //instance of HTTPClient
        //    HttpClient client = new HttpClient();

        //    //send  request asynchronously
        //    HttpResponseMessage response = await client.GetAsync("https://localhost:44323/weatherforecast");

        //    // Check that response was successful or throw exception
        //    //response.EnsureSuccessStatusCode();

        //    MemoryStream oMemory = new MemoryStream();

        //    // Read response asynchronously and save asynchronously to file
        //    //using (FileStream fileStream = new FileStream(@"C:\TEMP\entrada2\" + oGuid + ".JPG", FileMode.Create, FileAccess.Write, FileShare.None))
        //    //{
        //    //copy the content from response to filestream
        //    await response.Content.CopyToAsync(oMemory);


        //    //byte[] buff = ConverteStreamToByteArray(stream1);


        //    pictureBox2.Image = Image.FromStream(oMemory);
        //    //System.Threading.Thread.Sleep(200); // delay by 0.2 seconds to see the transition 
        //    Application.DoEvents();

        //    //return fileStream;
        //    //}
        //}

        private async void EnviaImagemServidor(byte[] dados)
        {
            //const string url = "https://localhost:44323/weatherforecast/RECEBEIMAGEM";
            const string url = "http://prorsoft.com.br.thor.hostazul.com.br/weatherforecast/RECEBEIMAGEM";

            //const string filePath = @"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg";

            using (var httpClient = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    using (var fileContent = new ByteArrayContent(dados))
                    {
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                        form.Add(fileContent, "arquivo", "arquivo.jpg");

                        HttpResponseMessage response = await httpClient.PostAsync(url, form);
                    }
                }
            }
        }


        private async void button3_Click(object sender, EventArgs e)
        {
            const string url = "https://localhost:44323/weatherforecast";
            const string filePath = @"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg";

            //using (var httpClient = new HttpClient())
            //{

            //    //const string fileName = "csvFile.csv";
            //    //var filePath = Path.Combine("IntegrationTests", fileName);

            //    var bytes = File.ReadAllBytes(filePath);

            //    var form = new MultipartFormDataContent();
            //    var content = new StreamContent(new MemoryStream(bytes));

            //    form.Add(content, "1ac3ed5b-8fb1-47d5-96a5-f25e8913397f");
            //    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            //    {
            //        Name = "1ac3ed5b-8fb1-47d5-96a5-f25e8913397f",
            //        FileName = filePath
            //    };
            //    content.Headers.Remove("Content-Type");
            //    content.Headers.Add("Content-Type", "application/octet-stream");
            //    form.Add(content);

            //    //Act
            //    var postResponse = await httpClient.PostAsync(url, form);
            //}

            //using (var httpClient = new HttpClient())
            //{
            //        var bytes = File.ReadAllBytes(filePath);

            //        var form = new MultipartFormDataContent();
            //        var content = new StreamContent(new MemoryStream(bytes));

            //    var multipartContent = new MultipartFormDataContent();
            //    multipartContent.Add(content, "arquivo", "arquivo.jpg");
            //    var postResponse = await httpClient.PostAsync(url, multipartContent);

            //}

            using (var httpClient = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    using (var fs = File.OpenRead(filePath))
                    {
                        using (var streamContent = new StreamContent(fs))
                        {
                            using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                            {
                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                                form.Add(fileContent, "arquivo", "arquivo.jpg");

                                HttpResponseMessage response = await httpClient.PostAsync(url, form);

                            }
                        }
                    }
                }
            }


        }


        //public static Task ReadAsFileAsync(this HttpContent content, string filename, bool overwrite)
        //{
        //    string pathname = Path.GetFullPath(filename);
        //    if (!overwrite && File.Exists(filename))
        //    {
        //        throw new InvalidOperationException(string.Format("File {0} already exists.", pathname));
        //    }

        //    FileStream fileStream = null;
        //    try
        //    {
        //        fileStream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None);
        //        return content.CopyToAsync(fileStream).ContinueWith(
        //            (copyTask) =>
        //            {
        //                fileStream.Close();
        //            });
        //    }
        //    catch
        //    {
        //        if (fileStream != null)
        //        {
        //            fileStream.Close();
        //        }

        //        throw;
        //    }
        //}

        //string requestString = @"https://example.com/path/file.pdf";

        //var GetTask = httpClient.GetAsync(requestString);
        //GetTask.Wait(WebCommsTimeout); // WebCommsTimeout is in milliseconds

        //        if (!GetTask.Result.IsSuccessStatusCode)
        //        {
        //            // write an error
        //            return;
        //        }

        //        using (var fs = new FileStream(@"c:\path\file.pdf", FileMode.CreateNew))
        //        {
        //            var ResponseTask = GetTask.Result.Content.CopyToAsync(fs);
        //ResponseTask.Wait(WebCommsTimeout);
        //        }





    private async void button2_Click(object sender, EventArgs e)
        {
            Guid oGuid = Guid.NewGuid();

            //instance of HTTPClient
            HttpClient client = new HttpClient();

            //send  request asynchronously
            HttpResponseMessage response = await client.GetAsync("https://localhost:44323/weatherforecast");

            // Check that response was successful or throw exception
           //response.EnsureSuccessStatusCode();

            // Read response asynchronously and save asynchronously to file
            using (FileStream fileStream = new FileStream(@"C:\TEMP\entrada2\" + oGuid + ".JPG", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //copy the content from response to filestream
                await response.Content.CopyToAsync(fileStream);


            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (chkIniciar.Checked)
            {
                ReceberImagemdoServidor();
            }
        }
    }
}
