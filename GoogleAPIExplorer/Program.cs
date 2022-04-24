using Google.Cloud.Vision.V1;

var fileName = Environment.GetCommandLineArgs()[1];
var image = Image.FromFile(fileName);

var client = ImageAnnotatorClient.Create();

var detectedFacesTask = client.DetectFacesAsync(image);
var detectedTextsTask = client.DetectTextAsync(image);
var detectedLabelsTask = client.DetectLabelsAsync(image);
var detectedLandmarksTask = client.DetectLandmarksAsync(image);
var detectedLogosTask = client.DetectLogosAsync(image);


foreach (var face in detectedFaces)
{
    string poly = string.Join(" - ", face.BoundingPoly.Vertices.Select(v => $"({v.X}, {v.Y})"));
    Console.WriteLine($"FACE | Confidence: {(int)(face.DetectionConfidence * 100)}%; BoundingPoly: {poly}");
}

var detectedTexts = await client.DetectTextAsync(image);
foreach (var text in detectedTexts)
{
    Console.WriteLine($"TEXT | Description: {text.Description}");
}

var detectedLabels = await client.DetectLabelsAsync(image);
foreach (var label in detectedLabels)
{
    Console.WriteLine($"LABEL | Score: {(int)(label.Score * 100)}%; Description: {label.Description}");
}

var detectedLandmarks = await client.DetectLandmarksAsync(image);
foreach (var landmark in detectedLandmarks)
{
    Console.WriteLine($"LANDMARK | Score: {(int)(landmark.Score * 100)}%; Description: {landmark.Description}");
}

var detectedLogos = await client.DetectLogosAsync(image);
foreach (var logo in detectedLogos)
{
    Console.WriteLine($"LOGO | Description: {logo.Description}");
}

var annotation = await client.DetectSafeSearchAsync(image);
Console.WriteLine($"SAFE SEARCH | Adult? {annotation.Adult}");
Console.WriteLine($"SAFE SEARCH | Spoof? {annotation.Spoof}");
Console.WriteLine($"SAFE SEARCH | Violence? {annotation.Violence}");
Console.WriteLine($"SAFE SEARCH | Medical? {annotation.Medical}");

