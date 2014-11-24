namespace OMControlLibrary
{
	/// <summary>
	/// Hit test result of <see cref="OMETabStrip"/>
	/// </summary>
	public enum HitTestResult
	{
		CloseButton,
		MenuGlyph,
		TabItem,
		None
	}

	/// <summary>
	/// Theme Type
	/// </summary>
	public enum ThemeTypes
	{
		WindowsXP,
		Office2000,
		Office2003
	}

	/// <summary>
	/// Indicates a change into TabStrip collection
	/// </summary>
	public enum OMETabStripItemChangeTypes
	{
		Added,
		Removed,
		Changed,
		SelectionChanged
	}
}
