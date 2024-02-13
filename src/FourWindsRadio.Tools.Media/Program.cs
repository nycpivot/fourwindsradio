using Azure.Storage.Blobs;
using System.Diagnostics;
using System.Net;
using FourWindsRadio.Tools.Media;
using System.Net.NetworkInformation;

var connectionString = "DefaultEndpointsProtocol=https;AccountName=fourwindsradio;AccountKey=vN4jcYe2g92UwaJcF9xxqzlmOnW8wjJObVHMFprrj9YG+0t21ABysOEgjdoyFZiAX3zRXnducf1B+ASteqheYw==;EndpointSuffix=core.windows.net";

var blobServiceClient = new BlobServiceClient(connectionString);

var homeDirectory = new DirectoryInfo(Environment.GetEnvironmentVariable("HOME"));

var mediaDirectory = homeDirectory.CreateSubdirectory("media");
var catalogDirectory = mediaDirectory.CreateSubdirectory("catalog");
var playlistDirectory = mediaDirectory.CreateSubdirectory("playlist");

var playlist = new FileInfo(Path.Combine(playlistDirectory.FullName, "playlist.txt"));
var writer = playlist.AppendText();

const string KJV_CHRISTOPHER = "mp3-bible-kjv-chapters-christopher";
const string KJV_ALEXANDER_SCOURBY = "mp3-bible-kjv-alexander-scourby";
const string KJV_AUDIO_TREASURE = "mp3-bible-kjv-audio-treasure";
const string DEVOTIONS = "mp3-devotions";
const string CLASSICAL = "mp3-classical";
const string MASTERPIECES = "media-audio-mp3-classical-millenium-masterpieces";
const string MISCELLANEOUS = "media-audio-mp3-classical-miscellaneous";
const string HYMNS = "media-audio-mp3-hymns";
const string THEMES = "media-audio-mp3-themes";


DownloadBlobs();

void DownloadBlobs()
{
    var containers = GetContainers();

    foreach (var container in containers)
    {
        var containerClient1 = blobServiceClient.GetBlobContainerClient(container);

        var containerDirectory = catalogDirectory.CreateSubdirectory(container);

        foreach (var blob in containerClient1.GetBlobs())
        {
            Console.WriteLine($"{container}/{blob.Name}");

            var blobUrl = $"{containerClient1.Uri}/{blob.Name}";

            var webClient = new WebClient();
            var file = webClient.DownloadData(blobUrl);

            File.WriteAllBytes(Path.Combine(containerDirectory.FullName, blob.Name), file);
        }
    }

    Console.WriteLine("DONE");
    Console.WriteLine();
    //Console.ReadLine();
}




//const string catalog = "~/media/catalog"; // @"C:\Projects\fourwindsradio\catalog";
//const string playlist = "~/media/playlist"; // @"C:\Projects\fourwindsradio\playlist";

Random random = new Random();

var hymns = new List<FileInfo>();
var devotions = new List<FileInfo>();
var classical = new List<FileInfo>();
var treasure = new List<FileInfo>();
var miscellaneous = new List<FileInfo>();
var scourby = new List<FileInfo>();
var masterpieces = new List<FileInfo>();


//DownloadFiles();

//void DownloadFiles()
//{
//    //var connectionString = "";

//    var blobServiceClient = new BlobServiceClient(connectionString);

//    var containers = GetContainers();

//    foreach (var container in containers)
//    {
//        var containerClient1 = blobServiceClient.GetBlobContainerClient(container);

//        foreach (var blob in containerClient1.GetBlobs())
//        {
//            Console.WriteLine($"{container}/{blob.Name}");

//            var blobUrl = $"{containerClient1.Uri}/{blob.Name}";

//            var webClient = new WebClient();
//            var file = webClient.DownloadData(blobUrl);

//            if (!Directory.Exists(@$"{catalog}\{container}"))
//            {
//                Directory.CreateDirectory(@$"{catalog}\{container}");
//            }

//            File.WriteAllBytes(@$"{catalog}\{container}\{blob.Name}", file);
//        }
//    }

//    Console.WriteLine("DONE");
//    Console.ReadLine();
//}





//LoadHymnsCatalog();
//LoadDevotionsCatalog();
//LoadClassicalCatalog();
//LoadTreasureCatalog();
//LoadMiscellaneousCatalog();
//LoadScourbyCatalog();
//LoadMasterpiecesCatalog();

//void LoadHymnsCatalog()
//{
//    foreach (var file in Directory.GetFiles(Path.Combine(catalog, HYMNS)))
//    {
//        hymns.Add(new FileInfo(file));
//    }
//}

//void LoadDevotionsCatalog()
//{
//    foreach (var file in Directory.GetFiles(Path.Combine(catalog, DEVOTIONS)))
//    {
//        devotions.Add(new FileInfo(file));
//    }
//}

