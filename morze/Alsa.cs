using System.Runtime.InteropServices;
using System.Runtime.Versioning;

class Alsa
{
    [DllImport("libasound.so.2")]
    public static extern int snd_pcm_open(out IntPtr handle, string name, int stream, int mode);

    [DllImport("libasound.so.2")]
    public static extern int snd_pcm_close(IntPtr handle);

    [DllImport("libasound.so.2")]
    public static extern int snd_pcm_writei(IntPtr handle, short[] buffer, int size);

    [DllImport("libasound.so.2")]
    public static extern int snd_pcm_prepare(IntPtr handle);

    [DllImport("libasound.so.2")]
    public static extern int snd_pcm_set_params(IntPtr handle, int format, int access, int channels, int rate, int soft_resample, int latency);

    [SupportedOSPlatform("linux")]
    public static void Beep(int frequency, int duration) 
    {
        const int sampleRate = 44100;
        const int channels = 1;
        // Generate sound wave buffer
        int bufferSize = sampleRate * duration / 1000;
        short[] buffer = new short[bufferSize];
        for (int i = 0; i < bufferSize; i++)
        {
            buffer[i] = (short)(Math.Sin(2 * Math.PI * frequency * i / sampleRate) * short.MaxValue);
        }
        // Open ALSA PCM device
        IntPtr handle;
        int err = snd_pcm_open(out handle, "default", 0, 0);
        if (err < 0)
        {
            Console.WriteLine("Error opening PCM device.");
            return;
        }

        // Set ALSA PCM parameters
        err = snd_pcm_set_params(handle, 2, 3, channels, sampleRate, 1, duration * 1000);
        if (err < 0)
        {
            Console.WriteLine("Error opening PCM device.");
            return;
        }

        // Prepare PCM device
        snd_pcm_prepare(handle);

        // Write the sound buffer to ALSA
        snd_pcm_writei(handle, buffer, buffer.Length);
        Thread.Sleep(duration);

        // Close ALSA PCM device
        snd_pcm_close(handle);
    }
}