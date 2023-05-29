using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tanks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string left1, left2, top1, top2;
        DispatcherTimer timer = new DispatcherTimer();

        List<Naboj> bList = new List<Naboj>();
        List<Rectangle> wList = new List<Rectangle>();

        Thickness start1 = new Thickness();
        Thickness start2 = new Thickness();
        public MainWindow()
        {
            InitializeComponent();

            start1 = new Thickness(tank1.Margin.Left, tank1.Margin.Top, tank1.Margin.Right, tank1.Margin.Bottom);
            start2 = new Thickness(tank2.Margin.Left, tank2.Margin.Top, tank2.Margin.Right, tank2.Margin.Bottom);

            spawnWalls();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void spawnWalls()
        {
            wList.Add(wall1); wList.Add(wall2); wList.Add(wall3); wList.Add(wall4); wList.Add(wall5);
            wList.Add(wall6); wList.Add(wall7); wList.Add(wall8); wList.Add(wall9); wList.Add(wall10);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            moveTanks();

            bulletsMove();
        }
        private void bulletsMove()
        {
            for (int i = 0; i < bList.Count; i++)
            {
                if (bList[i].LeftB == "" && bList[i].TopB == "")
                {
                    bList[i].TopB = "top";
                }

                if (bList[i].LeftB == "left")
                {
                    bList[i].Margin = new Thickness(bList[i].Margin.Left - 4, bList[i].Margin.Top, bList[i].Margin.Right + 4, bList[i].Margin.Bottom);
                    if (intersekt(bList[i], leftwall))
                    {
                        bList[i].LeftB = "right";
                        bList[i].Life--;
                    }
                    for (int j = 0; j < wList.Count; j++)
                    {
                        if (intersekt(bList[i], wList[j]) && bList[i].Margin.Left > wList[j].Margin.Left)
                        {
                            if (wList[j].Width < wList[j].Height)
                            {
                                bList[i].LeftB = "right";
                                bList[i].Life--;
                            }
                            j = wList.Count;
                        }
                    }
                }
                else if (bList[i].LeftB == "right")
                {
                    bList[i].Margin = new Thickness(bList[i].Margin.Left + 4, bList[i].Margin.Top, bList[i].Margin.Right - 4, bList[i].Margin.Bottom);
                    if (intersekt(bList[i], rightwall))
                    {
                        bList[i].LeftB = "left";
                        bList[i].Life--;
                    }
                    for (int j = 0; j < wList.Count; j++)
                    {
                        if (intersekt(bList[i], wList[j]) && bList[i].Margin.Left < wList[j].Margin.Left)
                        {
                            if (wList[j].Width < wList[j].Height)
                            {
                                bList[i].LeftB = "left";
                                bList[i].Life--;
                            }
                            j = wList.Count;
                        }
                    }
                }

                if (bList[i].TopB == "top")
                {
                    bList[i].Margin = new Thickness(bList[i].Margin.Left, bList[i].Margin.Top - 4, bList[i].Margin.Right, bList[i].Margin.Bottom + 4);
                    if (intersekt(bList[i], upwall))
                    {
                        bList[i].TopB = "down";
                        bList[i].Life--;
                    }
                    for (int j = 0; j < wList.Count; j++)
                    {
                        if (intersekt(bList[i], wList[j]) && bList[i].Margin.Bottom < wList[j].Margin.Bottom)
                        {
                            bList[i].TopB = "down";
                            j = wList.Count;
                            bList[i].Life--;
                        }
                    }
                }
                else if (bList[i].TopB == "down")
                {
                    bList[i].Margin = new Thickness(bList[i].Margin.Left, bList[i].Margin.Top + 4, bList[i].Margin.Right, bList[i].Margin.Bottom - 4);
                    if (intersekt(bList[i], bottomwall))
                    {
                        bList[i].TopB = "top";
                        bList[i].Life--;
                    }
                    for (int j = 0; j < wList.Count; j++)
                    {
                        if (intersekt(bList[i], wList[j]) && bList[i].Margin.Bottom > wList[j].Margin.Bottom)
                        {
                            bList[i].TopB = "top";
                            j = wList.Count;
                            bList[i].Life--;
                        }
                    }
                }

                if (!bList[i].outOfTank && !intersekt(tank1, bList[i]) && !intersekt(tank2, bList[i]))
                {
                    bList[i].outOfTank = true;
                }

                if (intersekt(tank1, bList[i])  && bList[i].outOfTank)
                {
                    endGame();
                    MessageBox.Show("Vyhrál červený gratulujeme!!!!");
                    return;
                }
                if (intersekt(tank2, bList[i]) && bList[i].outOfTank)
                {
                    endGame();
                    MessageBox.Show("Vyhrál řůžový gratulujeme!!!!");
                    return;
                }

                if (bList[i].Life <= 0)
                {
                    maingrid.Children.Remove(bList[i]);
                    bList.Remove(bList[i]);
                    return;
                }
            }
        }
        private void endGame()
        {
            foreach (Naboj naboj in bList)
            {
                maingrid.Children.Remove(naboj);
            }

            bList.Clear();

            tank1.Margin = start1;
            tank2.Margin = start2;
        }
        private bool intersekt(Shape s1, Shape s2)
        {
            GeneralTransform t1 = s1.TransformToVisual(this);
            GeneralTransform t2 = s2.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect() { X = 0, Y = 0, Width = s1.ActualWidth, Height = s1.ActualHeight });
            Rect r2 = t2.TransformBounds(new Rect() { X = 0, Y = 0, Width = s2.ActualWidth, Height = s2.ActualHeight });
            bool result = r1.IntersectsWith(r2);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void moveTanks()
        {
            if (top1 == "top")
            {
                tank1.Margin = new Thickness(tank1.Margin.Left, tank1.Margin.Top - 3, tank1.Margin.Right, tank1.Margin.Bottom + 3);
                if (intersekt(tank1, upwall))
                {
                    tank1.Margin = new Thickness(tank1.Margin.Left, tank1.Margin.Top + 3, tank1.Margin.Right, tank1.Margin.Bottom - 3);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank1, wall) && tank1.Margin.Bottom < wall.Margin.Bottom)
                    {
                        tank1.Margin = new Thickness(tank1.Margin.Left, tank1.Margin.Top + 3, tank1.Margin.Right, tank1.Margin.Bottom - 3);
                    }
                }
            }
            else if (top1 == "down")
            {
                tank1.Margin = new Thickness(tank1.Margin.Left, tank1.Margin.Top + 3, tank1.Margin.Right, tank1.Margin.Bottom - 3);
                if (intersekt(tank1, bottomwall))
                {
                    tank1.Margin = new Thickness(tank1.Margin.Left, tank1.Margin.Top - 3, tank1.Margin.Right, tank1.Margin.Bottom + 3);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank1, wall) && tank1.Margin.Bottom > wall.Margin.Bottom)
                    {
                        tank1.Margin = new Thickness(tank1.Margin.Left, tank1.Margin.Top - 3, tank1.Margin.Right, tank1.Margin.Bottom + 3);
                    }
                }
            }
            if (left1 == "left")
            {
                tank1.Margin = new Thickness(tank1.Margin.Left - 3, tank1.Margin.Top, tank1.Margin.Right + 3, tank1.Margin.Bottom);
                if (intersekt(tank1, leftwall))
                {
                    tank1.Margin = new Thickness(tank1.Margin.Left + 3, tank1.Margin.Top, tank1.Margin.Right - 3, tank1.Margin.Bottom);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank1, wall) && tank1.Margin.Left > wall.Margin.Left)
                    {
                        tank1.Margin = new Thickness(tank1.Margin.Left + 3, tank1.Margin.Top, tank1.Margin.Right - 3, tank1.Margin.Bottom);
                    }
                }
            }
            else if (left1 == "right")
            {
                tank1.Margin = new Thickness(tank1.Margin.Left + 3, tank1.Margin.Top, tank1.Margin.Right - 3, tank1.Margin.Bottom);
                if (intersekt(tank1, rightwall))
                {
                    tank1.Margin = new Thickness(tank1.Margin.Left - 3, tank1.Margin.Top, tank1.Margin.Right + 3, tank1.Margin.Bottom);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank1, wall) && tank1.Margin.Left < wall.Margin.Left)
                    {
                        tank1.Margin = new Thickness(tank1.Margin.Left - 3, tank1.Margin.Top, tank1.Margin.Right + 3, tank1.Margin.Bottom);
                    }
                }
            }

            if (top2 == "top")
            {
                tank2.Margin = new Thickness(tank2.Margin.Left, tank2.Margin.Top - 3, tank2.Margin.Right, tank2.Margin.Bottom + 3);
                if (intersekt(tank2, upwall))
                {
                    tank2.Margin = new Thickness(tank2.Margin.Left, tank2.Margin.Top + 3, tank2.Margin.Right, tank2.Margin.Bottom - 3);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank2, wall) && tank2.Margin.Bottom < wall.Margin.Bottom)
                    {
                        tank2.Margin = new Thickness(tank2.Margin.Left, tank2.Margin.Top + 3, tank2.Margin.Right, tank2.Margin.Bottom - 3);
                    }
                }
            }
            else if (top2 == "down")
            {
                tank2.Margin = new Thickness(tank2.Margin.Left, tank2.Margin.Top + 3, tank2.Margin.Right, tank2.Margin.Bottom - 3);
                if (intersekt(tank2, bottomwall))
                {
                    tank2.Margin = new Thickness(tank2.Margin.Left, tank2.Margin.Top - 3, tank2.Margin.Right, tank2.Margin.Bottom + 3);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank2, wall) && tank2.Margin.Bottom > wall.Margin.Bottom)
                    {
                        tank2.Margin = new Thickness(tank2.Margin.Left, tank2.Margin.Top - 3, tank2.Margin.Right, tank2.Margin.Bottom + 3);
                    }
                }
            }
            if (left2 == "left")
            {
                tank2.Margin = new Thickness(tank2.Margin.Left - 3, tank2.Margin.Top, tank2.Margin.Right + 3, tank2.Margin.Bottom);
                if (intersekt(tank2, leftwall))
                {
                    tank2.Margin = new Thickness(tank2.Margin.Left + 3, tank2.Margin.Top, tank2.Margin.Right - 3, tank2.Margin.Bottom);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank2, wall) && tank2.Margin.Left > wall.Margin.Left)
                    {
                        tank2.Margin = new Thickness(tank2.Margin.Left + 3, tank2.Margin.Top, tank2.Margin.Right - 3, tank2.Margin.Bottom);
                    }
                }
            }
            else if (left2 == "right")
            {
                tank2.Margin = new Thickness(tank2.Margin.Left + 3, tank2.Margin.Top, tank2.Margin.Right - 3, tank2.Margin.Bottom);
                if (intersekt(tank2, rightwall))
                {
                    tank2.Margin = new Thickness(tank2.Margin.Left - 3, tank2.Margin.Top, tank2.Margin.Right + 3, tank2.Margin.Bottom);
                }
                foreach (Rectangle wall in wList)
                {
                    if (intersekt(tank2, wall) && tank2.Margin.Left < wall.Margin.Left)
                    {
                        tank2.Margin = new Thickness(tank2.Margin.Left - 3, tank2.Margin.Top, tank2.Margin.Right + 3, tank2.Margin.Bottom);
                    }
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                top1 = "top";
            }
            if (e.Key == Key.Down)
            {
                top1 = "down";
            }
            if (e.Key == Key.Left)
            {
                left1 = "left";
            }
            if (e.Key == Key.Right)
            {
                left1 = "right";
            }

            if (e.Key == Key.W)
            {
                top2 = "top";
            }
            if (e.Key == Key.S)
            {
                top2 = "down";
            }
            if (e.Key == Key.A)
            {
                left2 = "left";
            }
            if (e.Key == Key.D)
            {
                left2 = "right";
            }

            if (e.Key == Key.RightShift)
            {
                Naboj naboj = new Naboj(tank1);
                if (left1 == "left")
                {
                    naboj.LeftB = "left";
                }
                else if (left1 == "right")
                {
                    naboj.LeftB = "right";
                }
                else
                {
                    naboj.LeftB = "";
                }
                if (top1 == "down")
                {
                    naboj.TopB = "down";
                }
                else if (top1 == "top")
                {
                    naboj.TopB = "top";
                }
                else
                {
                    naboj.TopB = "";
                }
                maingrid.Children.Add(naboj);
                bList.Add(naboj);
            }
            if (e.Key == Key.LeftShift)
            {
                Naboj naboj = new Naboj(tank2);
                if (left2 == "left")
                {
                    naboj.LeftB = "left";
                }
                else if (left2 == "right")
                {
                    naboj.LeftB = "right";
                }
                else
                {
                    naboj.LeftB = "";
                }
                if (top2 == "down")
                {
                    naboj.TopB = "down";
                }
                else if (top2 == "top")
                {
                    naboj.TopB = "top";
                }
                else
                {
                    naboj.TopB = "";
                }
                maingrid.Children.Add(naboj);
                bList.Add(naboj);
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                top1 = "";
            }
            if (e.Key == Key.Down)
            {
                top1 = "";
            }
            if (e.Key == Key.Left)
            {
                left1 = "";
            }
            if (e.Key == Key.Right)
            {
                left1 = "";
            }

            if (e.Key == Key.W)
            {
                top2 = "";
            }
            if (e.Key == Key.S)
            {
                top2 = "";
            }
            if (e.Key == Key.A)
            {
                left2 = "";
            }
            if (e.Key == Key.D)
            {
                left2 = "";
            }
        }
    }
}
