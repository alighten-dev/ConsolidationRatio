#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class ConsolidationRatio : Indicator
	{
		private int period;
		private double threshold;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Measures the ratio of bar overlap to determine consolidation level - by Alighten";
				Name										= "ConsolidationRatio";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				
				period 										= 14;
        		threshold 									= 6; 
				AddPlot(Brushes.Aquamarine, "Ratio");

                AddLine(Brushes.Red, threshold, "Threshold");
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
        {
            // Ensure we have enough bars for our lookback period.
            if (CurrentBar < period)
                return;

            double highestHigh = double.MinValue;
            double lowestLow   = double.MaxValue;
            double sumRange    = 0.0;

            // Loop through the defined period
            for (int i = 0; i < period; i++)
            {
                // Update highest high and lowest low for overall range
                if (High[i] > highestHigh)
                    highestHigh = High[i];
                if (Low[i] < lowestLow)
                    lowestLow = Low[i];

                // Accumulate each candle's range
                sumRange += (High[i] - Low[i]);
            }

            double overallRange = highestHigh - lowestLow;
            double avgRange     = sumRange / period;
            double ratio        = overallRange / avgRange;

            // Plot the ratio value
            Value[0] = ratio;
        }
		
		#region Properties
        [NinjaScriptProperty]
        [Range(1, int.MaxValue)]
        [Display(Name = "Period", Order = 1, GroupName = "Parameters")]
        public int Period
        {
            get { return period; }
            set { period = value; }
        }
        #endregion
	}
	
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private ConsolidationRatio[] cacheConsolidationRatio;
		public ConsolidationRatio ConsolidationRatio(int period)
		{
			return ConsolidationRatio(Input, period);
		}

		public ConsolidationRatio ConsolidationRatio(ISeries<double> input, int period)
		{
			if (cacheConsolidationRatio != null)
				for (int idx = 0; idx < cacheConsolidationRatio.Length; idx++)
					if (cacheConsolidationRatio[idx] != null && cacheConsolidationRatio[idx].Period == period && cacheConsolidationRatio[idx].EqualsInput(input))
						return cacheConsolidationRatio[idx];
			return CacheIndicator<ConsolidationRatio>(new ConsolidationRatio(){ Period = period }, input, ref cacheConsolidationRatio);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.ConsolidationRatio ConsolidationRatio(int period)
		{
			return indicator.ConsolidationRatio(Input, period);
		}

		public Indicators.ConsolidationRatio ConsolidationRatio(ISeries<double> input , int period)
		{
			return indicator.ConsolidationRatio(input, period);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.ConsolidationRatio ConsolidationRatio(int period)
		{
			return indicator.ConsolidationRatio(Input, period);
		}

		public Indicators.ConsolidationRatio ConsolidationRatio(ISeries<double> input , int period)
		{
			return indicator.ConsolidationRatio(input, period);
		}
	}
}

#endregion
