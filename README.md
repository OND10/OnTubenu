# OnTube

![Status](https://img.shields.io/badge/status-maintenance-ffd700.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/OnTube)

<p align="center">
  <img src='ontubePackage/assets/ontube.png' alt='Icon'>
</p>


**OnTube** is a library that provides an interface to easily convert YouTube videos to audio files and download them with any resolution.
Behind a layer of abstraction, this library works by scraping raw page data and exploiting reverse-engineered internal endpoints.

**Extension packages**:

- [OnTube.Converter](OnTube.Converter) — provides the ability to download and convert videos using FFmpeg.


You are free by using this project or its source code, for any purpose that will improve this package for future.


### Installation

OnTube is available on [NuGet](https://www.nuget.org/packages/OnTube/). Install the provider package. See the in the docs for additional downloads.

```sh
dotnet add package OnTube --version 1.0.0
```

### Download package using nuget
```
Only search for OnTube and please ignore this -> Ond.Util.OnTube
```

## Usage

**OnTube** exposes its functionality through a single entry point — the `OntubeAudioConverter` class.
Create an instance of this class and use the provided operations on `Videos` and `Audio` properties to send requests.

### Videos

To download the video from YouTube. 

```csharp
using ontubePackage;

var youtube = new OntubeAudioConverter();

// You can specify the video URL 
var videoUrl = "https://www.youtube.com/watch?v=BcmUqwC_2Uc";
// You can specify the Folder path for example in D 
var folderPath = @"D:\music";
//You can specify the file name as you like
var fileName = "song1";
//Finally, you can specify the resolution of video
int quality = 360;
 var downloadedVideo = await youtube.DownloadVideoAsync(videoUrl, folderPath, fileName, quality);
 //check if every thing is going well
if (downloadedVideo.IsSuccess != false)
{
    Console.WriteLine($"Audio file saved at: {downloadedVideo.Data}");
}
else
{
    Console.WriteLine($"{downloadedVideo.Message}");
}

```

#### Downloading video streams

Every YouTube video has a number of streams available, differing in containers, video quality, bitrate, framerate, and other parameters.

> **Warning**:
> Muxed streams contain both audio and video, but these streams are limited in quality (up to 720p30).
> Some videos when you download them in a low quality you got a video without sound so, try to download with high quality.


### Audios

To convert the YT video to audio. 

```csharp
using ontubePackage;

var youtube = new OntubeAudioConverter();

// You can specify the video URL 
var videoUrl = "https://www.youtube.com/watch?v=BcmUqwC_2Uc";
// You can specify the Folder path for example in D 
var folderPath = @"D:\music";
//You can specify the file name as you like
var fileName = "song1";

 var convertedAudio = await youtube.ConvertToAudioAsync(videoUrl, folderPath, fileName);
 
 //check if every thing is going well
if (convertedAudio.IsSuccess != false)
{
    Console.WriteLine($"Audio file saved at: {convertedAudio.Data}");
}
else
{
    Console.WriteLine($"{convertedAudio.Message}");
}


```


### Use it with MVC Apps


```csharp
using ontubePackage;

// This is the controller
public class HomeController : Controller
{
    private readonly OntubeAudioConverter _ontube;
    public HomeController(OntubeAudioConverter ontube)
    {
        _ontube = ontube;      
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>Download(string url)
    {
        var folderPath =@"D:\visualstudiocodes\Csharp\Test_Template\wwwroot\music";
        //You can specify the file name as you like
        var fileName = "song1";

        await _ontube.DownloadVideoAsync(url, folderPath, fileName, 360);

        return View("Index");
    }
}


//This is the view Download.cshtml
 <form asp-action="Download" asp-controller="Home" method="post">
     <input type="text"  name="url"/>
     <div>
         <button type="submit">Download</button>
     </div>
 </form>


// This is the program.cs
builder.Services.AddScoped<OntubeAudioConverter>(); 

```


Once the audio is obtained, you can find it in the purposed folder with a format of type mp3.

In the end don't hesitate to share your ability in developing this packge.

## 😁 Contributing

Pull requests are welcome but before that please open an issue first to discuss what you want to change.
please follow me for more C# blogs [linkedin](https://www.linkedin.com/in/osama-dammag-%F0%9F%87%B5%F0%9F%87%B8-b40739221/)


## 📎 License

This project is licensed under the [MIT License](https://github.com/OND10/OnTubenu?tab=MIT-1-ov-file).

