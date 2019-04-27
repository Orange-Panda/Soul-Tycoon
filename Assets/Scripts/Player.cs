public static class Player
{
	public static int currency = 0;

	/// <summary>
	/// Adds currency to the players balance. Will never fail.
	/// </summary>
	public static void Deposit(uint value)
	{
		currency += (int)value;
	}

	/// <summary>
	/// Attempts to remove currency from the player account. If they have insufficient funds will deny the withdrawal and return false.
	/// </summary>
	public static bool Withdraw(uint value)
	{
		bool insufficient = currency - value < 0;
		currency = insufficient ? currency : currency - (int)value;
		return !insufficient;
	}

	/// <summary>
	/// Forcibly withdraws currency.
	/// </summary>
	public static void ForceWithdraw(uint value)
	{
		currency -= (int)value;
	}
}
