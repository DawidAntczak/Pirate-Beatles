public abstract class SimpleMenu<T> : Menu<T> where T : SimpleMenu<T>
{
    /// a base menu class that implements parameterless Show and Hide methods

    public static void Show()
	{
		Open();
	}

	public static void Hide()
	{
		Close();
	}
}
