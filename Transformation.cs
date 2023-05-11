using System;
using System.IO;


namespace Mobile_app
{
    class Transformation
    {
        private static System.Timers.Timer aTimer;
        public static bool ReadWavFile(string filename, out float[] L, out float[] R, out int sampleRate)
        {
            L = null;
            R = null;
            sampleRate = 0;
            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    BinaryReader reader = new BinaryReader(fs);

                    // chunk 0
                    int chunkID = reader.ReadInt32();
                    int fileSize = reader.ReadInt32();
                    int riffType = reader.ReadInt32();


                    // chunk 1
                    int fmtID = reader.ReadInt32();
                    int fmtSize = reader.ReadInt32(); // bytes for this chunk
                    int fmtCode = reader.ReadInt16();
                    int channels = reader.ReadInt16();
                    sampleRate = reader.ReadInt32();
                    int byteRate = reader.ReadInt32();
                    int fmtBlockAlign = reader.ReadInt16();
                    int bitDepth = reader.ReadInt16();

                    if (fmtSize == 18)
                    {
                        // Read any extra values
                        int fmtExtraSize = reader.ReadInt16();
                        reader.ReadBytes(fmtExtraSize);
                    }


                    // chunk 2 -- HERE'S THE NEW STUFF (ignore these subchunks, I don't know what they are!)
                    int bytes;
                    while (new string(reader.ReadChars(4)) != "data")
                    {
                        bytes = reader.ReadInt32();
                        reader.ReadBytes(bytes);
                    }

                    // DATA!
                    bytes = reader.ReadInt32();
                    byte[] byteArray = reader.ReadBytes(bytes);

                    int bytesForSamp = bitDepth / 8;
                    int samps = byteArray.Length / bytesForSamp;


                    float[] asFloat = null;
                    switch (bitDepth)
                    {
                        case 64:

                            double[]
                            asDouble = new double[samps];
                            Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes);
                            asFloat = Array.ConvertAll(asDouble, e => (float)e);
                            break;
                        case 32:

                            asFloat = new float[samps];
                            Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
                            break;
                        case 16:
                            Int16[]
                            asInt16 = new Int16[samps];
                            Buffer.BlockCopy(byteArray, 0, asInt16, 0, byteArray.Length);
                            asFloat = Array.ConvertAll(asInt16, e => e / (float)Int16.MaxValue);
                            break;
                        default:
                            return false;
                    }

                    switch (channels)
                    {

                        case 1:
                            L = asFloat;
                            R = null;
                            return true;
                        case 2:
                            L = new float[samps];
                            R = new float[samps];
                            for (int i = 0, s = 0; i < samps; i++)
                            {
                                L[i] = asFloat[s++];
                                R[i] = asFloat[s++];
                            }
                            return true;
                        default:
                            return false;
                    }
                }
            }
            catch
            {
                return false;
                //left = new float[ 1 ]{ 0f };
            }

        }


        public static void Processing(string path, out double[] f, out double[] p, int min, int max)
        {
            ReadWavFile(path, out float[] left, out float[] right, out int sampleRate);
            int a = 1;
            while (a <= left.Length)
            {
                a = a * 2;
            }
            a = a / 2;
            if (a<16384)
            {
               f=null; p=null;return;
            }
            double[] signal = new double[a]; // Смещение байтов в wav-файле;

            for (int i = 0; i < signal.Length; i++)
            {
                signal[i] = left[i];
            }

            //фильтрация
            double[] filtered = FftSharp.Filter.BandPass(signal, sampleRate, minFrequency: min, maxFrequency: max);
            //окное преобразование
            var window = new FftSharp.Windows.Hanning();
            double[] windowed = window.Apply(filtered);

            double[] psd = FftSharp.Transform.FFTpower(windowed);
            //ср. знач. psd
            double mainValue = 0;
            for (int i = 0; i < psd.Length; i++)
            {
                if (!Double.IsInfinity(psd[i]))
                {
                    mainValue += psd[i];
                }
            }
            mainValue = mainValue / psd.Length;
            //избавляемся от бесконечностей после фильтрации
            for (int i = 0; i < psd.Length; i++)
            {
                if (Double.IsInfinity(psd[i]))
                {
                    psd[i] = mainValue;
                }
            }

            double[] freq = FftSharp.Transform.FFTfreq(sampleRate, psd.Length);


            int count = 0;//кол-во частот в диапазоне
            for (int i = 0; i < freq.Length; i++)
            {
                if (freq[i] < 1100 && freq[i] > 10)
                {
                    count++;
                }
            }
            //создание массивов с нужными частотами
            double[] power = new double[count];
            double[] frequency = new double[count];
            count = 0;
            for (int i = 0; i < freq.Length; i++)
            {
                if (freq[i] < 1100 && freq[i] > 10)
                {
                    power[count] = psd[i];
                    frequency[count] = freq[i];
                    count++;
                }
            }



            p = power;
            f = frequency;
        }
        public static void FindFreq(double[] p, double[] f, out double[] freq, out double[] power, out double[] PeaksFrequency)
        {
            int countOfPeaks = 2;
            double[] valuePeaks = new double[3];
            double maxPower;
            int index = 0;
            int k = 0;
            for (int i = 0; i < countOfPeaks; i++)
            {
                maxPower = -100000;
                for (int j = 0; j < p.Length; j++)
                {
                    if (p[j] > maxPower)
                    {
                        maxPower = p[j];
                        index = j;
                    }
                }
                valuePeaks[i] = f[index];
                k = index;
                while (f[k] < f[index] + 30)
                {
                    p[k] = -100f;
                    k++;
                }
                k = index;
                while (f[index] - 30 < f[k])
                {
                    p[k] = -100f;
                    k--;
                }
                p[index] = -200f;

            }
            freq = f;
            power = p;
            PeaksFrequency = valuePeaks;


        }
        public static void FindMainFreq(double[] PeaksFrequency, out double mainFreq)
        {
            Array.Sort(PeaksFrequency);
            double razn1 = PeaksFrequency[1] - PeaksFrequency[0];
            double razn2 = PeaksFrequency[2] - PeaksFrequency[1];


            for (int i = 0; i < PeaksFrequency.Length; i++)
            {
                if ((PeaksFrequency[i] >= razn1 - 2f && PeaksFrequency[i] <= razn1 + 2f) || (PeaksFrequency[i] >= razn2 - 2f && PeaksFrequency[i] <= razn2 + 2f))
                {
                    mainFreq = PeaksFrequency[i];
                    return;
                }
            }
            mainFreq = 0;


        }


    }
}
