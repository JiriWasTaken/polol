using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Tanks
{
    internal class Naboj : Shape
    {
        public string TopB { get; set; }
        public string LeftB { get; set; }
        public int Life { get; set; }
        public bool outOfTank { get; set; }
        public Naboj(Shape tank)
        {
            this.Life = 4;
            this.Width = 25;
            this.Height = 25;
            this.Fill = Brushes.Red;
            this.outOfTank = false;
            this.Margin = new Thickness(tank.Margin.Left + 12.5, tank.Margin.Top + 12.5, tank.Margin.Right - 12.5, tank.Margin.Bottom - 12.5);
            Name = "bullet";
        }

        public Double Radius
        {
            get { return (Double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value); }
        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
          "Radius", typeof(Double), typeof(Naboj), new PropertyMetadata(5.0));
        protected override Geometry DefiningGeometry
        {
            get { return new EllipseGeometry(new Point(0, 0), this.Radius, this.Radius); }
        }
    }
}