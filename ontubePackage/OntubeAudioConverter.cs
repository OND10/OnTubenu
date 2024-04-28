using NReco.VideoConverter;
using ontubePackage.Common.Enum;
using ontubePackage.Common.Exception;
using ontubePackage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace ontubePackage
{
    public class OntubeAudioConverter
    {
        private readonly YoutubeClient _youtubeClient;
        private readonly extractVideoIdService _extractId;

        public OntubeAudioConverter()
        {
            _youtubeClient = new YoutubeClient();
            _extractId = new extractVideoIdService();
        }

        public async Task<Result<string>> ConvertToAudioAsync(string youtubeUrl, string outputFolderPath, string outputFileName)
        {
            try
            {
                // Get video ID from URL
                var videoId = await _extractId.ExtractYTubeId(youtubeUrl);

                // Ensure that the output folder exists, create it if necessary
                Directory.CreateDirectory(outputFolderPath);

                // Get the best audio stream
                var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId.Data);
                var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();
                if (audioStreamInfo == null)
                    return await Result<string>.FaildAsync(false, "No audio stream found.");

                // Download the audio stream

                var tempAudioFilePath = Path.Combine(outputFolderPath, $"{outputFileName}.{audioFormatEnum.mp4}");
                await _youtubeClient.Videos.Streams.DownloadAsync(audioStreamInfo, tempAudioFilePath);

                //Convert the audio to MP3 format
                var outputFilePath = Path.Combine(outputFolderPath, $"{outputFileName}.{audioFormatEnum.mp3}");
                var ffmpeg = new FFMpegConverter();
                ffmpeg.ConvertMedia(tempAudioFilePath, outputFilePath, $"{audioFormatEnum.mp3}");

                //Delete the temporary audio file
                File.Delete(tempAudioFilePath);

                return await Result<string>.SuccessAsync(outputFilePath, "File Uploaded Sucessfully", true);
            }
            catch (Exception)
            {
                // Handle errors
                return await Result<string>.FaildAsync(false, "File not uploaded");
            }
        }
        public async Task<Result<string>> DownloadVideoAsync(string youtubeUrl, string outputFolderPath, string outputFileName, int videoQuality)
        {
            try
            {
                int[] qualties = { 144, 240, 360, 480, 720, 1080 };
                foreach (int item in qualties)
                {
                    if (videoQuality == item)
                    {

                        // Get video ID from URL
                        var videoId = await _extractId.ExtractYTubeId(youtubeUrl);

                        // Ensure that the output folder exists, create it if necessary
                        Directory.CreateDirectory(outputFolderPath);

                        // Get the best audio stream
                        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId.Data);
                        //var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();

                        var videoStreamInfo = streamManifest.GetVideoStreams().Where(s => s.VideoQuality.MaxHeight == videoQuality).GetWithHighestBitrate();


                        var audioStreams = streamManifest.GetAudioOnlyStreams();
                        if (!audioStreams.Any())
                            return await Result<string>.FaildAsync(false, "No audio stream found.");

                        // Download the audio stream
                        var AudioFilePath = Path.Combine(outputFolderPath, $"{outputFileName}.{audioFormatEnum.mp4}");
                        await _youtubeClient.Videos.Streams.DownloadAsync(videoStreamInfo, AudioFilePath);


                        return await Result<string>.SuccessAsync(AudioFilePath, "File Uploaded Sucessfully", true);

                    }
                }

                return await Result<string>.FaildAsync(false, "Video Quality is not correct");

            }
            catch (Exception)
            {
                // Handle errors
                return await Result<string>.FaildAsync(false, "Try to change the name of file");
            }
        }
    }
}
