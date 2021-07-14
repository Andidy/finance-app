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
	public struct FinanceObject
	{
		public string name;
		public enum Interval
		{
			Once,
			Daily,
			Weekly,
			Monthly,
			Quarterly,
			Yearly
		}
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
			if (!string.IsNullOrWhiteSpace(txtInterval.Text))
			{
				temp[1] = txtInterval.Text;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("You did not enter a valid interval for the finance object.");
				txtInterval.Clear();
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
			if (!string.IsNullOrWhiteSpace(txtGainLoss.Text))
			{
				temp[3] = txtGainLoss.Text;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("You did not enter a valid gain / loss" +
						" for the finance object.\nYou should type: " +
						" \"Gain\", \"Loss\", \"gain\" or \"loss.\"");
				txtGainLoss.Clear();
				return;
			}

			FinanceObject finObj = new FinanceObject();
			finObj.name = temp[0];
			switch (temp[1])
			{
				case "Once":
					finObj.interval = FinanceObject.Interval.Once; break;
				case "Daily":
					finObj.interval = FinanceObject.Interval.Daily; break;
				case "Weekly":
					finObj.interval = FinanceObject.Interval.Weekly; break;
				case "Monthly":
					finObj.interval = FinanceObject.Interval.Monthly; break;
				case "Quarterly":
					finObj.interval = FinanceObject.Interval.Quarterly; break;
				case "Yearly":
					finObj.interval = FinanceObject.Interval.Yearly; break;
				default:
					// Failure case. Tell the user what they did wrong and clear amount field.
					MessageBox.Show("You did not enter a valid interval" +
							" for the finance object.\nYou should type: " +
							" \"Once\", \"Daily\", \"Weekly\", \"Monthly\"," +
							" \"Quarterly\" or \"Yearly.\"");
					txtInterval.Clear();
					return;
			}

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

			temp[3] = temp[3].Trim().ToLower();
			switch (temp[3])
			{
				case "gain":
					finObj.gain = true; break;
				case "loss":
					finObj.gain = false; break;
				default:
					// Failure case. Tell the user what they did wrong and clear field.
					MessageBox.Show("You did not enter a valid gain / loss" +
						" for the finance object.\nYou should type: " +
						" \"Gain\", \"Loss\", \"gain\" or \"loss.\"");
					txtGainLoss.Clear();
					return;
			}

			finObj.timeAdded = DateTime.Now;
			finObj.timeOfObject = DateTime.Now;

			financeObjects.Add(finObj);
			
			lstNames.Items.Add($"{finObj}");
			if ((bool)clearName.IsChecked)
			{
				txtName.Clear();
			}
			if ((bool)clearInterval.IsChecked)
			{
				txtInterval.Clear();
			}
			if ((bool)clearAmount.IsChecked)
			{
				txtAmount.Clear();
			}
			if ((bool)clearGainLoss.IsChecked)
			{
				txtGainLoss.Clear();
			}
		}
	}
}
