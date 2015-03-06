using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioLib
{
    public static class Rabbitz
    {
        public static Decimal MakeOut(int months, int producing)
        {
            Decimal news = 1, olds = 0;
            for (Decimal i = 1; i < months; i++)
            {
                Decimal tmp = news;
                news = olds * producing;
                olds += tmp;
            }
            return news + olds;
        }
        public static Decimal MakeOutAndDie(int months, int suicideTime, int producing = 1)
        {
            Decimal[] bornMonthsAgo = new Decimal[suicideTime];
            bornMonthsAgo[0] = 1;

            Decimal news = 1, olds = 0;
            for (Decimal i = 1; i < months; i++)
            {

                Decimal tmp = news;
                news = olds * producing;

                olds -= bornMonthsAgo[suicideTime - 1];
                for (int j = suicideTime - 1; j > 0; j--)
                {
                    bornMonthsAgo[j] = bornMonthsAgo[j - 1];
                }
                bornMonthsAgo[0] = news;

                olds += tmp;
            }
            return news + olds;
        }
    }
}