void LoadClassicalCatalog()
{
    var classicalDirectory = new DirectoryInfo(Path.Combine(catalogDirectory.FullName, CLASSICAL));

    foreach (var file in classicalDirectory.GetFiles())
    {
        classical.Add(file);
    }
}

//void LoadTreasureCatalog()
//{
//    foreach (var file in Directory.GetFiles(Path.Combine(catalog, KJV_AUDIO_TREASURE)))
//    {
//        treasure.Add(new FileInfo(file));
//    }
//}

//void LoadMiscellaneousCatalog()
//{
//    foreach (var file in Directory.GetFiles(Path.Combine(catalog, MISCELLANEOUS)))
//    {
//        miscellaneous.Add(new FileInfo(file));
//    }
//}

//void LoadScourbyCatalog()
//{
//    foreach (var file in Directory.GetFiles(Path.Combine(catalog, KJV_ALEXANDER_SCOURBY)))
//    {
//        scourby.Add(new FileInfo(file));
//    }
//}

//void LoadMasterpiecesCatalog()
//{
//    foreach (var file in Directory.GetFiles(Path.Combine(catalog, MASTERPIECES)))
//    {
//        masterpieces.Add(new FileInfo(file));
//    }
//}



GeneratePlaylist();

void GeneratePlaylist() 
{
    var kjvChristopher = Path.Combine(catalogDirectory.FullName, KJV_CHRISTOPHER);
    var kjvChristopherDirectory = new DirectoryInfo(kjvChristopher);
    var kjvChristopherFiles = kjvChristopherDirectory
        .GetFiles()
        .OrderBy(f => f.Name)
        .ToList();

    // MOVE KJV CHRISTOPHER 5 FILES AT A TIME
    for (int kjv1 = 0; kjv1 < kjvChristopherFiles.Count; kjv1++)
    {
        var counter = kjv1 + 1;

        var kjvChristopherFile = new FileInfo(kjvChristopherFiles[kjv1].FullName);

        MoveToPlaylist(kjvChristopherFile.FullName, playlistDirectory.FullName, kjvChristopherFile.Name);

        if (counter % 5 == 0)
        {
            //AddHymns(3);
            //AddDevotions(1);

            AddClassical(2);
            //AddTreasure(1);
            //AddHymns(8);
            //AddDevotions(1);
            //AddMiscellaneous(1);
            //AddScourby(1);
            //AddMasterpiece(1);
            //AddHymns(2);
        }
    }

    Console.WriteLine(FileHelper.Playlist.Count);

    int count = 0;
    
    foreach(var item in FileHelper.Playlist)
    {
        count++;

        if (item.Filename.EndsWith("mp4"))
        {
            writer.WriteLine($"file '{item.FullName}'");

            if (count > 100)
            {
                writer.Flush();
                writer.Close();
                break;
            }
        }
    }
}

//void AddHymns(int count)
//{
//    //var hymnIndex = 0; //this was returned to keep track in calling method to generate files in sequence - not used with random

//    if(hymns.Count < count)
//    {
//        LoadHymnsCatalog();
//    }

//    var rnds = Enumerable.Range(0, hymns.Count - 1)
//        .OrderBy(r => random.Next(hymns.Count - 1))
//        .Take(count)
//        .ToList();

//    foreach(var rnd in rnds)
//    {
//        var file = hymns[rnd];

//        CopyToPlaylist(file.FullName, playlist, file.Name);

//        Console.WriteLine(file.Name);
//    }

//    foreach(var rnd in rnds.OrderByDescending(r => r))
//    {
//        hymns.RemoveAt(rnd);
//    }
//}

//void AddDevotions(int count)
//{
//    //var devotionIndex = 0; //this was returned to keep track in calling method to generate files in sequence - not used with random

//    if(devotions.Count < 1)
//    {
//        LoadDevotionsCatalog();
//    }

//    var rnd = Enumerable.Range(0, devotions.Count - 1)
//        .OrderBy(r => random.Next(devotions.Count - 1))
//        .Take(count)
//        .Single();

//    var file = devotions[rnd];

//    CopyToPlaylist(file.FullName, playlist, file.Name);

//    Console.WriteLine(file.Name);

//    devotions.RemoveAt(rnd);
//}

void AddClassical(int count)
{
    if(classical.Count < 2)
    {
        LoadClassicalCatalog();
    }

    if (classical.Count > 0)
    {
        var rnds = Enumerable.Range(0, classical.Count - 1)
            .OrderBy(r => random.Next(classical.Count - 1))
            .Take(count)
            .ToList();

        foreach (var rnd in rnds)
        {
            var file = classical[rnd];

            MoveToPlaylist(file.FullName, playlistDirectory.FullName, file.Name);

            Console.WriteLine(file.Name);
        }

        foreach (var rnd in rnds.OrderByDescending(r => r))
        {
            classical.RemoveAt(rnd);
        }
    }
}

