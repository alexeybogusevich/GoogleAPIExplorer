using Google.Cloud.Vision.V1;

var fileName = Environment.GetCommandLineArgs()[1];
var image = Image.FromFile(fileName);

var client = ImageAnnotatorClient.Create();

var detectedFacesTask = client.DetectFacesAsync(image);
var detectedTextsTask = client.DetectTextAsync(image);
var detectedLabelsTask = client.DetectLabelsAsync(image);
var detectedLandmarksTask = client.DetectLandmarksAsync(image);
var detectedLogosTask = client.DetectLogosAsync(image);
var safeSearchAnnotationTask = client.DetectSafeSearchAsync(image);

await Task.WhenAll(new Task[]
{
    detectedFacesTask,
    detectedTextsTask,
    detectedLabelsTask,
    detectedLandmarksTask,
    detectedLogosTask,
    safeSearchAnnotationTask,
});

foreach (var face in detectedFacesTask.Result)
{
    string poly = string.Join(" - ", face.BoundingPoly.Vertices.Select(v => $"({v.X}, {v.Y})"));
    Console.WriteLine($"FACE | Confidence: {(int)(face.DetectionConfidence * 100)}%; BoundingPoly: {poly}");
}

foreach (var text in detectedTextsTask.Result)
{
    Console.WriteLine($"TEXT | Description: {text.Description}");
}

foreach (var label in detectedLabelsTask.Result)
{
    Console.WriteLine($"LABEL | Score: {(int)(label.Score * 100)}%; Description: {label.Description}");
}

foreach (var landmark in detectedLandmarksTask.Result)
{
    Console.WriteLine($"LANDMARK | Score: {(int)(landmark.Score * 100)}%; Description: {landmark.Description}");
}

foreach (var logo in detectedLogosTask.Result)
{
    Console.WriteLine($"LOGO | Description: {logo.Description}");
}

var safeSearchAnnotation = safeSearchAnnotationTask.Result;
Console.WriteLine($"SAFE SEARCH | Adult? {safeSearchAnnotation.Adult}");
Console.WriteLine($"SAFE SEARCH | Spoof? {safeSearchAnnotation.Spoof}");
Console.WriteLine($"SAFE SEARCH | Violence? {safeSearchAnnotation.Violence}");
Console.WriteLine($"SAFE SEARCH | Medical? {safeSearchAnnotation.Medical}");

