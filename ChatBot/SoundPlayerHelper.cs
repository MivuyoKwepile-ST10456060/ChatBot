using System;
using System.IO;
using NAudio.Wave;

namespace Chatbot
{
    public static class SoundPlayerHelper
    {
        public static void PlayGreeting(string filePath)
        {
            if (!File.Exists(filePath))
            {
                ConsoleHelper.WriteLineColored(
                    $"Warning: Voice greeting file '{filePath}' not found. Continuing without audio.",
                    ConsoleColor.Yellow);
                return;
            }

            try
            {
                var audioFile = new AudioFileReader(filePath);
                var outputDevice = new WaveOutEvent();

                outputDevice.Init(audioFile);
                outputDevice.Play();

                // Dispose after playback finishes (background)
                outputDevice.PlaybackStopped += (s, e) =>
                {
                    outputDevice.Dispose();
                    audioFile.Dispose();
                };
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteLineColored($"Error playing audio: {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}