//void AddTreasure(int count)
//{
//    if(treasure.Count < 1)
//    {
//        LoadTreasureCatalog();
//    }

//    var rnd = Enumerable.Range(0, treasure.Count - 1)
//        .OrderBy(r => random.Next(treasure.Count - 1))
//        .Take(count)
//        .Single();

//    var file = treasure[rnd];

//    CopyToPlaylist(file.FullName, playlist, file.Name);

//    Console.WriteLine(file.Name);

//    treasure.RemoveAt(rnd);
//}

//void AddMiscellaneous(int count)
//{
//    if (miscellaneous.Count < 1)
//    {
//        LoadMiscellaneousCatalog();
//    }

//    var rnd = Enumerable.Range(0, miscellaneous.Count - 1)
//        .OrderBy(r => random.Next(miscellaneous.Count - 1))
//        .Take(count)
//        .Single();

//    var file = miscellaneous[rnd];

//    CopyToPlaylist(file.FullName, playlist, file.Name);

//    Console.WriteLine(file.Name);

//    miscellaneous.RemoveAt(rnd);
//}

//void AddScourby(int count)
//{
//    if (scourby.Count < 1)
//    {
//        LoadScourbyCatalog();
//    }

//    var rnd = Enumerable.Range(0, scourby.Count - 1)
//        .OrderBy(r => random.Next(scourby.Count - 1))
//        .Take(count)
//        .Single();

//    var file = scourby[rnd];

//    CopyToPlaylist(file.FullName, playlist, file.Name);

//    Console.WriteLine(file.Name);

//    scourby.RemoveAt(rnd);
//}

//void AddMasterpiece(int count)
//{
//    if (masterpieces.Count < 1)
//    {
//        LoadMasterpiecesCatalog();
//    }

//    var rnd = Enumerable.Range(0, masterpieces.Count - 1)
//        .OrderBy(r => random.Next(masterpieces.Count - 1))
//        .Take(count)
//        .Single();

//    var file = masterpieces[rnd];

//    CopyToPlaylist(file.FullName, playlist, file.Name);

//    Console.WriteLine(file.Name);

//    masterpieces.RemoveAt(rnd);
//}

//void CopyToPlaylist(string source, string targetPath, string targetFile)
//{
//    var filename = targetFile;

//    targetFile = PrefixFile(targetFile);

//    var target = Path.Combine(targetPath, targetFile);

//    Console.WriteLine(target);

//    if (!FileHelper.Playlist.Any(p => p.Filename == filename))
//    {
//        var item = new PlaylistItem
//        {
//            Source = source,
//            Target = target,
//            Filename = filename,
//            IsCopy = true
//        };

//        FileHelper.Playlist.Add(item);
//    }

//    File.Copy(source, target);

//    FileHelper.FileCounter += 1;
//}

void MoveToPlaylist(string source, string targetPath, string targetFile) //, int next)
{
    var filename = targetFile;

    targetFile = PrefixFile(targetFile);

    var target = Path.Combine(targetPath, targetFile);

    Console.WriteLine(target);

    //if(FileHelper.Playlist.Count(p => p.Filename == filename) > 0)
    //{
    //    Debugger.Break();
    //}

    if (!FileHelper.Playlist.Any(p => p.Filename == filename))
    {
        var item = new PlaylistItem
        {
            Source = source,
            Target = target,
            Filename = filename,
            FullName = targetFile,
            IsCopy = false,
            Next = 0
        };

        FileHelper.Playlist.Add(item);
    }

    File.Move(source, target);

    FileHelper.FileCounter += 1;
}

List<string> GetContainers()
{
    return new List<string>()
    {
        KJV_CHRISTOPHER,
        //KJV_ALEXANDER_SCOURBY,
        //KJV_AUDIO_TREASURE,
        //DEVOTIONS,
        CLASSICAL,
        //MASTERPIECES,
        //MISCELLANEOUS,
        //HYMNS,
        //THEMES
    };
}

string PrefixFile(string targetFile)
{
    var filename = String.Empty;
    var filePrefix = String.Empty;
    
    var fileCounter = FileHelper.FileCounter;

    if(fileCounter < 10)
    {
        filePrefix = $"00000{fileCounter}";
    }
    else if(fileCounter < 100)
    {
        filePrefix = $"0000{fileCounter}";
    }
    else if(fileCounter < 1000)
    {
        filePrefix = $"000{fileCounter}";
    }
    else if(fileCounter < 10000)
    {
        filePrefix = $"00{fileCounter}";
    }
    else if(fileCounter < 100000)
    {
        filePrefix = $"0{fileCounter}";
    }
    else if(fileCounter < 1000000)
    {
        filePrefix = fileCounter.ToString();
    }

    filename = $"{filePrefix}-{targetFile}";

    return filename;
}



