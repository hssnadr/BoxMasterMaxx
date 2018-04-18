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
	[SerializeField]
	protected UIPage _pagePrefabType1;
	[SerializeField]
	protected UIPage _pagePrefabType2;

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
		GameManager.instance.Home ();
		_UIMenuAnimator.SetState (false);
	}

	void OnActivity ()
	{
		_timeOutScreen.Hide ();
	}

	void Start() {
		_idleScreen.Show ();
		_pages = new UIPage[2];
		_pages [0] = GameObject.Instantiate (_pagePrefabType1, this.transform);
		_pages [1] = GameObject.Instantiate (_pagePrefabType2, this.transform);
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
		TextManager.instance.SetDefaultLang ();
		GameManager.instance.Home ();
	}

	public void GoToFirstPage()
	{
		GoTo (0);
		GameManager.instance.StartPages ();
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
		_UIMenuAnimator.Hide ();
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