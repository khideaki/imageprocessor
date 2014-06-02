using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace imagepresser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int i = 0;

        private System.Drawing.Imaging.ImageCodecInfo
            GetEncoderInfo(string mineType)
        {
            System.Drawing.Imaging.ImageCodecInfo[] encs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
            {
                if (enc.MimeType == mineType)
                {
                    return enc;
                }
            }
            return null;
        }

        public void SaveImage(string fileName, long size,int no)
        {
            //画像ファイルを読み込む
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fileName);
 
            //イメージエンコーダに関する情報を取得する
            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

            try
            {


                //EncoderParameterオブジェクトを1つ格納できる
                //EncoderParametersクラスの新しいインスタンスを初期化
                //ここでは品質のみ指定するため1つだけ用意する
                System.Drawing.Imaging.EncoderParameters eps = getOptiumQuarity(size, bmp, ici);

                //ファイル名を取得
                string saveName = getSaveFileName(fileName, size, no);

                //保存する
                bmp.Save(saveName, ici, eps);

                bmp.Dispose();
                eps.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static string getSaveFileName(string fileName, long size, int no)
        {
            //フォルダ名をファイルサイズの数字でフォルダを作成する
            string dirName = System.IO.Path.GetDirectoryName(fileName) + "\\" + size.ToString() + "\\";

            if (System.IO.Directory.Exists(dirName) == false)
            {
                System.IO.Directory.CreateDirectory(dirName);
            }

            string saveName = dirName + String.Format("test{0}.jpg", no);
            return saveName;
        }

        private static System.Drawing.Imaging.EncoderParameters getOptiumQuarity(long size, System.Drawing.Bitmap bmp, System.Drawing.Imaging.ImageCodecInfo ici)
        {
            long quality = 100;

            //EncoderParameterオブジェクトを1つ格納できる
            //EncoderParametersクラスの新しいインスタンスを初期化
            //ここでは品質のみ指定するため1つだけ用意する
            System.Drawing.Imaging.EncoderParameters eps =
                new System.Drawing.Imaging.EncoderParameters(1);

            System.IO.MemoryStream sr = new System.IO.MemoryStream();
            do
            {
                sr.Flush();//Streamの中身をクリア
                sr = new System.IO.MemoryStream();

                //品質を指定
                System.Drawing.Imaging.EncoderParameter ep =
                    new System.Drawing.Imaging.EncoderParameter(
                    System.Drawing.Imaging.Encoder.Quality, (long)quality);

                //EncoderParametersにセットする
                eps.Param[0] = ep;

                //試しに保存する
                bmp.Save(sr, ici, eps);

                quality -= 5;

            } while (sr.Length > size);
            return eps;
        }

        //ImageFormatで指定されたImageCodecInfoを探して返す
        private System.Drawing.Imaging.ImageCodecInfo
            GetEncoderInfo(System.Drawing.Imaging.ImageFormat f)
        {
            System.Drawing.Imaging.ImageCodecInfo[] encs =
                System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
            {
                if (enc.FormatID == f.Guid)
                {
                    return enc;
                }
            }
            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.Multiselect = true;
            fd.ShowDialog();

            if (String.IsNullOrEmpty(fd.FileName))
                return;

            foreach (string fname in fd.FileNames)
            {
                i++;
                //300Mbyte
                SaveImage(fname, 100 * 1000,i);
            }
        }
    }
}