//CalculateDuration();

void CalculateDuration()
{
    foreach(var folder in Directory.GetDirectories("C:\\Users\\mjames\\OneDrive\\bible\\media\\reading"))
    {
        var dirInfo = new DirectoryInfo(folder);

        Console.WriteLine(dirInfo.FullName);

        Directory.Move(dirInfo.FullName, $"C:\\Users\\mjames\\OneDrive\\bible\\media\\reading\\KJV-E-{dirInfo.Name}");
    }

    //long ticks = 0;
    //int count = 0;

    //var longItems = new List<string>();
    //var maxDuration = 8;

    //foreach (var filename in Directory.GetFiles(playlist))
    //{
    //    count++;

    //    var file = TagLib.File.Create(filename);

    //    //Console.WriteLine($"{count}) {file.Tag.Title}");

    //    //if (!String.IsNullOrWhiteSpace(file.Tag.Title) && file.Tag.Title.Contains("Peer"))
    //    //{
    //    //    Debugger.Break();
    //    //}

    //    if (file.Tag != null && file.Tag.Artists.Length > 0 && file.Tag.Artists[0].Contains("Georgian"))
    //    {
    //        Console.WriteLine(filename);
    //        Console.WriteLine(file.Tag.Artists[0]);
    //        Console.WriteLine();
    //        Console.WriteLine();
    //        //Debugger.Break();

    //        var fileInfo = new FileInfo(filename);

    //        File.Move(filename, $@"C:\Projects\media\catalog\_copyright-restricted\{fileInfo.Name}");
    //    }

    //    ticks += file.Properties.Duration.Ticks;

    //    var timespan = new TimeSpan(file.Properties.Duration.Ticks);

    //    if(timespan.Minutes >= maxDuration)
    //    {
    //        longItems.Add(filename);
    //    }
    //}

    //foreach(var item in longItems.OrderBy(f => f))
    //{
    //    Console.WriteLine(item);
    //}

    //Console.WriteLine();
    //Console.WriteLine($"Over {maxDuration} minutes: {longItems.Count}");
    //Console.WriteLine();

    //var days = new TimeSpan(ticks).Days;
    //var hours = new TimeSpan(ticks).Hours;
    //var minutes = new TimeSpan(ticks).Minutes;
    //var seconds = new TimeSpan(ticks).Seconds;

    //Console.WriteLine($"{days} days");
    //Console.WriteLine($"{hours} hours");
    //Console.WriteLine($"{minutes} minutes");
    //Console.WriteLine($"{seconds} seconds");
}

StartStreaming();

async void StartStreaming()
{
    var process = new Process();
    process.StartInfo.FileName = "ffmpeg";
    process.StartInfo.CreateNoWindow = true;

    // the following loads one media file at a time
    var files = new List<string>();
    files.Add("~/media/playlist/alb-adagio.mp4");
    files.Add("~/media/playlist/alb-op5-01.mp4");
    files.Add("~/media/playlist/alb-op5-02.mp4");

    foreach(var file in files)
    {
        process.StartInfo.Arguments = $"-re -i \"{file}\" -c:v copy -c:a aac -ar 44100 -ac 1 -f flv rtmp://localhost/live/stream";
        process.Start();

        // maybe check here for existing connections instead of a different thread
    }

    // the following uses the playlist file
    // process.StartInfo.Arguments = $"-f concat -re -safe 0 -stream_loop -1 -i \"{playlistDirectory.FullName}/playlist.txt\" -c copy -f flv rtmp://localhost/live/stream";
    
    // process.Start();

    // MonitorPort();

    ////process.WaitForExit();

    ////if (process.ExitCode != 0)
    ////{
    ////    Console.WriteLine("Event caught!");
    ////}
}


Task MonitorPort()
{
    while(true)
    {
        Thread.Sleep(10000);

        bool inUse = false;

        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var ipEndPoints = ipProperties.GetActiveTcpListeners();
        var ipConnections = ipProperties.GetActiveTcpConnections();

        var endpointsCount = ipEndPoints.Count(ep => ep.Port == 1935);
        foreach (var endPoint in ipEndPoints)
        {
            Console.WriteLine($"Port: {endPoint.Port}");

            //if (endPoint.Port == 1935)
            //{
            //    Console.WriteLine("CONNECTED");

            //    inUse = true;
            //    break;
            //}
        }

        var connectionsCount = ipConnections.Count();
        foreach(var connection in ipConnections)
        {
            Console.WriteLine($"Local Endpoint: {connection.LocalEndPoint}");
            Console.WriteLine($"Remote Endpoint: {connection.RemoteEndPoint}");
        }

        //return inUse;
    }
}