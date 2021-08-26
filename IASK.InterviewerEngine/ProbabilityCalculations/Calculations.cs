using System;

namespace IASK.InterviewerEngine
{
    internal class Calculations
    {
        /// <summary>
        /// Метод для получения суммы вероятностей принимает один параметр.
        /// </summary>
        /// <param name="veroiat">(double) Принимает значение вероятности.</param>
        /// <returns>(double) Сумма вероятностей.</returns>
        public double SummVeroiat(double veroiat)
        {
            return veroiat;
        }

        /// <summary>
        /// Метод для получения суммы вероятностей принимает два параметра.
        /// </summary>
        /// <param name="ver1">(double) Принимает первое значения вероятности.</param>
        /// <param name="ver2">(double) Принимает второе значения вероятности.</param>
        /// <returns>(double) Сумма вероятностей.</returns>
        public double SummVeroiat(double ver1, double ver2)
        {
            return (ver1 + ver2 - (ver1 * ver2));
        }

        /// <summary>
        /// Метод для получения суммы вероятностей принимает множество параметров.
        /// </summary>
        /// <param name="veroiats">(double) Принимает множество значений вероятностей.</param>
        /// <returns>(double) Сумма вероятностей.</returns>
        public double SummVeroiat(params double[] veroiats)
        {
            if (veroiats.Length == 1)
                return veroiats[0];

            if (veroiats.Length == 2)
                return SummVeroiat(veroiats[0], veroiats[1]);

            if (veroiats.Length > 2)
            {
                double[] temp = new double[veroiats.Length - 2];
                for (int i = 2; i < veroiats.Length; ++i)
                    temp[i - 2] = veroiats[i];

                return SummVeroiat(
                    SummVeroiat(veroiats[0], veroiats[1]),
                    SummVeroiat(temp));
            }

            return .0;
        }
    }
    internal class CheckerValue
    {
        /// <summary>
        /// Метод проверки вероятности.
        /// </summary>
        /// <param name="value">Проверяемая вероятность.</param>
        /// <returns>true - корректное значение. false - некорректное значение.</returns>
        public bool CheckValue_a(double? value)
        {
            if (value == null
                //|| (Math.Abs((double)value - 0) <= double.Epsilon)
                || ((double)value < 0 - double.Epsilon)
                || ((double)value > 1 + double.Epsilon))
                return false;

            return true;
        }

        /// <summary>
        /// Метод проверки вероятности Заболеваемости.
        /// </summary>
        /// <param name="value">Проверяемая вероятность.</param>
        /// <returns>true - корректное значение. false - некорректное значение.</returns>
        public bool CheckPrevalenceValue_a(double? value)
        {
            if (value == null
                //|| (Math.Abs((double)value - 0) <= double.Epsilon)

                || ((double)value <= 0 + double.Epsilon && Math.Abs(-1.0 - (double)value) > double.Epsilon)
                // || ((double)value > 1 + double.Epsilon)
                )
                return false;

            return true;
        }

        /// <summary>
        /// Метод проверки степени доказательности.
        /// </summary>
        /// <param name="value">Проверяемая степень доказательности.</param>
        /// <returns>true - корректное значение. false - некорректное значение.</returns>
        public bool CheckValue_d(double? value)
        {
            if (value == null) return true;

            if (((double)value < 0 - double.Epsilon)
                || ((double)value > 1 + double.Epsilon))
                return false;

            return true;
        }

        /// <summary>
        /// Метод возвращения значения вероятности, прошедшей проверку.
        /// </summary>
        /// <param name="value">Значения вероятности.</param>
        /// <returns>Корректное значение вероятности.</returns>
        public double GetCheckedValue_a(double? value)
        {
            return (double)value;
        }

        /// <summary>
        /// Метод возвращения значения степени доказательности, прошедшей проверку.
        /// </summary>
        /// <param name="value">Значения степени доказательности.</param>
        /// <returns>Корректное значение степени доказательности.</returns>
        public double GetCheckedValue_d(double? value)
        {
            if (value == null)
                return .0;

            return (double)value;
        }

        /// <summary>
        /// Метод возвращения значения вероятности Заболеваемости, прошедшей проверку.
        /// </summary>
        /// <param name="value">Значения вероятности.</param>
        /// <returns>Корректное значение вероятности.</returns>
        public double GetCheckedPrevalenceValue_a(double? value)
        {
            if (/*value <= 1 + double.Epsilon &&*/
                value > 0 + double.Epsilon)
                return (double)value;

            return -1.0;
        }

        /// <summary>
        /// Метод возвращения нового значения расчётной вероятности.
        /// </summary>
        /// <param name="value">Новое значение вероятности.</param>
        /// <returns>Корректное новое значение вероятности.</returns>
        public double GetNewCalcValue_a(double value)
        {
            if (value < 0 - double.Epsilon) return 0;
            if (value > 1 + double.Epsilon) return 1;
            return value;
        }

        /// <summary>
        /// Метод возвращения нового значения расчётной степени доказательности.
        /// </summary>
        /// <param name="value">Новое значение степени доказательности.</param>
        /// <returns>Корректное новое значение степени доказательности.</returns>
        public double GetNewCalcValue_d(double value)
        {
            if (value < 0 - double.Epsilon) return 0;
            if (value > 1 + double.Epsilon) return 1;
            return value;
        }
    }
    internal class DataCalcValue
    {
        public double CalcValue_A { get; set; }
        public double CalcValue_D { get; set; }

        public DataCalcValue(double v_a, double v_d)
        {
            CalcValue_A = v_a;
            CalcValue_D = v_d;
        }

        internal DataCalcValue Copy()
        {
            return (DataCalcValue)this.MemberwiseClone();
        }
    }
}
