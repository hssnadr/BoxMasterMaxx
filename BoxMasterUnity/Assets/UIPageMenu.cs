using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageMenu : MonoBehaviour {
	[SerializeField]
	protected UIPage[] _pages;
	[SerializeField]
	protected UIPage _idleScreen;
	[SerializeField]
	protected UIPage _timeOutScreen;
	[SerializeField]
	protected UIMenuAnimator _UIMenuAnimator;

	protected int _pageIndex = 0;

	void OnEnable()
	{
		GameManager.onActivity += OnActivity;
		GameManager.onTimeOut += OnTimeOut;
		GameManager.onTimeOutScreen += OnTimeOutScreen;
	}

	void OnTimeOutScreen ()
	{
		GetCurrentPage ().Hide ();
		_timeOutScreen.Show ();
	}

	void OnTimeOut ()
	{
		GoToHome ();
		_UIMenuAnimator.SetState (false);
	}

	void OnActivity ()
	{
		_idleScreen.Hide ();
		_timeOutScreen.Hide ();
		GetCurrentPage ().Show ();
	}

	void Start() {
		foreach (var page in _pages) {
			page.Hide ();
		}
		_idleScreen.Show ();
		_timeOutScreen.Hide ();
	}

	public UIPage GetCurrentPage() {
		return _pages [_pageIndex];
	}

	public UIPage GetNextPage() {
		var tempPageIndex = _pageIndex + 1;
		return (tempPageIndex >= _pages.Length) ? null : _pages [tempPageIndex];
	}

	public UIPage GetPreviousPage() {
		var tempPageIndex = _pageIndex - 1;
		return (tempPageIndex < 0) ? null : _pages [tempPageIndex];
	}

	public void GoToHome()
	{
		GoTo (0);
		_idleScreen.Show ();
		_timeOutScreen.Hide ();
		GameManager.instance.Sleep ();
	}

	public void GoToFirstPage()
	{
		GoTo (0);
	}

	public void GoToLastPage()
	{
		GoTo (_pages.Length - 1);
	}
		
	public void GoTo(int index)
	{
		var previous = GetCurrentPage ();
		int previousIndex = _pageIndex;
		UIPage current = null;
		try {
			_pageIndex = index;
			current = GetCurrentPage();
			previous.Hide();
			current.Show();
		}
		catch {
			_pageIndex = index;
			previous.Show ();
		}
		_idleScreen.Hide ();
		_timeOutScreen.Hide ();
	}

	public void GoToIdleScreen() {
		GetCurrentPage ().Hide ();
		_idleScreen.Show ();
		_timeOutScreen.Hide ();
	}

	public void GoToTimeoutScreen() {
		GetCurrentPage ().Hide ();
		_idleScreen.Hide ();
		_timeOutScreen.Show ();
	}

	public void GoToNext() {
		var previous = GetCurrentPage ();
		_pageIndex = Mathf.Clamp(_pageIndex + 1, 0, _pages.Length - 1);
		var current = GetCurrentPage ();
		previous.Hide ();
		current.Show ();
		_idleScreen.Hide ();
		_timeOutScreen.Hide ();
	}

	public void GoToPrevious() {
		var previous = GetCurrentPage ();
		_pageIndex = Mathf.Clamp (_pageIndex - 1, 0, _pages.Length - 1);
		var current = GetCurrentPage ();
		previous.Hide ();
		current.Show ();
		_idleScreen.Hide ();
		_timeOutScreen.Hide ();
	}
}