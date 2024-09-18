using System.Runtime.InteropServices;
using System.Runtime.Versioning;

class PulseAudio
{
    // PulseAudio Stream flags
    const int PA_STREAM_PLAYBACK = 0x0001;
    
    // Sample format (16-bit PCM)
    const int PA_SAMPLE_S16LE = 3;

    // PulseAudio native API calls
    [DllImport("libpulse-simple.so.0")]
    public static extern IntPtr pa_simple_new(
        string? server, string name, int direction, string? dev, string stream_name,
        ref SampleSpec ss, IntPtr map, ref BufferAttr attr, out int error);

    [DllImport("libpulse-simple.so.0")]
    public static extern int pa_simple_write(IntPtr s, byte[] data, uint bytes, out int error);

    [DllImport("libpulse-simple.so.0")]
    public static extern int pa_simple_free(IntPtr s);

    // PulseAudio Sample specification
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleSpec
    {
        public int format;
        public int rate;
        public int channels;
    }

    public struct BufferAttr
    {
        public uint maxLength;
        public uint tlength;
        public uint prebuf;
        public uint minreq;
        public uint fragsize;
    }

    [SupportedOSPlatform("linux")]
    static public void Beep(int frequency, int duration) 
    {
        const int sampleRate = 44100; // CD quality sample rate
        const int channels = 1;       // Mono sound
        int bufferSize = sampleRate * duration / 1000; // Total number of samples

        // Create the PulseAudio sample specification
        SampleSpec ss;
        ss.format = PA_SAMPLE_S16LE;  // 16-bit little-endian PCM
        ss.rate = sampleRate;         // Sample rate
        ss.channels = channels;       // Mono sound
        BufferAttr ba;
        ba.maxLength = (uint) bufferSize * sizeof(short);
        ba.tlength = 1000;
        ba.prebuf = (uint) bufferSize * sizeof(short);
        ba.minreq = (uint) bufferSize * sizeof(short);
        ba.fragsize = (uint) bufferSize * sizeof(short);

        int error;

        // Open PulseAudio stream
        IntPtr stream = pa_simple_new(
            null,                // Default server
            "C# Sound",          // Our application name
            PA_STREAM_PLAYBACK,  // Playback stream
            null,                // Default device
            "Test Sound",        // Stream description
            ref ss,              // Sample specification
            IntPtr.Zero,         // Channel map
            ref ba,              // Buffer attributes
            out error);

        if (stream == IntPtr.Zero)
        {
            Console.WriteLine("Failed to connect to PulseAudio.");
            return;
        }

        // Generate a 1000 Hz sine wave buffer
        short[] buffer = new short[bufferSize];
        for (int i = 0; i < bufferSize; i++)
        {
            buffer[i] = (short)(Math.Sin(2 * Math.PI * frequency * i / sampleRate) * short.MaxValue);
        }

        // Convert the buffer to byte array for PulseAudio
        byte[] byteBuffer = new byte[buffer.Length * sizeof(short)];
        Buffer.BlockCopy(buffer, 0, byteBuffer, 0, byteBuffer.Length);

        // Write the buffer to PulseAudio stream
        if (pa_simple_write(stream, byteBuffer, (uint)byteBuffer.Length, out error) < 0)
        {
            Console.WriteLine("Failed to write to PulseAudio.");
            pa_simple_free(stream);
            return;
        }

        // Sleep for the duration to allow playback
        Thread.Sleep(duration);

        // Clean up and close the PulseAudio stream
        pa_simple_free(stream);
    }
}