using System.Collections.Generic;

namespace IASK.InterviewerEngine.Delegates
{
    /// <summary>
    /// Шаблон попытки запроса вероятности нозологии.
    /// </summary>
    /// <param name="probabilityType">Тип расчитываемой вероятности</param>
    /// <param name="nosologyId">ulong-id (NewBigId) нозологии в UMKB</param>
    /// <param name="result">Результат запроса</param>
    /// <returns>Успех запроса</returns>
    public delegate bool TryGetProbability(InterviewerState.ProbabilityType probabilityType, ulong nosologyId, out double result);

    /// <summary>
    /// Шаблон попытки запроса перечня и названий нозологий, расчет вероятностей которых доступен.
    /// </summary>
    /// <param name="result">Результат запроса. 
    /// key - ulong-id (NewBigId) нозологии в UMKB, 
    /// value - текстовое название нозологии для вывода результатов.</param>
    /// <returns>Успех запроса</returns>
    public delegate bool TryGetNosologies(out Dictionary<ulong, string> result);
}
