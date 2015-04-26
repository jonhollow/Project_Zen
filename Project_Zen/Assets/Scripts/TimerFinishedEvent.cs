using UnityEngine;
using System.Collections;

/// <summary>
/// Delegate for handling the event
/// </summary>
public delegate void TimerFinishedEventHandler();

/// <summary>
/// An event for a timer finishing
/// </summary>
public class TimerFinishedEvent
{
	event TimerFinishedEventHandler eventHandlers;

    #region Public Methods

    /// <summary> 
	/// Register the given event handler 
	/// </summary> 
	/// <param name="handler">the event handler</param> 
	public void Register(TimerFinishedEventHandler handler)
	{
		eventHandlers += handler;
	}
	
	/// <summary> 
	/// Fire the event for all event handlers 
	/// </summary>
	public void FireEvent()
	{
		if (eventHandlers != null)
		{
			eventHandlers();
		}
    }

    #endregion
}
