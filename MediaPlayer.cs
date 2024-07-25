using Figgle;
using System;

namespace State_Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var player = new MediaPlayer();

            while (true)
            {
                player.DrawPlayer();
                var key = Console.ReadKey().Key;
                Console.WriteLine();

                switch (key)
                {
                    case ConsoleKey.P:
                        player.Play();
                        break;
                    case ConsoleKey.O:
                        player.Pause();
                        break;
                    case ConsoleKey.S:
                        player.Stop();
                        break;
                    case ConsoleKey.I:
                        player.InsertMedia();
                        break;
                    case ConsoleKey.R:
                        player.RemoveMedia();
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid key. Please try again.");
                        break;
                }
            }
        }
    }

    public class MediaPlayer
    {
        public MediaPlayer() => State = new IdleNoMedia(this);
        public MediaPlayerState State { get; set; }
        public string StateMessage => State.StateMessage;
        public string MediaFile => State.MediaFile;
        public void Play() => State.Play();
        public void Pause() => State.Pause();
        public void Stop() => State.Stop();
        public void InsertMedia() => State.InsertMedia();
        public void RemoveMedia() => State.RemoveMedia();
        public void DrawPlayer()
        {
            Console.Clear();

            Console.WriteLine(FiggleFonts.Standard.Render("Media Player"));
            Console.WriteLine("=======================================");
            Console.WriteLine($"Current State: {StateMessage}");
            Console.WriteLine($"Current Media File: {MediaFile}");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("CONTROLS:");
            Console.WriteLine("'p' to Play");
            Console.WriteLine("'o' to Pause");
            Console.WriteLine("'s' to Stop");
            Console.WriteLine("'r' to Remove Media");
            Console.WriteLine("'i' to Insert Media");
            Console.WriteLine("or 'q' to Quit");
            Console.WriteLine("=======================================");
        }
    }

    public abstract class MediaPlayerState
    {
        protected MediaPlayer _mediaPlayer;
        public MediaPlayerState(MediaPlayer mediaPlayer) => _mediaPlayer = mediaPlayer;
        public abstract string StateMessage { get; set; }
        public abstract string MediaFile { get; set; }
        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();
        public abstract void InsertMedia();
        public abstract void RemoveMedia();
    }

    public class IdleNoMedia : MediaPlayerState
    {
        private string _stateMessage = "No media found.";
        private string _mediaFile;

        public IdleNoMedia(MediaPlayer mediaPlayer) : base(mediaPlayer)
        {
            _mediaFile = string.Empty;
        }

        public override string StateMessage
        {
            get => _stateMessage;
            set => _stateMessage = value;
        }

        public override string MediaFile
        {
            get => _mediaFile;
            set => _mediaFile = value;
        }

        public override void Play()
        {
            StateMessage = "Please insert media before playing.";
        }

        public override void Pause()
        {
            StateMessage = "Cannot pause: NOT PLAYING.";
        }

        public override void Stop()
        {
            StateMessage = "Cannot stop: NOT PLAYING.";
        }

        public override void InsertMedia()
        {
            Console.Write("Input a string to represent media: ");
            MediaFile = Console.ReadLine();
            _mediaPlayer.State = new MediaStopped(_mediaPlayer);
        }

        public override void RemoveMedia()
        {
            StateMessage = "Error: No media inserted.";
        }
    }

    public class MediaStopped : MediaPlayerState
    {
        private string _stateMessage = "Media has stopped.";
        private string _mediaFile;

        public MediaStopped(MediaPlayer mediaPlayer) : base(mediaPlayer) 
        { 
            _mediaFile = mediaPlayer.MediaFile;
        }

        public override string StateMessage
        {
            get => _stateMessage;
            set => _stateMessage = value;
        }

        public override string MediaFile
        {
            get => _mediaFile;
            set => _mediaFile = value;
        }

        public override void Play()
        {
            _mediaPlayer.State = new MediaPlaying(_mediaPlayer);
        }

        public override void Pause()
        {
            StateMessage = "Media already stopped.";
        }

        public override void Stop()
        {
            StateMessage = "Media already stopped.";
        }

        public override void InsertMedia()
        {
            StateMessage = "Error: Media already inserted.";
        }

        public override void RemoveMedia()
        {
            MediaFile = string.Empty;
            _mediaPlayer.State = new IdleNoMedia(_mediaPlayer);
        }
    }

    public class MediaPlaying : MediaPlayerState
    {
        private string _stateMessage = "Media playing.";
        private string _mediaFile;

        public MediaPlaying(MediaPlayer mediaPlayer) : base(mediaPlayer) 
        {
            _mediaFile = mediaPlayer.MediaFile;
        }

        public override string StateMessage
        {
            get => _stateMessage;
            set => _stateMessage = value;
        }

        public override string MediaFile
        {
            get => _mediaFile;
            set => _mediaFile = value;
        }

        public override void Play()
        {
            StateMessage = "Error, media already playing.";
        }

        public override void Pause()
        {
            _mediaPlayer.State = new MediaPaused(_mediaPlayer);
        }

        public override void Stop()
        {
            _mediaPlayer.State = new MediaStopped(_mediaPlayer);
        }

        public override void InsertMedia()
        {
            StateMessage = "Please stop and remove media before inserting new.";
        }

        public override void RemoveMedia()
        {
            StateMessage = "Please stop and remove media before inserting new.";
        }
    }

    public class MediaPaused : MediaPlayerState
    {
        private string _stateMessage = "Media is paused.";
        private string _mediaFile;

        public MediaPaused(MediaPlayer mediaPlayer) : base(mediaPlayer) 
        {
            _mediaFile = mediaPlayer.MediaFile;
        }

        public override string StateMessage
        {
            get => _stateMessage;
            set => _stateMessage = value;
        }

        public override string MediaFile
        {
            get => _mediaFile;
            set => _mediaFile = value;
        }

        public override void Play()
        {
            _mediaPlayer.State = new MediaPlaying(_mediaPlayer);
        }

        public override void Pause()
        {
            StateMessage = "Media already paused.";
        }

        public override void Stop()
        {
            _mediaPlayer.State = new MediaStopped(_mediaPlayer);
        }

        public override void InsertMedia()
        {
            StateMessage = "Please stop and remove media before inserting new.";
        }

        public override void RemoveMedia()
        {
            StateMessage = "Please stop and remove media before inserting new.";
        }
    }
}

