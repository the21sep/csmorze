class Sound
{
    public static void Beep(int frequency, int duration)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Console.Beep(frequency, duration);
        }
        else if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            try 
            {
                Alsa.Beep(frequency, duration);
                return;
            } catch (System.DllNotFoundException) {}
            try {
                PulseAudio.Beep(frequency, duration);
                return;
            } catch (System.DllNotFoundException) {}
        }
    }
}