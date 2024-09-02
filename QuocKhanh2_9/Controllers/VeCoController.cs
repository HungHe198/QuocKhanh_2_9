using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace QuocKhanh2_9.Controllers
{
    public class VeCoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Draw()
        {

            int width = 1000;
            int height = (width * 2) / 3;
            var image = new Image<Rgba32>(width, height);

            // Vẽ nền đỏ từng pixel
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    image[x, y] = Color.Red;
                }

                Task.Delay(1000); // Thời gian delay cho hiệu ứng vẽ từng phần
            }

            // Thông số ngôi sao vàng
            var centerX = width / 2;
            var centerY = height / 2;
            var outerRadius = height * 0.25;
            var innerRadius = outerRadius * Math.Sin(Math.PI / 10) / Math.Cos(Math.PI / 5);

            // Tính toán các điểm của ngôi sao
            var starPoints = new PointF[10];
            for (int i = 0; i < 5; i++)
            {
                starPoints[2 * i] = new PointF(
                    centerX + (float)(outerRadius * Math.Cos(2 * Math.PI * i / 5 - Math.PI / 2)),
                    centerY + (float)(outerRadius * Math.Sin(2 * Math.PI * i / 5 - Math.PI / 2))
                );
                starPoints[2 * i + 1] = new PointF(
                    centerX + (float)(innerRadius * Math.Cos(2 * Math.PI * (i + 0.5) / 5 - Math.PI / 2)),
                    centerY + (float)(innerRadius * Math.Sin(2 * Math.PI * (i + 0.5) / 5 - Math.PI / 2))
                );
            }

            // Sử dụng Polygon để tạo ngôi sao
            var starPolygon = new Polygon(new LinearLineSegment(starPoints));

            // Vẽ ngôi sao vàng
            image.Mutate(ctx => ctx.Fill(Color.Yellow, starPolygon));


            using (var stream = new MemoryStream())
            {
                image.SaveAsPng(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream.ToArray(), "image/png");
            }
        }

        public IActionResult DrawGate()
        {
            int width = 600;
            int height = 400;
            using var image = new Image<Rgba32>(width, height);

            // Define the pen for drawing lines
            var blackPen = Pens.Solid(Color.Black, 2);
            var grayPen = Pens.Solid(Color.Gray, 1);

            // Draw vertical bars
            for (int x = 100; x < 500; x += 40)
            {
                image.Mutate(ctx => ctx.DrawLine(blackPen, new PointF[]
                {
                    new PointF(x, 50), new PointF(x, 350)
                }));
            }

            // Draw horizontal elements
            image.Mutate(ctx => ctx.DrawLine(blackPen, new PointF[]
            {
                new PointF(100, 50), new PointF(500, 50)
            }));
            image.Mutate(ctx => ctx.DrawLine(blackPen, new PointF[]
            {
                new PointF(100, 350), new PointF(500, 350)
            }));
            image.Mutate(ctx => ctx.DrawLine(blackPen, new PointF[]
            {
                new PointF(100, 150), new PointF(500, 150)
            }));

            // Draw decorative circles
            for (int x = 140; x < 500; x += 80)
            {
                image.Mutate(ctx => ctx.Draw(grayPen, new EllipsePolygon(x, 100, 20, 20)));
            }

            // Save the image to a stream
            using var stream = new MemoryStream();
            image.SaveAsPng(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream.ToArray(), "image/png");
        }
    }
}
