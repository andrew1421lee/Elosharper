using System;

namespace EloSharper.database
{
	public class EloManager
	{
		private const double KVALUE = 32.0;
		public static double[] CalculateElo(double p1_elo, double p2_elo, int winner)
		{
			double R1 = p1_elo / 400.0;
			double R2 = p2_elo / 400.0;

			R1 = Math.Pow(R1, 10.0);
			R2 = Math.Pow(R2, 10.0);

			double E1 = R1 / (R1 + R2);
			double E2 = R2 / (R2 + R1);

			double S1 = 0.0;
			double S2 = 0.0;
			switch (winner)
			{
				case 0:
					// P1 wins
					S1 = 1.0;
					S2 = 0.0;
					break;
				case 1:
					S1 = 0.0;
					S2 = 1.0;
					break;
				case 2:
					S1 = 0.5;
					S2 = 0.5;
					break;
			}

			double Result1 = p1_elo + KVALUE * (S1 - E1);
			double Result2 = p2_elo + KVALUE * (S2 - E2);

			return new double[] { Result1, Result2 };
		}
	}
}
