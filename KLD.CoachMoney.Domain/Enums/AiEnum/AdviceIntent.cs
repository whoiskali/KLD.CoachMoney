using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Enums.AiEnum
{
    public enum AdviceIntent
    {
        Snapshot,          // Explain current situation
        DebtPriority,      // Which debts to focus on
        SavingsAdvice,     // Emergency fund, savings balance
        RiskAssessment,    // Financial risk signals
        Coaching           // Motivation, behavioral advice
    }
}
