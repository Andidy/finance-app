# finance-app
A Finance / Budgeting Application to refresh my C# knowledge and Learn WPF.

![image](https://user-images.githubusercontent.com/37605997/126057229-7a19fa5d-5385-4f3b-b603-b81e3d02a4ce.png)

The app bases its feature set on two main ideas "Finance Objects" and "Finance Balances". A finance balance represents things like savings accounts, checking accounts, credit card balances, etc. A finance object is an event that affects the balances that have been created, representing things like gifts, paychecks, purchases, bills, etc.

Current Features:
- Create "Finance Objects" which store a name, interval, amount, whether they are a gain or loss, time of creation, time of the object occuring and which balance they apply to.
- Create "Finance Balances" which have a name, type and amount and initial amount (which exists before the history of finance objects is applied).
- Display list of created Finance Objects and Finance Balances
- Calculate Balances, this is a button that when pressed will walk through the balances and objects and apply the object amounts to the balances. This takes the selected date into account so that an object that is to occur after the specified date is not applied to the balance.

Here is an example of the app with some dumby data:
The initial configuration where the selected date is before the time of any finance objects:
![image](https://user-images.githubusercontent.com/37605997/126057444-8b2ead1f-0cca-4429-a650-b26747fc6631.png)

Now we move the date to a point mostly through the month of July so that the first two finance objects are applied:
![image](https://user-images.githubusercontent.com/37605997/126057633-2cacfcc9-3fba-450d-8c78-d37f2e9417d8.png)

And finally we move the date to August 1st so that all 4 finance objects are applied:
![image](https://user-images.githubusercontent.com/37605997/126057654-67741ecd-672d-4ce2-b58e-3afd6bcb707f.png)
