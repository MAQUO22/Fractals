using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals
{
    public class Triangle
    {
        Random rnd = new Random();
        int i = 0;
        public void SetPoint(Graphics g, Point[] A1, Point B, TextBox textBox,Pen pen1)
        {
            int k = rnd.Next(0, 3);
            Point B1 = new Point(((A1[k].X + B.X) / 2), ((A1[k].Y + B.Y) / 2));
            g.FillEllipse(pen1.Brush, B1.X, B1.Y, Convert.ToInt32(textBox.Text), Convert.ToInt32(textBox.Text));
            i++;
            while (i != 5000)
            {
                SetPoint(g, A1, B1, textBox,pen1); //рекурсия
            }

        }
    }
    public class Fractals
    {
        public static void Fractal_Julia(int w, int h, Graphics g, Pen pen, double _zoom, double _moveX, double _moveY, int _maxIt)
        {
            // при каждой итерации, вычисляется znew = zold² + С

            // вещественная  и мнимая части постоянной C
            double cRe, cIm;
            // вещественная и мнимая части старой и новой
            double newRe, newIm, oldRe, oldIm;
            // Можно увеличивать и изменять положение
            double zoom = _zoom, moveX = _moveX, moveY = _moveY;
            //Определяем после какого числа итераций функция должна прекратить свою работу
            int maxIterations = _maxIt;

            //выбираем несколько значений константы С, это определяет форму фрактала         Жюлиа
            cRe = -0.70176;
            cIm = -0.3842;

            //"перебираем" каждый пиксель
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    //вычисляется реальная и мнимая части числа z
                    //на основе расположения пикселей,масштабирования и значения позиции
                    newRe = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    newIm = (y - h / 2) / (0.5 * zoom * h) + moveY;

                    //i представляет собой число итераций 
                    int i;

                    //начинается процесс итерации
                    for (i = 0; i < maxIterations; i++)
                    {

                        //Запоминаем значение предыдущей итерации
                        oldRe = newRe;
                        oldIm = newIm;

                        // в текущей итерации вычисляются действительная и мнимая части 
                        newRe = oldRe * oldRe - oldIm * oldIm + cRe;
                        newIm = 2 * oldRe * oldIm + cIm;

                        // если точка находится вне круга с радиусом 2 - прерываемся
                        if ((newRe * newRe + newIm * newIm) > 4) break;
                    }

                    //определяем цвета
                    pen.Color = Color.FromArgb(255, (i * 9) % 255, 0, (i * 9) % 255);
                    //рисуем пиксель
                    g.DrawRectangle(pen, x, y, 1, 1);
                }
        }

        public static void Draw(Graphics g, int n, Pen pen1, Pen pen2 , int x1, int x2, int x3, int y1, int y2, int y3)
        {
            //Выбираем цвета зарисовки 

            //Определяем объект "g" класса Graphics
            g.Clear(Color.Black);//Зарисовка экрана черным фоном


            //Определим координаты исходного треугольника
            var point1 = new PointF(x1, y1); // 200 200
            var point2 = new PointF(x2, y2); // 500 200
            var point3 = new PointF(x3, y3); // 350 400

            //Зарисуем треугольник
            g.DrawLine(pen1, point1, point2);
            g.DrawLine(pen1, point2, point3);
            g.DrawLine(pen1, point3, point1);

            //Вызываем функцию Fractal для того, чтобы
            //нарисовать три кривых Коха на сторонах треугольника
            FractalKoha(point1, point2, point3, n, g, pen1, pen2);
            FractalKoha(point2, point3, point1, n, g, pen1, pen2);
            FractalKoha(point3, point1, point2, n, g, pen1, pen2);
        }

        public static int FractalKoha(PointF p1, PointF p2, PointF p3, int iter, Graphics g, Pen pen1, Pen pen2)
        {
            //n -количество итераций
            if (iter > 0)  //условие выхода из рекурсии
            {
                //средняя треть отрезка
                var p4 = new PointF((p2.X + 2 * p1.X) / 3, (p2.Y + 2 * p1.Y) / 3);
                var p5 = new PointF((2 * p2.X + p1.X) / 3, (p1.Y + 2 * p2.Y) / 3);
                //координаты вершины угла
                var ps = new PointF((p2.X + p1.X) / 2, (p2.Y + p1.Y) / 2);
                var pn = new PointF((4 * ps.X - p3.X) / 3, (4 * ps.Y - p3.Y) / 3);
                //рисуем его
                g.DrawLine(pen1, p4, pn);
                g.DrawLine(pen1, p5, pn);
                g.DrawLine(pen2, p4, p5);


                //рекурсивно вызываем функцию нужное число раз
                FractalKoha(p4, pn, p5, iter - 1, g, pen1, pen2);
                FractalKoha(pn, p5, p4, iter - 1, g, pen1, pen2);
                FractalKoha(p1, p4, new PointF((2 * p1.X + p3.X) / 3, (2 * p1.Y + p3.Y) / 3), iter - 1, g, pen1, pen2);
                FractalKoha(p5, p2, new PointF((2 * p2.X + p3.X) / 3, (2 * p2.Y + p3.Y) / 3), iter - 1, g, pen1, pen2);

            }
            return iter;
        }

        public static void DrawRectangle(Graphics gr, int level, RectangleF rect, Brush brush)
        {

            if (level == 0)
            {
                // Заполняем
                gr.FillRectangle(brush, rect);
            }
            else
            {
                //Делим прямоугольник на меньшие
                float wid = rect.Width / 3f;
                float x0 = rect.Left;
                float x1 = x0 + wid;
                float x2 = x0 + wid * 2f;

                float hgt = rect.Height / 3f;
                float y0 = rect.Top;
                float y1 = y0 + hgt;
                float y2 = y0 + hgt * 2f;

                // Рисуем меньшие прямоугольники
                DrawRectangle(gr, level - 1, new RectangleF(x0, y0, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x1, y0, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x2, y0, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x0, y1, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x2, y1, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x0, y2, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x1, y2, wid, hgt), brush);
                DrawRectangle(gr, level - 1, new RectangleF(x2, y2, wid, hgt), brush);
            }
        }


    }
}
