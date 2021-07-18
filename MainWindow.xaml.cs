using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace finance_app
{
	public enum Interval
	{
		Once,
		Daily,
		Weekly,
		Monthly,
		Quarterly,
		Yearly
	}
	public struct FinanceObject
	{
		public string name;
		public Interval interval;
		public float amount;
		public bool gain;
		public DateTime timeAdded;
		public DateTime timeOfObject;

		public override string ToString()
		{
			// Convert gain/loss to string.
			string gainLoss = gain ? "Gain" : "Loss";

			StringBuilder result = new StringBuilder();
			result.AppendFormat("{0}: {1:G}, {2:F}, {3}, {4}, {5}",
				name, interval, amount, gainLoss, timeAdded.ToString(), timeOfObject.ToString());
			return result.ToString();
		}
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<FinanceObject> financeObjects;

		public MainWindow()
		{
			InitializeComponent();

			txtDate.SelectedDate = DateTime.Today;
			calculationDate.SelectedDate = DateTime.Today;

			financeObjects = new List<FinanceObject>();

			checkGain.IsChecked = true;
			checkLoss.IsChecked = false;

			lstNames.ItemsSource = financeObjects;
		}

		private void ButtonAddObject_Click(object sender, RoutedEventArgs e)
		{
			string[] temp = new string[4];
			if (!string.IsNullOrWhiteSpace(txtName.Text))
			{
				temp[0] = txtName.Text;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("You did not enter a valid name for the finance object.");
				txtName.Clear();
				return;
			}
			if (!string.IsNullOrWhiteSpace(txtAmount.Text))
			{ 
				temp[2] = txtAmount.Text;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("You did not enter a valid amount for the finance object.");
				txtAmount.Clear();
				return;
			}

			FinanceObject finObj = new FinanceObject();
			finObj.name = temp[0];

			finObj.interval = (Interval)comboBoxInterval.SelectedItem;

			if(float.TryParse(temp[2], out finObj.amount))
			{
				// Success. Do Nothing
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("The amount you entered is not a decimal number within range.");
				txtAmount.Clear();
				return;
			}

			if (checkGain.IsChecked.HasValue)
			{
				finObj.gain = checkGain.IsChecked.Value;
			}

			finObj.timeAdded = DateTime.Now;
			finObj.timeOfObject = (DateTime)txtDate.SelectedDate;

			financeObjects.Add(finObj);
			financeObjects.Sort((x, y) => x.timeOfObject.CompareTo(y.timeOfObject));

			lstNames.Items.Refresh();

			txtName.Clear();
			txtAmount.Clear();
			txtDate.SelectedDate = DateTime.Now;
		}

		private void HandleGainCheck(object sender, RoutedEventArgs e)
		{
			checkLoss.IsChecked = false;
		}

		private void HandleGainUnchecked(object sender, RoutedEventArgs e)
		{
			checkLoss.IsChecked = true;
		}

		private void HandleLossCheck(object sender, RoutedEventArgs e)
		{
			checkGain.IsChecked = false;
		}

		private void HandleLossUnchecked(object sender, RoutedEventArgs e)
		{
			checkGain.IsChecked = true;
		}

		private void ButtonCalculateBalance_Click(object sender, RoutedEventArgs e)
		{
			float value = 0.0f;
			foreach(FinanceObject fo in financeObjects)
			{
				if(0 >= fo.timeOfObject.CompareTo(calculationDate.SelectedDate.Value))
				{
					float sign = fo.gain ? 1.0f : -1.0f;
					value += fo.amount * sign;
				}
				else
				{
					break;
				}
			}

			balanceAmount.Content = "Balance: " + value;
		}
	}
}
