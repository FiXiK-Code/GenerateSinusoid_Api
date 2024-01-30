using Microsoft.AspNetCore.Mvc;
using Sin_API.Models;

using SkiaSharp;
using System.IO;

namespace Sin_API.Controllers;

[ApiController]
[Route("[controller]")]
public class SinContrioller : ControllerBase
{
    [HttpPost]
    public IActionResult GetDocExcel([FromQuery] PostParameters parameters)
    {
        double amplitude = parameters.A;
        double samplingFrequency = parameters.Fd;
        double signalFrequency = parameters.Fs;
        int periods = parameters.N;

        int width = 800;
        int height = 600;
        using (var bitmap = new SKBitmap(width, height))
        {
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                double period = 1.0 / signalFrequency;
                double deltaX = period / samplingFrequency;
                int totalPoints = periods * (int)(samplingFrequency / signalFrequency);

                for (int i = 0; i < totalPoints - 1; i++)
                {
                    double t = i * deltaX;
                    double y = amplitude * Math.Sin(2 * Math.PI * signalFrequency * t);

                    float x1 = (float)(i * width / totalPoints);
                    float y1 = (float)(height / 2 - y * height / 2);
                    float x2 = (float)((i + 1) * width / totalPoints);
                    float y2 = (float)(height / 2 - amplitude * Math.Sin(2 * Math.PI * signalFrequency * (t + deltaX)) * height / 2);

                    canvas.DrawLine(x1, y1, x2, y2, new SKPaint { Color = SKColors.Black });
                }
            }

            using (var imageStream = new MemoryStream())
            {
                bitmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(imageStream);

                Response.Headers.Add("Content-Disposition", "attachment; filename=sinusoid.png");
                Response.Headers.Add("Content-Type", "image/png");

                return File(imageStream.ToArray(), "image/png");
            }
        }
    }
}