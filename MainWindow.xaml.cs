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
	public class FinanceObject
	{
		public string name;
		public Interval interval;
		public float amount;
		public bool gain;
		public DateTime timeAdded;
		public DateTime timeOfObject;
		public FinanceBalance financeBalance;

		public override string ToString()
		{
			// Convert gain/loss to string.
			string gainLoss = gain ? "Gain" : "Loss";

			StringBuilder result = new StringBuilder();
			result.AppendFormat("{0}: {1:G}, {2:F}, {3}, {4}, {5}, {6}",
				name, interval, amount, gainLoss, timeAdded.ToString(), timeOfObject.ToString(), financeBalance.name);
			return result.ToString();
		}
	}

	public enum BalanceType
	{
		SavingsAccount,
		CheckingAccount,
		Stock,
		CreditCard,
		Loan,
		Bond
	}
	public class FinanceBalance
	{
		public string name;
		public BalanceType type;
		public float amount;
		public float initialAmount;

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			result.AppendFormat("{0}, {1:G}, {2:F}", name, type, amount);
			return result.ToString();
		}
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<FinanceObject> financeObjects;
		public List<FinanceBalance> financeBalances;

		public MainWindow()
		{
			InitializeComponent();

			txtDate.SelectedDate = DateTime.Today;
			calculationDate.SelectedDate = DateTime.Today;

			financeObjects = new List<FinanceObject>();
			financeBalances = new List<FinanceBalance>();

			checkGain.IsChecked = true;
			checkLoss.IsChecked = false;

			lstFinObjs.ItemsSource = financeObjects;
			lstBalances.ItemsSource = financeBalances;

			comboBoxBalanceSelection.ItemsSource = financeBalances;
		}

		private void ButtonAddObject_Click(object sender, RoutedEventArgs e)
		{
			string[] temp = new string[2];
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
				temp[1] = txtAmount.Text;
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

			if(float.TryParse(temp[1], out finObj.amount))
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

			finObj.financeBalance = (FinanceBalance)comboBoxBalanceSelection.SelectedItem;

			financeObjects.Add(finObj);
			financeObjects.Sort((x, y) => x.timeOfObject.CompareTo(y.timeOfObject));

			lstFinObjs.Items.Refresh();

			txtName.Clear();
			txtAmount.Clear();
			txtDate.SelectedDate = DateTime.Today;
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

			foreach(FinanceBalance fb in financeBalances)
			{
				fb.amount = fb.initialAmount;
			}

			foreach(FinanceObject fo in financeObjects)
			{
				if(0 >= fo.timeOfObject.CompareTo(calculationDate.SelectedDate.Value))
				{
					FinanceBalance fb = financeBalances.First(item => item.name == fo.financeBalance.name);
					int index = financeBalances.IndexOf(fb);

					float sign = fo.gain ? 1.0f : -1.0f;
					financeBalances[index].amount += fo.amount * sign;
				}
				else
				{
					break;
				}
			}

			lstBalances.Items.Refresh();
			comboBoxBalanceSelection.Items.Refresh();

			foreach (FinanceBalance fb in financeBalances)
			{
				value += fb.amount;
			}
			balanceAmount.Content = "Global Balance: " + value;
		}

		private void ButtonAddBalance_Click(object sender, RoutedEventArgs e)
		{
			string[] temp = new string[2];
			if (!string.IsNullOrWhiteSpace(txtBalanceName.Text))
			{
				temp[0] = txtBalanceName.Text;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("You did not enter a valid name for the balance.");
				txtBalanceName.Clear();
				return;
			}
			if (!string.IsNullOrWhiteSpace(txtBalanceAmount.Text))
			{
				temp[1] = txtBalanceAmount.Text;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("You did not enter a valid amount for the balance.");
				txtBalanceAmount.Clear();
				return;
			}

			FinanceBalance finBal = new FinanceBalance();
			finBal.name = temp[0];
			finBal.type = (BalanceType)comboBoxBalanceType.SelectedItem;

			if (float.TryParse(temp[1], out finBal.initialAmount))
			{
				finBal.amount = finBal.initialAmount;
			}
			else
			{
				// Failure case. Tell the user what they did wrong and clear amount field.
				MessageBox.Show("The amount you entered is not a decimal number within range.");
				txtBalanceAmount.Clear();
				return;
			}

			financeBalances.Add(finBal);
			financeBalances.Sort((x, y) => x.name.CompareTo(y.name));

			lstBalances.Items.Refresh();
			comboBoxBalanceSelection.Items.Refresh();

			txtBalanceName.Clear();
			txtBalanceAmount.Clear();
		}
	}
}
