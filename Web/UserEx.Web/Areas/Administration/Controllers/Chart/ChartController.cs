namespace UserEx.Web.Areas.Administration.Controllers.Chart
{
    using System.Collections.Generic;
    using System.Linq;

    using ChartJSCore.Helpers;
    using ChartJSCore.Models;
    using ChartJSCore.Plugins.Zoom;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Services.Data.Billing;

    public class ChartController : AdministrationController
    {
        private readonly IBillingService billing;

        public ChartController(
            IBillingService billing)
        {
            this.billing = billing;
        }

        public IActionResult Chart()
        {
            Chart verticalBarChart = this.GenerateVerticalBarChart();
            Chart lineChart = this.GenerateLineChart();

            this.ViewData["VerticalBarChart"] = verticalBarChart;
            this.ViewData["LineChart"] = lineChart;

            return this.View();
        }

        private Chart GenerateVerticalBarChart()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;

            var query = this.billing.CostCallsByMonthChart();

            Data data = new Data();
            data.Labels = query.Select(r => r.MonthDisplay).ToList();
            BarDataset dataset = new BarDataset()
            {
                Label = "Calls cost by month 2022",
                Data = query.Select(r => (double?)r.CostSum).ToList(),
                BackgroundColor = new List<ChartColor>
                {
                    ChartColor.FromRgba(255, 99, 132, 0.2),
                    ChartColor.FromRgba(54, 162, 235, 0.2),
                    ChartColor.FromRgba(255, 206, 86, 0.2),
                    ChartColor.FromRgba(75, 192, 192, 0.2),
                    ChartColor.FromRgba(153, 102, 255, 0.2),
                    ChartColor.FromRgba(255, 159, 64, 0.2),
                },
                BorderColor = new List<ChartColor>
                {
                    ChartColor.FromRgb(255, 99, 132),
                    ChartColor.FromRgb(54, 162, 235),
                    ChartColor.FromRgb(255, 206, 86),
                    ChartColor.FromRgb(75, 192, 192),
                    ChartColor.FromRgb(153, 102, 255),
                    ChartColor.FromRgb(255, 159, 64),
                },
                BorderWidth = new List<int>() { 1 },
                BarPercentage = 0.5,
                BarThickness = 6,
                MaxBarThickness = 8,
                MinBarLength = 2,
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            var options = new Options
            {
                Scales = new Dictionary<string, Scale>()
                {
                    {
                        "y", new CartesianLinearScale()
                        {
                            BeginAtZero = true,
                        }
                    },
                    {
                        "x", new Scale()
                        {
                            Grid = new Grid()
                            {
                                Offset = true,
                            },
                        }
                    },
                },
            };

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12,
                    },
                },
            };

            return chart;
        }

        private Chart GenerateLineChart()
        {
            Chart chart = new Chart();

            var query = this.billing.CostProcuredNumbersByMonthChart();

            chart.Type = Enums.ChartType.Line;
            chart.Options.Scales = new Dictionary<string, Scale>();
            CartesianScale xAxis = new CartesianScale();
            xAxis.Display = true;
            xAxis.Title = new Title
            {
                Text = new List<string> { "Month" },
                Display = true,
            };
            chart.Options.Scales.Add("x", xAxis);

            Data data = new Data
            {
                Labels = query.Select(r => r.MonthDisplay).ToList(),
            };

            LineDataset dataset = new LineDataset()
            {
                Label = "DID numbers cost by month",
                Data = query.Select(r => (double?)r.CostSum).ToList(),
                Fill = "true",
                Tension = .01,
                BackgroundColor = new List<ChartColor> { ChartColor.FromRgba(75, 192, 192, 0.4) },
                BorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                BorderCapStyle = "butt",
                BorderDash = new List<int>(),
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false,
            };

            data.Datasets = new List<Dataset>
            {
                dataset,
            };

            chart.Data = data;

            ZoomOptions zoomOptions = new ZoomOptions
            {
                Zoom = new Zoom
                {
                    Wheel = new Wheel
                    {
                        Enabled = true,
                    },
                    Pinch = new Pinch
                    {
                        Enabled = true,
                    },
                    Drag = new Drag
                    {
                        Enabled = true,
                        ModifierKey = Enums.ModifierKey.alt,
                    },
                },
                Pan = new Pan
                {
                    Enabled = true,
                    Mode = "xy",
                },
            };

            chart.Options.Plugins = new ChartJSCore.Models.Plugins
            {
                PluginDynamic = new Dictionary<string, object> { { "zoom", zoomOptions } },
            };

            return chart;
        }
    }
}
