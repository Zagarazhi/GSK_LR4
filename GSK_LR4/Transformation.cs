using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSK_LR4
{
    /*
     * Класс, содержащий информацию о преобразованиях
     */
    public class Transformation
    {
        private Pgn Pgn; //Преобразумая фигура
        private int TransformationType; //Номер преобразования
        private int IterationCount; //Число итераций
        private int Iteration = 0; //Номер текущей итерации
        private Point FirstPosition; //Начальная позиция
        private Point LastPosition; //Конечная позиция
        private float mX, mY; //Малые изменения координат
        private float dX, dY; //Малые увеличения
        private float mF; //Малые углы

        /*
         * Конструктор трансформаций для перемещения и поворота
         */
        public Transformation(Pgn Pgn, int TransformationType, int IterationCount, Point FirstPosition, Point LastPosition)
        {
            this.Pgn = Pgn;
            this.TransformationType = TransformationType;
            this.IterationCount = IterationCount;
            this.FirstPosition = FirstPosition;
            this.LastPosition = LastPosition;
            this.mX = ((float)(LastPosition.X - FirstPosition.X)) / IterationCount;
            this.mY = ((float)(LastPosition.Y - FirstPosition.Y)) / IterationCount;
            float F = (LastPosition.X - FirstPosition.X) == 0F ?
                        1.5708F :
                        (float)(Math.Atan(((float)(FirstPosition.Y - LastPosition.Y)) / (LastPosition.X - FirstPosition.X)));
            F = F < 0 ? 1.5708F - F : F;
            F = (LastPosition.Y > FirstPosition.Y) ? F : -F;
            this.mF = F / IterationCount;
        }

        /*
         * Конструктор трансформаций для масштабирования
         */
        public Transformation(Pgn Pgn, int TransformationType, int IterationCount, Point FirstPosition, Point LastPosition, float XScale, float YScale)
        {
            this.Pgn = Pgn;
            this.TransformationType = TransformationType;
            this.IterationCount = IterationCount;
            this.FirstPosition = FirstPosition;
            this.LastPosition = LastPosition;
            this.dX = (float)Math.Pow(XScale, 1F / IterationCount) - 1;
            this.dY = (float)Math.Pow(YScale, 1F / IterationCount) - 1;
        }

        /*
         * Конструктор трансформаций для отражений
         */
        public Transformation(Pgn Pgn, int TransformationType, int IterationCount, Point FirstPosition)
        {
            this.Pgn = Pgn;
            this.TransformationType = TransformationType;
            this.IterationCount = IterationCount;
            this.FirstPosition = FirstPosition;
        }

        /*
         * Выполнение трансформаций
         */
        public void DoTransformation()
        {
            Iteration++;
            switch (TransformationType)
            {
                case 0:
                    Pgn.Move(mX, mY);
                    break;
                case 1:
                    Pgn.Rotate(mF, FirstPosition.X, FirstPosition.Y);
                    break;
                case 2:
                    Pgn.Resize(dX, dY, FirstPosition.X, FirstPosition.Y);
                    break;
                case 3:
                    Pgn.MirrorX(FirstPosition.Y);
                    Iteration = IterationCount;
                    break;
                case 4:
                    Pgn.MirrorY(FirstPosition.X);
                    Iteration = IterationCount;
                    break;
                case 5:
                    Pgn.MirrorXY(FirstPosition.X, FirstPosition.Y);
                    Iteration = IterationCount;
                    break;
            }
        }

        /*
         * Проверка, завершена для трансформация
         */
        public bool ShouldRemove()
        {
            return Iteration >= IterationCount;
        }
    }
}
