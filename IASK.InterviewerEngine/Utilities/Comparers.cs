using System.Collections.Generic;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    internal class ComparerByCalcValueSum_AToStepADecreases : IComparer<ETElement>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(ETElement o1, ETElement o2)
        {
            if (o1.CalcValueSum_AToStepA > o2.CalcValueSum_AToStepA + double.Epsilon) return -1;
            if (o1.CalcValueSum_AToStepA < o2.CalcValueSum_AToStepA - double.Epsilon) return 1;

            return 0;
        }
    }

    /// <summary>
    /// Класс для сравнения расчётных значений вероятностей.
    /// </summary>
    internal class ComparerByCalc_Value_A : IComparer<ETItem>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(ETItem o1, ETItem o2)
        {
            if (o1.CalcValue_A > o2.CalcValue_A + double.Epsilon) return 1;
            if (o1.CalcValue_A < o2.CalcValue_A - double.Epsilon) return -1;

            if (o1.CalcValue_D > o2.CalcValue_D + double.Epsilon) return 1;
            if (o1.CalcValue_D < o2.CalcValue_D - double.Epsilon) return -1;

            return 0;
        }
    }

    /// <summary>
    /// Класс для сравнения расчётных значений степеней доказательности.
    /// </summary>
    internal class ComparerByCalc_Value_D : IComparer<ETItem>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(ETItem o1, ETItem o2)
        {
            if (o1.CalcValue_D > o2.CalcValue_D + double.Epsilon) return 1;
            if (o1.CalcValue_D < o2.CalcValue_D - double.Epsilon) return -1;

            if (o1.CalcValue_A > o2.CalcValue_A + double.Epsilon) return 1;
            if (o1.CalcValue_A < o2.CalcValue_A - double.Epsilon) return -1;

            return 0;
        }
    }

    /// <summary>
    /// Класс для сравнения не расчётных значений вероятностей.
    /// </summary>
    internal class ComparerByValue_A : IComparer<ETItem>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(ETItem o1, ETItem o2)
        {
            if (o1.Value_A != null && o2.Value_A == null) return 1;
            if (o1.Value_A == null && o2.Value_A != null) return -1;
            if (o1.Value_A != null && o2.Value_A != null)
            {
                if (o1.Value_A > o2.Value_A + double.Epsilon) return 1;
                if (o1.Value_A < o2.Value_A - double.Epsilon) return -1;
            }

            if (o1.Value_D != null && o2.Value_D == null) return 1;
            if (o1.Value_D == null && o2.Value_D != null) return -1;
            if (o1.Value_D != null && o2.Value_D != null)
            {
                if (o1.Value_D > o2.Value_D + double.Epsilon) return 1;
                if (o1.Value_D < o2.Value_D - double.Epsilon) return -1;
            }

            return 0;
        }
    }

    /// <summary>
    /// Класс для сравнения не расчётных значений степеней доказательности.
    /// </summary>
    internal class ComparerByValue_D : IComparer<ETItem>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(ETItem o1, ETItem o2)
        {
            if (o1.Value_D != null && o2.Value_D == null) return 1;
            if (o1.Value_D == null && o2.Value_D != null) return -1;
            if (o1.Value_D != null && o2.Value_D != null)
            {
                if (o1.Value_D > o2.Value_D + double.Epsilon) return 1;
                if (o1.Value_D < o2.Value_D - double.Epsilon) return -1;
            }

            if (o1.Value_A != null && o2.Value_A == null) return 1;
            if (o1.Value_A == null && o2.Value_A != null) return -1;
            if (o1.Value_A != null && o2.Value_A != null)
            {
                if (o1.Value_A > o2.Value_A + double.Epsilon) return 1;
                if (o1.Value_A < o2.Value_A - double.Epsilon) return -1;
            }

            return 0;
        }
    }

    /// <summary>
    ///  Класс для сравнения idb.
    /// </summary>
    internal class ComparerByIdb : IComparer<ETItem>
    {
        /// <summary>
        ///  Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(ETItem o1, ETItem o2)
        {
            if (o1.idb > o2.idb) return 1;
            if (o1.idb < o2.idb) return -1;
            return 0;
        }
    }

    internal class ComparerPlexByDeepUpSortDecreases : IComparer<PlexNewBigId>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(PlexNewBigId o1, PlexNewBigId o2)
        {
            if (o1.deep > o2.deep) return 1;
            if (o1.deep < o2.deep) return -1;

            if (o1.sort != null && o2.sort == null) return -1;
            if (o1.sort == null && o2.sort != null) return 1;
            if (o1.sort != null && o2.sort != null)
            {
                if (o1.sort > o2.sort) return -1;
                if (o1.sort < o2.sort) return 1;
            }

            return 0;
        }
    }
    internal class ComparerPlexByDeepUpSortIncreases : IComparer<PlexNewBigId>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(PlexNewBigId o1, PlexNewBigId o2)
        {
            if (o1.deep > o2.deep) return 1;
            if (o1.deep < o2.deep) return -1;

            if (o1.sort != null && o2.sort == null) return 1;
            if (o1.sort == null && o2.sort != null) return -1;
            if (o1.sort != null && o2.sort != null)
            {
                if (o1.sort > o2.sort) return 1;
                if (o1.sort < o2.sort) return -1;
            }

            return 0;
        }
    }

    internal class ComparerPlexByDeepUpSortUpValueAIncreases : IComparer<PlexNewBigId>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(PlexNewBigId o1, PlexNewBigId o2)
        {
            if (o1.deep > o2.deep) return 1;
            if (o1.deep < o2.deep) return -1;

            if (o1.value_a != null && o2.value_a == null) return -1;
            if (o1.value_a == null && o2.value_a != null) return 1;
            if (o1.value_a != null && o2.value_a != null)
            {
                if (o1.value_a > o2.value_a + double.Epsilon) return -1;
                if (o1.value_a < o2.value_a - double.Epsilon) return 1;
            }

            if (o1.sort != null && o2.sort == null) return 1;
            if (o1.sort == null && o2.sort != null) return -1;
            if (o1.sort != null && o2.sort != null)
            {
                if (o1.sort > o2.sort) return 1;
                if (o1.sort < o2.sort) return -1;
            }

            return 0;
        }
    }

    internal class ComparerPlexByDeepUpSortUpValueADecreases : IComparer<PlexNewBigId>
    {
        /// <summary>
        /// Метод сравнения двух объектов.
        /// </summary>
        /// <param name="o1">Первый объект.</param>
        /// <param name="o2">Второй объект.</param>
        /// <returns>(1) - перый больше второго, (-1) - первый меньше второго, (0) - одинаковы значения.</returns>
        public int Compare(PlexNewBigId o1, PlexNewBigId o2)
        {
            if (o1.deep > o2.deep) return 1;
            if (o1.deep < o2.deep) return -1;

            if (o1.value_a != null && o2.value_a == null) return 1;
            if (o1.value_a == null && o2.value_a != null) return -1;
            if (o1.value_a != null && o2.value_a != null)
            {
                if (o1.value_a > o2.value_a + double.Epsilon) return 1;
                if (o1.value_a < o2.value_a - double.Epsilon) return -1;
            }

            if (o1.sort != null && o2.sort == null) return 1;
            if (o1.sort == null && o2.sort != null) return -1;
            if (o1.sort != null && o2.sort != null)
            {
                if (o1.sort > o2.sort) return 1;
                if (o1.sort < o2.sort) return -1;
            }

            return 0;
        }
    }

}
