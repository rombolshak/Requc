using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requc.Commands
{
    public enum ModelingMode
    {
        [Description("Случайный выбор")]
        Random,
        [Description("Есть отсчет (без Евы)")]
        NoEvaWithClick,
        [Description("Нет отсчета (без Евы)")]
        NoEvaNoClick,
        [Description("Неопределенный результат у Евы")]
        EvaUndefinedResult,
        [Description("Нет отсчета (с Евой)")]
        EvaDefinedResultNoClick,
        [Description("Ева обнаружена")]
        EvaDefinedResultDetected
    }
}
